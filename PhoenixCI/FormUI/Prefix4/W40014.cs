using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Shared;
using System.Threading;
using PhoenixCI.BusinessLogic.Prefix4;
using System.IO;
using Common;
using DevExpress.Spreadsheet;
using System.Data;
using DataObjects.Dao.Together;
using System.Linq;
/// <summary>
/// Sandylo,選擇權契約混合部位風險保證金狀況表
/// </summary>
namespace PhoenixCI.FormUI.Prefix4
{
    /// <summary>
    /// 選擇權契約混合部位風險保證金狀況表
    /// </summary>
    public partial class W40014 : FormParent
   {
        private D40014 dao40014;
      public W40014(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
      }

      protected override ResultStatus Open()
      {
         base.Open();

         txtDate.DateTimeValue = DateTime.Now;
         txtDate.Focus();

         dao40014 = new D40014();
         
          //預設交易時段
         OCFG daoOCFG = new OCFG();
         oswGrpLookItem.SetDataTable(daoOCFG.ListAllTime(), "OSW_GRP", "OSW_GRP_NAME", DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor, null);
         oswGrpLookItem.EditValue = daoOCFG.f_gen_osw_grp();

         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm()
      {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      /// <summary>
      /// 轉檔前檢查日期格式
      /// </summary>
      /// <returns></returns>
      private bool StartExport()
      {
         if (!txtDate.IsDate(txtDate.Text, "日期輸入錯誤")) {
            return false;
         }

         stMsgTxt.Visible = true;
         stMsgTxt.Text = "開始轉檔...";
         this.Cursor = Cursors.WaitCursor;
         this.Refresh();
         Thread.Sleep(5);
         return true;
      }

      /// <summary>
      /// 轉檔後清除文字訊息
      /// </summary>
      protected void EndExport()
      {
         stMsgTxt.Text = "";
         this.Cursor = Cursors.Arrow;
         this.Refresh();
         Thread.Sleep(5);
         stMsgTxt.Visible = false;
      }

      /// <summary>
      /// show出訊息在label
      /// </summary>
      /// <param name="msg"></param>
      protected void ShowMsg(string msg)
      {
         stMsgTxt.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }

      private bool OutputChooseMessage(string str)
      {
         DialogResult ChooseResult = MessageDisplay.Choose(str);
         if (ChooseResult == DialogResult.No) {
            EndExport();
            return false;
         }
         return true;
      }

      protected override ResultStatus Export()
      {
         if (!StartExport()) {
            return ResultStatus.Fail;
         }
         try {
                string file = PbFunc.wf_copy_file(_ProgramID, _ProgramID);
                if (file == "")
                {
                    return ResultStatus.Fail;
                }

                Workbook workbook = new Workbook();
                workbook.LoadDocument(file);

                //C值狀況表-機動
                Worksheet worksheet = workbook.Worksheets[0];
                if (wfSheet(worksheet, "D") == ResultStatus.Fail)
                {
                    return ResultStatus.Fail;
                }
                
                //C值狀況表-定期
                worksheet = workbook.Worksheets[1];
                if (wfSheet(worksheet, "R") == ResultStatus.Fail)
                {
                    return ResultStatus.Fail;
                }

                workbook.SaveDocument(file);
                ShowMsg("轉檔成功");
            }
         catch (Exception ex) {

            WriteLog(ex);
            return ResultStatus.Fail;
         }
         finally {
            EndExport();
         }

         return ResultStatus.Success;
      }

        private ResultStatus wfSheet(Worksheet worksheet,string kind) {
            DataTable dt = dao40014.ListData(kind, txtDate.FormatValue, oswGrpLookItem.EditValue.AsString() + "%");
            //worksheet.Import(dt, false, 19, 1);
            int rowCount = dt.Rows.Count;

            if (rowCount == 0)
            {
                MessageDisplay.Info(MessageDisplay.MSG_NO_DATA);
                return ResultStatus.Fail;
            }

            DataTable dtMGCT1 = dao40014.ListLevelData(kind);
            worksheet.Import(dtMGCT1, false, 7, 1);

            string groupTime = ((DataRowView)oswGrpLookItem.GetSelectedDataRow()).Row["OCFG_CLOSE_TIME"].AsDateTime().ToString("tthh時mm分");
            worksheet.Rows[0][0].Value = groupTime + worksheet.Rows[0][0].Value;

            int rowIndex = 19;

            string note = "";
            foreach (DataRow row in dt.Rows) {
                

                worksheet.Rows[rowIndex][1].SetValue(row["MGC1_KIND_ID"].AsString());
                worksheet.Rows[rowIndex][2].SetValue(row["APDK_NAME"].AsString());
                worksheet.Rows[rowIndex][3].SetValue(row["MGC1_CP_ACC_CNT"]);
                worksheet.Rows[rowIndex][4].SetValue(row["MGC1_CP_COMBI_CNT"]);

                switch (kind)
                {
                    case "D":
                        worksheet.Rows[1][12].SetValue(txtDate.Text);
                        worksheet.Rows[rowIndex][5].SetValue(row["MGC1_NATURE_COMBI_CNT"]);
                        worksheet.Rows[rowIndex][6].SetValue(row["MGC1_NATURE_OI"]);
                        worksheet.Rows[rowIndex][7].SetValue(row["MGC1_1DAY_RATE"]);
                        worksheet.Rows[rowIndex][8].SetValue(row["MGC1_CP_RATE"]);
                        worksheet.Rows[rowIndex][9].SetValue(row["MGCD1_R_DAY_CNT"]);
                        worksheet.Rows[rowIndex][10].SetValue(row["MGC1_CUR_RATE"]);
                        worksheet.Rows[rowIndex][11].SetValue(row["MGC1_RATE"]);
                        if (row["MGCD1_R_DAY_CNT"].AsDecimal() > 0)
                        {
                            note = $"戶數：{row["DAYS_CP_ACC_CNT"]}\n組數：{row["DAYS_CP_COMBI_CNT"]}\n比例(日期)：{row["DAYS_RATE"]}";
                        }
                        worksheet.Rows[rowIndex][12].SetValue(note);
                        break;
                    case "R":
                        worksheet.Rows[1][10].SetValue(txtDate.Text);
                        worksheet.Rows[rowIndex][5].SetValue(row["MGC1_CP_RATE"]);
                        worksheet.Rows[rowIndex][6].SetValue(row["MGC1_CUR_RATE"]);
                        worksheet.Rows[rowIndex][7].SetValue(row["MGC1_RATE"]);
                        worksheet.Rows[rowIndex][8].SetValue(row["MGC1_CHANGE_FLAG"].AsString());
                        note = $"戶數：{row["TOP5_ACC_CNT"]}\n組數：{row["TOP5_COMBI_CNT"]}\n比例(日期)：{row["TOP5_RATE"]}";
                        worksheet.Rows[rowIndex][9].SetValue(note);
                        break;
                }

                rowIndex++;
            }

            //int rowIndex = 19 + rowCount+1;
            

            dt = dt.Filter("MGC1_CHANGE_FLAG='Y'");
            string kindList = "";
            
            kindList = dt.Rows.Count > 0 ? string.Join(",", dt.Rows.OfType<DataRow>().Select(x => x["MGC1_KIND_ID"].ToString()).ToArray()) : "無";

            worksheet.Rows[219][1].Value += kindList;

            if (rowCount < 200)
                worksheet.Range[$"{rowIndex+1}:{219}"].Delete();

            return ResultStatus.Success;
        }
   }
}