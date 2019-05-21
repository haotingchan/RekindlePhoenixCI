using BaseGround;

namespace PhoenixCI.FormUI.Prefix2
{
   public partial class W28630 : FormParent
   {
      public W28630(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();

         this.Text = _ProgramID + "─" + _ProgramName;
      }
   }
}