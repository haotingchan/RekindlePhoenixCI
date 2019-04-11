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
using System.Linq;
using System.Reflection;
using System.Xml;

/// <summary>
/// Test Data 2B 20190102 / 1B 20190130 / 1E 20190212 / 0B 20190212
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
   public partial class W40110 : FormParent {
      private static D40xxx dao40xxx;

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
         dao40xxx = new D40xxx();
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtDate.DateTimeValue = GlobalInfo.OCF_DATE;

         //設定下拉選單
         ddlAdjType.SetDataTable(new COD().ListByCol2("400xx", "dw_adj_type"), "COD_ID", "COD_DESC", TextEditStyles.DisableTextEditor, null);
         ddlAdjType.EditValue = "0B";

#if DEBUG
         txtDate.DateTimeValue = ("20190212").AsDateTime("yyyyMMdd");
         ddlAdjType.EditValue = "1E";
#endif

         ExportShow.Hide();
      }

      protected override ResultStatus Export() {
         ExportShow.Text = "轉檔中...";
         ExportShow.Show();

         try {
            DataTable dt = new DataTable();
            string asAdjType = AdjType.SubStr(0, 1) == "0" ? "" : AdjType.SubStr(0, 1);

            Dictionary<string, string> fileChName = new Dictionary<string, string>();

            fileChName.Add("0B", "40110_新聞稿_一般_股票");
            fileChName.Add("1B", "40110_新聞稿_春節調整");
            fileChName.Add("1E", "40110_新聞稿_春節回調");
            fileChName.Add("2B", "40110_新聞稿_處置股票");

            dt = dao40xxx.GetData(TxtDate, asAdjType, AdjType.SubStr(1, 1));

            //一般 / 股票 要多撈一次資料
            if (AdjType == "0B") {
               DataTable dtTmp = dao40xxx.GetData(TxtDate, "3", AdjType.SubStr(1, 1));
               foreach (DataRow r in dtTmp.Rows) {
                  DataRow addRow = r;
                  dt.ImportRow(r);
               }
            }

            #region Check has Data
            if (dt == null) {
               ExportShow.Hide();
               MessageDisplay.Info(MessageDisplay.MSG_NO_DATA);
               return ResultStatus.Fail;
            }

            if (dt.Rows.Count <= 0) {
               ExportShow.Hide();
               MessageDisplay.Info(MessageDisplay.MSG_NO_DATA);
               return ResultStatus.Fail;
            }
            #endregion

            dt = dt.Sort("SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC, KIND_ID ASC, AB_TYPE ASC");
            string destinationFilePath = PbFunc.wf_copy_file(_ProgramID, fileChName[AdjType]);

            IXml40xxxData xmlData = CreateXmlData(GetType(), "ExportWord" + AdjType);
            xmlData.Export(dt, destinationFilePath);

         } catch (Exception ex) {
            ExportShow.Text = "轉檔失敗";
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

      /// <summary>
      /// 直接替換 rtf 上面文字
      /// </summary>
      /// <param name="docSev"></param>
      /// <param name="m40110"></param>
      private static void ReplaceWrod(RichEditDocumentServer docSev, M40110Word m40110) {

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
      private static void SetTableStyle(Document doc, ParagraphProperties paragraphProps, Table table) {

         // 預設Table內容都全部置中
         paragraphProps.Alignment = ParagraphAlignment.Center;
         doc.EndUpdateParagraphs(paragraphProps);

         // 預設Table內的字體
         CharacterProperties tableProp = doc.BeginUpdateCharacters(table.Range);
         tableProp.FontSize = 12;
         doc.EndUpdateCharacters(tableProp);

         // 垂直置中
         table.ForEachCell((c, i, j) => {
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
      private static void SetMMValue(Document doc, Table table, int rowIndedx, DataRow dr, string format, string[] colList) {
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
      private static string GenProdName(DataTable dt, string contract = "") {
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

      /// <summary>
      /// 產生簽核的表格
      /// </summary>
      private static void CreateSignTable(Document doc) {

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

      public IXml40xxxData CreateXmlData(Type type, string name) {

         string AssemblyName = type.Namespace.Split('.')[0];//最後compile出來的dll名稱
         string className = type.FullName + "+" + name;//完整的class路徑(注意,內部的class執行時其fullName是用+號連結起來)
         return (IXml40xxxData)Assembly.Load(AssemblyName).CreateInstance(className);
      }

      /// <summary>
      /// 一般 / 股票 輸出 rtf
      /// </summary>
      private class ExportWord0B : IXml40xxxData {
         public void Export(DataTable dt, string filePath) {
            try {
               using (RichEditDocumentServer docSev = new RichEditDocumentServer()) {
                  docSev.LoadDocument(filePath);

                  string validDate = dt.Rows[0]["ISSUE_BEGIN_YMD"].AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);
                  string ValidDatePrev = new MOCF().GetValidDatePrev(dt.Rows[0]["ISSUE_BEGIN_YMD"].AsString()).
                                          AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);

                  string prodName = "";

                  prodName = GenProdName(dt, "契約");

                  //取代 word 開頭文字
                  ReplaceWrod(docSev, new M40110Word(ValidDatePrev, validDate, prodName));

                  Document doc = docSev.Document;
                  doc.BeginUpdate();

                  foreach (DataRow dr in dt.Rows) {

                     TableCell cell;
                     Table table = doc.Tables.Create(doc.Range.End, 4, 7, AutoFitBehaviorType.AutoFitToWindow);
                     ParagraphProperties paragraphProps = doc.BeginUpdateParagraphs(table.Range);

                     SetTableStyle(doc, paragraphProps, table);

                     #region 單位幣別或比例標題

                     cell = table[0, 6];
                     string unitStr = "";

                     if (dr["amt_type"].AsString() == "F") {
                        unitStr = dr["currency_name"].AsString();
                     } else if (dr["amt_type"].AsString() == "P") {
                        unitStr = "比例(%)";
                     }

                     doc.InsertSingleLineText(cell.Range.Start, "單位：" + unitStr);

                     paragraphProps = doc.BeginUpdateParagraphs(cell.Range);
                     paragraphProps.Alignment = ParagraphAlignment.Right;
                     doc.EndUpdateParagraphs(paragraphProps);
                     table.MergeCells(table[0, 0], cell);

                     cell = table[0, 0];
                     cell.CellSetBorders(TableBorderLineStyle.None, TableBorderLineStyle.None, TableBorderLineStyle.None);

                     #endregion

                     #region 商品名稱

                     cell = table[1, 0];

                     string productName = "";

                     if (dr["amt_type"].AsString() == "F") {
                        productName = dr["kind_id_out"].AsString();
                     } else if (dr["amt_type"].AsString() == "P") {
                        productName = dr["kind_id_out"].AsString() + Characters.LineBreak + "(" + dr["kind_abbr_name"].AsString() + ")";
                     }

                     doc.InsertSingleLineText(cell.Range.Start, productName);
                     cell.VerticalAlignment = TableCellVerticalAlignment.Center;
                     table.MergeCells(cell, table[2, 0]);

                     #endregion

                     #region 調整後保證金金額的標題

                     string afterAdjustTitle = "";

                     if (dr["amt_type"].AsString() == "F") {
                        afterAdjustTitle = "調整後保證金金額";
                     } else if (dr["amt_type"].AsString() == "P") {
                        if (dr["m_level"].AsString() == "Z") {
                           afterAdjustTitle = "調整後保證金適用比例";
                        } else {
                           afterAdjustTitle = "調整後保證金適用比例" + Characters.LineBreak + "(" + dr["m_level_name"].AsString() + ")";
                        }

                     }

                     cell = table[1, 1];
                     doc.InsertSingleLineText(cell.Range.Start, afterAdjustTitle);
                     table.MergeCells(cell, table[1, 3]);

                     #endregion

                     #region 調整前保證金金額標題

                     string beforeAdjustTitle = "";

                     if (dr["amt_type"].AsString() == "F") {
                        beforeAdjustTitle = "調整前保證金金額";
                     } else if (dr["amt_type"].AsString() == "P") {
                        if (dr["cur_level"].AsString() == "Z") {
                           beforeAdjustTitle = "調整前保證金適用比例";
                        } else {
                           beforeAdjustTitle = "調整前保證金適用比例" + Characters.LineBreak + "(" + dr["cur_level_name"].AsString() + ")";
                        }
                     }

                     cell = table[1, 2];
                     doc.InsertSingleLineText(cell.Range.Start, beforeAdjustTitle);
                     table.MergeCells(cell, table[1, 4]);

                     #endregion

                     #region 保證金相關欄位

                     string numberFormat = "#,##0.####";

                     if (dr["amt_type"].AsString() == "P") {
                        numberFormat = "#0.00%";
                     }

                     string moneyOrPercent = "";

                     if (dr["amt_type"].AsString() == "F") {
                        moneyOrPercent = "金額";
                     } else if (dr["amt_type"].AsString() == "P") {
                        moneyOrPercent = "比例";
                     }

                     // 如果AB_TYPE為A代表有AB值的話長不一樣
                     if (dr["ab_type"].AsString() == "A") {

                        string[] titleText = { "原始", "維持", "結算" };
                        for (int i = 0; i < 3; i++) {
                           doc.InsertSingleLineText(table[2, i + 1].Range.Start, string.Format("計算賣出選擇權{0}保證金之適用{1}", titleText[i], moneyOrPercent));
                           doc.InsertSingleLineText(table[2, i + 4].Range.Start, string.Format("計算賣出選擇權{0}保證金之適用{1}", titleText[i], moneyOrPercent));
                        }

                        cell = table[3, 0];
                        doc.InsertSingleLineText(cell.Range.Start, "風險保證金(A值)");
                        cell.PreferredWidthType = WidthType.Fixed;
                        cell.PreferredWidth = DevExpress.Office.Utils.Units.InchesToDocumentsF(1f);

                        table.Rows.Append();

                        cell = table[4, 0];
                        doc.InsertSingleLineText(cell.Range.Start, "風險保證金最低值(B值)");
                        cell.PreferredWidthType = WidthType.Fixed;

                        string valueBFormat = numberFormat;

                        // 如果是比例又是B值的話，顯示百分比到小數第三位
                        if (dr["amt_type"].AsString() == "P") {
                           valueBFormat = "#0.000%";
                        }

                        //填寫B資料
                        string[] colNameB = { "m_im_b", "m_mm_b", "m_cm_b", "cur_im_b", "cur_mm_b", "cur_cm_b" };
                        SetMMValue(doc, table, 4, dr, valueBFormat, colNameB);

                     } else {

                        string[] titleText = { "原始", "維持", "結算" };
                        for (int i = 0; i < 3; i++) {
                           doc.InsertSingleLineText(table[2, i + 1].Range.Start, string.Format("{0}{1}保證金", titleText[i], Characters.LineBreak));
                           doc.InsertSingleLineText(table[2, i + 4].Range.Start, string.Format("{0}{1}保證金", titleText[i], Characters.LineBreak));
                        }

                        cell = table[3, 0];
                        doc.InsertSingleLineText(cell.Range.Start, "保證金");
                        cell.PreferredWidthType = WidthType.Fixed;
                        cell.PreferredWidth = DevExpress.Office.Utils.Units.InchesToDocumentsF(0.7f);
                     }

                     //填寫資料
                     string[] colName = { "m_im", "m_mm", "m_cm", "cur_im", "cur_mm", "cur_cm" };
                     SetMMValue(doc, table, 3, dr, numberFormat, colName);

                     #endregion

                     doc.AppendText(Environment.NewLine);
                  }// foreach

                  CreateSignTable(doc);
                  doc.EndUpdate();
                  docSev.SaveDocument(filePath, DocumentFormat.Rtf);
               }//using 

               System.Diagnostics.Process.Start(filePath);

            } catch (Exception ex) {
               throw ex;
            }
         }
      }

      /// <summary>
      /// 長假回調 輸出 rtf
      /// </summary>
      private class ExportWord1E : IXml40xxxData {
         public void Export(DataTable dt, string filePath) {
            try {
               using (RichEditDocumentServer docSev = new RichEditDocumentServer()) {
                  docSev.LoadDocument(filePath);

                  string validDate = dt.Rows[0]["ISSUE_BEGIN_YMD"].AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);
                  string ValidDatePrev = new MOCF().GetValidDatePrev(dt.Rows[0]["ISSUE_BEGIN_YMD"].AsString()).
                                          AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);

                  string subtypeName = "";
                  int k = 0;

                  IEnumerable<IGrouping<string, DataRow>> listGroupBySubtype = dt.AsEnumerable().GroupBy(g => g.Field<string>("PROD_SUBTYPE"));

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

                  //取代 word 開頭文字
                  ReplaceWrod(docSev, new M40110Word(ValidDatePrev, validDate, GenProdName(dt), subtypeName));

                  //內文表格
                  Document doc = docSev.Document;
                  doc.BeginUpdate();

                  foreach (var groupSubtype in listGroupBySubtype) {

                     #region Table上方的標題
                     SubtypeInfo subtypeInfo = new SubtypeInfo(groupSubtype.Key);

                     ParagraphProperties paragraphProps;
                     CharacterProperties characterProps;

                     doc.AppendText(subtypeInfo.TableTitle);
                     paragraphProps = doc.BeginUpdateParagraphs(doc.Paragraphs.Last().Range);
                     paragraphProps.Alignment = ParagraphAlignment.Center;
                     doc.EndUpdateParagraphs(paragraphProps);

                     characterProps = doc.BeginUpdateCharacters(doc.Paragraphs.Last().Range);
                     characterProps.FontSize = 14;
                     doc.EndUpdateCharacters(characterProps);

                     doc.AppendText("\n");

                     doc.AppendText(subtypeInfo.UnitDescription);
                     paragraphProps = doc.BeginUpdateParagraphs(doc.Paragraphs.Last().Range);
                     paragraphProps.Alignment = ParagraphAlignment.Left;
                     doc.EndUpdateParagraphs(paragraphProps);

                     characterProps = doc.BeginUpdateCharacters(doc.Paragraphs.Last().Range);
                     characterProps.FontSize = 12;
                     doc.EndUpdateCharacters(characterProps);

                     doc.AppendText("\n");

                     #endregion

                     #region Table

                     // Table
                     Table table = doc.Tables.Create(doc.Range.End, 2, 7);
                     paragraphProps = doc.BeginUpdateParagraphs(table.Range);

                     SetTableStyle(doc, paragraphProps, table);

                     #region Table的標題

                     TableCell cell;

                     cell = table[0, 0];
                     doc.InsertSingleLineText(cell.Range.Start, "契約名稱");
                     cell.PreferredWidthType = WidthType.Fixed;
                     cell.PreferredWidth = DevExpress.Office.Utils.Units.InchesToDocumentsF(2.3f);
                     cell.VerticalAlignment = TableCellVerticalAlignment.Center;
                     table.MergeCells(cell, table[1, 0]);

                     cell = table[0, 1];
                     doc.InsertSingleLineText(cell.Range.Start, "調整前保證金金額");
                     table.MergeCells(cell, table[0, 3]);

                     cell = table[0, 2];
                     doc.InsertSingleLineText(cell.Range.Start, "調整後保證金金額");
                     table.MergeCells(cell, table[0, 4]);

                     string[] cellTitle = { "結算", "維持", "原始" };
                     for (int i = 0; i < 3; i++) {
                        doc.InsertSingleLineText(table[1, i + 1].Range.Start, string.Format("{0}{1}保證金", cellTitle[i], Characters.LineBreak));
                        doc.InsertSingleLineText(table[1, i + 4].Range.Start, string.Format("{0}{1}保證金", cellTitle[i], Characters.LineBreak));
                     }
                     #endregion

                     #region 在每一種商品類別裡面填入資料

                     foreach (DataRow dr in dt.Select(string.Format("prod_subtype ='{0}'", groupSubtype.Key))) {
                        TableRow tableRow = table.Rows.Append();

                        string kindAbbrName = dr["kind_abbr_name"].AsString();

                        cell = tableRow.FirstCell;

                        if (dr["ab_type"].AsString() == "A") {
                           doc.InsertSingleLineText(cell.Range.Start, kindAbbrName + "風險保證金(A值)");
                        } else {
                           doc.InsertSingleLineText(cell.Range.Start, kindAbbrName);
                        }

                        paragraphProps = doc.BeginUpdateParagraphs(cell.Range);
                        paragraphProps.Alignment = ParagraphAlignment.Left;
                        doc.EndUpdateParagraphs(paragraphProps);

                        string[] curColName = { "cur_im2", "cur_im1", "cur_im", "m_im2", "m_im1", "m_im" };
                        SetMMValue(doc, table, tableRow.Index, dr, "#,##0", curColName);

                        // 如果ab_type為A，代表有AB值，這裡加上B值的列
                        if (dr["ab_type"].AsString() == "A") {

                           tableRow = table.Rows.Append();
                           cell = tableRow.FirstCell;

                           if (dr["ab_type"].AsString() == "A") {
                              doc.InsertSingleLineText(cell.Range.Start, kindAbbrName + "風險保證金最低值(B值)");
                           } else {
                              doc.InsertSingleLineText(cell.Range.Start, kindAbbrName);
                           }

                           string[] curColNameB = { "cur_cm_b", "cur_mm_b", "cur_im_b", "m_cm_b", "m_mm_b", "m_im_b" };
                           SetMMValue(doc, table, tableRow.Index, dr, "#,##0", curColNameB);
                        }

                     }// foreach

                     #endregion

                     #endregion

                     #region 如果是匯率類的話，表格下方加一段註解

                     if (groupSubtype.Key == "E") {
                        doc.AppendText("※本公司上揭契約公告之保證金收取金額，小型美元兌人民幣期貨、美元兌人民幣期貨、" +
                           "小型美元兌人民幣選擇權及美元兌人民幣選擇權為人民幣計價；澳幣兌美元期貨、英鎊兌美元期貨、歐元兌美元期貨為美元計價；美元兌日圓期貨為日圓計價。");

                        paragraphProps = doc.BeginUpdateParagraphs(doc.Paragraphs.Last().Range);
                        paragraphProps.LineSpacingType = ParagraphLineSpacing.Single;
                        doc.EndUpdateParagraphs(paragraphProps);

                        doc.AppendText(Characters.LineBreak.ToString());
                     }

                     #endregion

                  }// foreach

                  CreateSignTable(doc);
                  doc.EndUpdate();
                  docSev.SaveDocument(filePath, DocumentFormat.Rtf);
               }//using 

               System.Diagnostics.Process.Start(filePath);
            } catch (Exception ex) {
               throw ex;
            }
         }
      }

      /// <summary>
      /// 處置股票 輸出 rtf
      /// </summary>
      private class ExportWord2B : IXml40xxxData {
         public void Export(DataTable dt, string filePath) {
            try {

               using (RichEditDocumentServer docSev = new RichEditDocumentServer()) {
                  docSev.LoadDocument(filePath);

                  string validDate = dt.Rows[0]["ISSUE_BEGIN_YMD"].AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);
                  string ValidDatePrev = new MOCF().GetValidDatePrev(dt.Rows[0]["ISSUE_BEGIN_YMD"].AsString()).
                                          AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);

                  ReplaceWrod(docSev, new M40110Word(ValidDatePrev, validDate, ""));

                  Document doc = docSev.Document;
                  doc.BeginUpdate();




               }

            } catch (Exception ex) {
               throw ex;
            }
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

      private class SubtypeInfo {
         public string SubTypeId { get; set; }
         public string TableTitle { get; set; }
         public string UnitDescription { get; set; }

         public SubtypeInfo(string subtypeid) {
            SubTypeId = subtypeid;

            switch (subtypeid) {
               case "I":
                  TableTitle = "股價指數類期貨及選擇權契約保證金調整前後金額";
                  UnitDescription = "單位：新臺幣元";
                  break;
               case "C":
                  TableTitle = "商品類期貨及選擇權契約保證金調整前後金額";
                  UnitDescription = "單位：黃金期貨為美元計價，其餘商品為新臺幣元";
                  break;
               case "E":
                  TableTitle = "匯率類期貨及選擇權契約保證金調整前後金額";
                  UnitDescription = "單位：人民幣、美元及日圓";
                  break;
               default:
                  TableTitle = "其他";
                  UnitDescription = "單位：尚未定義";
                  break;
            }
         }
      }
   }
}