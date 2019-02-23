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
/// ken,2019/1/7
/// </summary>
namespace PhoenixCI.FormUI.Prefix5 {
    /// <summary>
    /// 手續費收入彙總表
    /// </summary>
    public partial class W55050 : FormParent {
        private D55050 dao55050;

        public W55050(string programID, string programName) : base(programID, programName) {
            InitializeComponent();

            this.Text = _ProgramID + "─" + _ProgramName;
            txtStartMonth.DateTimeValue = GlobalInfo.OCF_DATE;
            txtEndMonth.DateTimeValue = GlobalInfo.OCF_DATE;

            dao55050 = new D55050();
        }

        /// <summary>
        /// 視窗啟動時,設定一些UI元件初始值
        /// </summary>
        /// <returns></returns>
        protected override ResultStatus Open() {
            base.Open();

            //1.設定初始年月yyyy/MM
            txtStartMonth.Text = PbFunc.f_ocf_date(0).Substring(0, 7);
            txtStartMonth.EnterMoveNextControl = true;
            txtStartMonth.Focus();

            txtEndMonth.Text = txtStartMonth.Text;
            txtEndMonth.EnterMoveNextControl = true;

            //2.設定下拉選單
            //2.1先讀取db
            DataTable dt = new ABRK().ListAll();//第一行空白+ABRK_NO/ABRK_NAME/cp_display
            cbxFcmStartNo.SetDataTable(dt, "ABRK_NO");
            cbxFcmEndNo.SetDataTable(dt, "ABRK_NO");

            rgpType.SelectedIndex = 0;//直接預設為第一個選項
            rgpType.EnterMoveNextControl = true;




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
                dt = dao55050.ListAll(txtStartMonth.FormatValue,
                                        txtEndMonth.FormatValue,
                                        startNo,
                                        endNo);
            else//依照商品別
                dt = dao55050.ListAll2(txtStartMonth.FormatValue,
                                        txtEndMonth.FormatValue,
                                        startNo,
                                        endNo);

            if (dt.Rows.Count == 0) {
                MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtStartMonth.Text + "~" + txtEndMonth.Text, this.Text));
                return ResultStatus.Success;
            }

            try {
                //3.開始轉出資料
                panFilter.Enabled = false;
                labMsg.Visible = true;
                labMsg.Text = "訊息：資料轉出中........";

                //3.1 copy template xls to target path
                string excelDestinationPath = CopyExcelTemplateFile(_ProgramID, FileType.XLS);

                //3.2 open xls
                Workbook workbook = new Workbook();
                workbook.LoadDocument(excelDestinationPath);
                int sheetNo = rgpType.SelectedIndex;
                Worksheet worksheet = workbook.Worksheets[sheetNo];

                //3.3寫入檔頭
                worksheet.Cells[3, 0].Value += System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                worksheet.Cells[3, 3].Value += txtStartMonth.Text.Replace("/", "");
                worksheet.Cells[3, 6].Value += txtEndMonth.Text.Replace("/", "");
                //ken,沒撈出此資訊,不用顯示 worksheet.Cells[3, 9].Value = "資料更新時間 : " + dt.Rows[0]["FEETRD_UPD_TIME"].AsDateTime(System.DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss");

                //3.4寫入明細
                //ken,預先做好一堆空白行數,但是如果筆數超過預設空白行數,會把最後的統計那行覆蓋掉
                int rowIndex = 7;//起始row index position
                int pos = 0;
                int rowTotalCount = rowIndex + (rgpType.SelectedIndex == 0 ? 300 : 3500);//總行數(包含空白),每個範本的預設空白行都不一樣
                foreach (DataRow row in dt.Rows) {
                    if (pos % 20 == 0) {
                        labMsg.Text = string.Format("資料轉出中......{0} / {1}", pos, dt.Rows.Count);
                        this.Refresh();
                        //Application.DoEvents();
                    }

                    if(rgpType.SelectedIndex == 0) {
                        //依照期貨商別
                        worksheet.Cells[rowIndex, 0].Value = row["feetrd_fcm_no"].AsString();//期貨商代號
                        worksheet.Cells[rowIndex, 1].Value = row["brk_abbr_name"].AsString();//期貨商名稱

                        worksheet.Cells[rowIndex, 2].Value = row["feetrd_ar"].AsDecimal();//交易經手費--應收
                        worksheet.Cells[rowIndex, 3].Value = row["feetrd_disc_amt"].AsDecimal();//交易經手費--折減
                        worksheet.Cells[rowIndex, 4].Value = row["feetrd_rec_amt"].AsDecimal();//交易經手費--實收

                        worksheet.Cells[rowIndex, 5].Value = row["feetdcc_ar"].AsDecimal();//結算服務費--應收
                        worksheet.Cells[rowIndex, 6].Value = row["feetdcc_disc_amt"].AsDecimal();//結算服務費--折減
                        worksheet.Cells[rowIndex, 7].Value = row["feetdcc_rec_amt"].AsDecimal();//結算服務費--實收
                    } else {
                        //依照商品別
                        worksheet.Cells[rowIndex, 0].Value = row["feetrd_fcm_no"].AsString();//期貨商代號
                        worksheet.Cells[rowIndex, 1].Value = row["brk_abbr_name"].AsString();//期貨商名稱
                        worksheet.Cells[rowIndex, 2].Value = row["feetrd_kind_id"].AsString();//商品名稱

                        worksheet.Cells[rowIndex, 3].Value = row["feetrd_ar"].AsDecimal();//交易經手費--應收
                        worksheet.Cells[rowIndex, 4].Value = row["feetrd_disc_amt"].AsDecimal();//交易經手費--折減
                        worksheet.Cells[rowIndex, 5].Value = row["feetrd_rec_amt"].AsDecimal();//交易經手費--實收

                        worksheet.Cells[rowIndex, 6].Value = row["feetdcc_ar"].AsDecimal();//結算服務費--應收
                        worksheet.Cells[rowIndex, 7].Value = row["feetdcc_disc_amt"].AsDecimal();//結算服務費--折減
                        worksheet.Cells[rowIndex, 8].Value = row["feetdcc_rec_amt"].AsDecimal();//結算服務費--實收
                    }//if(rgpType.SelectedIndex == 0) {

                    rowIndex++; pos++;
                }//foreach (DataRow row in dt.Rows) {


                //刪除空白列
                if (rowIndex <= rowTotalCount) {
                    worksheet.Rows.Remove(rowIndex, rowTotalCount - rowIndex);
                }

                worksheet.Range["A1"].Select();
                worksheet.ScrollToRow(0);

                //存檔
                workbook.SaveDocument(excelDestinationPath);
                return ResultStatus.Success;
            } catch (Exception ex) {
                PbFunc.f_write_logf(_ProgramID, "Error", ex.Message);
            } finally {
                labMsg.Text = "";
                labMsg.Visible = false;
                panFilter.Enabled = true;
            }
            return ResultStatus.Fail;
        }

   
    }
}
