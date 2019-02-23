using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Shared;
using Common;
using DevExpress.Spreadsheet;
using DataObjects.Dao.Together.SpecificDao;

/// <summary>
/// Lukas, 2019/2/21
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {

    /// <summary>
    /// 30020 期貨交易累計開戶及交易戶數統計表
    /// </summary>
    public partial class W30020 : FormParent {

        private D30020 dao30020;

        public W30020(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {
            base.Open();
            txtEDate.EditValue = PbFunc.f_ocf_date(0);
            txtSDate.EditValue = txtEDate.Text.SubStr(0, 8) + "01";
            txtSDate.Focus();
            return ResultStatus.Success;
        }

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

        protected override ResultStatus Export() {

            dao30020 = new D30020();

            string ls_rpt_id, ls_file;
            ls_rpt_id = "30020";
            /******************
            複製檔案
            ******************/
            ls_file = PbFunc.wf_copy_file(ls_rpt_id, ls_rpt_id);
            if (ls_file == "") {
                return ResultStatus.Fail;
            }
            /******************
            開啟檔案
            ******************/
            Workbook workbook = new Workbook();
            workbook.LoadDocument(ls_file);

            #region 30021
            string ls_rpt_name;
            string ls_acc_type;
            DateTime ldt_date, ldt_max_date;
            int i, j, li_ole_row, li_ole_col, li_acc_row, li_acc_row_tol;
            long ll_found;
            /*************************************
            ls_rpt_name = 報表名稱
            ls_rpt_id = 報表代號

            li_ole_row = Excel的Row位置
            li_ole_col = Excel的Column位置
            li_acc_row = 身份碼Row位置
            li_acc_row_tol = 身份碼總RowCount
            ll_found = 比對照到資料的row number

            ls_acc_type = ids_1身份碼
            ldt_date = ids_1每筆日期
            ldt_max_date = ids_1最大日期
            *************************************/
            ls_rpt_name = "期貨交易累計開戶及交易戶數統計表";
            ls_rpt_id = "30021";
            //st_msg_txt.text = ls_rpt_id + '－' + ls_rpt_name + ' 轉檔中...';

            /******************
            讀取資料
            ******************/
            DataTable dt30021 = dao30020.d_30021(txtSDate.DateTimeValue, txtEDate.DateTimeValue);
            if (dt30021.Rows.Count == 0) {
                MessageDisplay.Info(txtSDate.Text + "～" + txtEDate.Text + "," + ls_rpt_id + '－' + ls_rpt_name + ",無任何資料!");
                return ResultStatus.Fail;
            }
            //ACC_TYPE
            //DataTable dt30021_acc_type = dao30020.d_30021_acc_type();
            #endregion

            return ResultStatus.Success;
        }
    }
}