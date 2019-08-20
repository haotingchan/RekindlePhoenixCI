namespace PhoenixCI
{
    public partial class UserControlMaskedTextBox_Year : UserControlMaskedTextBox
    {
        public UserControlMaskedTextBox_Year()
        {
            //InitializeComponent();
            setMaskedValue(DTTYPE.Year.ToString());
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
