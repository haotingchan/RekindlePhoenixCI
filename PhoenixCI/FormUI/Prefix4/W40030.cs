using BaseGround;
using BaseGround.Shared;
using BaseGround.Widget;
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

/// <summary>
/// Test Data 3B 20181228 / 1B 20190129 / 1E 20190129 / 0B 20190212
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

         //設定 下拉選單
         List<LookupItem> marketTimeList = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "1", DisplayMember = "Group1(13:45)"},
                                        new LookupItem() { ValueMember = "2", DisplayMember = "Group2(16:15)" }};

         //設定下拉選單
         ddlAdjType.SetDataTable(lstType, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, null);
         ETCSelect.SetDataTable(marketTimeList, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, null);
         ETCSelect.EditValue = "1";
         ddlAdjType.EditValue = "0B";

         MarketTimes.SelectedIndex = 1;
#if DEBUG
         txtDate.DateTimeValue = "2018/12/28".AsDateTime("yyyy/MM/dd");
         ddlAdjType.EditValue = "0B";
#endif

         ExportShow.Hide();
      }

      protected override ResultStatus Export() {
         ExportShow.Text = "轉檔中...";
         ExportShow.Show();
         try {

            List<CheckedItem> checkedItems = new List<CheckedItem>();
            foreach (CheckedListBoxItem c in MarketTimes.CheckedItems) {
               TextDateEdit control = (TextDateEdit)this.Controls.Find("txtDate" + c.Value.AsString(), true).FirstOrDefault();

               checkedItems.Add(
                  new CheckedItem {
                     CheckedValue = c.Value.AsInt(),
                     CheckedDate = control.DateTimeValue,
                     ETCSelected = ETCSelect.EditValue.AsString()
                  });
            }

            object[] args = { TxtDate, AdjType, _ProgramID, checkedItems };
            IExport40xxxData xmlData = CreateXmlData(GetType(), "ExportWord" + AdjType, args);
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

      private void ddlAdjType_EditValueChanged(object sender, EventArgs e) {

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


      private class ExportWord : IExport40xxxData {
         protected RPTF DaoRptf { get; }
         protected D40030 Dao40030 { get; }
         protected virtual string TxtDate { get; }
         protected virtual string AdjType { get; }
         protected string ProgramId { get; }

         protected virtual DataTable Dt { get; set; }
         protected virtual string AsAdjType {
            get {
               return AdjType.SubStr(0, 1);
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

         public ExportWord(string txtdate, string adjtype, string programId, List<CheckedItem> checkeditems) {
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

         public virtual ReturnMessageClass Export() {
            ReturnMessageClass msg = new ReturnMessageClass();
            msg.Status = ResultStatus.Fail;

            try {
               FilePath = PbFunc.wf_copy_file(ProgramId, MeetingLogFileName);

               //取得會議紀錄 / 議程資訊
               GetRPTF();

               //會議記錄
               OpenFile();

               //出席者 / 案由
               SetHead();

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
               DocSer.SaveDocument(FilePath, DocumentFormat.Rtf);
               DocSer.Dispose();
#if DEBUG
               System.Diagnostics.Process.Start(FilePath);
#endif
               //end 會議記錄

               //議程
               FilePath = PbFunc.wf_copy_file(ProgramId, AgendaFileName);

               OpenFile();

               //出席者 / 案由
               SetHead();

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
               DocSer.SaveDocument(FilePath, DocumentFormat.Rtf);
               DocSer.Dispose();

#if DEBUG
               System.Diagnostics.Process.Start(FilePath);
#endif
               //end 議程

               msg.Status = ResultStatus.Success;
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

         protected virtual string GenArrayTxt(List<string> strList) {
            string result = "";
            int k = 1;

            foreach (string s in strList) {
               result += s;
               if (k < strList.Count()) {
                  if (s != strList[strList.Count() - 2]) {
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
            string week = System.Globalization.DateTimeFormatInfo.GetInstance(
                           new System.Globalization.CultureInfo("zh-CHT")).DayNames[(byte)dtNow.DayOfWeek];

            string result = $"{date}({week})下午5時10分";

            return result;
         }

         protected virtual void SetHead() {
            string chairman = DtMinutes.Rows[0]["RPTF_TEXT"].AsString();

            SetSubjectText("案　　由： ");

            SetInnerText(CaseDescStr.Replace("#kind_name_list#", GenProdName(Dt, "契約")), false, 2.75f, 2.75f);

            SetDescStr();

            SetRtfDescText(GenMeetingDate(), chairman, GenAttend(DtMinutes));
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

         protected virtual void SetRtfDescText(string meetingDate, string chairman, string attend) {
            M40030Word m40030 = new M40030Word(meetingDate, chairman, attend);
            //Options.MailMerge 要用List 才會有作用
            List<M40030Word> listM40030 = new List<M40030Word>();
            listM40030.Add(m40030);

            //直接replace word 上面的字
            DocSer.Options.MailMerge.DataSource = listM40030;
            DocSer.Options.MailMerge.ViewMergedData = true;
         }

         protected virtual void SetDescStr() {
            string tempStr = "";
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

            SetSubjectText("說　　明： ");

            //說明一
            tempStr = string.Format("一、考量{0}年春節假期休市({1}至{2})長達{3}日，" +
                                    "參酌國外主要交易所逢較長假期採行調高保證金之風控措施，援引過往春節假期採行調高保證金之作法，" +
                                    "將於{0}年春節假期，調高{4}保證金。依本公司保證金調整作業規範，由督導結算業務主管召集業務相關部" +
                                    "門主管會商決定是否調整。", year, startYmd, endYmd, diffDays.ToString(), subTypeStr);
            SetInnerText(tempStr);

            //說明二
            tempStr = string.Format("二、有關調高{0}保證金，建議將結算保證金調高{1}，" +
                                    "維持保證金與原始保證金則依本公司訂定之成數加成計算，併同調高。" +
                                    "調高後之臺股期貨原始保證金能涵蓋之價格波動幅度為{2}點（{3}/200），" +
                                    "以{4}結算價{5}試算，可涵蓋{6}價格波動（{2}/{5}＝{6}）。",
                                    subTypeStr, adjRate, point, mim, dataYMD, settlePrice, dbRate);
            SetInnerText(tempStr);

            //說明三
            tempStr = string.Format("三、本次保證金調整實施期間自{0}一般交易時段結束後，" +
                                    "預計至{1}一般交易時段結束止。本公司於{2}，" +
                                    "另行公告前揭契約於{1}一般交易時段結束後起之保證金適用金額。",
                                    implBeginYmd.AsTaiwanDateTime(ymdFormat, 3), implEndYmd.AsTaiwanDateTime(ymdFormat, 3),
                                    mocfDate.AsTaiwanDateTime(ymdFormat, 3));
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
         protected virtual void CreateTable(Document doc, int rowCount, int colCount) {

            WordTable = doc.Tables.Create(doc.Range.End, rowCount, colCount);
            WordTable.TableAlignment = TableRowAlignment.Right;
            WordTable.PreferredWidthType = WidthType.Fixed;
            WordTable.PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(16.7f);
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
            tableProp.FontSize = 12;
            doc.EndUpdateCharacters(tableProp);

            // 垂直置中
            WordTable.ForEachCell((c, i, j) => {
               c.VerticalAlignment = TableCellVerticalAlignment.Center;
            });
         }

         /// <summary>
         /// 設定欄位名稱
         /// </summary>
         protected virtual void SetTableColTitle(string prodName, string[] colTitle, string afterAdjustTitle, string beforeAdjustTitle) {
            SetTableStr(0, 0, prodName);
            WordTableCell.PreferredWidthType = WidthType.Fixed;
            WordTableCell.PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(2.65f);
            WordTableCell.VerticalAlignment = TableCellVerticalAlignment.Center;
            WordTable.MergeCells(WordTableCell, WordTable[1, 0]);

            SetTableStr(0, 1, afterAdjustTitle);
            WordTable.MergeCells(WordTableCell, WordTable[0, 3]);

            SetTableStr(0, 2, beforeAdjustTitle);
            WordTable.MergeCells(WordTableCell, WordTable[0, 4]);

            SetTableTitle(colTitle, 1);
         }

         /// <summary>
         /// 設定欄位名稱
         /// </summary>
         protected virtual void SetTableColValue(I40030AmtProdType iAmtProd, DataRow dr) {

            int k = 1;

            foreach (string rowName in iAmtProd.RowName) {

               TableRow tableRow = WordTable.Rows.Append();

               WordTableCell = tableRow.FirstCell;
               Doc.InsertSingleLineText(WordTableCell.Range.Start, rowName);

               //特殊處理, 選擇權時有AB值, k=1 跑保證金或A值, k>1 跑B值
               string[] colNameList = k == 1 ? iAmtProd.DbColName : iAmtProd.DbColNameB;
               string numberFormat = k == 1 ? iAmtProd.NumberFormat : iAmtProd.NumberFormatB;

               int i = 1;
               foreach (string col in colNameList) {
                  Doc.InsertSingleLineText(WordTable[tableRow.Index, i].Range.Start, !decimal.Equals(dr[col].AsDecimal(), 0) ?
                     dr[col].AsDecimal().ToString(numberFormat) : string.Empty);

                  i++;
               }

               k++;
            }

         }

         protected virtual void SetTableStr(int rowIndex, int colIndex, string str) {
            WordTableCell = WordTable[rowIndex, colIndex];
            Doc.InsertSingleLineText(WordTableCell.Range.Start, str);
         }

         protected virtual void SetTableTitle(string[] strList, int rowIndex) {

            int k = 0;
            foreach (string str in strList) {
               Doc.InsertSingleLineText(WordTable[rowIndex, k + 1].Range.Start, strList[k]);
               Doc.InsertSingleLineText(WordTable[rowIndex, k + 4].Range.Start, strList[k]);
               k++;
            }

         }

         protected virtual void SetInnerText(string str, bool hasFirstIndent = true, float leftIndent = 2.98f, float fitstLineIndent = 1.18f) {
            Doc.AppendText(Environment.NewLine);
            Doc.AppendText(str);

            ParagraphProps = Doc.BeginUpdateParagraphs(Doc.Paragraphs.Last().Range);
            ParagraphProps.Alignment = ParagraphAlignment.Left;
            ParagraphProps.LeftIndent = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(leftIndent);

            if (hasFirstIndent) {
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

         protected virtual void SetSubjectText(string str) {
            Doc.AppendText(Environment.NewLine);
            Doc.AppendText(str);

            ParagraphProps = Doc.BeginUpdateParagraphs(Doc.Paragraphs.Last().Range);
            ParagraphProps.Alignment = ParagraphAlignment.Left;
            ParagraphProps.LeftIndent = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(0);
            ParagraphProps.FirstLineIndentType = ParagraphFirstLineIndent.None;
            Doc.EndUpdateParagraphs(ParagraphProps);

            CharacterProps = Doc.BeginUpdateCharacters(Doc.Paragraphs.Last().Range);
            CharacterProps.FontSize = 14;
            CharacterProps.Bold = true;
            CharacterProps.FontName = "標楷體";
            Doc.EndUpdateCharacters(CharacterProps);
         }

         protected virtual void SetCurrencyName(I40030AmtProdType iAmtProdType, ParagraphAlignment paragraphAlignment = ParagraphAlignment.Right,
                                                int fontSize = 12, string fontName = "標楷體") {

            Doc.AppendText(iAmtProdType.CurrencyName);
            ParagraphProps = Doc.BeginUpdateParagraphs(Doc.Paragraphs.Last().Range);
            ParagraphProps.Alignment = paragraphAlignment;
            Doc.EndUpdateParagraphs(ParagraphProps);

            CharacterProps = Doc.BeginUpdateCharacters(Doc.Paragraphs.Last().Range);
            CharacterProps.FontSize = fontSize;
            CharacterProps.FontName = fontName;
            Doc.EndUpdateCharacters(CharacterProps);
         }

         protected virtual void DrowTable(DataTable dataTable) {
            Doc.AppendText(Environment.NewLine);

            foreach (DataRow dr in dataTable.Rows) {

               string amtType = dr["AMT_TYPE"].AsString();
               string prodType = dr["PROD_TYPE"].AsString();
               object[] args = new object[] { dr };
               I40030AmtProdType iAmtProdType = CreateI40030AmtProdType(GetType(), "AmtProdType40030" + amtType + prodType, args);

               SetCurrencyName(iAmtProdType);

               CreateTable(Doc, 2, 7);

               SetTableColTitle(iAmtProdType.ProdName, iAmtProdType.TableTitle, iAmtProdType.AfterAdjustTitle, iAmtProdType.BeforeAdjustTitle);

               SetTableColValue(iAmtProdType, dr);
            }

         }

         protected I40030AmtProdType CreateI40030AmtProdType(Type type, string name, object[] args = null) {
            string AssemblyName = type.Namespace.Split('.')[0];//最後compile出來的dll名稱
            string className = type.Namespace + "." + type.ReflectedType.Name + "+" + name;//完整的class路徑(注意,內部的class執行時其fullName是用+號連結起來)
            return (I40030AmtProdType)Assembly.Load(AssemblyName).CreateInstance(className, true, BindingFlags.CreateInstance, null, args, null, null);
         }

         public virtual void ErrorHandle(Exception ex, ReturnMessageClass msg) {
            WriteLog(ex.ToString(), "Info", "Z");
            msg.Status = ResultStatus.Fail;
            msg.ReturnMessage = MessageDisplay.MSG_IMPORT_FAIL;
         }

         /// <summary>
         /// 設定英文和數字的字型
         /// </summary>
         protected virtual void SetNumberAndEnglishFontName(Document doc, DocumentRange docRange) {
            CharacterProperties cp = doc.BeginUpdateCharacters(docRange);
            cp.FontName = "Times New Roman";
            doc.EndUpdateCharacters(cp);
         }

         /// <summary>
         /// 將整份文件的英文和數字的字型設成某個字型
         /// </summary>
         protected virtual void SetAllNumberAndEnglishFont(Document doc) {
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
      /// 長假調整 輸出rtf
      /// </summary>
      private class ExportWord1B : ExportWord {
         public ExportWord1B(string txtdate, string adjtype, string programId, List<CheckedItem> checkeditems) :
                     base(txtdate, adjtype, programId, checkeditems) {
            OswGrp = "%";
            CaseDescStr = "因應春節假期，擬調整本公司#kind_name_list#所有月份保證金金額案，謹提請討論。";//案由
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

      /// <summary>
      /// 長假回調 輸出rtf
      /// </summary>
      private class ExportWord1E : ExportWord {
         public ExportWord1E(string txtdate, string adjtype, string programId, List<CheckedItem> checkeditems) :
                     base(txtdate, adjtype, programId, checkeditems) {
            OswGrp = "%";
            CaseDescStr = "因應春節假期結束，擬回調本公司#kind_name_list#所有月份保證金金額案，謹提請討論。";//案由
         }

         public override ReturnMessageClass Export() {
            ReturnMessageClass msg = new ReturnMessageClass();
            msg.Status = ResultStatus.Fail;

            try {
               FilePath = PbFunc.wf_copy_file(ProgramId, MeetingLogFileName);

               //取得會議紀錄 / 議程資訊
               GetRPTF();

               //會議記錄
               OpenFile();

               //表頭 出席者 / 案由

               SetHead();

               #region 表格
               Doc.BeginUpdate();
               Doc.AppendText(Environment.NewLine);

               DrowTable(Dt);

               #endregion

               #region 表尾 決議 

               base.SetSubjectText("決　　議：");

               string implDate = Dt.Rows[0]["impl_begin_ymd"].AsString();
               string beginDate = Dt.Rows[0]["issue_begin_ymd"].AsString();
               string mocfDate = new MOCF().GetMaxOcfDate(implDate, beginDate).AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);
               string resolutionStr = string.Format("一、照案通過，於{0}公告回調金額，並自{1}一般交易時段結束後實施。",
                                                      mocfDate, beginDate.AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3));

               base.SetInnerText(resolutionStr);
               base.SetSubjectText("貳、臨時動議：無");
               base.SetSubjectText("參、散　　會：下午5時30分");
               #endregion

               base.SetAllNumberAndEnglishFont(Doc);//設定英數字體

               Doc.EndUpdate();
               DocSer.SaveDocument(FilePath, DocumentFormat.Rtf);
               DocSer.Dispose();
#if DEBUG
               System.Diagnostics.Process.Start(FilePath);
#endif
               //end 會議記錄

               //議程
               FilePath = PbFunc.wf_copy_file(ProgramId, AgendaFileName);

               OpenFile();

               //表頭 出席者 / 案由
               SetHead();

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
               DocSer.SaveDocument(FilePath, DocumentFormat.Rtf);
               DocSer.Dispose();
#if DEBUG
               System.Diagnostics.Process.Start(FilePath);
#endif
               //end 議程

               msg.Status = ResultStatus.Success;
               return msg;

            } catch (Exception ex) {
               base.ErrorHandle(ex, msg);
               return msg;
            }
         }

         protected override void SetDescStr() {
            string tempStr = "";

            IEnumerable<IGrouping<string, DataRow>> listSubType = Dt.AsEnumerable().GroupBy(d => d.Field<string>("PROD_SUBTYPE"));
            string subTypeStr = base.GenProdSubtypeList(listSubType, "契約");

            SetSubjectText("說　　明： ");

            //說明一
            tempStr = string.Format("一、本公司於春節假期調高{0}保證金，因應春節假期結束，擬回調前揭契約保證金。" +
                                       "依本公司保證金調整作業規範，由督導結算業務主管召集業務相關部門主管會商決定是否調整。",
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

         public ExportWord3B(string txtdate, string adjtype, string programId, List<CheckedItem> checkeditems) :
                     base(txtdate, adjtype, programId, checkeditems) {
            OswGrp = "%";
            CaseDescStr = "檢陳本公司#kind_name_list#保證金調整案，謹提請討論。";//案由
         }

         public override ReturnMessageClass Export() {
            ReturnMessageClass msg = new ReturnMessageClass();
            msg.Status = ResultStatus.Fail;

            try {
               FilePath = PbFunc.wf_copy_file(ProgramId, MeetingLogFileName);

               //取得會議紀錄 / 議程資訊
               GetRPTF();

               //會議記錄
               OpenFile();

               //表頭 出席者 / 案由
               SetHead();

               #region 表格
               Doc.BeginUpdate();
               Doc.AppendText(Environment.NewLine);

               DrowTable(Dt);
               #endregion

               #region 表尾 決議 

               base.SetSubjectText("決　　議：");
               string resolutionStr = string.Format("經考量市場風險及保證金保守穩健原則，調整{0}保證金適用比例調整如說明四。", GenArrayTxt(allKindNameList));

               base.SetInnerText(resolutionStr);
               base.SetSubjectText("貳、臨時動議：無");
               base.SetSubjectText("參、散　　會：下午5時30分");
               #endregion

               base.SetAllNumberAndEnglishFont(Doc);//設定英數字體

               Doc.EndUpdate();
               DocSer.SaveDocument(FilePath, DocumentFormat.Rtf);
               DocSer.Dispose();
#if DEBUG
               System.Diagnostics.Process.Start(FilePath);
#endif
               //end 會議記錄

               //會議議程
               FilePath = PbFunc.wf_copy_file(ProgramId, AgendaFileName);

               OpenFile();

               //表頭 出席者 / 案由
               SetHead();

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
               DocSer.SaveDocument(FilePath, DocumentFormat.Rtf);
               DocSer.Dispose();
#if DEBUG
               System.Diagnostics.Process.Start(FilePath);
#endif
               //end 會議議程

               msg.Status = ResultStatus.Success;
               return msg;

            } catch (Exception ex) {
               base.ErrorHandle(ex, msg);
               return msg;
            }
         }

         protected override string GenProdName(DataTable dt, string contract = "") {
            string result = "";
            int k = 0;

            foreach (DataRow dr in dt.Rows) {
               string amtType = dr["AMT_TYPE"].AsString();
               string prodType = dr["PROD_TYPE"].AsString();
               object[] args = new object[] { dr };
               I40030AmtProdType iAmtProdType = CreateI40030AmtProdType(GetType(), "AmtProdType40030" + amtType + prodType, args);

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
            string quarterYear = (inputDate.Month / 3) == 0 ? inputDate.AddYears(-1).AsTaiwanDateTime("{0}年", 3) :
                                    inputDate.AsTaiwanDateTime("{0}年", 3);

            //季評估 替換參數
            List<string> kindNameQuarter = new List<string>();
            List<string> mLevelQuarter = new List<string>();
            List<string> curLevelQuarter = new List<string>();
            List<string> dayRiskQuarter = new List<string>();

            DataTable dtQuarter = Dt.Select("adj_rsn='1'").CopyToDataTable();
            foreach (DataRow dr in dtQuarter.Rows) {
               string kindName = $"{dr["KIND_ABBR_NAME"].AsString()}({dr["kind_id"].AsString()})";
               string mLevel = dr["M_LEVEL_NAME"].AsString();
               string curLevel = dr["CUR_LEVEL_NAME"].AsString();

               mLevelQuarter.Add($"{mLevel}({dr["m_cm"].AsPercent(0)})");
               curLevelQuarter.Add($"{curLevel}({dr["cur_cm"].AsPercent(0)})");
               dayRiskQuarter.Add($"{dr["m_day_risk"].AsPercent(2)}");

               if (!kindNameQuarter.Exists(k => k == kindName)) {
                  kindNameQuarter.Add(kindName);
                  allKindNameList.Add(kindName);
               }
            }

            //機動評估 替換參數
            List<string> kindNameInReserve = new List<string>();
            List<string> levelInReserve = new List<string>();
            List<string> dayRiskInReserve = new List<string>();
            List<string> monthRiskInReserve = new List<string>();

            DataTable dtInReserve = Dt.Select("adj_rsn='2'").CopyToDataTable();
            foreach (DataRow dr in dtInReserve.Rows) {
               string kindName = $"{dr["KIND_ABBR_NAME"].AsString()}({dr["kind_id"].AsString()})";
               string mLevel = dr["M_LEVEL_NAME"].AsString();
               string curLevel = dr["CUR_LEVEL_NAME"].AsString();

               levelInReserve.Add(string.Format("{0}由{1}({2})調為{3}({4})",
                                    kindName, curLevel, dr["cur_cm"].AsPercent(0), mLevel, dr["m_cm"].AsPercent(0)));

               dayRiskInReserve.Add($"{dr["m_day_risk"].AsPercent(2)}");
               monthRiskInReserve.Add($"{dr["m_30_risk"].AsPercent(0)}");

               if (!kindNameInReserve.Exists(k => k == kindName)) {
                  kindNameInReserve.Add(kindName);
                  allKindNameList.Add(kindName);
               }
            }

            SetSubjectText("說　　明： ");

            //說明一
            SetInnerText("一、依本公司「結算保證金收取方式及標準」與「保證金調整作業規範」辦理。");

            //說明二
            tempStr = string.Format("二、依前述作業規範，本部定期或機動評估股票期貨及股票選擇權結算保證金風險價格係數及保證金適用級距，" +
                                    "有關{0}第{1}季檢討結果，業經▲年▲月▲日▲▲▲號簽奉核可在案▲▲▲。",
                                    quarterYear, quarter);
            SetInnerText(tempStr);

            //說明三
            tempStr = string.Format("三、依前簽核示，建議調整保證金適用級距之股票期貨契約共計{0}檔，說明如下：", Dt.Rows.Count);
            SetInnerText(tempStr);

            //季評估
            string these = kindNameQuarter.Count() > 1 ? "等" : "";
            string respectively = kindNameQuarter.Count() > 1 ? "分別" : "";


            tempStr = string.Format("(一)、依股票期貨風險係數估算方式，現行結算保證金適用級距與第{0}季▲▲▲評估結果分屬不同級距者，" +
                                    "計有{1}，該{2}契約現行結算保證金級距{3}為{4}，經試算其標的證券風險價格係數平均值為{5}，{3}適用{6}，建議調整保證金適用級距。",
                                    quarter, GenArrayTxt(kindNameQuarter), these, respectively, GenArrayTxt(curLevelQuarter),
                                    GenArrayTxt(dayRiskQuarter), GenArrayTxt(mLevelQuarter));

            SetInnerText(tempStr, true, 4.11f, 1.25f);

            //機動評估
            these = kindNameInReserve.Count() > 1 ? "等" : "";
            respectively = kindNameInReserve.Count() > 1 ? "分別" : "";

            tempStr = string.Format("(二)、依股票期貨機動評估指標，各股票期貨契約30天期風險價格係數較現行結算保證金適用比例變動幅度連續3個交易日達10%，" +
                                    "且30天期風險價格係數與2年平均值取孰高者，該值所在級距與現行適用級距不同時，即進行機動檢討，並以30天期風險價格係數所在級距，" +
                                    "訂定該股票期貨結算保證金適用比例。經機動檢討結果，計有{0}達機動評估指標，觀察該{1}契約30天期風險價格" +
                                    "係數{2}為{3}，風險價格係數2年平均值{2}為{4}，建議調整保證金級距，{5}。",
                                    GenArrayTxt(kindNameInReserve), these, respectively, GenArrayTxt(monthRiskInReserve),
                                    GenArrayTxt(dayRiskInReserve), GenArrayTxt(levelInReserve));

            SetInnerText(tempStr, true, 4.11f, 1.25f);

            //說明四
            SetInnerText("四、本次保證金倘經調整，其金額變動如下：");
         }

         protected virtual string GetQuarter(int month) {

            int re = (month % 3) == 0 ? (month / 3) : (month / 3) + 1;

            return re.ToString();
         }

         /// <summary>
         /// 設定欄位值
         /// </summary>
         protected override void SetTableColValue(I40030AmtProdType iAmtProd, DataRow dr) {

            int k = 1;

            foreach (string rowName in iAmtProd.RowName) {

               TableRow tableRow = WordTable.Rows.Append();

               WordTableCell = tableRow.FirstCell;
               Doc.InsertSingleLineText(WordTableCell.Range.Start, rowName);

               //特殊處理, 選擇權時有AB值, k=1 跑保證金或A值, k>1 跑B值
               string[] colNameList = k == 1 ? iAmtProd.DbColName : iAmtProd.DbColNameB;
               string numberFormat = k == 1 ? iAmtProd.NumberFormat : iAmtProd.NumberFormatB;

               int i = 1;
               foreach (string col in colNameList) {
                  Doc.InsertSingleLineText(WordTable[tableRow.Index, i].Range.Start, !decimal.Equals(dr[col].AsDecimal(), 0) ?
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
               I40030AmtProdType iAmtProdType = CreateI40030AmtProdType(GetType(), "AmtProdType40030" + amtType + prodType, args);

               SetCurrencyName(iAmtProdType);

               CreateTable(Doc, 2, 7);

               SetTableColTitle(iAmtProdType.ProdName, iAmtProdType.TableTitle,
                              $"調整後保證金金額({dr["m_level_name"]})", $"調整前保證金金額({dr["cur_level_name"]})");

               SetTableColValue(iAmtProdType, dr);
            }

         }
      }

      private class ExportWord0B : ExportWord {
         protected virtual DataTable DtAbroad { get; set; }
         protected virtual DataTable DtSpan { get; set; }
         protected virtual string[] ChineseNumber { get; set; }

         public ExportWord0B(string txtdate, string adjtype, string programId, List<CheckedItem> checkeditems) :
                     base(txtdate, adjtype, programId, checkeditems) {

            ChineseNumber = new string[] { "0", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };
         }

         public override ReturnMessageClass Export() {
            ReturnMessageClass msg = new ReturnMessageClass();
            msg.Status = ResultStatus.Fail;

            try {
               FilePath = PbFunc.wf_copy_file(ProgramId, MeetingLogFileName);

               //取得會議紀錄 / 議程資訊
               GetRPTF();
               string chairman = DtMinutes.Rows[0]["RPTF_TEXT"].AsString();

               //會議記錄
               OpenFile();

               //表頭 出席者
               SetHead();

               //案由一(指數, 公債, 黃金)
               foreach (CheckedItem c in CheckedItems) {
                  DataTable dtTemp = Dt.Select("prod_subtype in ('I', 'B', 'C') and data_ymd = " +
                                       "'" + c.CheckedDate.ToString("yyyyMMdd") + "'").CopyToDataTable();

                  if (dtTemp.Rows.Count > 0) {
                     //案由後文字
                     SetFirstCase(dtTemp, c.CheckedValue);

                     //案由下說明文
                     SetFirstCaseDesc(dtTemp, c.CheckedValue, c.CheckedDate);
                  }
               }


               base.SetAllNumberAndEnglishFont(Doc);//設定英數字體

               Doc.EndUpdate();
               DocSer.SaveDocument(FilePath, DocumentFormat.Rtf);
               DocSer.Dispose();
#if DEBUG
               System.Diagnostics.Process.Start(FilePath);
#endif
               //end 會議紀錄

               msg.Status = ResultStatus.Success;
               return msg;
            } catch (Exception ex) {
               base.ErrorHandle(ex, msg);
               return msg;
            }
         }

         public override ReturnMessageClass GetData() {
            ReturnMessageClass msg = new ReturnMessageClass(MessageDisplay.MSG_NO_DATA);
            msg.Status = ResultStatus.Fail;

            foreach (CheckedItem c in CheckedItems) {
               DateTime searchDate = default(DateTime);
               DtAbroad = new DataTable();
               DtSpan = new DataTable();
               Dt = new DataTable();

               //全選時用 % 查詢
               OswGrp = CheckedItems.Count == 3 ? "%" : c.CheckedValue.AsString();
               searchDate = CheckedItems.Count == 3 ? CheckedItems.FirstOrDefault().CheckedDate : c.CheckedDate;

               //Abroad
               DtAbroad.Merge(Dao40030.GetAborad(searchDate, OswGrp));

               //主要資料
               Dt.Merge(Dao40030.GetData(searchDate.ToString("yyyyMMdd"), OswGrp, AsAdjType, AdjType.SubStr(1, 1)));

               //Span
               OswGrp = OswGrp == "%" ? "%" : $"{c.CheckedValue}%";

               if (OswGrp == "%" || c.ETCSelected == c.CheckedValue.AsString())
                  DtSpan.Merge(Dao40030.GetSpan(searchDate, OswGrp, "", "ETC"));
               else
                  DtSpan.Merge(Dao40030.GetSpan(searchDate, OswGrp, "ETC", ""));
            }


            if (Dt != null) {
               if (Dt.Rows.Count > 0) {
                  msg.Status = ResultStatus.Success;
               }
            }

            return msg;
         }

         protected override void SetHead() {
            string chairman = DtMinutes.Rows[0]["RPTF_TEXT"].AsString();

            SetRtfDescText(GenMeetingDate(), chairman, GenAttend(DtMinutes));
         }

         protected virtual void SetFirstCase(DataTable dtTemp, int checkedNu) {
            string tmpStr = "";

            SetSubjectText($"案 由 {ChineseNumber[checkedNu]}：");

            List<string> kindNameList = new List<string>();

            foreach (DataRow dr in dtTemp.Rows) {
               string kindName = dr["kind_abbr_name"].AsString();

               if (dr["prod_subtype"].AsString() == "S")
                  kindName = $"{kindName}({dr["kind_id"].AsString()})";

               if (kindNameList.Exists(k => k == kindName))
                  kindNameList.Add(kindName);
            }

            tmpStr = string.Format("檢陳本公司{0}保證金調整案，謹提請討論。", GenArrayTxt(kindNameList));
            SetInnerText(tmpStr, false, 2.75f, 2.75f);
         }

         protected virtual void SetFirstCaseDesc(DataTable dtTemp, int checkedNu, DateTime checkedDate) {
            string tmpStr = "";
            int licnt = 4;

            SetSubjectText($"說　　明：");

            //說明一
            tmpStr = string.Format("一、{0}本公司上開契約結算保證金之變動幅度已達得調整標準之百分比，且進位後金額有變動時，" +
                                    "依本公司保證金調整作業規範，由督導結算業務主管召集業務相關部門主管會商決定是否調整。",
                                    dtTemp.Rows[0]["data_ymd"].AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}/{1}/{2}", 3));
            SetInnerText(tmpStr);

            //說明二
            SetInnerText("二、本次保證金倘經調整，其金額變動如下：");
            DrowTable(dtTemp);

            //說明三
            SetInnerText("三、本公司上開契約保證金調整之考量因素，請詳保證金調整檢核表，如附件。");

            //說明四、公債類
            DataTable dtDebt = dtTemp.Select("prod_subtype = 'B' and data_ymd = '" + checkedDate.ToString("yyyyMMdd") + "'").CopyToDataTable();

            if (dtDebt.Rows.Count > 0) {
               SetSubjectText($"{ChineseNumber[licnt]}、");
               licnt++;

               List<string> kindNameList = new List<string>();
               List<string> adjRateList = new List<string>();
               foreach (DataRow dr in dtDebt.Rows) {

                  adjRateList.Add(dr["adj_rate"].AsPercent(2));

                  string kindName = dr["kind_abbr_name"].AsString();

                  if (dr["prod_subtype"].AsString() == "S")
                     kindName = $"{kindName}({dr["kind_id"].AsString()})";

                  if (!kindNameList.Exists(k => k == kindName))
                     kindNameList.Add(kindName);
               }

               tmpStr = string.Format("{0}之保證金變動幅度已達{1}：", GenArrayTxt(kindNameList), GenArrayTxt(adjRateList));
               SetInnerText(tmpStr);

               //特殊處理, 不知原因
               DataRow drGBF = dtTemp.AsEnumerable().Where(d => d.Field<string>("kind_id").AsString() == "GBF").FirstOrDefault();
               if (drGBF != null) {
                  string iqnty = drGBF["i_qnty"].AsInt() > 0 ? string.Format("(今日成交量為" + drGBF["i_qnty"].AsString() + "口") : "";
                  string ioi= drGBF["i_qnty"].AsInt() > 0 ? string.Format("(今日未沖銷部位為" + drGBF["ioi"].AsString() + "口") : "";
                  string warn = "▲▲▲";

                  if (iqnty == "" || ioi == "") warn = "";

                  tmpStr = string.Format("(一) {0}近期皆無成交量{1}，且未沖銷部位為0{2}；價格無變化，風險價格係數為{3}。" +
                                         "公債期貨契約現行結算保證金占契約價值之比例為{4}{5}",
                                         drGBF["kind_abbr_name"].AsString(), iqnty, ioi, drGBF["m_day_risk"].AsPercent(2),
                                         drGBF["cur_cm_rate"].AsPercent(2), warn);

                  SetInnerText(tmpStr, true, 4.11f, 1.25f);

                  tmpStr = string.Format("(二) 經權衡市場風險及該商品並無未沖銷部位{0}，建議暫不調整，持續觀察，注意未沖銷部位變化之狀況，" +
                                          "於必要時隨時召開會議討論是否調整保證金。", warn);

                  SetInnerText(tmpStr, true, 4.11f, 1.25f);
               }

            }

         }

      }

      /// <summary>
      /// 替換文字用class
      /// </summary>
      private class M40030Word {
         public string MeetingDate { get; set; }
         public string MeetingAddress { get; set; }
         public string Chairman { get; set; }
         public string Attend { get; set; }
         //public string CaseDesc { get; set; }

         public M40030Word(string meetingdate, string chairman, string attend, string meetingaddress = "研討室") {
            MeetingDate = meetingdate;
            MeetingAddress = meetingaddress;
            Chairman = chairman;
            Attend = attend;
            //CaseDesc = casedesc;
         }
      }

      private interface I40030AmtProdType {
         string CurrencyName { get; set; }
         string ProdName { get; set; }
         string BeforeAdjustTitle { get; set; }
         string AfterAdjustTitle { get; set; }
         string NumberFormat { get; set; }

         string NumberFormatB { get; set; }
         string[] TableTitle { get; set; }
         string[] RowName { get; set; }
         string[] DbColName { get; set; }
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
            ProdName = dr["kind_id_out"].AsString();
            DbColName = new string[] { "m_im", "m_mm", "m_cm", "cur_im", "cur_mm", "cur_cm" };

            AfterAdjustTitle = "調整後保證金金額";
            BeforeAdjustTitle = "調整前保證金金額";
            NumberFormat = "#,##0.##";
            TableTitle = new string[] { "原始保證金金額", "維持保證金金額", "結算保證金金額" };
            RowName = new[] { "保證金" };

            StockName = "，股票期貨標的證券代號";
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
            ProdName = dr["kind_id_out"].AsString();
            DbColName = new string[] { "m_im", "m_mm", "m_cm", "cur_im", "cur_mm", "cur_cm" };

            AfterAdjustTitle = "調整後保證金適用比例";
            BeforeAdjustTitle = "調整前保證金適用比例";
            NumberFormat = "#,##0.##";
            TableTitle = new string[] { "原始保證金", "維持保證金", "結算保證金" };
            RowName = new[] { "保證金" };

            StockName = "，股票期貨標的證券代號";
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
            ProdName = dr["kind_id_out"].AsString();
            DbColName = new string[] { "m_im", "m_mm", "m_cm", "cur_im", "cur_mm", "cur_cm" };
            DbColNameB = new string[] { "m_im_b", "m_mm_b", "m_cm_b", "cur_im_b", "cur_mm_b", "cur_cm_b" };

            AfterAdjustTitle = "調整後保證金金額";
            BeforeAdjustTitle = "調整前保證金金額";
            NumberFormat = "#,##0.##";
            NumberFormatB = "#,##0.###";
            TableTitle = new string[] { "計算賣出選擇權原始保證金之適用金額", "計算賣出選擇權維持保證金之適用金額", "計算賣出選擇權結算保證金之適用金額" };
            RowName = new string[] { "風險保證金（A值）", "風險保證金最低值（B值）" };

            StockName = "，股票選擇權標的證券代號";
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
            ProdName = dr["kind_id_out"].AsString();
            DbColName = new string[] { "m_im", "m_mm", "m_cm", "cur_im", "cur_mm", "cur_cm" };
            DbColNameB = new string[] { "m_im_b", "m_mm_b", "m_cm_b", "cur_im_b", "cur_mm_b", "cur_cm_b" };

            AfterAdjustTitle = "調整後保證金金額";
            BeforeAdjustTitle = "調整前保證金金額";
            NumberFormat = "#,##0.##";
            NumberFormatB = "#,##0.###";
            TableTitle = new string[] { "計算賣出選擇權原始保證金之適用比例", "計算賣出選擇權維持保證金之適用比例", "計算賣出選擇權結算保證金之適用比例" };
            RowName = new string[] { "風險保證金（A值）", "風險保證金最低值（B值）" };

            StockName = "，股票選擇權標的證券代號";
            AbbrName = dr["kind_abbr_name"].AsString();
            KindId = dr["kind_id"].AsString();
            StockId = dr["STOCK_ID"].AsString();
         }

      }


      private class CheckedItem {
         public int CheckedValue { get; set; }
         public DateTime CheckedDate { get; set; }
         public string ETCSelected { get; set; }
      }
   }
}