using BaseGround;
using BaseGround.Shared;
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
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

/// <summary>
/// Test Data 2B 20190102 / 1B 20190130 / 1E 20190212 / 0B 20190212
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
   public partial class W40110 : FormParent {

      #region Get UI Value
      /// <summary>
      /// yyyyMMdd
      /// </summary>
      protected string TxtDate {
         get {
            return txtDate.DateTimeValue.ToString("yyyyMMdd");
         }
      }

      /// <summary>
      /// 一般/股票, 長假調整, 長假回調, 處置股票調整
      /// 0B / 1B / 1E / 2B
      /// </summary>
      protected string AdjType {
         get {
            return ddlAdjType.EditValue.AsString();
         }
      }
      #endregion

      public W40110(string programID, string programName) : base(programID, programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtDate.DateTimeValue = GlobalInfo.OCF_DATE;

         //設定下拉選單
         ddlAdjType.SetDataTable(new COD().ListByCol2("400xx", "dw_adj_type"), "COD_ID", "COD_DESC", TextEditStyles.DisableTextEditor, null);
         ddlAdjType.EditValue = "0B";

#if DEBUG
         txtDate.DateTimeValue = ("20190212").AsDateTime("yyyyMMdd");
         ddlAdjType.EditValue = "0B";
#endif

         ExportShow.Hide();
      }

      protected override ResultStatus Export() {
         ExportShow.Text = "轉檔中...";
         ExportShow.Show();

         try {

            object[] args = { new D40xxx(), TxtDate, AdjType, _ProgramID };
            IExport40xxxData xmlData = CreateExportData(GetType(), "ExportWord" + AdjType, args);
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
         ExportShow.Text = MessageDisplay.MSG_IMPORT;
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();
         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      private enum SubTypeName {
         [Description("股價指數類契約")]
         I = 1,
         [Description("商品類契約")]
         C,
         [Description("利率類契約")]
         B,
         [Description("股票類契約")]
         S,
         [Description("匯率類契約")]
         E
      }

      public IExport40xxxData CreateExportData(Type type, string name, object[] args = null) {

         string AssemblyName = type.Namespace.Split('.')[0];//最後compile出來的dll名稱
         string className = type.FullName + "+" + name;//完整的class路徑(注意,內部的class執行時其fullName是用+號連結起來)
         return (IExport40xxxData)Assembly.Load(AssemblyName).CreateInstance(className, true, BindingFlags.CreateInstance, null, args, null, null);
      }

      private class ExportWord : IExport40xxxData {
         protected D40xxx Dao { get; }
         protected virtual string TxtDate { get; }
         protected virtual string AdjType { get; }
         protected virtual string FileChName { get; set; }
         protected string ProgramId { get; }

         protected virtual string AsAdjType {
            get {
               return AdjType.SubStr(0, 1) == "0" ? "" : AdjType.SubStr(0, 1);
            }
         }
         protected virtual string FilePath { get; set; }
         protected virtual string[] TableTitle { get; set; }
         protected virtual string[] ColName { get; set; }
         protected virtual string[] ColNameB { get; set; }
         protected virtual DataTable dt { get; set; }
         protected virtual Document doc { get; set; }
         protected virtual Table WordTable { get; set; }

         protected virtual TableCell WordTableCell { get; set; }
         protected virtual ParagraphProperties ParagraphProps { get; set; }

         public ExportWord(D40xxx dao, string txtdate, string adjtype, string programId) {
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

         /// <summary>
         /// 直接替換 rtf 上面文字
         /// </summary>
         /// <param name="docSev"></param>
         /// <param name="m40110"></param>
         protected virtual void ReplaceWrod(RichEditDocumentServer docSev, M40110Word m40110) {

            //Options.MailMerge 要用List 才會有作用
            List<M40110Word> listM40110 = new List<M40110Word>();
            listM40110.Add(m40110);

            //直接replace word 上面的字
            docSev.Options.MailMerge.DataSource = listM40110;
            docSev.Options.MailMerge.ViewMergedData = true;
         }

         /// <summary>
         /// set rtf table style
         /// </summary>
         /// <param name="doc"></param>
         /// <param name="paragraphProps"></param>
         /// <param name="table"></param>
         protected virtual void CreateTable(Document doc, int rowCount, int colCount) {

            WordTable = doc.Tables.Create(doc.Range.End, rowCount, colCount, AutoFitBehaviorType.AutoFitToWindow);
            ParagraphProps = doc.BeginUpdateParagraphs(WordTable.Range);

            // 預設Table內容都全部置中
            ParagraphProps.Alignment = ParagraphAlignment.Center;
            doc.EndUpdateParagraphs(ParagraphProps);

            // 預設Table內的字體
            CharacterProperties tableProp = doc.BeginUpdateCharacters(WordTable.Range);
            tableProp.FontSize = 12;
            doc.EndUpdateCharacters(tableProp);

            // 垂直置中
            WordTable.ForEachCell((c, i, j) => {
               c.VerticalAlignment = TableCellVerticalAlignment.Center;
            });

         }

         /// <summary>
         /// 將資料寫入表格
         /// </summary>
         /// <param name="doc"></param>
         /// <param name="table">要寫資料的表格</param>
         /// <param name="rowIndedx">表格的列數</param>
         /// <param name="dr"></param>
         /// <param name="format">display format</param>
         /// <param name="colList">DB data 順序</param>
         protected virtual void SetMMValue(Document doc, Table table, int rowIndedx, DataRow dr, string format, string[] colList) {
            int i = 1;
            foreach (string col in colList) {
               doc.InsertSingleLineText(table[rowIndedx, i].Range.Start, !decimal.Equals(dr[col].AsDecimal(), 0) ? dr[col].AsDecimal().ToString(format) : string.Empty);

               i++;
            }
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
            result += "之期貨契約保證金及選擇權契約";
            if (dt.Select("ab_type='A'").Count<DataRow>() > 0) {
               result += "風險保證金(A值)、風險保證金最低值(B值)";
            }

            return result;
         }

         protected virtual void SetTableTitle(string[] strList, int rowIndex, string strFormat, string sufStr) {

            int k = 0;
            foreach (string str in strList) {
               doc.InsertSingleLineText(WordTable[rowIndex, k + 1].Range.Start, string.Format(strFormat, TableTitle[k], sufStr));
               doc.InsertSingleLineText(WordTable[rowIndex, k + 4].Range.Start, string.Format(strFormat, TableTitle[k], sufStr));
               k++;
            }

         }

         protected virtual void SetTableStr(int rowIndex, int colIndex, string str) {
            WordTableCell = WordTable[rowIndex, colIndex];
            doc.InsertSingleLineText(WordTableCell.Range.Start, str);
         }

         /// <summary>
         /// 產生簽核的表格
         /// </summary>
         protected virtual void CreateSignTable(Document doc) {

            int j = 0;
            while (j < 8) {
               doc.AppendText(Environment.NewLine);
               j++;
            }

            Table tableSign = doc.Tables.Create(doc.Range.End, 2, 7, AutoFitBehaviorType.AutoFitToContents);
            string[] tableTitle = { "承　　辦", "組　　長" + Characters.LineBreak + "副 組 長", "副 經 理", "經　　理", "督導主管", "總 經 理", "董 事 長" };

            tableSign.TableAlignment = TableRowAlignment.Center;

            ParagraphProperties paragraphSignProps = doc.BeginUpdateParagraphs(tableSign.Range);
            paragraphSignProps.Alignment = ParagraphAlignment.Center;
            doc.EndUpdateParagraphs(paragraphSignProps);

            TableRow rowSign = tableSign.Rows.Last;
            rowSign.Height = DevExpress.Office.Utils.Units.InchesToDocumentsF(1f);
            rowSign.HeightType = HeightType.AtLeast;

            int k = 0;
            foreach (string s in tableTitle) {
               doc.InsertSingleLineText(tableSign[0, k].Range.Start, s);
               k++;
            }

            // 每個Cell垂直置中
            tableSign.ForEachCell((c, i, n) => {
               c.VerticalAlignment = TableCellVerticalAlignment.Center;
            });
         }

         protected I40110AmtType CreateI40110AmtType(Type type, string name, object[] args = null) {
            string AssemblyName = type.Namespace.Split('.')[0];//最後compile出來的dll名稱
            string className = type.Namespace + "." + type.ReflectedType.Name + "+" + name;//完整的class路徑(注意,內部的class執行時其fullName是用+號連結起來)
            return (I40110AmtType)Assembly.Load(AssemblyName).CreateInstance(className, true, BindingFlags.CreateInstance, null, args, null, null);
         }

         protected I40110SubType CreateI40110SubType(Type type, string name) {
            string AssemblyName = type.Namespace.Split('.')[0];//最後compile出來的dll名稱
            string className = type.Namespace + "." + type.ReflectedType.Name + "+" + name;//完整的class路徑(注意,內部的class執行時其fullName是用+號連結起來)
            return (I40110SubType)Assembly.Load(AssemblyName).CreateInstance(className);
         }

         /// <summary>
         /// 錯誤寫log
         /// </summary>
         /// <param name="msg"></param>
         /// <param name="logType"></param>
         /// <param name="operationType"></param>
         protected virtual void WriteLog(string msg, string logType = "Info", string operationType = "") {
            bool isNeedWriteFile = false;
            string dbErrorMsg = "";

            //1.write log to db
            //ken,先把WriteLog集中,之後可根據不同的logType,存放不同的TABLE或檔案
            //基本logType可定義為 info/operation/error
            //logf_job_type value: I = change data, E = export, R = query, P = print, X = execute
            try {
               switch (logType) {
                  case ("Info"):
                     operationType = "A";
                     break;
                  case ("Error"):
                     operationType = "Z";
                     isNeedWriteFile = true;
                     break;
               }
               //ken,LOGF_KEY_DATA長度要取前100字元,但是logf.LOGF_KEY_DATA型態為VARCHAR2 (100 Byte),如果有中文會算2byte...先取前80吧
               new LOGF().Insert(GlobalInfo.USER_ID, ProgramId, msg.SubStr(0, 80), operationType);

            } catch (Exception ex2) {
               // write log to db failed , ready write file to local
               isNeedWriteFile = true;
               dbErrorMsg = ex2.ToString();
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
                     if (dbErrorMsg != "")
                        sw.Write("dbErrorMsg=" + dbErrorMsg);
                  }//using (StreamWriter sw = File.AppendText(filepath)) {
               } catch (Exception fileEx) {
                  MessageDisplay.Error("將log寫入檔案發生錯誤,請聯絡管理員" + Environment.NewLine + "msg=" + fileEx.Message);
                  return;
               }
            }//if (isNeedWriteFile) 
         }

      }


      /// <summary>
      /// 一般 / 股票 輸出 rtf
      /// </summary>
      private class ExportWord0B : ExportWord {
         public ExportWord0B(D40xxx dao, string txtdate, string adjtype, string programId) : base(dao, txtdate, adjtype, programId) {
            FileChName = "40110_新聞稿_一般_股票";
            FilePath = PbFunc.wf_copy_file(ProgramId, FileChName);
            TableTitle = new string[] { "原始", "維持", "結算" };
            ColName = new string[] { "m_im", "m_mm", "m_cm", "cur_im", "cur_mm", "cur_cm" };
            ColNameB = new string[] { "m_im_b", "m_mm_b", "m_cm_b", "cur_im_b", "cur_mm_b", "cur_cm_b" };
         }

         public override ReturnMessageClass GetData() {
            ReturnMessageClass msg = new ReturnMessageClass(MessageDisplay.MSG_NO_DATA);

            dt = Dao.GetData(TxtDate, AsAdjType, AdjType.SubStr(1, 1));

            //一般 / 股票 要多撈一次資料
            if (AdjType == "0B") {
               DataTable dtTmp = Dao.GetData(TxtDate, "3", AdjType.SubStr(1, 1));
               if (dtTmp != null) {
                  if (dtTmp.Rows.Count > 0) {
                     foreach (DataRow r in dtTmp.Rows) {
                        DataRow addRow = r;
                        dt.ImportRow(r);
                     }
                  }
               }
            }

            if (dt == null) {
               msg.Status = ResultStatus.Fail;
               return msg;
            }

            if (dt.Rows.Count <= 0) {
               msg.Status = ResultStatus.Fail;
               return msg;
            }

            dt = dt.Sort("SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC, KIND_ID ASC, AB_TYPE ASC");
            msg.Status = ResultStatus.Success;
            return msg;
         }

         public override ReturnMessageClass Export() {
            ReturnMessageClass msg = new ReturnMessageClass();
            msg.Status = ResultStatus.Fail;

            try {
               using (RichEditDocumentServer docSev = new RichEditDocumentServer()) {
                  docSev.LoadDocument(FilePath);

                  string validDate = dt.Rows[0]["ISSUE_BEGIN_YMD"].AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);
                  string ValidDatePrev = new MOCF().GetValidDatePrev(dt.Rows[0]["ISSUE_BEGIN_YMD"].AsString()).
                                          AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);

                  string prodName = "";

                  prodName = GenProdName(dt, "契約");

                  //取代 word 開頭文字
                  ReplaceWrod(docSev, new M40110Word(ValidDatePrev, validDate, prodName));

                  doc = docSev.Document;
                  doc.BeginUpdate();

                  foreach (DataRow dr in dt.Rows) {
                     string ampType = dr["amt_type"].AsString();
                     object[] args = { dr };
                     I40110AmtType iAmtType = CreateI40110AmtType(GetType(), "AmtType" + ProgramId + ampType, args);

                     CreateTable(doc, 4, 7);

                     #region //3.1單位幣別或比例標題

                     SetTableStr(0, 6, iAmtType.CurrencyName);

                     ParagraphProps = doc.BeginUpdateParagraphs(WordTableCell.Range);
                     ParagraphProps.Alignment = ParagraphAlignment.Right;
                     doc.EndUpdateParagraphs(ParagraphProps);
                     WordTable.MergeCells(WordTable[0, 0], WordTableCell);

                     WordTableCell = WordTable[0, 0];
                     WordTableCell.CellSetBorders(TableBorderLineStyle.None, TableBorderLineStyle.None, TableBorderLineStyle.None);
                     #endregion

                     #region //3.2商品名稱

                     SetTableStr(1, 0, iAmtType.ProdName);
                     WordTableCell.VerticalAlignment = TableCellVerticalAlignment.Center;
                     WordTable.MergeCells(WordTableCell, WordTable[2, 0]);

                     #endregion

                     #region //3.3調整前後保證金金額的標題
                     //調整後
                     SetTableStr(1, 1, iAmtType.AfterAdjustTitle);
                     WordTable.MergeCells(WordTableCell, WordTable[1, 3]);

                     //調整前
                     SetTableStr(1, 2, iAmtType.BeforeAdjustTitle);
                     WordTable.MergeCells(WordTableCell, WordTable[1, 4]);
                     #endregion

                     #region //3.4保證金相關欄位

                     // 如果AB_TYPE為A代表有AB值的話長不一樣
                     if (dr["ab_type"].AsString() == "A") {

                        SetTableTitle(TableTitle, 2, "計算賣出選擇權{0}保證金之適用{1}", iAmtType.MoneyOrPercent);

                        WordTableCell = WordTable[3, 0];
                        doc.InsertSingleLineText(WordTableCell.Range.Start, "風險保證金(A值)");
                        WordTableCell.PreferredWidthType = WidthType.Fixed;
                        WordTableCell.PreferredWidth = DevExpress.Office.Utils.Units.InchesToDocumentsF(1f);

                        WordTable.Rows.Append();

                        WordTableCell = WordTable[4, 0];
                        doc.InsertSingleLineText(WordTableCell.Range.Start, "風險保證金最低值(B值)");
                        WordTableCell.PreferredWidthType = WidthType.Fixed;

                        //填寫B資料
                        SetMMValue(doc, WordTable, 4, dr, iAmtType.NumberFormatB, ColNameB);
                     } else {

                        SetTableTitle(TableTitle, 2, "{0}{1}保證金", Characters.LineBreak.ToString());
                        WordTableCell = WordTable[3, 0];
                        doc.InsertSingleLineText(WordTableCell.Range.Start, "保證金");
                        WordTableCell.PreferredWidthType = WidthType.Fixed;
                        WordTableCell.PreferredWidth = DevExpress.Office.Utils.Units.InchesToDocumentsF(0.7f);
                     }

                     //填寫資料
                     SetMMValue(doc, WordTable, 3, dr, iAmtType.NumberFormat, ColName);

                     #endregion

                     doc.AppendText(Environment.NewLine);
                  }// foreach

                  CreateSignTable(doc);
                  doc.EndUpdate();
                  docSev.SaveDocument(FilePath, DocumentFormat.Rtf);
               }//using 

               msg.Status = ResultStatus.Success;
               System.Diagnostics.Process.Start(FilePath);
               return msg;

            } catch (Exception ex) {
               WriteLog(ex.Message);
               msg.ReturnMessage = MessageDisplay.MSG_IMPORT_FAIL;
               return msg;
            }
         }
      }

      /// <summary>
      /// 長假回調 輸出 rtf
      /// </summary>
      private class ExportWord1E : ExportWord {
         public ExportWord1E(D40xxx dao, string txtdate, string adjtype, string programId) : base(dao, txtdate, adjtype, programId) {
            FileChName = "40110_新聞稿_春節回調";
            FilePath = PbFunc.wf_copy_file(ProgramId, FileChName);
            TableTitle = new string[] { "結算", "維持", "原始" };
            ColName = new string[] { "cur_im2", "cur_im1", "cur_im", "m_im2", "m_im1", "m_im" };
            ColNameB = new string[] { "cur_cm_b", "cur_mm_b", "cur_im_b", "m_cm_b", "m_mm_b", "m_im_b" };
         }

         public override ReturnMessageClass Export() {
            ReturnMessageClass msg = new ReturnMessageClass();
            msg.Status = ResultStatus.Fail;
            try {
               using (RichEditDocumentServer docSev = new RichEditDocumentServer()) {
                  docSev.LoadDocument(FilePath);

                  string validDate = dt.Rows[0]["ISSUE_BEGIN_YMD"].AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);
                  string ValidDatePrev = new MOCF().GetValidDatePrev(dt.Rows[0]["ISSUE_BEGIN_YMD"].AsString()).
                                          AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);

                  IEnumerable<IGrouping<string, DataRow>> listGroupBySubtype = dt.AsEnumerable().GroupBy(g => g.Field<string>("PROD_SUBTYPE"));

                  //取代 word 開頭文字
                  ReplaceWrod(docSev, new M40110Word(ValidDatePrev, validDate, GenProdName(dt), GenSubName(listGroupBySubtype)));

                  //內文表格
                  doc = docSev.Document;
                  doc.BeginUpdate();

                  foreach (var groupSubtype in listGroupBySubtype) {

                     #region Table上方的標題

                     I40110SubType isubType = CreateI40110SubType(GetType(), "SubType" + ProgramId + groupSubtype.Key);

                     doc.AppendText(isubType.TableTitle);
                     ParagraphProps = doc.BeginUpdateParagraphs(doc.Paragraphs.Last().Range);
                     ParagraphProps.Alignment = ParagraphAlignment.Center;
                     doc.EndUpdateParagraphs(ParagraphProps);

                     CharacterProperties characterProps = doc.BeginUpdateCharacters(doc.Paragraphs.Last().Range);
                     characterProps.FontSize = 14;
                     doc.EndUpdateCharacters(characterProps);

                     doc.AppendText("\n");

                     doc.AppendText(isubType.UnitDescription);
                     ParagraphProps = doc.BeginUpdateParagraphs(doc.Paragraphs.Last().Range);
                     ParagraphProps.Alignment = ParagraphAlignment.Left;
                     doc.EndUpdateParagraphs(ParagraphProps);

                     characterProps = doc.BeginUpdateCharacters(doc.Paragraphs.Last().Range);
                     characterProps.FontSize = 12;
                     doc.EndUpdateCharacters(characterProps);

                     doc.AppendText("\n");

                     #endregion

                     #region Table        

                     CreateTable(doc, 2, 7);

                     #region Table的標題

                     SetTableStr(0, 0, "契約名稱");
                     WordTableCell.PreferredWidthType = WidthType.Fixed;
                     WordTableCell.PreferredWidth = DevExpress.Office.Utils.Units.InchesToDocumentsF(2.3f);
                     WordTableCell.VerticalAlignment = TableCellVerticalAlignment.Center;
                     WordTable.MergeCells(WordTableCell, WordTable[1, 0]);

                     SetTableStr(0, 1, "調整前保證金金額");
                     WordTable.MergeCells(WordTableCell, WordTable[0, 3]);

                     SetTableStr(0, 2, "調整後保證金金額");
                     WordTable.MergeCells(WordTableCell, WordTable[0, 4]);

                     SetTableTitle(TableTitle, 1, "{0}{1}保證金", Environment.NewLine);
                     #endregion

                     #region 在每一種商品類別裡面填入資料

                     foreach (DataRow dr in dt.Select(string.Format("prod_subtype ='{0}'", groupSubtype.Key))) {
                        TableRow tableRow = WordTable.Rows.Append();

                        string kindAbbrName = dr["kind_abbr_name"].AsString();

                        WordTableCell = tableRow.FirstCell;

                        if (dr["ab_type"].AsString() == "A") {
                           doc.InsertSingleLineText(WordTableCell.Range.Start, kindAbbrName + "風險保證金(A值)");
                        } else {
                           doc.InsertSingleLineText(WordTableCell.Range.Start, kindAbbrName);
                        }

                        ParagraphProps = doc.BeginUpdateParagraphs(WordTableCell.Range);
                        ParagraphProps.Alignment = ParagraphAlignment.Left;
                        doc.EndUpdateParagraphs(ParagraphProps);

                        SetMMValue(doc, WordTable, tableRow.Index, dr, "#,##0", ColName);

                        // 如果ab_type為A，代表有AB值，這裡加上B值的列
                        if (dr["ab_type"].AsString() == "A") {

                           tableRow = WordTable.Rows.Append();
                           WordTableCell = tableRow.FirstCell;

                           if (dr["ab_type"].AsString() == "A") {
                              doc.InsertSingleLineText(WordTableCell.Range.Start, kindAbbrName + "風險保證金最低值(B值)");
                           } else {
                              doc.InsertSingleLineText(WordTableCell.Range.Start, kindAbbrName);
                           }

                           SetMMValue(doc, WordTable, tableRow.Index, dr, "#,##0", ColNameB);
                        }

                     }// foreach

                     #endregion

                     #endregion

                     #region 如果是匯率類的話，表格下方加一段註解

                     if (groupSubtype.Key == "E") {
                        doc.AppendText("※本公司上揭契約公告之保證金收取金額，小型美元兌人民幣期貨、美元兌人民幣期貨、" +
                           "小型美元兌人民幣選擇權及美元兌人民幣選擇權為人民幣計價；澳幣兌美元期貨、英鎊兌美元期貨、歐元兌美元期貨為美元計價；美元兌日圓期貨為日圓計價。");

                        ParagraphProps = doc.BeginUpdateParagraphs(doc.Paragraphs.Last().Range);
                        ParagraphProps.LineSpacingType = ParagraphLineSpacing.Single;
                        
                        doc.EndUpdateParagraphs(ParagraphProps);

                        doc.AppendText(Environment.NewLine);
                     }

                     #endregion

                  }// foreach

                  CreateSignTable(doc);
                  doc.EndUpdate();
                  docSev.SaveDocument(FilePath, DocumentFormat.Rtf);
               }//using 

               msg.Status = ResultStatus.Success;
#if DEBUG
               System.Diagnostics.Process.Start(FilePath);
#endif
               return msg;

            } catch (Exception ex) {
               WriteLog(ex.Message);
               msg.ReturnMessage = MessageDisplay.MSG_IMPORT_FAIL;
               return msg;
            }
         }

         protected virtual string GenSubName(IEnumerable<IGrouping<string, DataRow>> listGroupBySubtype) {
            string subtypeName = "";
            int k = 0;
            foreach (var item in listGroupBySubtype) {
               //取得 Enum Description
               subtypeName += ((SubTypeName)Enum.Parse(typeof(SubTypeName), item.Key)).GetDescriptionText();

               if (k != (listGroupBySubtype.Count() - 2)) {
                  subtypeName += "、";
               } else {
                  subtypeName += "及";
               }

               k++;
            }
            subtypeName = subtypeName.TrimEnd('、');

            return subtypeName;
         }

         /// <summary>
         /// set rtf table style
         /// </summary>
         /// <param name="doc"></param>
         /// <param name="paragraphProps"></param>
         /// <param name="table"></param>
         protected override void CreateTable(Document doc, int rowCount, int colCount) {

            WordTable = doc.Tables.Create(doc.Range.End, rowCount, colCount);
            ParagraphProps = doc.BeginUpdateParagraphs(WordTable.Range);

            // 預設Table內容都全部置中
            ParagraphProps = doc.BeginUpdateParagraphs(WordTable.Range);
            ParagraphProps.Alignment = ParagraphAlignment.Center;
            ParagraphProps.LineSpacingType = ParagraphLineSpacing.Single;
            ParagraphProps.SpacingBefore = 0;
            doc.EndUpdateParagraphs(ParagraphProps);

            // 預設Table內的字體
            CharacterProperties tableProp = doc.BeginUpdateCharacters(WordTable.Range);
            tableProp.FontSize = 12;
            doc.EndUpdateCharacters(tableProp);

            // 垂直置中
            WordTable.ForEachCell((c, i, j) => {
               c.VerticalAlignment = TableCellVerticalAlignment.Center;
            });

         }
      }

      private class M40110Word {
         public string ValidDatePrev { get; set; }
         public string ValidDate { get; set; }
         public string ProductList { get; set; }
         public string SubtypeList { get; set; }

         public M40110Word(string validdateprev, string validdate, string productlist) {
            ValidDatePrev = validdateprev;
            ValidDate = validdate;
            ProductList = productlist;
         }

         public M40110Word(string validdateprev, string validdate, string productlist, string subtypelist) {
            ValidDatePrev = validdateprev;
            ValidDate = validdate;
            ProductList = productlist;
            SubtypeList = subtypelist;
         }

      }

      private interface I40110SubType {
         string SubTypeId { get; set; }
         string TableTitle { get; set; }
         string UnitDescription { get; set; }
      }

      private class SubType40110I : I40110SubType {
         public string SubTypeId { get; set; }
         public string TableTitle { get; set; }
         public string UnitDescription { get; set; }

         public SubType40110I() {
            SubTypeId = "I";
            TableTitle = "股價指數類期貨及選擇權契約保證金調整前後金額";
            UnitDescription = "單位：新臺幣元";
         }

      }

      private class SubType40110C : I40110SubType {
         public string SubTypeId { get; set; }
         public string TableTitle { get; set; }
         public string UnitDescription { get; set; }

         public SubType40110C() {
            SubTypeId = "C";
            TableTitle = "商品類期貨及選擇權契約保證金調整前後金額";
            UnitDescription = "單位：黃金期貨為美元計價，其餘商品為新臺幣元";
         }

      }

      private class SubType40110E : I40110SubType {
         public string SubTypeId { get; set; }
         public string TableTitle { get; set; }
         public string UnitDescription { get; set; }

         public SubType40110E() {
            SubTypeId = "E";
            TableTitle = "匯率類期貨及選擇權契約保證金調整前後金額";
            UnitDescription = "單位：人民幣、美元及日圓";
         }

      }

      private interface I40110AmtType {
         string CurrencyName { get; set; }
         string ProdName { get; set; }
         string BeforeAdjustTitle { get; set; }
         string AfterAdjustTitle { get; set; }
         string NumberFormat { get; set; }
         string NumberFormatB { get; set; }
         string MoneyOrPercent { get; set; }
      }

      private class AmtType40110P : I40110AmtType {
         public string CurrencyName { get; set; }
         public string ProdName { get; set; }
         public string BeforeAdjustTitle { get; set; }
         public string AfterAdjustTitle { get; set; }
         public string NumberFormat { get; set; }
         public string NumberFormatB { get; set; }
         public string MoneyOrPercent { get; set; }

         public AmtType40110P(DataRow dr) {
            CurrencyName = "單位：比例(%)";
            MoneyOrPercent = "比例";
            NumberFormat = "#0.00%";
            NumberFormatB = "#0.000%";//特殊處理, 當P有B值時, 要顯示到小數點第三位

            ProdName = dr["kind_id_out"].AsString() + Characters.LineBreak + "(" + dr["kind_abbr_name"].AsString() + ")";

            BeforeAdjustTitle = dr["cur_level"].AsString() == "Z" ? "調整前保證金適用比例" :
                                 "調整前保證金適用比例" + Characters.LineBreak + "(" + dr["cur_level_name"].AsString() + ")";

            AfterAdjustTitle = dr["m_level"].AsString() == "Z" ? "調整後保證金適用比例" :
                                 "調整後保證金適用比例" + Characters.LineBreak + "(" + dr["m_level_name"].AsString() + ")";

         }

      }

      private class AmtType40110F : I40110AmtType {
         public string CurrencyName { get; set; }
         public string ProdName { get; set; }
         public string BeforeAdjustTitle { get; set; }
         public string AfterAdjustTitle { get; set; }
         public string NumberFormat { get; set; }
         public string NumberFormatB { get; set; }
         public string MoneyOrPercent { get; set; }

         public AmtType40110F(DataRow dr) {
            CurrencyName = "單位：" + dr["currency_name"].AsString();
            MoneyOrPercent = "金額";
            ProdName = dr["kind_id_out"].AsString();
            BeforeAdjustTitle = "調整後保證金金額";
            AfterAdjustTitle = "調整前保證金金額";
            NumberFormat = "#,##0.####";

         }

      }
   }
}