using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.IO;

/// <summary>
/// Winni, 2019/3/13
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
   /// <summary>
   /// 黃金類契約保證金調整補充說明
   /// </summary>
   public partial class W40140 : FormParent {

      public W40140(string programID , string programName) : base(programID , programName) {
         InitializeComponent();

         this.Text = _ProgramID + "─" + _ProgramName;
         txtStartDate.DateTimeValue = GlobalInfo.OCF_DATE;

#if DEBUG
         //ken test
         txtStartDate.DateTimeValue = DateTime.ParseExact("2015/04/22" , "yyyy/MM/dd" , null);
         this.Text += "(開啟測試模式),Date=2015/04/22";
#endif

      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      /// <summary>
      /// Export return 1 txt & 1 excel
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus Export() {
         try {

            //1. 寫資料到文字檔
            bool isText = false;
            isText = GetText();
            if (!isText) {
               return ResultStatus.Fail;
            }

            //2. 寫資料到excel
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "訊息：40140_2-保證金比較轉檔中...";
            this.Refresh();

            //2.1 複製、開啟檔案
            Workbook workbook = new Workbook();
            string excelDestinationPath = PbFunc.wf_copy_file(_ProgramID , _ProgramID);
            workbook.LoadDocument(excelDestinationPath);
            Worksheet worksheet = workbook.Worksheets[0];

            DataTable dt = new D40140().ListMoneyData(txtStartDate.DateTimeValue);
            if (dt.Rows.Count <= 0 ) {
               MessageDisplay.Info(String.Format("{0},讀取「國外保證金資料」無任何資料!",txtStartDate.Text));
            }//if (dt.Rows.Count <= 0 )

            foreach (DataRow dr in dt.Rows) {
               int rptSeqNo = dr["rpt_seq_no"].AsInt();
               string com = dr["com"].AsString();
               Decimal mg1Cm = dr["mg1_cm"].AsDecimal();
               Decimal mg1Mm = dr["mg1_mm"].AsDecimal();
               Decimal mg1Im = dr["mg1_im"].AsDecimal();
               Decimal exchangeRate = dr["exchange_rate"].AsDecimal();
               Decimal mg1Price = dr["mg1_price"].AsDecimal();
               Decimal mg1Xxx = dr["mg1_xxx"].AsDecimal();

               rptSeqNo += 2;
               worksheet.Cells[3 , rptSeqNo - 1].Value = mg1Cm;
               worksheet.Cells[4 , rptSeqNo - 1].Value = mg1Mm;
               worksheet.Cells[5 , rptSeqNo - 1].Value = mg1Im;

               if (com == "TOC01") {
                  if (exchangeRate != 0) {
                     worksheet.Cells[6 , rptSeqNo - 1].Value = mg1Im / exchangeRate * 3.11m;
                  }//if (exchangeRate != 0)
                  worksheet.Cells[7 , rptSeqNo - 1].Value = mg1Price * mg1Xxx;
               } else {
                  worksheet.Cells[7 , rptSeqNo - 1].Value = mg1Price * mg1Xxx;
               }//if (com == "TOC01")

            }//foreach (DataRow dr in dt.Rows)

            //2.3 關閉、儲存檔案
            workbook.SaveDocument(excelDestinationPath);
            labMsg.Visible = false;

            if (FlagAdmin)
               System.Diagnostics.Process.Start(excelDestinationPath);

            return ResultStatus.Success;

         } catch (Exception ex) {
            WriteLog(ex);
         } finally {
            panFilter.Enabled = true;
            labMsg.Text = "";
            labMsg.Visible = false;
         }
         return ResultStatus.Fail;
      }

      /// <summary>
      /// 寫資料到文字檔
      /// </summary>
      /// <returns></returns>
      protected bool GetText() {
         try {
            //1.1.ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "訊息：40141-文字說明轉檔中...";
            this.Refresh();

            //1.2 get txt data
            string txt = "";
            int pos = 0;
            string strIm = "";

            //dtGold 黃金期貨
            DataTable dtGold = new D40140().GetGoldData(txtStartDate.DateTimeValue);
            if (dtGold.Rows.Count <= 0) {
               MessageDisplay.Info(String.Format("{0},無任何「調整黃金期貨契約」資料!" , txtStartDate.Text));
               return false;
            }
            int dataGoldNum = dtGold.Rows.Count;

            //dt 國外保證金
            DataTable dt = new D40140().ListMoneyData(txtStartDate.DateTimeValue);
            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info(String.Format("{0},讀取「國外保證金資料」無任何資料!" , txtStartDate.Text));
               return false;
            }

            foreach (DataRow dr in dt.Rows) {
               pos++;
               string com = dr["com"].AsString();
               Decimal outIm = dr["out_im"].AsDecimal();
               Decimal exchangeRate = dr["exchange_rate"].AsDecimal();
               Decimal mg1Im = dr["mg1_im"].AsDecimal(); 
               dr.BeginEdit();
               if (com == "TOC01" && exchangeRate > 0) {
                  dr["out_im"] = Math.Round((mg1Im / exchangeRate * 3.11m) , 0);
               } else {
                  dr["out_im"] = mg1Im;
               }//if (com == "TOC01" && exchangeRate > 0)
               dr.BeginEdit();

            } //foreach (DataRow dr in dtGold.Rows)
            dt.AcceptChanges(); //這樣才會更新修改的dt資料

            txt += "二、保證金調整之合理性" + Environment.NewLine + "依結算保證金訂定公式計算，" +
                                          PbFunc.f_conv_date(txtStartDate.DateTimeValue , 3);

            pos = 0;
            foreach (DataRow drGold in dtGold.Rows) {
               pos++;
               string abbrName = drGold["mgt2_abbr_name"].AsString();
               Decimal mg1ChangeRate = drGold["mg1_change_range"].AsDecimal();

               if (pos > 1) {
                  txt += "；";
               }
               txt += abbrName + "結算保證金之變動幅度為" + Math.Round(mg1ChangeRate * 100 , 1).AsString() + "%";
            }//foreach (DataRow dr in dtGold.Rows)

            txt += "，其變動幅度達百分之十以上且進位後金額改變，依上開規定，經保證金調整審議會議決議後調整保證金。" + Environment.NewLine;

            //1.3 GDF
            DataTable dtGDF = dtGold.Filter("mg1_kind_id='GDF'");
            strIm = dtGold.Rows[0]["mg1_im"].AsDecimal().AsString();
            if (dtGDF.Rows.Count > 0) {
               Decimal mg1Im = dtGDF.Rows[0]["mg1_im"].AsDecimal();
               Decimal mg1CurCm = dtGDF.Rows[0]["mg1_cur_cm"].AsDecimal();
               Decimal mg1Cm = dtGDF.Rows[0]["mg1_cm"].AsDecimal();
               string mgt2AbbrName = dtGDF.Rows[0]["mgt2_abbr_name"].AsString();

               txt += "本次調整將美元計價" + mgt2AbbrName + "結算保證金金額由原先" +
                        String.Format("{0:N0}" , Math.Round(mg1CurCm , 0)) + "美元向";

               if (mg1CurCm > mg1Cm) {
                  txt += "下";
               } else {
                  txt += "上";
               } //if (mg1CurCm > mg1Cm)

               txt += "調整為" + String.Format("{0:N0}" , mg1Cm) + "美元，比例約為";

               if (mg1CurCm > 0) {
                  txt += (Math.Round((mg1Cm - mg1CurCm) / mg1CurCm * 100 , 1)).AsString() + "%";
               } //if (mg1CurCm > 0)
            } //if (dtGold.Rows.Count > 0)

            //1.4 TGF
            DataTable dtTGF = dtGold.Filter("mg1_kind_id='TGF'");
            if (dtTGF.Rows.Count > 0) {
               Decimal mg1CurCm = dtTGF.Rows[0]["mg1_cur_cm"].AsDecimal();
               Decimal mg1Cm = dtTGF.Rows[0]["mg1_cm"].AsDecimal();
               string mgt2AbbrName = dtTGF.Rows[0]["mgt2_abbr_name"].AsString();

               if (dataGoldNum > 0) {
                  txt += "；";
               }

               txt += "新臺幣計價" + mgt2AbbrName + "結算保證金金額由原先" +
                        String.Format("{0:N0}" , Math.Round(mg1CurCm , 0)) + "元向";

               if (mg1CurCm > mg1Cm) {
                  txt += "下";
               } else {
                  txt += "上";
               } //if (mg1CurCm > mg1Cm)

               txt += "調整為" + String.Format("{0:N0}" , mg1Cm) + "元，比例約為";

               if (mg1CurCm > 0) {
                  txt += (Math.Round((mg1Cm - mg1CurCm) / mg1CurCm * 100 , 1)).AsString() + "%";
               } //if (mg1CurCm > 0)

            }//if (dtGold.Rows.Count > 0)

            txt += "。茲將調整後本公司黃金期貨契約結算保證金金額，相較於世界主要交易所黃金期貨保證金金額分析如下：(詳如后附表)" + Environment.NewLine;

            //1.5
            txt += "1.本公司黃金期貨契約保證金";
            DataTable dtSmallstrIm = dt.Filter("out_im <" + strIm + " and data_type='2'");
            if (dtSmallstrIm.Rows.Count > 0) {
               txt += "較";
               pos = 0;

               foreach (DataRow dr in dtSmallstrIm.Rows) {
                  pos++;
                  string fName = dr["f_name"].AsString();
                  txt += fName;

                  if (pos == dtSmallstrIm.Rows.Count - 1) {
                     txt += "及";
                  } else if (pos == dtSmallstrIm.Rows.Count) {
                     //
                  } else {
                     txt += "、";
                  }//if (pos == dt.Rows.Count - 1)

               }//foreach (DataRow dr in dt.Rows)

               txt += "略高，";

            }//if (dt.Rows.Count > 0)

            DataTable dtBigstrIm = dt.Filter("out_im >" + strIm + " and data_type='2'");
            if (dtBigstrIm.Rows.Count > 0) {
               txt += "較";
               pos = 0;

               foreach (DataRow dr in dtBigstrIm.Rows) {
                  pos++;
                  string fName = dr["f_name"].AsString();
                  txt += fName;

                  if (pos == dtBigstrIm.Rows.Count - 1) {
                     txt += "及";
                  } else if (pos == dtBigstrIm.Rows.Count) {
                     //
                  } else {
                     txt += "、";
                  }//if (pos == dt.Rows.Count - 1)

               }//foreach (DataRow dr in dt.Rows)

               txt += "略低，";

            }//if (dt.Rows.Count > 0)

            txt += "應和各交易所風控機制與交易時間等因素相關" + Environment.NewLine;

            DataTable dtNYM01 = dt.Filter("com='NYM01'");
            if (dtNYM01.Rows.Count > 0) {
               string fName = dtNYM01.Rows[0]["f_name"].AsString();
               Decimal mg1Cm = dtNYM01.Rows[0]["mg1_cm"].AsDecimal();
               Decimal mg1Im = dtNYM01.Rows[0]["mg1_im"].AsDecimal();

               txt += "經查目前" + fName + "黃金期貨契約(契約規格100盎司)結算保證金為" + String.Format("{0:N0}" , Math.Round(mg1Cm , 0)) +
                  "美元，原始保證金為" + String.Format("{0:N0}" , Math.Round(mg1Im , 0)) + "美元；";
            } //if (dt.Rows.Count > 0)

            DataTable dtCME01 = dt.Filter("com='CME01'");
            if (dtCME01.Rows.Count > 0) {
               string fName = dtCME01.Rows[0]["f_name"].AsString();
               Decimal mg1Cm = dtCME01.Rows[0]["mg1_cm"].AsDecimal();
               Decimal mg1Im = dtCME01.Rows[0]["mg1_im"].AsDecimal();

               txt += "而" + fName + "黃金期貨契約(契約規格100盎司)結算保證金為" + String.Format("{0:N0}" , Math.Round(mg1Cm , 0)) +
                  "美元，原始保證金為" + String.Format("{0:N0}" , Math.Round(mg1Im , 0)) + "美元；";
            } //if (dt.Rows.Count > 0)

            DataTable dtTOC01 = dt.Filter("com='TOC01'");
            if (dtTOC01.Rows.Count > 0) {
               string fName = dtTOC01.Rows[0]["f_name"].AsString();
               Decimal exchangeRate = dtTOC01.Rows[0]["exchange_rate"].AsDecimal();
               Decimal mg1Im = dtTOC01.Rows[0]["mg1_im"].AsDecimal();

               txt += "另" + fName + "所收取之原始保證金金額，倘調整契約規格並依匯率(原契約規格1公斤並以" +
                  String.Format("{0:N0}" , Math.Round(exchangeRate , 0)) + "日圓對1美元計算)及重量單位(100盎司=3.11公斤)換算後，約為";

               if (exchangeRate > 0) {
                  
                  txt += String.Format("{0:N0}" , Math.Round((mg1Im / exchangeRate * 3.11m) , 0)).AsString();
               }//if (exchangeRate > 0)

               txt += "美元。";

            } //if (dt.Rows.Count > 0)

            txt += "有關各交易所保證金占契約總值比率詳附表。" + Environment.NewLine;

            //2.save string to txt
            string fileName = "40141_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".txt";
            string filePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , fileName);

            bool IsSuccess = ToText(txt , filePath , System.Text.Encoding.GetEncoding(950));
            if (!IsSuccess) {
               MessageDisplay.Error("文字檔「" + filePath + "」Open檔案錯誤!");
               return false;
            }

            if (FlagAdmin)
               System.Diagnostics.Process.Start(filePath);

            return true;
         } catch (Exception ex) {
            WriteLog(ex);
            return false;
         }
      }

      /// <summary>
      /// write string to txt
      /// </summary>
      /// <param name="source"></param>
      /// <param name="filePath"></param>
      /// <param name="encoding">System.Text.Encoding.GetEncoding(950)</param>
      /// <returns></returns>
      protected bool ToText(string source , string filePath , System.Text.Encoding encoding) {
         try {
            FileStream fs = new FileStream(filePath , FileMode.Create);
            StreamWriter str = new StreamWriter(fs , encoding);
            str.Write(source);

            str.Flush();
            str.Close();
            return true;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return false;
      }
   }
}