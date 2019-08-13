using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

//TODO : (CIN)servername登入才會看到STWD,SP Button 

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
      protected D20110 dao20110;

      #region 一般交易查詢條件縮寫
      /// <summary>
      /// yyyyMMdd
      /// </summary>
      public string DateYmd {
         get {
            return txtDate.DateTimeValue.ToString("yyyyMMdd");
         }
      }
      #endregion

      ResultStatus resultStatus = ResultStatus.Fail;

      public W28110(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         GridHelper.SetCommonGrid(gvMain);

         daoSTW = new STW();
         daoSTWD = new STWD();
         daoAMIF = new AMIF();
         dao28110 = new D28110();
         dao20110 = new D20110();
      }

      /// <summary>
      /// 需判斷帳號是否為FlagAdmin
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus Open() {
         base.Open();

         txtDate.DateTimeValue = GlobalInfo.OCF_DATE;
         return ResultStatus.Success;
      }

      /// <summary>
      /// 有用到的Icon
      /// </summary>
      /// <returns></returns>
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
      /// 刪除當日 CI.STW資料 (需再retrieve才會顯示資料庫最新資料)
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus DeleteRow() {
         DialogResult msgResult = MessageBox.Show("請問確定要刪除 " + txtDate.Text + " 資料嗎?" , "注意" , MessageBoxButtons.YesNo , MessageBoxIcon.Question);
         if (msgResult == DialogResult.Yes) {
            DataTable dtTmp = daoSTW.GetDataByDate(DateYmd);
            if (dtTmp.Rows.Count <= 0) {
               MessageDisplay.Error("刪除 " + txtDate.Text + " 資料失敗! " , GlobalInfo.ErrorText);
               return ResultStatus.Fail;
            }
            daoSTW.DeleteByDate(DateYmd);
            MessageDisplay.Info("刪除完成!" , GlobalInfo.ResultText);
         }

         return ResultStatus.Success;
      }

      /// <summary>
      /// 讀取查詢日期資料
      /// return STW datatable
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus Retrieve() {
         try {
            DataTable dt = daoSTW.GetDataByDate(DateYmd);
            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info(MessageDisplay.MSG_NO_DATA , GlobalInfo.ResultText);
            }

            //1. 設定gvMain
            gvMain.Columns.Clear();
            gvMain.OptionsBehavior.AutoPopulateColumns = true;
            gcMain.DataSource = dt;

            string[] showColCaption = {"Stw Ymd", "Stw Com", "Stw Settle M","Stw Settle Y","Stw Open 1",
                                       "Stw High", "Stw Low", "Stw Clse 1","Stw Settle","Stw Volumn",
                                       "Stw Oint", "Stw Del", "Stw Rectyp","Stw Open I1","Stw Open 2",
                                       "Stw Open I2", "Stw High I", "Stw Low I","Stw Clse I1","Stw Clse 2","Stw Clse I2"};

            //1.1 設定欄位caption       
            foreach (DataColumn dc in dt.Columns) {
               gvMain.SetColumnCaption(dc.ColumnName , showColCaption[dt.Columns.IndexOf(dc)]);

               //設定欄位header顏色
               gvMain.Columns[dc.ColumnName].AppearanceHeader.BackColor = GridHelper.NORMAL;
            }

            //1.2 設定欄位順序
            gvMain.Columns["STW_RECTYP"].VisibleIndex = 11;
            gvMain.Columns["STW_DEL"].VisibleIndex = 12;

            gvMain.BestFitColumns();
            GridHelper.SetCommonGrid(gvMain);
            gcMain.Visible = true;
            gcMain.Focus();

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
      }

      /// <summary>
      /// 匯入資料
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus Import() {

         string year, month;
         int reIndex; //li_rtn

         bool LastTradeDay = false; //lb_last_trade_day

         try {

            #region 1.讀檔並寫入DataTable 
            //(讀檔有共用函式，可改寫為56090的PbFunc.wf_getfileopenname())
            labMsg.Visible = true;
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
                     DataTable returnTable = daoSTW.GetDataByDate(DateYmd); //只有要用到colume

                     //填入欄位名稱
                     if (dtReadTxt.Columns.Count == 0) {
                        foreach (DataColumn column in returnTable.Columns) {
                           dtReadTxt.Columns.Add(column.ColumnName , typeof(string));
                        }
                     }
                     dtReadTxt.Rows.Add(items);
                  } //while ((line = sr.ReadLine()) != null)
               }
            } else {
               labMsg.Visible = false;
               return ResultStatus.Fail;
            } //if
            #endregion

            #region 2.刪除非TW的
            DataView dv = dtReadTxt.AsDataView();
            dv.RowFilter = "STW_COM IN ('TW')"; //改成只選擇為TW的
            DataTable dt = dv.ToTable();

            if (dt.Rows.Count <= 0) {
               MessageDisplay.Error("沒有TW的資料" , GlobalInfo.ErrorText);
               return ResultStatus.Fail;
            }
            #endregion

            #region 3.確認資料年月&畫面年月(讀取資料)
            string tmp = dt.Rows[0][0].AsString(); //yyyyMMdd
            string dataDate = tmp.Substring(0 , 4) + "/" + tmp.Substring(4 , 2) + "/" + tmp.Substring(6 , 2); //yyyy/MM/dd
            if (dataDate != txtDate.Text) {
               DialogResult result = MessageBox.Show("資料日期(" + dataDate + ")與畫面年月不同,是否將畫面改為資料年月?" , "注意" , MessageBoxButtons.YesNo , MessageBoxIcon.Question);
               if (result == DialogResult.No) {
                  labMsg.Visible = false;
                  return ResultStatus.Fail;
               } else {
                  txtDate.Text = dataDate;
               }
            }
            #endregion

            #region 4.刪除舊有資料
            daoSTW.DeleteByDate(tmp);
            //if (gvMain.DataRowCount > 0) {
            //   DialogResult result = MessageBox.Show("資料日期(" + dataDate + ")資料已存在,是否刪除?" , "注意" , MessageBoxButtons.YesNo , MessageBoxIcon.Question);
            //   if (result == DialogResult.No) {
            //      return ResultStatus.Fail;
            //   } else {
            //      daoSTW.DeleteByDate(tmp);
            //   }
            //}
            #endregion

            #region 5.整理資料
            int fillZero = 10;
            foreach (DataRow dr in dt.Rows) {

               //彙總資料,直接匯入,不需做移位處理	
               if (string.IsNullOrEmpty(dr["STW_SETTLE_M"].AsString())) {
                  dr["STW_OPEN_1"] = dr["STW_OPEN_1"].AsString();
                  dr["STW_HIGH"] = dr["STW_HIGH"].AsString();
                  dr["STW_LOW"] = dr["STW_LOW"].AsString();
                  dr["STW_CLSE_1"] = dr["STW_CLSE_1"].AsString();
               } else {
                  //防止使用者用excel編輯後格式跑掉,先將值固定格式化為9位文字,未滿9位前面補0 	
                  if (!string.IsNullOrEmpty(dr["STW_OPEN_1"].AsString())) {
                     dr["STW_OPEN_1"] = dr["STW_OPEN_1"].AsString().PadLeft(fillZero , '0');
                  }
                  if (!string.IsNullOrEmpty(dr["STW_OPEN_2"].AsString())) {
                     dr["STW_OPEN_2"] = dr["STW_OPEN_2"].AsString().PadLeft(fillZero , '0');
                  }
                  if (!string.IsNullOrEmpty(dr["STW_HIGH"].AsString())) {
                     dr["STW_HIGH"] = dr["STW_HIGH"].AsString().PadLeft(fillZero , '0');
                  }
                  if (!string.IsNullOrEmpty(dr["STW_LOW"].AsString())) {
                     dr["STW_LOW"] = dr["STW_LOW"].AsString().PadLeft(fillZero , '0');
                  }
                  if (!string.IsNullOrEmpty(dr["STW_CLSE_1"].AsString())) {
                     dr["STW_CLSE_1"] = dr["STW_CLSE_1"].AsString().PadLeft(fillZero , '0');
                  }
                  if (!string.IsNullOrEmpty(dr["STW_CLSE_2"].AsString())) {
                     dr["STW_CLSE_2"] = dr["STW_CLSE_2"].AsString().PadLeft(fillZero , '0');
                  }
                  if (!string.IsNullOrEmpty(dr["STW_SETTLE"].AsString())) {
                     dr["STW_SETTLE"] = dr["STW_SETTLE"].AsString().PadLeft(fillZero - 1 , '0');
                     if (dr["STW_SETTLE"].AsDecimal() == 0) {
                        LastTradeDay = true;
                     }
                  }
               }//if (String.IsNullOrEmpty(dr["STW_SETTLE_M"].AsString())) {

               #region 需求9800451(全部往前移1位)
               if (string.IsNullOrEmpty(dr["STW_SETTLE_M"].AsString())) {
                  dr["STW_SETTLE_M"] = "99";
               }
               #endregion

               #region 需求9700259(月份通通為2位格式,前面補0)
               if (dr["STW_SETTLE_M"].AsString().Length == 1) {
                  dr["STW_SETTLE_M"] = ("00" + dr["STW_SETTLE_M"].AsString()).Substring(1 , 2);
               }
               if (string.IsNullOrEmpty(dr["STW_SETTLE_Y"].AsString())) {
                  dr["STW_SETTLE_Y"] = "9999";
               }
               if (string.IsNullOrEmpty(dr["STW_RECTYP"].AsString())) {
                  if (dr["STW_SETTLE_M"].AsString() == "") {
                     dr["STW_RECTYP"] = " ";
                  } else {
                     dr["STW_RECTYP"] = "A";
                  }
               }
               if (string.IsNullOrEmpty(dr["STW_VOLUMN"].AsString())) {
                  dr["STW_VOLUMN"] = "0";
               }
               if (string.IsNullOrEmpty(dr["STW_OINT"].AsString())) {
                  dr["STW_OINT"] = "0";
               }
               #endregion

               #region 需求單9800144(當結算價=0,則OI=0)
               if (string.IsNullOrEmpty(dr["STW_SETTLE"].AsString())) {
                  dr["STW_SETTLE"] = "0";
               }

               if (dr["STW_SETTLE"].AsDecimal() == 0) {
                  dr["STW_OINT"] = "0";
               }
               #endregion

            }//foreach (DataRow dr in dt.Rows) { 
            #endregion

            #region 6.if ids_1.update() > 0  then commit
            if (dt.GetChanges().Rows != null) {
               if (dt.GetChanges().Rows.Count <= 0) {
                  WriteLog("寫入STW錯誤!" , "Error" , "I");
                  MessageDisplay.Error("寫入STW錯誤!" , GlobalInfo.ErrorText);
                  return ResultStatus.Fail;
               }

               resultStatus = daoSTW.UpdateData(dt).Status;
               Retrieve();
            }

            //20100402 最後交易日OI清為0(當日若出現一筆結算價為0,有可能是最後結算日,將履約年月最小的一筆資料OI清為0)
            if (LastTradeDay) {
               string minMonth = daoSTW.GetSettleYM(tmp); //ls_min_month

               DialogResult result = MessageBox.Show("最後交易日之OI應為0,是否要將契約月份" + minMonth + "之OI清為0?" , "注意" , MessageBoxButtons.YesNo , MessageBoxIcon.Question);
               if (result == DialogResult.Yes) {
                  year = minMonth.SubStr(0 , 4);
                  month = minMonth.SubStr(4 , 2);
                  int updateNum = daoSTW.updateOI(tmp , year , month);
                  if (updateNum == -1) {
                     WriteLog("SQL error!" , "Error" , "I");
                     MessageDisplay.Error("SQL error!" , GlobalInfo.ErrorText);
                     return ResultStatus.Fail;
                  }
               }
            }
            #endregion

            #region 7.update/delete amif data
            //需求單10000275 (更新STW 作業:20110資料)
            int tmpDiv; //tmpDiv
            DataTable dtAmif = new DataTable();
            dtAmif = dao28110.getAmifData(txtDate.DateTimeValue);
            if (string.IsNullOrEmpty(dtAmif.Rows.Count.AsString())) {
               return ResultStatus.Fail;
            }

            for (int w = 0 ; w < dtAmif.Rows.Count ; w++) {
               DataRow drAmif = dtAmif.Rows[w];

               if (drAmif["AMIF_SETTLE_DATE"].AsString() == "000000") {

                  reIndex = dt.Rows.IndexOf(dt.Select("STW_SETTLE_M = '' and  STW_SETTLE_Y = ''").FirstOrDefault());
                  DataView dvTemp = dt.AsDataView();
                  dvTemp.RowFilter = "STW_SETTLE_M = '' and  STW_SETTLE_Y = ''";
                  DataTable dtSub = dvTemp.ToTable();

                  tmpDiv = 1;
               } else {
                  reIndex = dt.Rows.IndexOf(dt.Select("stw_settle_y='" + drAmif["AMIF_SETTLE_DATE"].AsString().Substring(0 , 4) +
                           "' and stw_settle_m='" + drAmif["AMIF_SETTLE_DATE"].AsString().Substring(4 , 2) + "'").FirstOrDefault());
                  tmpDiv = 1;
               }

               if (reIndex > 0) {
                  DataRow drSTW = dt.Rows[reIndex - 1];
                  //Open
                  if (drAmif["AMIF_OPEN_PRICE"].AsDecimal() != drSTW["STW_OPEN_1"].AsDecimal() / tmpDiv) {
                     string temp = "更新20110,open:" + drAmif["AMIF_OPEN_PRICE"].AsString() + "," +
                                              string.Format("#0.0#" , drSTW["STW_OPEN_1"].AsDecimal() / tmpDiv).Trim();
                     WriteLog(temp , "Info" , "E");
                  }
                  drAmif["AMIF_OPEN_PRICE"] = drSTW["STW_OPEN_1"].AsDecimal() / tmpDiv;

                  //High
                  if (drAmif["AMIF_HIGH_PRICE"].AsDecimal() != drSTW["STW_HIGH"].AsDecimal() / tmpDiv) {
                     string temp = "更新20110,high:" + drAmif["AMIF_HIGH_PRICE"].AsString() + "," +
                                              string.Format("#0.0#" , drSTW["STW_HIGH"].AsDecimal() / tmpDiv).Trim();
                     WriteLog(temp , "Info" , "E");
                  }
                  drAmif["AMIF_HIGH_PRICE"] = drSTW["STW_HIGH"].AsDecimal() / tmpDiv;

                  //Low
                  if (drAmif["AMIF_LOW_PRICE"].AsDecimal() != drSTW["STW_LOW"].AsDecimal() / tmpDiv) {
                     string temp = "更新20110,low:" + drAmif["AMIF_LOW_PRICE"].AsString() + "," +
                                              string.Format("#0.0#" , drSTW["STW_LOW"].AsDecimal() / tmpDiv).Trim();
                     WriteLog(temp , "Info" , "E");
                  }
                  drAmif["AMIF_LOW_PRICE"] = drSTW["STW_LOW"].AsDecimal() / tmpDiv;

                  //Close
                  if (drAmif["AMIF_CLOSE_PRICE"].AsDecimal() != drSTW["STW_CLSE_1"].AsDecimal() / tmpDiv) {
                     string temp = "更新20110,close:" + drAmif["AMIF_CLOSE_PRICE"].AsString() + "," +
                                              string.Format("#0.0#" , drSTW["STW_CLSE_1"].AsDecimal() / tmpDiv).Trim();
                     WriteLog(temp , "Info" , "E");
                  }
                  drAmif["AMIF_CLOSE_PRICE"] = drSTW["STW_CLSE_1"].AsDecimal() / tmpDiv;

                  //up_down_val
                  //一般 = 本日收盤價 - 昨日收盤價
                  //只要STW = 本日收盤價 - 昨日結算價
                  //若本日收盤 = 0 ,則漲跌=0
                  if (drAmif["AMIF_SETTLE_DATE"].AsString() == "000000") {
                     drAmif["AMIF_UP_DOWN_VAL"] = drAmif["AMIF_CLOSE_PRICE"].AsDecimal() - drAmif["Y_CLOSE_PRICE"].AsDecimal();
                  } else {
                     if (drAmif["AMIF_CLOSE_PRICE"].AsDecimal() == 0) {
                        drAmif["AMIF_UP_DOWN_VAL"] = 0;
                     } else {
                        drAmif["AMIF_UP_DOWN_VAL"] = drAmif["AMIF_CLOSE_PRICE"].AsDecimal() - drAmif["Y_SETTLE_PRICE"].AsDecimal();
                     }
                  }

                  //Settle
                  if (drAmif["AMIF_SETTLE_PRICE"].AsDecimal() != drSTW["STW_SETTLE"].AsDecimal() / tmpDiv) {
                     string temp = "更新20110,settle:" + drAmif["AMIF_SETTLE_PRICE"].AsString() + "," +
                                              string.Format("#0.0#" , drSTW["STW_SETTLE"].AsDecimal() / tmpDiv).Trim();
                     WriteLog(temp , "Info" , "E");
                  }
                  drAmif["AMIF_SETTLE_PRICE"] = drSTW["STW_SETTLE"].AsDecimal() / tmpDiv;

                  //Qnty
                  if (drAmif["AMIF_M_QNTY_TAL"].AsDecimal() != drSTW["STW_VOLUMN"].AsDecimal() / tmpDiv) {
                     string temp = "更新20110,qnty:" + drAmif["AMIF_M_QNTY_TAL"].AsString() + "," +
                                              string.Format("#0" , drSTW["STW_VOLUMN"].AsDecimal() / tmpDiv).Trim();
                     WriteLog(temp , "Info" , "E");
                  }
                  drAmif["AMIF_M_QNTY_TAL"] = drSTW["STW_VOLUMN"].AsDecimal();

                  //Oi
                  if (drAmif["AMIF_OPEN_INTEREST"].AsDecimal() != drSTW["STW_OINT"].AsDecimal()) {
                     string temp = "更新20110,oi:" + drAmif["AMIF_OPEN_INTEREST"].AsString() + "," +
                                              string.Format("#0" , drSTW["STW_OINT"].AsDecimal()).Trim();
                     WriteLog(temp , "Info" , "E");
                  }
                  drAmif["AMIF_OPEN_INTEREST"] = drSTW["STW_OINT"].AsDecimal();
               } else {
                  string temp = "更新20110,刪除" + drAmif["AMIF_SETTLE_DATE"].AsString();
                  WriteLog(temp , "Info" , "E");

                  if (w != 0) {
                     dtAmif.Rows[w].Delete();
                  }

               }
            } //for

            //if    lds_amif.update() > 0  then
            if (dtAmif.GetChanges() != null) {
               if (dtAmif.GetChanges().Rows.Count > 0) {
                  resultStatus = daoAMIF.UpdateData(dtAmif).Status; //commit
               } else {
                  MessageDisplay.Error("更新28110資料錯誤!" , GlobalInfo.ErrorText);
                  return ResultStatus.Fail;
               }
               WriteLog("更新20110資料" , "Info" , "E");
            }
            #endregion

            #region 8.add/delete STWD data (轉統計資料ci.STWD)
            daoSTWD.DeleteByDate(DateYmd);
            WriteLog("刪STWD資料" , "Info" , "E");

            //8.1
            tmpDiv = 1;
            DataTable dtStwd = dao28110.InsertData("I0001" , tmpDiv , tmp); //"I0001"先寫死
            WriteLog("新增STWD資料" , "Info" , "E");
            #endregion

            #region 9.SP

            #region 9.1轉完資料後執行SP
            string prodType = "M";
            int rtn;
            DateTime dateTime = txtDate.DateTimeValue;
            if (RunSP(dateTime , "28110") != "") {
               return ResultStatus.Fail;
            }
            #endregion

            #region 9.2轉統計資料AI2
            if (dao28110.ExecuteSP(dateTime , prodType , "CI.sp_U_stt_H_AI2_Day").Status != ResultStatus.Success) {
               MessageBox.Show("執行SP(sp_U_stt_H_AI2_Day)錯誤! " , "注意" , MessageBoxButtons.OK , MessageBoxIcon.Stop);
               WriteLog("執行SP(sp_U_stt_H_AI2_Day)錯誤!" , "Error");
               return ResultStatus.Fail;
            } else {
               rtn = 0;
            }
            WriteLog("執行sp_U_gen_H_TDT(" + prodType + ")");

            if (dao28110.ExecuteSP(dateTime , prodType , "CI.sp_U_stt_H_AI2_Month").Status != ResultStatus.Success) {
               MessageBox.Show("執行SP(sp_U_stt_H_AI2_Month)錯誤! " , "注意" , MessageBoxButtons.OK , MessageBoxIcon.Stop);
               WriteLog("執行SP(sp_U_stt_H_AI2_Month)錯誤!" , "Error");
               return ResultStatus.Fail;
            } else {
               rtn = 0;
            }
            WriteLog("sp_U_stt_H_AI2_Month(" + prodType + ")");
            #endregion

            #region 9.3轉統計資料ci.STW1
            if (dao28110.ExecuteSP(dateTime , prodType , "CI.sp_M_stt_H_STW1").Status != ResultStatus.Success) {
               MessageBox.Show("執行SP(sp_M_stt_H_STW1)錯誤! " , "注意" , MessageBoxButtons.OK , MessageBoxIcon.Stop);
               WriteLog("執行SP(sp_M_stt_H_STW1)錯誤!" , "Error");
               return ResultStatus.Fail;
            } else {
               rtn = 0;
            }
            WriteLog("sp_M_stt_H_STW1(" + prodType + ")");
            #endregion

            #endregion

            labMsg.Visible = false;
            return ResultStatus.Success;

         } catch (Exception ex) {
            WriteLog(ex);
            MessageBox.Show(ex.Message);
         }

         return ResultStatus.Success;
      }

      /// <summary>
      /// f_20110_SP (同20110的f_20110_SP)
      /// </summary>
      /// <param name="date"></param>
      /// <param name="txnId"></param>
      /// <returns></returns>
      private string RunSP(DateTime date , string txnId) {
         string prodType = "M"; //ls_prod_type
         int rtn; //li_return

         //轉統計資料TDT
         if (dao20110.sp_U_gen_H_TDT(date , prodType).Status != ResultStatus.Success) {
            MessageBox.Show("執行SP(sp_U_gen_H_TDT(" + prodType + "))錯誤! " , "錯誤訊息" , MessageBoxButtons.OK , MessageBoxIcon.Stop);
            return "E";
         } else {
            rtn = 0;
         }
         WriteLog("執行sp_U_gen_H_TDT(" + prodType + ")" , "Info" , "E");

         //Austin 20190813 判斷AOCF該日如無交易不轉統計資料
         AOCF daoAOCF = new AOCF();
         string sdate = date.ToString("yyyyMMdd");
         int AOCFcount = daoAOCF.GetAOCFDates(sdate,sdate);
         if (AOCFcount > 0) {
            /*******************
            轉統計資料AI3
            *******************/
            if (dao20110.sp_H_stt_AI3(date).Status != ResultStatus.Success) {
               MessageBox.Show("執行SP(sp_H_stt_AI3)錯誤! ", "錯誤訊息", MessageBoxButtons.OK, MessageBoxIcon.Stop);
               return "E";
            }
            else {
               rtn = 0;
            }
            WriteLog("執行sp_H_stt_AI3", "Info", "E");

            /*******************
            更新AI6 (震幅波動度)
            *******************/
            if (dao20110.sp_H_gen_AI6(date).Status != ResultStatus.Success) {
               MessageBox.Show("執行SP(sp_H_gen_AI6)錯誤! ", "錯誤訊息", MessageBoxButtons.OK, MessageBoxIcon.Stop);
               return "E";
            }
            else {
               rtn = 0;
            }
            WriteLog("執行sp_H_gen_AI6", "Info", "E");

            /*******************
            更新AA3
            *******************/
            if (dao20110.sp_H_upd_AA3(date).Status != ResultStatus.Success) {
               MessageBox.Show("執行SP(sp_H_upd_AA3)錯誤! ", "錯誤訊息", MessageBoxButtons.OK, MessageBoxIcon.Stop);
               return "E";
            }
            else {
               rtn = 0;
            }
            WriteLog("執行sp_H_upd_AA3", "Info", "E");

            /*******************
            更新AI8
            *******************/
            if (dao20110.sp_H_gen_H_AI8(date).Status != ResultStatus.Success) {
               MessageBox.Show("執行SP(sp_H_gen_H_AI8)錯誤! ", "錯誤訊息", MessageBoxButtons.OK, MessageBoxIcon.Stop);
               return "E";
            }
            else {
               rtn = 0;
            }
            WriteLog("執行sp_H_gen_H_AI8", "Info", "E");
         }
         return "";
      }

      /// <summary>
      /// 決定哪些欄位無法編輯的事件
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void gvMain_ShowingEditor(object sender , CancelEventArgs e) {
         GridView gv = sender as GridView;
         e.Cancel = true;
      }

   }

}