using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraWaitForm;
using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/3/12
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
   /// <summary>
   /// 保證金調整影響分析說明
   /// </summary>
   public partial class W40120 : FormParent {
      protected static string taiwanFuture = "TX";

      public W40120(string programID , string programName) : base(programID , programName) {
         InitializeComponent();

         this.Text = _ProgramID + "─" + _ProgramName;
         txtStartDate.DateTimeValue = GlobalInfo.OCF_DATE;

#if DEBUG
         //ken test
         txtStartDate.DateTimeValue = DateTime.ParseExact("2008/10/23" , "yyyy/MM/dd" , null);
         this.Text += "(開啟測試模式),Date=2008/10/23";
#endif

      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Export() {
         try {
            //1.1 ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "訊息：資料轉出中........";
            this.Refresh();

            //SplashScreenManager.ShowForm(this , typeof(FormWait) , true , true);
            //SplashScreenManager.CloseForm();

            this.Invoke(new MethodInvoker(() => {
               FormWait formWait = new FormWait();

               SplashScreenManager.ShowForm(this , typeof(FormWait) , true , true);

               //SplashScreenManager.ShowForm(this , typeof(FormWait) , true , true , SplashFormStartPosition.Manual , pointWait , ParentFormState.Locked);
            }));



            //1.2 get data
            DataTable dt = new D40120().ListData(txtStartDate.DateTimeValue);
            DataTable dt1 = new D40120().ListData2(txtStartDate.DateTimeValue);//新的SQL
            if (dt.Rows.Count <= 0) {
               SplashScreenManager.CloseForm();
               MessageDisplay.Info(string.Format("{0},{1}文字說明,讀取「案由一契約名稱」無任何資料!" , txtStartDate.Text , _ProgramID) , GlobalInfo.ResultText);
               return ResultStatus.Fail;
            }

            //1.3 表頭
            string source = "本公司保證金調整影響分析" + Environment.NewLine;
            source += "一、調整之合理性：" + Environment.NewLine;

            //1.4一、調整之合理性 mgt2_prod_type='F'
            string resFuture = "";
            string tempFuture = "";
            string ls_txt_end = "";
            int pos = 0;

            DataTable dtFuture = dt.Filter("mgt2_prod_type = 'F'");
            if (dtFuture.Rows.Count > 0) {
               resFuture = "依結算保證金訂定公式計算，";

               //1.4.1抓TX轉字串
               foreach (DataRow dr in dtFuture.Rows) {
                  pos++;
                  string abbrName = dr["mgt2_abbr_name"].AsString();
                  string kindIdOut = dr["mgt2_kind_id_out"].AsString();
                  Decimal changeRange = Math.Round(dr["mgd2_adj_rate"].AsDecimal() * 100 , 1 , MidpointRounding.AwayFromZero);
                  string kindId = dr["mgd2_kind_id"].AsString();

                  resFuture += abbrName;
                  tempFuture += kindIdOut + "：" + changeRange.ToString() + "%";

                  //台指期貨(taiwanFuture)
                  if (kindIdOut == taiwanFuture) {
                     //dtMgt2 return mgt2_group_kind_id/mgt2_prod_type/mgt2_kind_id/mgt2_kind_id_out/mgt2_abbr_name/mgt2_name
                     DataTable dtMgt2 = new D40120().ListMgt2ByKindId(kindIdOut);
                     DataTable dttempFuture = dtMgt2.Filter(string.Format("mgt2_prod_type='F' and mgt2_kind_id <> '{0}'" , kindId));

                     foreach (DataRow drtempFuture in dttempFuture.Rows) {
                        string abbrName2 = drtempFuture["mgt2_abbr_name"].AsString();

                        if (pos == dtFuture.Rows.Count) {
                           resFuture += "及";
                        } else {
                           resFuture += "、";
                        }
                        resFuture += abbrName2;

                     }//foreach (DataRow drtempFuture in dttempFuture.Rows) {
                  }//if (kindIdOut == taiwanFuture) {

                  if (pos == dtFuture.Rows.Count - 1) {
                     string lastRowKindIdOut = dtFuture.Rows[pos]["mgt2_kind_id_out"].AsString();
                     if (lastRowKindIdOut == taiwanFuture) {
                        resFuture += "、";
                        tempFuture += "、";
                     } else {
                        resFuture += "及";
                        tempFuture += "及";
                     }
                  } else if (pos == dtFuture.Rows.Count) {
                     //
                  } else {
                     resFuture += "、";
                     tempFuture += "、";
                  }//if (pos == dtFuture.Rows.Count - 1) {

               }//foreach(DataRow dr in dtFuture.Rows) {

               resFuture = resFuture + '，';


               #region //1.4.2分日期轉字串
               DateTime ldt_date = dtFuture.Rows[0]["mgd2_ymd"].AsDateTime();
               DataTable dtDate = dtFuture.Filter(string.Format("mgt2_prod_type='F' and mgd2_ymd =#{0}#" , ldt_date.ToString("yyyy/MM/dd")));//mm/dd/yyyy

               while (ldt_date != DateTime.MinValue) {
                  DateTime lastMg2Date = dtFuture.Rows[dtFuture.Rows.Count - 1]["mgd2_ymd"].AsDateTime();
                  tempFuture = PbFunc.f_conv_date(lastMg2Date , 3) + "前揭商品之結算保證金變動幅度";
                  if (dtFuture.Rows.Count >= 2)
                     tempFuture += "分別為 ";
                  else
                     tempFuture += "為 ";

                  for (int k = 0 ; k < dtFuture.Rows.Count ; k++) {
                     string kindIdOut = dtFuture.Rows[k]["mgt2_kind_id_out"].AsString();
                     Decimal changeRange = Math.Round(dtFuture.Rows[k]["mgd2_adj_rate"].AsDecimal() * 100 , 1 , MidpointRounding.AwayFromZero);
                     tempFuture += kindIdOut + "：" + changeRange + "%";
                     if (k == dtFuture.Rows.Count - 2) {
                        tempFuture += "及";
                     } else if (k == dtFuture.Rows.Count - 1) {
                        //
                     } else {
                        tempFuture += "、";
                     }
                  }

                  resFuture += tempFuture + "，";

                  DataTable dtDate2 = dtFuture.Filter(string.Format("mgt2_prod_type='F' and mgd2_ymd > #{0}#" , ldt_date.ToString("yyyy/MM/dd")));//mm/dd/yyyy
                  if (dtDate2.Rows.Count > 0) {
                     ldt_date = dtDate2.Rows[0]["mgd2_ymd"].AsDateTime();
                  } else {
                     break;// ldt_date = DateTime.MinValue;
                  }
               }//while (ldt_date != DateTime.MinValue) {
               #endregion

               Decimal mg1_change_cond = Math.Round(dtFuture.Rows[dtFuture.Rows.Count - 1]["mg1_change_cond"].AsDecimal() * 100 , 0 , MidpointRounding.AwayFromZero);
               ls_txt_end = string.Format("其變動幅分達{0}%以上且進位後金額改變，依本公司結算保證金收取方式及標準規定，已達得調整保證金之標準。" ,
                                           mg1_change_cond.ToString());
            }//if (dtFuture.Rows.Count > 0) {


            //1.5一、調整之合理性 mgt2_prod_type='O'
            DataTable dtOption = dt.Filter("mgt2_prod_type = 'O'");
            string resOption = "";
            string tempFutureOption = "";

            if (dtOption.Rows.Count > 0) {
               pos = 0;

               foreach (DataRow dr in dtOption.Rows) {
                  pos++;
                  string abbrName = dr["mgt2_abbr_name"].AsString();
                  string kindIdOut = dr["mgt2_kind_id_out"].AsString();
                  Decimal changeRange = Math.Round(dr["mgd2_adj_rate"].AsDecimal() * 100 , 1 , MidpointRounding.AwayFromZero);

                  resOption += abbrName;
                  tempFutureOption += kindIdOut + "：" + changeRange.ToString() + "%";

                  if (pos == dtOption.Rows.Count - 1) {
                     resOption += "及";
                     tempFutureOption += "及";
                  } else if (pos == dtOption.Rows.Count) {
                     //
                  } else {
                     resOption += "、";
                     tempFutureOption += "、";
                  }//if (pos == dtOption.Rows.Count - 1) {

               }//foreach(DataRow dr in dtOption.Rows) {

               resOption += "風險保證金(A值)之變動幅度";

               if (dtOption.Rows.Count >= 2)
                  resOption += "分別為 ";
               else
                  resOption += "為 ";

               resOption += tempFutureOption + "，";

               Decimal mg1_change_cond = Math.Round(dtOption.Rows[dtOption.Rows.Count - 1]["mg1_change_cond"].AsDecimal() * 100 , 0 , MidpointRounding.AwayFromZero);
               ls_txt_end = string.Format("其變動幅分達{0}%以上且進位後金額改變，依本公司結算保證金收取方式及標準規定，已達得調整保證金之標準。" ,
                                               mg1_change_cond.ToString());

            }//if (dtOption.Rows.Count > 0) {


            //1.6 save string to txt
            source += resFuture + resOption + ls_txt_end + Environment.NewLine;//ken,有四個部分組合而成

            string fileName = _ProgramID + "_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".txt";
            string filePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , fileName);

            bool IsSuccess = ToText(source , filePath , System.Text.Encoding.GetEncoding(950));
            if (!IsSuccess) {
               MessageDisplay.Error("文字檔「" + filePath + "」Open檔案錯誤!" , GlobalInfo.ErrorText);
               return ResultStatus.Fail;
            }

            SplashScreenManager.Default.SetWaitFormCaption("請燒等");
            SplashScreenManager.Default.SetWaitFormDescription("訊息：資料轉出中........12345");
            Thread.Sleep(2500);
            SplashScreenManager.CloseForm();

            //if (FlagAdmin)
            //   System.Diagnostics.Process.Start(filePath);

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