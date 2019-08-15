using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using System;
using System.Data;
using System.IO;

/// <summary>
/// Winni, 2019/3/12
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
   /// <summary>
   /// SPAN參數調整影響分析說明
   /// </summary>
   public partial class W40122 : FormParent {
      protected static string taiwanFuture = "TX";

      public W40122(string programID , string programName) : base(programID , programName) {
         InitializeComponent();

         this.Text = _ProgramID + "─" + _ProgramName;
         txtStartDate.DateTimeValue = GlobalInfo.OCF_DATE;

#if DEBUG
         //ken test
         txtStartDate.DateTimeValue = DateTime.ParseExact("2018/06/14" , "yyyy/MM/dd" , null);
         this.Text += "(開啟測試模式),Date=2018/06/14";
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

            //1.2 get data
            DataTable dt = new D40122().ListData(txtStartDate.DateTimeValue);
            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1},讀取「SPAN參數調整資料」無任何資料!" , txtStartDate.Text , this.Text) , GlobalInfo.ResultText);
               return ResultStatus.Fail;
            }

            string source = "";

            //1.3 sp2_type='SV'
            string resSV = "";
            DataTable dtSV = dt.Filter("sp2_type='SV'");
            if (dtSV.Rows.Count > 0) {
               resSV = "依";

               for (int k = 0 ; k < dtSV.Rows.Count ; k++) {
                  resSV += dtSV.Rows[k]["spt1_abbr_name"].AsString() + ConnString(k , dtSV.Rows.Count);
               }//for (int k = 0;k < dtSV.Rows.Count;k++) {

               DateTime lastSp2Date = dtSV.Rows[dtSV.Rows.Count - 1]["sp2_date"].AsDateTime();
               resSV += string.Format("波動度偵測全距訂定公式計算，{0}前揭商品之波動度偵測全距變動幅度分別為" , PbFunc.f_conv_date(lastSp2Date , 3));

               for (int k = 0 ; k < dtSV.Rows.Count ; k++) {
                  string kindIdOut = dtSV.Rows[k]["spt1_kind_id1_out"].AsString();
                  Decimal changeRange = Math.Round(dtSV.Rows[k]["sp1_change_range"].AsDecimal() * 100 , 1 , MidpointRounding.AwayFromZero);
                  resSV += kindIdOut + "：" + changeRange + "%" + ConnString(k , dtSV.Rows.Count);
               }//for (int k = 0;k < dtSV.Rows.Count;k++) {

               string temp = new D40122().GetRate("SV");
               decimal rate = Math.Round(temp.AsDecimal(0) * 100 , 0 , MidpointRounding.AwayFromZero);
               resSV += string.Format("，其變動幅度達{0}%以上，依本公司「結算保證金收取方式及標準」規定，已達得調整前揭契約波動度偵測全距之標準。" , rate.ToString()) + Environment.NewLine;

            }//if (dtSV.Rows.Count > 0) {

            //1.4 sp2_type='SD'
            string resSD = "";
            DataTable dtSD = dt.Filter("sp2_type='SD'");
            if (dtSD.Rows.Count > 0) {
               resSD = "依兩商品組合間契約價值耗用比率訂定公式計算，";

               for (int k = 0 ; k < dtSD.Rows.Count ; k++) {
                  resSD += dtSD.Rows[k]["spt1_com_id"].AsString() + ConnString(k , dtSD.Rows.Count);
               }//for (int k = 0;k < dtSD.Rows.Count;k++) {

               DateTime lastSp2Date = dtSD.Rows[dtSD.Rows.Count - 1]["sp2_date"].AsDateTime();
               resSD += string.Format("，{0}前揭兩兩商品間契約價值耗用比率變動幅度分別為" , PbFunc.f_conv_date(lastSp2Date , 3));

               for (int k = 0 ; k < dtSD.Rows.Count ; k++) {
                  Decimal changeRange = Math.Round(dtSD.Rows[k]["sp1_change_range"].AsDecimal() * 100 , 1 , MidpointRounding.AwayFromZero);
                  resSD += changeRange + "%" + ConnString(k , dtSD.Rows.Count);
               }//for (int k = 0;k < dtSD.Rows.Count;k++) {

               string temp = new D40122().GetRate("SD");
               decimal rate = Math.Round(temp.AsDecimal(0) * 100 , 0 , MidpointRounding.AwayFromZero);
               resSD += string.Format("，其變動幅度達{0}%以上，依本公司「結算保證金收取方式及標準」規定，已達得調整前揭兩商品組合間契約價值耗用比率之標準。" , rate.ToString()) + Environment.NewLine;

            }//if (dtSD.Rows.Count > 0) {


            //1.5 sp2_type='SS'
            string resSS = "";
            DataTable dtSS = dt.Filter("sp2_type='SS'");
            if (dtSS.Rows.Count > 0) {
               resSS = "依兩商品組合間跨商品價差折抵率訂定公式計算，";

               for (int k = 0 ; k < dtSS.Rows.Count ; k++) {
                  resSS += dtSS.Rows[k]["spt1_com_id"].AsString() + ConnString(k , dtSS.Rows.Count);
               }//for (int k = 0;k < dtSS.Rows.Count;k++) {

               DateTime lastSp2Date = dtSS.Rows[dtSS.Rows.Count - 1]["sp2_date"].AsDateTime();
               resSS += string.Format("，{0}前揭兩商品組合間跨商品價差折抵率變動幅度分別為" , PbFunc.f_conv_date(lastSp2Date , 3));

               for (int k = 0 ; k < dtSS.Rows.Count ; k++) {
                  Decimal changeRange = Math.Round(dtSS.Rows[k]["sp1_change_range"].AsDecimal() * 100 , 1 , MidpointRounding.AwayFromZero);
                  resSS += changeRange + "%" + ConnString(k , dtSS.Rows.Count);
               }//for (int k = 0;k < dtSS.Rows.Count;k++) {

               string temp = new D40122().GetRate("SS");
               decimal rate = Math.Round(temp.AsDecimal(0) * 100 , 0 , MidpointRounding.AwayFromZero);
               resSS += string.Format("，其變動幅度達{0}%以上，依本公司「結算保證金收取方式及標準」規定，已達得調整前揭兩商品組合間跨商品價差折抵率之標準。" , rate.ToString()) + Environment.NewLine;

            }//if (dtSS.Rows.Count > 0) {


            //1.6 save string to txt
            source += resSV + resSD + resSS;//ken,有3個部分組合而成

            string fileName = _ProgramID + "_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".txt";
            string filePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , fileName);

            bool IsSuccess = ToText(source , filePath , System.Text.Encoding.GetEncoding(950));
            if (!IsSuccess) {
               MessageDisplay.Error("文字檔「" + filePath + "」Open檔案錯誤!" , GlobalInfo.ErrorText);
               return ResultStatus.Fail;
            }

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

      /// <summary>
      /// 返回連接字串(倒數第二個用"及",前面用"、")
      /// </summary>
      /// <param name="pos"></param>
      /// <param name="totalCount"></param>
      /// <returns></returns>
      protected string ConnString(int pos , int totalCount) {
         string res = "";

         if (pos == totalCount - 2) {
            res += "及";
         } else if (pos == totalCount - 1) {
            //
         } else {
            res += "、";
         }

         return res;
      }

   }
}