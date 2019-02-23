using ActionService;
using BaseGround;
using BaseGround.Report;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using Log;
using System;
using System.Data;
using System.Text.RegularExpressions;

namespace PhoenixCI.FormUI.PrefixZ
{
    public partial class WZ0990 : FormParent
    {
      private UPF daoUPF;

      public WZ0990(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();

            this.Text = _ProgramID + "─" + _ProgramName;
         daoUPF = new UPF();
      }

        public override ResultStatus BeforeOpen()
        {
            base.BeforeOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus Open()
        {
            base.Open();

            lblUserId.Text = GlobalInfo.USER_ID;
            lblUserName.Text = GlobalInfo.USER_NAME;

            return ResultStatus.Success;
        }

        protected override ResultStatus AfterOpen()
        {
            base.AfterOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus ActivatedForm()
        {
            base.ActivatedForm();

            return ResultStatus.Success;
        }

        protected override ResultStatus Print(ReportHelper reportHelper)
        {
            base.Print(reportHelper);

            return ResultStatus.Success;
        }

        protected override ResultStatus Export(ReportHelper reportHelper)
        {
            base.Export(reportHelper);

            return ResultStatus.Success;
        }

        protected override ResultStatus CheckShield()
        {
            base.CheckShield();
            string orgPassword = txtOrgPassword.Text;
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (orgPassword == password)
            {
                MessageDisplay.Warning("「原始舊密碼」與「變更後新密碼」相同");
                txtPassword.Focus();
                return ResultStatus.Fail;
            }

            if (string.IsNullOrEmpty(confirmPassword))
            {
                MessageDisplay.Warning("請輸入「再次確認新密碼」欄位");
                txtConfirmPassword.Focus();
                return ResultStatus.Fail;
            }

            if (confirmPassword != password)
            {
                MessageDisplay.Warning("「變更後新密碼」與「再次確認新密碼」不符");
                txtPassword.Focus();
                return ResultStatus.Fail;
            }

            if (password.Length < 8)
            {
                MessageDisplay.Warning("密碼長度必須大於8位!");
                txtPassword.Focus();
                return ResultStatus.Fail;
            }

            Regex regx = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,10}$");
            if (!regx.IsMatch(password))
            {
                MessageDisplay.Warning("密碼不符規定，至少(大寫英文'A',小字英文'a',數字'1')型態各一!");
                txtPassword.Focus();
                return ResultStatus.Fail;
            }

            DataTable dt = daoUPF.ListDataByUserIdAndPassword(lblUserId.Text, orgPassword);
            if (dt.Rows.Count == 0)
            {
                MessageDisplay.Warning("原密碼輸入錯誤");
                txtOrgPassword.Focus();
                return ResultStatus.Fail;
            }

            return ResultStatus.Success;
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if (CheckShield() == ResultStatus.Success)
            {
                bool result = daoUPF.UpdatePasswordByUserId(lblUserId.Text, txtPassword.Text, "N");
                if (result == false)
                {
                    MessageDisplay.Error("密碼變更失敗");
                }
                else
                {
                    SingletonLogger.Instance.Info(GlobalInfo.USER_ID, _ProgramID, "變更密碼", "I");
                    COMPLETE();
                }
            }
        }

        protected override ResultStatus COMPLETE()
        {
            MessageDisplay.Info("密碼變更完成!");

            this.Close();

            return ResultStatus.Success;
        }
    }
}