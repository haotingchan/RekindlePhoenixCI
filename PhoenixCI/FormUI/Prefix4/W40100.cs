using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao;
using DevExpress.XtraEditors.Controls;
using PhoenixCI.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml;

/// <summary>
/// Test Data 2B 20190102 / 1B 20190130 / 1E 20190212 / 0B 20190212
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
   public partial class W40100 : FormParent {

      #region Get UI Value
      /// <summary>
      /// yyyyMMdd
      /// </summary>
      public string TxtDate {
         get {
            return txtDate.DateTimeValue.ToString("yyyyMMdd");
         }
      }

      /// <summary>
      /// 一般/股票, 長假調整, 長假回調, 處置股票調整
      /// 0B / 1B / 1E / 2B
      /// </summary>
      public string AdjType {
         get {
            return ddlAdjType.EditValue.AsString();
         }
      }
      #endregion

      public W40100(string programID, string programName) : base(programID, programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtDate.DateTimeValue = GlobalInfo.OCF_DATE;

         //設定 下拉選單
         List<LookupItem> lstType = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "0B", DisplayMember = "一般 / 股票"},
                                        new LookupItem() { ValueMember = "1B", DisplayMember = "長假調整" },
                                        new LookupItem() { ValueMember = "1E", DisplayMember = "長假回調" },
                                        new LookupItem() { ValueMember = "2B", DisplayMember = "處置股票調整"}};

         //設定下拉選單
         ddlAdjType.SetDataTable(lstType, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, null);
         ddlAdjType.EditValue = "0B";
          
         ExportShow.Hide();
      }

      protected override ResultStatus Export() {
         ExportShow.Text = "轉檔中...";
         ExportShow.Show();

         try {

            object[] args = { new D40xxx(), TxtDate, AdjType, _ProgramID };
            IExport40xxxData xmlData = CreateXmlData(GetType(), "ExportXml" + AdjType, args);
            ReturnMessageClass msg = xmlData.GetData();

            if (msg.Status != ResultStatus.Success) {
               ExportShow.Text = MessageDisplay.MSG_IMPORT_FAIL;
               MessageDisplay.Info(MessageDisplay.MSG_NO_DATA);
               return msg.Status;
            }

            msg = xmlData.Export();

            if (msg.Status != ResultStatus.Success) {
               ExportShow.Text = MessageDisplay.MSG_IMPORT_FAIL;
               MessageDisplay.Info(MessageDisplay.MSG_IMPORT_FAIL);
               return msg.Status;
            }

         } catch (Exception ex) {
            ExportShow.Text = MessageDisplay.MSG_IMPORT_FAIL;
            WriteLog(ex);
            return ResultStatus.Fail;
         }
         ExportShow.Text = "轉檔成功!";
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();
         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      private static void ReplaceXmlInnterText(XmlNode element, string oldString, string newString) {

         string innertext = element.InnerText;
         innertext = innertext.Replace(oldString, newString);
         element.InnerText = innertext;

      }

      private static void SetYear(XmlDocument doc) {

         //年號
         ReplaceXmlInnterText(doc.GetElementsByTagName("檔號")[0], "#year#", DateTime.Now.AsTaiwanDateTime("{0}", 4));
         ReplaceXmlInnterText(doc.GetElementsByTagName("年度號")[0], "#year#", DateTime.Now.AsTaiwanDateTime("{0}", 4));
      }

      private static string GenArrayTxt(List<string> kindNameList) {
         string result = "";
         int k = 1;

         foreach (string s in kindNameList) {
            result += s;
            if (k < kindNameList.Count()) {
               if (s != kindNameList[kindNameList.Count() - 2]) {
                  result += "、";
               } else {
                  result += "及";
               }
            }
            k++;
         }

         result = result.TrimEnd('、');
         return result;
      }

      public IExport40xxxData CreateXmlData(Type type, string name, object[] args) {

         string AssemblyName = type.Namespace.Split('.')[0];//最後compile出來的dll名稱
         string className = type.FullName + "+" + name;//完整的class路徑(注意,內部的class執行時其fullName是用+號連結起來)
         return (IExport40xxxData)Assembly.Load(AssemblyName).CreateInstance(className, true, BindingFlags.CreateInstance, null, args, null, null);
      }

      private class ExportXml : IExport40xxxData {
         protected D40xxx Dao { get; }
         protected virtual string TxtDate { get; }
         protected virtual string AdjType { get; }
         protected string ProgramId { get; }
         protected virtual DataTable dt { get; set; }

         protected virtual string AsAdjType {
            get {
               return AdjType.SubStr(0, 1) == "0" ? "" : AdjType.SubStr(0, 1);
            }
         }

         public ExportXml(D40xxx dao, string txtdate, string adjtype, string programId) {
            Dao = dao;
            TxtDate = txtdate;
            AdjType = adjtype;
            ProgramId = programId;
         }

         public virtual ReturnMessageClass GetData() {
            ReturnMessageClass msg = new ReturnMessageClass(MessageDisplay.MSG_NO_DATA);
            msg.Status = ResultStatus.Fail;

            dt = Dao.GetData(TxtDate, AsAdjType, AdjType.SubStr(1, 1));

            if (dt != null) {
               if (dt.Rows.Count > 0) {
                  dt = dt.Sort("SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC, KIND_ID ASC, AB_TYPE ASC");
                  msg.Status = ResultStatus.Success;
               }
            }

            return msg;
         }

         public virtual ReturnMessageClass Export() {
            ReturnMessageClass msg = new ReturnMessageClass();

            return msg;
         }

      }

      /// <summary>
      /// 處置股票 輸出xml
      /// </summary>
      private class ExportXml2B : IXml40xxxData {
         public void Export(DataTable dt, string filePath) {
            try {
               var doc = new XmlDocument();
               doc.Load(filePath);

               dt = dt.Sort("SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC");

               SetYear(doc);

               //說明文
               dt.Filter("ab_type in ('A','-')");
               string descTxt = "本次#kind_name_llist#調整自#issue_begin_ymd#(證券市場處置生效日次一營業日)該股票期貨契約交易時段結束後起實施，" +
                  "並於#issue_end_ymd#該契約交易時段結束後恢復為#issue_begin_ymd#調整前之保證金，參照證券市場處置措施，調整期間如遇休市、" +
                  "有價證券停止買賣、全日暫停交易，則恢復日順延執行。";

               List<string> kindNameList_desc = new List<string>();
               List<string> kindNameList = new List<string>();
               string prodName = "";

               foreach (DataRow dr in dt.Rows) {
                  string abbrName = string.Format("{0}({1})", dr["KIND_ABBR_NAME"].AsString(), dr["kind_id"].AsString());
                  string abbrName_desc = string.Format("{0}({1})", dr["KIND_ABBR_NAME"].AsString(), dr["kind_id"].AsString());

                  DateTime beginYmd = dr["issue_begin_ymd"].AsDateTime("yyyyMMdd");
                  string issueBeginYmd = beginYmd.AsTaiwanDateTime("{0}年{1}月{2}日", 3);

                  DateTime endYmd = dr["issue_end_ymd"].AsDateTime("yyyyMMdd");
                  string issueEndYmd = endYmd.AsTaiwanDateTime("{0}年{1}月{2}日", 3); 

                  if (!kindNameList.Exists(k => k == abbrName)) {
                     kindNameList.Add(abbrName);
                  }

                  if (dr["prod_type"].AsString() == "F") {
                     prodName = "期貨契約保證金及";
                  }

                  if (dr["prod_type"].AsString() == "O" && dr == dt.Rows[dt.Rows.Count - 1]) {
                     abbrName_desc += "之" + prodName + "選擇權契約風險保證金(A值)、風險保證金最低值(B值)";
                  }

                  if (!kindNameList_desc.Exists(k => k == abbrName_desc)) {
                     kindNameList_desc.Add(abbrName_desc);
                  }

                  XmlElement element = doc.CreateElement("文字");
                  element.InnerText = descTxt;
                  ReplaceXmlInnterText(element, "#kind_name_llist#", abbrName);
                  ReplaceXmlInnterText(element, "#issue_begin_ymd#", issueBeginYmd);
                  ReplaceXmlInnterText(element, "#issue_end_ymd#", issueEndYmd);

                  XmlElement element_Tmp = doc.CreateElement("條列");
                  element_Tmp.AppendChild(element);

                  doc.GetElementsByTagName("段落")[0].AppendChild(element_Tmp);

               }

               ReplaceXmlInnterText(doc.GetElementsByTagName("主旨")[0].ChildNodes[0], "#kind_name_list#", GenArrayTxt(kindNameList_desc));
               ReplaceXmlInnterText(doc.GetElementsByTagName("段落")[0].ChildNodes[1].ChildNodes[0], "#kind_name_list#", GenArrayTxt(kindNameList));

               doc.Save(filePath);
            } catch (Exception ex) {
               throw ex;
            }
         }
      }

      /// <summary>
      /// 長假調整 輸出xml
      /// </summary>
      private class ExportXml1B : IXml40xxxData {
         public void Export(DataTable dt, string filePath) {
            try {
               var doc = new XmlDocument();
               doc.Load(filePath);

               dt = dt.Sort("SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC");

               SetYear(doc);

               //說明文
               dt.Filter("ab_type in ('A','-')");

               List<string> kindNameList_desc = new List<string>();
               string prodName = "";

               foreach (DataRow dr in dt.Rows) {
                  string abbrName_desc = dr["KIND_ABBR_NAME"].AsString();

                  if (dr["prod_subtype"].AsString() == "S") {
                     abbrName_desc = abbrName_desc + "契約(" + dr["kind_id"].AsString() + ")";
                  }

                  DateTime beginYmd = dr["issue_begin_ymd"].AsDateTime("yyyyMMdd");
                  string issueBeginYmd = beginYmd.AsTaiwanDateTime("{0}年{1}月{2}日", 3);

                  DateTime endYmd = dr["issue_end_ymd"].AsDateTime("yyyyMMdd");
                  string issueEndYmd = endYmd.AsTaiwanDateTime("{0}年{1}月{2}日", 3);

                  if (dr["prod_type"].AsString() == "F") {
                     prodName = "期貨契約保證金及";
                  }

                  if (dr["prod_type"].AsString() == "O" && dr == dt.Rows[dt.Rows.Count - 1]) {
                     abbrName_desc += "之" + prodName + "選擇權契約風險保證金(A值)、風險保證金最低值(B值)";
                  }

                  if (!kindNameList_desc.Exists(k => k == abbrName_desc)) {
                     kindNameList_desc.Add(abbrName_desc);
                  }
               }

               string implBeginDate = dt.Rows[0]["impl_begin_ymd"].AsString();
               string implEndDate = dt.Rows[0]["impl_end_ymd"].AsString();
               string mocfDate = new MOCF().GetMaxOcfDate(implBeginDate, implEndDate);

               implBeginDate = implBeginDate.AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);
               implEndDate = implEndDate.AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);
               mocfDate = mocfDate.AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);

               //主旨 -> 文字
               ReplaceXmlInnterText(doc.GetElementsByTagName("主旨")[0].ChildNodes[0], "#impl_begin_ymd#", implBeginDate);
               ReplaceXmlInnterText(doc.GetElementsByTagName("主旨")[0].ChildNodes[0], "#impl_end_ymd#", implEndDate);
               //主旨裡商品
               ReplaceXmlInnterText(doc.GetElementsByTagName("主旨")[0].ChildNodes[0], "#kind_name_list#", GenArrayTxt(kindNameList_desc));

               //段落 序號三
               ReplaceXmlInnterText(doc.GetElementsByTagName("段落")[0].ChildNodes[2].ChildNodes[0], "#mocf_ymd#", mocfDate);
               ReplaceXmlInnterText(doc.GetElementsByTagName("段落")[0].ChildNodes[2].ChildNodes[0], "#impl_end_ymd#", implEndDate);

               doc.Save(filePath);
            } catch (Exception ex) {
               throw ex;
            }
         }
      }

      /// <summary>
      /// 長假回調 輸出xml
      /// </summary>
      private class ExportXml1E : IXml40xxxData {
         public void Export(DataTable dt, string filePath) {
            try {
               var doc = new XmlDocument();
               doc.Load(filePath);

               dt = dt.Sort("SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC");

               SetYear(doc);

               //說明文
               dt.Filter("ab_type in ('A','-')");

               List<string> fullNameList = new List<string>();
               List<string> kindNameList_desc = new List<string>();
               string prodName = "";

               foreach (DataRow dr in dt.Rows) {
                  string abbrName_desc = dr["KIND_ABBR_NAME"].AsString();
                  string fullName = "本公司" + dr["rule_full_name"].AsString() + "交易規則";

                  if (dr["prod_subtype"].AsString() == "S") {
                     abbrName_desc = abbrName_desc + "契約(" + dr["kind_id"].AsString() + ")";
                  }

                  if (dr["prod_type"].AsString() == "F") {
                     prodName = "期貨契約保證金及";
                  }

                  if (dr["prod_type"].AsString() == "O" && dr == dt.Rows[dt.Rows.Count - 1]) {
                     abbrName_desc += "之" + prodName + "選擇權契約風險保證金(A值)、風險保證金最低值(B值)";
                  }

                  if (!kindNameList_desc.Exists(k => k == abbrName_desc)) {
                     kindNameList_desc.Add(abbrName_desc);
                  }

                  if (!fullNameList.Exists(f => f == fullName)) {
                     fullNameList.Add(fullName);
                  }
               }

               string beginYmd = dt.Rows[0]["issue_begin_ymd"].AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);

               ReplaceXmlInnterText(doc.GetElementsByTagName("主旨")[0].ChildNodes[0], "#kind_name_list#", GenArrayTxt(kindNameList_desc));
               ReplaceXmlInnterText(doc.GetElementsByTagName("主旨")[0].ChildNodes[0], "#issue_begin_ymd#", beginYmd);
               ReplaceXmlInnterText(doc.GetElementsByTagName("段落")[0].ChildNodes[0].ChildNodes[0], "#full_name_llist#", GenArrayTxt(fullNameList));

               doc.Save(filePath);
            } catch (Exception ex) {
               throw ex;
            }
         }
      }

      /// <summary>
      /// 一般 / 股票 輸出xml
      /// </summary>
      private class ExportXml0B : IXml40xxxData {
         public void Export(DataTable dt, string filePath) {
            try {
               var doc = new XmlDocument();
               doc.Load(filePath);

               dt = dt.Sort("SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC, KIND_ID ASC, AB_TYPE ASC");

               SetYear(doc);

               //說明文
               dt.Filter("ab_type in ('A','-')");

               List<string> fullNameList = new List<string>();
               List<string> kindNameList = new List<string>();
               string prodName = "";

               foreach (DataRow dr in dt.Rows) {
                  string abbrName = dr["KIND_ABBR_NAME"].AsString();
                  string fullName = "本公司" + dr["rule_full_name"].AsString() + "交易規則";

                  if (dr["prod_subtype"].AsString() == "S") {
                     abbrName = abbrName + "契約(" + dr["kind_id"].AsString() + ")";
                  }

                  if (!kindNameList.Exists(k => k == abbrName)) {
                     kindNameList.Add(abbrName);
                  }

                  if (dr["prod_type"].AsString() == "F") {
                     prodName = "期貨契約保證金及";
                  }

                  if (dr["prod_type"].AsString() == "O" && dr == dt.Rows[dt.Rows.Count - 1]) {
                     abbrName += "之" + prodName + "選擇權契約風險保證金(A值)、風險保證金最低值(B值)";
                  }

                  if (!fullNameList.Exists(f => f == fullName)) {
                     fullNameList.Add(fullName);
                  }
               }

               string beginDate = dt.Rows[0]["issue_begin_ymd"].AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);

               ReplaceXmlInnterText(doc.GetElementsByTagName("主旨")[0].ChildNodes[0], "#kind_name_list#", GenArrayTxt(kindNameList));
               ReplaceXmlInnterText(doc.GetElementsByTagName("主旨")[0].ChildNodes[0], "#issue_begin_ymd#", beginDate);
               ReplaceXmlInnterText(doc.GetElementsByTagName("段落")[0].ChildNodes[0].ChildNodes[0], "#full_name_llist#", GenArrayTxt(fullNameList));
               ReplaceXmlInnterText(doc.GetElementsByTagName("段落")[0].ChildNodes[1].ChildNodes[0], "#kind_name_list#", GenArrayTxt(kindNameList));

               if (dt.Select("amt_type = 'F'").Count() > 0) {
                  ReplaceXmlInnterText(doc.GetElementsByTagName("主旨")[0].ChildNodes[0], "#amt_type#", "金額");
               } else {
                  ReplaceXmlInnterText(doc.GetElementsByTagName("主旨")[0].ChildNodes[0], "#amt_type#", "適用比例");
               }

               doc.Save(filePath);
            } catch (Exception ex) {
               throw ex;
            }
         }

      }

   }
}