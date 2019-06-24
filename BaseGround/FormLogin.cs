using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects;
using DataObjects.Dao.Together;
using Log;
using System;
using System.Data;
using System.Windows.Forms;

namespace BaseGround
{
    public partial class FormLogin : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private int _Count = 0;

        public FormLogin()
        {
            InitializeComponent();
            this.InputLanguageChanged += new InputLanguageChangedEventHandler(languageChange);   // 輸入法改變時呼叫 
            txtID.ImeMode = System.Windows.Forms.ImeMode.OnHalf; // 預設txtID的輸入法為半型
            txtPassword.ImeMode = System.Windows.Forms.ImeMode.OnHalf; // 預設txtPassword的輸入法為半型
        }

        private void languageChange(Object sender, InputLanguageChangedEventArgs e) {
            txtID.ImeMode = System.Windows.Forms.ImeMode.OnHalf;  // 將txtID的輸入法變半型 
            txtPassword.ImeMode = System.Windows.Forms.ImeMode.OnHalf; // 將txtPassword的輸入法變半型
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            _Count++;
            try
            {
                string myID = txtID.Text.Trim();
                string myPassword = txtPassword.Text.Trim();
                if (myID.ToUpper() == GlobalDaoSetting.GetConnectionInfo.ConnectionName)
                {
                    GlobalInfo.USER_ID = myID.ToUpper();
                    GlobalInfo.USER_NAME = myID.ToUpper();
                    GlobalInfo.USER_DPT_ID = " ";

                    this.Hide();
                    FormMain form = new FormMain();
                    form.Show();
                    
                }
                else
                {

                    ResultData result = CheckLogin(myID, myPassword);
                    if (result.Status == ResultStatus.Success)
                    {
                        this.DialogResult = DialogResult.OK;

                        GlobalInfo.USER_ID = myID;
                        GlobalInfo.USER_NAME = result.ReturnData.Rows[0]["UPF_USER_NAME"].ToString().Trim();
                        GlobalInfo.USER_DPT_ID = result.ReturnData.Rows[0]["UPF_DPT_ID"].ToString().Trim();
                        GlobalInfo.USER_DPT_NAME = result.ReturnData.Rows[0]["DPT_NAME"].ToString().Trim();

                        SingletonLogger.Instance.Info(GlobalInfo.USER_ID, "Login", "簽到：執行Path:" + Application.StartupPath, " ");

                        this.Hide();
                        FormMain form = new FormMain();
                        form.Show();

                        //第一次登入
                        string chgFlag = result.ReturnData.Rows[0]["UPF_CHANGE_FLAG"].ToString().Trim();
                        if (chgFlag == "Y")
                        {
                            form.OpenForm("Z0990", "使用者密碼變更");
                            SingletonLogger.Instance.Info(GlobalInfo.USER_ID, "Login", "密碼強迫變更", " ");
                        }

                        //判斷過期
                        DateTime wDate = Convert.ToDateTime(result.ReturnData.Rows[0]["UPF_W_TIME"]);
                        double dateDiff = (DateTime.Today - wDate).TotalDays;
                        if (dateDiff > 90)
                        {
                            MessageDisplay.Warning("密碼已過期,請重新變更密碼才可進入!");
                            form.OpenForm("Z0990", "使用者密碼變更");
                            SingletonLogger.Instance.Info(GlobalInfo.USER_ID, "Login", "密碼強迫變更", " ");
                        }
                        else if (dateDiff >= 75 && dateDiff <= 90)
                        {
                            MessageDisplay.Warning(string.Format("密碼即將過期(還有{0}天過期),請儘快變更!", 90 - dateDiff));
                        }
                    }
                    else
                    {
                        this.DialogResult = DialogResult.No;
                        if (_Count == 3)
                        {
                            MessageDisplay.Error("使用者或密碼輸入錯誤超過3次,程式即將關閉!");
                            SingletonLogger.Instance.Info(" ", "Login", "使用者或密碼輸入錯誤超過3次(帳號錯誤)", " ");
                            this.Close();
                        }
                    }
                }
            }
            catch (ApplicationException ex)
            {
                MessageDisplay.Error(ex.Message, "登入錯誤");
            }
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public ResultData CheckLogin(string id, string password)
        {
            ResultData result = new ResultData();
            UPF daoUPF = new UPF();
            DataTable dtUPF = daoUPF.ListDataByUserId(id);
            result.ReturnData = dtUPF;

            if (dtUPF.Rows.Count != 0)
            {
                if (password == dtUPF.Rows[0]["UPF_PASSWORD"].ToString().Trim())
                {
                    result.Status = ResultStatus.Success;
                }
                else
                {
                    MessageDisplay.Error("密碼錯誤");
                    result.Status = ResultStatus.Fail;
                }
            }
            else
            {
                MessageDisplay.Error("無此帳號");
                result.Status = ResultStatus.Fail;
            }
            return result;
        }
    }
}