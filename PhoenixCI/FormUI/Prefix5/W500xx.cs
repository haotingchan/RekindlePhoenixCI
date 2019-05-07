using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Report;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together;
using BaseGround.Shared;
using DevExpress.XtraEditors.Controls;

namespace PhoenixCI.FormUI.Prefix5
{
   public partial class W500xx : FormParent
   {
      public D500xx D500xx { get; set; }
      private ABRK daoABRK;
      private APDK daoAPDK;

      public W500xx(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         daoABRK = new ABRK();
         daoAPDK = new APDK();
      }

      public override ResultStatus BeforeOpen()
      {
         if(!PbFunc.f_chk_run_timing(_ProgramID))
            MessageDisplay.Info("今日盤後轉檔作業還未完畢!");

         return ResultStatus.Success;
      }

      protected override ResultStatus Open()
      {
         //Input Condition
         emEndDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
         emStartDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/01");
         emStartYM.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM");
         emEndYM.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM");
         /* 造市者代號 */
         //起始選項
         dwSbrkno.SetDataTable(daoABRK.ListAll2(), "ABRK_NO", "CP_DISPLAY", TextEditStyles.Standard, null);
         //目的選項
         dwEbrkno.SetDataTable(daoABRK.ListAll2(), "ABRK_NO", "CP_DISPLAY", TextEditStyles.Standard, null);
         /* 商品群組 */
         dwProdCt.SetDataTable(daoAPDK.ListParamKey(), "APDK_PARAM_KEY", "APDK_PARAM_KEY", TextEditStyles.Standard, null);
         /* 造市商品 */
         dwProdKd.SetDataTable(daoAPDK.ListAll3(), "PDK_KIND_ID", "PDK_KIND_ID", TextEditStyles.Standard, null);
         /* 2碼商品 */
         dwProdKdSto.SetDataTable(daoAPDK.ListKind2(), "APDK_KIND_ID_STO", "APDK_KIND_ID_STO", TextEditStyles.Standard, null);
         //預設資料表
         D500xx.TableName = "AMM0";

         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen()
      {
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm()
      {
         _ToolBtnRetrieve.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Retrieve()
      {
         return ResultStatus.Success;
      }

      protected override ResultStatus Export()
      {
         stMsgTxt.Visible = true;
         stMsgTxt.Text = "開始轉檔...";
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper)
      {
         base.Print(reportHelper);

         return ResultStatus.Success;
      }

      protected override ResultStatus BeforeClose()
      {
         D500xx.Data.Clear();
         documentViewer1.DocumentSource = null;
         return ResultStatus.Success;
      }
   }
}