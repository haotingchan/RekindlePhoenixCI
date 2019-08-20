namespace PhoenixCI
{
    public partial class UserControlMaskedTextBoxMonth : UserControlMaskedTextBox
    {
        public UserControlMaskedTextBoxMonth()
        {
            //InitializeComponent();
            setMaskedValue(DTTYPE.Month.ToString());
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
