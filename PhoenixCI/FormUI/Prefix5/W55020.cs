using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using DevExpress.XtraEditors.Controls;
using System;
using System.Data;
using System.Linq;

/// <summary>
/// Winni, 2019/04/25 (修改f_ocf_date -> GlobalInfo.OCF_DATE)
/// </summary>
namespace PhoenixCI.FormUI.Prefix5 {
   /// <summary>
   /// 55020 交易經手費收費明細表
   /// </summary>
   public partial class W55020 : FormParent {
      private D55020 dao55020;

      public W55020(string programID , string programName) : base(programID , programName) {
         InitializeComponent();

         this.Text = _ProgramID + "─" + _ProgramName;
         txtStartMonth.DateTimeValue = GlobalInfo.OCF_DATE;
         txtEndMonth.DateTimeValue = GlobalInfo.OCF_DATE;

         dao55020 = new D55020();
      }

      /// <summary>
      /// 視窗啟動時,設定一些UI元件初始值
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus Open() {
         base.Open();

         //1.設定初始年月yyyy/MM
         txtStartMonth.DateTimeValue = GlobalInfo.OCF_DATE;
         txtStartMonth.EnterMoveNextControl = true;
         txtStartMonth.Focus();

         txtEndMonth.DateTimeValue = GlobalInfo.OCF_DATE;
         txtEndMonth.EnterMoveNextControl = true;
         txtStartMonth.Focus();

         //2.設定下拉選單
         //2.1先讀取db
         DataTable dt = new ABRK().ListAll2();//第一行空白+ABRK_NO/ABRK_NAME/cp_display
         cbxFcmStartNo.SetDataTable(dt , "ABRK_NO","CP_DISPLAY2",TextEditStyles.DisableTextEditor," ");
         cbxFcmEndNo.SetDataTable(dt , "ABRK_NO" , "CP_DISPLAY2" , TextEditStyles.DisableTextEditor , " ");

         rgpType.SelectedIndex = 0;//直接預設為第一個選項
         rgpType_EditValueChanged(rgpType , null);//觸發事件
         rgpType.EnterMoveNextControl = true;

         DataTable dtProdType = new APDK().ListAll();//前面[全部/期貨/選擇權]+APDK_PROD_TYPE/PDK_KIND_ID
         cbxProdType.SetDataTable(dtProdType , "PDK_KIND_ID" , textEditStyles: TextEditStyles.DisableTextEditor);
         cbxProdType.ItemIndex = 0;//直接預設為[全部]


         return ResultStatus.Success;
      }

      /// <summary>
      /// 設定此功能哪些按鈕可以按
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnInsert.Enabled = false;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
         _ToolBtnSave.Enabled = false;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
         _ToolBtnDel.Enabled = false;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

         _ToolBtnRetrieve.Enabled = false;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
         _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

         _ToolBtnImport.Enabled = false;//匯入
         _ToolBtnExport.Enabled = true;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
         _ToolBtnPrintAll.Enabled = false;//列印

         return ResultStatus.Success;
      }

      /// <summary>
      /// 按下[讀取/預覽]按鈕時,去資料庫撈資料
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus Retrieve() {
         base.Retrieve();

         return ResultStatus.Success;
      }

