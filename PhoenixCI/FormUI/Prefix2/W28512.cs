using ActionService.DbDirect;
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
using System.Windows.Forms;

//TODO: 確認SP執行是否有影響到CI.AM2資料表

/// <summary>
/// Winni, 2019/01/28
/// </summary>
namespace PhoenixCI.FormUI.Prefix2 {
   /// <summary>
   /// 28512 每日買賣比重資料轉入(結6250政府基金(L))－日 
   /// 有寫到的功能：Retrieve、Import、Run
   /// </summary>
   public partial class W28512 : FormParent {

      private AM22F daoAM22F;
      private D28512 dao28512;

      public W28512(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         GridHelper.SetCommonGrid(gvMain);
         daoAM22F = new AM22F();
         dao28512 = new D28512();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtMonth.DateTimeValue = GlobalInfo.OCF_DATE;
      }

      protected override ResultStatus Open() {
         base.Open();

         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm() {

         _ToolBtnInsert.Enabled = false;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
         _ToolBtnSave.Enabled = false;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
         _ToolBtnDel.Enabled = false;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

         _ToolBtnRetrieve.Enabled = true;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
         _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

         _ToolBtnImport.Enabled = true;//匯入
         _ToolBtnExport.Enabled = false;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
         _ToolBtnPrintAll.Enabled = false;//列印_ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      /// <summary>
      /// 讀取查詢月份資料
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus Retrieve() {
         base.Retrieve();
         daoAM22F = new AM22F();
         string as_ym = txtMonth.Text.Replace("/" , "");
         DataTable returnTable = daoAM22F.ListData(as_ym);
         if (returnTable.Rows.Count == 0) {
            MessageBox.Show("無任何資料" , "訊息" , MessageBoxButtons.OK , MessageBoxIcon.Information);
         }

         gcMain.DataSource = returnTable;
         gcMain.Visible = true;
         gcMain.Focus(); base.Retrieve();

         return ResultStatus.Success;
      }

      protected override ResultStatus Import() {
         //base.Import();
         //1.讀檔並寫入DataTable
         try {
            lblProcessing.Visible = true;
            DataTable dtReadTxt = new DataTable();
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "*.txt (*.txt)|*.txt";
            open.Title = "請點選儲存檔案之目錄";
            open.FileName = "6250_政府基金(L)_日明細.txt";
            DialogResult openResult = open.ShowDialog();
            if (openResult == DialogResult.OK) {
               using (StreamReader sr = new StreamReader(open.FileName , System.Text.Encoding.Default)) {
                  string line;
                  while ((line = sr.ReadLine()) != null) {
                     string[] items = line.Split('\t');
                     daoAM22F = new AM22F();
                     string as_ym = txtMonth.Text.Replace("/" , "");
                     DataTable returnTable = daoAM22F.ListData(as_ym);

                     //填入欄位名稱
                     if (dtReadTxt.Columns.Count == 0) {
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
            //2.確認資料年月&畫面年月(讀取資料)
            string datadate = dtReadTxt.Rows[0][3].AsString();
            if (datadate != txtMonth.Text.Replace("/" , "")) {
               DialogResult result = MessageBox.Show("資料年月(" + datadate + ")與畫面年月不同,是否將畫面改為資料年月?" , "注意" , MessageBoxButtons.YesNo , MessageBoxIcon.Question);
               if (result == DialogResult.No) {
                  return ResultStatus.Fail;
               } else {
                  txtMonth.Text = datadate.SubStr(0 , 4) + "/" + datadate.SubStr(4 , 2);
                  Retrieve();
               }
            }

            //3.刪除舊有資料
            string ls_symd = "", ls_eymd = "";
            if (gvMain.DataRowCount > 0) {
               DialogResult result = MessageBox.Show("資料年月(" + datadate + ")資料已存在,是否刪除?" , "注意" , MessageBoxButtons.YesNo , MessageBoxIcon.Question);
               if (result == DialogResult.No) {
                  return ResultStatus.Fail;
               } else {
                  ls_symd = datadate + "01";
                  ls_eymd = datadate + "31";
                  datadate += "%";
                  dao28512.DeleteByDate(datadate);
               }
            }

            ls_symd = ls_eymd;
            ls_eymd = "";
            for (int i = 0 ; i < dtReadTxt.Rows.Count ; i++) {
               //找起始日期
               if (ls_symd.AsInt() > dtReadTxt.Rows[i]["AM2F_YMD"].AsInt()) {
                  ls_symd = dtReadTxt.Rows[i]["AM2F_YMD"].AsString();
               }

               //找終止日期
               if (ls_eymd.AsInt() < dtReadTxt.Rows[i]["AM2F_YMD"].AsInt()) {
                  ls_eymd = dtReadTxt.Rows[i]["AM2F_YMD"].AsString();
               }

               if (String.IsNullOrEmpty(dtReadTxt.Rows[i]["AM2F_PC_CODE"].AsString())) {
                  string replaceEmpty = " ";
                  dtReadTxt.Rows[i]["AM2F_PC_CODE"] = replaceEmpty;
               }
               if (dtReadTxt.Rows[i]["AM2F_KIND_ID"].AsString() == "8888888") {
                  string replaceEmpty = "STC";
                  dtReadTxt.Rows[i]["AM2F_PC_CODE"] = replaceEmpty;
               }
            }

            PokeBall pb = new PokeBall();
            gcMain.DataSource = dtReadTxt;
            Save(pb);
            Run(pb);

         } catch (Exception ex) {
            MessageBox.Show(ex.Message);
         }

         return ResultStatus.Success;
      }

      protected override ResultStatus Save(PokeBall pokeBall) {
         base.Save(gcMain);

         DataTable dt = (DataTable)gcMain.DataSource;

         //CI.AM22F在DB沒有PKey，依PB datawindow 設的Key去存檔
         //更新主要Table
         string tableName = "CI.AM22F";
         string keysColumnList = "AM2F_KIND_ID, AM2F_YMD, AM2F_MARKET_CODE";
         string insertColumnList = "AM2F_KIND_ID, AM2F_PC_CODE, AM2F_ACC_CODE, AM2F_SETTLE_MONTH1, AM2F_SETTLE_MONTH2, AM2F_SETTLE_MONTH3, " +
                                   "AM2F_SETTLE_MONTH4, AM2F_SETTLE_MONTH5, AM2F_SETTLE_MONTH6, AM2F_SETTLE_MONTH7, AM2F_SETTLE_MONTH8, " +
                                   "AM2F_SETTLE_MONTH9, AM2F_SETTLE_MONTH10, AM2F_SETTLE_MONTH11, AM2F_SETTLE_MONTH12, AM2F_SETTLE_MONTH13, " +
                                   "AM2F_BPOS1, AM2F_SPOS1, AM2F_L_BPOS1, AM2F_L_SPOS1, AM2F_BPOS2, AM2F_SPOS2, AM2F_L_BPOS2, AM2F_L_SPOS2, " +
                                   "AM2F_BPOS3, AM2F_SPOS3, AM2F_L_BPOS3, AM2F_L_SPOS3, AM2F_BPOS4, AM2F_SPOS4, AM2F_L_BPOS4, AM2F_L_SPOS4, " +
                                   "AM2F_BPOS5, AM2F_SPOS5, AM2F_L_BPOS5, AM2F_L_SPOS5, AM2F_BPOS6, AM2F_SPOS6, AM2F_L_BPOS6, AM2F_L_SPOS6, " +
                                   "AM2F_BPOS7, AM2F_SPOS7, AM2F_L_BPOS7, AM2F_L_SPOS7, AM2F_BPOS8, AM2F_SPOS8, AM2F_L_BPOS8, AM2F_L_SPOS8, " +
                                   "AM2F_BPOS9, AM2F_SPOS9, AM2F_L_BPOS9, AM2F_L_SPOS9, AM2F_BPOS10, AM2F_SPOS10, AM2F_L_BPOS10, AM2F_L_SPOS10, " +
                                   "AM2F_BPOS11, AM2F_SPOS11, AM2F_L_BPOS11, AM2F_L_SPOS11, AM2F_BPOS12, AM2F_SPOS12, AM2F_L_BPOS12, AM2F_L_SPOS12, " +
                                   "AM2F_BPOS13, AM2F_SPOS13, AM2F_L_BPOS13, AM2F_L_SPOS13, AM2F_YMD, AM2F_MARKET_CODE ";
         string updateColumnList = insertColumnList;
         try {
            //update to DB
            ServiceCommon serviceCommon = new ServiceCommon();
            ResultData ResultData = serviceCommon.SaveForChanged(dt , tableName , insertColumnList , updateColumnList , keysColumnList , pokeBall);

            return ResultStatus.Success;

         } catch (Exception ex) {
            MessageBox.Show(ex.Message);
            MessageBox.Show("資料庫寫入AM22F錯誤!" , "注意" , MessageBoxButtons.OK , MessageBoxIcon.Exclamation);
            return ResultStatus.Fail;
         }
      }

      protected override ResultStatus Run(PokeBall args) {
         base.Run(args);

         //轉統計資料AM2
         DataTable dt = (DataTable)gcMain.DataSource;
         DataTable reResult = dao28512.ExecuteStoredProcedure(txtMonth.Text.Replace("/" , ""));
         if (reResult.Rows.Count < 0) {
            MessageBox.Show("執行SP(ci.sp_H_stt_AM21_Day)錯誤!" , "注意" , MessageBoxButtons.OK , MessageBoxIcon.Exclamation);
         } else {
            lblProcessing.Text = "轉檔完成!";
            MessageBox.Show("轉檔完成" , "訊息" , MessageBoxButtons.OK , MessageBoxIcon.Information);
         }
         lblProcessing.Visible = false;
         return ResultStatus.Success;
      }

   }

}