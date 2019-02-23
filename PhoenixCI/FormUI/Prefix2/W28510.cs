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
/// Winni, 2019/01/18
/// </summary>
namespace PhoenixCI.FormUI.Prefix2 {
   /// <summary>
   /// 28510 每月買賣比重資料轉入(結6250)
   /// 有寫到的功能：Retrieve、Import、Run
   /// </summary>
   public partial class W28510 : FormParent {

      private AM2F daoAM2F;
      private D28510 dao28510;

      public W28510(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         GridHelper.SetCommonGrid(gvMain);
         daoAM2F = new AM2F();
         dao28510 = new D28510();
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
         daoAM2F = new AM2F();
         string as_ym = txtMonth.Text.Replace("/" , "");
         DataTable returnTable = daoAM2F.ListData(as_ym);
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
            open.FileName = "6250.txt";
            DialogResult openResult = open.ShowDialog();
            if (openResult == DialogResult.OK) {
               using (StreamReader sr = new StreamReader(open.FileName , System.Text.Encoding.Default)) {
                  string line;
                  while ((line = sr.ReadLine()) != null) {
                     string[] items = line.Split('\t');
                     daoAM2F = new AM2F();
                     string as_ym = txtMonth.Text.Replace("/" , "");
                     DataTable returnTable = daoAM2F.ListData(as_ym);

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
            if (gvMain.DataRowCount > 0) {
               DialogResult result = MessageBox.Show("資料年月(" + datadate + ")資料已存在,是否刪除?" , "注意" , MessageBoxButtons.YesNo , MessageBoxIcon.Question);
               if (result == DialogResult.No) {
                  return ResultStatus.Fail;
               } else {
                  dao28510.DeleteByDate(datadate);
               }
            }

            //4. 資料條件過濾
            for (int i = 0 ; i < dtReadTxt.Rows.Count ; i++) {
               if (String.IsNullOrEmpty(dtReadTxt.Rows[i]["AM2F_PC_CODE"].AsString())) {
                  string replaceEmpty = " ";
                  dtReadTxt.Rows[i]["AM2F_PC_CODE"] = replaceEmpty;
               }
               if (dtReadTxt.Rows[i]["AM2F_KIND_ID"].AsString() == "8888888") {
                  string replaceEmpty = "STO";
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

         ResultStatus status = base.Save_Override(dt , "AM2F");
         if (status == ResultStatus.Fail) {
            MessageBox.Show("資料庫寫入AM2F錯誤!" , "注意" , MessageBoxButtons.OK , MessageBoxIcon.Exclamation);
            return ResultStatus.Fail;
         }
         return ResultStatus.Success;
      }

      protected override ResultStatus Run(PokeBall args) {
         base.Run(args);

         //轉統計資料AM2 (此段不確定是否有確實執行到，要再確定)
         DataTable dt = (DataTable)gcMain.DataSource;
         DataTable reResult = dao28510.ExecuteStoredProcedure(txtMonth.Text.Replace("/" , ""));
         if (reResult.Rows.Count < 0) {
            MessageBox.Show("執行SP(ci.sp_H_stt_AM2)錯誤!" , "注意" , MessageBoxButtons.OK , MessageBoxIcon.Exclamation);
         } else {
            lblProcessing.Text = "轉檔完成!";
            MessageBox.Show("轉檔完成" , "訊息" , MessageBoxButtons.OK , MessageBoxIcon.Information);
         }
         lblProcessing.Visible = false;
         return ResultStatus.Success;
      }

   }
}