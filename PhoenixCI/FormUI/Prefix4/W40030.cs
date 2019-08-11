using BaseGround;
using BaseGround.Shared;
using BaseGround.Widget;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao;
using DevExpress.Office;
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

/// <summary>
/// 依照類別 產生 rtf 會議記錄, 選擇一般時, 多一份excel
/// Test Data 3B 20181228 / 1B 20190129 / 1E 20190129 / 0B 20190212
/// 0B	一般
/// 1B  長假調整
/// 1E	長假回調
/// 3B  股票
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
      /// 一般/股票, 長假調整, 長假回調, 股票
      /// 0B / 1B / 1E / 3B
      /// </summary>
      public string AdjType {
         get {
            return ddlAdjType.EditValue.AsString();
         }
      }
      #endregion

      public W40030(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtDate.DateTimeValue = DateTime.Now;

         foreach (CheckedListBoxItem c in MarketTimes.Items) {
            TextDateEdit control = (TextDateEdit)this.Controls.Find("txtDate" + c.Value.AsString() , true).FirstOrDefault();
            control.DateTimeValue = DateTime.Now;
         }

         //設定 下拉選單
         //List<LookupItem> lstType = new List<LookupItem>(){
         //                               new LookupItem() { ValueMember = "0B", DisplayMember = "一般"},
         //                               new LookupItem() { ValueMember = "1B", DisplayMember = "長假調整" },
         //                               new LookupItem() { ValueMember = "1E", DisplayMember = "長假回調" },
         //                               new LookupItem() { ValueMember = "3B", DisplayMember = "股票"}};
         DataTable dtType = new CODW().ListLookUpEdit("40030" , "MGD2_ADJ_TYPE");

         //List<LookupItem> dtETC = new List<LookupItem>(){
         //                               new LookupItem() { ValueMember = "1", DisplayMember = "有"},
         //                               new LookupItem() { ValueMember = "2", DisplayMember = "無" }};
         DataTable dtETC = new CODW().ListLookUpEdit("40030" , "40030_ETC_VSR");

         //設定下拉選單
         ddlAdjType.SetDataTable(dtType , "CODW_ID" , "CODW_DESC" , TextEditStyles.DisableTextEditor , null);
         ddlAdjType.ItemIndex = 0; //0B
         ddlAdjType.Properties.DropDownRows = dtType.Rows.Count;
         ETCSelect.SetDataTable(dtETC, "CODW_ID" , "CODW_DESC" , TextEditStyles.DisableTextEditor , null);
         ETCSelect.ItemIndex = 0; //1
         ETCSelect.Properties.DropDownRows = dtETC.Rows.Count;
         MarketTimes.SetItemChecked(0 , true);
#if DEBUG
         txtDate.DateTimeValue = "2018/12/28".AsDateTime("yyyy/MM/dd");
         ddlAdjType.ItemIndex = 0;
#endif

         ExportShow.Hide();

        }

        protected override ResultStatus AfterOpen()
        {
            _ToolBtnExport.Enabled = true;
            return base.AfterOpen();
        }

        protected override ResultStatus Export() {
         ExportShow.Text = "轉檔中...";
         ExportShow.Show();
         try {

            //當選擇一般情況時, 會有check 選項, 造成getDate 不同
            List<CheckedItem> checkedItems = new List<CheckedItem>();
            foreach (CheckedListBoxItem c in MarketTimes.CheckedItems) {
               TextDateEdit control = (TextDateEdit)this.Controls.Find("txtDate" + c.Value.AsString() , true).FirstOrDefault();

               checkedItems.Add(
                  new CheckedItem {
                     CheckedValue = c.Value.AsInt() ,
                     CheckedDate = control.DateTimeValue ,
                     ETCSelected = ETCSelect.EditValue.AsString()
                  });
            }

            //1. 從DB SP SP_H_TXN_40030_MG_DETL 取資料
            object[] args = { TxtDate , AdjType , _ProgramID , checkedItems };
            IExport40xxxData xmlData = CreateXmlData(GetType() , "ExportWord" + AdjType , args);
            ReturnMessageClass msg = xmlData.GetData();

            //1.1 無資料時不產檔
            if (msg.Status != ResultStatus.Success) {
               ExportShow.Text = MessageDisplay.MSG_IMPORT_FAIL;

               if (AdjType == "0B")
                  MessageDisplay.Info($"{txtDate.DateTimeValue.ToShortDateString()},{_ProgramID}-{ddlAdjType.Properties.GetDisplayText(AdjType)},{MessageDisplay.MSG_NO_DATA}");
               else
                  MessageDisplay.Info($"{msg.ReturnMessage},{MessageDisplay.MSG_NO_DATA}");

               return msg.Status;
            }

            // 輸出 rtf
            msg = xmlData.Export();

            if (msg.Status == ResultStatus.Fail) {
               ExportShow.Text = MessageDisplay.MSG_IMPORT_FAIL;
               MessageDisplay.Info(MessageDisplay.MSG_IMPORT_FAIL);
               return msg.Status;
            } else if (msg.Status == ResultStatus.FailButNext) {
               ExportShow.Hide();
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

      /// <summary>
      /// 下拉選單事件, 選擇一般時出現check 選項
      /// </summary>
      private void ddlAdjType_EditValueChanged(object sender , EventArgs e) {

         if (ddlAdjType.EditValue.AsString() == "0B") {
            groupBox1.Visible = true;
            lblDate.Visible = false;
            txtDate.Visible = false;
         } else {
            groupBox1.Visible = false;
            lblDate.Visible = true;
            txtDate.Visible = true;
         }

      }

      /// <summary>
      /// 主要 export 其他直接複寫其function
      /// </summary>
      private class ExportWord : IExport40xxxData {
         protected RPTF DaoRptf { get; }
         protected D40030 Dao40030 { get; }
         protected virtual string TxtDate { get; }
         protected virtual string AdjType { get; }
         protected string ProgramId { get; }

         protected virtual DataTable Dt { get; set; }
         protected virtual string AsAdjType {
            get {
               return AdjType.SubStr(0 , 1);
            }
         }
         protected virtual string FilePath { get; set; }
         protected virtual RichEditDocumentServer DocSer { get; set; }
         protected virtual Document Doc { get; set; }

         protected virtual string MeetingLogFileName { get; set; }
         protected virtual string AgendaFileName { get; set; }
         protected virtual DataTable DtAgenda { get; set; }
         protected virtual DataTable DtMinutes { get; set; }
         protected virtual string DescStr { get; set; }

         protected virtual string OswGrp { get; set; }
         protected virtual string CaseDescStr { get; set; }
         protected virtual Table WordTable { get; set; }
         protected virtual TableCell WordTableCell { get; set; }
         protected virtual ParagraphProperties ParagraphProps { get; set; }

         protected virtual CharacterProperties CharacterProps { get; set; }
         protected virtual List<CheckedItem> CheckedItems { get; set; }

         public ExportWord(string txtdate , string adjtype , string programId , List<CheckedItem> checkeditems) {
            DaoRptf = new RPTF();
            Dao40030 = new D40030();
            TxtDate = txtdate;
            AdjType = adjtype;
            ProgramId = programId;

            CheckedItems = checkeditems;
            MeetingLogFileName = "40030_MeetingLog";
            AgendaFileName = "40030_Agenda";
         }

         public virtual ReturnMessageClass GetData() {
            ReturnMessageClass msg = new ReturnMessageClass(MessageDisplay.MSG_NO_DATA);
            msg.Status = ResultStatus.Fail;

            Dt = Dao40030.GetData(TxtDate , OswGrp , AsAdjType , AdjType.SubStr(1 , 1));

            if (Dt != null) {
               if (Dt.Rows.Count > 0) {
                  msg.Status = ResultStatus.Success;
               }
            }

            return msg;
         }

         /// <summary>
         /// 取得議程資訊及開會資訊
         /// </summary>
         protected virtual void GetRPTF() {
            DtAgenda = DaoRptf.ListData("49074" , "49074" , "agenda");
            DtMinutes = DaoRptf.ListData("49074" , "49074" , "minutes");
         }

         public virtual ReturnMessageClass Export() {
            ReturnMessageClass msg = new ReturnMessageClass();
            msg.Status = ResultStatus.Fail;

            try {
               FilePath = PbFunc.wf_copy_file(ProgramId , MeetingLogFileName);

               //取得會議紀錄 / 議程資訊
               GetRPTF();

               #region 會議記錄
               OpenFile();

               //出席者 / 案由
               SetHead(DtMinutes);

               #region 表格
               Doc.BeginUpdate();
               Doc.AppendText(Environment.NewLine);

               DrowTable(Dt);

               #endregion

               #region 表尾 決議 

               SetSubjectText("決　　議：");

               SetInnerText("一、請於市場公告函，提醒期貨商得依客戶風險狀況加收保證金並通知交易人多加注意國際行情變化及持有部位狀況，適時繳足保證金。");
               SetInnerText("二、請提醒期貨商因應春節長假，應加強交易人部位風險管理並預作風險控管準備，維護交易人權益。");
               SetInnerText("三、餘照案通過。");


               SetSubjectText("貳、臨時動議：無");
               SetSubjectText("參、散　　會：下午5時30分");
               #endregion

               SetAllNumberAndEnglishFont(Doc);//設定英數字體

               Doc.EndUpdate();
               DocSer.SaveDocument(FilePath , DocumentFormat.Rtf);
               DocSer.Dispose();
#if DEBUG
               System.Diagnostics.Process.Start(FilePath);
#endif
               #endregion 會議記錄

               #region 議程
               FilePath = PbFunc.wf_copy_file(ProgramId , AgendaFileName);

               OpenFile();

               //出席者 / 案由
               SetHead(DtAgenda);

               #region 表格
               Doc.BeginUpdate();
               Doc.AppendText(Environment.NewLine);

               DrowTable(Dt);

               #endregion

               #region 表尾 

               SetSubjectText("貳、臨時動議：");
               SetSubjectText("參、散　　會：");
               #endregion

               SetAllNumberAndEnglishFont(Doc);//設定英數字體

               Doc.EndUpdate();
               DocSer.SaveDocument(FilePath , DocumentFormat.Rtf);
               DocSer.Dispose();

#if DEBUG
               System.Diagnostics.Process.Start(FilePath);
#endif
               #endregion 議程

               msg.Status = ResultStatus.Success;
               return msg;

            } catch (Exception ex) {
               ErrorHandle(ex , msg);
               return msg;
            }
         }

         /// <summary>
         /// 開檔
         /// </summary>
         protected virtual void OpenFile() {
            DocSer = new RichEditDocumentServer();
            DocSer.LoadDocument(FilePath);
            Doc = DocSer.Document;
         }

         /// <summary>
         /// 組說明文字 以 "、" 連接
         /// </summary>
         /// <param name="strList"></param>
         /// <returns></returns>
         protected virtual string GenArrayTxt(List<string> strList) {
            string result = "";
            int k = 0;

            foreach (string s in strList) {
               result += s;
               if (k < strList.Count()) {
                  if (k != (strList.Count() - 2)) {
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
         protected virtual string GenProdName(DataTable dt , string contract = "") {
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

         /// <summary>
         /// 組出席者
         /// </summary>
         /// <param name="dtAttend">出席者資料</param>
         /// <returns></returns>
         protected virtual string GenAttend(DataTable dtAttend) {
            string result = "";
            int k = 0;

            dtAttend = dtAttend.Select("RPTF_SEQ_NO <> 1").CopyToDataTable();

            foreach (DataRow dr in dtAttend.Rows) {

               result += dr["RPTF_TEXT"].AsString() + "\n\t";

               k++;
            }
            result = result.TrimEnd('\n' , '\t');

            return result;
         }

         /// <summary>
         /// 組開會時間
         /// </summary>
         /// <returns></returns>
         protected virtual string GenMeetingDate() {
            DateTime dtNow = DateTime.Now;

            string date = dtNow.AsTaiwanDateTime("{0}年{1}月{2}日" , 3);
            string week = System.Globalization.DateTimeFormatInfo.GetInstance(
                           new System.Globalization.CultureInfo("zh-CHT")).DayNames[(byte)dtNow.DayOfWeek];

            string result = $"{date}({week})下午5時10分";

            return result;
         }

         /// <summary>
         /// 設定開頭文字
         /// </summary>
         /// <param name="dtAttend"></param>
         protected virtual void SetHead(DataTable dtAttend) {
            string chairman = DtMinutes.Rows[0]["RPTF_TEXT"].AsString();

            SetSubjectText("案　　由： ");

            SetInnerText(CaseDescStr.Replace("#kind_name_list#" , GenProdName(Dt , "契約")) , false , 2.75f , 2.75f);

            SetDescStr();

            SetRtfDescText(GenMeetingDate() , chairman , GenAttend(dtAttend));
         }

         protected virtual string GenProdSubtypeList(IEnumerable<IGrouping<string , DataRow>> listSubType , string contract = "") {
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

         protected virtual string GenAdjRate(IEnumerable<IGrouping<string , DataRow>> listSubType) {
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

         /// <summary>
         /// 替換 rtf 開頭文字
         /// </summary>
         /// <param name="meetingDate"></param>
         /// <param name="chairman"></param>
         /// <param name="attend"></param>
         protected virtual void SetRtfDescText(string meetingDate , string chairman , string attend) {
            M40030Word m40030 = new M40030Word(meetingDate , chairman , attend);
            //Options.MailMerge 要用List 才會有作用
            List<M40030Word> listM40030 = new List<M40030Word>();
            listM40030.Add(m40030);

            //直接replace word 上面的字
            DocSer.Options.MailMerge.DataSource = listM40030;
            DocSer.Options.MailMerge.ViewMergedData = true;
         }

         /// <summary>
         /// 說明文
         /// </summary>
         protected virtual void SetDescStr() {
            string tempStr = "";
            string ymdFormat = "{0}年{1}月{2}日";
            string dataYMD = Dt.Rows[0]["DATA_YMD"].AsDateTime("yyyyMMdd").AsTaiwanDateTime(ymdFormat , 3);
            DateTime implBeginYmd = Dt.Rows[0]["IMPL_BEGIN_YMD"].AsDateTime("yyyyMMdd");
            DateTime implEndYmd = Dt.Rows[0]["IMPL_END_YMD"].AsDateTime("yyyyMMdd");
            DateTime mocfDate = new MOCF().GetMaxOcfDate(implBeginYmd.ToString("yyyyMMdd") , implEndYmd.ToString("yyyyMMdd")).AsDateTime("yyyyMMdd");

            DataTable startDateEndDate = new MOCF().GetStartEndDate(implBeginYmd.ToString("yyyyMMdd") , implEndYmd.AddDays(-2).ToString("yyyyMMdd"));
            DateTime startDate = startDateEndDate.Rows[0]["LS_START_YMD"].AsDateTime("yyyyMMdd");
            DateTime endDate = startDateEndDate.Rows[0]["LS_END_YMD"].AsDateTime("yyyyMMdd");

            int diffDays = endDate.Subtract(startDate).Days + 1;
            string year = startDate.AsTaiwanDateTime("{0}" , 3);
            string startYmd = startDate.AsTaiwanDateTime("{1}月{2}日" , 3);
            string endYmd = endDate.AsTaiwanDateTime("{1}月{2}日" , 3);

            IEnumerable<IGrouping<string , DataRow>> listSubType = Dt.AsEnumerable().GroupBy(d => d.Field<string>("PROD_SUBTYPE"));
            string subTypeStr = GenProdSubtypeList(listSubType , "契約");
            string adjRate = GenAdjRate(listSubType);

            DataRow drTXF = Dt.AsEnumerable().Where(d => d.Field<string>("KIND_ID").AsString() == "TXF").FirstOrDefault();
            string mim = "", point = "", settlePrice = "", dbRate = "";

            if (drTXF != null) {
               mim = drTXF["M_IM"].AsDecimal().ToString("###,###");
               point = (drTXF["M_IM"].AsInt() / 200).ToString("###,###");

               settlePrice = drTXF["MGP1_CLOSE_PRICE"].AsDecimal().ToString("###,###");

               dbRate = drTXF["MGP1_CLOSE_PRICE"].AsDouble() == 0 ? "0" :
                  (point.AsDouble() / drTXF["MGP1_CLOSE_PRICE"].AsDouble()).AsPercent(2);

            }

            SetSubjectText("說　　明： ");

            //說明一
            tempStr = string.Format("一、考量{0}年春節假期休市({1}至{2})長達{3}日，" +
                                    "參酌國外主要交易所逢較長假期採行調高保證金之風控措施，援引過往春節假期採行調高保證金之作法，" +
                                    "將於{0}年春節假期，調高{4}保證金。依本公司保證金調整作業規範，由督導結算業務主管召集業務相關部" +
                                    "門主管會商決定是否調整。" , year , startYmd , endYmd , diffDays.ToString() , subTypeStr);
            SetInnerText(tempStr);

            //說明二
            tempStr = string.Format("二、有關調高{0}保證金，建議將結算保證金調高{1}，" +
                                    "維持保證金與原始保證金則依本公司訂定之成數加成計算，併同調高。" +
                                    "調高後之臺股期貨原始保證金能涵蓋之價格波動幅度為{2}點（{3}/200），" +
                                    "以{4}結算價{5}試算，可涵蓋{6}價格波動（{2}/{5}＝{6}）。" ,
                                    subTypeStr , adjRate , point , mim , dataYMD , settlePrice , dbRate);
            SetInnerText(tempStr);

            //說明三
            tempStr = string.Format("三、本次保證金調整實施期間自{0}一般交易時段結束後，" +
                                    "預計至{1}一般交易時段結束止。本公司於{2}，" +
                                    "另行公告前揭契約於{1}一般交易時段結束後起之保證金適用金額。" ,
                                    implBeginYmd.AsTaiwanDateTime(ymdFormat , 3) , implEndYmd.AsTaiwanDateTime(ymdFormat , 3) ,
                                    mocfDate.AsTaiwanDateTime(ymdFormat , 3));
            SetInnerText(tempStr);

            //說明四
            SetInnerText("四、本次保證金倘經調整，其金額變動如下：");
         }

         /// <summary>
         /// Create table style
         /// </summary>
         /// <param name="doc"></param>
         /// <param name="rowCount">列數</param>
         /// <param name="colCount">欄位數</param>
         protected virtual void CreateTable(Document doc , int rowCount , int colCount) {

            WordTable = doc.Tables.Create(doc.Range.End , rowCount , colCount);
            WordTable.TableAlignment = TableRowAlignment.Right;
            WordTable.PreferredWidthType = WidthType.Fixed;
            WordTable.PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(14f);
            ParagraphProps = doc.BeginUpdateParagraphs(WordTable.Range);

            // 預設Table內容都全部置中
            ParagraphProps = doc.BeginUpdateParagraphs(WordTable.Range);
            ParagraphProps.Alignment = ParagraphAlignment.Center;
            ParagraphProps.LineSpacingType = ParagraphLineSpacing.Single;
            ParagraphProps.SpacingBefore = 0;
            doc.EndUpdateParagraphs(ParagraphProps);

            // 預設Table內的字體
            CharacterProperties tableProp = doc.BeginUpdateCharacters(WordTable.Range);
            tableProp.FontName = "標楷體";
            tableProp.FontSize = 11;
            doc.EndUpdateCharacters(tableProp);

           // 垂直置中
            WordTable.ForEachCell((c , i , j) => {
               c.VerticalAlignment = TableCellVerticalAlignment.Center;
            });
         }

         /// <summary>
         /// 設定欄位名稱
         /// </summary>
         protected virtual void SetTableColTitle(string prodName , string[] colTitle , string afterAdjustTitle , string beforeAdjustTitle) {
            SetTableStr(0 , 0 , prodName);
            WordTableCell.PreferredWidthType = WidthType.Fixed;
            WordTableCell.PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(1.5f);
            WordTableCell.VerticalAlignment = TableCellVerticalAlignment.Center;
            WordTable.MergeCells(WordTableCell , WordTable[1 , 0]);

            SetTableStr(0 , 1 , afterAdjustTitle);
            WordTable.MergeCells(WordTableCell , WordTable[0 , 3]);
            WordTable[0, 1].PreferredWidthType = WidthType.Fixed;
            WordTable[0, 1].PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(5f);

            SetTableStr(0 , 2 , beforeAdjustTitle);
            WordTable.MergeCells(WordTableCell , WordTable[0 , 4]);
            WordTable[0, 2].PreferredWidthType = WidthType.Fixed;
            WordTable[0, 2].PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(5f);

            SetTableTitle(colTitle , 1);
         }

         /// <summary>
         /// 設定欄位名稱
         /// </summary>
         protected virtual void SetTableColValue(I40030AmtProdType iAmtProd , DataRow dr) {

            int k = 1;

            foreach (string rowName in iAmtProd.RowName) {

               TableRow tableRow = WordTable.Rows.Append();

               WordTableCell = tableRow.FirstCell;
               Doc.InsertSingleLineText(WordTableCell.Range.Start , rowName);

               //特殊處理, 選擇權時有AB值, k=1 跑保證金或A值, k>1 跑B值
               string[] colNameList = k == 1 ? iAmtProd.DbColName : iAmtProd.DbColNameB;
               string numberFormat = k == 1 ? iAmtProd.NumberFormat : iAmtProd.NumberFormatB;

               int i = 1;
               foreach (string col in colNameList) {
                  Doc.InsertSingleLineText(WordTable[tableRow.Index , i].Range.Start , !decimal.Equals(dr[col].AsDecimal() , 0) ?
                     dr[col].AsDecimal().ToString(numberFormat) : string.Empty);

                  i++;
               }

               k++;
            }

         }

         /// <summary>
         /// 設定表格內文
         /// </summary>
         /// <param name="rowIndex">第幾列</param>
         /// <param name="colIndex">第幾行</param>
         /// <param name="str">寫入文字</param>
         protected virtual void SetTableStr(int rowIndex , int colIndex , string str) {
            WordTableCell = WordTable[rowIndex , colIndex];
            Doc.InsertSingleLineText(WordTableCell.Range.Start , str);
         }

            /// <summary>
            /// 設定表格內文(\r\n)
            /// </summary>
            /// <param name="rowIndex">第幾列</param>
            /// <param name="colIndex">第幾行</param>
            /// <param name="str">寫入文字</param>
            protected virtual void SetTableRNStr(int rowIndex, int colIndex, string str)
            {
                WordTableCell = WordTable[rowIndex, colIndex];
                Doc.InsertText(WordTableCell.Range.Start, str);
            }

            /// <summary>
            /// 表頭
            /// </summary>
            /// <param name="strList"></param>
            /// <param name="rowIndex"></param>
            protected virtual void SetTableTitle(string[] strList , int rowIndex) {

            int k = 0;
            foreach (string str in strList) {
               WordTableCell = WordTable[rowIndex , k + 1];
               WordTableCell.PreferredWidthType = WidthType.Fixed;
               WordTableCell.PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(2f);

               Doc.InsertText(WordTableCell.Range.Start , strList[k]);

               WordTableCell = WordTable[rowIndex , k + 4];
               WordTableCell.PreferredWidthType = WidthType.Fixed;
               WordTableCell.PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(2f);

               Doc.InsertText(WordTableCell.Range.Start , strList[k]);
               k++;
            }
         }

         /// <summary>
         /// 設定內文
         /// </summary>
         /// <param name="str"></param>
         /// <param name="hasFirstIndent">第一行是否凸排</param>
         /// <param name="leftIndent">左縮排</param>
         /// <param name="fitstLineIndent">第一行位移點數</param>
         protected virtual void SetInnerText(string str , bool hasFirstIndent = true , float leftIndent = 2.98f , float fitstLineIndent = 1.18f ,
                                                bool hasNewLine = true) {
            if (hasNewLine) Doc.AppendText(Environment.NewLine);
            Doc.AppendText(str);

            ParagraphProps = Doc.BeginUpdateParagraphs(Doc.Paragraphs.Last().Range);
            ParagraphProps.Alignment = ParagraphAlignment.Left;
            ParagraphProps.LeftIndent = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(leftIndent);
            ParagraphProps.LineSpacing = DevExpress.Office.Utils.Units.PointsToDocuments(27);

            if (hasFirstIndent) {
               //設定凸排
               ParagraphProps.FirstLineIndentType = ParagraphFirstLineIndent.Hanging;
               ParagraphProps.FirstLineIndent = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(fitstLineIndent);
            }

            Doc.EndUpdateParagraphs(ParagraphProps);

            CharacterProps = Doc.BeginUpdateCharacters(Doc.Paragraphs.Last().Range);
            CharacterProps.FontSize = 14;
            CharacterProps.Bold = false;
            CharacterProps.FontName = "標楷體";
            Doc.EndUpdateCharacters(CharacterProps);
         }

         /// <summary>
         /// 設定標題
         /// </summary>
         /// <param name="str">標題文字</param>
         protected virtual void SetSubjectText(string str , bool hasBold = true , bool hasNewLine = true) {
            if (hasNewLine) Doc.AppendText(Environment.NewLine);
            Doc.AppendText(str);

            //段落設定
            ParagraphProps = Doc.BeginUpdateParagraphs(Doc.Paragraphs.Last().Range);
            ParagraphProps.Alignment = ParagraphAlignment.Left;//靠左對其
            ParagraphProps.LeftIndent = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(0);//縮排
            ParagraphProps.LineSpacing = DevExpress.Office.Utils.Units.PointsToDocuments(27);
            ParagraphProps.FirstLineIndentType = ParagraphFirstLineIndent.None;
            Doc.EndUpdateParagraphs(ParagraphProps);

            //文章設定
            CharacterProps = Doc.BeginUpdateCharacters(Doc.Paragraphs.Last().Range);
            CharacterProps.FontSize = 14;
            CharacterProps.FontName = "標楷體";
            if (hasBold)
               CharacterProps.Bold = true;
            Doc.EndUpdateCharacters(CharacterProps);
         }

         /// <summary>
         /// 設定表格上方文字
         /// </summary>
         /// <param name="comment">文字</param>
         /// <param name="lineSpacing"></param>
         /// <param name="paragraphAlignment"></param>
         /// <param name="fontSize"></param>
         /// <param name="fontName"></param>
         protected virtual void SetComment(string comment , int lineSpacing = 27 , ParagraphAlignment paragraphAlignment = ParagraphAlignment.Right ,
                                       int fontSize = 11 , string fontName = "標楷體") {

            Doc.AppendText(comment);
            ParagraphProps = Doc.BeginUpdateParagraphs(Doc.Paragraphs.Last().Range);
            ParagraphProps.Alignment = paragraphAlignment;
            ParagraphProps.LineSpacing = DevExpress.Office.Utils.Units.PointsToDocuments(lineSpacing);
            Doc.EndUpdateParagraphs(ParagraphProps);

            CharacterProps = Doc.BeginUpdateCharacters(Doc.Paragraphs.Last().Range);
            CharacterProps.FontSize = fontSize;
            CharacterProps.FontName = fontName;
            Doc.EndUpdateCharacters(CharacterProps);
         }

         /// <summary>
         /// 畫表格
         /// </summary>
         /// <param name="dataTable"></param>
         protected virtual void DrowTable(DataTable dataTable) {
            Doc.AppendText(Environment.NewLine);

            foreach (DataRow dr in dataTable.Rows) {

               string amtType = dr["AMT_TYPE"].AsString();
               string prodType = dr["PROD_TYPE"].AsString();
               object[] args = new object[] { dr };
               I40030AmtProdType iAmtProdType = CreateI40030AmtProdType(GetType() , "AmtProdType40030" + amtType + prodType , args);

               SetComment(iAmtProdType.CurrencyName);

               CreateTable(Doc , 2 , 7);

               SetTableColTitle(iAmtProdType.ProdName , iAmtProdType.TableTitle , iAmtProdType.AfterAdjustTitle , iAmtProdType.BeforeAdjustTitle);

               SetTableColValue(iAmtProdType , dr);

               Doc.AppendText(Environment.NewLine);
            }

         }

         /// <summary>
         /// 依照 Amt Prod Type 設定其參數
         /// </summary>
         /// <param name="type"></param>
         /// <param name="name"></param>
         /// <param name="args"></param>
         /// <returns></returns>
         protected I40030AmtProdType CreateI40030AmtProdType(Type type , string name , object[] args = null) {
            string AssemblyName = type.Namespace.Split('.')[0];//最後compile出來的dll名稱
            string className = type.Namespace + "." + type.ReflectedType.Name + "+" + name;//完整的class路徑(注意,內部的class執行時其fullName是用+號連結起來)
            return (I40030AmtProdType)Assembly.Load(AssemblyName).CreateInstance(className , true , BindingFlags.CreateInstance , null , args , null , null);
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
         /// 設定英文和數字的字型
         /// </summary>
         protected virtual void SetNumberAndEnglishFontName(Document doc , DocumentRange docRange) {
            CharacterProperties cp = doc.BeginUpdateCharacters(docRange);
            cp.FontName = "Times New Roman";
            doc.EndUpdateCharacters(cp);
         }

         /// <summary>
         /// 將整份文件的英文和數字的字型設成某個字型
         /// </summary>
         protected virtual void SetAllNumberAndEnglishFont(Document doc) {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"[A-Za-z0-9\)\(\.\,\%\-]+");
            DocumentRange[] foundNumberAndEnglish = doc.FindAll(regex);

            foreach (DocumentRange r in foundNumberAndEnglish) {
               SetNumberAndEnglishFontName(doc , r);
            }
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
      /// 長假調整 輸出rtf
      /// </summary>
      private class ExportWord1B : ExportWord {
         public ExportWord1B(string txtdate , string adjtype , string programId , List<CheckedItem> checkeditems) :
                     base(txtdate , adjtype , programId , checkeditems) {
            OswGrp = "%";
            CaseDescStr = "因應春節假期，擬調整本公司#kind_name_list#所有月份保證金金額案，謹提請討論。";//案由
         }

         public override ReturnMessageClass Export() {
            ReturnMessageClass msg = new ReturnMessageClass();
            msg.Status = ResultStatus.Fail;

            try {
               //檢查批次作業
               string check130Wf = PbFunc.f_chk_130_wf("40030" , TxtDate.AsDateTime("yyyyMMdd") , OswGrp);
               if (check130Wf != "") {
                  DialogResult result = MessageDisplay.Choose($"{TxtDate}-{check130Wf}，是否要繼續?");
                  if (result == DialogResult.No) {
                     msg.Status = ResultStatus.FailButNext;
                     return msg;
                  }
               }

               return base.Export();

            } catch (Exception ex) {
               base.ErrorHandle(ex , msg);
               return msg;
            }
         }
      }

      /// <summary>
      /// 長假回調 輸出rtf
      /// </summary>
      private class ExportWord1E : ExportWord {
         public ExportWord1E(string txtdate , string adjtype , string programId , List<CheckedItem> checkeditems) :
                     base(txtdate , adjtype , programId , checkeditems) {
            OswGrp = "%";
            CaseDescStr = "因應春節假期結束，擬回調本公司#kind_name_list#所有月份保證金金額案，謹提請討論。";//案由
         }

         public override ReturnMessageClass Export() {
            ReturnMessageClass msg = new ReturnMessageClass();
            msg.Status = ResultStatus.Fail;

            try {
               FilePath = PbFunc.wf_copy_file(ProgramId , MeetingLogFileName);

               //取得會議紀錄 / 議程資訊
               GetRPTF();

               #region 會議記錄
               OpenFile();

               //表頭 出席者 / 案由

               SetHead(DtMinutes);

               #region 表格
               Doc.BeginUpdate();
               Doc.AppendText(Environment.NewLine);

               DrowTable(Dt);

               #endregion

               #region 表尾 決議 

               base.SetSubjectText("決　　議：");

               string implDate = Dt.Rows[0]["impl_begin_ymd"].AsString();
               string beginDate = Dt.Rows[0]["issue_begin_ymd"].AsString();
               string mocfDate = new MOCF().GetMaxOcfDate(implDate , beginDate).AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日" , 3);
               string resolutionStr = string.Format("照案通過，於{0}公告回調金額，並自{1}一般交易時段結束後實施。" ,
                                                      mocfDate , beginDate.AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日" , 3));

               base.SetInnerText(resolutionStr , false , 2.75f);
               base.SetSubjectText("貳、臨時動議：無");
               base.SetSubjectText("參、散　　會：下午5時30分");
               #endregion

               base.SetAllNumberAndEnglishFont(Doc);//設定英數字體

               Doc.EndUpdate();
               DocSer.SaveDocument(FilePath , DocumentFormat.Rtf);
               DocSer.Dispose();
#if DEBUG
               System.Diagnostics.Process.Start(FilePath);
#endif
               #endregion 會議記錄

               #region 議程
               FilePath = PbFunc.wf_copy_file(ProgramId , AgendaFileName);

               OpenFile();

               //表頭 出席者 / 案由
               SetHead(DtAgenda);

               #region 表格
               Doc.BeginUpdate();
               Doc.AppendText(Environment.NewLine);

               DrowTable(Dt);

               #endregion

               #region 表尾 

               base.SetSubjectText("貳、臨時動議：");
               base.SetSubjectText("參、散　　會：");
               #endregion

               base.SetAllNumberAndEnglishFont(Doc);//設定英數字體

               Doc.EndUpdate();
               DocSer.SaveDocument(FilePath , DocumentFormat.Rtf);
               DocSer.Dispose();
#if DEBUG
               System.Diagnostics.Process.Start(FilePath);
#endif
               #endregion 議程

               msg.Status = ResultStatus.Success;
               return msg;

            } catch (Exception ex) {
               base.ErrorHandle(ex , msg);
               return msg;
            }
         }

         protected override void SetDescStr() {
            string tempStr = "";

            IEnumerable<IGrouping<string , DataRow>> listSubType = Dt.AsEnumerable().GroupBy(d => d.Field<string>("PROD_SUBTYPE"));
            string subTypeStr = base.GenProdSubtypeList(listSubType , "契約");

            SetSubjectText("說　　明： ");

            //說明一
            tempStr = string.Format("一、本公司於春節假期調高{0}保證金，因應春節假期結束，擬回調前揭契約保證金。" +
                                       "依本公司保證金調整作業規範，由督導結算業務主管召集業務相關部門主管會商決定是否調整。" ,
                                       subTypeStr);
            base.SetInnerText(tempStr);

            //說明二
            base.SetInnerText("二、本次保證金倘經調整，其金額變動如下：");
         }
      }

      /// <summary>
      /// 股票 輸出rtf
      /// </summary>
      private class ExportWord3B : ExportWord {
         protected List<string> allKindNameList { get; set; }

         public ExportWord3B(string txtdate , string adjtype , string programId , List<CheckedItem> checkeditems) :
                     base(txtdate , adjtype , programId , checkeditems) {
            OswGrp = "%";
            CaseDescStr = "檢陳本公司#kind_name_list#保證金調整案，謹提請討論。";//案由
         }

         public override ReturnMessageClass Export() {
            ReturnMessageClass msg = new ReturnMessageClass();
            msg.Status = ResultStatus.Fail;

            try {
               FilePath = PbFunc.wf_copy_file(ProgramId , MeetingLogFileName);

               //取得會議紀錄 / 議程資訊
               GetRPTF();

               #region 會議記錄
               OpenFile();

               //表頭 出席者 / 案由
               SetHead(DtMinutes);

               #region 表格 說明四
               Doc.BeginUpdate();
               Doc.AppendText(Environment.NewLine);

               DrowTable(Dt);
               #endregion

               #region 表尾 決議 

               base.SetSubjectText("決　　議：");
               string resolutionStr = string.Format("經考量市場風險及保證金保守穩健原則，調整{0}保證金適用比例調整如說明四。" , GenArrayTxt(allKindNameList));

               base.SetInnerText(resolutionStr , false);
               base.SetSubjectText("貳、臨時動議：無");
               base.SetSubjectText("參、散　　會：下午5時30分");
               #endregion

               base.SetAllNumberAndEnglishFont(Doc);//設定英數字體

               Doc.EndUpdate();
               DocSer.SaveDocument(FilePath , DocumentFormat.Rtf);
               DocSer.Dispose();
#if DEBUG
               System.Diagnostics.Process.Start(FilePath);
#endif
               #endregion 會議記錄

               #region 會議議程
               FilePath = PbFunc.wf_copy_file(ProgramId , AgendaFileName);

               OpenFile();

               //表頭 出席者 / 案由
               SetHead(DtAgenda);

               #region 表格
               Doc.BeginUpdate();
               Doc.AppendText(Environment.NewLine);

               DrowTable(Dt);

               #endregion

               #region 表尾 決議 

               base.SetSubjectText("貳、臨時動議：");
               base.SetSubjectText("參、散　　會：");
               #endregion

               base.SetAllNumberAndEnglishFont(Doc);//設定英數字體

               Doc.EndUpdate();
               DocSer.SaveDocument(FilePath , DocumentFormat.Rtf);
               DocSer.Dispose();
#if DEBUG
               System.Diagnostics.Process.Start(FilePath);
#endif
               #endregion 會議議程

               msg.Status = ResultStatus.Success;
               return msg;

            } catch (Exception ex) {
               base.ErrorHandle(ex , msg);
               return msg;
            }
         }

         protected override string GenProdName(DataTable dt , string contract = "") {
            string result = "";
            int k = 0;

            foreach (DataRow dr in dt.Rows) {
               string amtType = dr["AMT_TYPE"].AsString();
               string prodType = dr["PROD_TYPE"].AsString();
               object[] args = new object[] { dr };
               I40030AmtProdType iAmtProdType = CreateI40030AmtProdType(GetType() , "AmtProdType40030" + amtType + prodType , args);

               result += $"{iAmtProdType.AbbrName + contract}({iAmtProdType.KindId + iAmtProdType.StockName + iAmtProdType.StockId})";

               if (k == (dt.Rows.Count - 2)) {
                  result += "及";
               } else {
                  result += "、";
               }

               k++;
            }

            result = result.TrimEnd('、');

            return result;
         }

         protected override void SetDescStr() {
            string tempStr = "";
            allKindNameList = new List<string>();

            DateTime inputDate = TxtDate.AsDateTime("yyyyMMdd");
            string quarter = GetQuarter(inputDate.AddMonths(-3).Month);
            string quarterYear = (inputDate.Month / 3) == 0 ? inputDate.AddYears(-1).AsTaiwanDateTime("{0}年" , 3) :
                                    inputDate.AsTaiwanDateTime("{0}年" , 3);

            //季評估 替換參數
            List<string> kindNameQuarter = new List<string>();
            List<string> mLevelQuarter = new List<string>();
            List<string> curLevelQuarter = new List<string>();
            List<string> dayRiskQuarter = new List<string>();

            DataTable dtQuarter = Dt.Select("adj_rsn='1'").CopyToDataTable();
            dtQuarter = dtQuarter.Sort("SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC, KIND_ID ASC, AB_TYPE ASC");
            foreach (DataRow dr in dtQuarter.Rows) {
               string kindName = $"{dr["KIND_ABBR_NAME"].AsString()}({dr["kind_id"].AsString()})";
               string mLevel = dr["M_LEVEL_NAME"].AsString();
               string curLevel = dr["CUR_LEVEL_NAME"].AsString();

               mLevelQuarter.Add($"{mLevel}({dr["m_cm"].AsPercent(0)})");
               curLevelQuarter.Add($"{curLevel}({dr["cur_cm"].AsPercent(0)})");
               dayRiskQuarter.Add($"{dr["m_day_risk"].AsPercent(2)}");

               if (!kindNameQuarter.Exists(k => k == kindName)) {
                  kindNameQuarter.Add(kindName);
               }
            }

            //機動評估 替換參數
            List<string> kindNameInReserve = new List<string>();
            List<string> levelInReserve = new List<string>();
            List<string> dayRiskInReserve = new List<string>();
            List<string> monthRiskInReserve = new List<string>();

            DataTable dtInReserve = Dt.Select("adj_rsn='2'").CopyToDataTable();
            dtInReserve = dtInReserve.Sort("SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC, KIND_ID ASC, AB_TYPE ASC");
            foreach (DataRow dr in dtInReserve.Rows) {
               string kindName = $"{dr["KIND_ABBR_NAME"].AsString()}({dr["kind_id"].AsString()})";
               string mLevel = dr["M_LEVEL_NAME"].AsString();
               string curLevel = dr["CUR_LEVEL_NAME"].AsString();

               levelInReserve.Add(string.Format("{0}由{1}({2})調為{3}({4})" ,
                                    kindName , curLevel , dr["cur_cm"].AsPercent(0) , mLevel , dr["m_cm"].AsPercent(0)));

               dayRiskInReserve.Add($"{dr["m_day_risk"].AsPercent(2)}");
               monthRiskInReserve.Add($"{dr["m_30_risk"].AsPercent(2)}");

               if (!kindNameInReserve.Exists(k => k == kindName)) {
                  kindNameInReserve.Add(kindName);
               }
            }

            DataTable dtKindName = Dt.Select("adj_rsn in ('1','2')").CopyToDataTable();
            dtKindName = dtKindName.Sort("SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC, KIND_ID ASC, AB_TYPE ASC");
            foreach (DataRow dr in dtKindName.Rows) {
               string kindName = $"{dr["KIND_ABBR_NAME"].AsString()}({dr["kind_id"].AsString()})";
               if (!allKindNameList.Exists(k => k == kindName)) {
                  allKindNameList.Add(kindName);
               }
            }

            SetSubjectText("說　　明： ");

            //說明一
            SetInnerText("一、依本公司「結算保證金收取方式及標準」與「保證金調整作業規範」辦理。");

            //說明二
            tempStr = string.Format("二、依前述作業規範，本部定期或機動評估股票期貨及股票選擇權結算保證金風險價格係數及保證金適用級距，" +
                                    "有關{0}第{1}季檢討結果，業經▲年▲月▲日▲▲▲號簽奉核可在案▲▲▲。" ,
                                    quarterYear , quarter);
            SetInnerText(tempStr);

            //說明三
            tempStr = string.Format("三、依前簽核示，建議調整保證金適用級距之股票期貨契約共計{0}檔，說明如下：" , Dt.Rows.Count);
            SetInnerText(tempStr);

            //季評估
            string these = kindNameQuarter.Count() > 1 ? "等" : "";
            string respectively = kindNameQuarter.Count() > 1 ? "分別" : "";


            tempStr = string.Format("(一) 依股票期貨風險係數估算方式，現行結算保證金適用級距與第{0}季▲▲▲評估結果分屬不同級距者，" +
                                    "計有{1}，該{2}契約現行結算保證金級距{3}為{4}，經試算其標的證券風險價格係數平均值為{5}，{3}適用{6}，建議調整保證金適用級距。" ,
                                    quarter , GenArrayTxt(kindNameQuarter) , these , respectively , GenArrayTxt(curLevelQuarter) ,
                                    GenArrayTxt(dayRiskQuarter) , GenArrayTxt(mLevelQuarter));

            SetInnerText(tempStr , true , 4.11f , 1.25f);

            //機動評估
            these = kindNameInReserve.Count() > 1 ? "等" : "";
            respectively = kindNameInReserve.Count() > 1 ? "分別" : "";

            tempStr = string.Format("(二) 依股票期貨機動評估指標，各股票期貨契約30天期風險價格係數較現行結算保證金適用比例變動幅度連續3個交易日達10%，" +
                                    "且30天期風險價格係數與2年平均值取孰高者，該值所在級距與現行適用級距不同時，即進行機動檢討，並以30天期風險價格係數所在級距，" +
                                    "訂定該股票期貨結算保證金適用比例。經機動檢討結果，計有{0}達機動評估指標，觀察該{1}契約30天期風險價格" +
                                    "係數{2}為{3}，風險價格係數2年平均值{2}為{4}，建議調整保證金級距，{5}。" ,
                                    GenArrayTxt(kindNameInReserve) , these , respectively , GenArrayTxt(monthRiskInReserve) ,
                                    GenArrayTxt(dayRiskInReserve) , GenArrayTxt(levelInReserve));

            SetInnerText(tempStr , true , 4.11f , 1.25f);

            //說明四
            SetInnerText("四、本次保證金倘經調整，其金額變動如下：");
         }

         /// <summary>
         /// 取得季資訊
         /// </summary>
         /// <param name="month">月份</param>
         /// <returns></returns>
         protected virtual string GetQuarter(int month) {

            int re = (month % 3) == 0 ? (month / 3) : (month / 3) + 1;

            return re.ToString();
         }

         /// <summary>
         /// 設定開頭文字
         /// </summary>
         /// <param name="dtAttend"></param>
         protected override void SetHead(DataTable dtAttend) {
            string chairman = DtMinutes.Rows[0]["RPTF_TEXT"].AsString();

            SetSubjectText("案　　由： ");

            SetInnerText(CaseDescStr.Replace("#kind_name_list#" , GenProdName(Dt , "")) , false , 2.75f , 2.75f);

            SetDescStr();

            SetRtfDescText(GenMeetingDate() , chairman , GenAttend(dtAttend));
         }

         /// <summary>
         /// 設定欄位值
         /// </summary>
         protected override void SetTableColValue(I40030AmtProdType iAmtProd , DataRow dr) {

            int k = 1;

            foreach (string rowName in iAmtProd.RowName) {

               TableRow tableRow = WordTable.Rows.Append();

               WordTableCell = tableRow.FirstCell;
               WordTableCell.PreferredWidthType = WidthType.Fixed;
               WordTableCell.PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(1.5f);

               Doc.InsertSingleLineText(WordTableCell.Range.Start , rowName);

               //特殊處理, 選擇權時有AB值, k=1 跑保證金或A值, k>1 跑B值
               string[] colNameList = k == 1 ? iAmtProd.DbColName : iAmtProd.DbColNameB;
               string numberFormat = k == 1 ? iAmtProd.NumberFormat : iAmtProd.NumberFormatB;

               int i = 1;
               foreach (string col in colNameList) {
                  Doc.InsertSingleLineText(WordTable[tableRow.Index , i].Range.Start , !decimal.Equals(dr[col].AsDecimal() , 0) ?
                  dr[col].AsPercent(2) : string.Empty);
                  i++;
               }
               k++;
            }
         }

         protected override void DrowTable(DataTable dataTable) {

            foreach (DataRow dr in dataTable.Rows) {

               string amtType = dr["AMT_TYPE"].AsString();
               string prodType = dr["PROD_TYPE"].AsString();
               object[] args = new object[] { dr };
               I40030AmtProdType iAmtProdType = CreateI40030AmtProdType(GetType() , "AmtProdType40030" + amtType + prodType , args);

               SetComment(iAmtProdType.CurrencyName);

               CreateTable(Doc , 2 , 7);

               SetTableColTitle(iAmtProdType.ProdName , iAmtProdType.TableTitle ,
                              $"調整後保證金金額{Characters.LineBreak}({dr["m_level_name"]})" , $"調整前保證金金額{Characters.LineBreak}({dr["cur_level_name"]})");

               SetTableColValue(iAmtProdType , dr);
            }

         }
      }

      /// <summary>
      /// 一般 輸出 rtf and excel
      /// </summary>
      private class ExportWord0B : ExportWord {
         protected virtual DataTable DtAbroad { get; set; }
         protected virtual DataTable DtSpan { get; set; }
         protected virtual DataTable DtSpanTable { get; set; }
         protected virtual string[] ChineseNumber { get; set; }

         public ExportWord0B(string txtdate , string adjtype , string programId , List<CheckedItem> checkeditems) :
                     base(txtdate , adjtype , programId , checkeditems) {

            ChineseNumber = new string[] { "零" , "一" , "二" , "三" , "四" , "五" , "六" , "七" , "八" , "九" , "十" };
         }

         public override ReturnMessageClass Export() {
            ReturnMessageClass msg = new ReturnMessageClass();
            msg.Status = ResultStatus.Fail;

            try {
               #region 批次檢查
               //group 正常值
               foreach (CheckedItem c in CheckedItems) {
                  int cntValue = c.CheckedValue == 1 ? 1 : 2;
                  string checkBatch = "";

                  OswGrp = c.CheckedValue.AsString() + "%";

                  if (CheckedItems.Count == 3) {
                     if (CheckedItems[0].CheckedDate == CheckedItems[1].CheckedDate && CheckedItems[0].CheckedDate == CheckedItems[2].CheckedDate)
                        OswGrp = "%";
                  }

                  //Grp3 不檢查Ai2
                  if (c.CheckedValue != 7) {
                     checkBatch = PbFunc.f_chk_ai2(c.CheckedDate.ToString("yyyyMMdd") , OswGrp , "Y" , $"Group{c.CheckedValue}" , cntValue);

                     if (checkBatch != "") throw new Exception("批次檢查錯誤");
                  }

                  checkBatch = PbFunc.f_chk_130_wf("40030" , c.CheckedDate , OswGrp);
                  if (checkBatch != "") {
                     DialogResult result = MessageDisplay.Choose($"{c.CheckedDate.ToShortDateString()}-{checkBatch}，是否要繼續?");
                     if (result == DialogResult.No) {
                        msg.Status = ResultStatus.FailButNext;
                        return msg;
                     }

                     if (CheckedItems.Count == 3) {
                        if (CheckedItems[0].CheckedDate == CheckedItems[1].CheckedDate && CheckedItems[0].CheckedDate == CheckedItems[2].CheckedDate)
                           break;
                     }
                  }
               }
               #endregion 

               FilePath = PbFunc.wf_copy_file(ProgramId , MeetingLogFileName);

               //取得會議紀錄 / 議程資訊
               GetRPTF();
               string chairman = DtMinutes.Rows[0]["RPTF_TEXT"].AsString();

               #region 會議記錄
               OpenFile();

               //表頭 出席者
               SetHead(DtMinutes);

               int caseNo = 0;//案由數
               //案由一(指數, 公債, 黃金)
               foreach (CheckedItem c in CheckedItems) {
                  List<DataRow> drsTemp = Dt.Select("prod_subtype in ('I', 'B', 'C') and data_ymd = " +
                                                "'" + c.CheckedDate.ToString("yyyyMMdd") + "'").ToList();

                  //判斷日期是否一樣, 一樣的話跳過
                  if (CheckedItems.IndexOf(c) != 0) {
                     if (CheckedItems.Exists(i => i.CheckedDate == c.CheckedDate))
                        continue;
                  }

                  if (drsTemp.Count > 0) {
                     DataTable dtTemp = drsTemp.CopyToDataTable();
                     dtTemp = dtTemp.Sort("SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC, KIND_ID ASC, AB_TYPE ASC");//排序

                     //案由一後文字
                     SetCase(++caseNo , "檢陳本公司{0}保證金調整案，謹提請討論。" , GenArrayTxt(wfKindIdC(dtTemp)));

                     //案由一下說明文
                     SetFirstCaseDesc(dtTemp , c.CheckedDate);

                     //案由一決議
                     SetFirstCaseResult(dtTemp , c.CheckedDate);
                  }
               }

               //案由二(匯率期) 夜盤才有匯率資料
               CheckedItem group2 = CheckedItems.Where(c => c.CheckedValue == 5).FirstOrDefault();
               if (group2 != null) {
                  List<DataRow> drsTemp = Dt.Select("prod_subtype = 'E'").ToList();

                  if (drsTemp.Count > 0) {
                     DataTable dtTemp = drsTemp.CopyToDataTable();
                     dtTemp = dtTemp.Sort("SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC, KIND_ID ASC, AB_TYPE ASC");

                     //案由二後文字
                     SetCase(++caseNo , "本公司{0}保證金調整案，謹提請討論。" , GenArrayTxt(wfKindIdC(dtTemp)));

                     //案由二下說明文
                     SetSecondCaseDesc(dtTemp , group2.CheckedDate);

                     //案由二決議
                     SetSecondCaseResult(dtTemp , group2.CheckedDate);
                  }
               }

               //案由三(ETF) 夜盤才有ETF資料
               if (group2 != null) {
                  List<DataRow> drsTemp = Dt.Select("prod_subtype = 'S'").ToList();

                  if (drsTemp.Count > 0) {
                     DataTable dtTemp = drsTemp.CopyToDataTable();
                     dtTemp = dtTemp.Sort("SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC, KIND_ID ASC, AB_TYPE ASC");

                     //案由三後文字
                     SetCase(++caseNo , "檢陳本公司{0}保證金調整案，謹提請討論。" , GenArrayTxt(wfKindIdA(dtTemp)));

                     //案由三下說明文
                     SetThirdCaseDesc(dtTemp , group2.CheckedDate);

                     //案由三決議
                     SetThirdCaseResult(dtTemp , CheckedItems);
                  }
               }

               //案由四(STF) 日盤才有STF資料
               CheckedItem group1 = CheckedItems.Where(c => c.CheckedValue == 1).FirstOrDefault();
               if (group1 != null) {
                  decimal cmMax = new MGRT1().GetCmMax("F").AsDecimal();
                  DataRow drCmRate = new MGRT1().GetCmRate("F").AsEnumerable().FirstOrDefault();
                  decimal ldValue = new MGT2().GetldValue("F" , "STF").AsDecimal();

                  decimal cmRate1 = drCmRate["ld_cm_rate1"].AsDecimal();
                  decimal cmRate2 = drCmRate["ld_cm_rate2"].AsDecimal();
                  decimal cmRate3 = drCmRate["ld_cm_rate3"].AsDecimal();

                  if (ldValue == 0) ldValue = cmRate1 + (decimal)0.01;

                  DataTable dtSTF = new D42011().d_42011_detl(group1.CheckedDate , null , ldValue , cmRate2 , cmRate3 , cmMax , cmRate1 , cmRate2 , cmRate3 , cmMax);
                  List<DataRow> drsSTF = dtSTF.Select("day_cnt >= 1").ToList();

                  if (drsSTF.Count > 0) {
                     DataTable dtTemp = drsSTF.CopyToDataTable();
                     dtTemp = dtTemp.Sort("APDK_KIND_GRP2 ASC, APDK_KIND_LEVEL ASC, MGR3_KIND_ID ASC");

                     //案由四後文字
                     SetCase(++caseNo , "本公司{0}保證金調整案，謹提請討論。" , GenArrayTxt(wfKindIdSTFA(dtTemp)));

                     //案由四下說明文
                     SetForthCaseDesc(dtTemp , new MGRT1().GetDistinctMGRT1Level() , group1.CheckedDate);

                     //案由四決議
                     SetForthCaseResult(group1.CheckedDate);
                  }
               }

               //案由五(SPAN)
               List<DataRow> drsSPAN = DtSpan.AsEnumerable().ToList();
               List<CheckedItem> checkedItems = CheckedItems.Where(c => c.CheckedValue == 1 || c.CheckedValue == 5).ToList();
               if (drsSPAN.Count > 0) {
                  DataTable dtTemp = drsSPAN.CopyToDataTable();

                  //案由五後文字
                  SetSubjectText($"案 由 {ChineseNumber[++caseNo]}：");
                  SetInnerText("檢陳本公司SPAN參數調整案，謹提請討論。" , false , 2.75f , 2.75f);

                  //案由五下說明文
                  SetFifthCaseDesc(dtTemp , checkedItems , caseNo);

                  //案由五決議
                  SetFifthCaseResult(dtTemp);
               }

               base.SetSubjectText("貳、臨時動議：無");
               base.SetSubjectText("參、散　　會：下午5時30分");

               base.SetAllNumberAndEnglishFont(Doc);//設定英數字體

               Doc.EndUpdate();
               DocSer.SaveDocument(FilePath , DocumentFormat.Rtf);
               DocSer.Dispose();
#if DEBUG
               System.Diagnostics.Process.Start(FilePath);
#endif
               #endregion 會議紀錄

               FilePath = PbFunc.wf_copy_file(ProgramId , AgendaFileName);

               #region 議程
               OpenFile();

               //表頭 出席者
               SetHead(DtAgenda);

               caseNo = 0;//案由數
               //案由一(指數, 公債, 黃金)
               foreach (CheckedItem c in CheckedItems) {
                  List<DataRow> drsTemp = Dt.Select("prod_subtype in ('I', 'B', 'C') and data_ymd = " +
                                                "'" + c.CheckedDate.ToString("yyyyMMdd") + "'").ToList();

                  if (CheckedItems.IndexOf(c) != 0) {
                     if (CheckedItems.Exists(i => i.CheckedDate == c.CheckedDate))
                        continue;
                  }

                  if (drsTemp.Count > 0) {
                     DataTable dtTemp = drsTemp.CopyToDataTable();
                     dtTemp = dtTemp.Sort("SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC, KIND_ID ASC, AB_TYPE ASC");

                     //案由一後文字
                     SetCase(++caseNo , "檢陳本公司{0}保證金調整案，謹提請討論。" , GenArrayTxt(wfKindIdC(dtTemp)));

                     //案由一下說明文
                     SetFirstCaseDesc(dtTemp , c.CheckedDate);
                  }
               }

               //案由二(匯率期) 夜盤才有匯率資料
               group2 = CheckedItems.Where(c => c.CheckedValue == 5).FirstOrDefault();
               if (group2 != null) {
                  List<DataRow> drsTemp = Dt.Select("prod_subtype = 'E'").ToList();

                  if (drsTemp.Count > 0) {
                     DataTable dtTemp = drsTemp.CopyToDataTable();
                     dtTemp = dtTemp.Sort("SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC, KIND_ID ASC, AB_TYPE ASC");

                     //案由二後文字
                     SetCase(++caseNo , "本公司{0}保證金調整案，謹提請討論。" , GenArrayTxt(wfKindIdC(dtTemp)));

                     //案由二下說明文
                     SetSecondCaseDesc(dtTemp , group2.CheckedDate);
                  }
               }

               //案由三(ETF) 夜盤才有ETF資料
               if (group2 != null) {
                  List<DataRow> drsTemp = Dt.Select("prod_subtype = 'S'").ToList();

                  if (drsTemp.Count > 0) {
                     DataTable dtTemp = drsTemp.CopyToDataTable();
                     dtTemp = dtTemp.Sort("SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC, KIND_ID ASC, AB_TYPE ASC");

                     //案由三後文字
                     SetCase(++caseNo , "檢陳本公司{0}保證金調整案，謹提請討論。" , GenArrayTxt(wfKindIdA(dtTemp)));

                     //案由三下說明文
                     SetThirdCaseDesc(dtTemp , group2.CheckedDate);
                  }
               }

               //案由四(STF) 日盤才有STF資料
               group1 = CheckedItems.Where(c => c.CheckedValue == 1).FirstOrDefault();
               if (group1 != null) {
                  decimal cmMax = new MGRT1().GetCmMax("F").AsDecimal();
                  DataRow drCmRate = new MGRT1().GetCmRate("F").AsEnumerable().FirstOrDefault();
                  decimal ldValue = new MGT2().GetldValue("F" , "STF").AsDecimal();

                  decimal cmRate1 = drCmRate["ld_cm_rate1"].AsDecimal();
                  decimal cmRate2 = drCmRate["ld_cm_rate2"].AsDecimal();
                  decimal cmRate3 = drCmRate["ld_cm_rate3"].AsDecimal();

                  if (ldValue == 0) ldValue = cmRate1 + (decimal)0.01;

                  DataTable dtSTF = new D42011().d_42011_detl(group1.CheckedDate , null , ldValue , cmRate2 , cmRate3 , cmMax , cmRate1 , cmRate2 , cmRate3 , cmMax);
                  List<DataRow> drsSTF = dtSTF.Select("day_cnt >= 1").ToList();

                  if (drsSTF.Count > 0) {
                     DataTable dtTemp = drsSTF.CopyToDataTable();

                     //案由四後文字
                     SetCase(++caseNo , "本公司{0}保證金調整案，謹提請討論。" , GenArrayTxt(wfKindIdSTFA(dtTemp)));

                     //案由四下說明文
                     SetForthCaseDesc(dtTemp , new MGRT1().GetDistinctMGRT1Level() , group1.CheckedDate);
                  }
               }

               //案由五(SPAN)
               drsSPAN = DtSpan.AsEnumerable().ToList();
               checkedItems = CheckedItems.Where(c => c.CheckedValue == 1 || c.CheckedValue == 5).ToList();
               if (drsSPAN.Count > 0 && checkedItems.Count > 0) {
                  DataTable dtTemp = drsSPAN.CopyToDataTable();

                  //案由五後文字
                  SetSubjectText($"案 由 {ChineseNumber[++caseNo]}：");
                  SetInnerText("檢陳本公司SPAN參數調整案，謹提請討論。" , false , 2.75f , 2.75f);

                  //案由五下說明文
                  SetFifthCaseDesc(dtTemp , checkedItems , caseNo);
               }

               base.SetSubjectText("貳、臨時動議：");
               base.SetSubjectText("參、散　　會：");
               base.SetAllNumberAndEnglishFont(Doc);//設定英數字體

               Doc.EndUpdate();
               DocSer.SaveDocument(FilePath , DocumentFormat.Rtf);
               DocSer.Dispose();
#if DEBUG
               System.Diagnostics.Process.Start(FilePath);
#endif
               #endregion 議程

               #region Export Excel
               string ExcelFilePath = PbFunc.wf_copy_file("40030" , "40030");
               DevExpress.Spreadsheet.Workbook workbook = new DevExpress.Spreadsheet.Workbook();
               workbook.LoadDocument(ExcelFilePath);

               List<DataRow> drsFUT = Dt.Select("prod_type='F' and m_change_flag = 'Y'").ToList();
               if (drsFUT.Count > 0) {
                  ExportExcelFut(workbook , drsFUT.CopyToDataTable().Sort("SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC, KIND_ID ASC, AB_TYPE ASC"));
               }

               List<DataRow> drsOPT = Dt.Select("prod_type='O' and m_change_flag = 'Y'").ToList();
               if (drsOPT.Count > 0) {
                  ExportExcelOPT(workbook , drsOPT.CopyToDataTable().Sort("SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC, KIND_ID ASC, AB_TYPE ASC"));
               }

               checkedItems = CheckedItems.Where(c => c.CheckedValue == 1 || c.CheckedValue == 5).ToList();
               if (DtSpanTable.Rows.Count > 0 && checkedItems.Count > 0) {
                  ExportExcelSpan(workbook , DtSpanTable , checkedItems);
               }

               //依照選擇下拉選單 選擇sheet 匯出
               foreach (CheckedItem items in CheckedItems) {
                  switch (items.CheckedValue) {
                     case 1: {
                           ExportCompareExcel(workbook , "TXF" , "TX" , 5);
                           ExportCompareExcel(workbook , "UDF" , "DJIA" , 5);
                           ExportCompareExcel(workbook , "SPF" , "S&P500" , 5);
                           ExportCompareExcel(workbook , "BRF" , "Brent Crude" , 5);

                           break;
                        }
                     case 5: {
                           ExportCompareExcel(workbook , "GDF" , "黃金" , 6);
                           ExportCompareExcel(workbook , "RHF" , "人民幣" , 6);
                           ExportCompareExcel(workbook , "XEF" , "歐元" , 3);
                           ExportCompareExcel(workbook , "XJF" , "日圓" , 3);
                           ExportCompareExcel(workbook , "XBF" , "英鎊" , 3);
                           ExportCompareExcel(workbook , "XAF" , "澳幣" , 3);

                           break;
                        }
                     case 7: {
                           ExportCompareExcel(workbook , "TJF" , "TOPIX" , 7);
                           ExportCompareExcel(workbook , "I5F" , "Nifty50" , 5);

                           break;
                        }
                  }
               }

               workbook.SaveDocument(ExcelFilePath);
#if DEBUG
               System.Diagnostics.Process.Start(ExcelFilePath);
#endif
               #endregion Export Excel

               msg.Status = ResultStatus.Success;
               return msg;
            } catch (Exception ex) {
               base.ErrorHandle(ex , msg);
               return msg;
            }
         }

         public override ReturnMessageClass GetData() {
            ReturnMessageClass msg = new ReturnMessageClass(MessageDisplay.MSG_NO_DATA);
            msg.Status = ResultStatus.Fail;
            DtAbroad = new DataTable();
            DtSpan = new DataTable();
            DtSpanTable = new DataTable();
            Dt = new DataTable();

            foreach (CheckedItem c in CheckedItems) {
               DateTime searchDate = default(DateTime);

               OswGrp = c.CheckedValue.AsString();
               searchDate = c.CheckedDate;

               if (CheckedItems.Count == 3) {
                  if (CheckedItems[0].CheckedDate == CheckedItems[1].CheckedDate && CheckedItems[0].CheckedDate == CheckedItems[2].CheckedDate) {
                     searchDate = CheckedItems.FirstOrDefault().CheckedDate;
                     OswGrp = "%";
                  }
               }

               //Abroad
               DataColumn[] abroadKeys = new DataColumn[2];
               abroadKeys[0] = DtAbroad.Columns["f_id"];
               abroadKeys[1] = DtAbroad.Columns["kind_id"];
               DtAbroad.PrimaryKey = abroadKeys;
               DtAbroad.Merge(Dao40030.GetAborad(searchDate , OswGrp));

               //主要資料
               DataColumn[] keys = new DataColumn[1];
               keys[0] = Dt.Columns["kind_id_out"];
               Dt.PrimaryKey = keys;
               DataTable dtMain = Dao40030.GetData(searchDate.ToString("yyyyMMdd") , OswGrp , AsAdjType , AdjType.SubStr(1 , 1));
               if (dtMain.Rows.Count > 0) {
                  Dt.Merge(dtMain);
               } else {
                  msg.ReturnMessage += $"{c.CheckedDate.ToShortDateString()},40030-一般{Characters.LineBreak}";
               }

               //Span
               OswGrp = OswGrp == "%" ? "%" : $"{c.CheckedValue}%";
               DataColumn[] spanKeys = new DataColumn[1];
               spanKeys[0] = DtSpan.Columns["SPT1_COM_ID"];
               DtSpan.PrimaryKey = spanKeys;

               spanKeys[0] = DtSpanTable.Columns["SPT1_COM_ID"];
               DtSpanTable.PrimaryKey = spanKeys;

               if (OswGrp == "%" || c.ETCSelected == c.CheckedValue.AsString()) {
                  DtSpan.Merge(Dao40030.GetSpan(searchDate , OswGrp , "" , "ETC"));
                  DtSpanTable.Merge(Dao40030.GetSpanTableData(searchDate , OswGrp , "" , "ETC"));
               } else {
                  DtSpan.Merge(Dao40030.GetSpan(searchDate , OswGrp , "ETC" , ""));
                  DtSpanTable.Merge(Dao40030.GetSpanTableData(searchDate , OswGrp , "ETC" , ""));
               }

            }

            if (DtSpan != null)
               if (DtSpan.Rows.Count > 0) DtSpan = DtSpan.Sort("SP1_SEQ_NO ASC");

            if (DtSpanTable != null)
               if (DtSpanTable.Rows.Count > 0) {
                  DtSpanTable = DtSpanTable.Sort("SP1_SEQ_NO ASC");
                  DtSpanTable.Columns.Remove("SP1_SEQ_NO");
               }

            if (Dt != null) {
               if (Dt.Rows.Count > 0) {
                  msg.Status = ResultStatus.Success;
               }
            }

            return msg;
         }

         protected override void SetHead(DataTable dtAttend) {
            string chairman = DtMinutes.Rows[0]["RPTF_TEXT"].AsString();

            SetRtfDescText(GenMeetingDate() , chairman , GenAttend(dtAttend));
         }

         protected virtual void SetCase(int caseNo , string caseTxt , string kindName , bool hasNewLine = true) {
            string tmpStr = "";

            SetSubjectText($"案 由 {ChineseNumber[caseNo]}：");

            tmpStr = string.Format(caseTxt , kindName);
            SetInnerText(tmpStr , false , 2.75f , 2.75f , hasNewLine);
         }

         /// <summary>
         /// 案由一 說明
         /// </summary>
         /// <param name="dtTemp"></param>
         /// <param name="checkedDate"></param>
         protected virtual void SetFirstCaseDesc(DataTable dtTemp , DateTime checkedDate) {
            string tmpStr = "";
            int licnt = 3;

            SetSubjectText($"說　　明：");

            //說明一
            tmpStr = string.Format("一、{0}本公司上開契約結算保證金之變動幅度已達得調整標準之百分比，且進位後金額有變動時，" +
                                    "依本公司保證金調整作業規範，由督導結算業務主管召集業務相關部門主管會商決定是否調整。" ,
                                    dtTemp.Rows[0]["data_ymd"].AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}/{1}/{2}" , 3));
            SetInnerText(tmpStr);

            //說明二
            SetInnerText("二、經以簡單移動平均法(SMA)、加權指數移動平均(EWMA)、簡單移動平均法_日內變動(MAX)方式試算上開契約結算保證金之變動幅度如下：");
            String info01 = "(一)臺股期貨契約(TX)結算保證金變動幅度分別為 - 18.5 %、-13.3 % 及14.2 %。";
            SetInnerText(info01, true, 4.11f, 1.25f);
            String info02 = "(二)臺指選擇權契約(TXO)結算保證金採現貨資料計算之變動幅度分別為 - 15.0 %、-15.0 % 及 - 8.7 %，及採期貨資料計算是變動幅度分別為 - 15.2 %、-9.9 % 及18.7 %。";
            SetInnerText(info02, true, 4.11f, 1.25f);
            String info03 = "(三)電子期貨契約(TE)結算保證金變動幅度分別為11.9 %、32.9 % 及27.7 %。";
            SetInnerText(info03, true, 4.11f, 1.25f);
            String info04 = "(四)電子選擇權契約(TEO)結算保證金採現貨資料計算之變動幅度分別為2.7 %、21.9 % 及15.4 %。";
            SetInnerText(info04, true, 4.11f, 1.25f);
             
            DrowTable(dtTemp);

            //說明三
            SetInnerText("三、基於穩健保守及貼近國際同類商品保證金水準之原則，建議TX及TXO採加權指數移動平均(EWMA)調整保證金，TE及採簡單移動平均法(SMA)調整保證金，TF、XI及TEO採簡單移動平均法_日內變動(MAX)調整保證金其金額變動如下：");

            //說明四、公債類
            List<DataRow> drsDebt = dtTemp.Select("prod_subtype = 'B' and data_ymd = '" + checkedDate.ToString("yyyyMMdd") + "'").ToList();
            if (drsDebt.Count > 0) SetFirstCaseDebtDesc(drsDebt.CopyToDataTable() , $"{ChineseNumber[++licnt]}、");

            //說明五、指數類
            List<DataRow> drsIndex = dtTemp.Select("prod_subtype='I' and data_ymd = '" + checkedDate.ToString("yyyyMMdd") + "' and kind_id<>'MXF'").ToList();
            if (drsIndex.Count > 0) SetFirstCaseIndexDesc(drsIndex.CopyToDataTable() , $"{ChineseNumber[++licnt]}、" , checkedDate);

            //說明六、黃金類
            List<DataRow> drsGold = dtTemp.Select("prod_subtype='C' and (kind_id = 'GDF' or kind_id='TGF') and data_ymd = '" + checkedDate.ToString("yyyyMMdd") + "'").ToList();
            if (drsGold.Count > 0) SetFirstCaseGoldDesc(drsGold.CopyToDataTable() , $"{ChineseNumber[++licnt]}、" , checkedDate);

            //說明七、布蘭特原油
            List<DataRow> drsBRF = dtTemp.Select("kind_id = 'BRF'").ToList();
            if (drsBRF.Count > 0) SetFirstCaseBRFDesc(drsBRF.CopyToDataTable() , $"{ChineseNumber[++licnt]}、" , checkedDate);

            //說明八
            SetFirstCaseLastDesc(dtTemp , $"{ChineseNumber[++licnt]}、" , checkedDate);
         }

         /// <summary>
         /// 案由一 決議
         /// </summary>
         /// <param name="dtTemp"></param>
         /// <param name="checkedDate"></param>
         protected virtual void SetFirstCaseResult(DataTable dtTemp , DateTime checkedDate) {
            string tmpStr = "";
            int licnt = 0;
            string checkedDateStr = checkedDate.ToString("yyyyMMdd");
            List<DataRow> drsTemp = new List<DataRow>();

            SetSubjectText($"決　　議：");

            drsTemp = dtTemp.Select("prod_subtype = 'B' and adj_code = 'Y' and data_ymd = '" + checkedDateStr + "'").ToList();
            if (drsTemp.Count > 0) {
               SetInnerText($"{ChineseNumber[++licnt]}、調整GBF保證金如說明二。");
            }

            drsTemp.Clear();
            drsTemp = dtTemp.Select("prod_subtype in ('I','C') and adj_code = 'Y' and data_ymd = '" + checkedDateStr + "'").ToList();
            if (drsTemp.Count > 0) {
               SetInnerText($"{ChineseNumber[++licnt]}、調整{GenArrayTxt(wfKindIdE(drsTemp.CopyToDataTable().Sort("SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC, KIND_ID ASC, AB_TYPE ASC")))}保證金如說明二。");
            }

            drsTemp.Clear();
            drsTemp = dtTemp.Select("prod_subtype = 'B' and adj_code = ' ' and data_ymd = '" + checkedDateStr + "'").ToList();
            if (drsTemp.Count > 0) {
               SetInnerText($"{ChineseNumber[++licnt]}、不調整GBF保證金，持續觀察，注意未沖銷部位數變化之狀況，於必要時隨時召開會議討論是否調整保證金。");
            }

            drsTemp.Clear();
            drsTemp = dtTemp.Select("prod_subtype in ('I','C')  and adj_code = ' ' and data_ymd ='" + checkedDateStr + "'").ToList();
            if (drsTemp.Count > 0) {
               string iibsDate = PbFunc.f_get_ocf_next_n_day(checkedDate , 10).AsTaiwanDateTime("{0}/{1}/{2}" , 3);

               tmpStr = string.Format("{0}、不調整{1}保證金，觀察▲▲10個交易日(至▲▲{2})，惟仍需持續注意各契約保證金變動幅度、" +
                                    "未沖銷部位數明顯變化之狀況、保證金占契約價值比重及國際同類商品保證金調整等情事，於必要時隨時召開會議討論是否調整保證金。" ,
                                    ChineseNumber[++licnt] , GenArrayTxt(wfKindIdE(drsTemp.CopyToDataTable().Sort("SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC, KIND_ID ASC, AB_TYPE ASC"))) , iibsDate);
               SetInnerText(tmpStr);
            }

         }

         /// <summary>
         /// 案由一 債券說明
         /// </summary>
         /// <param name="dtDebt"></param>
         /// <param name="descPoint">說明項數</param>
         protected virtual void SetFirstCaseDebtDesc(DataTable dtDebt , string descPoint) {
            string tmpStr = "";

            if (dtDebt.Rows.Count > 0) {

               List<string> kindNameList = wfKindIdC(dtDebt);

               List<string> adjRateList = new List<string>();
               List<DataRow> tmpList = dtDebt.AsEnumerable().ToList();
               tmpList.ForEach(l => adjRateList.Add(l.Field<double>("adj_rate").AsPercent(2)));

               tmpStr = string.Format("{0}{1}之保證金變動幅度已達{2}：" , descPoint , GenArrayTxt(kindNameList) , GenArrayTxt(adjRateList));
               SetInnerText(tmpStr);

               //特殊處理, 不知原因
               DataRow drGBF = dtDebt.AsEnumerable().Where(d => d.Field<string>("kind_id").AsString() == "GBF").FirstOrDefault();
               if (drGBF != null) {
                  string iqnty = drGBF["i_qnty"].AsInt() > 0 ? string.Format("(今日成交量為" + drGBF["i_qnty"].AsString() + "口") : "";
                  string ioi = drGBF["i_qnty"].AsInt() > 0 ? string.Format("(今日未沖銷部位為" + drGBF["ioi"].AsString() + "口") : "";
                  string warn = "▲▲▲";

                  if (iqnty == "" || ioi == "") warn = "";

                  tmpStr = string.Format("(一) {0}近期皆無成交量{1}，且未沖銷部位為0{2}；價格無變化，風險價格係數為{3}。" +
                                         "公債期貨契約現行結算保證金占契約價值之比例為{4}{5}。" ,
                                         drGBF["kind_abbr_name"].AsString() , iqnty , ioi , drGBF["m_day_risk"].AsPercent(2) ,
                                         drGBF["cur_cm_rate"].AsPercent(2) , warn);

                  SetInnerText(tmpStr , true , 4.11f , 1.25f);

                  tmpStr = string.Format("(二) 經權衡市場風險及該商品並無未沖銷部位{0}，建議暫不調整，持續觀察，注意未沖銷部位變化之狀況，" +
                                          "於必要時隨時召開會議討論是否調整保證金。" , warn);

                  SetInnerText(tmpStr , true , 4.11f , 1.25f);
               }
            }
         }

         /// <summary>
         /// 案由一 指數類說明
         /// </summary>
         /// <param name="dtIndex"></param>
         /// <param name="descPoint">說明項數</param>
         /// <param name="checkedDate"></param>
         protected virtual void SetFirstCaseIndexDesc(DataTable dtIndex , string descPoint , DateTime checkedDate) {
            string tmpStr = "";
            int node = 0;
            List<string> kindNameList = wfKindIdC(dtIndex);

            List<string> adjRateList = new List<string>();
            List<string> lastAdjRateList = new List<string>();
            List<DataRow> tmpList = dtIndex.AsEnumerable().ToList();
            tmpList.ForEach(l => adjRateList.Add(l.Field<double?>("adj_rate").AsPercent(1)));
            tmpList.ForEach(l => lastAdjRateList.Add(l.Field<double?>("last_adj_rate").AsPercent(1)));

            tmpStr = string.Format("{0}{1}，其結算保證金變動幅度{2}達{3}(前一營業日保證金變動幅度{2}達{4})，已達得調整標準百分比，" +
                                    "且進位後金額有變動，依下列考量因素說明:" , descPoint , GenArrayTxt(kindNameList) , SingleOrMore(dtIndex) ,
                                    GenArrayTxt(adjRateList) , GenArrayTxt(lastAdjRateList));
            SetInnerText(tmpStr);

            //(一) 
            kindNameList.Clear();
            tmpList.ForEach(l => kindNameList.Add(l.Field<string>("kind_id_out").AsString()));
            List<string> dayCntList = new List<string>();
            tmpList.ForEach(l => {
               if (l.Field<decimal>("day_cnt") == 0)
                  dayCntList.Add("第1天");
               else
                  dayCntList.Add($"第{l.Field<decimal>("day_cnt")}天");
            });


            tmpStr = string.Format("({3}) 保證金變動幅度達10%，且進位後金額有變動之天數：觀察{0}，{1}為{2}達調整標準。" ,
                                    GenArrayTxt(kindNameList) , SingleOrMore(dtIndex) , GenArrayTxt(dayCntList) , ChineseNumber[++node]);
            SetInnerText(tmpStr , true , 4.11f , 1.25f);

            #region 二
            List<DateTime> deliveryDateList = new List<DateTime>();
            tmpList.ForEach(l => {
               if (!string.IsNullOrEmpty(l.Field<DateTime?>("i_mth_delivery_date").ToString())) {
                  if (!deliveryDateList.Exists(d => d == l.Field<DateTime>("i_mth_delivery_date")))
                     deliveryDateList.Add(l.Field<DateTime>("i_mth_delivery_date"));
               }
            });

            List<string> kindTypeList1 = new List<string>();// for type1
            List<string> kindTypeList2 = new List<string>();//for type2
            List<string> oiTypeList1 = new List<string>();// for type1
            List<string> oiTypeList2 = new List<string>();//for type2
            List<string> oiRateList = new List<string>();
            List<string> monthOiRateList = new List<string>();
            string settleMonth = "";
            foreach (DateTime deliveryDate in deliveryDateList) {
               DataTable dtDelivery = dtIndex.Select("prod_subtype = 'I' and kind_id <> 'MXF' and i_mth_delivery_date = " +
                                                      "'" + deliveryDate.ToString("yyyy/MM/dd") + "'").CopyToDataTable();

               List<DataRow> drsDelivery = dtDelivery.AsEnumerable().ToList();
               DateTime isEndDate = PbFunc.f_get_ocf_next_n_day(deliveryDate , -7);

               if ((checkedDate < isEndDate) || (checkedDate >= deliveryDate)) {
                  //type1 參數
                  drsDelivery.ForEach(r => kindTypeList1.Add(r.Field<string>("kind_id_out").AsString()));
                  drsDelivery.ForEach(r => oiTypeList1.Add($"{r.Field<decimal>("i_oi").ToString("#,##0")}口"));

                  drsDelivery.ForEach(d => {
                     if (d.Field<decimal>("i_oi_rate").AsDouble() < 0.01 && d.Field<decimal>("i_oi") > 0)
                        oiRateList.Add("小於0.01%");
                     else
                        oiRateList.Add(Math.Round(d.Field<decimal>("i_oi_rate") , 2 , MidpointRounding.AwayFromZero).AsString() + "%");
                  });
               } else {
                  //type2 參數
                  drsDelivery.ForEach(r => kindTypeList2.Add(r.Field<string>("kind_id_out").AsString()));
                  drsDelivery.ForEach(r => oiTypeList2.Add($"{r.Field<decimal>("i_oi").ToString("#,##0")}口"));

                  drsDelivery.ForEach(r => monthOiRateList.Add($"{r.Field<decimal>("i_mth_oi").ToString("#,##0")}口"));
                  settleMonth = dtDelivery.Rows[0]["i_mth_settle_date"].AsDateTime("yyyyMM").Month.AsString();
               }
            }

            if (kindTypeList1.Count > 0) {
               //type1
               string single = kindTypeList1.Count > 0 ? "分別" : "";

               tmpStr = string.Format("({5}) 未沖銷部位數：{0} {1}未沖銷部位數{2}為{3}，占全市場未沖銷部位之比例{2}為{4}。" ,
                                       checkedDate.AsTaiwanDateTime("{0}/{1}/{2}" , 3) , GenArrayTxt(kindTypeList1) , single ,
                                       GenArrayTxt(oiTypeList1) , GenArrayTxt(oiRateList) , ChineseNumber[++node]);

            }
            if (kindTypeList2.Count > 0) {
               //type2
               tmpStr = string.Format("({5}) 臨屆契約到期日之未沖銷部位數：考量將屆{0}月份契約到期結算，未沖銷部位較高({1} {2}未沖銷部位數為" +
                                    "{3}，其中{0}月份契約為{4})，需考量到期結算前調整保證金對交易人及市場之影響。" ,
                                    settleMonth , checkedDate.AsTaiwanDateTime("{0}/{1}/{2}" , 3) ,
                                    GenArrayTxt(kindTypeList2) , GenArrayTxt(oiTypeList2) , GenArrayTxt(monthOiRateList) , ChineseNumber[++node]);
            }

            SetInnerText(tmpStr , true , 4.11f , 1.25f);

            dtIndex = dtIndex.Select("prod_subtype='I' and kind_id<>'MXF'").CopyToDataTable();
            #endregion

            #region 三
            SetInnerText($"({ChineseNumber[++node]}) 現貨及期貨市場漲跌變化：" , true , 4.11f , 1.25f);
            int thridNode = 0;
            foreach (DataRow dr in dtIndex.Rows) {
               string prodType = dr["prod_type"].AsString();

               if (prodType == "O") {
                  List<DataRow> drsF = dtIndex.Select($"prod_type='F' and stock_id ='{dr["stock_id"].AsString()}'").ToList();
                  if (drsF.Count > 0) continue;
               }

               string kindIdOut = dr["kind_id_out"].AsString();
               tmpStr = $"{kindIdOut.SubStr(0 , 2)}現貨";

               decimal upDownPoint = GetUpDown("O" , dr);
               decimal upDownPoint2 = GetUpDown("F" , dr);
               decimal rateDiff = dr["m_cm"].AsDecimal() - dr["cur_cm"].AsDecimal();

               //現貨
               tmpStr += GenProdString("O" , dr);

               //期貨
               tmpStr += $"{kindIdOut.SubStr(0 , 2)}期貨";
               tmpStr += GenProdString("F" , dr);

               tmpStr = tmpStr.TrimEnd('、');
               tmpStr += "，";
               if (prodType == "F") {
                  DataRow drO = dtIndex.Select($"prod_type='O' and stock_id ='{dr["stock_id"].AsString()}'").FirstOrDefault();
                  string warn = "";
                  if (drO != null) {
                     decimal rateDiff2 = drO["m_cm"].AsDecimal() - drO["cur_cm"].AsDecimal();
                     //保證金:+都漲,-都跌,x不同
                     if (rateDiff > 0 && rateDiff2 > 0)
                        warn = "+";
                     else if (rateDiff < 0 && rateDiff2 < 0)
                        warn = "-";
                     else
                        warn = "x";

                     //現貨&期貨相同
                     //期貨&選擇權保證金相同
                     if (warn == "+" || warn == "-" || upDownPoint == 0 || upDownPoint2 == 0) {
                        if (warn != "x") {
                           tmpStr += $"{kindIdOut}及{drO["kind_id_out"].AsString()}保證金調整之方向與現貨及期貨市場漲跌方向";
                           tmpStr += MarketDirectionWarn(upDownPoint , warn);

                        } //期貨&選擇權保證金不同
                        else {
                           tmpStr += $"{kindIdOut}保證金調整之方向與現貨市場漲跌方向";
                           tmpStr += MarketDirection(upDownPoint , rateDiff);
                           tmpStr += $"{drO["kind_id_out"].AsString()}保證金調整之方向與現貨市場漲跌方向";
                           tmpStr += MarketDirection(upDownPoint2 , rateDiff2);
                        }
                     } else {
                        //現貨&期貨不同
                        //期貨&選擇權保證金相同
                        if (warn != "x") {
                           tmpStr += $"{kindIdOut}及{drO["kind_id_out"].AsString()}保證金調整之方向與現貨市場漲跌方向";

                           tmpStr += MarketDirectionWarn(upDownPoint , warn);

                           if (upDownPoint2 != 0) tmpStr += $"，與期貨市場漲跌方向{MarketDirectionWarn(upDownPoint2 , warn)}";
                        } //期貨&選擇權保證金不同
                        else {
                           //FUT
                           tmpStr += $"{kindIdOut}保證金調整之方向與現貨市場漲跌方向";
                           tmpStr += MarketDirection(upDownPoint , rateDiff);

                           if (upDownPoint2 != 0) {
                              tmpStr = "；與期貨市場漲跌方向";
                              tmpStr += MarketDirection(upDownPoint2 , rateDiff);
                           }
                           //OPT
                           tmpStr += $"，{drO["kind_id_out"].AsString()}保證金調整之方向與現貨市場漲跌方向";
                           tmpStr += MarketDirection(upDownPoint , rateDiff2);

                           if (upDownPoint2 != 0) {
                              tmpStr = "；與期貨市場漲跌方向";
                              tmpStr += MarketDirection(upDownPoint2 , rateDiff2);
                           }
                        }
                     }
                     SetInnerText($"{++thridNode}. {tmpStr}。" , true , 4.17f , 0.6f);
                     continue;
                  }//if (drO != null)
                   //無選擇權狀況
                   //現貨&期貨相同
                  if ((upDownPoint > 0 && upDownPoint2 > 0) || (upDownPoint < 0 && upDownPoint2 < 0)
                        || upDownPoint == 0 || upDownPoint2 == 0) {
                     tmpStr += $"{kindIdOut}保證金調整之方向與現貨及期貨市場漲跌方向{MarketDirection(upDownPoint , rateDiff)}";
                  } //現貨&期貨不同	
                  else {
                     tmpStr += $"{kindIdOut}保證金調整之方向與現貨市場漲跌方向{MarketDirection(upDownPoint , rateDiff)}";
                     tmpStr += $"；與期貨市場漲跌方向{MarketDirection(upDownPoint2 , rateDiff)}";
                  }
               } //if(prodType == "F")
               else {
                  //單一狀況
                  //現貨&期貨相同
                  if ((upDownPoint > 0 && upDownPoint2 > 0) || (upDownPoint < 0 && upDownPoint2 < 0)
                        || upDownPoint == 0 || upDownPoint2 == 0) {
                     tmpStr += $"{kindIdOut}保證金調整之方向與現貨及期貨市場漲跌方向{MarketDirection(upDownPoint , rateDiff)}";
                  } //現貨&期貨不同	
                  else {
                     tmpStr += $"{kindIdOut}保證金調整之方向與現貨市場漲跌方向{MarketDirection(upDownPoint , rateDiff)}";
                     tmpStr += $"；與期貨市場漲跌方向{MarketDirection(upDownPoint2 , rateDiff)}";
                  }
               }
               tmpStr += "。";
               decimal isZero = GetUpDown("F" , dr);
               if (dr["prod_subtype"].AsString() == "C" || isZero == 0) tmpStr += "▲▲▲";

               SetInnerText($"{++thridNode}. {tmpStr}" , true , 4.17f , 0.6f);
            }//foreach (DataRow dr in dtIndex.Rows) 
            #endregion

            #region 四
            string prep = dtIndex.Rows.Count == 1 ? "係因其" : "係因：";
            tmpStr = string.Format("({2}) 觀察{0}保證金變動幅度達10%，{1}" , GenArrayTxt(wfKindIdE(dtIndex)) , prep , ChineseNumber[++node]);

            //只有一筆資料時特殊處理
            if (dtIndex.Rows.Count != 1)
               SetInnerText(tmpStr , true , 4.11f , 1.25f);

            int forthNode = 0;

            foreach (DataRow dr in dtIndex.Rows) {
               string prodType = dr["prod_type"].AsString();

               string FOrN = prodType == "F" ? "期貨" : "現貨";

               //只有一筆資料時特殊處理
               if (dtIndex.Rows.Count != 1)
                  tmpStr = $"{++forthNode}. 近期{dr["kind_id_out"].AsString()}之{FOrN}指數";
               else
                  tmpStr += $"近期{dr["kind_id_out"].AsString()}之{FOrN}指數";

               DataRow drFind = dtIndex.AsEnumerable().Where(d => d.Field<string>("stock_id").AsString() == dr["stock_id"].AsString()).FirstOrDefault();
               if (drFind != null) {

                  decimal idValue = prodType == "F" ? drFind["m_up_down"].AsDecimal() :
                                       drFind["oth_up_down"].AsDecimal();

                  tmpStr += idValue > 0 ? "上漲" : "下跌";
               }

               tmpStr += "，且風險價格係數";
               decimal mDayRisk = dr["m_day_risk"].AsDecimal();
               decimal lastDayRisk = dr["last_risk"].AsDecimal();


               if (mDayRisk == lastDayRisk)
                  tmpStr += "變動幅度為0";
               else if (mDayRisk > lastDayRisk)
                  tmpStr += "上揚";
               else
                  tmpStr += "下降";

               tmpStr += $"，致{dr["kind_id_out"].AsString()}本日結算保證金隨之";

               if (dr["m_cm"].AsDecimal() > dr["cur_cm"].AsDecimal())
                  tmpStr += "提高。";
               else
                  tmpStr += "降低。";

               tmpStr += "▲▲▲";

               //只有一筆資料時特殊處理
               if (dtIndex.Rows.Count != 1)
                  SetInnerText(tmpStr , true , 4.17f , 0.6f);
               else
                  SetInnerText(tmpStr , true , 4.11f , 1.25f);
            }
            #endregion

            #region 五
            SetInnerText($"({ChineseNumber[++node]}) 原始保證金占契約總值比例與國際主要交易所比較：" , true , 4.11f , 1.25f);
            //特殊處理, 排除以下幾檔
            dtIndex = dtIndex.Select("kind_id <>'RTF' and kind_id <>'MXF' and kind_id <>'TGF' and prod_type <>'O'").CopyToDataTable();
            dtIndex = dtIndex.Sort("SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC, KIND_ID ASC, AB_TYPE ASC");
            int fifthNode = 0;
            foreach (DataRow dr in dtIndex.Rows) {
               string kindId = dr["kind_id"].AsString();

               DataRow drAbroad = DtAbroad.AsEnumerable().
                                     Where(a => a.Field<string>("kind_id").AsString() == kindId).FirstOrDefault();

               if (drAbroad != null) {
                  string str1 = GetSpot(kindId , "TAIFEX" , "cur");
                  string str2 = GetSpot(kindId , "TAIFEX" , "m");
                  tmpStr = dtIndex.Rows.Count > 0 ? $"{++fifthNode}. " : "  ";

                  tmpStr += $"現行本公司{kindId}原始保證金占契約總值比例{str1}，倘{checkedDate.AsTaiwanDateTime("{0}/{1}/{2}" , 3)}" +
                           $"依說明二調整，則本公司{kindId}原始保證金占契約總值比例{str2}。";

                  SetInnerText(tmpStr , true , 4.17f , 0.6f);

                  DrowCompareTableI(kindId);
               }
            }
            #endregion
         }

         /// <summary>
         /// 案由一 黃金說明
         /// </summary>
         /// <param name="dtGold"></param>
         /// <param name="descPoint">說明項數</param>
         /// <param name="checkedDate"></param>
         protected virtual void SetFirstCaseGoldDesc(DataTable dtGold , string descPoint , DateTime checkedDate) {
            string tmpStr = "";
            int node = 0;
            List<string> kindNameList = wfKindIdC(dtGold);

            List<string> adjRateList = new List<string>();
            List<string> lastAdjRateList = new List<string>();
            List<DataRow> tmpList = dtGold.AsEnumerable().ToList();
            tmpList.ForEach(l => adjRateList.Add(l.Field<double>("adj_rate").AsPercent(1)));
            tmpList.ForEach(l => lastAdjRateList.Add(l.Field<double>("last_adj_rate").AsPercent(1)));

            tmpStr = string.Format("{0}{1}，其結算保證金變動幅度{2}達{3}(前一營業日保證金變動幅度{2}達{4})，已達得調整標準百分比，" +
                                    "且進位後金額有變動，依下列考量因素說明:" , descPoint , GenArrayTxt(kindNameList) , SingleOrMore(dtGold) ,
                                    GenArrayTxt(adjRateList) , GenArrayTxt(lastAdjRateList));
            SetInnerText(tmpStr);

            //(一)
            kindNameList.Clear();
            tmpList.ForEach(l => kindNameList.Add(l.Field<string>("kind_id_out").AsString()));
            List<string> dayCntList = new List<string>();
            tmpList.ForEach(l => {
               if (l.Field<decimal>("day_cnt") == 0)
                  dayCntList.Add("第1天");
               else
                  dayCntList.Add($"第{l.Field<decimal>("day_cnt")}天");
            });


            tmpStr = string.Format("({3}) 保證金變動幅度達10%，且進位後金額有變動之天數：觀察{0}，{1}為{2}達調整標準。" ,
                                    GenArrayTxt(kindNameList) , SingleOrMore(dtGold) , GenArrayTxt(dayCntList) , ChineseNumber[++node]);
            SetInnerText(tmpStr , true , 4.11f , 1.25f);

            #region 二
            List<DateTime> deliveryDateList = new List<DateTime>();
            tmpList.ForEach(l => {
               if (!string.IsNullOrEmpty(l.Field<DateTime?>("i_mth_delivery_date").ToString())) {
                  if (!deliveryDateList.Exists(d => d == l.Field<DateTime>("i_mth_delivery_date")))
                     deliveryDateList.Add(l.Field<DateTime>("i_mth_delivery_date"));
               }
            });

            List<string> kindTypeList1 = new List<string>();// for type1
            List<string> kindTypeList2 = new List<string>();//for type2
            List<string> oiTypeList1 = new List<string>();// for type1
            List<string> oiTypeList2 = new List<string>();//for type2
            List<string> oiRateList = new List<string>();
            List<string> monthOiRateList = new List<string>();
            string settleMonth = "";
            foreach (DateTime deliveryDate in deliveryDateList) {
               DataTable dtDelivery = dtGold.Select("prod_subtype = 'C' and(kind_id = 'GDF' or kind_id = 'TGF') and " +
                                                           "i_mth_delivery_date ='" + deliveryDate + "'").CopyToDataTable();

               List<DataRow> drsDelivery = dtDelivery.AsEnumerable().ToList();
               DateTime isEndDate = PbFunc.f_get_ocf_next_n_day(deliveryDate , -7);

               if ((checkedDate < isEndDate) || (checkedDate >= deliveryDate)) {
                  //type1 參數
                  drsDelivery.ForEach(r => kindTypeList1.Add(r.Field<string>("kind_id_out").AsString()));
                  drsDelivery.ForEach(r => oiTypeList1.Add($"{r.Field<decimal>("i_oi").ToString("#,##0")}口"));

                  drsDelivery.ForEach(d => {
                     if (d.Field<decimal>("i_oi_rate").AsDouble() < 0.01 && d.Field<decimal>("i_oi") > 0)
                        oiRateList.Add("小於0.01%");
                     else
                        oiRateList.Add(Math.Round(d.Field<decimal>("i_oi_rate") , 2 , MidpointRounding.AwayFromZero).AsString() + "%");
                  });
               } else {
                  //type2 參數
                  drsDelivery.ForEach(r => kindTypeList2.Add(r.Field<string>("kind_id_out").AsString()));
                  drsDelivery.ForEach(r => oiTypeList2.Add($"{r.Field<decimal>("i_oi").ToString("#,##0")}口"));

                  drsDelivery.ForEach(r => monthOiRateList.Add($"{r.Field<decimal>("i_mth_oi").ToString("#,##0")}口"));
                  settleMonth = dtDelivery.Rows[0]["i_mth_settle_date"].AsDateTime("yyyyMM").Month.AsString();
               }
            }

            if (kindTypeList1.Count > 0) {
               //type1
               string single = kindTypeList1.Count > 0 ? "分別" : "";

               tmpStr = string.Format("({5}) 未沖銷部位數：{0} {1}未沖銷部位數{2}為{3}，占全市場未沖銷部位之比例{2}為{4}。" ,
                                       checkedDate.AsTaiwanDateTime("{0}/{1}/{2}" , 3) , GenArrayTxt(kindTypeList1) , single ,
                                       GenArrayTxt(oiTypeList1) , GenArrayTxt(oiRateList) , ChineseNumber[++node]);

            }
            if (kindTypeList2.Count > 0) {
               //type2
               tmpStr = string.Format("({5}) 臨屆契約到期日之未沖銷部位數：考量將屆{0}月份契約到期結算，未沖銷部位較高({1} {2}未沖銷部位數為" +
                                       "{3}，其中{0}月份契約為{4})，需考量到期結算前調整保證金對交易人及市場之影響。" ,
                                    settleMonth , checkedDate.AsTaiwanDateTime("{0}/{1}/{2}" , 3) ,
                                    GenArrayTxt(kindTypeList2) , GenArrayTxt(oiTypeList2) , GenArrayTxt(monthOiRateList) , ChineseNumber[++node]);
            }

            SetInnerText(tmpStr , true , 4.11f , 1.25f);

            dtGold = dtGold.Select("prod_subtype = 'C' and (kind_id = 'GDF' or kind_id='TGF')").CopyToDataTable();
            #endregion

            #region 三
            SetInnerText($"({ChineseNumber[++node]}) 現貨及期貨市場漲跌變化：" , true , 4.11f , 1.25f);
            int thridNode = 0;
            foreach (DataRow dr in dtGold.Rows) {
               string prodType = dr["prod_type"].AsString();

               if (prodType == "O") {
                  List<DataRow> drsF = dtGold.Select($"prod_type='F' and stock_id ='{dr["stock_id"].AsString()}'").ToList();
                  if (drsF.Count > 0) continue;
               }

               string kindIdOut = dr["kind_id_out"].AsString();
               tmpStr = $"{kindIdOut.SubStr(0 , 2)}現貨";

               decimal upDownPoint = GetUpDown("O" , dr);
               decimal upDownPoint2 = GetUpDown("F" , dr);
               decimal rateDiff = dr["m_cm"].AsDecimal() - dr["cur_cm"].AsDecimal();

               //現貨
               tmpStr += GenProdString("O" , dr);

               //期貨
               tmpStr += $"{kindIdOut.SubStr(0 , 2)}期貨";
               tmpStr += GenProdString("F" , dr);

               tmpStr = tmpStr.TrimEnd('、');
               tmpStr += "，";
               if (prodType == "F") {
                  DataRow drO = dtGold.Select($"prod_type='O' and stock_id ='{dr["stock_id"].AsString()}'").FirstOrDefault();
                  string warn = "";
                  if (drO != null) {
                     decimal rateDiff2 = drO["m_cm"].AsDecimal() - drO["cur_cm"].AsDecimal();
                     //保證金:+都漲,-都跌,x不同
                     if (rateDiff > 0 && rateDiff2 > 0)
                        warn = "+";
                     else if (rateDiff < 0 && rateDiff2 < 0)
                        warn = "-";
                     else
                        warn = "x";

                     //現貨&期貨相同
                     //期貨&選擇權保證金相同
                     if (warn == "+" || warn == "-" || upDownPoint == 0 || upDownPoint2 == 0) {
                        if (warn != "x") {
                           tmpStr += $"{kindIdOut}及{drO["kind_id_out"].AsString()}保證金調整之方向與現貨及期貨市場漲跌方向";
                           tmpStr += MarketDirectionWarn(upDownPoint , warn);

                        } //期貨&選擇權保證金不同
                        else {
                           tmpStr += $"{kindIdOut}保證金調整之方向與現貨及期貨市場漲跌方向";
                           tmpStr += MarketDirection(upDownPoint , rateDiff);
                           tmpStr += $"{drO["kind_id_out"].AsString()}保證金調整之方向與現貨及期貨市場漲跌方向";
                           tmpStr += MarketDirection(upDownPoint2 , rateDiff2);
                        }
                     } else {
                        //現貨&期貨不同
                        //期貨&選擇權保證金相同
                        if (warn != "x") {
                           tmpStr += $"{kindIdOut}及{drO["kind_id_out"].AsString()}保證金調整之方向與現貨及期貨市場漲跌方向";

                           tmpStr += MarketDirectionWarn(upDownPoint , warn);

                           if (upDownPoint2 != 0) tmpStr += $"，與期貨市場漲跌方向{MarketDirectionWarn(upDownPoint2 , warn)}";
                        } //期貨&選擇權保證金不同
                        else {
                           //FUT
                           tmpStr += $"{kindIdOut}保證金調整之方向與現貨及期貨市場漲跌方向";
                           tmpStr += MarketDirection(upDownPoint , rateDiff);

                           if (upDownPoint2 != 0) {
                              tmpStr = "；與期貨市場漲跌方向";
                              tmpStr += MarketDirection(upDownPoint2 , rateDiff);
                           }
                           //OPT
                           tmpStr += $"，{drO["kind_id_out"].AsString()}保證金調整之方向與現貨市場漲跌方向";
                           tmpStr += MarketDirection(upDownPoint , rateDiff2);

                           if (upDownPoint2 != 0) {
                              tmpStr = "；與期貨市場漲跌方向";
                              tmpStr += MarketDirection(upDownPoint2 , rateDiff2);
                           }
                        }
                     }
                  }//if (drO != null)
                   //無選擇權狀況
                   //現貨&期貨相同
                  if ((upDownPoint > 0 && upDownPoint2 > 0) || (upDownPoint < 0 && upDownPoint2 < 0)
                        || upDownPoint == 0 || upDownPoint2 == 0) {
                     tmpStr += $"{kindIdOut}保證金調整之方向與現貨及期貨市場漲跌方向{MarketDirection(upDownPoint , rateDiff)}";
                  } //現貨&期貨不同	
                  else {
                     tmpStr += $"{kindIdOut}保證金調整之方向與現貨及期貨市場漲跌方向{MarketDirection(upDownPoint , rateDiff)}";
                     tmpStr += $"；與期貨市場漲跌方向{MarketDirection(upDownPoint2 , rateDiff)}";
                  }
               } //if(prodType == "F")
               else {
                  //單一狀況
                  //現貨&期貨相同
                  if ((upDownPoint > 0 && upDownPoint2 > 0) || (upDownPoint < 0 && upDownPoint2 < 0)
                        || upDownPoint == 0 || upDownPoint2 == 0) {
                     tmpStr += $"{kindIdOut}保證金調整之方向與現貨及期貨市場漲跌方向{MarketDirection(upDownPoint , rateDiff)}";
                  } //現貨&期貨不同	
                  else {
                     tmpStr += $"{kindIdOut}保證金調整之方向與現貨及期貨市場漲跌方向{MarketDirection(upDownPoint , rateDiff)}";
                     tmpStr += $"；與期貨市場漲跌方向{MarketDirection(upDownPoint2 , rateDiff)}";
                  }
               }
               tmpStr += "。";
               decimal isZero = GetUpDown("F" , dr);
               if (dr["prod_subtype"].AsString() == "C" || isZero == 0) tmpStr += "▲▲▲";

               SetInnerText($"{++thridNode}. {tmpStr}" , true , 4.17f , 0.6f);
            }//foreach (DataRow dr in dtIndex.Rows) 
            #endregion

            #region 四
            string prep = dtGold.Rows.Count == 1 ? "係因其" : "係因: ";
            tmpStr = string.Format("({2}) 觀察{0}保證金變動幅度達10%，{1}" , GenArrayTxt(wfKindIdE(dtGold)) , prep , ChineseNumber[++node]);
            SetInnerText(tmpStr , true , 4.11f , 1.25f);
            int forthNode = 1;

            foreach (DataRow dr in dtGold.Rows) {
               string prodType = dr["prod_type"].AsString();

               string FOrN = prodType == "F" ? "期貨" : "現貨";

               tmpStr = $"{forthNode}. 近期{dr["kind_id_out"].AsString()}之{FOrN}指數";

               DataRow drFind = dtGold.AsEnumerable().Where(d => d.Field<string>("stock_id").AsString() == dr["stock_id"].AsString()).FirstOrDefault();
               if (drFind != null) {

                  decimal idValue = prodType == "F" ? drFind["m_up_down"].AsDecimal() :
                                       drFind["oth_up_down"].AsDecimal();

                  tmpStr += idValue > 0 ? "上漲" : "下跌";
               }

               tmpStr += "，且風險價格係數";
               decimal mDayRisk = dr["m_day_risk"].AsDecimal();
               decimal lastDayRisk = dr["last_risk"].AsDecimal();

               if (mDayRisk == lastDayRisk)
                  tmpStr += "變動幅度為0";
               else if (mDayRisk > lastDayRisk)
                  tmpStr += "上揚";
               else
                  tmpStr += "下降";

               tmpStr += $"，致{dr["kind_id_out"].AsString()}本日結算保證金隨之";

               if (dr["m_cm"].AsDecimal() > dr["cur_cm"].AsDecimal())
                  tmpStr += "提高。";
               else
                  tmpStr += "降低。";

               tmpStr += "▲▲▲";

               SetInnerText(tmpStr , true , 4.17f , 0.6f);

               forthNode++;
            }
            #endregion

            #region 五
            SetInnerText($"({ChineseNumber[++node]}) 結算保證金占契約總值比例與國際主要交易所比較：" , true , 4.11f , 1.25f);
            //特殊處理, 排除以下幾檔
            dtGold = dtGold.Select("kind_id <>'RTF' and kind_id <>'MXF' and kind_id <>'TGF' and prod_type <>'O'").CopyToDataTable();
            foreach (DataRow dr in dtGold.Rows) {
               string kindId = dr["kind_id"].AsString();

               DataRow drAbroad = DtAbroad.AsEnumerable().
                                     Where(a => a.Field<string>("kind_id").AsString() == kindId).FirstOrDefault();

               if (drAbroad != null) {

                  kindId = "GOLD";//不知原因

                  string str1 = GetSpot("GDF" , "TAIFEX" , "cur");
                  string str2 = GetSpot("TGF" , "TAIFEX" , "cur");
                  tmpStr = $"1. {WfCampreGold(str1 , str2)}";

                  SetInnerText(tmpStr , true , 4.17f , 0.6f);

                  str1 = GetSpot("GDF" , "TAIFEX" , "m");
                  str2 = GetSpot("TGF" , "TAIFEX" , "m");
                  tmpStr = $"2. 倘{checkedDate.AsTaiwanDateTime("{0}/{1}/{2}" , 3)}依說明二調整，則{WfCampreGold(str1 , str2)}。";
                  SetInnerText(tmpStr , true , 4.17f , 0.6f);
                  DrowCompareTableE(kindId);
               }
            }
            #endregion

         }

         /// <summary>
         /// 案由一 BRF 說明
         /// </summary>
         /// <param name="dtBRF"></param>
         /// <param name="descPoint">說明項數</param>
         /// <param name="checkedDate"></param>
         protected virtual void SetFirstCaseBRFDesc(DataTable dtBRF , string descPoint , DateTime checkedDate) {
            string tmpStr = "";
            int node = 0;
            List<string> kindNameList = wfKindIdC(dtBRF);

            List<string> adjRateList = new List<string>();
            List<string> lastAdjRateList = new List<string>();
            List<DataRow> tmpList = dtBRF.AsEnumerable().ToList();
            tmpList.ForEach(l => adjRateList.Add(l.Field<double?>("adj_rate").AsPercent(1)));
            tmpList.ForEach(l => lastAdjRateList.Add(l.Field<double?>("last_adj_rate").AsPercent(1)));

            tmpStr = string.Format("{0}{1}，其結算保證金變動幅度{2}達{3}(前一營業日保證金變動幅度{2}達{4})，已達得調整標準百分比，" +
                                    "且進位後金額有變動，依下列考量因素說明:" , descPoint , GenArrayTxt(kindNameList) , SingleOrMore(dtBRF) ,
                                    GenArrayTxt(adjRateList) , GenArrayTxt(lastAdjRateList));
            SetInnerText(tmpStr);

            //(一)
            kindNameList.Clear();
            tmpList.ForEach(l => kindNameList.Add(l.Field<string>("kind_id_out").AsString()));
            List<string> dayCntList = new List<string>();
            tmpList.ForEach(l => {
               if (l.Field<decimal>("day_cnt") == 0)
                  dayCntList.Add("第1天");
               else
                  dayCntList.Add($"第{l.Field<decimal>("day_cnt")}天");
            });


            tmpStr = string.Format("({3}) 保證金變動幅度達10%，且進位後金額有變動之天數：觀察{0}，{1}為{2}達調整標準。" ,
                                    GenArrayTxt(kindNameList) , SingleOrMore(dtBRF) , GenArrayTxt(dayCntList) , ChineseNumber[++node]);
            SetInnerText(tmpStr , true , 4.11f , 1.25f);

            #region 二
            List<DateTime> deliveryDateList = new List<DateTime>();
            tmpList.ForEach(l => {
               if (!string.IsNullOrEmpty(l.Field<DateTime?>("i_mth_delivery_date").ToString())) {
                  if (!deliveryDateList.Exists(d => d == l.Field<DateTime>("i_mth_delivery_date")))
                     deliveryDateList.Add(l.Field<DateTime>("i_mth_delivery_date"));
               }
            });

            List<string> kindTypeList1 = new List<string>();// for type1
            List<string> kindTypeList2 = new List<string>();//for type2
            List<string> oiTypeList1 = new List<string>();// for type1
            List<string> oiTypeList2 = new List<string>();//for type2
            List<string> oiRateList = new List<string>();
            List<string> monthOiRateList = new List<string>();
            string settleMonth = "";
            foreach (DateTime deliveryDate in deliveryDateList) {
               DataTable dtDelivery = dtBRF.Select("prod_subtype = 'C' and kind_id = 'BRF' and " +
                                                           "i_mth_delivery_date ='" + deliveryDate + "'").CopyToDataTable();

               List<DataRow> drsDelivery = dtDelivery.AsEnumerable().ToList();
               DateTime isEndDate = PbFunc.f_get_ocf_next_n_day(deliveryDate , -7);

               if ((checkedDate < isEndDate) || (checkedDate >= deliveryDate)) {
                  //type1 參數
                  drsDelivery.ForEach(r => kindTypeList1.Add(r.Field<string>("kind_id_out").AsString()));
                  drsDelivery.ForEach(r => oiTypeList1.Add($"{r.Field<decimal>("i_oi").ToString("#,##0")}口"));

                  drsDelivery.ForEach(d => {
                     if (d.Field<decimal>("i_oi_rate").AsDouble() < 0.01 && d.Field<decimal>("i_oi") > 0)
                        oiRateList.Add("小於0.01%");
                     else
                        oiRateList.Add(Math.Round(d.Field<decimal>("i_oi_rate") , 2 , MidpointRounding.AwayFromZero).AsString() + "%");
                  });
               } else {
                  //type2 參數
                  drsDelivery.ForEach(r => kindTypeList2.Add(r.Field<string>("kind_id_out").AsString()));
                  drsDelivery.ForEach(r => oiTypeList2.Add($"{r.Field<decimal>("i_oi").ToString("#,##0")}口"));

                  drsDelivery.ForEach(r => monthOiRateList.Add($"{r.Field<decimal>("i_mth_oi").ToString("#,##0")}口"));
                  settleMonth = dtDelivery.Rows[0]["i_mth_settle_date"].AsDateTime("yyyyMM").Month.AsString();
               }
            }

            if (kindTypeList1.Count > 0) {
               //type1
               string single = kindTypeList1.Count > 0 ? "分別" : "";

               tmpStr = string.Format("({5}) 未沖銷部位數：{0} {1}未沖銷部位數{2}為{3}，占全市場未沖銷部位之比例{2}為{4}。" ,
                                       checkedDate.AsTaiwanDateTime("{0}/{1}/{2}" , 3) , GenArrayTxt(kindTypeList1) , single ,
                                       GenArrayTxt(oiTypeList1) , GenArrayTxt(oiRateList) , ChineseNumber[++node]);

            }
            if (kindTypeList2.Count > 0) {
               //type2
               tmpStr = string.Format("({5}) 臨屆契約到期日之未沖銷部位數：考量將屆{0}月份契約到期結算，未沖銷部位較高({1} {2}未沖銷部位數為" +
                                       "{3}，其中{0}月份契約為{4})，需考量到期結算前調整保證金對交易人及市場之影響。" ,
                                    settleMonth , checkedDate.AsTaiwanDateTime("{0}/{1}/{2}" , 3) ,
                                    GenArrayTxt(kindTypeList2) , GenArrayTxt(oiTypeList2) , GenArrayTxt(monthOiRateList) , ChineseNumber[++node]);
            }

            SetInnerText(tmpStr , true , 4.11f , 1.25f);

            dtBRF = dtBRF.Select("prod_subtype = 'C' and kind_id = 'BRF'").CopyToDataTable();
            #endregion

            #region 三
            string prep = dtBRF.Rows.Count == 1 ? "係因其" : "係因: ";
            tmpStr = string.Format("({2}) 觀察{0}保證金變動幅度達10%，{1}" , GenArrayTxt(wfKindIdE(dtBRF)) , prep , ChineseNumber[++node]);
            if (dtBRF.Rows.Count > 1) SetInnerText(tmpStr , true , 4.11f , 1.25f);//超過一筆時要有小點
            int thirdNode = 1;

            foreach (DataRow dr in dtBRF.Rows) {
               string prodType = dr["prod_type"].AsString();

               string FOrN = prodType == "F" ? "期貨" : "現貨";
               if (dtBRF.Rows.Count > 1)
                  tmpStr = $"{thirdNode}. 近期{dr["kind_id_out"].AsString()}之{FOrN}指數";
               else
                  tmpStr += $"近期{dr["kind_id_out"].AsString()}之{FOrN}指數";

               DataRow drFind = dtBRF.AsEnumerable().Where(d => d.Field<string>("stock_id").AsString() == dr["stock_id"].AsString()).FirstOrDefault();
               if (drFind != null) {

                  decimal idValue = prodType == "F" ? drFind["m_up_down"].AsDecimal() :
                                       drFind["oth_up_down"].AsDecimal();

                  tmpStr += idValue > 0 ? "上漲" : "下跌";
               }

               tmpStr += "，且風險價格係數";
               decimal mDayRisk = dr["m_day_risk"].AsDecimal();
               decimal lastDayRisk = dr["last_risk"].AsDecimal();

               if (mDayRisk == lastDayRisk)
                  tmpStr += "變動幅度為0";
               else if (mDayRisk > lastDayRisk)
                  tmpStr += "上揚";
               else
                  tmpStr += "下降";

               tmpStr += $"，致{dr["kind_id_out"].AsString()}本日結算保證金隨之";

               if (dr["m_cm"].AsDecimal() > dr["cur_cm"].AsDecimal())
                  tmpStr += "提高。";
               else
                  tmpStr += "降低。";

               //tmpStr += "▲▲▲";

               if (dtBRF.Rows.Count > 1)
                  SetInnerText(tmpStr , true , 4.17f , 0.6f);
               else
                  SetInnerText(tmpStr , true , 4.11f , 1.25f);

               node++;
            }
            #endregion

            #region 四
            SetInnerText($"({ChineseNumber[++node]}) 結算保證金占契約總值比例與國際主要交易所比較：" , true , 4.11f , 1.25f);
            //特殊處理, 排除以下幾檔
            dtBRF = dtBRF.Select("kind_id <>'RTF' and kind_id <>'MXF' and kind_id <>'TGF' and prod_type <>'O'").CopyToDataTable();
            foreach (DataRow dr in dtBRF.Rows) {
               string kindId = dr["kind_id"].AsString();

               DataRow drAbroad = DtAbroad.AsEnumerable().
                                     Where(a => a.Field<string>("kind_id").AsString() == kindId).FirstOrDefault();

               if (drAbroad != null) {
                  string str1 = GetSpot(kindId , "TAIFEX" , "cur");
                  string str2 = GetSpot(kindId , "TAIFEX" , "m");

                  tmpStr = $"  現行本公司{kindId}結算保證金占契約總值比例{str1}，倘{checkedDate.AsTaiwanDateTime("{0}/{1}/{2}" , 3)}" +
                           $"依說明二調整，則本公司{kindId}結算保證金占契約總值比例{str2}。";

                  SetInnerText(tmpStr , true , 4.17f , 0.6f);

                  DrowCompareTableI(kindId);
               }

            }
            #endregion
         }

         /// <summary>
         /// 案由一 最後說明
         /// </summary>
         /// <param name="dtTemp"></param>
         /// <param name="descPoint">說明項數</param>
         /// <param name="checkedDate"></param>
         protected virtual void SetFirstCaseLastDesc(DataTable dtTemp , string descPoint , DateTime checkedDate) {
            int point = 0;
            List<DataRow> drsTemp = new List<DataRow>();
            string tmpStr = "";
            string checkedDateStr = checkedDate.ToString("yyyyMMdd");

            SetInnerText($"{descPoint}綜上，經考量市場風險，建議如下：");

            //(一)
            drsTemp = dtTemp.Select("prod_subtype = 'B' and data_ymd = '" + checkedDateStr + "'").ToList();
            if (drsTemp.Count > 0) {
               SetInnerText($"({ChineseNumber[++point]}) 不調整GBF保證金。" , true , 4.11f , 1.25f);
            }

            drsTemp.Clear();

            //(二)
            //調整
            drsTemp = dtTemp.Select("prod_subtype in ('I','C') and adj_code = 'Y' and data_ymd = '" + checkedDateStr + "'").ToList();
            if (drsTemp.Count > 0) {
               SetInnerText($"({ChineseNumber[++point]}) {GenArrayTxt(wfKindIdE(drsTemp.CopyToDataTable()))}" +
                              $"保證金已達得調整百分比，經考量市場風險，基於穩健保守之原則，建議調整如說明二。" , true , 4.11f , 1.25f);
            }
            drsTemp.Clear();

            //觀察
            drsTemp = dtTemp.Select("prod_subtype in ('I','C') and adj_code = ' ' and data_ymd = '" + checkedDateStr + "'").ToList();
            if (drsTemp.Count > 0) {
               drsTemp = dtTemp.Select("prod_subtype in ('I','C') and adj_rate < 0 and adj_code = ' ' " +
                                          "and data_ymd = '" + checkedDateStr + "'").ToList();
               if (drsTemp.Count > 0) {
                  DateTime obsDate = PbFunc.f_get_ocf_next_n_day(checkedDate , 10);
                  tmpStr = string.Format("({0}) 鑒於全球金融市場不確定性較高，基於市場風險控管穩健原則，" +
                                         "建議暫不調整{1}保證金，再觀察▲▲10個交易日至(▲▲{2})。" ,
                                         ChineseNumber[++point] , GenArrayTxt(wfKindIdE(drsTemp.CopyToDataTable().Sort("SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC, KIND_ID ASC, AB_TYPE ASC"))) ,
                                         obsDate.AsTaiwanDateTime("{0}/{1}/{2}" , 3));

                  SetInnerText(tmpStr , true , 4.11f , 1.25f);
               }

               drsTemp.Clear();
               drsTemp = dtTemp.Select("prod_subtype in ('I','C') and adj_rate > 0 and adj_code = ' ' and m_cp_risk < m_min_risk " +
                                          "and data_ymd = '" + checkedDateStr + "'").ToList();
               if (drsTemp.Count > 0) {
                  if (drsTemp.Where(r => r.Field<string>("kind_id").AsString() == "TXF").FirstOrDefault() != null) {
                     tmpStr = $"({ChineseNumber[++point]}) 以原始保證金計算期貨契約槓桿倍數，" +
                                 $"新交所摩臺指期貨之槓桿倍數為30.00倍，現行本公司臺指期貨之槓桿倍數為23.81倍，" +
                                 $"依說明二調高保證金後本公司之槓桿倍數為20.81，較新交所為低。";

                     SetInnerText(tmpStr , true , 4.11f , 1.25f);
                  }

                  List<string> riskList = new List<string>();
                  drsTemp.ForEach(r => riskList.Add(r.Field<double>("m_cp_risk").AsPercent(2)));

                  List<string> minRiskList = new List<string>();
                  //指數類
                  drsTemp.Clear();
                  drsTemp = dtTemp.Select("prod_subtype= 'I' and adj_rate > 0 and adj_code = ' ' " +
                                             "and data_ymd = '" + checkedDateStr + "'").ToList();
                  if (drsTemp.Count > 0) {
                     drsTemp.ForEach(r => minRiskList.Add($"指數類契約為{r.Field<double>("m_min_risk").AsPercent(1)}"));
                  }
                  //黃金類
                  drsTemp.Clear();
                  drsTemp = dtTemp.Select("prod_subtype= 'C' and adj_rate > 0 and adj_code = ' ' and (kind_id = 'GDF' or kind_id = 'TGF') " +
                                             "and data_ymd = '" + checkedDateStr + "'").ToList();
                  if (drsTemp.Count > 0) {
                     drsTemp.ForEach(r => minRiskList.Add($"黃金類契約為{r.Field<double>("m_min_risk").AsPercent(1)}"));
                  }
                  //原油類
                  drsTemp.Clear();
                  drsTemp = dtTemp.Select("prod_subtype= 'C' and adj_rate > 0 and adj_code = ' ' and kind_id = 'BRF' " +
                                             "and data_ymd = '" + checkedDateStr + "'").ToList();
                  if (drsTemp.Count > 0) {
                     drsTemp.ForEach(r => minRiskList.Add($"原油類契約為{r.Field<double>("m_min_risk").AsPercent(1)}"));
                  }
                  
                  tmpStr = string.Format("({0}) 本公司{1}之保證金變動幅度達10%，主係因期貨或現貨指數近期大幅上漲所致，" +
                        "然前揭契約之實際風險價格係數近期未見有明顯擴大之情事，且{2}之實際風險價格係數分別為{3}，仍較現行之最小風險價格係數({4})為低sk。" ,
                        ChineseNumber[++point] , GenArrayTxt(wfKindIdE(dtTemp)) , checkedDate.AsTaiwanDateTime("{0}/{1}/{2}" , 3) ,
                        GenArrayTxt(riskList) , GenArrayTxt(minRiskList));
                        
                  SetInnerText(tmpStr , true , 4.11f , 1.25f);
               }
            }//if (drsTemp.Count > 0)

            drsTemp.Clear();
            drsTemp = dtTemp.Select("prod_subtype in ('I','C') and adj_rate > 0 and adj_code = ' ' and m_cp_risk >= m_min_risk " +
                                    "and data_ymd = '" + checkedDateStr + "'").ToList();
            if (drsTemp.Count > 0) {
               tmpStr = $"({ChineseNumber[++point]}) 本公司{GenArrayTxt(wfKindIdE(drsTemp.CopyToDataTable().Sort("SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC, KIND_ID ASC, AB_TYPE ASC")))}之保證金變動幅度達10%，" +
                        $"主係因期貨或現貨指數近期大幅上漲所致▲▲▲，然前揭契約之實際風險價格係數近期未見有明顯擴大之情事。";

               SetInnerText(tmpStr , true , 4.11f , 1.25f);
            }
         }

         /// <summary>
         /// 案由二 說明文
         /// </summary>
         /// <param name="dtTemp"></param>
         /// <param name="checkedDate"></param>
         protected virtual void SetSecondCaseDesc(DataTable dtTemp , DateTime checkedDate) {
            string tmpStr = "";

            SetSubjectText($"說　　明：");

            //說明一
            tmpStr = string.Format("一、{0}本公司上開契約結算保證金之變動幅度已達得調整標準之百分比，且進位後金額有變動時，依本公司保證金調整作業規範，" +
                                   "由督導結算業務主管召集業務相關部門主管會商決定是否調整。" ,
                                    dtTemp.Rows[0]["data_ymd"].AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}/{1}/{2}" , 3));
            SetInnerText(tmpStr);

            //說明二
            SetInnerText("二、本次保證金倘經調整，其金額變動如下：");
            DrowTable(dtTemp);

            //說明三
            SetSecondCaseThirdDesc(dtTemp , "三、" , checkedDate);

            //說明四
            SetSecondLastDesc(dtTemp , "四、" , checkedDate);
         }

         /// <summary>
         /// 案由二 第三項說明
         /// </summary>
         /// <param name="dtTemp"></param>
         /// <param name="descPoint">說明項數</param>
         /// <param name="checkedDate"></param>
         protected virtual void SetSecondCaseThirdDesc(DataTable dtTemp , string descPoint , DateTime checkedDate) {
            string tmpStr = "";
            List<string> kindNameList = wfKindIdC(dtTemp);

            List<string> adjRateList = new List<string>();
            List<string> lastAdjRateList = new List<string>();
            List<DataRow> tmpList = dtTemp.AsEnumerable().ToList();
            tmpList.ForEach(l => adjRateList.Add(l.Field<double>("adj_rate").AsPercent(1)));
            tmpList.ForEach(l => lastAdjRateList.Add(l.Field<double>("last_adj_rate").AsPercent(1)));

            tmpStr = string.Format("{0}{1}，其結算保證金變動幅度{2}達{3}(前一營業日保證金變動幅度{2}達{4})，已達得調整標準百分比，" +
                                    "且進位後金額有變動，依下列考量因素說明:" , descPoint , GenArrayTxt(kindNameList) , SingleOrMore(dtTemp) ,
                                    GenArrayTxt(adjRateList) , GenArrayTxt(lastAdjRateList));
            SetInnerText(tmpStr);

            //(一)
            kindNameList.Clear();
            tmpList.ForEach(l => kindNameList.Add(l.Field<string>("kind_id_out").AsString()));
            List<string> dayCntList = new List<string>();
            tmpList.ForEach(l => {
               if (l.Field<decimal>("day_cnt") == 0)
                  dayCntList.Add("第1天");
               else
                  dayCntList.Add($"第{l.Field<decimal>("day_cnt")}天");
            });


            tmpStr = string.Format("(一) 保證金變動幅度達5%，且進位後金額有變動之天數：觀察{0}，{1}為{2}達調整標準。" ,
                                    GenArrayTxt(kindNameList) , SingleOrMore(dtTemp) , GenArrayTxt(dayCntList));
            SetInnerText(tmpStr , true , 4.11f , 1.25f);

            //(二)
            List<DateTime> deliveryDateList = new List<DateTime>();
            tmpList.ForEach(l => {
               if (!string.IsNullOrEmpty(l.Field<DateTime?>("i_mth_delivery_date").ToString())) {
                  if (!deliveryDateList.Exists(d => d == l.Field<DateTime>("i_mth_delivery_date")))
                     deliveryDateList.Add(l.Field<DateTime>("i_mth_delivery_date"));
               }
            });

            foreach (DateTime deliveryDate in deliveryDateList) {
               List<DataRow> drsDelivery = dtTemp.Select("prod_subtype = 'E' and kind_id <> 'MXF' and i_mth_delivery_date = " +
                                                      "'" + deliveryDate + "'").ToList();
               if (drsDelivery.Count <= 0) continue;

               DataTable dtDelivery = drsDelivery.CopyToDataTable();
               dtDelivery = dtDelivery.Sort("SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC, KIND_ID ASC, AB_TYPE ASC");

               drsDelivery = dtDelivery.AsEnumerable().ToList();
               DateTime isEndDate = PbFunc.f_get_ocf_next_n_day(deliveryDate , -7);
               List<string> kindTypeList = new List<string>();
               List<string> oiTypeList = new List<string>();
               List<string> oiRateList = new List<string>();
               List<string> monthOiRateList = new List<string>();

               drsDelivery.ForEach(r => kindTypeList.Add(r.Field<string>("kind_id_out").AsString()));
               drsDelivery.ForEach(r => oiTypeList.Add($"{r.Field<decimal>("i_oi").ToString("#,##0")}口"));

               if (checkedDate < isEndDate || checkedDate >= deliveryDate.AsDateTime("yyyyMMdd")) {
                  //type1
                  drsDelivery.ForEach(d => {
                     if (d.Field<decimal>("i_oi_rate").AsDouble() < 0.01 && d.Field<decimal>("i_oi") > 0)
                        oiRateList.Add("小於0.01%");
                     else
                        oiRateList.Add(d.Field<decimal>("i_oi_rate").ToString("#,##0.##") + "%");
                  });

                  tmpStr = string.Format("(二) 未沖銷部位數：{0} {1}未沖銷部位數{2}為{3}，占全市場未沖銷部位之比例{2}為{4}。" ,
                                          checkedDate.AsTaiwanDateTime("{0}/{1}/{2}" , 3) , GenArrayTxt(kindTypeList) , SingleOrMore(dtDelivery) ,
                                          GenArrayTxt(oiTypeList) , GenArrayTxt(oiRateList));

               } else {
                  //type2
                  drsDelivery.ForEach(r => monthOiRateList.Add($"{r.Field<decimal>("i_mth_oi").ToString("#,##0")}口"));
                  string settleMonth = dtDelivery.Rows[0]["i_mth_settle_date"].AsDateTime("yyyyMMdd").Month.AsString();

                  tmpStr = string.Format("(二) 臨屆契約到期日之未沖銷部位數：考量將屆{0}月份契約到期結算，未沖銷部位較高({1} {2}未沖銷部位數為" +
                                       "{3}，其中{0}月份契約為{4})，需考量到期結算前調整保證金對交易人及市場之影響。" ,
                                       settleMonth , checkedDate.AsTaiwanDateTime("{0}/{1}/{2}" , 3) ,
                                       GenArrayTxt(kindTypeList) , GenArrayTxt(oiTypeList) , GenArrayTxt(monthOiRateList));
               }

               SetInnerText(tmpStr , true , 4.11f , 1.25f);

               dtTemp = dtTemp.Select("prod_subtype='E' and kind_id<>'MXF'").CopyToDataTable();
               dtTemp = dtTemp.Sort("SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC, KIND_ID ASC, AB_TYPE ASC");

               #region 三
               SetInnerText("(三) 現貨及期貨市場漲跌變化：" , true , 4.11f , 1.25f);
               int thridNode = 0;
               foreach (DataRow dr in dtTemp.Rows) {
                  string prodType = dr["prod_type"].AsString();

                  if (prodType == "O") {
                     List<DataRow> drsF = dtTemp.Select($"prod_type='F' and stock_id ='{dr["stock_id"].AsString()}'").ToList();
                     if (drsF.Count > 0) continue;
                  }

                  string kindIdOut = dr["kind_id_out"].AsString();
                  tmpStr = $"{kindIdOut.SubStr(0 , 2)}現貨";

                  decimal upDownPoint = GetUpDown("O" , dr);
                  decimal upDownPoint2 = GetUpDown("F" , dr);
                  decimal rateDiff = dr["m_cm"].AsDecimal() - dr["cur_cm"].AsDecimal();

                  //現貨
                  tmpStr += GenProdString("O" , dr);

                  //期貨
                  tmpStr += $"{kindIdOut.SubStr(0 , 2)}期貨";
                  tmpStr += GenProdString("F" , dr);

                  tmpStr = tmpStr.TrimEnd('、');
                  tmpStr += "，";
                  if (prodType == "F") {
                     DataRow drO = dtTemp.Select($"prod_type='O' and stock_id ='{dr["stock_id"].AsString()}'").FirstOrDefault();
                     string warn = "";
                     if (drO != null) {
                        decimal rateDiff2 = drO["m_cm"].AsDecimal() - drO["cur_cm"].AsDecimal();
                        //保證金:+都漲,-都跌,x不同
                        if (rateDiff > 0 && rateDiff2 > 0)
                           warn = "+";
                        else if (rateDiff < 0 && rateDiff2 < 0)
                           warn = "-";
                        else
                           warn = "x";

                        //現貨&期貨相同
                        //期貨&選擇權保證金相同
                        if (upDownPoint == upDownPoint2 || upDownPoint == 0 || upDownPoint2 == 0) {
                           if (warn != "x") {
                              tmpStr += $"{kindIdOut}及{drO["kind_id_out"].AsString()}保證金調整之方向與現貨及期貨市場漲跌方向";
                              tmpStr += MarketDirectionWarn(upDownPoint , warn);

                           } //期貨&選擇權保證金不同
                           else {
                              tmpStr += $"{kindIdOut}保證金調整之方向與現貨及期貨市場漲跌方向";
                              tmpStr += MarketDirection(upDownPoint , rateDiff);
                              tmpStr += $"{drO["kind_id_out"].AsString()}保證金調整之方向與現貨及期貨市場漲跌方向";
                              tmpStr += MarketDirection(upDownPoint2 , rateDiff2);
                           }
                        } else {
                           //現貨&期貨不同
                           //期貨&選擇權保證金相同
                           if (warn != "x") {
                              tmpStr += $"{kindIdOut}及{drO["kind_id_out"].AsString()}保證金調整之方向與現貨市場漲跌方向";

                              tmpStr += MarketDirectionWarn(upDownPoint , warn);

                              if (upDownPoint2 != 0) tmpStr += $"，與期貨市場漲跌方向{MarketDirectionWarn(upDownPoint2 , warn)}";
                           } //期貨&選擇權保證金不同
                           else {
                              //FUT
                              tmpStr += $"{kindIdOut}保證金調整之方向與現貨市場漲跌方向";
                              tmpStr += MarketDirection(upDownPoint , rateDiff);

                              if (upDownPoint2 != 0) {
                                 tmpStr = "；與期貨市場漲跌方向";
                                 tmpStr += MarketDirection(upDownPoint2 , rateDiff);
                              }
                              //OPT
                              tmpStr += $"，{drO["kind_id_out"].AsString()}保證金調整之方向與現貨市場漲跌方向";
                              tmpStr += MarketDirection(upDownPoint , rateDiff2);

                              if (upDownPoint2 != 0) {
                                 tmpStr = "；與期貨市場漲跌方向";
                                 tmpStr += MarketDirection(upDownPoint2 , rateDiff2);
                              }
                           }
                        }
                        SetInnerText($"{++thridNode}. {tmpStr}" , true , 4.17f , 0.6f);
                        continue;
                     }//if (drO != null)
                      //無選擇權狀況
                      //現貨&期貨相同
                     if ((upDownPoint > 0 && upDownPoint2 > 0) || (upDownPoint < 0 && upDownPoint2 < 0)
                           || upDownPoint == 0 || upDownPoint2 == 0) {
                        tmpStr += $"{kindIdOut}保證金調整之方向與現貨及期貨市場漲跌方向{MarketDirection(upDownPoint , rateDiff)}";
                     } //現貨&期貨不同	
                     else {
                        tmpStr += $"{kindIdOut}保證金調整之方向與現貨市場漲跌方向{MarketDirection(upDownPoint , rateDiff)}";
                        tmpStr += $"；與期貨市場漲跌方向{MarketDirection(upDownPoint2 , rateDiff)}";
                     }
                  } //if(prodType == "F")
                  else {
                     //單一狀況
                     //現貨&期貨相同
                     if ((upDownPoint > 0 && upDownPoint2 > 0) || (upDownPoint < 0 && upDownPoint2 < 0)
                           || upDownPoint == 0 || upDownPoint2 == 0) {
                        tmpStr += $"{kindIdOut}保證金調整之方向與現貨及期貨市場漲跌方向{MarketDirection(upDownPoint , rateDiff)}";
                     } //現貨&期貨不同	
                     else {
                        tmpStr += $"{kindIdOut}保證金調整之方向與現貨市場漲跌方向{MarketDirection(upDownPoint , rateDiff)}";
                        tmpStr += $"；與期貨市場漲跌方向{MarketDirection(upDownPoint2 , rateDiff)}";
                     }
                  }
                  tmpStr += "。";
                  decimal isZero = GetUpDown("F" , dr);
                  if (dr["prod_subtype"].AsString() == "C" || isZero == 0) tmpStr += "▲▲▲";

                  SetInnerText($"{++thridNode}. {tmpStr}" , true , 4.17f , 0.6f);
               }//foreach (DataRow dr in dtIndex.Rows) 
               #endregion

               #region 四
               string prep = dtTemp.Rows.Count == 1 ? "係因其" : "係因：";
               tmpStr = string.Format("(四) 觀察{0}保證金變動幅度達5%，{1}" , GenArrayTxt(wfKindIdE(dtTemp)) , prep);

               if (dtTemp.Rows.Count > 1)
                  SetInnerText(tmpStr , true , 4.11f , 1.25f);

               int node = 1;

               foreach (DataRow dr in dtTemp.Rows) {
                  string prodType = dr["prod_type"].AsString();

                  string FOrN = prodType == "F" ? "期貨" : "現貨";

                  if (dtTemp.Rows.Count > 1)
                     tmpStr = $"{node}. 近期{dr["kind_id_out"].AsString()}之{FOrN}指數";
                  else
                     tmpStr += $"近期{dr["kind_id_out"].AsString()}之{FOrN}指數";

                  DataRow drFind = dtTemp.AsEnumerable().Where(d => d.Field<string>("stock_id").AsString() == dr["stock_id"].AsString()).FirstOrDefault();
                  if (drFind != null) {

                     decimal idValue = prodType == "F" ? drFind["m_up_down"].AsDecimal() :
                                          drFind["oth_up_down"].AsDecimal();

                     tmpStr += idValue > 0 ? "上漲" : "下跌";
                  }

                  tmpStr += "，且風險價格係數";
                  decimal mDayRisk = dr["m_day_risk"].AsDecimal();
                  decimal lastDayRisk = dr["last_risk"].AsDecimal();


                  if (mDayRisk == lastDayRisk)
                     tmpStr += "變動幅度為0";
                  else if (mDayRisk > lastDayRisk)
                     tmpStr += "上揚";
                  else
                     tmpStr += "下降";

                  tmpStr += $"，致{dr["kind_id_out"].AsString()}本日結算保證金隨之";

                  if (dr["m_cm"].AsDecimal() > dr["cur_cm"].AsDecimal())
                     tmpStr += "提高。";
                  else
                     tmpStr += "降低。";

                  tmpStr += "▲▲▲";

                  if (dtTemp.Rows.Count > 1)
                     SetInnerText(tmpStr , true , 4.17f , 0.6f);
                  else
                     SetInnerText(tmpStr , true , 4.11f , 1.25f);

                  node++;
               }
               #endregion

               #region 五
               SetInnerText("(五) 結算保證金占契約總值比例與國際主要交易所比較：" , true , 4.11f , 1.25f);
               //特殊處理, 排除以下幾檔
               dtTemp = dtTemp.Select("kind_id <>'RTF' and kind_id <>'MXF' and kind_id <>'TGF' and prod_type <>'O'").CopyToDataTable();
               foreach (DataRow dr in dtTemp.Rows) {
                  string kindId = dr["kind_id"].AsString();

                  DataRow drAbroad = DtAbroad.AsEnumerable().
                                        Where(a => a.Field<string>("kind_id").AsString() == kindId).FirstOrDefault();

                  if (drAbroad != null) {
                     string str1 = GetSpot(kindId , "TAIFEX" , "cur");
                     string str2 = GetSpot(kindId , "TAIFEX" , "m");
                     tmpStr = dtTemp.Rows.Count > 0 ? $"{++node}. " : "  ";

                     tmpStr = $"  現行本公司{kindId}結算保證金占契約總值比例{str1}，倘{checkedDate.AsTaiwanDateTime("{0}/{1}/{2}" , 3)}" +
                              $"依說明二調整，則本公司{kindId}結算保證金占契約總值比例{str2}。";

                     SetInnerText(tmpStr , true , 4.17f , 0.6f);

                     DrowCompareTableE(kindId);
                  }
               }
               #endregion

            }
         }

         /// <summary>
         /// 案由二 最後項說明
         /// </summary>
         /// <param name="dtTemp"></param>
         /// <param name="descPoint">說明項數</param>
         /// <param name="checkedDate"></param>
         protected virtual void SetSecondLastDesc(DataTable dtTemp , string descPoint , DateTime checkedDate) {
            int point = 0;
            List<DataRow> drsTemp = new List<DataRow>();
            string tmpStr = "";
            string checkedDateStr = checkedDate.ToString("yyyyMMdd");

            SetInnerText($"{descPoint}綜上，經考量市場風險，建議如下：");

            //(一)
            //調整
            drsTemp = dtTemp.Select("prod_subtype = 'E' and adj_rate < 0 and adj_code = 'Y'").ToList();
            if (drsTemp.Count > 0) {
               SetInnerText($"({ChineseNumber[++point]}) {GenArrayTxt(wfKindIdE(drsTemp.CopyToDataTable()))}" +
                              $"保證金已達得調整百分比，經考量市場風險，基於穩健保守之原則，建議調整如說明二。" , true , 4.11f , 1.25f);
            }
            drsTemp.Clear();

            drsTemp = dtTemp.Select("prod_subtype = 'E'  and adj_rate > 0 and adj_code = 'Y'").ToList();
            if (drsTemp.Count > 0) {
               SetInnerText($"({ChineseNumber[++point]}) {GenArrayTxt(wfKindIdE(drsTemp.CopyToDataTable()))}" +
                              $"之保證金已達得調整百分比，經考量市場風險，基於穩健保守之原則，建議調整如說明二。" , true , 4.11f , 1.25f);
            }
            drsTemp.Clear();


            //觀察
            drsTemp = dtTemp.Select("prod_subtype = 'E'  and adj_rate < 0 and adj_code = ' '").ToList();
            if (drsTemp.Count > 0) {
               DateTime obsDate = PbFunc.f_get_ocf_next_n_day(checkedDate , 10);
               tmpStr = string.Format("({0}) 鑒於全球金融市場不確定性較高，基於市場風險控管穩健原則，" +
                                      "建議暫不調整{1}保證金，再觀察▲▲10個交易日至(▲▲{2})。" ,
                                      ChineseNumber[++point] , GenArrayTxt(wfKindIdE(drsTemp.CopyToDataTable())) , obsDate.AsTaiwanDateTime("{0}/{1}/{2}" , 3));

               SetInnerText(tmpStr , true , 4.11f , 1.25f);

            }

            drsTemp.Clear();
            drsTemp = dtTemp.Select("prod_subtype = 'E'  and adj_rate > 0 and adj_code = ' '").ToList();
            if (drsTemp.Count > 0) {
               List<string> adjRateList = new List<string>();
               drsTemp.ForEach(r => adjRateList.Add((r.Field<object>("adj_rate").AsDecimal() * 100).ToString("#,##0.##") + "%"));

               tmpStr = string.Format("({0}) {1}之保證金變動幅度達{2}，考量本公司匯率期貨契約於國際市場之競爭性，建議暫不調整。" ,
                                      ChineseNumber[++point] , GenArrayTxt(wfKindIdE(drsTemp.CopyToDataTable())) , GenArrayTxt(adjRateList));

               string warn = tmpStr.IndexOf("高") < 0 ? "▲▲▲" : "";

               SetInnerText(tmpStr + warn , true , 4.11f , 1.25f);
            }
         }

         /// <summary>
         /// 案由二 決議
         /// </summary>
         /// <param name="dtTemp"></param>
         /// <param name="checkedDate"></param>
         protected virtual void SetSecondCaseResult(DataTable dtTemp , DateTime checkedDate) {
            string tmpStr = "";
            int licnt = 0;
            string checkedDateStr = checkedDate.ToString("yyyyMMdd");
            List<DataRow> drsTemp = new List<DataRow>();

            SetSubjectText($"決　　議：");

            drsTemp = dtTemp.Select("prod_subtype = 'E' and adj_code = 'Y'").ToList();
            if (drsTemp.Count > 0) {
               tmpStr = $"調整{GenArrayTxt(wfKindIdE(dtTemp))}保證金如說明二。";
            }

            drsTemp.Clear();
            drsTemp = dtTemp.Select("prod_subtype = 'E'  and adj_code = ' '").ToList();
            if (drsTemp.Count > 0) {
               if (tmpStr != "") {
                  SetInnerText(ChineseNumber[++licnt] + tmpStr);
               }
               string iibsDate = PbFunc.f_get_ocf_next_n_day(checkedDate , 10).AsTaiwanDateTime("{0}/{1}/{2}" , 3);
               tmpStr = string.Format("不調整{0}保證金，觀察▲▲10個交易日(至▲▲{1})，惟仍需持續注意各契約保證金變動幅度、未沖銷部位數明顯變化之狀況、" +
                                       "保證金占契約價值比重及國際同類商品保證金調整等情事，於必要時隨時召開會議討論是否調整保證金。" ,
                                       GenArrayTxt(wfKindIdE(dtTemp)) , iibsDate);

               if (licnt == 0) {
                  SetInnerText(tmpStr , false);
               } else {
                  SetInnerText(ChineseNumber[++licnt] + tmpStr);
               }
            }
         }

         /// <summary>
         /// 案由三 說明
         /// </summary>
         /// <param name="dtTemp"></param>
         /// <param name="checkedDate"></param>
         protected virtual void SetThirdCaseDesc(DataTable dtTemp , DateTime checkedDate) {
            string tmpStr = "";
            SetSubjectText($"說　　明：");

            //說明一
            tmpStr = "一、本公司上開契約結算保證金之變動幅度已達得調整標準之百分比，且進位後金額有變動時，依本公司保證金調整作業規範，" +
                                    "由督導結算業務主管召集業務相關部門主管會商決定是否調整。";
            SetInnerText(tmpStr);

            //說明二
            SetInnerText("二、本次保證金倘經調整，其金額變動如下：");
            DrowTable(dtTemp);

            //說明三
            SetInnerText("三、前揭契約之結算保證金變動幅度、適用風險價格係數、近月合約之結算價、標的證券之收盤價及各月份契約合計成交量及未沖銷部位，列表如下：");
            DrowETFTable(dtTemp);

            ////說明四
            SetThirdLastDesc(dtTemp);
         }

         /// <summary>
         /// 案由三 最後項 說明
         /// </summary>
         /// <param name="dtTmp"></param>
         protected virtual void SetThirdLastDesc(DataTable dtTmp) {
            int licnt = 3;
            string tmpStr = "";
            List<DataRow> drsTmp = dtTmp.Select("prod_subtype = 'S' and adj_code = 'Y'").ToList();

            if (drsTmp.Count > 0) {
               tmpStr = $"{ChineseNumber[++licnt]}、{GenArrayTxt(wfKindIdC(drsTmp.CopyToDataTable()))}已達調整標準百分比，考量市場風險，建議調整如說明二。";
               SetInnerText(tmpStr);
               tmpStr = "餘";
            }

            drsTmp = dtTmp.Select("prod_subtype = 'S' and adj_code = ' '").ToList();
            if (drsTmp.Count > 0) {
               tmpStr += drsTmp.Count == 1 ? GenArrayTxt(wfKindIdC(drsTmp.CopyToDataTable())) : "上開契約";

               tmpStr = $"{ChineseNumber[++licnt]}、基於市場風險控管穩健原則，建議暫不調整{tmpStr}之保證金。";
               SetInnerText(tmpStr);
            }

            Doc.AppendText(Environment.NewLine);
         }

         /// <summary>
         /// 案由三 決議
         /// </summary>
         /// <param name="dtTemp"></param>
         protected virtual void SetThirdCaseResult(DataTable dtTemp , List<CheckedItem> CheckedItems) {
            string tmpStr = "";
            int licnt = 0;
            List<DataRow> drsTemp = new List<DataRow>();

            SetSubjectText($"決　　議：");

            drsTemp = dtTemp.Select("prod_subtype = 'S' and adj_code = 'Y'").ToList();
            if (drsTemp.Count > 0) {
               tmpStr = $"調整{GenArrayTxt(wfKindIdC(dtTemp))}保證金如說明二，以適當控管市場風險。";
               SetInnerText($"{ChineseNumber[++licnt]}、{tmpStr}");
            }

            drsTemp.Clear();
            drsTemp = dtTemp.Select($"prod_subtype = 'S'  and adj_code = ' '").ToList();
            if (drsTemp.Count > 0) {
               if (drsTemp.Count == 1)
                  tmpStr = $"{GenArrayTxt(wfKindIdC(dtTemp))}之保證金暫不調整，";
               else
                  tmpStr = tmpStr != "" ? "餘上開契約之保證金暫不調整，" : "不調整上開契約之保證金，";

               //沒有選grp1 的情況
               if (CheckedItems.Where(c => c.CheckedValue == 1).Count() == 0) {
                  DateTime checkedDate = CheckedItems.Where(c => c.CheckedValue == 5).FirstOrDefault().CheckedDate;
                  string toDate = PbFunc.f_get_ocf_next_n_day(checkedDate , 10).AsTaiwanDateTime("{0}/{1}/{2}" , 3);
                  tmpStr += $"觀察▲▲10個交易日(至▲▲{toDate})，惟仍須持續注意各契約保證金變動幅度及未沖銷部位數變化之狀況，於必要時隨時召開會議討論是否調整保證金。";
               } else {
                  //有選grp1的情況
                  DateTime grp1Date = CheckedItems.Where(c => c.CheckedValue == 1).FirstOrDefault().CheckedDate;
                  DateTime grp2Date = CheckedItems.Where(c => c.CheckedValue == 5).FirstOrDefault().CheckedDate;

                  //grp1 日期與 grp2 日期相等
                  if (grp1Date == grp2Date) {
                     DateTime checkedDate = CheckedItems.Where(c => c.CheckedValue == 5).FirstOrDefault().CheckedDate;
                     string toDate = PbFunc.f_get_ocf_next_n_day(checkedDate , 10).AsTaiwanDateTime("{0}/{1}/{2}" , 3);
                     tmpStr += $"觀察▲▲10個交易日(至▲▲{toDate})，惟仍須持續注意各契約保證金變動幅度及未沖銷部位數變化之狀況，於必要時隨時召開會議討論是否調整保證金。";
                  } else {
                     //日期不相等, 要區分日期
                     tmpStr += "觀察▲▲10個交易日(";

                     drsTemp = dtTemp.Select($"prod_subtype = 'S'  and adj_code = ' ' and data_ymd = '{grp1Date.ToString("yyyyMMdd")}'").ToList();

                     if (drsTemp.Count > 0) {
                        string toDate = PbFunc.f_get_ocf_next_n_day(grp1Date , 10).AsTaiwanDateTime("{0}/{1}/{2}" , 3);
                        DataTable drsTable = drsTemp.CopyToDataTable();
                        drsTable = drsTable.Sort("SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC, KIND_ID ASC, AB_TYPE ASC");

                        tmpStr += $"{GenArrayTxt(wfKindIdE(drsTable))}至▲▲{toDate}";
                     }

                     drsTemp = dtTemp.Select($"prod_subtype = 'S'  and adj_code = ' ' and data_ymd = '{grp2Date.ToString("yyyyMMdd")}'").ToList();
                     if (drsTemp.Count > 0) {
                        string toDate = PbFunc.f_get_ocf_next_n_day(grp1Date , 10).AsTaiwanDateTime("{0}/{1}/{2}" , 3);
                        tmpStr += $"其餘契約至▲▲{toDate}";
                     }

                     tmpStr += ")，惟仍須持續注意各契約保證金變動幅度及未沖銷部位數變化之狀況，於必要時隨時召開會議討論是否調整保證金。";
                  }
               }

               SetInnerText($"{ChineseNumber[++licnt]}、{tmpStr}");
            }
         }

         /// <summary>
         /// 案由四 說明文
         /// </summary>
         /// <param name="dtSTF"></param>
         /// <param name="dtLevel">MGRT Level Data(級距)</param>
         /// <param name="checkedDate"></param>
         protected virtual void SetForthCaseDesc(DataTable dtSTF , DataTable dtLevel , DateTime checkedDate) {
            string tmpStr = "";
            int licnt = 0;
            List<DataRow> drsSTF = new List<DataRow>();
            SetSubjectText($"說　　明：");

            //一
            drsSTF = dtSTF.Select("day_cnt >= 3").ToList();
            drsSTF = drsSTF.OrderBy(r => r.Field<string>("mgr3_cur_level")).ToList();

            if (drsSTF.Count > 0) {
               foreach (DataRow dr in dtLevel.Rows) {
                  string curlevel = dr["MGRT1_LEVEL"].AsString();
                  List<DataRow> drsTmp = drsSTF.Where(r => r.Field<string>("mgr3_cur_level").AsString() == curlevel).ToList();
                  List<string> kindNameList = new List<string>();

                  if (drsTmp.Count <= 0) continue;

                  drsTmp.ForEach(r => kindNameList.Add(r.Field<string>("apdk_name").AsString()));
                  string levelTxt = curlevel == "Z" ? "保證金適用比例" : $"所屬級距({curlevel})";

                  tmpStr += $"{GenArrayTxt(kindNameList)}契約之30天期風險價格係數▲▲連續3日以上(含3日)▲▲與現行{levelTxt}變動幅度達10%以上，";
               }
            }
            tmpStr += "且Max(30天期風險價格係數,風險價格係數平均值)所在級距與現行所屬級距不同，依本公司「保證金調整作業規範」及本部105年5月9日第1050300347號簽，機動評估調整股票期貨結算保證金收取級距。";
            SetInnerText($"{ChineseNumber[++licnt]}、{checkedDate.AsTaiwanDateTime("{0}/{1}/{2}" , 3)}本公司{tmpStr}");

            //二
            drsSTF.Clear();
            tmpStr = "";
            drsSTF = dtSTF.Select("day_cnt > 0 and day_cnt < 3").ToList();
            drsSTF = drsSTF.OrderBy(r => r.Field<string>("mgr3_cur_level")).ToList();

            if (drsSTF.Count > 0) {
               foreach (DataRow dr in dtLevel.Rows) {
                  string curlevel = dr["MGRT1_LEVEL"].AsString();
                  List<DataRow> drsTmp = drsSTF.Where(r => r.Field<string>("mgr3_cur_level").AsString() == curlevel).ToList();
                  List<string> kindNameList = new List<string>();

                  if (drsTmp.Count > 0) {
                     drsTmp.ForEach(r => kindNameList.Add(r.Field<string>("apdk_name").AsString()));
                     string levelTxt = curlevel == "Z" ? "保證金適用比例" : $"所屬級距({curlevel})";

                     tmpStr += $"{GenArrayTxt(kindNameList)}契約之30天期風險價格係數與現行{levelTxt}變動幅度達10%以上，";
                  }
               }
            }
            tmpStr += "考量該檔股票期貨近期價格波動趨勢改變，評估調整股票期貨結算保證金收取級距。";
            SetInnerText($"{ChineseNumber[++licnt]}、{tmpStr}");

            //三
            tmpStr = $"前揭股票期貨{checkedDate.AsTaiwanDateTime("{0}/{1}/{2}" , 3)}之風險價格係數平均值、30天期風險價格係數近月合約之結算價、各月份契約合計成交量及未沖銷部位，列表如下：";
            SetInnerText($"{ChineseNumber[++licnt]}、{tmpStr}");
            dtSTF = dtSTF.Sort("MGR3_CUR_LEVEL,MGR3_KIND_ID");
            DrowSTFTable(dtSTF);

            //四
            SetInnerText($"{ChineseNumber[++licnt]}、考量前揭股票期貨之風險價格係數平均值均明顯低於▲▲所屬級距之保證金適用比例，建議暫不調整保證金。");
         }

         /// <summary>
         /// 案由四 決議
         /// </summary>
         /// <param name="checkedDate"></param>
         protected virtual void SetForthCaseResult(DateTime checkedDate) {
            SetSubjectText($"決　　議：");
            string tmpStr = "";
            string ocfNextDate = PbFunc.f_get_ocf_next_n_day(checkedDate , 10).ToString("MM/dd");

            tmpStr = $"暫不調整，觀察▲▲10個交易日(至▲▲{ocfNextDate})，惟仍須持續注意各契約保證金變動幅度及未沖銷部位數變化之狀況，於必要時隨時召開會議討論是否調整保證金。";
            SetInnerText(tmpStr , false);

         }

         /// <summary>
         /// 案由五 說明文
         /// </summary>
         /// <param name="dtSpan"></param>
         /// <param name="checkedItemsSpan"></param>
         /// <param name="caseNo">案由項數</param>
         protected virtual void SetFifthCaseDesc(DataTable dtSpan , List<CheckedItem> checkedItemsSpan , int caseNo) {
            List<DataRow> drsSpan = dtSpan.Select("sp1_type='SV'").ToList();
            string tmpStr = "";
            SetSubjectText($"說　　明：");

            #region 一
            if (drsSpan.Count > 0) {
               foreach (CheckedItem c in checkedItemsSpan) {
                  List<DataRow> drsTmp = drsSpan.Where(r => r.Field<string>("sp1_type").AsString() == "SV" &&
                                                         r.Field<DateTime>("sp1_date") == c.CheckedDate).ToList();

                  if (checkedItemsSpan.IndexOf(c) != 0) {
                     if (checkedItemsSpan.Exists(i => i.CheckedDate == c.CheckedDate))
                        continue;
                  }

                  if (drsTmp.Count == 0) continue;
                  //加上連接詞
                  if (checkedItemsSpan.Count > 1 && c == checkedItemsSpan.Last()) tmpStr += "，及";

                  List<string> kindNameSV = new List<string>();
                  drsTmp.ForEach(r => kindNameSV.Add(r.Field<string>("spt1_abbr_name").AsString()));

                  tmpStr += $"{c.CheckedDate.AsTaiwanDateTime("{0}/{1}/{2}" , 3)}本公司{GenArrayTxt(kindNameSV)}";
               }
               tmpStr += $"之波動度偵測全距(VSR)變動幅度已達得調整標準之百分比；";
            }

            drsSpan.Clear();
            drsSpan = DtSpan.Select("sp1_type='SD'").ToList();
            if (drsSpan.Count > 0) {
               foreach (CheckedItem c in checkedItemsSpan) {
                  List<DataRow> drsTmp = drsSpan.Where(r => r.Field<string>("sp1_type").AsString() == "SD" &&
                                                         r.Field<DateTime>("sp1_date") == c.CheckedDate).ToList();

                  if (checkedItemsSpan.IndexOf(c) != 0) {
                     if (checkedItemsSpan.Exists(i => i.CheckedDate == c.CheckedDate))
                        continue;
                  }

                  if (drsTmp.Count == 0) continue;
                  //加上連接詞
                  if (checkedItemsSpan.Count > 1 && c == checkedItemsSpan.Last()) tmpStr += "，及";

                  List<string> kindNameSD = new List<string>();
                  drsTmp.ForEach(r => kindNameSD.Add(r.Field<string>("spt1_abbr_name").AsString()));

                  tmpStr += $"{c.CheckedDate.AsTaiwanDateTime("{0}/{1}/{2}" , 3)}本公司{GenArrayTxt(kindNameSD)}";
               }
               tmpStr += "之delta耗用比率變動幅度已達得調整標準之百分比";
            }

            drsSpan.Clear();
            drsSpan = DtSpan.Select("sp1_type='SS'").ToList();
            if (drsSpan.Count > 0) {
               foreach (CheckedItem c in checkedItemsSpan) {
                  List<DataRow> drsTmp = drsSpan.Where(r => r.Field<string>("sp1_type").AsString() == "SS" &&
                                                         r.Field<DateTime>("sp1_date") == c.CheckedDate).ToList();

                  if (checkedItemsSpan.IndexOf(c) != 0) {
                     if (checkedItemsSpan.Exists(i => i.CheckedDate == c.CheckedDate))
                        continue;
                  }

                  if (drsTmp.Count == 0) continue;
                  //加上連接詞
                  if (checkedItemsSpan.Count > 1 && c == checkedItemsSpan.Last()) tmpStr += "，及";

                  List<string> kindNameSS = new List<string>();
                  drsTmp.ForEach(r => kindNameSS.Add(r.Field<string>("spt1_abbr_name").AsString()));

                  tmpStr += $"{c.CheckedDate.AsTaiwanDateTime("{0}/{1}/{2}" , 3)}本公司{GenArrayTxt(kindNameSS)}";
               }
               tmpStr += "之兩商品組合間跨商品折抵率變動幅度已達得調整標準之百分比";
            }
            tmpStr += "，依本公司SPAN相關參數之訂定與調整標準說明，由督導結算業務主管召集業務相關部門主管會商決定是否調整。";

            SetInnerText("一、" + tmpStr);
            #endregion

            //二
            SetInnerText("二、本次參數調整變動如下：");
            DrowSpanTable(DtSpanTable);

            //三
            string caseTxt = "";
            //當Span 在案由二以上時加文字, 例案由一、二、三
            if (caseNo > 2) {
               for (int i = 1 ; i < caseNo ; i++) {
                  caseTxt += ChineseNumber[i] + "、";
               }
               caseTxt = caseTxt.TrimEnd('、');
            }
            tmpStr = $"三、倘僅調整上開契約參數（不含案由{caseTxt}各契約保證金PSR之調整），經以{DateTime.Now.AsTaiwanDateTime("{0}/{1}/{2}" , 3)}收盤後未沖銷部位試算，" +
                     "對結算會員收取之SPAN結算保證金由【★請鍵入數值】元減少(增加)至【★請鍵入數值】元，減少(增加)金額【★請鍵入數值】元，降幅(增幅)約【★請鍵入數值】%。";
            SetInnerText(tmpStr);

            //四
            tmpStr = $"四、倘依案由{caseTxt}建議調整各契約結算保證金及上開契約參數，經以{DateTime.Now.AsTaiwanDateTime("{0}/{1}/{2}" , 3)}收盤後未沖銷部位試算，" +
                     "對結算會員收取之SPAN結算保證金由【★請鍵入數值】元減少(增加)至【★請鍵入數值】元，減少(增加)金額【★請鍵入數值】元，降幅(增幅)約【★請鍵入數值】%。";
            SetInnerText(tmpStr);

            //五
            drsSpan.Clear();
            drsSpan = dtSpan.Select("sp2_adj_code = 'Y'").ToList();
            if (drsSpan.Count <= 0) {
               SetInnerText("五、綜上，基於穩健保守原則，建議暫不調整。");
            }
         }

         /// <summary>
         /// 案由五 決議
         /// </summary>
         /// <param name="dtSpan"></param>
         protected virtual void SetFifthCaseResult(DataTable dtSpan) {
            SetSubjectText($"決　　議：");

            List<DataRow> drsSpan = dtSpan.Select("sp2_adj_code = 'Y'").ToList();
            string tmpStr = "";

            if (drsSpan.Count <= 0) {
               SetInnerText("暫不調整，惟仍持續注意各契約參數變動幅度及市場行情波動狀況等情事，於必要時隨時召開會議討論是否調整SPAN保證金參數。" , false);
               return;
            }

            List<string> kindNameSpan = new List<string>();
            drsSpan.ForEach(r => kindNameSpan.Add($"{r.Field<string>("spt1_abbr_name").AsString()}之{r.Field<string>("SP1_TYPE_RE").AsString()}"));

            drsSpan = dtSpan.Select("sp2_adj_code = ' '").ToList();

            tmpStr = drsSpan.Count == 0 ? "前揭參數" : GenArrayTxt(kindNameSpan);

            SetInnerText($"經考量市場風險，{tmpStr}調整如說明二。" , false);
         }

         protected override void DrowTable(DataTable dataTable) {

            foreach (DataRow dr in dataTable.Rows) {
               Doc.AppendText(Environment.NewLine);

               string amtType = dr["AMT_TYPE"].AsString();
               string prodType = dr["PROD_TYPE"].AsString();
               object[] args = new object[] { dr };
               I40030AmtProdType iAmtProdType = CreateI40030AmtProdType(GetType() , "AmtProdType40030" + amtType + prodType , args);

               SetComment(iAmtProdType.CurrencyName , 23);

               CreateTable(Doc , 2 , 7);

               SetTableColTitle(iAmtProdType.ProdName , iAmtProdType.TableTitle , iAmtProdType.AfterAdjustTitle , iAmtProdType.BeforeAdjustTitle);

               SetTableColValue(iAmtProdType , dr);

               //特殊處理, 表格下方小註
               string comment = iAmtProdType.ProdName == "MTX" ? "(註：依TX保證金四分之一計算)" : "(註：風險價格係數：{0}，結算保證金變動幅度：{1})";
               comment = string.Format(comment , dr["m_day_risk"].AsPercent(2) , dr["adj_rate"].AsPercent(2));
               SetComment(comment , 15);

               Doc.AppendText(Environment.NewLine);
            }

         }

         /// <summary>
         /// 組商品名稱
         /// </summary>
         /// <param name="dtTemp"></param>
         /// <returns></returns>
         protected virtual List<string> wfKindIdC(DataTable dtTemp) {

            List<string> kindNameList = new List<string>();
            List<DataRow> drTemp = dtTemp.Select("prod_subtype = 'S'").ToList();

            foreach (DataRow dr in dtTemp.Rows) {
               string kindName = dr["kind_abbr_name"].AsString();

               kindName += dr["prod_subtype"].AsString() == "S" ?
                              $"({dr["kind_id"].AsString()})" : "";

               if (!kindNameList.Exists(k => k == kindName)) kindNameList.Add(kindName);
            }

            return kindNameList;
         }

         /// <summary>
         /// 組商品名稱
         /// </summary>
         /// <param name="dtTemp"></param>
         /// <returns></returns>
         protected virtual List<string> wfKindIdE(DataTable dtTemp) {

            List<string> kindNameList = new List<string>();
            List<DataRow> drTemp = dtTemp.AsEnumerable().ToList();

            drTemp.ForEach(r => kindNameList.Add(r.Field<string>("kind_id_out").AsString()));

            return kindNameList;
         }

         /// <summary>
         /// 組商品名稱
         /// </summary>
         /// <param name="dtTemp"></param>
         /// <returns></returns>
         protected virtual List<string> wfKindIdA(DataTable dtTemp) {

            List<string> kindNameList = new List<string>();
            List<DataRow> drTemp = dtTemp.AsEnumerable().ToList();

            drTemp.ForEach(r => kindNameList.Add($"{r.Field<string>("kind_abbr_name").AsString()}(" +
                                                 $"{r.Field<string>("kind_id").AsString()}，股票期貨標的證券代號" +
                                                 $" {r.Field<string>("stock_id").AsString()})"));


            return kindNameList;
         }

         /// <summary>
         /// 組商品名稱 STF
         /// </summary>
         /// <param name="dtTemp"></param>
         /// <returns></returns>
         protected virtual List<string> wfKindIdSTFA(DataTable dtTemp) {

            List<string> kindNameList = new List<string>();
            List<DataRow> drTemp = dtTemp.AsEnumerable().ToList();

            drTemp.ForEach(r => kindNameList.Add($"{r.Field<string>("apdk_name").AsString()}(" +
                                                 $"{r.Field<string>("mgr3_kind_id").AsString()}，股票期貨標的證券代號" +
                                                 $" {r.Field<string>("mgr3_sid").AsString()})"));


            return kindNameList;
         }

         /// <summary>
         /// 單筆資料 return "" 多筆 return "分別"
         /// </summary>
         /// <param name="dtTemp"></param>
         /// <returns></returns>
         protected virtual string SingleOrMore(DataTable dtTemp) {

            return dtTemp.Rows.Count > 1 ? "分別" : "";
         }

         /// <summary>
         /// 比較商品
         /// </summary>
         /// <param name="kindId"></param>
         /// <param name="cmRate"></param>
         /// <returns></returns>
         protected virtual string WfCompareKind(string kindId , decimal cmRate) {
            List<string> fNameList = new List<string>();
            string re = "";
            List<DataRow> drTmp = new List<DataRow>();

            drTmp = DtAbroad.Select("m_cm_rate <" + cmRate + " and kind_id = '" + kindId + "' and data_type='2'").ToList();

            if (drTmp.Count > 0) {
               re += "較";

               drTmp.ForEach(r => fNameList.Add(r.Field<string>("f_name").AsString()));

               re += GenArrayTxt(fNameList);
               re += "高";
            }

            drTmp = DtAbroad.Select("m_cm_rate >" + cmRate + " and kind_id = '" + kindId + "' and data_type='2'").ToList();

            if (drTmp.Count > 0) {
               if (re != "") re += "，";

               re += "較";

               drTmp.ForEach(r => fNameList.Add(r.Field<string>("f_name").AsString()));

               re += GenArrayTxt(fNameList);
               re += "低";
            }

            return re;
         }

         /// <summary>
         /// 查看現貨資料
         /// </summary>
         /// <param name="kindId"></param>
         /// <param name="fId"></param>
         /// <param name="ratePendStr">現行 或 調整</param>
         /// <returns></returns>
         protected virtual string GetSpot(string kindId , string fId , string ratePendStr) {


            DataRow drTmp = DtAbroad.AsEnumerable().Where(a => a.Field<string>("kind_id").AsString() == kindId &&
                                                            a.Field<string>("f_id").AsString() == fId).FirstOrDefault();

            return drTmp != null ? WfCompareKind(kindId , drTmp[$"{ratePendStr}_cm_rate"].AsDecimal()) : "★無現貨資料";
         }

         /// <summary>
         /// 比較黃金期貨
         /// </summary>
         /// <param name="str1">GDF 現貨資料</param>
         /// <param name="str2">TGF 現貨資料</param>
         /// <returns></returns>
         protected virtual string WfCampreGold(string str1 , string str2) {

            string re = "";

            if ((str1 == str2) || string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2)) {
               re = "本公司GDF及TGF之結算保證金占契約總值比例";

               re += str1 == str2 ? "均" : "";

               re += !string.IsNullOrEmpty(str1) ? str1 : str2;
            } else {
               re += $"本公司GDF結算保證金占契約總值比例{str1}；TGF結算保證金占契約總值比例{str2}";
            }

            return re;
         }

         /// <summary>
         /// interface 指數類商品參數
         /// </summary>
         /// <param name="type"></param>
         /// <param name="name"></param>
         /// <param name="args"></param>
         /// <returns></returns>
         protected I40030KindInfoI CreateI40030KindInfoI(Type type , string name , object[] args = null) {
            string AssemblyName = type.Namespace.Split('.')[0];//最後compile出來的dll名稱
            string className = type.Namespace + "." + type.ReflectedType.Name + "+" + name;//完整的class路徑(注意,內部的class執行時其fullName是用+號連結起來)
            return (I40030KindInfoI)Assembly.Load(AssemblyName).CreateInstance(className , true , BindingFlags.CreateInstance , null , args , null , null);
         }

         /// <summary>
         /// interface ETF 商品參數
         /// </summary>
         /// <param name="type"></param>
         /// <param name="name"></param>
         /// <param name="args"></param>
         /// <returns></returns>
         protected I40030KindInfoE CreateI40030KindInfoE(Type type , string name , object[] args = null) {
            string AssemblyName = type.Namespace.Split('.')[0];//最後compile出來的dll名稱
            string className = type.Namespace + "." + type.ReflectedType.Name + "+" + name;//完整的class路徑(注意,內部的class執行時其fullName是用+號連結起來)
            return (I40030KindInfoE)Assembly.Load(AssemblyName).CreateInstance(className , true , BindingFlags.CreateInstance , null , args , null , null);
         }

         /// <summary>
         /// 商品現貨漲跌說明
         /// </summary>
         /// <param name="prodType"></param>
         /// <param name="dr"></param>
         /// <returns></returns>
         protected virtual string GenProdString(string prodType , DataRow dr) {
            string tmpStr = "";
            string colValue = "";
            string drprodType = dr["prod_type"].AsString();

            //現貨漲跌
            string upDown = "";
            decimal upDownPoint = GetUpDown(prodType , dr);

            if (drprodType == prodType)
               colValue = "m";
            else
               colValue = string.IsNullOrEmpty(dr["oth_kind_id"].AsString()) ? "pdk" : "oth";

            if (upDownPoint == 0) {
               tmpStr += "漲跌為0";
               upDown = ChineseNumber[0];
            } else {
               upDown = upDownPoint > 0 ? "漲" : "跌";
            }
            int pointFormat = dr["prod_subtype"].AsString() == "E" ? 4 : 2;
            tmpStr += upDown + Math.Abs(upDownPoint).ToString() + "點";

            if (drprodType == prodType)
               upDownPoint = dr["m_rtn"].AsDecimal();
            else
               upDownPoint = dr[colValue + "_return_rate"].AsDecimal();

            tmpStr += $"，{upDown}幅{Math.Abs(upDownPoint).AsPercent(pointFormat)}、";

            return tmpStr;
         }

         /// <summary>
         /// 現貨漲跌值
         /// </summary>
         /// <param name="prodType"></param>
         /// <param name="dr"></param>
         /// <returns></returns>
         protected virtual decimal GetUpDown(string prodType , DataRow dr) {
            string colValue = "";
            string drprodType = dr["prod_type"].AsString();

            if (drprodType == prodType)
               colValue = "m";
            else
               colValue = string.IsNullOrEmpty(dr["oth_kind_id"].AsString()) ? "pdk" : "oth";

            //現貨漲跌值
            return dr[colValue + "_up_down"].AsDecimal();
         }

         /// <summary>
         /// 市場方向
         /// </summary>
         /// <param name="upDownPoint">漲跌點數</param>
         /// <param name="warn">插入文字</param>
         /// <returns></returns>
         protected virtual string MarketDirectionWarn(decimal upDownPoint , string warn) {

            string tmpStr = "";
            if ((upDownPoint > 0 && warn == "+") || (upDownPoint < 0 && warn == "-"))
               tmpStr += "相同";
            else
               tmpStr += "相反";

            return tmpStr;
         }

         /// <summary>
         /// 市場方向
         /// </summary>
         /// <param name="upDownPoint">漲跌點數</param>
         /// <param name="rateDiff">漲跌值</param>
         /// <returns></returns>
         protected virtual string MarketDirection(decimal upDownPoint , decimal rateDiff) {

            string tmpStr = "";
            if ((upDownPoint > 0 && rateDiff > 0) || (upDownPoint < 0 && rateDiff < 0))
               tmpStr += "相同";
            else
               tmpStr += "相反";

            return tmpStr;
         }

         /// <summary>
         /// 商品比較excel 
         /// </summary>
         /// <param name="wb"></param>
         /// <param name="kindId">商品ID</param>
         /// <param name="sheetName"></param>
         protected virtual void ExportCompareExcel(DevExpress.Spreadsheet.Workbook wb , string kindId , string sheetName , int dateCol) {

            List<DataRow> drs = Dt.Select($"kind_id='{kindId}'").ToList();
            if (drs.Count <= 0) return;

            DevExpress.Spreadsheet.Worksheet worksheet = wb.Worksheets[sheetName];

            List<DataRow> drsAbroad = DtAbroad.Select($"kind_grp='{kindId}'").ToList();
            drsAbroad = drsAbroad.OrderBy(r => r.Field<int>("seq_no")).ToList();

            worksheet.Cells[0 , dateCol].SetValue($"資料日期：{drsAbroad.First().Field<DateTime>("data_date").ToString("yyyy年MM月dd日")}");

            int rowIndexBefore = drsAbroad.FirstOrDefault().Field<int>("rpt_row1");
            int rowIndexAfter = drsAbroad.FirstOrDefault().Field<int>("rpt_row2");
            string[] hasValKind = new string[] { "GDF" , "RHF" , "XEF" , "XJF" , "XBF" , "XAF" };
            bool hasVal = hasValKind.ToList().Where(h => h == kindId).Count() > 0 ? true : false;
            int rowIndex = 0;

            //現行
            foreach (DataRow dr in drsAbroad) {
               rowIndex = rowIndexBefore - 2;
               int seqNo = dr["seq_no"].AsInt();

               if (seqNo == 0) continue;

               int colIndex = seqNo + 1;
               worksheet.Cells[++rowIndex , colIndex].SetValue(dr["cur_cm"].AsDecimal());

               if (hasVal) worksheet.Cells[++rowIndex , colIndex].SetValue(dr["v_val"].AsDecimal());

               worksheet.Cells[++rowIndex , colIndex].SetValue(dr["cur_cm_rate"].AsDecimal());
               worksheet.Cells[++rowIndex , colIndex].SetValue(dr["cur_im_rate"].AsDecimal());
            }

            //調整後
            foreach (DataRow dr in drsAbroad) {
               rowIndex = rowIndexAfter - 2;
               int seqNo = dr["seq_no"].AsInt();

               if (seqNo == 0) continue;

               int colIndex = rowIndexBefore != rowIndexAfter ? seqNo + 1 : seqNo + drsAbroad.Count + 1;
               worksheet.Cells[++rowIndex , colIndex].SetValue(dr["m_cm"].AsDecimal());

               if (hasVal) worksheet.Cells[++rowIndex , colIndex].SetValue(dr["v_val"].AsDecimal());

               worksheet.Cells[++rowIndex , colIndex].SetValue(dr["m_cm_rate"].AsDecimal());
               worksheet.Cells[++rowIndex , colIndex].SetValue(dr["m_im_rate"].AsDecimal());
            }
         }

         /// <summary>
         /// 期貨excel 
         /// </summary>
         /// <param name="wb"></param>
         /// <param name="dtTmp"></param>
         protected virtual void ExportExcelFut(DevExpress.Spreadsheet.Workbook wb , DataTable dtTmp) {

            DevExpress.Spreadsheet.Worksheet worksheet = wb.Worksheets["保證金(Fut)"];
            DateTime dataDate = dtTmp.Rows[0]["data_ymd"].AsDateTime("yyyyMMdd");

            worksheet.Cells[0 , 7].SetValue($"資料日期：{dataDate.ToString("yyyy年MM月dd日")}");

            int rowIndex = 1;
            foreach (DataRow dr in dtTmp.Rows) {
               int colIndex = 1;

               worksheet.Cells[rowIndex , colIndex].SetValue("單位：" + dr["currency_name"].AsString());
               rowIndex += 1;
               worksheet.Cells[rowIndex , colIndex].SetValue(dr["kind_id_out"].AsString());
               rowIndex += 2;
               colIndex = 2;

               string[] cols = new string[] { "m_im" , "m_mm" , "m_cm" , "cur_im" , "cur_mm" , "cur_cm" };
               foreach (string dc in cols) {

                  worksheet.Cells[rowIndex , colIndex].SetValue(dr[dc].AsDecimal());
                  colIndex++;
               }
               rowIndex += 1;
               worksheet.Cells[rowIndex , 1].SetValue($"(註：風險價格係數：{dr["m_day_risk"].AsPercent(2)}，結算保證金變動幅度：{dr["adj_rate"].AsPercent(2)})");
               rowIndex += 2;
            }
         }

         /// <summary>
         /// 選擇權 excel
         /// </summary>
         /// <param name="wb"></param>
         /// <param name="dtTmp"></param>
         protected virtual void ExportExcelOPT(DevExpress.Spreadsheet.Workbook wb , DataTable dtTmp) {

            DevExpress.Spreadsheet.Worksheet worksheet = wb.Worksheets["保證金(Opt)"];
            DateTime dataDate = dtTmp.Rows[0]["data_ymd"].AsDateTime("yyyyMMdd");

            worksheet.Cells[0 , 7].SetValue($"資料日期：{dataDate.ToString("yyyy年MM月dd日")}");

            int rowIndex = 1;
            string kindId = "";
            foreach (DataRow dr in dtTmp.Rows) {
               int colIndex = 1;
               if (dr["kind_id"].AsString() != kindId) {
                  kindId = dr["kind_id"].AsString();

                  if (dr["ab_type"].AsString() == "B") continue;
               }

               worksheet.Cells[rowIndex , colIndex].SetValue("單位：" + dr["currency_name"].AsString());
               rowIndex += 1;
               worksheet.Cells[rowIndex , colIndex].SetValue(dr["kind_id_out"].AsString());
               rowIndex += 2;
               colIndex = 2;

               string[] cols = new string[] { "m_im" , "m_mm" , "m_cm" , "cur_im" , "cur_mm" , "cur_cm" };
               foreach (string dc in cols) {

                  worksheet.Cells[rowIndex , colIndex].SetValue(dr[dc].AsDecimal());
                  colIndex++;
               }
               rowIndex += 1;
               colIndex = 2;

               cols = new string[] { "m_im_b" , "m_mm_b" , "m_cm_b" , "cur_im_b" , "cur_mm_b" , "cur_cm_b" };
               foreach (string dc in cols) {

                  worksheet.Cells[rowIndex , colIndex].SetValue(dr[dc].AsDecimal());
                  colIndex++;
               }

               rowIndex += 1;
               worksheet.Cells[rowIndex , 1].SetValue($"(註：風險價格係數：{dr["m_day_risk"].AsPercent(2)}，結算保證金變動幅度：{dr["adj_rate"].AsPercent(2)})");
               rowIndex += 2;
            }
         }

         /// <summary>
         /// Span excel
         /// </summary>
         /// <param name="wb"></param>
         /// <param name="dtTmp"></param>
         /// <param name="checkItems"></param>
         protected virtual void ExportExcelSpan(DevExpress.Spreadsheet.Workbook wb , DataTable dtTmp , List<CheckedItem> checkItems) {

            DevExpress.Spreadsheet.Worksheet worksheet = wb.Worksheets["SPAN參數"];
            DateTime dataDate = checkItems.FirstOrDefault().CheckedDate;

            worksheet.Cells[0 , 5].SetValue($"資料日期：{dataDate.ToString("yyyy年MM月dd日")}");

            int rowIndex = 2;
            foreach (DataRow dr in dtTmp.Rows) {
               int colIndex = 1;

               foreach (DataColumn dc in dtTmp.Columns) {
                  worksheet.Cells[rowIndex , colIndex].SetValue(dr[dc]);
                  colIndex++;
               }
               rowIndex += 4;
            }
         }

         /// <summary>
         /// rtf 指數類 表格
         /// </summary>
         /// <param name="kindId"></param>
         protected virtual void DrowCompareTableI(string kindId) {
            Doc.AppendText(Environment.NewLine);
            List<DataRow> drsAbroad = DtAbroad.Select("kind_grp='" + kindId + "'").OrderBy(r => r.Field<int>("seq_no")).ToList();

            if (drsAbroad.Count < 1) return;

            I40030KindInfoI IkindInfo = CreateI40030KindInfoI(GetType() , "KindInfo40030" + kindId , null);

            CreateTable(Doc , IkindInfo.RowCount , IkindInfo.ColCount);

            SetTableStr(0 , 0 , IkindInfo.TableName);
            //WordTableCell.PreferredWidthType = WidthType.Fixed;
            //WordTableCell.PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(2.6f);
            WordTableCell.VerticalAlignment = TableCellVerticalAlignment.Center;
            WordTable.MergeCells(WordTableCell , WordTable[0 , IkindInfo.ColCount - 1]);

            SetTableStr(1 , 0 , "交易所");
            WordTable[1, 0].PreferredWidthType = WidthType.Fixed;
            WordTable[1, 0].PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(2.8f);
            WordTable.MergeCells(WordTableCell , WordTable[2 , 0]);

            SetTableStr(1 , 1 , "現行比例");
            WordTable[1, 1].PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(2.6f);
            SetTableStr(1 , IkindInfo.ColTtile.Length + 1 , "調整後");

            WordTable.MergeCells(WordTableCell , WordTable[1 , IkindInfo.ColTtile.Length * 2]);
            WordTable.MergeCells(WordTable[1 , 1] , WordTable[1 , IkindInfo.ColTtile.Length]);

            //欄位名
            int k = 1;
            foreach (string str in IkindInfo.ColTtile) {
               WordTableCell = WordTable[2 , k];
               WordTableCell.PreferredWidthType = WidthType.Fixed;
               WordTableCell.PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(1.8f);

               Doc.InsertSingleLineText(WordTableCell.Range.Start , str);
               k++;
            }

            foreach (string str in IkindInfo.ColTtile) {
               WordTableCell = WordTable[2 , k];
               WordTableCell.PreferredWidthType = WidthType.Fixed;
               WordTableCell.PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(1.8f);

               Doc.InsertSingleLineText(WordTableCell.Range.Start , str);
               k++;
            }

            //欄位填值
            string[] dbCols = new string[] { "_cm" , "_cm_rate" , "_im_rate" };

            foreach (string str in IkindInfo.RowTitle) {
               TableRow tableRow = WordTable.Rows.Append();

               WordTableCell = tableRow.FirstCell;
               Doc.InsertSingleLineText(WordTableCell.Range.Start , str);
            }

            foreach (DataRow dr in drsAbroad) {
               int formatIndex = 0;
               int rowindex = IkindInfo.RowCount;

               foreach (string dbcol in dbCols) {

                  if (IkindInfo.FieldFormat[formatIndex] == "%") {
                     Doc.InsertSingleLineText(WordTable[rowindex , dr["seq_no"].AsInt()].Range.Start , dr["cur" + dbcol].AsPercent(2));
                     Doc.InsertSingleLineText(WordTable[rowindex , dr["seq_no"].AsInt() + IkindInfo.ColTtile.Length].Range.Start , dr["m" + dbcol].AsPercent(2));
                  } else {
                     Doc.InsertSingleLineText(WordTable[rowindex , dr["seq_no"].AsInt()].Range.Start , dr["cur" + dbcol].AsDecimal().ToString(IkindInfo.FieldFormat[formatIndex]));
                     Doc.InsertSingleLineText(WordTable[rowindex , dr["seq_no"].AsInt() + IkindInfo.ColTtile.Length].Range.Start , dr["m" + dbcol].AsDecimal().ToString(IkindInfo.FieldFormat[formatIndex]));
                  }

                  rowindex++;
                  formatIndex++;
               }
            }
         }

         /// <summary>
         /// rtf ETF 表格
         /// </summary>
         /// <param name="kindId"></param>
         protected virtual void DrowCompareTableE(string kindId) {
            Doc.AppendText(Environment.NewLine);

            I40030KindInfoE IkindInfo = CreateI40030KindInfoE(GetType() , "KindInfo40030" + kindId , null);

            List<DataRow> drsAbroad = DtAbroad.Select("kind_grp='" + kindId + "'").OrderBy(r => r.Field<int>("seq_no")).ToList();

            int t = 0;
            foreach (string noworafter in IkindInfo.NowOrAfter) {
               SetComment(IkindInfo.CurrencyName , 23);

               CreateTable(Doc , IkindInfo.RowCount , IkindInfo.ColCount);

               SetTableStr(0 , 0 , string.Format(IkindInfo.TableName[t++] , noworafter));
               WordTableCell.PreferredWidthType = WidthType.Fixed;
               WordTableCell.PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(2.65f);
               WordTableCell.VerticalAlignment = TableCellVerticalAlignment.Center;
               WordTable.MergeCells(WordTableCell , WordTable[0 , IkindInfo.ColCount - 1]);

               //欄位名
               int k = 0;
               foreach (string str in IkindInfo.ColTtile) {
                  Doc.InsertSingleLineText(WordTable[1 , k].Range.Start , str);
                  k++;
               }

               //欄位填值
               foreach (string str in IkindInfo.RowTitle) {
                  TableRow tableRow = WordTable.Rows.Append();

                  WordTableCell = tableRow.FirstCell;
                  Doc.InsertSingleLineText(WordTableCell.Range.Start , str);
               }

               foreach (DataRow dr in drsAbroad) {
                  int formatindex = 0;
                  int rowindex = IkindInfo.RowCount;

                  foreach (string dbcol in IkindInfo.DbCol(noworafter)) {

                     if (IkindInfo.FieldFormat[formatindex] == "%") {
                        Doc.InsertSingleLineText(WordTable[rowindex , dr["seq_no"].AsInt()].Range.Start , dr[dbcol].AsPercent(2));
                     } else {
                        Doc.InsertSingleLineText(WordTable[rowindex , dr["seq_no"].AsInt()].Range.Start , dr[dbcol].AsDecimal().ToString(IkindInfo.FieldFormat[formatindex]));
                     }

                     rowindex++;
                     formatindex++;
                  }
               }
               Doc.AppendText(Environment.NewLine);
            }
         }

         /// <summary>
         /// 畫ETF 表格
         /// </summary>
         /// <param name="dtTmp">ETF 資料</param>
         protected virtual void DrowETFTable(DataTable dtTmp) {
            Doc.AppendText(Environment.NewLine);

            CreateTable(Doc , 2 , 7);
            string[] firstRowColName = new string[] { "契約名稱" , "交易日期" , "保證金\r\n變動幅度" , "適用風險\r\n價格係數" , "近月合約結算價/標的證券收盤價" };

            int c = 0;
            foreach (string col in firstRowColName) {
               SetTableRNStr(0 , c , col);
               WordTableCell.PreferredWidthType = WidthType.Fixed;
               WordTableCell.PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(2f);
               WordTableCell.VerticalAlignment = TableCellVerticalAlignment.Center;
               WordTable.MergeCells(WordTableCell , WordTable[1 , c]);
               c++;
            }

            SetTableStr(0 , 5 , "未沖銷部位數及成交量");
            WordTable.MergeCells(WordTableCell , WordTable[0 , 6]);

            SetTableRNStr(1 , 5 , "未沖銷\r\n部位數");
            WordTable[1, 5].PreferredWidthType = WidthType.Fixed;
            WordTable[1, 5].PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(1.7f);
            WordTableCell.VerticalAlignment = TableCellVerticalAlignment.Center;

            SetTableStr(1 , 6 , "成交量");
            WordTable[1, 6].PreferredWidthType = WidthType.Fixed;
            WordTable[1, 6].PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(1.7f);
            WordTableCell.VerticalAlignment = TableCellVerticalAlignment.Center;

            string[] dbcols = new string[] { "adj_rate" , "m_day_risk" , "m_price" , "i_oi" , "i_qnty" };
            string[] fieldFormat = new string[] { "%" , "%" , "#,##0.##" , "#,##0" , "#,##0" };
            foreach (DataRow dr in dtTmp.Rows) {
               TableRow tableRow = WordTable.Rows.Append();
               WordTableCell = tableRow.FirstCell;

               Doc.InsertSingleLineText(WordTableCell.Range.Start , $"{dr["kind_id"].AsString()}{Characters.LineBreak}({dr["kind_abbr_name"].AsString()})");
               Doc.InsertSingleLineText(WordTable[tableRow.Index , 1].Range.Start , dr["data_ymd"].AsDateTime("yyyyMMdd").ToString("yyyy/MM/dd"));

               int formatindex = 0;
               int colindex = 2;
               foreach (string col in dbcols) {
                  if (fieldFormat[formatindex] == "%") {
                     Doc.InsertSingleLineText(WordTable[tableRow.Index , colindex].Range.Start , dr[col].AsPercent(2));
                  } else {
                     Doc.InsertSingleLineText(WordTable[tableRow.Index , colindex].Range.Start , dr[col].AsDecimal().ToString(fieldFormat[formatindex]));
                  }
                  formatindex++;
                  colindex++;
               }
            }
         }

         /// <summary>
         /// 畫Span 表格
         /// </summary>
         /// <param name="dtSpan">Span 資料</param>
         protected virtual void DrowSpanTable(DataTable dtSpan) {
            CreateTable(Doc , 1 , 5);
            string[] colName = new string[] { "參數名稱" , "適用商品組合" , "調整後" , "調整前" , "變動幅度" };

            int c = 0;
            foreach (string col in colName) {
               Doc.InsertSingleLineText(WordTable[0 , c].Range.Start , col);
               c++;
            }

            foreach (DataRow dr in dtSpan.Rows) {
               TableRow tableRow = WordTable.Rows.Append();
               WordTableCell = tableRow.FirstCell;

               int colinedx = 0;
               foreach (DataColumn dc in dtSpan.Columns) {
                  Doc.InsertSingleLineText(WordTable[tableRow.Index , colinedx].Range.Start , dr[dc].AsString());
                  colinedx++;
               }
            }
         }

         /// <summary>
         /// 畫STF 表格
         /// </summary>
         /// <param name="dtSTF">STF 資料</param>
         protected virtual void DrowSTFTable(DataTable dtSTF) {
            CreateTable(Doc , 3 , 8);

            SetTableStr(0 , 0 , "股票期貨英文代碼");
            WordTableCell.PreferredWidthType = WidthType.Fixed;
            WordTableCell.PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(1.6f);
            WordTableCell.VerticalAlignment = TableCellVerticalAlignment.Center;
            WordTable.MergeCells(WordTableCell , WordTable[2 , 0]);

            SetTableRNStr(0 , 1 , "股票期貨\r\n中文簡稱");
            WordTableCell.PreferredWidthType = WidthType.Fixed;
            WordTableCell.PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(1.9f);
            WordTable.MergeCells(WordTableCell , WordTable[2 , 1]);

            SetTableStr(0 , 2 , "風險價格係數");
            SetTableStr(0 , 4 , "現貨、期貨價格");
            SetTableStr(0 , 6 , "未沖銷部位數及成交量");
            WordTable.MergeCells(WordTableCell , WordTable[0 , 7]);
            WordTable.MergeCells(WordTable[0 , 4] , WordTable[0 , 5]);
            WordTable.MergeCells(WordTable[0 , 2] , WordTable[0 , 3]);

            SetTableRNStr(1 , 2 , "本日\r\n30天期風險價格係數");
            WordTableCell.PreferredWidthType = WidthType.Fixed;
            WordTableCell.PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(2.08f);
            WordTable.MergeCells(WordTableCell , WordTable[2 , 2]);


            SetTableStr(1 , 3 , "本日\r\n風險價格係數平均值");
            WordTableCell.PreferredWidthType = WidthType.Fixed;
            WordTableCell.PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(1.9f);
            WordTable.MergeCells(WordTableCell , WordTable[2 , 3]);

            SetTableRNStr(1 , 4 , "本日\r\n收盤價/結算價");
            SetTableRNStr(1 , 6 , "本日\r\n未沖銷部位數");
            WordTableCell.PreferredWidthType = WidthType.Fixed;
            WordTableCell.PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(1.6f);
            SetTableRNStr(1 , 7 , "本日\r\n成交量");
            WordTableCell.PreferredWidthType = WidthType.Fixed;
            WordTableCell.PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(1.6f);
            WordTable.MergeCells(WordTableCell , WordTable[2 , 7]);
            WordTable.MergeCells(WordTable[1 , 6] , WordTable[2 , 6]);
            WordTable.MergeCells(WordTable[1 , 4] , WordTable[1 , 5]);

            SetTableStr(2 , 4 , "現貨");
            WordTable[2,4].PreferredWidthType = WidthType.Fixed;
            WordTable[2, 4].PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(0.9f);
            SetTableStr(2 , 5 , "現貨");
            WordTable[2, 5].PreferredWidthType = WidthType.Fixed;
            WordTable[2, 5].PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(0.9f);

                string[] colName = new string[] { "t_30_rate" , "mgr2_day_rate" , "tfxm1_price" , "ai5_price" , "ai2_oi" , "ai2_m_qnty" };
            string[] fieldFormat = new string[] { "%" , "%" , "#,##0.##" , "#,##0.##" , "#,##0" , "#,##0" };

            foreach (DataRow dr in dtSTF.Rows) {
               TableRow tableRow = WordTable.Rows.Append();
               WordTableCell = tableRow.FirstCell;

               Doc.InsertSingleLineText(WordTable[tableRow.Index , 0].Range.Start , dr["mgr3_kind_id"].AsString());
               Doc.InsertSingleLineText(WordTable[tableRow.Index , 1].Range.Start , dr["apdk_name"].AsString());

               int formatindex = 0;
               int colindex = 2;
               foreach (string col in colName) {
                  if (fieldFormat[formatindex] == "%") {
                     Doc.InsertSingleLineText(WordTable[tableRow.Index , colindex].Range.Start , dr[col].AsPercent(2));
                  } else {
                     Doc.InsertSingleLineText(WordTable[tableRow.Index , colindex].Range.Start , dr[col].AsDecimal().ToString(fieldFormat[formatindex]));
                  }
                  formatindex++;
                  colindex++;
               }
            }
         }
      }

      /// <summary>
      /// 替換文字用class
      /// </summary>
      private class M40030Word {
         //會議時間
         public string MeetingDate { get; set; }
         //會議地點
         public string MeetingAddress { get; set; }
         //主席
         public string Chairman { get; set; }
         //出席
         public string Attend { get; set; }

         public M40030Word(string meetingdate , string chairman , string attend , string meetingaddress = "研討室") {
            MeetingDate = meetingdate;
            MeetingAddress = meetingaddress;
            Chairman = chairman;
            Attend = attend;
         }
      }

      /// <summary>
      /// interface AmtType + ProdType 設定一些參數
      /// </summary>
      private interface I40030AmtProdType {
         //幣別
         string CurrencyName { get; set; }
         //商品名
         string ProdName { get; set; }
         //調整前欄位名
         string BeforeAdjustTitle { get; set; }
         //調整後欄位名
         string AfterAdjustTitle { get; set; }
         //數字格式
         string NumberFormat { get; set; }

         //B值數字格式
         string NumberFormatB { get; set; }
         //表格名
         string[] TableTitle { get; set; }
         // 列名稱
         string[] RowName { get; set; }
         //資料欄位名
         string[] DbColName { get; set; }
         //資料B值欄位名
         string[] DbColNameB { get; set; }

         string AbbrName { get; set; }
         string StockName { get; set; }
         string KindId { get; set; }
         string StockId { get; set; }
      }

      private class AmtProdType40030FF : I40030AmtProdType {
         public string CurrencyName { get; set; }
         public string ProdName { get; set; }
         public string BeforeAdjustTitle { get; set; }
         public string AfterAdjustTitle { get; set; }
         public string NumberFormat { get; set; }

         public string NumberFormatB { get; set; }
         public string[] TableTitle { get; set; }
         public string[] RowName { get; set; }
         public string[] DbColName { get; set; }
         public string[] DbColNameB { get; set; }

         public string AbbrName { get; set; }
         public string StockName { get; set; }
         public string KindId { get; set; }
         public string StockId { get; set; }

         public AmtProdType40030FF(DataRow dr) {
            CurrencyName = "單位：" + dr["currency_name"].AsString();

            if (dr["prod_subtype"].AsString() == "S") {
               ProdName = $"{dr["kind_id_out"].AsString()}{Characters.LineBreak}({dr["kind_abbr_name"].AsString()})";
            } else {
               ProdName = $"{dr["kind_id_out"].AsString()}";
            }

            DbColName = new string[] { "m_im" , "m_mm" , "m_cm" , "cur_im" , "cur_mm" , "cur_cm" };

            AfterAdjustTitle = "調整後保證金金額";
            BeforeAdjustTitle = "調整前保證金金額";
            NumberFormat = "#,##0.##";
            TableTitle = new string[] { "原始保證金\r\n金額" , "維持保證金\r\n金額" , "結算保證金\r\n金額" };
            RowName = new[] { "保證金" };

            StockName = "，股票期貨標的證券代號 ";
            AbbrName = dr["kind_abbr_name"].AsString();
            KindId = dr["kind_id"].AsString();
            StockId = dr["STOCK_ID"].AsString();
         }

      }

      private class AmtProdType40030PF : I40030AmtProdType {
         public string CurrencyName { get; set; }
         public string ProdName { get; set; }
         public string BeforeAdjustTitle { get; set; }
         public string AfterAdjustTitle { get; set; }
         public string NumberFormat { get; set; }

         public string NumberFormatB { get; set; }
         public string[] TableTitle { get; set; }
         public string[] RowName { get; set; }
         public string[] DbColName { get; set; }
         public string[] DbColNameB { get; set; }

         public string AbbrName { get; set; }
         public string StockName { get; set; }
         public string KindId { get; set; }
         public string StockId { get; set; }

         public AmtProdType40030PF(DataRow dr) {
            CurrencyName = "單位：比例(%)";

            if (dr["prod_subtype"].AsString() == "S") {
               ProdName = $"{dr["kind_id_out"].AsString()}{Characters.LineBreak}({dr["kind_abbr_name"].AsString()})";
            } else {
               ProdName = $"{dr["kind_id_out"].AsString()}";
            }

            DbColName = new string[] { "m_im" , "m_mm" , "m_cm" , "cur_im" , "cur_mm" , "cur_cm" };

            AfterAdjustTitle = "調整後保證金適用比例";
            BeforeAdjustTitle = "調整前保證金適用比例";
            NumberFormat = "#,##0.##";
            TableTitle = new string[] { "原始保證金" , "維持保證金" , "結算保證金" };
            RowName = new[] { "保證金" };

            StockName = "，股票期貨標的證券代號 ";
            AbbrName = dr["kind_abbr_name"].AsString();
            KindId = dr["kind_id"].AsString();
            StockId = dr["STOCK_ID"].AsString();
         }

      }

      private class AmtProdType40030FO : I40030AmtProdType {
         public string CurrencyName { get; set; }
         public string ProdName { get; set; }
         public string BeforeAdjustTitle { get; set; }
         public string AfterAdjustTitle { get; set; }
         public string NumberFormat { get; set; }

         public string NumberFormatB { get; set; }
         public string[] TableTitle { get; set; }
         public string[] RowName { get; set; }
         public string[] DbColName { get; set; }
         public string[] DbColNameB { get; set; }

         public string AbbrName { get; set; }
         public string StockName { get; set; }
         public string KindId { get; set; }
         public string StockId { get; set; }

         public AmtProdType40030FO(DataRow dr) {
            CurrencyName = "單位：" + dr["currency_name"].AsString();

            if (dr["prod_subtype"].AsString() == "S") {
               ProdName = $"{dr["kind_id_out"].AsString()}{Characters.LineBreak}({dr["kind_abbr_name"].AsString()})";
            } else {
               ProdName = $"{dr["kind_id_out"].AsString()}";
            }

            DbColName = new string[] { "m_im" , "m_mm" , "m_cm" , "cur_im" , "cur_mm" , "cur_cm" };
            DbColNameB = new string[] { "m_im_b" , "m_mm_b" , "m_cm_b" , "cur_im_b" , "cur_mm_b" , "cur_cm_b" };

            AfterAdjustTitle = "調整後保證金金額";
            BeforeAdjustTitle = "調整前保證金金額";
            NumberFormat = "#,##0.##";
            NumberFormatB = "#,##0.###";
            TableTitle = new string[] { "計算賣出選擇權原始保證金之適用金額" , "計算賣出選擇權維持保證金之適用金額" , "計算賣出選擇權結算保證金之適用金額" };
            RowName = new string[] { "風險保證金（A值）" , "風險保證金最低值（B值）" };

            StockName = "，股票選擇權標的證券代號 ";
            AbbrName = dr["kind_abbr_name"].AsString();
            KindId = dr["kind_id"].AsString();
            StockId = dr["STOCK_ID"].AsString();
         }

      }

      private class AmtProdType40030PO : I40030AmtProdType {
         public string CurrencyName { get; set; }
         public string ProdName { get; set; }
         public string BeforeAdjustTitle { get; set; }
         public string AfterAdjustTitle { get; set; }
         public string NumberFormat { get; set; }

         public string NumberFormatB { get; set; }
         public string[] TableTitle { get; set; }
         public string[] RowName { get; set; }
         public string[] DbColName { get; set; }
         public string[] DbColNameB { get; set; }

         public string AbbrName { get; set; }
         public string StockName { get; set; }
         public string KindId { get; set; }
         public string StockId { get; set; }

         public AmtProdType40030PO(DataRow dr) {
            CurrencyName = "單位：比例(%)";

            if (dr["prod_subtype"].AsString() == "S") {
               ProdName = $"{dr["kind_id_out"].AsString()}{Characters.LineBreak}({dr["kind_abbr_name"].AsString()})";
            } else {
               ProdName = $"{dr["kind_id_out"].AsString()}";
            }

            DbColName = new string[] { "m_im" , "m_mm" , "m_cm" , "cur_im" , "cur_mm" , "cur_cm" };
            DbColNameB = new string[] { "m_im_b" , "m_mm_b" , "m_cm_b" , "cur_im_b" , "cur_mm_b" , "cur_cm_b" };

            AfterAdjustTitle = "調整後保證金金額";
            BeforeAdjustTitle = "調整前保證金金額";
            NumberFormat = "#,##0.##";
            NumberFormatB = "#,##0.###";
            TableTitle = new string[] { "計算賣出選擇權原始保證金之適用比例" , "計算賣出選擇權維持保證金之適用比例" , "計算賣出選擇權結算保證金之適用比例" };
            RowName = new string[] { "風險保證金（A值）" , "風險保證金最低值（B值）" };

            StockName = "，股票選擇權標的證券代號 ";
            AbbrName = dr["kind_abbr_name"].AsString();
            KindId = dr["kind_id"].AsString();
            StockId = dr["STOCK_ID"].AsString();
         }

      }

      /// <summary>
      /// 指數類商品參數 kinf_id
      /// </summary>
      private interface I40030KindInfoI {
         int RowCount { get; set; }
         int ColCount { get; set; }
         string TableName { get; set; }
         string[] ColTtile { get; set; }
         string[] RowTitle { get; set; }
         string[] FieldFormat { get; set; }
      }

      private class KindInfo40030TXF : I40030KindInfoI {
         public int RowCount { get; set; }
         public int ColCount { get; set; }
         public string TableName { get; set; }
         public string[] ColTtile { get; set; }
         public string[] RowTitle { get; set; }
         public string[] FieldFormat { get; set; }

         public KindInfo40030TXF() {
            RowCount = 3;
            ColCount = 5;
            TableName = "本公司臺股期貨期貨保證金占契約價值比與國際主要交易所比較表";
            FieldFormat = new string[] { "#,##0.##" , "%" , "%" , "#,##0.##" , "%" , "%" };
            RowTitle = new string[] { "臺股期貨契約原始保證金", "結算保證金占契約總值比例" , "原始保證金占契約總值比例" };
            ColTtile = new string[] { $"TAIFEX{Characters.LineBreak}TXF{Characters.LineBreak}(新臺幣元)",
                                      $"SGX{Characters.LineBreak}摩臺指期貨{Characters.LineBreak}(美元)"};
         }
      }

      private class KindInfo40030BRF : I40030KindInfoI {
         public int RowCount { get; set; }
         public int ColCount { get; set; }
         public string TableName { get; set; }
         public string[] ColTtile { get; set; }
         public string[] RowTitle { get; set; }
         public string[] FieldFormat { get; set; }

         public KindInfo40030BRF() {
            RowCount = 3;
            ColCount = 5;
            TableName = "本公司布蘭特原油期貨保證金占契約價值比與國際主要交易所比較表";
            RowTitle = new string[] { "布蘭特原油期貨契約原始保證金", "結算保證金占契約總值比例" , "原始保證金占契約總值比例" };
            FieldFormat = new string[] { "#,##0.##" , "%" , "%" , "#,##0.##" , "%" , "%" };
            ColTtile = new string[] { $"TAIFEX{Characters.LineBreak}BRF{Characters.LineBreak}(新臺幣元)",
                                    $"ICE{Characters.LineBreak}Brent Crude Futures{Characters.LineBreak}(美元)"};
         }
      }

      private class KindInfo40030TJF : I40030KindInfoI {
         public int RowCount { get; set; }
         public int ColCount { get; set; }
         public string TableName { get; set; }
         public string[] ColTtile { get; set; }
         public string[] RowTitle { get; set; }
         public string[] FieldFormat { get; set; }

         public KindInfo40030TJF() {
            RowCount = 3;
            ColCount = 7;
            TableName = "本公司東證期貨保證金占契約價值比與國際主要交易所比較表";
            FieldFormat = new string[] { "#,##0.##" , "%" , "%" , "#,##0.##" , "%" , "%" };
            RowTitle = new string[] { "東證期貨契約原始保證金", "結算保證金占契約總值比例" , "原始保證金占契約總值比例" };
            ColTtile = new string[] { $"TAIFEX{Characters.LineBreak}TJF{Characters.LineBreak}(新臺幣元)",
                                      $"JPX TOPIX{Characters.LineBreak}Futures{Characters.LineBreak}(日幣)",
                                      $"JPX mini TOPIX{Characters.LineBreak}Futures{Characters.LineBreak}(日幣)"};
         }
      }

      private class KindInfo40030I5F : I40030KindInfoI {
         public int RowCount { get; set; }
         public int ColCount { get; set; }
         public string TableName { get; set; }
         public string[] ColTtile { get; set; }
         public string[] RowTitle { get; set; }
         public string[] FieldFormat { get; set; }

         public KindInfo40030I5F() {
            RowCount = 3;
            ColCount = 5;
            TableName = "本公司Nifty50期貨保證金占契約價值比與國際主要交易所比較表";
            FieldFormat = new string[] { "#,##0.##" , "%" , "%" , "#,##0.##" , "%" , "%" };
            RowTitle = new string[] { "Nifty50期貨契約原始保證金", "結算保證金占契約總值比例" , "原始保證金占契約總值比例" };
            ColTtile = new string[] { $"TAIFEX{Characters.LineBreak}I5F{Characters.LineBreak}(新臺幣元)",
                                      $"SGX{Characters.LineBreak}(美元)"};
         }
      }

      private class KindInfo40030UDF : I40030KindInfoI {
         public int RowCount { get; set; }
         public int ColCount { get; set; }
         public string TableName { get; set; }
         public string[] ColTtile { get; set; }
         public string[] RowTitle { get; set; }
         public string[] FieldFormat { get; set; }

         public KindInfo40030UDF() {
            RowCount = 3;
            ColCount = 5;
            TableName = "本公司美國道瓊期貨保證金占契約價值比與國際主要交易所比較表";
            FieldFormat = new string[] { "#,##0.##" , "%" , "%" , "#,##0.##" , "%" , "%" };
            RowTitle = new string[] { "道瓊期貨契約原始保證金", "結算保證金占契約總值比例" , "原始保證金占契約總值比例" };
            ColTtile = new string[] { $"TAIFEX{Characters.LineBreak}UDF{Characters.LineBreak}(新臺幣元)",
                                      $"CME{Characters.LineBreak}E-mini Dow($5){Characters.LineBreak}Futures{Characters.LineBreak}(美元)"};
         }
      }

      private class KindInfo40030SPF : I40030KindInfoI {
         public int RowCount { get; set; }
         public int ColCount { get; set; }
         public string TableName { get; set; }
         public string[] ColTtile { get; set; }
         public string[] RowTitle { get; set; }
         public string[] FieldFormat { get; set; }

         public KindInfo40030SPF() {
            RowCount = 3;
            ColCount = 5;
            TableName = "本公司美國標普500期貨保證金占契約價值比與國際主要交易所比較表";
            FieldFormat = new string[] { "#,##0.##" , "%" , "%" , "#,##0.##" , "%" , "%" };
            RowTitle = new string[] { "標普500期貨契約原始保證金", "結算保證金占契約總值比例" , "原始保證金占契約總值比例" };
            ColTtile = new string[] { $"TAIFEX{Characters.LineBreak}SPF{Characters.LineBreak}(新臺幣元)",
                                      $"CME{Characters.LineBreak}E-mini S&P{Characters.LineBreak}500 Futures{Characters.LineBreak}(美元)"};
         }
      }

      private class KindInfo40030UNF : I40030KindInfoI {
         public int RowCount { get; set; }
         public int ColCount { get; set; }
         public string TableName { get; set; }
         public string[] ColTtile { get; set; }
         public string[] RowTitle { get; set; }
         public string[] FieldFormat { get; set; }

         public KindInfo40030UNF() {
            RowCount = 3;
            ColCount = 5;
            TableName = "本公司美國那斯達克100期貨保證金占契約價值比與國際主要交易所";
            FieldFormat = new string[] { "#,##0.##" , "%" , "%" , "#,##0.##" , "%" , "%" };
            RowTitle = new string[] { "美國那斯達克100期貨契約原始保證金", "結算保證金占契約總值比例" , "原始保證金占契約總值比例" };
            ColTtile = new string[] { $"TAIFEX{Characters.LineBreak}UNF{Characters.LineBreak}(新臺幣元)",
                                      $"CME{Characters.LineBreak}E-mini{Characters.LineBreak}NASDAQ{Characters.LineBreak}100 Futures{Characters.LineBreak}(美元)"};
         }
      }

      /// <summary>
      /// ETF 商品參數 kinf_id
      /// </summary>
      private interface I40030KindInfoE {
         int RowCount { get; set; }
         int ColCount { get; set; }
         string CurrencyName { get; set; }
         string[] TableName { get; set; }
         string[] NowOrAfter { get; set; }
         string[] ColTtile { get; set; }
         string[] RowTitle { get; set; }
         string[] FieldFormat { get; set; }

         string[] DbCol(string nowORAfter);
      }

      private class KindInfo40030Gold : I40030KindInfoE {
         public int RowCount { get; set; }
         public int ColCount { get; set; }
         public string CurrencyName { get; set; }
         public string[] NowOrAfter { get; set; }
         public string[] TableName { get; set; }
         public string[] ColTtile { get; set; }
         public string[] RowTitle { get; set; }
         public string[] FieldFormat { get; set; }

         public KindInfo40030Gold() {
            RowCount = 2;
            ColCount = 6;
            NowOrAfter = new string[] { "現行" , "調整後" };
            TableName = new string[] { "本公司{0}黃金期貨保證金占契約價值比與國際主要黃金交易所比較表",
                                       "本公司{0}黃金期貨保證金與國際主要黃金交易所保證金比較表" };

            FieldFormat = new string[] { "#,##0.##" , "%" , "%" };
            RowTitle = new string[] { "黃金期貨契約原始保證金", "結算保證金占契約總值比例" , "原始保證金占契約總值比例" };
            ColTtile = new string[] { "交易所",
                                      $"GDF{Characters.LineBreak}TAIFEX{Characters.LineBreak}(美元)",
                                      $"TGF{Characters.LineBreak}TAIFEX{Characters.LineBreak}(新臺幣元)",
                                      $"NYSE/LIFFE{Characters.LineBreak}(美元)",
                                      $"CME/COMEX{Characters.LineBreak}(美元)",
                                      $"TOCOM{Characters.LineBreak}(日幣)"};
         }

         public string[] DbCol(string nowORAfter) {
            string[] dbCurCols = new string[] { "cur_cm" , "cur_cm_rate" , "cur_im_rate" };
            string[] dbMCols = new string[] { "m_cm" , "m_cm_rate" , "m_im_rate" };

            return nowORAfter == "現行" ? dbCurCols : dbMCols;
         }
      }

      private class KindInfo40030XBF : I40030KindInfoE {
         public int RowCount { get; set; }
         public int ColCount { get; set; }
         public string CurrencyName { get; set; }
         public string[] NowOrAfter { get; set; }
         public string[] TableName { get; set; }
         public string[] ColTtile { get; set; }
         public string[] RowTitle { get; set; }
         public string[] FieldFormat { get; set; }

         public KindInfo40030XBF() {
            RowCount = 2;
            ColCount = 3;
            CurrencyName = "單位：美元";
            NowOrAfter = new string[] { "現行" , "調整後" };
            TableName = new string[] { "本公司{0}英鎊兌美元匯率期貨保證金占契約價值比與國際主要交易所比較表",
                                       "本公司{0}英鎊兌美元匯率期貨保證金與國際主要交易所保證金比較表" };

            FieldFormat = new string[] { "#,##0.##" , "%" , "%" };
            RowTitle = new string[] { $"英鎊兌美元{Characters.LineBreak}匯率期貨契約{Characters.LineBreak}原始保證金" , "結算保證金占契約總值比例" , "原始保證金占契約總值比例" };
            ColTtile = new string[] { "交易所",
                                      $"XBF{Characters.LineBreak}TAIFEX",
                                      $"CME{Characters.LineBreak}"};
         }

         public string[] DbCol(string nowORAfter) {
            string[] dbCurCols = new string[] { "cur_cm" , "cur_cm_rate" , "cur_im_rate" };
            string[] dbMCols = new string[] { "m_cm" , "m_cm_rate" , "m_im_rate" };

            return nowORAfter == "現行" ? dbCurCols : dbMCols;
         }
      }

      private class KindInfo40030XEF : I40030KindInfoE {
         public int RowCount { get; set; }
         public int ColCount { get; set; }
         public string CurrencyName { get; set; }
         public string[] NowOrAfter { get; set; }
         public string[] TableName { get; set; }
         public string[] ColTtile { get; set; }
         public string[] RowTitle { get; set; }
         public string[] FieldFormat { get; set; }

         public KindInfo40030XEF() {
            RowCount = 2;
            ColCount = 3;
            CurrencyName = "單位：美元";
            NowOrAfter = new string[] { "現行" , "調整後" };
            TableName = new string[] { "本公司{0}歐元兌美元匯率期貨保證金占契約價值比與國際主要交易所比較表",
                                       "本公司{0}歐元兌美元匯率期貨保證金與國際主要交易所保證金比較表" };

            FieldFormat = new string[] { "#,##0.##" , "%" , "%" };
            RowTitle = new string[] { $"歐元兌美元{Characters.LineBreak}匯率期貨契約{Characters.LineBreak}原始保證金" , "結算保證金占契約總值比例" , "原始保證金占契約總值比例" };
            ColTtile = new string[] { "交易所",
                                      $"XEF{Characters.LineBreak}TAIFEX",
                                      $"CME{Characters.LineBreak}"};
         }

         public string[] DbCol(string nowORAfter) {
            string[] dbCurCols = new string[] { "cur_cm" , "cur_cm_rate" , "cur_im_rate" };
            string[] dbMCols = new string[] { "m_cm" , "m_cm_rate" , "m_im_rate" };

            return nowORAfter == "現行" ? dbCurCols : dbMCols;
         }
      }

      private class KindInfo40030XJF : I40030KindInfoE {
         public int RowCount { get; set; }
         public int ColCount { get; set; }
         public string CurrencyName { get; set; }
         public string[] NowOrAfter { get; set; }
         public string[] TableName { get; set; }
         public string[] ColTtile { get; set; }
         public string[] RowTitle { get; set; }
         public string[] FieldFormat { get; set; }

         public KindInfo40030XJF() {
            RowCount = 2;
            ColCount = 3;
            CurrencyName = "日圓";
            NowOrAfter = new string[] { "現行" , "調整後" };
            TableName = new string[] { "本公司{0}美元兌日圓匯率期貨保證金占契約價值比與國際主要交易所比較表",
                                       "本公司{0}美元兌日圓匯率期貨保證金與國際主要交易所保證金比較表" };

            FieldFormat = new string[] { "#,##0.##" , "%" , "%" };
            RowTitle = new string[] { $"美元兌日圓{Characters.LineBreak}匯率期貨契約{Characters.LineBreak}原始保證金" , "結算保證金占契約總值比例" , "原始保證金占契約總值比例" };
            ColTtile = new string[] { "交易所",
                                      $"XJF{Characters.LineBreak}TAIFEX",
                                      $"SGX{Characters.LineBreak}"};
         }

         public string[] DbCol(string nowORAfter) {
            string[] dbCurCols = new string[] { "cur_cm" , "cur_cm_rate" , "cur_im_rate" };
            string[] dbMCols = new string[] { "m_cm" , "m_cm_rate" , "m_im_rate" };

            return nowORAfter == "現行" ? dbCurCols : dbMCols;
         }
      }

      private class KindInfo40030XAF : I40030KindInfoE {
         public int RowCount { get; set; }
         public int ColCount { get; set; }
         public string CurrencyName { get; set; }
         public string[] NowOrAfter { get; set; }
         public string[] TableName { get; set; }
         public string[] ColTtile { get; set; }
         public string[] RowTitle { get; set; }
         public string[] FieldFormat { get; set; }

         public KindInfo40030XAF() {
            RowCount = 2;
            ColCount = 3;
            CurrencyName = "單位：美元";
            NowOrAfter = new string[] { "現行" , "調整後" };
            TableName = new string[] { "本公司{0}澳幣兌美元匯率期貨保證金占契約價值比與國際主要交易所比較表",
                                       "本公司{0}澳幣兌美元匯率期貨保證金與國際主要交易所保證金比較表" };

            FieldFormat = new string[] { "#,##0.##" , "%" , "%" };
            RowTitle = new string[] { $"澳幣兌美元{Characters.LineBreak}匯率期貨契約{Characters.LineBreak}原始保證金" , "結算保證金占契約總值比例" , "原始保證金占契約總值比例" };
            ColTtile = new string[] { "交易所",
                                      $"XAF{Characters.LineBreak}TAIFEX",
                                      $"CME{Characters.LineBreak}"};
         }

         public string[] DbCol(string nowORAfter) {
            string[] dbCurCols = new string[] { "cur_cm" , "cur_cm_rate" , "cur_im_rate" };
            string[] dbMCols = new string[] { "m_cm" , "m_cm_rate" , "m_im_rate" };

            return nowORAfter == "現行" ? dbCurCols : dbMCols;
         }
      }

      private class KindInfo40030RHF : I40030KindInfoE {
         public int RowCount { get; set; }
         public int ColCount { get; set; }
         public string CurrencyName { get; set; }
         public string[] NowOrAfter { get; set; }
         public string[] TableName { get; set; }
         public string[] ColTtile { get; set; }
         public string[] RowTitle { get; set; }
         public string[] FieldFormat { get; set; }

         public KindInfo40030RHF() {
            RowCount = 2;
            ColCount = 6;
            CurrencyName = "單位：人民幣";
            NowOrAfter = new string[] { "現行" , "調整後" };
            TableName = new string[] { "本公司{0}美元兌人民幣匯率期貨保證金占契約價值比與國際主要交易所比較表",
                                       "本公司{0}美元兌人民幣匯率期貨保證金與國際主要交易所保證金比較表" };

            FieldFormat = new string[] { "#,##0.##" , "%" , "%" };
            RowTitle = new string[] { $"美元兌人民幣{Characters.LineBreak}匯率期貨契約{Characters.LineBreak}原始保證金" , "結算保證金占契約總值比例" , "原始保證金占契約總值比例" };
            ColTtile = new string[] { "交易所",
                                      $"RHF{Characters.LineBreak}TAIFEX",
                                      $"RTF{Characters.LineBreak}TAIFEX",
                                      "HKEX","CME","SGX"};
         }

         public string[] DbCol(string nowORAfter) {
            string[] dbCurCols = new string[] { "cur_cm" , "cur_cm_rate" , "cur_im_rate" };
            string[] dbMCols = new string[] { "m_cm" , "m_cm_rate" , "m_im_rate" };

            return nowORAfter == "現行" ? dbCurCols : dbMCols;
         }
      }

      /// <summary>
      /// 選擇 一般時的勾選群組
      /// </summary>
      private class CheckedItem {
         public int CheckedValue { get; set; }
         public DateTime CheckedDate { get; set; }
         public string ETCSelected { get; set; }
      }
   }
}