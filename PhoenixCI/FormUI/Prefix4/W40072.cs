using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

/// <summary>
/// Lukas, 2019/5/8
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {

   public partial class W40072 : FormParent {

      /// <summary>
      /// 調整類型 0一般 1長假 2處置股票 3股票
      /// </summary>
      protected string isAdjType { get; set; }
      /// <summary>
      /// 交易日期
      /// </summary>
      protected string ymd { get; set; }
      private D40071 dao40071;
      private D40072 dao40072;
      private MGD2 daoMGD2;
      private MGD2L daoMGD2L;
      private MGRT1 daoMGRT1;
      private MOCF daoMOCF;
      private RepositoryItemLookUpEdit rateLookUpEdit;
      private RepositoryItemLookUpEdit prodTypeLookUpEdit1;//期貨
      private RepositoryItemLookUpEdit prodTypeLookUpEdit2;//選擇權
      private DataTable dtFLevel;//期貨級距
      private DataTable dtOLevel;//選擇權級距

      public W40072(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         dao40071 = new D40071();
         dao40072 = new D40072();
         daoMGD2 = new MGD2();
         daoMGD2L = new MGD2L();
         daoMGRT1 = new MGRT1();
         daoMOCF = new MOCF();
         dtFLevel = new DataTable();
         dtFLevel = daoMGRT1.dddw_mgrt1("F");//先讀，後面在不同的地方會用到
         dtOLevel = new DataTable();
         dtOLevel = daoMGRT1.dddw_mgrt1("O");//先讀，後面在不同的地方會用到
         GridHelper.SetCommonGrid(gvMain);
         GridHelper.SetCommonGrid(gvDetail);
         gvDetail.AppearancePrint.BandPanel.Font = new Font("Microsoft YaHei" , 10);
         gvDetail.AppearancePrint.BandPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
      }

      protected override ResultStatus Open() {
         base.Open();
         //設定日期和全域變數
         txtSDate.DateTimeValue = DateTime.Now;
#if DEBUG
         txtSDate.EditValue = "2019/02/27";
#endif
         ymd = txtSDate.DateTimeValue.ToString("yyyyMMdd");
         isAdjType = "2";

         //取得table的schema，因為程式開啟會預設插入一筆空資料列
         DataTable dtMGD2 = dao40071.d_40071();
         gcMain.DataSource = dtMGD2;

         #region 下拉選單設定
         //調整倍數下拉選單
         //List<LookupItem> rateList = new List<LookupItem>(){
         //                               new LookupItem() { ValueMember = "1.5", DisplayMember = "1.5"},
         //                               new LookupItem() { ValueMember = "2", DisplayMember = "2"},
         //                               new LookupItem() { ValueMember = "3", DisplayMember = "3" }};

         DataTable dtRateList = new CODW().ListLookUpEdit("40072" , "40072_RATE");
         rateLookUpEdit = new RepositoryItemLookUpEdit();
         rateLookUpEdit.SetColumnLookUp(dtRateList , "CODW_ID" , "CODW_DESC" , TextEditStyles.DisableTextEditor , null);
         gcMain.RepositoryItems.Add(rateLookUpEdit);
         RATE_INPUT.ColumnEdit = rateLookUpEdit;

         //級距下拉選單
         //期貨
         prodTypeLookUpEdit1 = new RepositoryItemLookUpEdit();
         prodTypeLookUpEdit1.SetColumnLookUp(dtFLevel , "MGRT1_LEVEL" , "MGRT1_LEVEL_NAME" , TextEditStyles.DisableTextEditor , null);
         gcDetail.RepositoryItems.Add(prodTypeLookUpEdit1);
         //選擇權
         prodTypeLookUpEdit2 = new RepositoryItemLookUpEdit();
         prodTypeLookUpEdit2.SetColumnLookUp(dtOLevel , "MGRT1_LEVEL" , "MGRT1_LEVEL_NAME" , TextEditStyles.DisableTextEditor , null);
         gcDetail.RepositoryItems.Add(prodTypeLookUpEdit2);
         #endregion

         //預設新增一筆設定資料
         InsertRow();


         //txtSDate.Focus();
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnInsert.Enabled = true;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
         _ToolBtnSave.Enabled = true;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
         _ToolBtnDel.Enabled = true;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

         _ToolBtnRetrieve.Enabled = true;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
         _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

         _ToolBtnImport.Enabled = false;//匯入
         _ToolBtnExport.Enabled = false;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
         _ToolBtnPrintAll.Enabled = true;//列印

         return ResultStatus.Success;
      }

      protected override ResultStatus Retrieve() {

         try {
            //清空Grid
            gcMain.DataSource = null;
            gcDetail.DataSource = null;

            //日期檢核
            if (txtSDate.Text == "1901/01/01") {
               MessageDisplay.Error("請輸入交易日期");
               return ResultStatus.Fail;
            }
            int currRow = 0;
            string kindID = "", abType, stockID = "";
            //讀取資料
            ymd = txtSDate.DateTimeValue.ToString("yyyyMMdd");
            DataTable dtMGD2 = dao40071.d_40071(ymd , isAdjType);
            if (dtMGD2.Rows.Count == 0) {
               MessageDisplay.Info("無任何資料！");
               gcMain.DataSource = dao40071.d_40071();
               //若無資料，預設新增一筆設定資料
               InsertRow();
               return ResultStatus.Fail;
            }
            dtMGD2 = dtMGD2.Sort("mgd2_stock_id,mgd2_kind_id");

            //準備兩個空的table給兩個grid
            DataTable dtInput = dao40071.d_40071();
            DataTable dtDetail = dao40071.d_40071_detail();
            dtDetail.Columns["ADJ_TYPE"].ColumnName = "OP_TYPE";
            dtDetail.Columns["DATA_YMD"].ColumnName = "YMD";

            //依條件將讀取來的資料分配給兩個grid
            foreach (DataRow dr in dtMGD2.Rows) {
               if (stockID != dr["MGD2_STOCK_ID"].AsString()) {
                  stockID = dr["MGD2_STOCK_ID"].AsString();
                  currRow = dtInput.Rows.Count;
                  dtInput.Rows.Add();
                  dtInput.Rows[currRow]["STOCK_ID"] = stockID;
                  dtInput.Rows[currRow]["RATE"] = dr["MGD2_ADJ_RATE"].AsDecimal() + 1;
                  dtInput.Rows[currRow]["PUB_YMD"] = dr["MGD2_PUB_YMD"];
                  dtInput.Rows[currRow]["IMPL_BEGIN_YMD"] = dr["MGD2_IMPL_BEGIN_YMD"];
                  dtInput.Rows[currRow]["IMPL_END_YMD"] = dr["MGD2_IMPL_END_YMD"];

                  dtInput.Rows[currRow]["ISSUE_BEGIN_YMD"] = dr["MGD2_ISSUE_BEGIN_YMD"];
                  dtInput.Rows[currRow]["ISSUE_END_YMD"] = dr["MGD2_ISSUE_END_YMD"];
                  dtInput.Rows[currRow]["YMD"] = dr["MGD2_YMD"];
               }
               if (kindID != dr["MGD2_KIND_ID"].AsString()) {
                  kindID = dr["MGD2_KIND_ID"].AsString();
                  currRow = dtDetail.Rows.Count;
                  dtDetail.Rows.Add();
                  dtDetail.Rows[currRow]["PROD_TYPE"] = dr["MGD2_PROD_TYPE"];
                  dtDetail.Rows[currRow]["KIND_ID"] = dr["MGD2_KIND_ID"];
                  dtDetail.Rows[currRow]["STOCK_ID"] = dr["MGD2_STOCK_ID"];
                  dtDetail.Rows[currRow]["ADJ_RATE"] = dr["MGD2_ADJ_RATE"].AsDecimal() + 1;
                  dtDetail.Rows[currRow]["DATA_FLAG"] = "Y";

                  dtDetail.Rows[currRow]["PROD_SUBTYPE"] = dr["MGD2_PROD_SUBTYPE"];
                  dtDetail.Rows[currRow]["PARAM_KEY"] = dr["MGD2_PARAM_KEY"];
                  dtDetail.Rows[currRow]["M_CUR_LEVEL"] = dr["MGD2_CUR_LEVEL"];
                  dtDetail.Rows[currRow]["CURRENCY_TYPE"] = dr["MGD2_CURRENCY_TYPE"];
                  dtDetail.Rows[currRow]["SEQ_NO"] = dr["MGD2_SEQ_NO"];

                  dtDetail.Rows[currRow]["OSW_GRP"] = dr["MGD2_OSW_GRP"];
                  dtDetail.Rows[currRow]["AMT_TYPE"] = dr["MGD2_AMT_TYPE"];
                  dtDetail.Rows[currRow]["ISSUE_BEGIN_YMD"] = dr["MGD2_ISSUE_BEGIN_YMD"];
                  dtDetail.Rows[currRow]["ISSUE_END_YMD"] = dr["MGD2_ISSUE_END_YMD"];
                  dtDetail.Rows[currRow]["IMPL_BEGIN_YMD"] = dr["MGD2_IMPL_BEGIN_YMD"];

                  dtDetail.Rows[currRow]["IMPL_END_YMD"] = dr["MGD2_IMPL_END_YMD"];
                  dtDetail.Rows[currRow]["PUB_YMD"] = dr["MGD2_PUB_YMD"];
                  dtDetail.Rows[currRow]["YMD"] = dr["MGD2_YMD"];
                  dtDetail.Rows[currRow]["OP_TYPE"] = " "; //預設為空格
               }
               if (dr["MGD2_AB_TYPE"].AsString() == "B") {
                  dtDetail.Rows[currRow]["CM_CUR_B"] = dr["MGD2_CUR_CM"];
                  dtDetail.Rows[currRow]["MM_CUR_B"] = dr["MGD2_CUR_MM"];
                  dtDetail.Rows[currRow]["IM_CUR_B"] = dr["MGD2_CUR_IM"];
                  dtDetail.Rows[currRow]["CM_B"] = dr["MGD2_CM"];
                  dtDetail.Rows[currRow]["MM_B"] = dr["MGD2_MM"];

                  dtDetail.Rows[currRow]["IM_B"] = dr["MGD2_IM"];
               } else {
                  dtDetail.Rows[currRow]["CM_CUR_A"] = dr["MGD2_CUR_CM"];
                  dtDetail.Rows[currRow]["MM_CUR_A"] = dr["MGD2_CUR_MM"];
                  dtDetail.Rows[currRow]["IM_CUR_A"] = dr["MGD2_CUR_IM"];
                  dtDetail.Rows[currRow]["CM_A"] = dr["MGD2_CM"];
                  dtDetail.Rows[currRow]["MM_A"] = dr["MGD2_MM"];

                  dtDetail.Rows[currRow]["IM_A"] = dr["MGD2_IM"];
               }
            }//foreach (DataRow dr in dtMGD2.Rows)

            //資料繫結
            gcMain.DataSource = dtInput;
            gcDetail.DataSource = dtDetail;

            //若無資料，預設新增一筆設定資料
            if (gvDetail.RowCount == 0) {
               InsertRow();
            }

         } catch (Exception ex) {
            MessageDisplay.Error("讀取錯誤");
            throw ex;
         }

         return ResultStatus.Success;
      }

      protected override ResultStatus InsertRow() {
         base.InsertRow(gvMain);

         return ResultStatus.Success;
      }

      protected override ResultStatus DeleteRow() {
         base.DeleteRow(gvMain);

         return ResultStatus.Success;
      }

      protected override ResultStatus Save(PokeBall pokeBall) {
         try {
            if (gvDetail.RowCount == 0) {
               MessageDisplay.Info("沒有變更資料,不需要存檔!");
               return ResultStatus.Fail;
            }
            #region ue_save_before
            gvMain.CloseEditor();
            gvMain.UpdateCurrentRow();
            gvDetail.CloseEditor();
            gvDetail.UpdateCurrentRow();

            string stockID, ymd, kindID, adjTypeName, opType, dbname, flag;
            string issueBeginYmd, issueEndYmd, implBeginYmd, implEndYmd, pubYmd, tradeYmd, mocfYmd, nextYmd;
            int found, count, row, col, currRow;
            decimal ldblRate;
            DateTime ldtWTIME = DateTime.Now;

            DataTable dtGrid = (DataTable)gcDetail.DataSource;
            found = dtGrid.Rows.IndexOf(dtGrid.Select("OP_TYPE <> ' '").FirstOrDefault());
            if (found == -1) {
               MessageDisplay.Warning("沒有變更資料,不需要存檔!");
               return ResultStatus.Fail;
            }

            if (dtGrid.Rows.Count == 0) {
               MessageDisplay.Warning("無明細資料，請重新產生明細");
               return ResultStatus.FailButNext;
            }

            DataTable dtMGD2; //ids_mgd2
            DataTable dtMGD2Log = dao40071.d_40071_log(); //ids_old 
            dtMGD2Log.Clear(); //只取schema

            for (int f = 0 ; f < dtGrid.Rows.Count ; f++) {
               if (dtGrid.Rows[f].RowState == DataRowState.Deleted) continue;
               DataRow dr = dtGrid.Rows[f];
               opType = dr["OP_TYPE"].ToString();
               flag = dr["DATA_FLAG"].AsString();
               stockID = dr["STOCK_ID"].AsString();

               //檢查同一標的的級距是否一致
               if ((f + 1) < dtGrid.Rows.Count) {
                  if (stockID == dtGrid.Rows[f + 1]["STOCK_ID"].AsString() &&
                      dr["M_CUR_LEVEL"].AsString() != dtGrid.Rows[f + 1]["M_CUR_LEVEL"].AsString()) {
                     MessageDisplay.Error(stockID + "的級距不一致");
                     return ResultStatus.FailButNext;
                  }
               }

               //檢查有異動的資料
               if (opType != " ") {
                  kindID = dr["KIND_ID"].AsString();
                  ymd = dr["YMD"].ToString();
                  issueBeginYmd = dr["ISSUE_BEGIN_YMD"].ToString();
                  issueEndYmd = dr["ISSUE_END_YMD"].ToString();
                  implBeginYmd = dr["IMPL_BEGIN_YMD"].ToString();
                  implEndYmd = dr["IMPL_END_YMD"].ToString();
                  pubYmd = dr["PUB_YMD"].ToString();

                  if (ymd != implBeginYmd) {
                     DialogResult result = MessageDisplay.Choose(stockID + "," + kindID + "交易日不等於處置起日，請問是否更新");
                     if (result == DialogResult.No) return ResultStatus.FailButNext;
                  }
                  if (issueEndYmd != implEndYmd) {
                     DialogResult result = MessageDisplay.Choose(stockID + "," + kindID + "生效迄日不等於處置迄日，請問是否更新");
                     if (result == DialogResult.No) return ResultStatus.FailButNext;
                  }

                  //處置期間首日+1個月
                  mocfYmd = PbFunc.relativedate(implBeginYmd.AsDateTime("yyyyMMdd") , 30).ToString("yyyyMMdd");

                  /*次一營業日*/
                  nextYmd = daoMOCF.GetNextTradeDay(implBeginYmd , mocfYmd);
                  if (issueBeginYmd != nextYmd) {
                     DialogResult result = MessageDisplay.Choose(stockID + "," + kindID + "生效起日不等於處置起日之次一營業日，請問是否更新");
                     if (result == DialogResult.No) return ResultStatus.FailButNext;
                  }

                  dtMGD2 = dao40072.d_40072(ymd , isAdjType , stockID);

                  //資料修改，將修改前舊資料寫入log
                  if (opType == "U") {
                     dtMGD2.Filter("mgd2_kind_id = '" + kindID + "'");
                     foreach (DataRow drU in dtMGD2.Rows) {
                        currRow = dtMGD2Log.Rows.Count;
                        dtMGD2Log.Rows.Add();
                        for (col = 0 ; col < dtMGD2.Columns.Count ; col++) {
                           //先取欄位名稱，因為兩張table欄位順序不一致
                           dbname = dtMGD2.Columns[col].ColumnName;
                           if (dbname == "CPSORT") continue; //這個欄位是拿來排序用的，故無需複製
                           dtMGD2Log.Rows[currRow][dbname] = drU[col];
                        }
                        if (flag == "Y") dtMGD2Log.Rows[currRow]["MGD2_L_TYPE"] = "U";
                        if (flag == "N") dtMGD2Log.Rows[currRow]["MGD2_L_TYPE"] = "D";
                        dtMGD2Log.Rows[currRow]["MGD2_L_USER_ID"] = GlobalInfo.USER_ID;
                        dtMGD2Log.Rows[currRow]["MGD2_L_TIME"] = ldtWTIME;
                     }
                  }

                  /******************************************
                     確認商品是否在同一交易日不同情境下設定過
                  ******************************************/
                  DataTable dtSet = dao40071.IsSetOnSameDay(kindID , ymd , isAdjType);
                  if (dtSet.Rows.Count == 0) {
                     MessageDisplay.Info("MGD2 " + kindID + " 無任何資料！");
                     return ResultStatus.FailButNext;
                  }
                  count = dtSet.Rows[0]["LI_COUNT"].AsInt();
                  adjTypeName = dtSet.Rows[0]["LS_ADJ_TYPE_NAME"].AsString();
                  if (count > 0) {
                     MessageDisplay.Error(kindID + ",交易日(" + ymd + ")在" + adjTypeName + "已有資料");
                     return ResultStatus.FailButNext;
                  }

                  /*********************************
                  確認商品是否在同一生效日區間設定過
                  生效起日若與生效迄日相同，不重疊
                  ex: 10/11的至10/31一般交易時段結束止，10/30的從10/31一般交易時段結束後始>>應不重疊
                  *************************************/
                  dtSet = dao40071.IsSetInSameSession(kindID , ymd , issueBeginYmd , issueEndYmd);
                  count = dtSet.Rows[0]["LI_COUNT"].AsInt();
                  adjTypeName = dtSet.Rows[0]["LS_ADJ_TYPE_NAME"].AsString();
                  tradeYmd = dtSet.Rows[0]["LS_TRADE_YMD"].AsString();
                  if (count > 0) {
                     MessageDisplay.Error(kindID + "," + adjTypeName + ",交易日(" + tradeYmd + ")在同一生效日區間內已有資料");
                     return ResultStatus.FailButNext;
                  }

                  //判斷調整幅度是否為0
                  ldblRate = dr["ADJ_RATE"].AsDecimal();
                  if (ldblRate == 0) {
                     MessageDisplay.Error("商品調整幅度不可為0");
                     return ResultStatus.FailButNext;
                  }

               }//if (ls_op_type != " ")
            }//for (int f = 0; f < dtGrid.Rows.Count; f++)
            #endregion
            string prodType;

            DataTable dtTemp = dao40072.d_40072(); //ids_tmp
            foreach (DataRow dr in dtGrid.Rows) {
               if (dr.RowState == DataRowState.Deleted) continue;
               opType = dr["OP_TYPE"].ToString();
               //只更新有異動的資料
               if (opType != " ") {
                  kindID = dr["KIND_ID"].AsString();
                  stockID = dr["STOCK_ID"].AsString();
                  issueBeginYmd = dr["ISSUE_BEGIN_YMD"].ToString();
                  issueEndYmd = dr["ISSUE_END_YMD"].ToString();
                  implBeginYmd = dr["IMPL_BEGIN_YMD"].ToString();
                  implEndYmd = dr["IMPL_END_YMD"].ToString();
                  pubYmd = dr["PUB_YMD"].ToString();
                  ymd = dr["YMD"].ToString();
                  ldblRate = dr["ADJ_RATE"].AsDecimal();

                  //刪除已存在資料
                  if (daoMGD2.DeleteMGD2(ymd , isAdjType , stockID , kindID) < 0) {
                     MessageDisplay.Error("MGD2資料刪除失敗");
                     return ResultStatus.FailButNext;
                  }

                  if (dr["DATA_FLAG"].AsString() == "Y") {
                     currRow = dtTemp.Rows.Count;
                     prodType = dr["PROD_TYPE"].AsString();
                     dtTemp.Rows.Add();
                     dtTemp.Rows[currRow]["MGD2_YMD"] = ymd;
                     dtTemp.Rows[currRow]["MGD2_PROD_TYPE"] = prodType;
                     dtTemp.Rows[currRow]["MGD2_KIND_ID"] = kindID;
                     dtTemp.Rows[currRow]["MGD2_STOCK_ID"] = stockID;
                     dtTemp.Rows[currRow]["MGD2_ADJ_TYPE"] = isAdjType;

                     dtTemp.Rows[currRow]["MGD2_ADJ_RATE"] = ldblRate;
                     dtTemp.Rows[currRow]["MGD2_ADJ_CODE"] = "Y";
                     dtTemp.Rows[currRow]["MGD2_ISSUE_BEGIN_YMD"] = issueBeginYmd;
                     dtTemp.Rows[currRow]["MGD2_ISSUE_END_YMD"] = issueEndYmd;
                     dtTemp.Rows[currRow]["MGD2_IMPL_BEGIN_YMD"] = implBeginYmd;

                     dtTemp.Rows[currRow]["MGD2_IMPL_END_YMD"] = implEndYmd;
                     dtTemp.Rows[currRow]["MGD2_PUB_YMD"] = pubYmd;
                     dtTemp.Rows[currRow]["MGD2_PROD_SUBTYPE"] = dr["PROD_SUBTYPE"];
                     dtTemp.Rows[currRow]["MGD2_PARAM_KEY"] = dr["PARAM_KEY"];
                     dtTemp.Rows[currRow]["MGD2_CUR_CM"] = dr["CM_CUR_A"];

                     dtTemp.Rows[currRow]["MGD2_CUR_MM"] = dr["MM_CUR_A"];
                     dtTemp.Rows[currRow]["MGD2_CUR_IM"] = dr["IM_CUR_A"];
                     dtTemp.Rows[currRow]["MGD2_CUR_LEVEL"] = dr["M_CUR_LEVEL"];
                     dtTemp.Rows[currRow]["MGD2_CM"] = dr["CM_A"];
                     dtTemp.Rows[currRow]["MGD2_MM"] = dr["MM_A"];

                     dtTemp.Rows[currRow]["MGD2_IM"] = dr["IM_A"];
                     dtTemp.Rows[currRow]["MGD2_CURRENCY_TYPE"] = dr["CURRENCY_TYPE"];
                     dtTemp.Rows[currRow]["MGD2_SEQ_NO"] = dr["SEQ_NO"];
                     dtTemp.Rows[currRow]["MGD2_OSW_GRP"] = dr["OSW_GRP"];
                     dtTemp.Rows[currRow]["MGD2_AMT_TYPE"] = dr["AMT_TYPE"];

                     dtTemp.Rows[currRow]["MGD2_W_TIME"] = ldtWTIME;
                     dtTemp.Rows[currRow]["MGD2_W_USER_ID"] = GlobalInfo.USER_ID;

                     /******************************
                           AB TYTPE：	-期貨
                                       A選擇權A值
                                       B選擇權B值
                     *******************************/
                     if (prodType == "F") {
                        dtTemp.Rows[currRow]["MGD2_AB_TYPE"] = "-";
                     } else {
                        dtTemp.Rows[currRow]["MGD2_AB_TYPE"] = "A";
                        //複製一筆一樣的，AB Type分開存
                        dtTemp.ImportRow(dtTemp.Rows[currRow]);
                        //dtTemp.Rows.Add(dtTemp.Rows[ii_curr_row]);//會跳錯
                        currRow = dtTemp.Rows.Count - 1;
                        dtTemp.Rows[currRow]["MGD2_AB_TYPE"] = "B";
                        dtTemp.Rows[currRow]["MGD2_CUR_CM"] = dr["CM_CUR_B"];
                        dtTemp.Rows[currRow]["MGD2_CUR_MM"] = dr["MM_CUR_B"];
                        dtTemp.Rows[currRow]["MGD2_CUR_IM"] = dr["IM_CUR_B"];
                        dtTemp.Rows[currRow]["MGD2_CM"] = dr["CM_B"];

                        dtTemp.Rows[currRow]["MGD2_MM"] = dr["MM_B"];
                        dtTemp.Rows[currRow]["MGD2_IM"] = dr["IM_B"];
                     }
                  }//if (dr["DATA_FLAG"].AsString()=="Y")
               }//if (ls_op_type != " ")
            }//foreach (DataRow dr in dtGrid.Rows)

            //Update DB
            //ids_tmp.update()
            if (dtTemp.Rows.Count > 0) {
               ResultData myResultData = daoMGD2.UpdateMGD2(dtTemp);
               if (myResultData.Status == ResultStatus.Fail) {
                  MessageDisplay.Error("更新資料庫MGD2錯誤! ");
                  return ResultStatus.FailButNext;
               }
            }
            //ids_old.update()
            if (dtMGD2Log.Rows.Count > 0) {
               ResultData myResultData = daoMGD2L.UpdateMGD2L(dtMGD2Log);
               if (myResultData.Status == ResultStatus.Fail) {
                  MessageDisplay.Error("更新資料庫MGD2L錯誤! ");
                  return ResultStatus.FailButNext;
               }
            }
            //Write LOGF
            WriteLog("變更資料 " , "Info" , "I");
            //報表儲存pdf
            ReportHelper _ReportHelper = new ReportHelper(gcDetail , _ProgramID , this.Text);
            CommonReportLandscapeA3 reportLandscape = new CommonReportLandscapeA3();//設定為橫向列印
            reportLandscape.printableComponentContainerMain.PrintableComponent = gcDetail;
            reportLandscape.IsHandlePersonVisible = false;
            reportLandscape.IsManagerVisible = false;
            _ReportHelper.Create(reportLandscape);
            _ReportHelper.Export(FileType.PDF , _ReportHelper.FilePath);
            MessageDisplay.Info("報表儲存完成!");
         } catch (Exception ex) {
            MessageDisplay.Error("儲存錯誤");
            throw ex;
         }
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper) {
         try {
            ReportHelper _ReportHelper = new ReportHelper(gcDetail , _ProgramID , this.Text);
            CommonReportLandscapeA3 reportLandscape = new CommonReportLandscapeA3();//設定為橫向列印
            reportLandscape.printableComponentContainerMain.PrintableComponent = gcDetail;
            reportLandscape.IsHandlePersonVisible = false;
            reportLandscape.IsManagerVisible = false;
            _ReportHelper.Create(reportLandscape);

            _ReportHelper.Print();//如果有夜盤會特別標註
            _ReportHelper.Export(FileType.PDF , _ReportHelper.FilePath);
            MessageDisplay.Info("報表儲存完成!");
            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
      }

      #region GridView Events
      /// <summary>
      /// 級距下拉選單根據商品類別轉換的事件
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void gvDetail_CustomRowCellEdit(object sender , CustomRowCellEditEventArgs e) {
         GridView gv = sender as GridView;
         gv.CloseEditor();
         gv.UpdateCurrentRow();
         if (e.Column.FieldName == "M_CUR_LEVEL") {
            string prodType = gv.GetRowCellValue(e.RowHandle , "PROD_TYPE").AsString();
            if (prodType == "F") e.RepositoryItem = prodTypeLookUpEdit1;
            if (prodType == "O") e.RepositoryItem = prodTypeLookUpEdit2;
         }
      }

      private void gvDetail_CellValueChanged(object sender , CellValueChangedEventArgs e) {
         GridView gv = sender as GridView;
         if (e.Column.Name != "OP_TYPE") {
            //如果OP_TYPE是I則固定不變
            if (gv.GetRowCellValue(e.RowHandle , "OP_TYPE").ToString() == " ") gv.SetRowCellValue(e.RowHandle , "OP_TYPE" , "U");
         }
      }

      private void gvDetail_CellValueChanging(object sender , CellValueChangedEventArgs e) {
         GridView gv = sender as GridView;
         if (e.Column.Name == "M_CUR_LEVEL") {
            //如果改變級距
            string level = e.Value.AsString();
            if (gv.GetRowCellValue(e.RowHandle , "PROD_TYPE").AsString() == "F") {
               DataRow dr = dtFLevel.Select("mgrt1_level = '" + level + "'")[0];
               gv.SetRowCellValue(e.RowHandle , "CM_CUR_A" , dr["MGRT1_CM_RATE"]);
               gv.SetRowCellValue(e.RowHandle , "MM_CUR_A" , dr["MGRT1_MM_RATE"]);
               gv.SetRowCellValue(e.RowHandle , "IM_CUR_A" , dr["MGRT1_IM_RATE"]);
               if (gv.GetRowCellValue(e.RowHandle , "CM_CUR_B") != DBNull.Value) {
                  gv.SetRowCellValue(e.RowHandle , "CM_CUR_B" , dr["MGRT1_CM_RATE_B"]);
               }
               if (gv.GetRowCellValue(e.RowHandle , "MM_CUR_B") != DBNull.Value) {
                  gv.SetRowCellValue(e.RowHandle , "MM_CUR_B" , dr["MGRT1_MM_RATE_B"]);
               }
               if (gv.GetRowCellValue(e.RowHandle , "IM_CUR_B") != DBNull.Value) {
                  gv.SetRowCellValue(e.RowHandle , "IM_CUR_B" , dr["MGRT1_IM_RATE_B"]);
               }
            }
            if (gv.GetRowCellValue(e.RowHandle , "PROD_TYPE").AsString() == "O") {
               DataRow dr = dtOLevel.Select("mgrt1_level = '" + level + "'")[0];
               gv.SetRowCellValue(e.RowHandle , "CM_CUR_A" , dr["MGRT1_CM_RATE"]);
               gv.SetRowCellValue(e.RowHandle , "MM_CUR_A" , dr["MGRT1_MM_RATE"]);
               gv.SetRowCellValue(e.RowHandle , "IM_CUR_A" , dr["MGRT1_IM_RATE"]);
               if (gv.GetRowCellValue(e.RowHandle , "CM_CUR_B") != DBNull.Value) {
                  gv.SetRowCellValue(e.RowHandle , "CM_CUR_B" , dr["MGRT1_CM_RATE_B"]);
               }
               if (gv.GetRowCellValue(e.RowHandle , "MM_CUR_B") != DBNull.Value) {
                  gv.SetRowCellValue(e.RowHandle , "MM_CUR_B" , dr["MGRT1_MM_RATE_B"]);
               }
               if (gv.GetRowCellValue(e.RowHandle , "IM_CUR_B") != DBNull.Value) {
                  gv.SetRowCellValue(e.RowHandle , "IM_CUR_B" , dr["MGRT1_IM_RATE_B"]);
               }
            }
         }//if (e.Column.Name == "M_CUR_LEVEL")
      }

      /// <summary>
      /// 期貨的保證金B值不能key
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void gvDetail_ShowingEditor(object sender , CancelEventArgs e) {
         GridView gv = sender as GridView;
         string prodType = gv.GetRowCellValue(gv.FocusedRowHandle , "PROD_TYPE").ToString();
         //string stock_id = gv.GetRowCellValue(gv.FocusedRowHandle, "STOCK_ID").AsString();
         if (gv.FocusedColumn.Name == "CM_CUR_B" ||
             gv.FocusedColumn.Name == "MM_CUR_B" ||
             gv.FocusedColumn.Name == "IM_CUR_B" ||
             gv.FocusedColumn.Name == "CM_B" ||
             gv.FocusedColumn.Name == "MM_B" ||
             gv.FocusedColumn.Name == "IM_B") {
            e.Cancel = prodType == "F" ? true : false;
            //e.Cancel = stock_id == null ? true : false;
         }
      }

      private void gvDetail_RowCellStyle(object sender , RowCellStyleEventArgs e) {
         GridView gv = sender as GridView;
         string amtType = gv.GetRowCellValue(e.RowHandle , gv.Columns["AMT_TYPE"]).AsString();

         switch (e.Column.FieldName) {
            case "KIND_ID":
            case "STOCK_ID":
            case "IMPL_BEGIN_YMD":
            case "IMPL_END_YMD":
            case "ISSUE_BEGIN_YMD":
            case "ISSUE_END_YMD":
            case "PUB_YMD":
            case "YMD":
            case "ADJ_RATE":
               e.Appearance.BackColor = Color.FromArgb(224 , 224 , 224);
               break;
            case "CM_CUR_A":
            case "CM_CUR_B":
            case "MM_CUR_A":
            case "MM_CUR_B":
            case "IM_CUR_A":
            case "IM_CUR_B":
            case "CM_A":
            case "CM_B":
            case "MM_A":
            case "MM_B":
            case "IM_A":
            case "IM_B":
               e.Column.DisplayFormat.FormatString = amtType == "P" ? "{0:0.###%}" : "#,###";
               break;
         }
      }

      /// <summary>
      /// 只要gvMain有異動，gvDetail就要清空
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void gvMain_CellValueChanging(object sender , CellValueChangedEventArgs e) {

         gcDetail.DataSource = null;
      }
      #endregion

      /// <summary>
      /// 全選
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnAll_Click(object sender , EventArgs e) {
         for (int f = 0 ; f < gvDetail.RowCount ; f++) {
            gvDetail.SetRowCellValue(f , "DATA_FLAG" , "Y");
            if (gvDetail.GetRowCellValue(f , "OP_TYPE").ToString() != "I") {
               gvDetail.SetRowCellValue(f , "OP_TYPE" , "U");
            }
         }
      }

      /// <summary>
      /// 不全選
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnNone_Click(object sender , EventArgs e) {
         for (int f = 0 ; f < gvDetail.RowCount ; f++) {
            gvDetail.SetRowCellValue(f , "DATA_FLAG" , "N");
            if (gvDetail.GetRowCellValue(f , "OP_TYPE").ToString() != "I") {
               gvDetail.SetRowCellValue(f , "OP_TYPE" , "U");
            }
         }
      }

      /// <summary>
      /// 顯示明細
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnDetail_Click(object sender , EventArgs e) {

         //重設gridview
         gcDetail.DataSource = null;

         int row, col, found;
         string prodType, prodTypeName, kindID, stockID, paramKey, abroad, implBeginYmd, issueBeginYmd, mocfYmd;
         string opType;
         decimal ldcRate;
         gvMain.CloseEditor();
         gvMain.UpdateCurrentRow();

         DataTable dtGrid = dao40071.d_40071_detail(); //ids_tmp 空的，拿來重置gcDetail 
         dtGrid.Columns["ADJ_TYPE"].ColumnName = "OP_TYPE";
         dtGrid.Columns["DATA_YMD"].ColumnName = "YMD";
         //dtTemp.Columns["CM_A * NVL(MGT6_REF_XXX,1)"].ColumnName = "CM_A"; 沒成功撈到資料的話欄位名稱不會變?
         DataTable dtMGD2 = dao40072.d_40072(); //ids_mgd2 空的

         txtSDate.Text = "1901/01/01";
         prodType = "S";
         paramKey = "ST%";
         abroad = "%";
         kindID = "%";

         //產生明細檔
         DataTable dtInput = (DataTable)gcMain.DataSource;
         foreach (DataRow drInput in dtInput.Rows) {
            opType = "I";
            stockID = drInput["STOCK_ID"].ToString();
            implBeginYmd = drInput["IMPL_BEGIN_YMD"].ToString();

            //交易日為處置期間之首日
            drInput["YMD"] = implBeginYmd;

            dtMGD2 = dao40072.d_40072(drInput["YMD"].AsDateTime().ToString("yyyyMMdd") , isAdjType , stockID);
            if (dtMGD2.Rows.Count > 0) {
               DialogResult result = MessageDisplay.Choose(stockID + "資料已存在，是否重新產製資料,若不重產資料，請按「預覽」!");
               if (result == DialogResult.No) return;
               opType = "U";
            }

            //處置期間首日+1個月
            mocfYmd = PbFunc.relativedate(implBeginYmd.AsDateTime() , 30).ToString("yyyyMMdd");
            /*次一營業日*/
            implBeginYmd = implBeginYmd.AsDateTime().ToString("yyyyMMdd");
            issueBeginYmd = daoMOCF.GetNextTradeDay(implBeginYmd , mocfYmd);

            //終止生效日為處置期間迄日
            drInput["ISSUE_END_YMD"] = drInput["IMPL_END_YMD"];
            //開始生效日為處置期間首日之次一個營業日
            drInput["ISSUE_BEGIN_YMD"] = issueBeginYmd;

            //判斷是否有空值 
            for (col = 0 ; col < dtInput.Columns.Count ; col++) {
               if (dtInput.Columns[col].ColumnName == "CPSORT") continue; //這欄是排序用的毋須判斷
               if (drInput[col] == DBNull.Value || drInput[col].ToString() == "") {
                  MessageDisplay.Warning("請確認資料是否輸入完成!");
                  return;
               }
            }
            stockID = stockID + "%";

            //調整倍數(計算用1+調整倍數)
            ldcRate = drInput["RATE"].AsDecimal() - 1;

            //這邊才去讀SP
            DataTable dtTemp = dao40071.d_40071_detail(implBeginYmd , prodType , paramKey , abroad , kindID , stockID , ldcRate);
            dtTemp.Columns["ADJ_TYPE"].ColumnName = "OP_TYPE";
            dtTemp.Columns["DATA_YMD"].ColumnName = "YMD";
            if (dtTemp.Columns["CM_A*NVL(MGT6_REF_XXX,1)"] != null) dtTemp.Columns["CM_A*NVL(MGT6_REF_XXX,1)"].ColumnName = "CM_A"; //沒撈到值的話欄位名稱不會變，若資料為個股類也不會變
            foreach (DataRow drTemp in dtTemp.Rows) {
               drTemp["ISSUE_BEGIN_YMD"] = issueBeginYmd;
               drTemp["ISSUE_END_YMD"] = drInput["impl_end_ymd"];
               drTemp["IMPL_BEGIN_YMD"] = implBeginYmd;
               drTemp["IMPL_END_YMD"] = drInput["impl_end_ymd"];
               drTemp["PUB_YMD"] = drInput["pub_ymd"];
               drTemp["YMD"] = implBeginYmd;
               drTemp["OP_TYPE"] = opType;
               drTemp["ADJ_RATE"] = drInput["RATE"];
            }

            //將資料複製到明細表
            //dtGrid = dtTemp.Clone();
            foreach (DataRow drTemp in dtTemp.Rows) {
               dtGrid.ImportRow(drTemp);
            }
            dtGrid.AcceptChanges();
            gcDetail.DataSource = dtGrid;
         }//foreach (DataRow drInput in dtInput.Rows)

         //sort("stock_id A prod_type A ")
         if (dtGrid.Rows.Count != 0) {
            dtGrid = dtGrid.AsEnumerable().OrderBy(x => x.Field<string>("STOCK_ID"))
                    .ThenBy(x => x.Field<string>("PROD_TYPE"))
                    .CopyToDataTable();
         }
         gcDetail.DataSource = dtGrid;

         if (gvDetail.RowCount == 0) {
            MessageDisplay.Warning("無明細資料，請確認「交易日期」及「商品調整幅度」是否填寫正確!");
            return;
         }
      }
   }
}