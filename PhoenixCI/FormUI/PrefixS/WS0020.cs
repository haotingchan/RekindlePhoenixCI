using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using System.IO;
using Common;
using DevExpress.Spreadsheet;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects;
using System.Data.Common;

namespace PhoenixCI.FormUI.PrefixS
{
    public partial class WS0020 : FormParent
    {
        private DS0020 daoS0020;
        private ResultStatus exportStatus = ResultStatus.Fail;

        public WS0020(string programID, string programName) : base(programID, programName)
        {
            daoS0020 = new DS0020();
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;

            ExportShow.Hide();
        }

        protected override ResultStatus Export()
        {
            ExportShow.Text = "轉檔中...";
            ExportShow.Show();

            Workbook workbook = new Workbook();
            DataTable dtSP4 = new DataTable();
            DataTable dtSP5 = new DataTable();
            string ls_rpt_id = _ProgramID;// 報表代號
            string ls_filename = _ProgramID + "_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("HH.mm.ss") + ".xls";
            string sourceFilePath = Path.Combine(GlobalInfo.DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH, ls_rpt_id + ".xls");
            string destinationFilePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, ls_filename);

            try
            {
                //判斷檔案是否存在,不存在就開一個新檔案
                if (!File.Exists(destinationFilePath))
                {
                    File.Create(destinationFilePath).Close();
                }
                File.Copy(sourceFilePath, destinationFilePath, true);
                workbook.LoadDocument(destinationFilePath);
                dtSP4 = daoS0020.GetSP4Data(txtDate.DateTimeValue.ToString("yyyy/MM/dd"));
                dtSP5 = daoS0020.GetSP5Data(txtDate.DateTimeValue.ToString("yyyy/MM/dd"));

                if (dtSP4.Rows.Count <= 0)
                {
                    ExportShow.Hide();
                    MessageDisplay.Info("無任何資料");
                    return ResultStatus.Fail;
                }

                //將SP4資料放入Excel
                Worksheet worksheet = workbook.Worksheets[0];
                worksheet.Cells[0, 3].Value = txtDate.DateTimeValue.ToString("yyyy/MM/dd");
                for (int i = 0; i < dtSP4.Rows.Count; i++)
                {
                    int row = dtSP4.Rows[i]["SP4_TYPE"].AsInt();
                    worksheet.Cells[row + 7, 1].Value = dtSP4.Rows[i]["sp4_span_cnt"].AsInt();
                    worksheet.Cells[row + 7, 2].Value = dtSP4.Rows[i]["sp4_mkt_cnt"].AsInt();
                }

                //將SP5資料放入Excel(◎尚未申報者:)
                if (dtSP5.Rows.Count > 0)
                {
                    //從Excel 第40列開始塞資料
                    for (int i = 39; i < dtSP5.Rows.Count + 39; i++)
                    {
                        worksheet.Cells[i, 0].Value = dtSP5.Rows[i - 39]["sp5_brk_no"].AsString() + '－' +
                            dtSP5.Rows[i - 39]["sp5_brk_abbr_name"].AsString();
                    }
                }
                workbook.SaveDocument(destinationFilePath);
            }
            catch (Exception ex)
            {
                ExportShow.Text = "轉檔失敗";
                return ResultStatus.Fail;
            }
            ExportShow.Text = "轉檔成功!";
            exportStatus = ResultStatus.Success;
            return ResultStatus.Success;
        }

        protected override ResultStatus ExportAfter(string startTime)
        {
            if (exportStatus == ResultStatus.Success)
            {
                MessageDisplay.Info("轉檔完成!");
                return ResultStatus.Success;
            }
            else
            {
                MessageDisplay.Warning("轉檔失敗");
                return ResultStatus.Fail;
            }
        }

        protected override ResultStatus ActivatedForm()
        {
            base.ActivatedForm();
            _ToolBtnExport.Enabled = true;

            return ResultStatus.Success;
        }
    }
}