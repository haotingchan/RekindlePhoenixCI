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

      public W40110(string programID, string programName) : base(programID, programName) {
         dao40xxx = new D40xxx();
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtDate.DateTimeValue = GlobalInfo.OCF_DATE;

         //設定下拉選單
         ddlAdjType.SetDataTable(new COD().ListByCol2("400xx", "dw_adj_type"), "COD_ID", "COD_DESC", TextEditStyles.DisableTextEditor, null);
         ddlAdjType.EditValue = "0B";

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
            fileChName.Add("1B", "40110_新聞稿_春節回調");
            fileChName.Add("1E", "40110_新聞稿_春節調整");
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

      private static void ReplaceXmlInnterText(XmlNode element, string oldString, string newString) {

         string innertext = element.InnerText;
         innertext = innertext.Replace(oldString, newString);
         element.InnerText = innertext;

      }

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
      /// 產生簽核的表格
      /// </summary>
      private static void CreateSignTable(Document doc) {
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
         tableSign.ForEachCell((c, i, j) => {
            c.VerticalAlignment = TableCellVerticalAlignment.Center;
         });
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

                  string validDate = PbFunc.f_conv_date(dt.Rows[0]["ISSUE_BEGIN_YMD"].AsDateTime("yyyyMMdd"), 3);
                  string ValidDatePrev = PbFunc.f_conv_date(new MOCF().GetValidDatePrev(dt.Rows[0]["ISSUE_BEGIN_YMD"].AsString()).AsDateTime("yyyyMMdd"), 3);
                  string prodName = "";
                  int k = 0;

                  foreach (DataRow dr in dt.Rows) {

                     if (dr["prod_subtype"].AsString() == "S") {
                        prodName += dr["kind_abbr_name"].AsString() + "契約(" + dr["kind_id"].AsString() + ")";
                     } else {
                        prodName += dr["kind_abbr_name"].AsString();
                     }

                     if (k == (dt.Rows.Count - 2)) {
                        prodName += "及";
                     } else {
                        prodName += "、";
                     }

                     k++;
                  }

                  prodName = prodName.TrimEnd('、');

                  prodName += "之期貨契約保證金及選擇權契約";

                  if (dt.Select("ab_type='A'").Count<DataRow>() > 0) {
                     prodName += "風險保證金(A值)、風險保證金最低值(B值)";
                  }

                  M40110Word m40110 = new M40110Word(ValidDatePrev, validDate, prodName);

                  List<M40110Word> listM40110 = new List<M40110Word>();
                  listM40110.Add(m40110);

                  docSev.Options.MailMerge.DataSource = listM40110;
                  docSev.Options.MailMerge.ViewMergedData = true;

                  Document doc = docSev.Document;
                  doc.BeginUpdate();

                  foreach(DataRow dr in dt.Rows){

                     TableCell cell;
                     Table table = doc.Tables.Create(doc.Range.End, 4, 7, AutoFitBehaviorType.AutoFitToWindow);
                     ParagraphProperties paragraphProps = doc.BeginUpdateParagraphs(table.Range);

                     SetTableStyle(doc, paragraphProps, table);

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
                     cell.Borders.Top.LineStyle = TableBorderLineStyle.None;
                     cell.Borders.Left.LineStyle = TableBorderLineStyle.None;
                     cell.Borders.Right.LineStyle = TableBorderLineStyle.None;

                  }

                  CreateSignTable(doc);

                  docSev.SaveDocument(filePath, DocumentFormat.Rtf);
               }
               System.Diagnostics.Process.Start(filePath);
            } catch (Exception ex) {
               throw ex;
            }
         }
      }

      private class M40110Word {
         public string ValidDatePrev { get; set; }
         public string ValidDate { get; set; }
         public string ProductList { get; set; }

         public M40110Word(string validdateprev, string validdate, string productlist) {
            ValidDatePrev = validdateprev;
            ValidDate = validdate;
            ProductList = productlist;
         }
      }
   }
}