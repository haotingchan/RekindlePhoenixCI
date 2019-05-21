using BaseGround;

namespace PhoenixCI.FormUI.Prefix2
{
   public partial class W28620 : FormParent
   {
      public W28620(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();

         this.Text = _ProgramID + "─" + _ProgramName;
      }
   }
}