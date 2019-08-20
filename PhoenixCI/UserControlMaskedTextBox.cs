using System.Windows.Forms;

namespace PhoenixCI
{
    public partial class UserControlMaskedTextBox : UserControl
    {
        public enum DTTYPE
        {
            Year,
            Month,
            Date
        }

        private string DType;
        private int timeout = 1500;
        public UserControlMaskedTextBox()
        {
            InitializeComponent();
            MaskedTextBox.MaskInputRejected += maskedTextBox1_MaskInputRejected;
            MaskedTextBox.TypeValidationCompleted += maskedTextBox1_TypeValidationCompleted;
        }
        public void setMaskedValue(string Value)
        {
            // 設定西元簡單日期
            if (Value.Equals(DTTYPE.Month.ToString())) MaskedTextBox.Mask = "0000/00";
            else if (Value.Equals(DTTYPE.Year.ToString())) MaskedTextBox.Mask = "0000";
            else MaskedTextBox.Mask = "0000/00/00";
            MaskedTextBox.ValidatingType = typeof(System.DateTime);
            DType = Value;
        }
        protected void SetTextValue(string str)
        {
            MaskedTextBox.Text = str;
        }
        protected string GetTextValue()
        {
            return MaskedTextBox.Text;
        }
        void maskedTextBox1_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            int Check_Length = MaskedTextBox.Text.Replace("/", "").Trim().Length;
            if (e.IsValidInput || Check_Length == 0) return;
            if (MaskedTextBox.Text.Trim().Length == 4 && DType.Equals(DTTYPE.Year.ToString()))
                if (int.Parse(MaskedTextBox.Text.Substring(0, 1)) > 0) return;
            toolTip1.Show("無效日期格式或輸入未完整", MaskedTextBox, timeout);
            //取消該事件，並把 focus 鎖定在 maskedTextbox
            e.Cancel = true;
        }

        void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            toolTip1.Show("只能輸入數字(0-9)", MaskedTextBox, timeout);
        }
    }
}

