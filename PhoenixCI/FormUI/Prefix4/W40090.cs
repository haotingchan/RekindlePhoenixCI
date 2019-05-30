using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao;
using DevExpress.Spreadsheet;
using DevExpress.XtraEditors.Controls;
using PhoenixCI.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using Table = DevExpress.XtraRichEdit.API.Native.Table;

/// <summary>
/// Test Data 2B 20190102 / 1B 20190130 / 1E 20190212 / 0B 20190212
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
   public partial class W40090 : FormParent {
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

      public W40090(string programID, string programName) : base(programID, programName) {
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

#if DEBUG
         txtDate.DateTimeValue = "20190304".AsDateTime("yyyyMMdd");
         ddlAdjType.EditValue = "2B";
#endif
      }

      protected override ResultStatus AfterOpen() {

#if RELEASE
         MessageDisplay.Info("施工中 !!");
         this.Close();
#endif
         return ResultStatus.Success;
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
         protected virtual DataTable Dt { get; set; }

         protected virtual string AsAdjType {
            get {
               return AdjType.SubStr(0, 1) == "0" ? "" : AdjType.SubStr(0, 1);
            }
         }
         protected virtual string FilePath { get; set; }
         protected virtual XmlDocument Doc { get; set; }
         protected virtual List<string> KindNameList_Desc { get; set; }
         protected virtual List<string> KindNameList { get; set; }

         protected virtual string DescTxt { get; set; }
         protected virtual bool AddDescElement { get; set; }
         protected virtual string[] ColsA { get; set; }
         protected virtual string[] ColsB { get; set; }

         public ExportXml(D40xxx dao, string txtdate, string adjtype, string programId) {
            Dao = dao;
            TxtDate = txtdate;
            AdjType = adjtype;
            ProgramId = programId;
            AddDescElement = false;

            KindNameList_Desc = new List<string>();
            KindNameList = new List<string>();
            ColsA = new string[] { "m_im", "m_mm", "m_cm", "cur_im", "cur_mm", "cur_cm" };
            //ColsA = new string[] { "m_im", "m_im1", "m_im2", "cur_im", "cur_im1", "cur_im2" };
            ColsB = new string[] { "m_im_b", "m_mm_b", "m_cm_b", "cur_im_b", "cur_mm_b", "cur_cm_b" };
         }

         public virtual ReturnMessageClass GetData() {
            ReturnMessageClass msg = new ReturnMessageClass(MessageDisplay.MSG_NO_DATA);
            msg.Status = ResultStatus.Fail;

            Dt = Dao.GetData(TxtDate, AsAdjType, AdjType.SubStr(1, 1));

            if (Dt != null) {
               if (Dt.Rows.Count > 0) {
                  msg.Status = ResultStatus.Success;
               }
            }

            //Dt.Filter("ab_type in ('A','-')");
            return msg;
         }

         public virtual ReturnMessageClass Export() {
            ReturnMessageClass msg = new ReturnMessageClass();
            msg.Status = ResultStatus.Fail;

            try {
               #region ifd
               FilePath = PbFunc.wf_copy_file(ProgramId, $"{ProgramId}_{AdjType}");

               OpenFileAndSetYear();

               string prepoStr = Dt.AsEnumerable().Any(d => d.Field<string>("prod_type") == "F") ? "期貨契約保證金及" : "";
               List<string> subTypeString = new List<string>();
               List<string> adjRate = new List<string>();
               //說明文
               foreach (DataRow dr in Dt.Rows) {
                  string abbrName = dr["KIND_ABBR_NAME"].AsString();
                  string fullName = $"本公司{dr["rule_full_name"].AsString()}交易規則";

                  GenKindNameList(dr, prepoStr, abbrName, fullName);

                  string subTypeName = GetProdSubTypeDesc(dr["prod_subtype"].AsString());
                  if (!subTypeString.Exists(s => s == subTypeName)) {
                     subTypeString.Add(subTypeName);

                     //rfa 檔會用到的參數
                     adjRate.Add(dr["adj_rate"].AsPercent(2));
                  }
               }

               string implBeginDate = Dt.Rows[0]["impl_begin_ymd"].AsString();
               string implEndDate = Dt.Rows[0]["impl_end_ymd"].AsString();
               string mocfDate = new MOCF().GetMaxOcfDate(implBeginDate, implEndDate);

               implBeginDate = implBeginDate.AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);
               implEndDate = implEndDate.AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);
               mocfDate = mocfDate.AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);

               ReplaceElementWord(GenArrayTxt(KindNameList), GenArrayTxt(KindNameList_Desc), GenArrayTxt(subTypeString), implBeginDate,
                                    implEndDate, mocfDate);

               Doc.Save(FilePath);
               #endregion

               #region ifd 簽核 ifd_rfa
               FilePath = PbFunc.wf_copy_file(ProgramId, $"{ProgramId}_{AdjType}_rfa");
               OpenFileAndSetYear();

               ReplaceElementWordRfa(GenArrayTxt(subTypeString), GenArrayTxt(adjRate), implBeginDate, implEndDate, mocfDate);
               Doc.Save(FilePath);
               #endregion

               #region Excel rtf
               //保證金
               FilePath = PbFunc.wf_copy_file(ProgramId, ProgramId);
               Workbook workbook = new Workbook();
               workbook.LoadDocument(FilePath);
               Worksheet ws = workbook.Worksheets["保證金(Fut)"];
               ExcelFut(ws, Dt);

               ws = workbook.Worksheets["保證金(Opt)"];
               ExcelOpt(ws, Dt);

               RichEditDocumentServer rtf = new RichEditDocumentServer();
               rtf.CreateNewDocument();
               Document rtfDoc = rtf.Document;

               WordFut(rtfDoc, Dt);

               string wordFilePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, $"40090_{DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss")}.rtf");
               rtf.SaveDocument(wordFilePath, DevExpress.XtraRichEdit.DocumentFormat.Rtf);
               rtf.Dispose();


               //Span 資料
               DataTable dtSpan = Dao.GetSpanData(TxtDate.AsDateTime("yyyyMMdd"));
               if (dtSpan.Rows.Count > 0) {
                  ws = workbook.Worksheets["SPAN參數"];
                  ExcelSpan(ws, dtSpan);

                  WordSpan(dtSpan);
               }
               workbook.SaveDocument(FilePath);
               #endregion

               msg.Status = ResultStatus.Success;
               return msg;
            } catch (Exception ex) {
               ErrorHandle(ex, msg);
               return msg;
            }
         }

         protected virtual void OpenFileAndSetYear() {
            Doc = new XmlDocument();
            Doc.Load(FilePath);

            //年號
            ReplaceXmlInnterText(Doc.GetElementsByTagName("檔號")[0], "#year#", DateTime.Now.AsTaiwanDateTime("{0}", 4));
            ReplaceXmlInnterText(Doc.GetElementsByTagName("年度號")[0], "#year#", DateTime.Now.AsTaiwanDateTime("{0}", 4));
         }

         protected virtual void ReplaceXmlInnterText(XmlNode element, string oldString, string newString) {

            string innertext = element.InnerText;
            innertext = innertext.Replace(oldString, newString);
            element.InnerText = innertext;
         }

         protected virtual void ReplaceElementWord(params string[] args) {

            ReplaceXmlInnterText(Doc.GetElementsByTagName("主旨")[0].ChildNodes[0], "#kind_name_list#", args[0]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[0].ChildNodes[0], "#full_name_llist#", args[1]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[1].ChildNodes[0], "#prod_subtype_list#", args[2]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[1].ChildNodes[0], "#impl_begin_ymd#", args[3]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[1].ChildNodes[0], "#impl_end_ymd#", args[4]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[1].ChildNodes[0], "#mocf_ymd#", args[5]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[1].ChildNodes[0], "#impl_end_ymd#", args[4]);
         }

         protected virtual void ReplaceElementWordRfa(params string[] args) {

            ReplaceXmlInnterText(Doc.GetElementsByTagName("主旨")[0].ChildNodes[0], "#prod_subtype_list#", args[0]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[1].ChildNodes[0], "#year#", DateTime.Now.AsTaiwanDateTime("{0}", 3));
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[1].ChildNodes[0], "#prod_subtype_list#", args[0]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[1].ChildNodes[0], "#adj_rate#", args[1]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[2].ChildNodes[0], "#impl_begin_ymd#", args[2]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[2].ChildNodes[0], "#impl_end_ymd#", args[3]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[2].ChildNodes[0], "#mocf_ymd#", args[4]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[2].ChildNodes[0], "#impl_end_ymd#", args[3]);
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

         protected virtual string GetProdSubTypeDesc(string prodsubtype) {
            string re = "";

            re = Dao.GetProdSubType(prodsubtype);

            return re;
         }

         protected virtual void GenKindNameList(DataRow dr, string prepoStr, string abbrName, string abbrName_Desc) {

            if (dr["prod_subtype"].AsString() == "S") {
               abbrName += $"契約({dr["kind_id"].AsString()})";
            }

            if (dr["prod_type"].AsString() == "O" && dr == Dt.Rows[Dt.Rows.Count - 1]) {
               abbrName += $"之{prepoStr}選擇權契約風險保證金(A值)、風險保證金最低值(B值)";
            }

            if (!KindNameList.Exists(k => k == abbrName)) {
               KindNameList.Add(abbrName);
            }

            if (!KindNameList_Desc.Exists(f => f == abbrName_Desc)) {
               KindNameList_Desc.Add(abbrName_Desc);
            }
         }

         protected virtual void ExcelSpan(Worksheet ws, DataTable dtSpan) {

            if (dtSpan.Rows.Count == 0) return;

            int rowIndex = 2;
            foreach (DataRow dr in dtSpan.Rows) {
               int colIndex = 1;

               foreach (DataColumn dc in dtSpan.Columns) {

                  ws.Cells[rowIndex, colIndex].SetValue(dr[dc]);
                  colIndex++;
               }
               rowIndex += 4;
            }

         }

         protected virtual void ExcelFut(Worksheet ws, DataTable dtFut) {

            int rowIndex = 0;
            //幣別
            List<DataRow> drsTmp = dtFut.Select("prod_type='F' and amt_type = 'F'").ToList();
            if (drsTmp.Count > 0) {
               foreach (DataRow dr in drsTmp) {
                  int colIndex = 1;

                  ws.Cells[rowIndex, colIndex].SetValue("單位：" + dr["currency_name"].AsString());
                  rowIndex += 1;
                  ws.Cells[rowIndex, colIndex].SetValue(dr["kind_id_out"].AsString());
                  rowIndex += 2;
                  colIndex = 2;

                  foreach (string dc in ColsA) {
                     ws.Cells[rowIndex, colIndex].SetValue(dr[dc].AsDecimal());
                     colIndex++;
                  }
                  rowIndex += 3;
               }
               //刪除空白列 template 576行後表格不同
               if (rowIndex < 576)
                  ws.Range[$"{rowIndex}:575"].Delete();
            }//if (drsTmp.Count > 0)
            else {
               ws.Range["1:575"].Delete();
               rowIndex = 1;
            }

            //比例 template 576行後表格不同
            drsTmp = dtFut.Select("prod_type='F' and amt_type = 'P'").ToList();
            if (drsTmp.Count > 0) {
               foreach (DataRow dr in drsTmp) {
                  int colIndex = 1;

                  ws.Cells[rowIndex, colIndex].SetValue(dr["kind_id_out"].AsString());
                  rowIndex += 1;

                  if (dr["m_level_name"].AsString() == "從其高")
                     ws.Cells[rowIndex, 2].SetValue("");
                  else
                     ws.Cells[rowIndex, 2].SetValue(dr["m_level_name"]);

                  if (dr["cur_level_name"].AsString() == "從其高")
                     ws.Cells[rowIndex, 5].SetValue("");
                  else
                     ws.Cells[rowIndex, 5].SetValue(dr["cur_level_name"]);

                  rowIndex += 2;
                  colIndex = 2;
                  foreach (string dc in ColsA) {

                     ws.Cells[rowIndex, colIndex].SetValue(dr[dc].AsDecimal());
                     colIndex++;
                  }
                  rowIndex += 3;
               }
            }//if (drsTmp.Count > 0)
            else {
               ws.Range["1:882"].Delete();
            }

            ws.ScrollTo(0, 0);
         }

         protected virtual void ExcelOpt(Worksheet ws, DataTable dtOpt) {

            int rowIndex = 0;
            //幣別
            List<DataRow> drsTmp = dtOpt.Select("prod_type='O' and amt_type = 'F'").ToList();
            if (drsTmp.Count > 0) {
               foreach (DataRow dr in drsTmp) {
                  int colIndex = 1;

                  ws.Cells[rowIndex, colIndex].SetValue("單位：" + dr["currency_name"].AsString());
                  rowIndex += 1;
                  ws.Cells[rowIndex, colIndex].SetValue(dr["kind_id_out"].AsString());
                  rowIndex += 2;
                  colIndex = 2;

                  foreach (string dc in ColsA) {

                     ws.Cells[rowIndex, colIndex].SetValue(dr[dc].AsDecimal());
                     colIndex++;
                  }

                  rowIndex += 1;
                  colIndex = 2;
                  foreach (string dc in ColsB) {

                     ws.Cells[rowIndex, colIndex].SetValue(dr[dc].AsDecimal());
                     colIndex++;
                  }
                  rowIndex += 3;
               }
               //刪除空白列 template 211行後表格不同
               if (rowIndex < 209)
                  ws.Range[$"{rowIndex}:209"].Delete();
            } else {
               //用刪除的方式會造成template 跑掉, 所以改用隱藏
               ws.Rows.Hide(0, 209);
               rowIndex = 211;//從 row 212 開始填值
            }

            //比例
            drsTmp = dtOpt.Select("prod_type='O' and amt_type = 'P'").ToList();
            if (drsTmp.Count > 0) {
               foreach (DataRow dr in drsTmp) {
                  int colIndex = 1;

                  ws.Cells[rowIndex, colIndex].SetValue(dr["kind_id_out"].AsString());
                  rowIndex += 1;

                  if (dr["m_level_name"].AsString() == "從其高")
                     ws.Cells[rowIndex, 2].SetValue("");
                  else
                     ws.Cells[rowIndex, 2].SetValue(dr["m_level_name"]);

                  if (dr["cur_level_name"].AsString() == "從其高")
                     ws.Cells[rowIndex, 5].SetValue("");
                  else
                     ws.Cells[rowIndex, 5].SetValue(dr["cur_level_name"]);

                  rowIndex += 2;
                  colIndex = 2;
                  foreach (string dc in ColsA) {

                     ws.Cells[rowIndex, colIndex].SetValue(dr[dc].AsDecimal());
                     colIndex++;
                  }

                  rowIndex += 1;
                  colIndex = 2;
                  foreach (string dc in ColsB) {

                     ws.Cells[rowIndex, colIndex].SetValue(dr[dc].AsDecimal());
                     colIndex++;
                  }

                  rowIndex += 3;
               }
            } else {
               ws.Range["1:440"].Delete();
            }
            ws.ScrollTo(0, 0);
         }

         protected virtual void WordFut(Document rtfDoc, DataTable dt) {
            //dt = dt.Sort("prod_type A, amt_type A");

            foreach (DataRow dr in dt.Rows) {
               rtfDoc.AppendText(Environment.NewLine);
               string amtType = dr["amt_type"].AsString();
               string prodsubType = dr["prod_subtype"].AsString();

               Table futTable = CreateTable(rtfDoc, 4, 7);
               #region 幣別 / 比例
               string currency = amtType == "P" ? "單位：比例(%)" : $"單位：{dr["currency_name"].AsString()}";

               TableCell WordTableCell = futTable[0, 6];
               rtfDoc.InsertSingleLineText(WordTableCell.Range.Start, currency);
               ParagraphProperties ParagraphProps = rtfDoc.BeginUpdateParagraphs(WordTableCell.Range);
               ParagraphProps.Alignment = ParagraphAlignment.Right;
               rtfDoc.EndUpdateParagraphs(ParagraphProps);
               futTable.MergeCells(futTable[0, 0], WordTableCell);

               WordTableCell = futTable[0, 0];
               WordTableCell.CellSetBorders(TableBorderLineStyle.None, TableBorderLineStyle.None, TableBorderLineStyle.None);
               #endregion

               #region 表頭
               string kindout = prodsubType == "S" ? $"{dr["kind_id_out"].AsString()}({dr["kind_abbr_name"].AsString()})" : dr["kind_id_out"].AsString();
               WordTableCell = futTable[1, 0];
               rtfDoc.InsertSingleLineText(WordTableCell.Range.Start, kindout);
               WordTableCell.PreferredWidthType = WidthType.Fixed;
               WordTableCell.PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(2.65f);
               WordTableCell.VerticalAlignment = TableCellVerticalAlignment.Center;
               futTable.MergeCells(WordTableCell, futTable[2, 0]);

               if (dr["prod_type"].AsString() == "O") {
                  futTable.Rows.Append();

                  WordTableCell = futTable[3, 0];
                  rtfDoc.InsertSingleLineText(WordTableCell.Range.Start, "風險保證金（A值）");
                  WordTableCell = futTable[4, 0];
                  rtfDoc.InsertSingleLineText(WordTableCell.Range.Start, "風險保證金最低值（B值）");
               } else {
                  WordTableCell = futTable[3, 0];
                  rtfDoc.InsertSingleLineText(WordTableCell.Range.Start, "保證金");
               }

               string AfterAdjust = amtType == "F" ? "調整後保證金金額" : "調整後保證金適用比例";
               WordTableCell = futTable[1, 1];
               rtfDoc.InsertSingleLineText(WordTableCell.Range.Start, AfterAdjust);
               futTable.MergeCells(WordTableCell, futTable[1, 3]);

               string BeforeAdjust = amtType == "F" ? "調整前保證金金額" : "調整前保證金適用比例";
               WordTableCell = futTable[1, 2];
               rtfDoc.InsertSingleLineText(WordTableCell.Range.Start, BeforeAdjust);
               futTable.MergeCells(WordTableCell, futTable[1, 4]);

               string[] colName = new string[] { "原始保證金金額", "維持保證金金額", "結算保證金金額" };
               string[] colNameOpt = new string[] { "計算賣出選擇權原始保證金之適用比例", "計算賣出選擇權維持保證金之適用比例", "計算賣出選擇權結算保證金之適用比例" };

               colName = dr["prod_type"].AsString() == "O" ? colNameOpt : colName;

               int k = 1;
               foreach (string c in colName) {
                  WordTableCell = futTable[2, k];
                  rtfDoc.InsertSingleLineText(WordTableCell.Range.Start, c);
                  k++;
               }

               foreach (string c in colName) {
                  WordTableCell = futTable[2, k];
                  rtfDoc.InsertSingleLineText(WordTableCell.Range.Start, c);
                  k++;
               }
               #endregion

               #region 填值
               foreach (string c in ColsA) {
                  string stringVal = amtType == "P" ? dr[c].AsPercent(2) : dr[c].AsString();

                  WordTableCell = futTable[3, Array.IndexOf(ColsA, c) + 1];
                  rtfDoc.InsertSingleLineText(WordTableCell.Range.Start, stringVal);
               }

               if (dr["prod_type"].AsString() == "O") {
                  foreach (string c in ColsB) {
                     string stringVal = amtType == "P" ? dr[c].AsPercent(2) : dr[c].AsString();

                     WordTableCell = futTable[4, Array.IndexOf(ColsB, c) + 1];
                     rtfDoc.InsertSingleLineText(WordTableCell.Range.Start, stringVal);
                  }
               }
               #endregion
            }
         }

         protected virtual void WordSpan(DataTable dtSpan) {

            RichEditDocumentServer rtf = new RichEditDocumentServer();
            rtf.CreateNewDocument();
            Document rtfDoc = rtf.Document;

            Table spanTable = CreateTable(rtfDoc, 1, 5);

            TableCell WordTableCell;
            string[] tableTitle = new[] { "參數名稱", "	適用商品組合", "調整後", "調整前", "變動幅度" };
            tableTitle.ToList().ForEach(t => {
               WordTableCell = spanTable[0, Array.IndexOf(tableTitle, t)];
               rtfDoc.InsertSingleLineText(WordTableCell.Range.Start, t);
            });

            foreach (DataRow drSpan in dtSpan.Rows) {

               TableRow tableRow = spanTable.Rows.Append();
               foreach (DataColumn dcSpan in dtSpan.Columns) {
                  WordTableCell = spanTable[tableRow.Index, dtSpan.Columns.IndexOf(dcSpan)];
                  rtfDoc.InsertSingleLineText(WordTableCell.Range.Start, drSpan[dcSpan].AsString());
               }
            }

            string wordFilePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, $"40090_SPAN_{DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss")}.rtf");
            rtf.SaveDocument(wordFilePath, DevExpress.XtraRichEdit.DocumentFormat.Rtf);
            rtf.Dispose();
         }

         public virtual void ErrorHandle(Exception ex, ReturnMessageClass msg) {
            WriteLog(ex.ToString(), "Info", "Z");
            msg.Status = ResultStatus.Fail;
            msg.ReturnMessage = MessageDisplay.MSG_IMPORT_FAIL;
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

         protected virtual Table CreateTable(Document rtfDoc, int rowcount, int colcount) {

            Table wordtable = rtfDoc.Tables.Create(rtfDoc.Range.End, rowcount, colcount);
            wordtable.TableAlignment = TableRowAlignment.Right;
            wordtable.PreferredWidthType = WidthType.Fixed;
            wordtable.PreferredWidth = DevExpress.Office.Utils.Units.CentimetersToDocumentsF(16.7f);
            ParagraphProperties ParagraphProps = rtfDoc.BeginUpdateParagraphs(wordtable.Range);

            // 預設Table內容都全部置中
            ParagraphProps = rtfDoc.BeginUpdateParagraphs(wordtable.Range);
            ParagraphProps.Alignment = ParagraphAlignment.Center;
            ParagraphProps.LineSpacingType = ParagraphLineSpacing.Single;
            ParagraphProps.SpacingBefore = 0;
            rtfDoc.EndUpdateParagraphs(ParagraphProps);

            // 預設Table內的字體
            CharacterProperties tableProp = rtfDoc.BeginUpdateCharacters(wordtable.Range);
            tableProp.FontName = "標楷體";
            tableProp.FontSize = 12;
            rtfDoc.EndUpdateCharacters(tableProp);

            // 垂直置中
            wordtable.ForEachCell((c, i, j) => {
               c.VerticalAlignment = TableCellVerticalAlignment.Center;
            });

            return wordtable;
         }
      }

      /// <summary>
      /// 處置股票 輸出xml
      /// </summary>
      private class ExportXml2B : ExportXml {
         public ExportXml2B(D40xxx dao, string txtdate, string adjtype, string programId) :
                              base(dao, txtdate, adjtype, programId) {

            AddDescElement = true;
            DescTxt = "本公司#full_name_llist#經#underlying_market#於#pub_ymd#公布為處置有價證券，" +
                     "處置期間為#impl_begin_ymd#至#impl_end_ymd#，依前揭規定辦理保證金調整，列表如附。" +
                     "本次調整自#issue_begin_ymd#(證券市場處置生效日次一營業日)該股票期貨契約交易時段結束後起實施，" +
                     "並於#issue_end_ymd#該契約交易時段結束後恢復為#issue_begin_ymd#調整前之保證金，" +
                     "參照證券市場處置措施，調整期間如遇休市、有價證券停止買賣、全日暫停交易，則恢復日順延執行。";
         }

         public override ReturnMessageClass Export() {
            ReturnMessageClass msg = new ReturnMessageClass();
            msg.Status = ResultStatus.Fail;

            try {
               #region ifd
               FilePath = PbFunc.wf_copy_file(ProgramId, $"{ProgramId}_{AdjType}");

               OpenFileAndSetYear();

               string prepoStr = Dt.AsEnumerable().Any(d => d.Field<string>("prod_type") == "F") ? "期貨契約保證金及" : "";
               List<string> subTypeString = new List<string>();

               //說明文
               foreach (DataRow dr in Dt.Rows) {
                  string abbrName = $"{dr["KIND_ABBR_NAME"].AsString()}({dr["kind_id"].AsString()})";

                  string fullName = "";
                  if (dr["prod_type"].AsString() == "O" && dr == Dt.Rows[Dt.Rows.Count - 1]) {
                     int lipos = PbFunc.Pos(abbrName, "選擇權");
                     fullName = $"{abbrName}之標的證券-{abbrName.SubStr(0, lipos)}({dr["stock_id"].AsString()})"; ;
                     abbrName += $"之{prepoStr}選擇權契約風險保證金(A值)、風險保證金最低值(B值)";

                  } else {
                     int lipos = PbFunc.Pos(abbrName, "期貨");
                     fullName = $"{abbrName}之標的證券-{abbrName.SubStr(0, lipos)}({dr["stock_id"].AsString()})"; ;
                  }

                  MakeDescElement(dr, fullName);

                  if (!KindNameList.Exists(k => k == abbrName))
                     KindNameList.Add(abbrName);
               }

               ReplaceElementWord(GenArrayTxt(KindNameList));

               Doc.Save(FilePath);
               #endregion

               #region Excel rtf
               //保證金
               FilePath = PbFunc.wf_copy_file(ProgramId, ProgramId);
               Workbook workbook = new Workbook();
               workbook.LoadDocument(FilePath);
               Worksheet ws = workbook.Worksheets["保證金(Fut)"];
               ExcelFut(ws, Dt);

               ws = workbook.Worksheets["保證金(Opt)"];
               ExcelOpt(ws, Dt);

               RichEditDocumentServer rtf = new RichEditDocumentServer();
               rtf.CreateNewDocument();
               Document rtfDoc = rtf.Document;

               WordFut(rtfDoc, Dt);

               string wordFilePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, $"40090_{DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss")}.rtf");
               rtf.SaveDocument(wordFilePath, DevExpress.XtraRichEdit.DocumentFormat.Rtf);
               rtf.Dispose();


               //Span 資料
               DataTable dtSpan = Dao.GetSpanData(TxtDate.AsDateTime("yyyyMMdd"));
               if (dtSpan.Rows.Count > 0) {
                  ws = workbook.Worksheets["SPAN參數"];
                  ExcelSpan(ws, dtSpan);

                  WordSpan(dtSpan);
               }
               workbook.SaveDocument(FilePath);
               #endregion

               msg.Status = ResultStatus.Success;
            } catch (Exception ex) {
               ErrorHandle(ex, msg);
            }
            return msg;
         }
         protected virtual void MakeDescElement(DataRow dr, string abbrName) {
            DateTime beginYmd = dr["issue_begin_ymd"].AsDateTime("yyyyMMdd");
            string issueBeginYmd = beginYmd.AsTaiwanDateTime("{0}年{1}月{2}日", 3);

            DateTime endYmd = dr["issue_end_ymd"].AsDateTime("yyyyMMdd");
            string issueEndYmd = endYmd.AsTaiwanDateTime("{0}年{1}月{2}日", 3);

            DateTime pubYmd = dr["pub_ymd"].AsDateTime("yyyyMMdd");
            string issuePubYmd = pubYmd.AsTaiwanDateTime("{0}年{1}月{2}日", 3);

            string underlying = dr["underlying_market"].AsInt() == 1 ? "證交所" : "櫃買中心";

            string implBeginDate = dr["impl_begin_ymd"].AsString();
            implBeginDate = implBeginDate.AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);

            string implEndDate = dr["impl_end_ymd"].AsString();
            implEndDate = implEndDate.AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);

            XmlElement element = Doc.CreateElement("文字");
            element.InnerText = DescTxt;
            ReplaceXmlInnterText(element, "#full_name_llist#", abbrName);
            ReplaceXmlInnterText(element, "#pub_ymd#", issuePubYmd);
            ReplaceXmlInnterText(element, "#underlying_market#", underlying);
            ReplaceXmlInnterText(element, "#issue_begin_ymd#", issueBeginYmd);
            ReplaceXmlInnterText(element, "#issue_end_ymd#", issueEndYmd);
            ReplaceXmlInnterText(element, "#impl_begin_ymd#", implBeginDate);
            ReplaceXmlInnterText(element, "#impl_end_ymd#", implEndDate);


            XmlElement element_Tmp = Doc.CreateElement("條列");
            element_Tmp.AppendChild(element);

            Doc.GetElementsByTagName("段落")[0].ChildNodes[0].AppendChild(element_Tmp);
         }

         protected override void ReplaceElementWord(params string[] args) {

            ReplaceXmlInnterText(Doc.GetElementsByTagName("主旨")[0].ChildNodes[0], "#kind_name_list#", args[0]);
         }
      }

      /// <summary>
      /// 長假調整 輸出xml
      /// </summary>
      private class ExportXml1B : ExportXml {
         public ExportXml1B(D40xxx dao, string txtdate, string adjtype, string programId) :
                     base(dao, txtdate, adjtype, programId) { }

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
      /// 長假回調 輸出xml
      /// </summary>
      private class ExportXml1E : ExportXml {
         public ExportXml1E(D40xxx dao, string txtdate, string adjtype, string programId) :
            base(dao, txtdate, adjtype, programId) { }

         public override ReturnMessageClass Export() {
            ReturnMessageClass msg = new ReturnMessageClass();
            msg.Status = ResultStatus.Fail;
            try {
               #region ifd
               FilePath = PbFunc.wf_copy_file(ProgramId, $"{ProgramId}_{AdjType}");

               OpenFileAndSetYear();

               string prepoStr = Dt.AsEnumerable().Any(d => d.Field<string>("prod_type") == "F") ? "期貨契約保證金及" : "";
               List<string> subTypeString = new List<string>();
               List<string> adjRate = new List<string>();
               //說明文
               foreach (DataRow dr in Dt.Rows) {
                  string abbrName = dr["KIND_ABBR_NAME"].AsString();
                  string fullName = $"本公司{dr["rule_full_name"].AsString()}交易規則";

                  GenKindNameList(dr, prepoStr, abbrName, fullName);

                  string subTypeName = GetProdSubTypeDesc(dr["prod_subtype"].AsString());
                  if (!subTypeString.Exists(s => s == subTypeName)) {
                     subTypeString.Add(subTypeName);

                     //rfa 檔會用到的參數
                     adjRate.Add(dr["adj_rate"].AsPercent(2));
                  }
               }

               string implBeginDate = Dt.Rows[0]["impl_begin_ymd"].AsString();
               string implEndDate = Dt.Rows[0]["impl_end_ymd"].AsString();

               implBeginDate = implBeginDate.AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);
               implEndDate = implEndDate.AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);

               ReplaceElementWord(GenArrayTxt(KindNameList), GenArrayTxt(KindNameList_Desc), GenArrayTxt(subTypeString), implEndDate);

               Doc.Save(FilePath);
               #endregion

               #region ifd 簽核 ifd_rfa
               FilePath = PbFunc.wf_copy_file(ProgramId, $"{ProgramId}_{AdjType}_rfa");
               OpenFileAndSetYear();
               string issueBeginDate = Dt.Rows[0]["issue_begin_ymd"].AsString();
               issueBeginDate = issueBeginDate.AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);

               ReplaceElementWordRfa(GenArrayTxt(subTypeString), GenArrayTxt(adjRate), implBeginDate, implEndDate, issueBeginDate);
               Doc.Save(FilePath);
               #endregion

               #region Excel rtf
               //保證金
               FilePath = PbFunc.wf_copy_file(ProgramId, ProgramId);
               Workbook workbook = new Workbook();
               workbook.LoadDocument(FilePath);
               Worksheet ws = workbook.Worksheets["保證金(Fut)"];
               ExcelFut(ws, Dt);

               ws = workbook.Worksheets["保證金(Opt)"];
               ExcelOpt(ws, Dt);

               RichEditDocumentServer rtf = new RichEditDocumentServer();
               rtf.CreateNewDocument();
               Document rtfDoc = rtf.Document;

               WordFut(rtfDoc, Dt);

               string wordFilePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, $"40090_{DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss")}.rtf");
               rtf.SaveDocument(wordFilePath, DevExpress.XtraRichEdit.DocumentFormat.Rtf);
               rtf.Dispose();


               //Span 資料
               DataTable dtSpan = Dao.GetSpanData(TxtDate.AsDateTime("yyyyMMdd"));
               if (dtSpan.Rows.Count > 0) {
                  ws = workbook.Worksheets["SPAN參數"];
                  ExcelSpan(ws, dtSpan);

                  WordSpan(dtSpan);
               }
               workbook.SaveDocument(FilePath);
               #endregion

               msg.Status = ResultStatus.Success;
               return msg;
            } catch (Exception ex) {
               base.ErrorHandle(ex, msg);
               return msg;
            }
         }

         protected override void ReplaceElementWord(params string[] args) {

            ReplaceXmlInnterText(Doc.GetElementsByTagName("主旨")[0].ChildNodes[0], "#kind_name_list#", args[0]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[0].ChildNodes[0], "#full_name_llist#", args[1]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[1].ChildNodes[0], "#prod_subtype_list#", args[2]);
            //這個點很奇怪, template 寫beginDate 但是實際帶EndDate
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[1].ChildNodes[0], "#issue_begin_ymd#", args[3]);
         }

         protected override void ReplaceElementWordRfa(params string[] args) {

            ReplaceXmlInnterText(Doc.GetElementsByTagName("主旨")[0].ChildNodes[0], "#prod_subtype_list#", args[0]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[1].ChildNodes[0], "#year#", DateTime.Now.AsTaiwanDateTime("{0}", 3));
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[1].ChildNodes[0], "#prod_subtype_list#", args[0]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[1].ChildNodes[0], "#adj_rate#", args[1]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[1].ChildNodes[0], "#impl_begin_ymd#", args[2]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[1].ChildNodes[0], "#impl_end_ymd#", args[3]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[2].ChildNodes[0], "#prod_subtype_list#", args[0]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[2].ChildNodes[0], "#issue_begin_ymd#", args[4]);
         }
      }

      /// <summary>
      /// 一般 / 股票 輸出xml
      /// </summary>
      private class ExportXml0B : ExportXml {

         public ExportXml0B(D40xxx dao, string txtdate, string adjtype, string programId) :
                     base(dao, txtdate, adjtype, programId) { }

         public override ReturnMessageClass GetData() {
            ReturnMessageClass msg = new ReturnMessageClass(MessageDisplay.MSG_NO_DATA);

            Dt = Dao.GetData(TxtDate, AsAdjType, AdjType.SubStr(1, 1));

            //一般 / 股票 要多撈一次資料
            if (AdjType == "0B") {
               DataTable dtTmp = Dao.GetData(TxtDate, "3", AdjType.SubStr(1, 1));
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
               FilePath = PbFunc.wf_copy_file(ProgramId, $"{ProgramId}_{AdjType}");

               base.OpenFileAndSetYear();

               string prepoStr = Dt.AsEnumerable().Any(d => d.Field<string>("prod_type") == "F") ? "期貨契約保證金及" : "";
               foreach (DataRow dr in Dt.Rows) {
                  string abbrName = dr["KIND_ABBR_NAME"].AsString();
                  string abbrName_Desc = dr["rule_full_name"].AsString() + "交易規則";

                  base.GenKindNameList(dr, prepoStr, abbrName, abbrName_Desc);
               }

               string beginDate = Dt.Rows[0]["issue_begin_ymd"].AsDateTime("yyyyMMdd").AsTaiwanDateTime("{0}年{1}月{2}日", 3);
               ReplaceElementWord(GenArrayTxt(KindNameList), beginDate, GenArrayTxt(KindNameList_Desc), GenArrayTxt(KindNameList));

               Doc.Save(FilePath);
               msg.Status = ResultStatus.Success;
               return msg;
            } catch (Exception ex) {
               base.ErrorHandle(ex, msg);
               return msg;
            }
         }

         protected override void ReplaceElementWord(params string[] args) {

            ReplaceXmlInnterText(Doc.GetElementsByTagName("主旨")[0].ChildNodes[0], "#kind_name_list#", args[0]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("主旨")[0].ChildNodes[0], "#issue_begin_ymd#", args[1]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[0].ChildNodes[0], "#full_name_llist#", args[2]);
            ReplaceXmlInnterText(Doc.GetElementsByTagName("段落")[0].ChildNodes[1].ChildNodes[0], "#kind_name_list#", args[3]);

            if (Dt.Select("amt_type = 'F'").Count() > 0) {
               ReplaceXmlInnterText(Doc.GetElementsByTagName("主旨")[0].ChildNodes[0], "#amt_type#", "金額");
            } else {
               ReplaceXmlInnterText(Doc.GetElementsByTagName("主旨")[0].ChildNodes[0], "#amt_type#", "適用比例");
            }
         }
      }
   }
}