using BaseGround;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

//TODO: 確認SP執行是否有影響到CI.AB3資料表

/// <summary>
/// Winni, 2019/01/28
/// </summary>
namespace PhoenixCI.FormUI.Prefix2 {
   /// <summary>
   /// 28610 每日戶數資料轉入(結6240)
   /// 有寫到的功能：Retrieve、Import
   /// </summary>
   public partial class W28610 : FormParent {

      private AB1 daoAB1;
      private D28610 dao28610;

      public W28610(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         GridHelper.SetCommonGrid(gvMain);
         daoAB1 = new AB1();
         dao28610 = new D28610();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtMonth.DateTimeValue = GlobalInfo.OCF_DATE;
      }

      protected override ResultStatus Open() {
         base.Open();

         //隱藏一些開發用的資訊和測試按鈕
         if (!FlagAdmin) {
            btnSp.Visible = false;
         } else {
            btnSp.Visible = true; //功能尚未實作
         }

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
      /// 讀取查詢日期資料
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus Retrieve() {
         base.Retrieve();
         daoAB1 = new AB1();
         DateTime as_ym = txtMonth.DateTimeValue;
         DataTable returnTable = daoAB1.ListData(as_ym);
         if (returnTable.Rows.Count == 0) {
            MessageBox.Show("無任何資料" , "訊息" , MessageBoxButtons.OK , MessageBoxIcon.Information);
         }

         gcMain.DataSource = returnTable;
         gcMain.Visible = true;
         gcMain.Focus();

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
            open.FileName = "6240.txt";
            DialogResult openResult = open.ShowDialog();
            if (openResult == DialogResult.OK) {
               using (StreamReader sr = new StreamReader(open.FileName , System.Text.Encoding.Default)) {
                  string line;
                  while ((line = sr.ReadLine()) != null) {
                     string[] items = line.Split('\t');
                     daoAB1 = new AB1();
                     DateTime as_ym = txtMonth.DateTimeValue;
                     DataTable returnTable = daoAB1.ListData(as_ym);

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
            string datadate = dtReadTxt.Rows[0][5].AsString();
            if (datadate != txtMonth.Text) {
               DialogResult result = MessageBox.Show("資料年月(" + datadate + ")與畫面年月不同,是否將畫面改為資料年月?" , "注意" , MessageBoxButtons.YesNo , MessageBoxIcon.Question);
               if (result == DialogResult.No) {
                  lblProcessing.Visible = false;
                  return ResultStatus.Fail;
               } else {
                  txtMonth.Text = datadate;
                  Retrieve();
               }
            }

            //3.刪除舊有資料
            if (gvMain.DataRowCount > 0) {
               DialogResult result = MessageBox.Show("資料年月(" + datadate + ")資料已存在,是否刪除?" , "注意" , MessageBoxButtons.YesNo , MessageBoxIcon.Question);
               if (result == DialogResult.No) {
                  return ResultStatus.Fail;
               } else {
                  dao28610.DeleteByDate(datadate);
               }
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
                     bool rtn = dao28610.InsertAB1(ab1_acc_type , ab1_count , ab1_accu_count , ab1_trade_count , ab1_date);
                  }
               }
            } catch (Exception ex) {
               MessageBox.Show(ex.Message);
            }

            //4.轉統計資料AB3
            DataTable dt = (DataTable)gcMain.DataSource;
            DataTable reResult = dao28610.ExecuteStoredProcedure(txtMonth.DateTimeValue);
            if (reResult.Rows.Count < 0) {
               MessageBox.Show("執行SP(ci.sp_H_stt_AB3)錯誤!" , "注意" , MessageBoxButtons.OK , MessageBoxIcon.Exclamation);
            } else {
               lblProcessing.Text = "轉檔完成!";
               MessageBox.Show("轉檔完成" , "訊息" , MessageBoxButtons.OK , MessageBoxIcon.Information);
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