using BaseGround;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/02/12
/// </summary>
namespace PhoenixCI.FormUI.Prefix2 {
   /// <summary>
   /// 28110 每日摩根台指交易資料轉入
   /// 有寫到的功能：Retrieve、Import
   /// </summary>
   public partial class W28110 : FormParent {

      protected STW daoSTW;
      protected STWD daoSTWD;
      protected AMIF daoAMIF;
      protected D28110 dao28110;
      protected bool flagTest = true;
      string as_ym;

      public W28110(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         GridHelper.SetCommonGrid(gvMain);
         GridHelper.SetCommonGrid(gvMain2);
         daoSTW = new STW();
         daoSTWD = new STWD();
         daoAMIF = new AMIF();
         dao28110 = new D28110();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtDate.DateTimeValue = GlobalInfo.OCF_DATE;
      }

      protected override ResultStatus Open() {
         base.Open();

         if (FlagAdmin) {
            btnStwd.Visible = true;
            btnSp.Visible = true;
         }

         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnInsert.Enabled = false;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
         _ToolBtnSave.Enabled = false;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
         _ToolBtnDel.Enabled = true;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

         _ToolBtnRetrieve.Enabled = true;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
         _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

         _ToolBtnImport.Enabled = true;//匯入
         _ToolBtnExport.Enabled = false;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
         _ToolBtnPrintAll.Enabled = false;//列印_ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      /// <summary>
      /// 讀取查詢日期資料
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus Retrieve() {
         base.Retrieve();
         daoSTW = new STW();
         string as_ym = txtDate.Text.Replace("/" , "");
         DataTable returnTable = daoSTW.d_28110(as_ym);
         if (returnTable.Rows.Count <= 0) {
            MessageBox.Show("無任何資料" , "訊息" , MessageBoxButtons.OK , MessageBoxIcon.Information);
         }

         gcMain.DataSource = returnTable;
         gcMain.Visible = true;
         gcMain2.DataSource = returnTable;
         gcMain2.Visible = true;
         gcMain.Focus();

         return ResultStatus.Success;
      }

      protected override ResultStatus Import() {

         string ls_stw_open_1, ls_stw_open_2, ls_stw_high, ls_stw_low, ls_stw_clse_1, ls_stw_clse_2;
         string ls_stw_settle, ls_stw_volumn, ls_stw_oint, ls_min_month;
         string ls_pathname, ls_rtn, ls_year, ls_month, ls_ymd;
         int li_return, li_rtn;
         bool lb_last_trade_day = false;

         try {
            //1.讀檔並寫入DataTable
            lblProcessing.Visible = true;
            DataTable dtReadTxt = new DataTable();
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "*.csv (*.csv)|*.csv";
            open.Title = "請點選儲存檔案之目錄";
            open.FileName = "FUT.csv";
            DialogResult openResult = open.ShowDialog();
            if (openResult == DialogResult.OK) {
               using (StreamReader sr = new StreamReader(open.FileName , System.Text.Encoding.Default)) {
                  string line;
                  while ((line = sr.ReadLine()) != null) {
                     string[] items = line.Split(',');
                     as_ym = txtDate.Text.Replace("/" , "");
                     DataTable returnTable = daoSTW.d_28110(as_ym);

                     //填入欄位名稱
                     if (dtReadTxt.Columns.Count <= 0) {
                        foreach (DataColumn column in returnTable.Columns) {
                           dtReadTxt.Columns.Add(column.ColumnName , typeof(string));
                        }
                     }
                     dtReadTxt.Rows.Add(items);
                  }
               }
            } else {
               lblProcessing.Visible = false;
               return ResultStatus.Fail;
            }

            //刪除非TW的
            DataView dv = dtReadTxt.AsDataView();
            //stw_com <> 'TW' or isnull(stw_com)
            dv.RowFilter = "STW_COM IN ('TW')"; //改成只選擇為TW的
            DataTable dt = dv.ToTable();
            //直接選擇TW的資料，此段不用執行
            //do {
            //   dt.Rows.RemoveAt(0);
            //   } while (dt.Rows.Count != 0) ;

            if (dt.Rows.Count <= 0) {
               MessageBox.Show("沒有TW的資料" , "訊息" , MessageBoxButtons.OK , MessageBoxIcon.Information);
               return ResultStatus.Fail;
            }

            //3.確認資料年月&畫面年月(讀取資料)
            string tmp = dt.Rows[0][0].AsString();
            string dataDate = tmp.Substring(0 , 4) + "/" + tmp.Substring(4 , 2) + "/" + tmp.Substring(6 , 2);
            if (dataDate != txtDate.Text) {
               DialogResult result = MessageBox.Show("資料年月(" + dataDate + ")與畫面年月不同,是否將畫面改為資料年月?" , "注意" , MessageBoxButtons.YesNo , MessageBoxIcon.Question);
               if (result == DialogResult.No) {
                  lblProcessing.Visible = false;
                  return ResultStatus.Fail;
               } else {
                  txtDate.Text = dataDate;
                  Retrieve();
               }
            }

            //4.刪除舊有資料
            if (gvMain.DataRowCount > 0) {
               DialogResult result = MessageBox.Show("資料年月(" + dataDate + ")資料已存在,是否刪除?" , "注意" , MessageBoxButtons.YesNo , MessageBoxIcon.Question);
               if (result == DialogResult.No) {
                  return ResultStatus.Fail;
               } else {
                  dao28110.DeleteByDate(tmp);
               }
            }

            int li_fill = 10, li_shift = 0;
            for (int i = 0 ; i < dt.Rows.Count ; i++) {
               //彙總資料,直接匯入,不需做移位處理	
               if (String.IsNullOrEmpty(dt.Rows[i]["STW_SETTLE_M"].AsString())) {
                  dt.Rows[i]["STW_OPEN_1"] = dt.Rows[i]["STW_OPEN_1"].AsString().Trim();
                  dt.Rows[i]["STW_HIGH"] = dt.Rows[i]["STW_HIGH"].AsString().Trim();
                  dt.Rows[i]["STW_LOW"] = dt.Rows[i]["STW_LOW"].AsString().Trim();
                  dt.Rows[i]["STW_CLSE_1"] = dt.Rows[i]["STW_CLSE_1"].AsString().Trim();
               } else {
                  //防止使用者用excel編輯後格式跑掉,先將值固定格式化為9位文字,未滿9位前面補0 	
                  if (!String.IsNullOrEmpty(dt.Rows[i]["STW_OPEN_1"].AsString().Trim())) {
                     int addZero = li_fill - dt.Rows[i]["STW_OPEN_1"].AsString().Trim().Length; //要補0的長度
                     dt.Rows[i]["STW_OPEN_1"] = dt.Rows[i]["STW_OPEN_1"].AsString().Trim().PadLeft(li_fill , '0');
                  }
                  if (!String.IsNullOrEmpty(dt.Rows[i]["STW_OPEN_2"].AsString().Trim())) {
                     dt.Rows[i]["STW_OPEN_2"] = dt.Rows[i]["STW_OPEN_2"].AsString().Trim().PadLeft(li_fill , '0');
                  }
                  if (!String.IsNullOrEmpty(dt.Rows[i]["STW_HIGH"].AsString().Trim())) {
                     dt.Rows[i]["STW_HIGH"] = dt.Rows[i]["STW_HIGH"].AsString().Trim().PadLeft(li_fill , '0');
                  }
                  if (!String.IsNullOrEmpty(dt.Rows[i]["STW_LOW"].AsString().Trim())) {
                     dt.Rows[i]["STW_LOW"] = dt.Rows[i]["STW_LOW"].AsString().Trim().PadLeft(li_fill , '0');
                  }
                  if (!String.IsNullOrEmpty(dt.Rows[i]["STW_CLSE_1"].AsString().Trim())) {
                     dt.Rows[i]["STW_CLSE_1"] = dt.Rows[i]["STW_CLSE_1"].AsString().Trim().PadLeft(li_fill , '0');
                  }
                  if (!String.IsNullOrEmpty(dt.Rows[i]["STW_CLSE_2"].AsString().Trim())) {
                     dt.Rows[i]["STW_CLSE_2"] = dt.Rows[i]["STW_CLSE_2"].AsString().Trim().PadLeft(li_fill , '0');
                  }
                  if (!String.IsNullOrEmpty(dt.Rows[i]["STW_SETTLE"].AsString().Trim())) {
                     dt.Rows[i]["STW_SETTLE"] = dt.Rows[i]["STW_SETTLE"].AsString().Trim().PadLeft(li_fill - 1 , '0');
                     if (dt.Rows[i]["STW_SETTLE"].AsInt() == 0) {
                        lb_last_trade_day = true;
                     }
                  }
               }

               //需求9800451
               //全部往前移1位
               if (String.IsNullOrEmpty(dt.Rows[i]["STW_SETTLE_M"].AsString())) {
                  dt.Rows[i]["STW_SETTLE_M"] = "99";
               }

               //需求9700259
               //月份通通為2位格式,前面補0
               if (dt.Rows[i]["STW_SETTLE_M"].AsString().Length == 1) {
                  dt.Rows[i]["STW_SETTLE_M"] = ("00" + dt.Rows[i]["STW_SETTLE_M"].AsString().Trim()).Substring(1 , 2);
               }
               if (String.IsNullOrEmpty(dt.Rows[i]["STW_SETTLE_Y"].AsString())) {
                  dt.Rows[i]["STW_SETTLE_Y"] = "9999";
               }
               if (String.IsNullOrEmpty(dt.Rows[i]["STW_RECTYP"].AsString())) {
                  if (dt.Rows[i]["STW_SETTLE_M"].AsString().Trim() == "") {
                     dt.Rows[i]["STW_RECTYP"] = " ";
                  } else {
                     dt.Rows[i]["STW_RECTYP"] = "A";
                  }
               }
               if (String.IsNullOrEmpty(dt.Rows[i]["STW_VOLUMN"].AsString())) {
                  dt.Rows[i]["STW_VOLUMN"] = "0";
               }
               if (String.IsNullOrEmpty(dt.Rows[i]["STW_OINT"].AsString())) {
                  dt.Rows[i]["STW_OINT"] = "0";
               }

               //需求單9800144
               //當結算價=0,則OI=0
               if (String.IsNullOrEmpty(dt.Rows[i]["STW_SETTLE"].AsString())) {
                  dt.Rows[i]["STW_SETTLE"] = "0";
               }
               if (dt.Rows[i]["STW_SETTLE"].AsDecimal() == 0) {
                  dt.Rows[i]["STW_OINT"] = "0";
               }
            }

            //if ids_1.update() > 0  then
            if (dt.Rows.Count > 0) {
               //20100402 最後交易日OI清為0(當日若出現一筆結算價為0,有可能是最後結算日,將履約年月最小的一筆資料OI清為0)
               if (lb_last_trade_day) {
                  DataTable dtYM = new DataTable();
                  dtYM = daoSTW.getSettleYM(as_ym);
                  ls_min_month = Convert.ToString(dtYM.Compute("Min(A)" , "2323"));
                  DialogResult result = MessageBox.Show("最後交易日之OI應為0,是否要將契約月份" + ls_min_month + "之OI清為0?" , "注意" , MessageBoxButtons.YesNo , MessageBoxIcon.Question);
                  if (result == DialogResult.No) {
                     return ResultStatus.Fail;
                  } else {
                     ls_year = ls_min_month.Substring(0 , 4);
                     ls_month = ls_min_month.Substring(4 , 2);
                     int updateNum = daoSTW.updateOI(as_ym , ls_year , ls_month);
                     if (updateNum == -1) {
                        DialogResult Message = MessageBox.Show("SQL error" , "注意" , MessageBoxButtons.YesNo , MessageBoxIcon.Question);
                     }
                  }
               }
            } else {
               DialogResult Message = MessageBox.Show("寫入STW錯誤!" , "注意" , MessageBoxButtons.YesNo , MessageBoxIcon.Question);
            }

            //需求單10000275 (更新STW 作業:20110資料)
            int li_div;
            string ls_prev_ym;
            DataTable dtAmit = dao28110.d_28110_amif(txtDate.DateTimeValue);
            for (int i = 0 ; i < dtAmit.Rows.Count ; i++) {
               if (dtAmit.Rows[i]["AMIF_SETTLE_DATE"].AsString() == "000000") {
                  //30650 參考用 看懂後刪掉
                  //dtTmp.PrimaryKey = new DataColumn[] { dtTmp.Columns["ABRK_NO"] };
                  //ll_found = dtTmp.Rows.IndexOf(dtTmp.Rows.Find(dtContent.Rows[i]["ABRK_NO"])).AsInt();               
                  //ll_found = ids_tmp.find("abrk_no='" + ids_1.getitemstring(i , "abrk_no") + "'" , 1 , ids_tmp.rowcount())

                  //li_rtn = ids_1.find("stw_settle_m = '' and  stw_settle_y = ''" , 1 , ids_1.rowcount())
                  li_rtn = dt.Rows.IndexOf(dt.Select("STW_SETTLE_M = '' and  STW_SETTLE_Y = ''").FirstOrDefault());
                  li_div = 1;
               } else {
                  li_rtn = dt.Rows.IndexOf(dt.Select("stw_settle_y='" + dtAmit.Rows[i]["AMIF_SETTLE_DATE"].AsString().Substring(0 , 4) +
                           "' and stw_settle_m='" + dtAmit.Rows[i]["AMIF_SETTLE_DATE"].AsString().Substring(4 , 2) + "'").FirstOrDefault());
                  li_div = 1;
               }

               if (li_rtn > 0) {
                  //Open
                  if (dtAmit.Rows[i]["AMIF_OPEN_PRICE"].AsDecimal() != dt.Rows[li_rtn - 1]["STW_OPEN_1"].AsDecimal() / li_div) {
                     PbFunc.f_write_logf(_ProgramID , "E" , "更新20110,open:" + dtAmit.Rows[i]["AMIF_OPEN_PRICE"].AsString().Trim() + "," +
                                              String.Format("#0.0#" , dt.Rows[li_rtn - 1]["STW_OPEN_1"].AsDecimal() / li_div).Trim());
                  }
                  dtAmit.Rows[i]["AMIF_OPEN_PRICE"] = dt.Rows[li_rtn - 1]["STW_OPEN_1"].AsDecimal() / li_div;

                  //high
                  if (dtAmit.Rows[i]["AMIF_HIGH_PRICE"].AsDecimal() != dt.Rows[li_rtn - 1]["STW_HIGH"].AsDecimal() / li_div) {
                     PbFunc.f_write_logf(_ProgramID , "E" , "更新20110,high:" + dtAmit.Rows[i]["AMIF_HIGH_PRICE"].AsString().Trim() + "," +
                                              String.Format("#0.0#" , dt.Rows[li_rtn - 1]["STW_HIGH"].AsDecimal() / li_div).Trim());
                  }
                  dtAmit.Rows[i]["AMIF_HIGH_PRICE"] = dt.Rows[li_rtn - 1]["STW_HIGH"].AsDecimal() / li_div;

                  //low
                  if (dtAmit.Rows[i]["AMIF_LOW_PRICE"].AsDecimal() != dt.Rows[li_rtn - 1]["STW_LOW"].AsDecimal() / li_div) {
                     PbFunc.f_write_logf(_ProgramID , "E" , "更新20110,low:" + dtAmit.Rows[i]["AMIF_LOW_PRICE"].AsString().Trim() + "," +
                                              String.Format("#0.0#" , dt.Rows[li_rtn - 1]["STW_LOW"].AsDecimal() / li_div).Trim());
                  }
                  dtAmit.Rows[i]["AMIF_LOW_PRICE"] = dt.Rows[li_rtn - 1]["STW_LOW"].AsDecimal() / li_div;

                  //close
                  if (dtAmit.Rows[i]["AMIF_CLOSE_PRICE"].AsDecimal() != dt.Rows[li_rtn - 1]["STW_CLSE_1"].AsDecimal() / li_div) {
                     PbFunc.f_write_logf(_ProgramID , "E" , "更新20110,close:" + dtAmit.Rows[i]["AMIF_CLOSE_PRICE"].AsString().Trim() + "," +
                                              String.Format("#0.0#" , dt.Rows[li_rtn - 1]["STW_CLSE_1"].AsDecimal() / li_div).Trim());
                  }
                  dtAmit.Rows[i]["AMIF_CLOSE_PRICE"] = dt.Rows[li_rtn - 1]["STW_CLSE_1"].AsDecimal() / li_div;

                  //up_down_val
                  //一般 = 本日收盤價 - 昨日收盤價
                  //只要STW = 本日收盤價 - 昨日結算價
                  //若本日收盤 = 0 ,則漲跌=0
                  if (dtAmit.Rows[i]["AMIF_SETTLE_DATE"].AsString() == "000000") {
                     dtAmit.Rows[i]["AMIF_UP_DOWN_VAL"] = dtAmit.Rows[i]["AMIF_CLOSE_PRICE"].AsDecimal() - dtAmit.Rows[i]["Y_CLOSE_PRICE"].AsDecimal();
                  } else {
                     if (dtAmit.Rows[i]["AMIF_CLOSE_PRICE"].AsDecimal() == 0) {
                        dtAmit.Rows[i]["AMIF_UP_DOWN_VAL"] = 0;
                     } else {
                        dtAmit.Rows[i]["AMIF_UP_DOWN_VAL"] = dtAmit.Rows[i]["AMIF_CLOSE_PRICE"].AsDecimal() - dtAmit.Rows[i]["Y_SETTLE_PRICE"].AsDecimal();
                     }
                  }

                  //settle
                  if (dtAmit.Rows[i]["AMIF_SETTLE_PRICE"].AsDecimal() != dt.Rows[li_rtn - 1]["STW_SETTLE"].AsDecimal() / li_div) {
                     PbFunc.f_write_logf(_ProgramID , "E" , "更新20110,settle:" + dtAmit.Rows[i]["AMIF_SETTLE_PRICE"].AsString().Trim() + "," +
                                              String.Format("#0.0#" , dt.Rows[li_rtn - 1]["STW_SETTLE"].AsDecimal() / li_div).Trim());
                  }
                  dtAmit.Rows[i]["AMIF_SETTLE_PRICE"] = dt.Rows[li_rtn - 1]["STW_SETTLE"].AsDecimal() / li_div;

                  //qnty
                  if (dtAmit.Rows[i]["AMIF_M_QNTY_TAL"].AsDecimal() != dt.Rows[li_rtn - 1]["STW_VOLUMN"].AsDecimal() / li_div) {
                     PbFunc.f_write_logf(_ProgramID , "E" , "更新20110,qnty:" + dtAmit.Rows[i]["AMIF_M_QNTY_TAL"].AsString().Trim() + "," +
                                              String.Format("#0" , dt.Rows[li_rtn - 1]["STW_VOLUMN"].AsDecimal() / li_div).Trim());
                  }
                  dtAmit.Rows[i]["AMIF_M_QNTY_TAL"] = dt.Rows[li_rtn - 1]["STW_VOLUMN"].AsDecimal();

                  // oi
                  if (dtAmit.Rows[i]["AMIF_OPEN_INTEREST"].AsDecimal() != dt.Rows[li_rtn - 1]["STW_OINT"].AsDecimal()) {
                     PbFunc.f_write_logf(_ProgramID , "E" , "更新20110,oi:" + dtAmit.Rows[i]["AMIF_OPEN_INTEREST"].AsString().Trim() + "," +
                                              String.Format("#0" , dt.Rows[li_rtn - 1]["STW_OINT"].AsDecimal()).Trim());
                  }
                  dtAmit.Rows[i]["AMIF_OPEN_INTEREST"] = dt.Rows[li_rtn - 1]["STW_OINT"].AsDecimal();
               } else {
                  PbFunc.f_write_logf(_ProgramID , "E" , "更新20110,刪除" + dtAmit.Rows[i]["AMIF_SETTLE_DATE"].AsString().Trim());
                  dt.Rows.RemoveAt(i);
                  i--;
               }
            }

            //if    lds_amif.update() > 0  then
            if (dtAmit.Rows.Count > 0) {
               //
            } else {
               DialogResult Message = MessageBox.Show("更新28110資料錯誤!" , "注意" , MessageBoxButtons.YesNo , MessageBoxIcon.Question);
            }
            PbFunc.f_write_logf(_ProgramID , "E" , "更新20110資料");

            //轉統計資料ci.STWD
            daoSTWD.DeleteByDate(tmp);
            PbFunc.f_write_logf(_ProgramID , "E" , "刪STWD資料");

            li_div = 1;
            DataTable dtStwd = dao28110.InsertData("I0001" , li_div , tmp);
            PbFunc.f_write_logf(_ProgramID , "E" , "新增STWD資料");

            //轉完資料後執行SP
            string ls_prod_type = "M";
            DateTime ldt_date = txtDate.DateTimeValue;
            //if (f_20110_SP(ldt_date , "28110") != "") {
            //   //
            //}

            //轉統計資料AI2
            DataTable resDay = dao28110.ExecuteStoredProcedure(ldt_date , ls_prod_type , "sp_U_stt_H_AI2_Day");
            if (resDay.Rows.Count < 0) {
               MessageBox.Show("執行SP(sp_U_stt_H_AI2_Day)錯誤! " , "注意" , MessageBoxButtons.OK , MessageBoxIcon.Exclamation);
            } else {
               lblProcessing.Text = "轉檔完成!";
               MessageBox.Show("轉檔完成" , "訊息" , MessageBoxButtons.OK , MessageBoxIcon.Information);
            }
            PbFunc.f_write_logf(_ProgramID , "E" , "執行sp_U_stt_H_AI2_Day");

            DataTable resMon = dao28110.ExecuteStoredProcedure(ldt_date , ls_prod_type , "sp_U_stt_H_AI2_Month");
            if (resMon.Rows.Count < 0) {
               MessageBox.Show("執行SP(sp_U_stt_H_AI2_Month)錯誤! " , "注意" , MessageBoxButtons.OK , MessageBoxIcon.Exclamation);
            } else {
               lblProcessing.Text = "轉檔完成!";
               MessageBox.Show("轉檔完成" , "訊息" , MessageBoxButtons.OK , MessageBoxIcon.Information);
            }
            PbFunc.f_write_logf(_ProgramID , "E" , "執行sp_U_stt_H_AI2_Month");

            //轉統計資料ci.STW1
            DataTable resStw1 = dao28110.ExecuteStoredProcedure(ldt_date , ls_prod_type , "sp_M_stt_H_STW1");
            if (resStw1.Rows.Count < 0) {
               MessageBox.Show("執行SP(sp_M_stt_H_STW1)錯誤! " , "注意" , MessageBoxButtons.OK , MessageBoxIcon.Exclamation);
            } else {
               lblProcessing.Text = "轉檔完成!";
               MessageBox.Show("轉檔完成" , "訊息" , MessageBoxButtons.OK , MessageBoxIcon.Information);
            }



            PokeBall pb = new PokeBall();
            dtReadTxt.Columns.Remove("ACC_NAME");

            gcMain.DataSource = dtReadTxt;

            try {
               //insert to DB
               foreach (DataRow dr in dtReadTxt.Rows) {
                  if (dr.RowState == DataRowState.Added) {
                     string ab1_acc_type = dr["AB1_ACC_TYPE"].AsString();
                     string ab1_count = dr["AB1_COUNT"].AsString();
                     string ab1_accu_count = dr["AB1_ACCU_COUNT"].AsString();
                     string ab1_trade_count = dr["AB1_TRADE_COUNT"].AsString();
                     DateTime ab1_date = dr["AB1_DATE"].AsDateTime();
                     //bool rtn = dao28610.InsertAB1(ab1_acc_type , ab1_count , ab1_accu_count , ab1_trade_count , ab1_date);
                  }
               }
            } catch (Exception ex) {
               MessageBox.Show(ex.Message);
            }

            lblProcessing.Visible = false;
            return ResultStatus.Success;

         } catch (Exception ex) {
            MessageBox.Show(ex.Message);
         }

         return ResultStatus.Success;
      }

   }
}