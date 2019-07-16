using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.Threading;
using System.Windows.Forms;
/// <summary>
/// Lukas, 2018/12/27
/// </summary>
namespace PhoenixCI.FormUI.Prefix5
{
    /// <summary>
    /// 55060 CME美國道瓊及標普500期貨授權費表
    /// 有寫到的功能：Export
    /// </summary>
    public partial class W55070 : FormParent {

        private D55070 dao55070;

        public W55070(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            dao55070 = new D55070();
            this.Text = _ProgramID + "─" + _ProgramName;
            txtFromMonth.DateTimeValue = GlobalInfo.OCF_DATE;
            txtToMonth.DateTimeValue = GlobalInfo.OCF_DATE;
        }

        protected override ResultStatus Open() {
            base.Open();

            return ResultStatus.Success;
        }

        protected override ResultStatus ActivatedForm() {
            base.ActivatedForm();

            _ToolBtnExport.Enabled = true;

            return ResultStatus.Success;
        }

        protected void ShowMsg(string msg) {
            lblProcessing.Text = msg;
            this.Refresh();
            Thread.Sleep(5);
        }

        protected override ResultStatus Export() {
            try
            {
                if (txtFromMonth.Text.SubStr(0, 4) != txtToMonth.Text.SubStr(0, 4))
                {
                    MessageBox.Show("不可跨年度查詢!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return ResultStatus.Fail;
                }
                base.Export();
                this.Cursor = Cursors.WaitCursor;
                this.Refresh();
                Thread.Sleep(5);
                lblProcessing.Visible = true;
                ShowMsg("開始轉檔...");

                /*************************************
                (3).Deductable item：
                    Market maker rebate
                檢查trd_ym,cm_ym是否符合輸入條件，若沒有要警示
                *************************************/
                DataTable dtAmt = dao55070.getAMT(txtFromMonth.FormatValue, txtToMonth.FormatValue);
                if (dtAmt.Rows[0]["trd_ym"].AsString() == "-" || string.IsNullOrEmpty(dtAmt.Rows[0]["cm_ym"].AsString()))
                {
                    if (MessageDisplay.Choose("Market maker rebate 無符合時間區間內的資料，是否繼續?") == DialogResult.No)
                    {
                        ShowMsg("轉檔錯誤");
                        return ResultStatus.Fail;
                    }

                }

                string excelDestinationPath = PbFunc.wf_copy_file(_ProgramID, _ProgramID);
                Workbook workbook = new Workbook();
                workbook.LoadDocument(excelDestinationPath);
                Worksheet ws = workbook.Worksheets[0];
                ws.Cells[0, 0].Value = txtFromMonth.Text + "/01 ~"+ txtToMonth.DateTimeValue.AddMonths(1).AddDays(-1).ToString("yyyy/MM/dd") + ws.Cells[0, 0].Value;
                ws.Cells[14, 1].SetValue(dtAmt.Rows[0]["trd_amt"]);

                /*************************************
                Volume-based incentive program
                20420新增55070設定法人身份碼，目前是'2','H'
                *************************************/
                DataTable dtCurrAmt = dao55070.getCurrAMT(txtFromMonth.FormatValue, txtToMonth.FormatValue);
                if (dtCurrAmt.Rows.Count > 0)
                {
                    ws.Cells[15, 1].SetValue(dtCurrAmt.Rows[0]["curr_amt"]);
                }

                /*************************************
                   (4). Pro-rata share of market data information revenue
                         (a)TAIFEX total market data revenue from members and IB
                *************************************/
                DataTable dtInfoFee = dao55070.getInfoFee(txtFromMonth.FormatValue, txtToMonth.FormatValue);
                if (dtInfoFee.Rows.Count > 0)
                {
                    ws.Cells[18, 1].SetValue(dtInfoFee.Rows[0]["INFO_FEE"]);
                }

                /**************************************
                   (1). TAIFEX Brent Crude Oil futures revenue- by trading volume
                           (a)Trading volume 
                   (4). Pro-rata share of market data information revenue
                         (b)TAIFEX Brent Crude Oil futures ADV
                         (c)TAIFEX total products ADV

                **************************************/
                DataTable dtQnty = dao55070.getQnty(txtFromMonth.FormatValue, txtToMonth.FormatValue);
                if (dtQnty.Rows.Count > 0)
                {
                    ws.Cells[4, 1].SetValue(dtQnty.Rows[0]["QNTY"]);
                    ws.Cells[19, 1].SetValue(dtQnty.Rows[0]["QNTY_AVG"]);
                    ws.Cells[20, 1].SetValue(dtQnty.Rows[0]["TOL_QNTY_AVG"]);
                }


                DataTable dtOI = dao55070.getOI(txtFromMonth.FormatValue, txtToMonth.FormatValue);
                if (dtOI.Rows.Count > 0)
                {
                    ws.Cells[9, 1].SetValue(dtOI.Rows[0]["BRF_OI"]);
                }

                //存檔
                workbook.SaveDocument(excelDestinationPath);

                /**********************
                轉檔後資訊
                **********************/
                ShowMsg("轉檔成功");
            }
            catch (Exception ex)
            {
                MessageDisplay.Error("輸出錯誤");
                ShowMsg("轉檔錯誤");
                throw ex;
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
                this.Refresh();
                Thread.Sleep(5);
            }
            return ResultStatus.Success;
        }

        
    }
}