      /// <summary>
      /// 按下[匯出]按鈕時
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus Export() {

         //0.將畫面資訊做些轉換
         string startNo = cbxFcmStartNo.EditValue.AsString("");
         string endNo = cbxFcmEndNo.EditValue.AsString("");


         //1.檢查
         //1.1期貨商後面號碼不能小於前面號碼
         //ken,注意,期貨商代號第一碼為英文,如果要比較字串大小,則要使用string.Compare
         if (startNo.Length > 0)
            if (endNo.Length > 0)
               if (startNo.CompareTo(endNo) > 0) {
                  MessageDisplay.Warning("造市者代號起始不可大於迄止");
                  cbxFcmStartNo.Focus();
                  return ResultStatus.Fail;
               }

         //2.get data
         DataTable dt;
         if (rgpType.SelectedIndex == 0)//依照期貨商別
            dt = dao55020.ListAll(txtStartMonth.FormatValue ,
                                    txtEndMonth.FormatValue ,
                                    startNo ,
                                    endNo ,
                                    cbxProdType.EditValue.AsString());
         else//依照商品別
            dt = dao55020.ListAll2(txtStartMonth.FormatValue ,
                                    txtEndMonth.FormatValue ,
                                    startNo ,
                                    endNo);

         if (dt.Rows.Count <= 0) {
            MessageDisplay.Info(string.Format("{0},{1},無任何資料!" , txtStartMonth.Text + "~" + txtEndMonth.Text , this.Text));
            return ResultStatus.Fail;
         }

         try {
            //3.開始轉出資料
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "訊息：資料轉出中........";

            //3.1 copy template xls to target path
            string excelDestinationPath = CopyExcelTemplateFile(_ProgramID , FileType.XLS);

            //3.2 open xls
            Workbook workbook = new Workbook();
            workbook.LoadDocument(excelDestinationPath);
            int sheetNo = rgpType.SelectedIndex;
            Worksheet worksheet = workbook.Worksheets[sheetNo];

            //3.3寫入檔頭
            worksheet.Cells[3 , 0].Value += System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            worksheet.Cells[3 , 3].Value += txtStartMonth.Text.Replace("/" , "");
            worksheet.Cells[3 , 5].Value += txtEndMonth.Text.Replace("/" , "");
            worksheet.Cells[3 , 7].Value = "資料更新時間 : " + dt.Rows[0]["FEETRD_UPD_TIME"].AsDateTime(System.DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss");

            //3.4寫入明細
            //ken,預先做好一堆空白行數,但是如果筆數超過預設空白行數,會把最後的統計那行覆蓋掉
            int rowIndex = 7;//起始row index position
            int pos = 0;
            int rowTotalCount = rowIndex + (rgpType.SelectedIndex == 0 ? 3000 : 200);//總行數(包含空白)
            foreach (DataRow row in dt.Rows) {
               if (pos % 20 == 0) {
                  labMsg.Text = string.Format("資料轉出中......{0} / {1}" , pos , dt.Rows.Count);
                  this.Refresh();
                  //Application.DoEvents();
               }

               if (rgpType.SelectedIndex == 0) {
                  //依照期貨商別
                  worksheet.Cells[rowIndex , 0].Value = row["feetrd_fcm_no"].AsString();//期貨商代號
                  worksheet.Cells[rowIndex , 1].Value = row["brk_abbr_name"].AsString();//期貨商名稱
                                                                                        //ken,商品名稱feetrd_kind_id不要用AsString,會把最前面的空白trim掉,excel總計有用到公式比對前面空白
                  worksheet.Cells[rowIndex , 2].Value = (!DBNull.Value.Equals(row["feetrd_kind_id"]) ? row["feetrd_kind_id"].ToString() : "");//商品名稱

                  worksheet.Cells[rowIndex , 3].Value = row["feetrd_m_qnty"].AsDouble();//成交口數
                  worksheet.Cells[rowIndex , 4].Value = row["feetrd_ar"].AsDecimal();//應收交易經手費
                  worksheet.Cells[rowIndex , 5].Value = row["feetrd_mk_disc_amt"].AsDecimal();//造市折減
                  worksheet.Cells[rowIndex , 6].Value = row["feetrd_oth_disc_amt"].AsDecimal();//其他折減
                  worksheet.Cells[rowIndex , 8].Value = row["feetrd_rec_amt"].AsDecimal();//金額
               } else {
                  //依照商品別
                  //ken,商品名稱feetrd_kind_id不要用AsString,會把最前面的空白trim掉,excel總計有用到公式比對前面空白
                  worksheet.Cells[rowIndex , 0].Value = (!DBNull.Value.Equals(row["feetrd_kind_id"]) ? row["feetrd_kind_id"].ToString() : "");//商品名稱

                  worksheet.Cells[rowIndex , 1].Value = row["feetrd_m_qnty"].AsDouble();//成交口數
                  worksheet.Cells[rowIndex , 2].Value = row["feetrd_ar"].AsDecimal();//應收交易經手費
                  worksheet.Cells[rowIndex , 3].Value = row["feetrd_mk_disc_amt"].AsDecimal();//造市折減
                  worksheet.Cells[rowIndex , 4].Value = row["feetrd_oth_disc_amt"].AsDecimal();//其他折減
                  worksheet.Cells[rowIndex , 6].Value = row["feetrd_rec_amt"].AsDecimal();//金額
               }

               rowIndex++; pos++;
            }//foreach (DataRow row in dt.Rows) {


            //刪除空白列
            if (rowIndex <= rowTotalCount) {
               worksheet.Rows.Remove(rowIndex , rowTotalCount - rowIndex);
            }

            worksheet.Range["A1"].Select();
            worksheet.ScrollToRow(0);

            //存檔
            workbook.SaveDocument(excelDestinationPath);
            return ResultStatus.Success;
         } catch (Exception ex) {
            PbFunc.f_write_logf(_ProgramID , "Error" , ex.Message);
         } finally {
            labMsg.Text = "";
            labMsg.Visible = false;
            panFilter.Enabled = true;
         }
         return ResultStatus.Fail;
      }


      private void rgpType_EditValueChanged(object sender , System.EventArgs e) {
         if (rgpType.SelectedIndex == 0) {
            labProdType.Visible = true;
            cbxProdType.Visible = true;
         } else {
            labProdType.Visible = false;
            cbxProdType.Visible = false;
         }
      }

   }
}
