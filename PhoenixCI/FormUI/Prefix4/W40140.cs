using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms;

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
         txtStartDate.DateTimeValue = DateTime.Now;

#if DEBUG
         txtStartDate.DateTimeValue = DateTime.ParseExact("2015/04/22" , "yyyy/MM/dd" , null);
         this.Text += "(開啟測試模式),Date=2015/04/22";
#endif
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      protected void ShowMsg(string msg) {
         labMsg.Text = msg;
         labMsg.Visible = true;
         this.Refresh();
         Thread.Sleep(5);
      }

      /// <summary>
      /// Export return 1 txt & 1 excel
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus Export() {
         try {
            //0. ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            ShowMsg("開始轉檔...");
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
            Thread.Sleep(5);

            //1. 寫資料到文字檔
            bool isText = false;
            isText = wf_40140_1();

            //2. 複製、開啟檔案(wf_40140_2)
            ShowMsg("40140_2-保證金比較 轉檔中...");

            Workbook workbook = new Workbook();
            string excelDestinationPath = PbFunc.wf_copy_file(_ProgramID , _ProgramID);
            workbook.LoadDocument(excelDestinationPath);
            Worksheet worksheet = workbook.Worksheets[0];

            //2.1 填資料
            DataTable dt = new D40140().ListMoneyData(txtStartDate.DateTimeValue);
            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},讀取「國外保證金資料」無任何資料!" , txtStartDate.Text) , GlobalInfo.ResultText);
               File.Delete(excelDestinationPath);
               return ResultStatus.Fail;
            }//if (dt.Rows.Count <= 0 )

            //wf_40140_2()
            foreach (DataRow dr in dt.Rows) {
               int rptSeqNo = dr["rpt_seq_no"].AsInt();
               string com = dr["com"].AsString();
               decimal mg1Cm = dr["mg1_cm"].AsDecimal();
               decimal mg1Mm = dr["mg1_mm"].AsDecimal();
               decimal mg1Im = dr["mg1_im"].AsDecimal();
               decimal exchangeRate = dr["exchange_rate"].AsDecimal();
               decimal mg1Price = dr["mg1_price"].AsDecimal();
               decimal mg1Xxx = dr["mg1_xxx"].AsDecimal();

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

            //3. 關閉、儲存檔案
            workbook.SaveDocument(excelDestinationPath);
            labMsg.Visible = false;

#if DEBUG
            if (FlagAdmin)
               System.Diagnostics.Process.Start(excelDestinationPath);
#endif
            return ResultStatus.Success;

         } catch (Exception ex) {
            WriteLog(ex);
         } finally {
            panFilter.Enabled = true;
            labMsg.Text = "";
            labMsg.Visible = false;
            this.Cursor = Cursors.Arrow;
         }
         return ResultStatus.Fail;
      }

      /// <summary>
      /// 寫資料到文字檔
      /// </summary>
      /// <returns></returns>
      protected bool wf_40140_1() {
         try {
            //1.1.ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "40141-文字說明 轉檔中...";
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
            Thread.Sleep(5);

            //1.2 get txt data
            string txt = "";
            int pos = 0;
            string strIm = "";

            //dtGold 黃金期貨
            DataTable dtGold = new D40140().GetGoldData(txtStartDate.DateTimeValue); //d_40140_1_mg1
            if (dtGold.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},無任何「調整黃金期貨契約」資料!" , txtStartDate.Text) , GlobalInfo.ResultText);
               return false;
            }

            int dataGoldNum = dtGold.Rows.Count;

            //dt 國外保證金
            DataTable dt = new D40140().ListMoneyData(txtStartDate.DateTimeValue);
            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},讀取「國外保證金資料」無任何資料!" , txtStartDate.Text) , GlobalInfo.ResultText);
               return false;
            }

            foreach (DataRow dr in dt.Rows) {
               pos++;
               string com = dr["com"].AsString();
               decimal outIm = dr["out_im"].AsDecimal();
               decimal exchangeRate = dr["exchange_rate"].AsDecimal();
               decimal mg1Im = dr["mg1_im"].AsDecimal();
               dr.BeginEdit();
               if (com == "TOC01" && exchangeRate > 0) {
                  dr["out_im"] = Math.Round((mg1Im / exchangeRate * 3.11m) , 0 , MidpointRounding.AwayFromZero);
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
               decimal mgd2ChangeRate = drGold["mgd2_adj_rate"].AsDecimal();

               if (pos > 1) {
                  txt += "；";
               }
               txt += abbrName + "結算保證金之變動幅度為" + Math.Round(mgd2ChangeRate * 100 , 1 , MidpointRounding.AwayFromZero).AsString() + "%";
            }//foreach (DataRow dr in dtGold.Rows)

            txt += "，其變動幅度達百分之十以上且進位後金額改變，依上開規定，經保證金調整審議會議決議後調整保證金。" + Environment.NewLine;

            //1.3 GDF
            if (dtGold.Rows.Count > 0) {
               DataTable dtGDF = dtGold.Filter("mgd2_kind_id='GDF'");
               strIm = dtGold.Rows[0]["mgd2_im"].AsDecimal().AsString();
               if (dtGDF.Rows.Count > 0) {
                  decimal mgd2Im = dtGDF.Rows[0]["mgd2_im"].AsDecimal();
                  decimal mgd2CurCm = dtGDF.Rows[0]["mgd2_cur_cm"].AsDecimal();
                  decimal mgd2Cm = dtGDF.Rows[0]["mgd2_cm"].AsDecimal();
                  string mgt2AbbrName = dtGDF.Rows[0]["mgt2_abbr_name"].AsString();

                  txt += "本次調整將美元計價" + mgt2AbbrName + "結算保證金金額由原先" +
                           string.Format("{0:N0}" , Math.Round(mgd2CurCm , 0 , MidpointRounding.AwayFromZero)) + "美元向";

                  if (mgd2CurCm > mgd2Cm) {
                     txt += "下";
                  } else {
                     txt += "上";
                  } //if (mg1CurCm > mg1Cm)

                  txt += "調整為" + string.Format("{0:N0}" , mgd2Cm) + "美元，比例約為";

                  if (mgd2CurCm > 0) {
                     txt += (Math.Round((mgd2Cm - mgd2CurCm) / mgd2CurCm * 100 , 1 , MidpointRounding.AwayFromZero)).AsString() + "%";
                  } //if (mg1CurCm > 0)
               } //if (dtGold.Rows.Count > 0)
            }

            //1.4 TGF
            if (dtGold.Rows.Count > 0) {
               DataTable dtTGF = dtGold.Filter("mgd2_kind_id='TGF'");
               if (dtTGF.Rows.Count > 0) {
                  decimal mgd2CurCm = dtTGF.Rows[0]["mgd2_cur_cm"].AsDecimal();
                  decimal mgd2Cm = dtTGF.Rows[0]["mgd2_cm"].AsDecimal();
                  string mgt2AbbrName = dtTGF.Rows[0]["mgt2_abbr_name"].AsString();

                  if (dataGoldNum > 0) {
                     txt += "；";
                  }

                  txt += "新臺幣計價" + mgt2AbbrName + "結算保證金金額由原先" +
                           string.Format("{0:N0}" , Math.Round(mgd2CurCm , 0 , MidpointRounding.AwayFromZero)) + "元向";

                  if (mgd2CurCm > mgd2Cm) {
                     txt += "下";
                  } else {
                     txt += "上";
                  } //if (mg1CurCm > mg1Cm)

                  txt += "調整為" + string.Format("{0:N0}" , mgd2Cm) + "元，比例約為";

                  if (mgd2CurCm > 0) {
                     txt += (Math.Round((mgd2Cm - mgd2CurCm) / mgd2CurCm * 100 , 1 , MidpointRounding.AwayFromZero)).AsString() + "%";
                  } //if (mg1CurCm > 0)

               }//if (dtGold.Rows.Count > 0)

               txt += "。茲將調整後本公司黃金期貨契約結算保證金金額，相較於世界主要交易所黃金期貨保證金金額分析如下：(詳如后附表)" + Environment.NewLine;
            }

            //1.5
            if (dt.Rows.Count > 0) {
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

                  txt += "經查目前" + fName + "黃金期貨契約(契約規格100盎司)結算保證金為" + string.Format("{0:N0}" , Math.Round(mg1Cm , 0 , MidpointRounding.AwayFromZero)) +
                     "美元，原始保證金為" + string.Format("{0:N0}" , Math.Round(mg1Im , 0 , MidpointRounding.AwayFromZero)) + "美元；";
               } //if (dt.Rows.Count > 0)

               DataTable dtCME01 = dt.Filter("com='CME01'");
               if (dtCME01.Rows.Count > 0) {
                  string fName = dtCME01.Rows[0]["f_name"].AsString();
                  Decimal mg1Cm = dtCME01.Rows[0]["mg1_cm"].AsDecimal();
                  Decimal mg1Im = dtCME01.Rows[0]["mg1_im"].AsDecimal();

                  txt += "而" + fName + "黃金期貨契約(契約規格100盎司)結算保證金為" + string.Format("{0:N0}" , Math.Round(mg1Cm , 0 , MidpointRounding.AwayFromZero)) +
                     "美元，原始保證金為" + string.Format("{0:N0}" , Math.Round(mg1Im , 0 , MidpointRounding.AwayFromZero)) + "美元；";
               } //if (dt.Rows.Count > 0)

               DataTable dtTOC01 = dt.Filter("com='TOC01'");
               if (dtTOC01.Rows.Count > 0) {
                  string fName = dtTOC01.Rows[0]["f_name"].AsString();
                  Decimal exchangeRate = dtTOC01.Rows[0]["exchange_rate"].AsDecimal();
                  Decimal mg1Im = dtTOC01.Rows[0]["mg1_im"].AsDecimal();

                  txt += "另" + fName + "所收取之原始保證金金額，倘調整契約規格並依匯率(原契約規格1公斤並以" +
                     string.Format("{0:N0}" , Math.Round(exchangeRate , 0 , MidpointRounding.AwayFromZero)) + "日圓對1美元計算)及重量單位(100盎司=3.11公斤)換算後，約為";

                  if (exchangeRate > 0) {

                     txt += string.Format("{0:N0}" , Math.Round((mg1Im / exchangeRate * 3.11m) , 0 , MidpointRounding.AwayFromZero)).AsString();
                  }//if (exchangeRate > 0)

                  txt += "美元。";

               } //if (dt.Rows.Count > 0)

               txt += "有關各交易所保證金占契約總值比率詳附表。" + Environment.NewLine;

            }

            //2.save string to txt
            string fileName = "40141_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".txt";
            string filePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , fileName);

            bool IsSuccess = ToText(txt , filePath , System.Text.Encoding.GetEncoding(950));
            if (!IsSuccess) {
               MessageDisplay.Error("文字檔「" + filePath + "」Open檔案錯誤!" , GlobalInfo.ErrorText);
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