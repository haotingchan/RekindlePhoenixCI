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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

/// <summary>
/// 依照不同類別產生 rtf 
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

         //設定 下拉選單
         List<LookupItem> lstType = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "0B", DisplayMember = "一般 / 股票"},
                                        new LookupItem() { ValueMember = "1B", DisplayMember = "長假調整" },
                                        new LookupItem() { ValueMember = "1E", DisplayMember = "長假回調" },
                                        new LookupItem() { ValueMember = "2B", DisplayMember = "處置股票調整"}};

         //設定下拉選單
         ddlAdjType.SetDataTable(lstType, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, null);
         ddlAdjType.EditValue = "0B";

#if DEBUG
         txtDate.DateTimeValue = ("20190130").AsDateTime("yyyyMMdd");
         ddlAdjType.EditValue = "1B";
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
         ExportShow.Text = MessageDisplay.MSG_IMPORT;
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();
         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      /// <summary>
      /// SubType 中文描述
      /// </summary>
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

      /// <summary>
      /// 產出rtf 其他情況複寫其function
      /// </summary>
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
         protected virtual CharacterProperties CharacterProps { get; set; }

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
         protected virtual void SetRtfDescText(RichEditDocumentServer docSev, string prodList) {
            string validDate = dt.Rows[0]["ISSUE_BEGIN_YMD"].AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);
            string ValidDatePrev = new MOCF().GetValidDatePrev(dt.Rows[0]["ISSUE_BEGIN_YMD"].AsString()).
                                    AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);

            M40110Word m40110 = new M40110Word(ValidDatePrev, validDate, prodList);
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
         /// 設定表格表頭
         /// </summary>
         /// <param name="strList"></param>
         /// <param name="rowIndex">列數</param>
         /// <param name="strFormat">文字格式</param>
         /// <param name="sufStr"></param>
         protected virtual void SetTableTitle(string[] strList, int rowIndex, string strFormat, string sufStr) {

            int k = 0;
            foreach (string str in strList) {
               doc.InsertSingleLineText(WordTable[rowIndex, k + 1].Range.Start, string.Format(strFormat, TableTitle[k], sufStr));
               doc.InsertSingleLineText(WordTable[rowIndex, k + 4].Range.Start, string.Format(strFormat, TableTitle[k], sufStr));
               k++;
            }

         }

         /// <summary>
         /// 寫表格中文字
         /// </summary>
         /// <param name="rowIndex">第幾列</param>
         /// <param name="colIndex">第幾欄</param>
         /// <param name="str">文字</param>
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
         /// 開檔
         /// </summary>
         /// <returns></returns>
         protected virtual RichEditDocumentServer OpenRtfFile() {
            RichEditDocumentServer docSev = new RichEditDocumentServer();
            docSev.LoadDocument(FilePath);

            return docSev;
         }

         /// <summary>
         /// 結束編輯 關檔
         /// </summary>
         /// <param name="docSev"></param>
         /// <param name="msg"></param>
         protected virtual void EndUpdate(RichEditDocumentServer docSev, ReturnMessageClass msg) {
            CreateSignTable(doc);
            doc.EndUpdate();
            docSev.SaveDocument(FilePath, DocumentFormat.Rtf);
            docSev.Dispose();
            msg.Status = ResultStatus.Success;
#if DEBUG
            System.Diagnostics.Process.Start(FilePath);
#endif
         }

         /// <summary>
         /// 錯誤處理
         /// </summary>
         /// <param name="ex">Exception</param>
         /// <param name="msg"></param>
         public virtual void ErrorHandle(Exception ex, ReturnMessageClass msg) {
            WriteLog(ex.ToString(), "Info", "Z");
            msg.Status = ResultStatus.Fail;
            msg.ReturnMessage = MessageDisplay.MSG_IMPORT_FAIL;
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
         /// 設定英文和數字的字型
         /// </summary>
         protected virtual void SetNumberAndEnglishFontName(Document doc, DocumentRange docRange) {
            CharacterProperties cp = doc.BeginUpdateCharacters(docRange);
            cp.FontName = "Times New Roman";
            doc.EndUpdateCharacters(cp);
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
      /// 長假調整與回調
      /// </summary>
      private class ExportWordVacationAdjust : ExportWord {
         protected virtual string AppendText { get; set; }

         public ExportWordVacationAdjust(D40xxx dao, string txtdate, string adjtype, string programId) : base(dao, txtdate, adjtype, programId) {
            TableTitle = new string[] { $"結算{Characters.LineBreak}", $"維持{Characters.LineBreak}", $"原始{Characters.LineBreak}" };
            ColName = new string[] { "cur_im2", "cur_im1", "cur_im", "m_im2", "m_im1", "m_im" };
            ColNameB = new string[] { "cur_cm_b", "cur_mm_b", "cur_im_b", "m_cm_b", "m_mm_b", "m_im_b" };
            AppendText = "※本公司上揭契約公告之保證金收取金額，小型美元兌人民幣期貨、美元兌人民幣期貨、" +
                        "小型美元兌人民幣選擇權及美元兌人民幣選擇權為人民幣計價；澳幣兌美元期貨、英鎊兌美元期貨、歐元兌美元期貨為美元計價；美元兌日圓期貨為日圓計價。";
         }

         public override ReturnMessageClass Export() {
            ReturnMessageClass msg = new ReturnMessageClass();
            msg.Status = ResultStatus.Fail;
            try {
               FilePath = PbFunc.wf_copy_file(ProgramId, FileChName);

               //1.0 oprne file
               RichEditDocumentServer docSev = OpenRtfFile();

               IEnumerable<IGrouping<string, DataRow>> listGroupBySubtype = dt.AsEnumerable().GroupBy(g => g.Field<string>("PROD_SUBTYPE"));

               //1.1 取代rtf 開頭說明文
               SetRtfDescText(docSev, listGroupBySubtype);

               //1.2 內文
               doc = docSev.Document;
               doc.BeginUpdate();

               foreach (var groupSubtype in listGroupBySubtype) {

                  //1.2.1 Table上方的標題
                  SetAboveTableText(groupSubtype, 14, 12);

                  CreateTable(doc, 2, 7);

                  //1.2.2 Table的標題
                  SetTableColTitle();

                  //1.2.3 在每一種商品類別裡面填入資料
                  SetTableData(groupSubtype);

                  //1.2.4 如果是匯率類的話，表格下方加一段註解
                  if (groupSubtype.Key == "E") SetAppendText();

                  doc.AppendText(Environment.NewLine);
               }

               //設定文數字的自型
               SetAllNumberAndEnglishFont(doc);
               //1.2.5 寫完存檔
               EndUpdate(docSev, msg);
               return msg;

            } catch (Exception ex) {
               ErrorHandle(ex, msg);
               return msg;
            }
         }

         /// <summary>
         /// Table上方的標題
         /// </summary>
         /// <param name="groupSubtype">分群資料</param>
         /// <param name="titleSize">標題字體大小</param>
         /// <param name="unitDescSize">單位字體大小</param>
         protected virtual void SetAboveTableText(IGrouping<string, DataRow> groupSubtype, int titleSize, int unitDescSize) {
            I40110SubType isubType = CreateI40110SubType(GetType(), "SubType" + ProgramId + groupSubtype.Key);

            doc.AppendText(isubType.TableTitle);
            ParagraphProps = doc.BeginUpdateParagraphs(doc.Paragraphs.Last().Range);
            ParagraphProps.Alignment = ParagraphAlignment.Center;
            doc.EndUpdateParagraphs(ParagraphProps);

            CharacterProps = doc.BeginUpdateCharacters(doc.Paragraphs.Last().Range);
            CharacterProps.FontSize = titleSize;
            doc.EndUpdateCharacters(CharacterProps);

            doc.AppendText("\n");

            doc.AppendText(isubType.UnitDescription);
            ParagraphProps = doc.BeginUpdateParagraphs(doc.Paragraphs.Last().Range);
            ParagraphProps.Alignment = ParagraphAlignment.Left;
            doc.EndUpdateParagraphs(ParagraphProps);

            CharacterProps = doc.BeginUpdateCharacters(doc.Paragraphs.Last().Range);
            CharacterProps.FontSize = unitDescSize;
            doc.EndUpdateCharacters(CharacterProps);

            doc.AppendText("\n");
         }

         /// <summary>
         /// 設定欄位名稱
         /// </summary>
         protected virtual void SetTableColTitle() {
            SetTableStr(0, 0, "契約名稱");
            WordTableCell.PreferredWidthType = WidthType.Fixed;
            WordTableCell.PreferredWidth = DevExpress.Office.Utils.Units.InchesToDocumentsF(2.3f);
            WordTableCell.VerticalAlignment = TableCellVerticalAlignment.Center;
            WordTable.MergeCells(WordTableCell, WordTable[1, 0]);

            SetTableStr(0, 1, "調整後保證金金額");
            WordTable.MergeCells(WordTableCell, WordTable[0, 3]);

            SetTableStr(0, 2, "調整前保證金金額");
            WordTable.MergeCells(WordTableCell, WordTable[0, 4]);

            SetTableTitle(TableTitle, 1, "{0}{1}保證金", Environment.NewLine);
         }

         /// <summary>
         /// 寫入表格資料
         /// </summary>
         /// <param name="groupSubtype"></param>
         protected virtual void SetTableData(IGrouping<string, DataRow> groupSubtype) {
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
         }

         /// <summary>
         /// 寫入下方說明文
         /// </summary>
         protected virtual void SetAppendText() {
            doc.AppendText(AppendText);

            ParagraphProps = doc.BeginUpdateParagraphs(doc.Paragraphs.Last().Range);
            ParagraphProps.LineSpacingType = ParagraphLineSpacing.Single;

            doc.EndUpdateParagraphs(ParagraphProps);

            doc.AppendText(Environment.NewLine);
         }

         /// <summary>
         /// 取得Subtype 說明中文
         /// </summary>
         /// <param name="listGroupBySubtype"></param>
         /// <returns></returns>
         protected virtual string GenSubName(IEnumerable<IGrouping<string, DataRow>> listGroupBySubtype) {
            string subtypeName = "";
            int k = 0;
            foreach (var item in listGroupBySubtype) {
               //取得 Enum Description
               subtypeName += ((SubTypeName)Enum.Parse(typeof(SubTypeName), item.Key)).GetDesc();

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
         /// 取代rtf 開頭說明文
         /// </summary>
         /// <param name="docSev">檔案本體</param>
         /// <param name="listGroupBySubtype">分群資料</param>
         protected virtual void SetRtfDescText(RichEditDocumentServer docSev, IEnumerable<IGrouping<string, DataRow>> listGroupBySubtype) {
            string validDate = dt.Rows[0]["ISSUE_BEGIN_YMD"].AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);
            string ValidDatePrev = new MOCF().GetValidDatePrev(dt.Rows[0]["ISSUE_BEGIN_YMD"].AsString()).
                                    AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);

            //取代 word 開頭文字
            M40110Word m40110 = new M40110Word(ValidDatePrev, validDate, GenProdName(dt, ""), GenSubName(listGroupBySubtype));

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

      /// <summary>
      /// 一般 / 股票 輸出 rtf
      /// </summary>
      private class ExportWord0B : ExportWord {
         public ExportWord0B(D40xxx dao, string txtdate, string adjtype, string programId) : base(dao, txtdate, adjtype, programId) {
            FileChName = "40110_新聞稿_一般_股票";
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

            msg.Status = ResultStatus.Success;
            return msg;
         }

         public override ReturnMessageClass Export() {
            ReturnMessageClass msg = new ReturnMessageClass();
            msg.Status = ResultStatus.Fail;

            try {
               FilePath = PbFunc.wf_copy_file(ProgramId, FileChName);

               RichEditDocumentServer docSev = OpenRtfFile();

               SetRtfDescText(docSev, GenProdName(dt, "契約"));

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

               //設定文數字的自型
               SetAllNumberAndEnglishFont(doc);

               EndUpdate(docSev, msg);
               return msg;

            } catch (Exception ex) {
               ErrorHandle(ex, msg);
               return msg;
            }
         }
      }

      /// <summary>
      /// 處置股票 輸出 rtf
      /// </summary>
      private class ExportWord2B : ExportWord {
         public ExportWord2B(D40xxx dao, string txtdate, string adjtype, string programId) : base(dao, txtdate, adjtype, programId) {
            FileChName = "40110_新聞稿_處置股票";
            TableTitle = new string[] { "原始", "維持", "結算" };
            ColName = new string[] { "m_im", "m_mm", "m_cm", "cur_im", "cur_mm", "cur_cm" };
            ColNameB = new string[] { "m_im_b", "m_mm_b", "m_cm_b", "cur_im_b", "cur_mm_b", "cur_cm_b" };
         }

         public override ReturnMessageClass Export() {
            ReturnMessageClass msg = new ReturnMessageClass();
            msg.Status = ResultStatus.Fail;

            try {
               FilePath = PbFunc.wf_copy_file(ProgramId, FileChName);

               RichEditDocumentServer docSev = OpenRtfFile();

               //文章上半部填值
               SetRtfDescText(docSev, "");

               doc = docSev.Document;
               doc.BeginUpdate();

               #region 組上半部商品說明文章
               IEnumerable<IGrouping<string, DataRow>> listGroupByStockID = dt.AsEnumerable().GroupBy(g => g.Field<string>("stock_id"));

               string validDate = dt.Rows[0]["ISSUE_BEGIN_YMD"].AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);
               SetMainText(listGroupByStockID, validDate);

               doc.AppendText("參照證券市場處置措施，調整期間如遇休市、有價證券停止買賣、全日暫停交易，則恢復日順延執行。");

               CharacterProps = doc.BeginUpdateCharacters(doc.Paragraphs.Last().Range);
               CharacterProps.FontSize = 14;
               CharacterProps.FontName = "標楷體";
               doc.EndUpdateCharacters(CharacterProps);

               doc.AppendText("\n\n");

               doc.AppendText("本次保證金調整情形列表如下：");

               doc.AppendText("\n");

               #endregion

               foreach (DataRow dr in dt.Rows) {
                  #region Table上方單位文字

                  string unitDescription = "單位：比例(%)";
                  doc.AppendText(unitDescription);

                  ParagraphProps = doc.BeginUpdateParagraphs(doc.Paragraphs.Last().Range);
                  ParagraphProps.Alignment = ParagraphAlignment.Right;
                  doc.EndUpdateParagraphs(ParagraphProps);

                  CharacterProps = doc.BeginUpdateCharacters(doc.Paragraphs.Last().Range);
                  CharacterProps.FontSize = 12;
                  doc.EndUpdateCharacters(CharacterProps);

                  #endregion

                  // Table
                  CreateTable(doc, 3, 7);

                  #region 商品名稱

                  WordTableCell = WordTable[0, 0];
                  string kindName = $"{dr["kind_id"].AsString()}{Characters.LineBreak}({dr["kind_abbr_name"].AsString()})";
                  doc.InsertSingleLineText(WordTableCell.Range.Start, kindName);
                  WordTableCell.VerticalAlignment = TableCellVerticalAlignment.Center;
                  WordTableCell.PreferredWidthType = WidthType.Fixed;
                  WordTableCell.PreferredWidth = DevExpress.Office.Utils.Units.InchesToDocumentsF(1.5f);
                  WordTable.MergeCells(WordTableCell, WordTable[1, 0]);

                  #endregion

                  #region 調整後保證金適用比例的標題

                  WordTableCell = WordTable[0, 1];
                  doc.InsertSingleLineText(WordTableCell.Range.Start, "調整後保證金適用比例");
                  WordTable.MergeCells(WordTableCell, WordTable[0, 3]);

                  #endregion

                  #region 調整前保證金適用比例的標題

                  WordTableCell = WordTable[0, 2];
                  doc.InsertSingleLineText(WordTableCell.Range.Start, "調整前保證金適用比例");
                  WordTable.MergeCells(WordTableCell, WordTable[0, 4]);

                  #endregion

                  #region 保證金相關欄位

                  SetTableTitle(TableTitle, 1, "{0}{1}保證金", Characters.LineBreak.ToString());

                  // 如果AB_TYPE為A代表有AB值的話長不一樣
                  if (dr["ab_type"].AsString() == "A") {
                     doc.InsertSingleLineText(WordTable[2, 0].Range.Start, "風險保證金(A值)");

                     TableRow newRow = WordTable.Rows.Append();
                     doc.InsertSingleLineText(WordTable[newRow.Index, 0].Range.Start, "風險保證金最低值(B值)");
                     SetMMValue(doc, WordTable, newRow.Index, dr, "#0.000%", ColNameB);

                  } else {
                     doc.InsertSingleLineText(WordTable[2, 0].Range.Start, "保證金");
                  }

                  SetMMValue(doc, WordTable, 2, dr, "#0.00%", ColName);
                  #endregion

               }

               //設定文數字的自型
               SetAllNumberAndEnglishFont(doc);

               EndUpdate(docSev, msg);
               return msg;

            } catch (Exception ex) {
               ErrorHandle(ex, msg);
               return msg;
            }
         }

         protected virtual void SetMainText(IEnumerable<IGrouping<string, DataRow>> listGroupByStockID, string validDate) {
            string mainText = "";

            foreach (var itemGroup in listGroupByStockID) {
               DataRow rowAny = itemGroup.First();
               string stockID = itemGroup.Key;

               string productList = "";

               foreach (DataRow dr in itemGroup.ToList()) {
                  productList += dr["kind_abbr_name"].AsString() + "(" + dr["kind_id"].AsString() + ")" + "、";
               }

               productList = productList.TrimEnd('、');

               mainText += productList;
               string str = "所有月份保證金適用比例為現行所屬級距適用比例之{0}倍，本次調高係{1}(股票代號：{2})經證交所於{3}公布為處置有價證券" +
                  "(處置期間為{4}至{5})期交所依規定調高{6}契約所有月份保證金適用比例，自{7}(證券市場處至生效日次一營業日)該契約交易時段結束後起實施，" +
                  "並於證券市場處置期間結束後，於{5}該契約交易時段結束後恢復為調整前之保證金。";

               string kindAbbrName = rowAny["kind_abbr_name"].AsString().Replace("期貨", "").Replace("選擇權", "");

               mainText += string.Format(str, rowAny["adj_rate"].AsDecimal(0) + 1, kindAbbrName, rowAny["stock_id"].AsString(),
                                       rowAny["pub_ymd"].AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3),
                                       rowAny["impl_begin_ymd"].AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3),
                                       rowAny["impl_end_ymd"].AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3),
                                       productList, validDate);


               doc.AppendText(mainText);
               CharacterProperties characterProps = doc.BeginUpdateCharacters(doc.Paragraphs.Last().Range);
               characterProps.FontSize = 14;
               characterProps.FontName = "標楷體";
               doc.EndUpdateCharacters(characterProps);

               doc.AppendText("\n");
               mainText = "";
            }
         }
      }

      /// <summary>
      /// 長假調整 輸出 rtf
      /// </summary>
      private class ExportWord1B : ExportWordVacationAdjust {
         public ExportWord1B(D40xxx dao, string txtdate, string adjtype, string programId) : base(dao, txtdate, adjtype, programId) {
            FileChName = "40110_新聞稿_春節調整";
         }

         public override ReturnMessageClass Export() {
            ReturnMessageClass msg = base.Export();
            return msg;            
         }

         /// <summary>
         /// 取代rtf 開頭說明文
         /// </summary>
         /// <param name="docSev">檔案本體</param>
         /// <param name="listGroupBySubtype">分群資料</param>
         protected override void SetRtfDescText(RichEditDocumentServer docSev, IEnumerable<IGrouping<string, DataRow>> listGroupBySubtype) {
            DateTime validDate = dt.Rows[0]["ISSUE_BEGIN_YMD"].AsDateTime("yyyyMMdd");
            DateTime validDatePrev = new MOCF().GetValidDatePrev(dt.Rows[0]["ISSUE_BEGIN_YMD"].AsString()).AsDateTime("yyyyMMdd");

            string validDateStr = validDate.AsTaiwanDateTime("{0}年{1}月{2}日", 3);
            string validDatePrevStr = validDatePrev.AsTaiwanDateTime("{0}年{1}月{2}日", 3);
            string validDateMD = validDate.ToString("M月dd日");
            string validDatePrevMD = validDatePrev.ToString("M月dd日");
            string validDateYear = validDate.AsTaiwanDateTime("{0}", 3);

            //極特殊, 基本上一定會有TXF資料, 遇長假都會調整TXF
            DataRow TXFRow = dt.Select("kind_id='TXF'")[0];
            string adjRateTxf = TXFRow["adj_rate"].AsDecimal() != 0 ? TXFRow["adj_rate"].AsPercent(1) : string.Empty;
            decimal curIm = TXFRow["cur_im"].AsDecimal();
            decimal mIm = TXFRow["m_im"].AsDecimal();
            string curImTxf = curIm.ToString("#,##0");
            string mImTxf = mIm.ToString("#,##0");
            string DiffRateTxf = ((mIm - curIm) / curIm).ToString("#0.0%");

            DateTime validDateEnd = TXFRow["impl_end_ymd"].AsDateTime("yyyyMMdd");
            string validDateEndStr = validDateEnd.AsTaiwanDateTime("{0}年{1}月{2}日", 3);
            string validDateEndMDStr = validDateEnd.ToString("M月dd日");

            DateTime validDateEndPrevTwoAny = validDateEnd.AddDays(-2);
            string validDateEndPrevTwoAnyStr = validDateEndPrevTwoAny.ToString("M月dd日");

            DateTime validDateNextAny = validDate.AddDays(1);
            string validDateNextAnyMD = validDateNextAny.AsTaiwanDateTime("{0}年{1}月{2}日", 3);
            string daysCount = ((validDateEndPrevTwoAny - validDateNextAny).TotalDays + 1).ToString();

            //取代 word 開頭文字
            M40110Word m40110 = new M40110Word(validDateStr, validDateMD, validDateEndStr, validDateEndMDStr, validDateEndPrevTwoAnyStr,
                        validDatePrevStr, validDatePrevMD, validDateYear, validDateNextAnyMD, daysCount, GenSubName(listGroupBySubtype),
                        adjRateTxf, DiffRateTxf, curImTxf, mImTxf);

            //Options.MailMerge 要用List 才會有作用
            List<M40110Word> listM40110 = new List<M40110Word>();
            listM40110.Add(m40110);

            //直接replace word 上面的字
            docSev.Options.MailMerge.DataSource = listM40110;
            docSev.Options.MailMerge.ViewMergedData = true;
         }

         /// <summary>
         /// 設定欄位名稱
         /// </summary>
         protected override void SetTableColTitle() {
            SetTableStr(0, 0, "契約名稱");
            WordTableCell.PreferredWidthType = WidthType.Fixed;
            WordTableCell.PreferredWidth = DevExpress.Office.Utils.Units.InchesToDocumentsF(2.3f);
            WordTableCell.VerticalAlignment = TableCellVerticalAlignment.Center;
            WordTable.MergeCells(WordTableCell, WordTable[1, 0]);

            SetTableStr(0, 1, "現行收取標準");
            WordTable.MergeCells(WordTableCell, WordTable[0, 3]);

            SetTableStr(0, 2, "因應農曆春節調高保證金");
            WordTable.MergeCells(WordTableCell, WordTable[0, 4]);

            SetTableTitle(TableTitle, 1, "{0}{1}保證金", Environment.NewLine);
         }
      }

      /// <summary>
      /// 長假回調 輸出 rtf
      /// </summary>
      private class ExportWord1E : ExportWordVacationAdjust {
         public ExportWord1E(D40xxx dao, string txtdate, string adjtype, string programId) : base(dao, txtdate, adjtype, programId) {
            FileChName = "40110_新聞稿_春節回調";
         }

         public override ReturnMessageClass Export() {
            ReturnMessageClass msg = base.Export();
            return msg;
         }
      }

      /// <summary>
      /// 替換rtf 文字用class
      /// </summary>
      private class M40110Word {
         public string ValidDatePrev { get; set; }
         public string ValidDate { get; set; }
         public string ProductList { get; set; }
         public string SubtypeList { get; set; }
         public string ValidDateYear { get; set; }

         public string ValidDateNextAnyMD { get; set; }
         public string ValidDateEndPrevTwoAnyMD { get; set; }
         public string DaysCount { get; set; }
         public string ValidDateMD { get; set; }
         public string ValidDatePrevMD { get; set; }

         public string AdjRateTxf { get; set; }
         public string CurImTxf { get; set; }
         public string MImTxf { get; set; }
         public string DiffRateTxf { get; set; }
         public string ValidDateEnd { get; set; }
         public string ValidDateEndMD { get; set; }

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

         public M40110Word(string validdate, string validdatemd, string validdateend, string validdateendmd,
                           string validdateendprevtwoanymd, string validdateprev, string validdateprevmd, string validdateyear,
                           string validdatenextanymd, string dayscount, string subtypelist, string adjratetxf, string diffratetxf,
                           string curimtxf, string mimtxf) {

            ValidDate = validdate;
            ValidDateMD = validdatemd;
            ValidDateEnd = validdateend;
            ValidDateEndMD = validdateendmd;
            ValidDateEndPrevTwoAnyMD = validdateendprevtwoanymd;

            ValidDatePrev = validdateprev;
            ValidDatePrevMD = validdateprevmd;
            ValidDateYear = validdateyear;
            ValidDateNextAnyMD = validdatenextanymd;
            DaysCount = dayscount;

            SubtypeList = subtypelist;
            AdjRateTxf = adjratetxf;
            DiffRateTxf = diffratetxf;
            CurImTxf = curimtxf;
            MImTxf = mimtxf;
         }

      }

      /// <summary>
      /// interface 依照不同subType 有不同參數
      /// </summary>
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

      /// <summary>
      /// interface 依照不同AmtType 有不同參數
      /// </summary>
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
            BeforeAdjustTitle = "調整前保證金金額";
            AfterAdjustTitle = "調整後保證金金額";
            NumberFormat = "#,##0.####";

         }

      }
   }
}