﻿using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
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
/// Test Data 3B 20190102 / 1B 20190129 / 1E 20190212 / 0B 20190212
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
   public partial class W40030 : FormParent {

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

      public W40030(string programID, string programName) : base(programID, programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtDate.DateTimeValue = DateTime.Now;
         //設定 下拉選單
         List<LookupItem> lstType = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "0B", DisplayMember = "一般"},
                                        new LookupItem() { ValueMember = "1B", DisplayMember = "長假調整" },
                                        new LookupItem() { ValueMember = "1E", DisplayMember = "長假回調" },
                                        new LookupItem() { ValueMember = "3B", DisplayMember = "股票"}};

         //設定下拉選單
         ddlAdjType.SetDataTable(lstType, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, null);
         ddlAdjType.EditValue = "0B";

#if DEBUG
         txtDate.DateTimeValue = "2019/01/29".AsDateTime("yyyy/MM/dd");
         ddlAdjType.EditValue = "1B";
#endif

         ExportShow.Hide();
      }

      protected override ResultStatus Export() {
         ExportShow.Text = "轉檔中...";
         ExportShow.Show();
         try {

            object[] args = { TxtDate, AdjType, _ProgramID };
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

      public IExport40xxxData CreateXmlData(Type type, string name, object[] args) {

         string AssemblyName = type.Namespace.Split('.')[0];//最後compile出來的dll名稱
         string className = type.FullName + "+" + name;//完整的class路徑(注意,內部的class執行時其fullName是用+號連結起來)
         return (IExport40xxxData)Assembly.Load(AssemblyName).CreateInstance(className, true, BindingFlags.CreateInstance, null, args, null, null);
      }

      private class ExportWord : IExport40xxxData {
         protected RPTF DaoRptf { get; }
         protected D40030 Dao40030 { get; }
         protected virtual string TxtDate { get; }
         protected virtual string AdjType { get; }
         protected string ProgramId { get; }

         protected virtual DataTable Dt { get; set; }
         protected virtual string AsAdjType {
            get {
               return AdjType.SubStr(0, 1) == "0" ? "" : AdjType.SubStr(0, 1);
            }
         }
         protected virtual string FilePath { get; set; }
         protected virtual RichEditDocumentServer DocSer { get; set; }
         protected virtual Document Doc { get; set; }

         protected virtual DataTable DtAgenda { get; set; }
         protected virtual DataTable DtMinutes { get; set; }
         protected virtual DataTable DtAbroad { get; set; }

         protected virtual DataTable DtSpan { get; set; }
         protected virtual string DescStr { get; set; }
         protected virtual string OswGrp { get; set; }
         protected virtual string CaseDescStr { get; set; }
         protected virtual string[] ChineseNumber { get; }

         public ExportWord(string txtdate, string adjtype, string programId) {
            DaoRptf = new RPTF();
            Dao40030 = new D40030();
            TxtDate = txtdate;
            AdjType = adjtype;
            ProgramId = programId;

            ChineseNumber = new string[] { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
         }

         public virtual ReturnMessageClass GetData() {
            ReturnMessageClass msg = new ReturnMessageClass(MessageDisplay.MSG_NO_DATA);
            msg.Status = ResultStatus.Fail;

            Dt = Dao40030.GetData(TxtDate, OswGrp, AsAdjType, AdjType.SubStr(1, 1));

            if (Dt != null) {
               if (Dt.Rows.Count > 0) {
                  msg.Status = ResultStatus.Success;
               }
            }

            return msg;
         }

         protected virtual void GetRPTF() {
            DtAgenda = DaoRptf.ListData("49074", "49074", "agenda");
            DtMinutes = DaoRptf.ListData("49074", "49074", "minutes");
         }

         protected virtual void GetAborad() {
            DtAbroad = Dao40030.GetAborad(TxtDate.AsDateTime("yyyyMMdd"), OswGrp);
         }

         protected virtual void GetSpan() {
            DtSpan = Dao40030.GetSpan(TxtDate.AsDateTime("yyyyMMdd"), OswGrp, AdjType, AsAdjType);
         }

         public virtual ReturnMessageClass Export() {
            ReturnMessageClass msg = new ReturnMessageClass();
            msg.Status = ResultStatus.Fail;

            try {
               FilePath = PbFunc.wf_copy_file(ProgramId, "40030_MeetingLog");

               //開檔
               OpenFile();

               //取得會議紀錄 / 議程資訊
               GetRPTF();

               //
               GetAborad();

               //
               GetSpan();


               //會議記錄
               OpenFile();

               #region 表頭

               string chairman = DtMinutes.Rows[0]["RPTF_TEXT"].AsString();

               CaseDescStr = CaseDescStr.Replace("#kind_name_list#", GenProdName(Dt, "契約"));


               DescStr = GenDescStr();


               SetRtfDescText(GenMeetingDate(), chairman, GenAttend(DtMinutes), CaseDescStr, DescStr);

               #endregion

               SetAllNumberAndEnglishFont(Doc);//設定英數字體

               DocSer.SaveDocument(FilePath, DocumentFormat.Rtf);
               DocSer.Dispose();
               msg.Status = ResultStatus.Success;
#if DEBUG
               System.Diagnostics.Process.Start(FilePath);
#endif
               return msg;

            } catch (Exception ex) {
               ErrorHandle(ex, msg);
               return msg;
            }
         }

         protected virtual void OpenFile() {
            DocSer = new RichEditDocumentServer();
            DocSer.LoadDocument(FilePath);
            Doc = DocSer.Document;
         }

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

         /// <summary>
         /// 產生 rtf 上方prod Lsit
         /// </summary>
         /// <param name="dt"></param>
         /// <param name="contract">後贅字</param>
         /// <returns></returns>
         protected virtual string GenProdName(DataTable dt, string contract = "") {
            string result = "";
            int k = 0;

            foreach (DataRow dr in dt.Rows) {
               string prodSubType = dr["prod_subtype"].AsString();
               string abbrName = dr["kind_abbr_name"].AsString();

               if (prodSubType == "S") {
                  result += $"{abbrName + contract}({dr["kind_id"].AsString()})";
               } else {
                  result += abbrName;
               }

               if (k == (dt.Rows.Count - 2)) {
                  result += "及";
               } else {
                  result += "、";
               }

               k++;
            }

            result = result.TrimEnd('、');
            result += "之";

            if (dt.Select("prod_type='O'").Count<DataRow>() > 0 &&
               dt.Select("prod_type='F'").Count<DataRow>() > 0) {

               result += "期貨契約保證金及選擇權契約風險保證金(A值)、風險保證金最低值(B值)";
            } else if (dt.Select("prod_type='F'").Count<DataRow>() > 0 &&
                 dt.Select("prod_type='O'").Count<DataRow>() <= 0) {

               result += "期貨契約保證金";
            } else if (dt.Select("prod_type='F'").Count<DataRow>() <= 0 &&
               dt.Select("prod_type='O'").Count<DataRow>() > 0) {

               result += "選擇權契約風險保證金(A值)、風險保證金最低值(B值)";
            }

            return result;
         }

         protected virtual string GenAttend(DataTable dtAttend) {
            string result = "";
            int k = 0;

            dtAttend = dtAttend.Select("RPTF_SEQ_NO <> 1").CopyToDataTable();

            foreach (DataRow dr in dtAttend.Rows) {

               result += dr["RPTF_TEXT"].AsString() + "\n\t";

               k++;
            }
            result = result.TrimEnd('\n', '\t');

            return result;
         }

         protected virtual string GenMeetingDate() {
            DateTime dtNow = DateTime.Now;

            string date = dtNow.AsTaiwanDateTime("{0}年{1}月{2}日", 3);
            string week = dtNow.DayOfWeek.ToString();

            string result = $"{date}({week})下午5時10分";

            return result;
         }

         protected virtual string GenProdSubtypeList(IEnumerable<IGrouping<string, DataRow>> listSubType, string contract = "") {
            string result = "";
            int k = 1;

            foreach (var s in listSubType) {
               result += s.Key + contract;

               if (k == listSubType.Count() - 1) {
                  result += "及";
               } else {
                  result += "、";
               }

               k++;
            }

            result = result.TrimEnd('、');
            return result;
         }

         protected virtual string GenAdjRate(IEnumerable<IGrouping<string, DataRow>> listSubType) {
            string result = "";
            int k = 1;

            foreach (var s in listSubType) {
               result += s.FirstOrDefault()["ADJ_RATE"].AsPercent(0);

               if (k == listSubType.Count() - 1) {
                  result += "及";
               } else {
                  result += "、";
               }

               k++;
            }

            result = result.TrimEnd('、');
            return result;
         }

         protected virtual void SetRtfDescText(string meetingDate, string chairman, string attend, string caseDesc, string descStr) {
            M40030Word m40030 = new M40030Word(meetingDate, chairman, attend, caseDesc, descStr);
            //Options.MailMerge 要用List 才會有作用
            List<M40030Word> listM40030 = new List<M40030Word>();
            listM40030.Add(m40030);

            //直接replace word 上面的字
            DocSer.Options.MailMerge.DataSource = listM40030;
            DocSer.Options.MailMerge.ViewMergedData = true;
         }

         protected virtual string GenDescStr() {
            string tempStr = "", result = "";
            string ymdFormat = "{0}年{1}月{2}日";
            string dataYMD = Dt.Rows[0]["DATA_YMD"].AsDateTime("yyyyMMdd").AsTaiwanDateTime(ymdFormat, 3);
            DateTime implBeginYmd = Dt.Rows[0]["IMPL_BEGIN_YMD"].AsDateTime("yyyyMMdd");
            DateTime implEndYmd = Dt.Rows[0]["IMPL_END_YMD"].AsDateTime("yyyyMMdd");
            DateTime mocfDate = new MOCF().GetMaxOcfDate(implBeginYmd.ToString("yyyyMMdd"), implEndYmd.ToString("yyyyMMdd")).AsDateTime("yyyyMMdd");

            DataTable startDateEndDate = new MOCF().GetStartEndDate(implBeginYmd.ToString("yyyyMMdd"), implEndYmd.ToString("yyyyMMdd"));
            DateTime startDate = startDateEndDate.Rows[0]["LS_START_YMD"].AsDateTime("yyyyMMdd");
            DateTime endDate = startDateEndDate.Rows[0]["LS_END_YMD"].AsDateTime("yyyyMMdd");

            int diffDays = endDate.Subtract(startDate).Days;
            string year = startDate.AsTaiwanDateTime("{0}", 3);
            string startYmd = startDate.AsTaiwanDateTime("{1}月{2}日", 3);
            string endYmd = endDate.AsTaiwanDateTime("{1}月{2}日", 3);

            IEnumerable<IGrouping<string, DataRow>> listSubType = Dt.AsEnumerable().GroupBy(d => d.Field<string>("PROD_SUBTYPE"));
            string subTypeStr = GenProdSubtypeList(listSubType, "契約");
            string adjRate = GenAdjRate(listSubType);

            DataRow drTXF = Dt.AsEnumerable().Where(d => d.Field<string>("KIND_ID").AsString() == "TXF").FirstOrDefault();
            string mim = "", point = "", settlePrice = "", dbRate = "";

            if (drTXF != null) {
               mim = drTXF["M_IM"].AsDecimal().AsString("###,###");
               point = (mim.AsInt() / 200).AsString();

               settlePrice = drTXF["MGP1_CLOSE_PRICE"].AsDecimal().AsString("###,###");

               dbRate = settlePrice.AsInt() == 0 ? "0" :
                  (point.AsDouble() / settlePrice.AsDouble()).AsPercent(2);

            }

            //說明一
            tempStr = string.Format("\n 一、考量{0}年春節假期休市({1}至{2})長達{3}日，" +
                              "參酌國外主要交易所逢較長假期採行調高保證金之風控措施，援引過往春節假期採行調高保證金之作法，" +
                              "將於{0}年春節假期，調高{4}保證金。依本公司保證金調整作業規範，由督導結算業務主管召集業務相關部" +
                              "門主管會商決定是否調整。\n", year, startYmd, endYmd, diffDays.ToString(), subTypeStr);

            result = tempStr;

            //說明二
            tempStr = string.Format("二、有關調高{0}保證金，建議將結算保證金調高{1}，" +
                                    "維持保證金與原始保證金則依本公司訂定之成數加成計算，併同調高。" +
                                    "調高後之臺股期貨原始保證金能涵蓋之價格波動幅度為{2}點（{3}/200），" +
                                    "以{4}結算價{5}試算，可涵蓋{6}價格波動（{2}/{5}＝{6}）。\n",
                                    subTypeStr, adjRate, point, mim, dataYMD, settlePrice, dbRate);
            result += tempStr;

            //說明三
            tempStr = string.Format("三、本次保證金調整實施期間自{0}一般交易時段結束後，" +
                                    "預計至{1}一般交易時段結束止。本公司於{2}，" +
                                    "另行公告前揭契約於{1}一般交易時段結束後起之保證金適用金額。\n",
                                    implBeginYmd.AsTaiwanDateTime(ymdFormat, 3), implEndYmd.AsTaiwanDateTime(ymdFormat, 3),
                                    mocfDate.AsTaiwanDateTime(ymdFormat, 3));
            result += tempStr;

            //說明四
            tempStr = "四、本次保證金倘經調整，其金額變動如下：\n";

            result += tempStr;

            return result;
         }

         public virtual void ErrorHandle(Exception ex, ReturnMessageClass msg) {
            WriteLog(ex.ToString(), "Info", "Z");
            msg.Status = ResultStatus.Fail;
            msg.ReturnMessage = MessageDisplay.MSG_IMPORT_FAIL;
         }

         /// <summary>
         /// 設定英文和數字的字型
         /// </summary>
         private void SetNumberAndEnglishFontName(Document doc, DocumentRange docRange) {
            CharacterProperties cp = doc.BeginUpdateCharacters(docRange);
            cp.FontName = "Times New Roman";
            doc.EndUpdateCharacters(cp);
         }

         /// <summary>
         /// 將整份文件的英文和數字的字型設成某個字型
         /// </summary>
         private void SetAllNumberAndEnglishFont(Document doc) {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"[A-Za-z0-9\)\(\.\,]+");
            DocumentRange[] foundNumberAndEnglish = doc.FindAll(regex);

            foreach (DocumentRange r in foundNumberAndEnglish) {
               SetNumberAndEnglishFontName(doc, r);
            }
         }

         /// <summary>
         /// 錯誤寫log
         /// </summary>
         /// <param name="msg"></param>
         /// <param name="logType"></param>
         /// <param name="operationType"></param>
         public virtual void WriteLog(string msg, string logType = "Info", string operationType = "") {
            bool isNeedWriteFile = true;

            try {
               //ken,LOGF_KEY_DATA長度要取前100字元,但是logf.LOGF_KEY_DATA型態為VARCHAR2 (100 Byte),如果有中文會算2byte...先取前80吧
               new LOGF().Insert(GlobalInfo.USER_ID, ProgramId, msg.SubStr(0, 80), operationType);

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
                  string filepath = Path.Combine(Application.StartupPath, "Log", DateTime.Today.ToString("yyyyMM"));
                  Directory.CreateDirectory(filepath);
                  filepath = Path.Combine(filepath, filename);
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
      /// 長假調整 輸出xml
      /// </summary>
      private class ExportXml1B : ExportWord {
         public ExportXml1B(string txtdate, string adjtype, string programId) :
                     base(txtdate, adjtype, programId) {
            OswGrp = "%";
            CaseDescStr = "因應春節假期，擬調整本公司#kind_name_list#所有月份保證金金額案，謹提請討論。";
         }

         public override ReturnMessageClass Export() {
            ReturnMessageClass msg = new ReturnMessageClass();
            msg.Status = ResultStatus.Fail;

            try {
               return base.Export();

            } catch (Exception ex) {
               base.ErrorHandle(ex, msg);
               return msg;
            }
         }
      }


      private class M40030Word {
         public string MeetingDate { get; set; }
         public string MeetingAddress { get; set; }
         public string Chairman { get; set; }
         public string Attend { get; set; }
         public string CaseDesc { get; set; }

         public string Desc { get; set; }

         public M40030Word(string meetingdate, string chairman, string attend, string casedesc, string descstr, string meetingaddress = "研討室") {
            MeetingDate = meetingdate;
            MeetingAddress = meetingaddress;
            Chairman = chairman;
            Attend = attend;
            CaseDesc = casedesc;

            Desc = descstr;
         }
      }
   }
}