using System;
using System.Data;
using System.Windows.Forms;
using BaseGround;
using Common;
using BusinessObjects.Enums;
using DataObjects.Dao.Together.SpecificDao;
using BusinessObjects;
using System.IO;
using BaseGround.Shared;
using Log;

namespace PhoenixCI.FormUI.Prefix5
{
    public partial class W56090 : FormParent
    {
        D56090 dao56090;
        DataTable dtReadTxt;

        public W56090(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();
            dao56090 = new D56090();
            dtReadTxt = new DataTable();
            //設定資料欄位
            dtReadTxt.Columns.Add("FEETDCC_YM", typeof(string));
            dtReadTxt.Columns.Add("FEETDCC_FCM_NO", typeof(string));
            dtReadTxt.Columns.Add("FEETDCC_ACC_NO", typeof(string));
            dtReadTxt.Columns.Add("FEETDCC_KIND_ID", typeof(string));
            dtReadTxt.Columns.Add("FEETDCC_DISC_QNTY", typeof(int));
            dtReadTxt.Columns.Add("FEETDCC_DISC_RATE", typeof(string));
            dtReadTxt.Columns.Add("FEETDCC_ORG_AR", typeof(int));
            dtReadTxt.Columns.Add("FEETDCC_DISC_AMT", typeof(int));
            dtReadTxt.Columns.Add("FEETDCC_W_USER_ID", typeof(string));
            dtReadTxt.Columns.Add("FEETDCC_W_TIME", typeof(DateTime));
            dtReadTxt.Columns.Add("FEETDCC_SESSION", typeof(string));

            ImportShow.Hide();

            this.Text = _ProgramID + "─" + _ProgramName;

            gcMain.Hide();
            txtYM.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM");
        }

        protected override ResultStatus Import()
        {
            base.Import(gcMain);
            ImportShow.Text = "開始轉檔...";
            ImportShow.Show();

            //1.讀檔並寫入DataTable
            try {
                OpenFileDialog open = new OpenFileDialog();

                open.Filter = "*.txt (*.txt)|*.txt";
                open.Title = "請點選儲存檔案之目錄";
                open.FileName = "56090.txt";

                if (open.ShowDialog() != DialogResult.OK) {
                    ImportShow.Hide();
                    return ResultStatus.Fail;
                }

                using (TextReader tr = File.OpenText(open.FileName)) {
                    string line;
                    while ((line = tr.ReadLine()) != null) {
                        DataRow d = dtReadTxt.NewRow();
                        float DISC_RATE = float.Parse(line.SubStr(32, 9)) == 0 ? 0 :
                            float.Parse(line.SubStr(32, 9)) / 100000000;
                        d[0] = line.SubStr(0, 6);
                        d[1] = line.SubStr(6, 7);
                        d[2] = line.SubStr(13, 7);
                        d[3] = line.SubStr(20, 4);
                        d[4] = line.SubStr(24, 8);
                        d[5] = DISC_RATE == 0 ? "0" : DISC_RATE.ToString("##.##");
                        d[6] = line.SubStr(41, 10);
                        d[7] = line.SubStr(51, 10);
                        d[8] = GlobalInfo.USER_ID;
                        d[9] = DateTime.Now;
                        d[10] = line.SubStr(61, 2);

                        dtReadTxt.Rows.Add(d);
                    }
                }

                //2.確認資料日期&畫面日期(讀取資料)
                string datadate = dtReadTxt.Rows[0][0].ToString();
                if (datadate != txtYM.Text.Replace("/", "")) {
                    DialogResult result = MessageBox.Show("資料年月(" + datadate + ")與畫面年月不同,是否將畫面改為資料年月?", "注意", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.No) {
                        return ResultStatus.Fail;
                    }
                    else {
                        txtYM.Text = datadate.SubStr(0, 4) + "/" + datadate.SubStr(4, 2);
                    }
                }
                //3.刪除舊有資料
                if (dao56090.DeleteByYM(datadate) < 0) {
                    MessageDisplay.Error("刪除失敗");
                    return ResultStatus.Fail;
                }
                //4.轉入資料即PB的wf_importfile()→wf_importfile_extra()
                if (Save(new PokeBall()) == ResultStatus.Success) {
                    ImportShow.Text = "轉檔完成!";
                }
                else {
                    throw new Exception("轉檔失敗");
                }
            }
            catch (Exception ex) {
                throw ex;
            }
            return ResultStatus.Success;
        }

        protected override ResultStatus Save(PokeBall poke)
        {
            gvMain.CloseEditor();
            gvMain.UpdateCurrentRow();

            try {
                ResultStatus status = dao56090.updateData(dtReadTxt).Status;//base.Save_Override(dtReadTxt, "FEETDCC");
                if (status == ResultStatus.Fail) {
                    return ResultStatus.Fail;
                }
                _IsPreventFlowPrint = true;
                return ResultStatus.Success;
            }
            catch(Exception ex) {
                throw ex;
            }
        }

        protected override ResultStatus Retrieve()
        {
            gcMain.Show();
            base.Retrieve(gcMain);
            DataTable returnTable = new DataTable();
            returnTable = dao56090.GetData();
            if (returnTable.Rows.Count == 0)
            {
                MessageBox.Show("無任何資料", "訊息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            gcMain.DataSource = returnTable;

            gcMain.Focus();


            return ResultStatus.Success;
        }

        protected override ResultStatus ActivatedForm()
        {
            base.ActivatedForm();
            _ToolBtnImport.Enabled = true;
            _ToolBtnRetrieve.Enabled = true;

            return ResultStatus.Success;
        }

        protected override ResultStatus BeforeClose()
        {
            return base.BeforeClose();
        }

        protected override ResultStatus COMPLETE()
        {
            MessageDisplay.Info(MessageDisplay.MSG_OK);
            Retrieve();
            return ResultStatus.Success;
        }

        private bool CheckDate() {
            try {
                DateTime YM = DateTime.Parse(txtYM.Text);
            }
            catch (Exception ex) {
                MessageDisplay.Info("請確認輸入的日期");
                return false;
            }
            return true;
        }
    }
}