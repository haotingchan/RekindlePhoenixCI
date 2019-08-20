namespace PhoenixCI
{
    public partial class UserControlMaskedTextBoxDay : UserControlMaskedTextBox
    {
        public UserControlMaskedTextBoxDay()
        {
            //InitializeComponent();
            setMaskedValue(DTTYPE.Date.ToString());
        }
        public void setTextValue(string str)
        {
            SetTextValue(str);
        }
        public string getTextValue()
        {
            return GetTextValue();
        }
    }
}
