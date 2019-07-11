using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Data;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/3/19
/// </summary>
namespace PhoenixCI.FormUI.Prefix3
{
    /// <summary>
    /// 股票期貨每週交概況統計表
    /// </summary>
    public partial class W30418 : FormParent {

      #region 一般交易查詢條件縮寫
      /// <summary>
      /// yyyyMMdd
      /// </summary>
      public string StartDate {
         get {
            return txtStartDate.DateTimeValue.ToString("yyyyMMdd");
         }
      }

      /// <summary>
      /// yyyyMMdd
      /// </summary>
      public string EndDate {
         get {
            return txtEndDate.DateTimeValue.ToString("yyyyMMdd");
         }
      }

        #endregion

        D30418 dao30418 = new D30418();
      public W30418(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
      }

      protected override ResultStatus Open() {
         base.Open();
         try {

                //1.設定初始年月yyyy/MM/dd     
                txtStartDate.DateTimeValue = GlobalInfo.OCF_DATE;
                txtStartDate.EnterMoveNextControl = true;
                txtStartDate.Focus();

                txtEndDate.DateTimeValue = GlobalInfo.OCF_DATE;
                txtEndDate.EnterMoveNextControl = true;

                //2. 設定dropdownlist(商品)
                DataTable dtKindId = new APDK().ListParamKeyAndProd();
                dwKindId.SetDataTable(dtKindId, "APDK_PARAM_KEY", "APDK_PARAM_KEY", TextEditStyles.DisableTextEditor, "");
                dwKindId.ItemIndex = 0;

                return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
            return ResultStatus.Fail;
         }
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

        protected override ResultStatus Export()
        {
            try
            {

                #region 輸入&日期檢核

                if (string.Compare(txtStartDate.Text, txtEndDate.Text) > 0)
                {
                    MessageDisplay.Error(CheckDate.Datedif, GlobalInfo.ErrorText);
                    return ResultStatus.Fail;
                }
                #endregion

                //1. ready
                panFilter.Enabled = false;
                labMsg.Visible = true;
                labMsg.Text = "開始轉檔...";
                this.Cursor = Cursors.WaitCursor;
                this.Refresh();
                Thread.Sleep(5);


                string market = gbMarket.EditValue.AsString();
                string pcCode = gbPC.EditValue.AsString();
                string kindId = dwKindId.Text.AsString();
                string ws = "";

                DataTable dt = new DataTable();
                //4. 填資料
                if (market == "rbTFXM")//現貨
                {
                    dt = dao30418.ListTfxmData(StartDate, EndDate);
                    ws = "現貨市場三大法人";
                }
                else if (market == "rbFut")//期貨市場
                {
                    ws = "期貨市場三大法人";
                    dt = dao30418.ListFutAndOptData(txtStartDate.DateTimeValue, txtEndDate.DateTimeValue, pcCode,kindId);
                    
                }
                if (dt.Rows.Count == 0)
                {
                    MessageDisplay.Info(MessageDisplay.MSG_NO_DATA);
                    return ResultStatus.Fail;
                }

                //2. copy template xls to target path
                string excelDestinationPath = PbFunc.wf_copy_file(_ProgramID, _ProgramID);

                //3. open xls
                Workbook workbook = new Workbook();
                workbook.LoadDocument(excelDestinationPath);

                Worksheet worksheet = workbook.Worksheets[ws];
                worksheet.Import(dt,false, 1, 0);
                workbook.Worksheets.ActiveWorksheet = worksheet;


                //5. save 
                workbook.SaveDocument(excelDestinationPath);
                labMsg.Visible = false;

                if (FlagAdmin)
                    System.Diagnostics.Process.Start(excelDestinationPath);

                return ResultStatus.Success;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
            finally
            {
                panFilter.Enabled = true;
                labMsg.Text = "";
                labMsg.Visible = false;
                this.Cursor = Cursors.Arrow;
            }
            return ResultStatus.Fail;

        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool IsEnable = (sender as DevExpress.XtraEditors.RadioGroup).SelectedIndex == 0 ? false : true;
            label5.Visible = label2.Visible = dwKindId.Visible = gbPC.Visible = IsEnable;
        }

        private void dwKindId_EditValueChanged(object sender, EventArgs e)
        {
            DataRow row = ((DataRowView)((LookUpEdit)sender).GetSelectedDataRow()).Row;
            

            bool IsEnable = row["APDK_PROD_TYPE"].AsString() == "F" ? false : true;
            label5.Visible = gbPC.Visible = IsEnable;
        }
    }
}