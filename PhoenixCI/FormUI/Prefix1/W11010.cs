using ActionService.Extensions;
using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DevExpress.XtraEditors.Repository;
using System;
using System.Windows.Forms;

namespace PhoenixCI.FormUI.Prefix1
{
    public partial class W11010 : W1xxx
    {
        public W11010(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();
            _DB_TYPE = "monit";
        }

        protected override ResultStatus Open()
        {
            if (DesignMode) return ResultStatus.Success;
            base.Open();
            return ResultStatus.Success;
        }

        protected override ResultStatus RunBefore(PokeBall args)
        {
            base.RunBefore(args);


            return base.RunBefore(args);
        }
    }
}