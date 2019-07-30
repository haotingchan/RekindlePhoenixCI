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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

/// <summary>
/// 依照不同類別產生 ifd
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

      public W40100(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtDate.DateTimeValue = GlobalInfo.OCF_DATE;
         //設定 下拉選單
         //List<LookupItem> lstType = new List<LookupItem>(){
         //                               new LookupItem() { ValueMember = "0B", DisplayMember = "一般 / 股票"},
         //                               new LookupItem() { ValueMember = "1B", DisplayMember = "長假調整" },
         //                               new LookupItem() { ValueMember = "1E", DisplayMember = "長假回調" },
         //                               new LookupItem() { ValueMember = "2B", DisplayMember = "處置股票調整"}};

         DataTable dtType = new CODW().ListLookUpEdit("MGD2" , "MGD2_ADJ_TYPE");
         //設定下拉選單
         ddlAdjType.SetDataTable(dtType , "CODW_ID" , "CODW_DESC" , TextEditStyles.DisableTextEditor , null);
         ddlAdjType.ItemIndex = 0; // 0B

         ExportShow.Hide();
      }

      protected override ResultStatus Open() {
         base.Open();

         if (!FlagAdmin) { //此管理者測試功能並未實作
            groupAdmin.Visible = false;
            chkTxt.Visible = false;
         } else {
            groupAdmin.Visible = true;
            chkTxt.Visible = true;
         }
         return ResultStatus.Success;
      }

      protected override ResultStatus Export() {
         ExportShow.Text = "轉檔中...";
         ExportShow.Show();
         try {

            object[] args = { new D40xxx() , TxtDate , AdjType , _ProgramID };
            IExport40xxxData xmlData = CreateXmlData(GetType() , "ExportXml" + AdjType , args);
            ReturnMessageClass msg = xmlData.GetData();

            //無資料時不產檔
            if (msg.Status != ResultStatus.Success) {
               ExportShow.Text = MessageDisplay.MSG_IMPORT_FAIL;
               MessageDisplay.Info($"{txtDate.DateTimeValue.ToShortDateString()},{_ProgramID}-{ddlAdjType.Properties.GetDisplayText(AdjType)},{MessageDisplay.MSG_NO_DATA}");
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

      public IExport40xxxData CreateXmlData(Type type , string name , object[] args) {

         string AssemblyName = type.Namespace.Split('.')[0];//最後compile出來的dll名稱
         string className = type.FullName + "+" + name;//完整的class路徑(注意,內部的class執行時其fullName是用+號連結起來)
         return (IExport40xxxData)Assembly.Load(AssemblyName).CreateInstance(className , true , BindingFlags.CreateInstance , null , args , null , null);
      }

      private class ExportXml : IExport40xxxData {
         protected D40xxx Dao { get; }
         protected virtual string TxtDate { get; }
         protected virtual string AdjType { get; }
         protected string ProgramId { get; }
         protected virtual DataTable Dt { get; set; }

         protected virtual string AsAdjType {
            get {
               return AdjType.SubStr(0 , 1) == "0" ? "" : AdjType.SubStr(0 , 1);
            }
         }
         protected virtual string FilePath { get; set; }
         protected virtual XmlDocument Doc { get; set; }
         protected virtual List<string> KindNameList_Desc { get; set; }
         protected virtual List<string> KindNameList { get; set; }

         protected virtual string DescTxt { get; set; }
         protected virtual bool AddDescElement { get; set; }

         public ExportXml(D40xxx dao , string txtdate , string adjtype , string programId) {
            Dao = dao;
            TxtDate = txtdate;
            AdjType = adjtype;
            ProgramId = programId;
            AddDescElement = false;

            KindNameList_Desc = new List<string>();
            KindNameList = new List<string>();
         }

         public virtual ReturnMessageClass GetData() {
            ReturnMessageClass msg = new ReturnMessageClass(MessageDisplay.MSG_NO_DATA);
            msg.Status = ResultStatus.Fail;

            Dt = Dao.GetData(TxtDate , AsAdjType , AdjType.SubStr(1 , 1));

            //無資料時return fail (不產檔)
            if (Dt != null) {
               if (Dt.Rows.Count > 0) {
                  msg.Status = ResultStatus.Success;
               }
            }

            Dt.Filter("ab_type in ('A','-')");
            return msg;
         }

         public virtual ReturnMessageClass Export() {
            ReturnMessageClass msg = new ReturnMessageClass();
            msg.Status = ResultStatus.Fail;

            try {
               FilePath = PbFunc.wf_copy_file(ProgramId , $"{ProgramId}_{AdjType}");

               OpenFileAndSetYear();

               //說明文
               string prepoStr = Dt.AsEnumerable().Any(d => d.Field<string>("prod_type") == "F") ? "期貨契約保證金及" : "";

               foreach (DataRow dr in Dt.Rows) {
                  string abbrName = string.Format("{0}({1})" , dr["KIND_ABBR_NAME"].AsString() , dr["kind_id"].AsString());
                  string abbrName_Desc = string.Format("{0}({1})" , dr["KIND_ABBR_NAME"].AsString() , dr["kind_id"].AsString());

                  GenKindNameList(dr , prepoStr , abbrName , abbrName_Desc);

                  if (AddDescElement) MakeDescElement(dr , abbrName);
               }

               ReplaceElementWord(GenArrayTxt(KindNameList_Desc) , GenArrayTxt(KindNameList));

               Doc.Save(FilePath);

               msg.Status = ResultStatus.Success;
               return msg;

            } catch (Exception ex) {
               ErrorHandle(ex , msg);
               return msg;
            }
         }

         /// <summary>
         /// 開檔並設定年度號
         /// </summary>
         protected virtual void OpenFileAndSetYear() {
            Doc = new XmlDocument();
            Doc.Load(FilePath);

            //年號
            ReplaceXmlInnterText(Doc.GetElementsByTagName("檔號")[0] , "#year#" , DateTime.Now.AsTaiwanDateTime("{0}" , 4));
            ReplaceXmlInnterText(Doc.GetElementsByTagName("年度號")[0] , "#year#" , DateTime.Now.AsTaiwanDateTime("{0}" , 4));
         }

         /// <summary>
         /// 取代node 內文字
         /// </summary>
         /// <param name="element"></param>
         /// <param name="oldString">被取代問字</param>
         /// <param name="newString">取代文字</param>
         protected virtual void ReplaceXmlInnterText(XmlNode element , string oldString , string newString) {

            string innertext = element.InnerText;
            innertext = innertext.Replace(oldString , newString);
            element.InnerText = innertext;
         }

         protected virtual void ReplaceElementWord(params string[] args) {

            ReplaceXmlInnterText(Doc.GetElementsByTagName("主旨")[0].ChildNodes[0] , "#kind_name_list#" , args[0]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[1].ChildNodes[0] , "#kind_name_list#" , args[1]);
         }

         /// <summary>
         /// 將list 組成字串, 中間以"、"連接
         /// </summary>
         /// <param name="kindNameList"></param>
         /// <returns></returns>
         protected virtual string GenArrayTxt(List<string> kindNameList) {
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

         protected virtual void GenKindNameList(DataRow dr , string prepoStr , string abbrName , string abbrName_Desc) {

            if (dr["prod_subtype"].AsString() == "S") {
               abbrName = abbrName + "契約(" + dr["kind_id"].AsString() + ")";
            }

            if (!KindNameList.Exists(k => k == abbrName)) {
               KindNameList.Add(abbrName);
            }

            if (dr["prod_type"].AsString() == "O" && dr == Dt.Rows[Dt.Rows.Count - 1]) {
               abbrName += "之" + prepoStr + "選擇權契約風險保證金(A值)、風險保證金最低值(B值)";
            }

            if (!KindNameList_Desc.Exists(f => f == abbrName_Desc)) {
               KindNameList_Desc.Add(abbrName_Desc);
            }
         }

         /// <summary>
         /// 新增說明tag
         /// </summary>
         /// <param name="dr"></param>
         /// <param name="abbrName">商品中說明</param>
         protected virtual void MakeDescElement(DataRow dr , string abbrName) {
            DateTime beginYmd = dr["issue_begin_ymd"].AsDateTime("yyyyMMdd");
            string issueBeginYmd = beginYmd.AsTaiwanDateTime("{0}年{1}月{2}日" , 3);

            DateTime endYmd = dr["issue_end_ymd"].AsDateTime("yyyyMMdd");
            string issueEndYmd = endYmd.AsTaiwanDateTime("{0}年{1}月{2}日" , 3);

            XmlElement element = Doc.CreateElement("文字");
            element.InnerText = DescTxt;
            ReplaceXmlInnterText(element , "#kind_name_llist#" , abbrName);
            ReplaceXmlInnterText(element , "#issue_begin_ymd#" , issueBeginYmd);
            ReplaceXmlInnterText(element , "#issue_end_ymd#" , issueEndYmd);

            XmlElement element_Tmp = Doc.CreateElement("條列");
            element_Tmp.AppendChild(element);

            Doc.GetElementsByTagName("段落")[0].AppendChild(element_Tmp);
         }

         /// <summary>
         /// 錯誤處理
         /// </summary>
         /// <param name="ex">Exception</param>
         /// <param name="msg"></param>
         public virtual void ErrorHandle(Exception ex , ReturnMessageClass msg) {
            WriteLog(ex.ToString() , "Info" , "Z");
            msg.Status = ResultStatus.Fail;
            msg.ReturnMessage = MessageDisplay.MSG_IMPORT_FAIL;
         }

         /// <summary>
         /// 錯誤寫log
         /// </summary>
         /// <param name="msg"></param>
         /// <param name="logType"></param>
         /// <param name="operationType"></param>
         public virtual void WriteLog(string msg , string logType = "Info" , string operationType = "") {
            bool isNeedWriteFile = true;

            try {
               //ken,LOGF_KEY_DATA長度要取前100字元,但是logf.LOGF_KEY_DATA型態為VARCHAR2 (100 Byte),如果有中文會算2byte...先取前80吧
               new LOGF().Insert(GlobalInfo.USER_ID , ProgramId , msg.SubStr(0 , 80) , operationType);

            } catch (Exception ex2) {
               // write log to db failed , ready write file to local
               isNeedWriteFile = true;
               msg = ex2.ToString();
               MessageDisplay.Error("資料庫連線發生錯誤,先將錯誤訊息寫到檔案");
            }//try {

            //2.write file to local
            if (isNeedWriteFile) {
               try {
                  string filename = "log_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                  string filepath = Path.Combine(Application.StartupPath , "Log" , DateTime.Today.ToString("yyyyMM"));
                  Directory.CreateDirectory(filepath);
                  filepath = Path.Combine(filepath , filename);
                  using (StreamWriter sw = File.AppendText(filepath)) {
                     sw.WriteLine("");
                     sw.WriteLine("");
                     sw.WriteLine("==============================");
                     sw.WriteLine("datetime=" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                     sw.WriteLine("userId=" + GlobalInfo.USER_ID);
                     sw.WriteLine("txnId=" + ProgramId);
                     sw.WriteLine("logType=" + logType);
                     sw.WriteLine("operationType=" + operationType);
                     sw.Write("msg=" + msg);
                     sw.WriteLine("");
                     if (msg != "")
                        sw.Write("dbErrorMsg=" + msg);
                  }//using (StreamWriter sw = File.AppendText(filepath)) {
               } catch (Exception fileEx) {
                  MessageDisplay.Error("將log寫入檔案發生錯誤,請聯絡管理員" + Environment.NewLine + "msg=" + fileEx.Message);
                  return;
               }
            }//if (isNeedWriteFile) 
         }
      }

      /// <summary>
      /// 處置股票 輸出xml
      /// </summary>
      private class ExportXml2B : ExportXml {
         public ExportXml2B(D40xxx dao , string txtdate , string adjtype , string programId) :
                              base(dao , txtdate , adjtype , programId) {

            AddDescElement = true;
            DescTxt = "本次#kind_name_llist#調整自#issue_begin_ymd#(證券市場處置生效日次一營業日)該股票期貨契約交易時段結束後起實施，" +
                  "並於#issue_end_ymd#該契約交易時段結束後恢復為#issue_begin_ymd#調整前之保證金，參照證券市場處置措施，調整期間如遇休市、" +
                  "有價證券停止買賣、全日暫停交易，則恢復日順延執行。";
         }

         public override ReturnMessageClass Export() {
            ReturnMessageClass msg = base.Export();
            return msg;
         }

         protected override void GenKindNameList(DataRow dr , string prepoStr , string abbrName , string abbrName_Desc) {

            if (!KindNameList.Exists(k => k == abbrName)) {
               KindNameList.Add(abbrName);
            }

            if (dr["prod_type"].AsString() == "O" && dr == Dt.Rows[Dt.Rows.Count - 1]) {
               abbrName_Desc += "之" + prepoStr + "選擇權契約風險保證金(A值)、風險保證金最低值(B值)";
            }

            if (!KindNameList_Desc.Exists(k => k == abbrName_Desc)) {
               KindNameList_Desc.Add(abbrName_Desc);
            }
         }
      }

      /// <summary>
      /// 長假調整 輸出xml
      /// </summary>
      private class ExportXml1B : ExportXml {
         public ExportXml1B(D40xxx dao , string txtdate , string adjtype , string programId) :
                     base(dao , txtdate , adjtype , programId) { }

         public override ReturnMessageClass Export() {
            ReturnMessageClass msg = new ReturnMessageClass();
            msg.Status = ResultStatus.Fail;

            try {
               FilePath = PbFunc.wf_copy_file(ProgramId , $"{ProgramId}_{AdjType}");

               base.OpenFileAndSetYear();

               //說明文
               string prepoStr = Dt.AsEnumerable().Any(d => d.Field<string>("prod_type") == "F") ? "期貨契約保證金及" : "";

               foreach (DataRow dr in Dt.Rows) {
                  string abbrName_Desc = dr["KIND_ABBR_NAME"].AsString();

                  GenKindNameList(dr , prepoStr , "" , abbrName_Desc);
               }

               string implBeginDate = Dt.Rows[0]["impl_begin_ymd"].AsString();
               string implEndDate = Dt.Rows[0]["impl_end_ymd"].AsString();
               string mocfDate = new MOCF().GetMaxOcfDate(implBeginDate , implEndDate);

               implBeginDate = implBeginDate.AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日" , 3);
               implEndDate = implEndDate.AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日" , 3);
               mocfDate = mocfDate.AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日" , 3);

               ReplaceElementWord(implBeginDate , implEndDate , GenArrayTxt(KindNameList_Desc) , mocfDate , implEndDate);

               Doc.Save(FilePath);
               msg.Status = ResultStatus.Success;
               return msg;

            } catch (Exception ex) {
               base.ErrorHandle(ex , msg);
               return msg;
            }
         }

         protected override void ReplaceElementWord(params string[] args) {

            //主旨 -> 文字
            ReplaceXmlInnterText(Doc.GetElementsByTagName("主旨")[0].ChildNodes[0] , "#impl_begin_ymd#" , args[0]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("主旨")[0].ChildNodes[0] , "#impl_end_ymd#" , args[1]);
            //主旨裡商品
            ReplaceXmlInnterText(Doc.GetElementsByTagName("主旨")[0].ChildNodes[0] , "#kind_name_list#" , args[2]);

            //段落 序號三
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[2].ChildNodes[0] , "#mocf_ymd#" , args[3]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[2].ChildNodes[0] , "#impl_end_ymd#" , args[4]);
         }

         protected override void GenKindNameList(DataRow dr , string prepoStr , string abbrName , string abbrName_Desc) {

            if (dr["prod_subtype"].AsString() == "S") {
               abbrName_Desc = abbrName_Desc + "契約(" + dr["kind_id"].AsString() + ")";
            }

            if (dr["prod_type"].AsString() == "O" && dr == Dt.Rows[Dt.Rows.Count - 1]) {
               abbrName_Desc += "之" + prepoStr + "選擇權契約風險保證金(A值)、風險保證金最低值(B值)";
            }

            if (!KindNameList_Desc.Exists(k => k == abbrName_Desc)) {
               KindNameList_Desc.Add(abbrName_Desc);
            }
         }
      }

      /// <summary>
      /// 長假回調 輸出xml
      /// </summary>
      private class ExportXml1E : ExportXml {
         public ExportXml1E(D40xxx dao , string txtdate , string adjtype , string programId) :
            base(dao , txtdate , adjtype , programId) { }

         public override ReturnMessageClass Export() {
            ReturnMessageClass msg = new ReturnMessageClass();
            msg.Status = ResultStatus.Fail;
            try {
               FilePath = PbFunc.wf_copy_file(ProgramId , $"{ProgramId}_{AdjType}");

               base.OpenFileAndSetYear();

               //說明文
               string prepoStr = Dt.AsEnumerable().Any(d => d.Field<string>("prod_type") == "F") ? "期貨契約保證金及" : "";

               foreach (DataRow dr in Dt.Rows) {
                  string abbrName = dr["KIND_ABBR_NAME"].AsString();
                  string fullName = "本公司" + dr["rule_full_name"].AsString() + "交易規則";

                  GenKindNameList(dr , prepoStr , abbrName , fullName);
               }

               string beginYmd = Dt.Rows[0]["issue_begin_ymd"].AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日" , 3);
               ReplaceElementWord(GenArrayTxt(KindNameList_Desc) , beginYmd , GenArrayTxt(KindNameList));

               Doc.Save(FilePath);
               msg.Status = ResultStatus.Success;
               return msg;
            } catch (Exception ex) {
               base.ErrorHandle(ex , msg);
               return msg;
            }
         }

         protected override void ReplaceElementWord(params string[] args) {

            ReplaceXmlInnterText(Doc.GetElementsByTagName("主旨")[0].ChildNodes[0] , "#kind_name_list#" , args[0]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("主旨")[0].ChildNodes[0] , "#issue_begin_ymd#" , args[1]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[0].ChildNodes[0] , "#full_name_llist#" , args[2]);
         }

         protected override void GenKindNameList(DataRow dr , string prepoStr , string abbrName , string abbrName_Desc) {
            if (dr["prod_subtype"].AsString() == "S") {
               abbrName = abbrName + "契約(" + dr["kind_id"].AsString() + ")";
            }

            if (dr["prod_type"].AsString() == "F") {
               prepoStr = "期貨契約保證金及";
            }

            if (dr["prod_type"].AsString() == "O" && dr == Dt.Rows[Dt.Rows.Count - 1]) {
               abbrName += "之" + prepoStr + "選擇權契約風險保證金(A值)、風險保證金最低值(B值)";
            }

            if (!KindNameList_Desc.Exists(k => k == abbrName)) {
               KindNameList_Desc.Add(abbrName);
            }

            if (!KindNameList.Exists(f => f == abbrName_Desc)) {
               KindNameList.Add(abbrName_Desc);
            }
         }
      }

      /// <summary>
      /// 一般 / 股票 輸出xml
      /// </summary>
      private class ExportXml0B : ExportXml {

         public ExportXml0B(D40xxx dao , string txtdate , string adjtype , string programId) :
                     base(dao , txtdate , adjtype , programId) { }

         public override ReturnMessageClass GetData() {
            ReturnMessageClass msg = new ReturnMessageClass(MessageDisplay.MSG_NO_DATA);

            Dt = Dao.GetData(TxtDate , AsAdjType , AdjType.SubStr(1 , 1));

            //一般 / 股票 要多撈一次資料
            if (AdjType == "0B") {
               DataTable dtTmp = Dao.GetData(TxtDate , "3" , AdjType.SubStr(1 , 1));
               if (dtTmp != null) {
                  if (dtTmp.Rows.Count > 0) {
                     foreach (DataRow r in dtTmp.Rows) {
                        DataRow addRow = r;
                        Dt.ImportRow(r);
                     }
                  }
               }
            }

            if (Dt == null) {
               msg.Status = ResultStatus.Fail;
               return msg;
            }

            if (Dt.Rows.Count <= 0) {
               msg.Status = ResultStatus.Fail;
               return msg;
            }

            msg.Status = ResultStatus.Success;
            return msg;
         }

         public override ReturnMessageClass Export() {
            ReturnMessageClass msg = new ReturnMessageClass();
            msg.Status = ResultStatus.Fail;

            try {
               FilePath = PbFunc.wf_copy_file(ProgramId , $"{ProgramId}_{AdjType}");

               base.OpenFileAndSetYear();

               string prepoStr = Dt.AsEnumerable().Any(d => d.Field<string>("prod_type") == "F") ? "期貨契約保證金及" : "";
               foreach (DataRow dr in Dt.Rows) {
                  string abbrName = dr["KIND_ABBR_NAME"].AsString();
                  string abbrName_Desc = dr["rule_full_name"].AsString() + "交易規則";

                  base.GenKindNameList(dr , prepoStr , abbrName , abbrName_Desc);
               }

               string beginDate = Dt.Rows[0]["issue_begin_ymd"].AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日" , 3);
               ReplaceElementWord(GenArrayTxt(KindNameList) , beginDate , GenArrayTxt(KindNameList_Desc) , GenArrayTxt(KindNameList));

               Doc.Save(FilePath);
               msg.Status = ResultStatus.Success;
               return msg;
            } catch (Exception ex) {
               base.ErrorHandle(ex , msg);
               return msg;
            }
         }

         protected override void ReplaceElementWord(params string[] args) {

            ReplaceXmlInnterText(Doc.GetElementsByTagName("主旨")[0].ChildNodes[0] , "#kind_name_list#" , args[0]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("主旨")[0].ChildNodes[0] , "#issue_begin_ymd#" , args[1]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[0].ChildNodes[0] , "#full_name_llist#" , args[2]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[1].ChildNodes[0] , "#kind_name_list#" , args[3]);

            //當 amt_type ="F" 時 文字不同, 特殊處理
            if (Dt.Select("amt_type = 'F'").Count() > 0) {
               ReplaceXmlInnterText(Doc.GetElementsByTagName("主旨")[0].ChildNodes[0] , "#amt_type#" , "金額");
            } else {
               ReplaceXmlInnterText(Doc.GetElementsByTagName("主旨")[0].ChildNodes[0] , "#amt_type#" , "適用比例");
            }
         }
      }
   }
}