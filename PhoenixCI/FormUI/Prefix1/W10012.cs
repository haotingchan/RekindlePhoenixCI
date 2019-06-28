using ActionService.Extensions;
using BaseGround;
using BaseGround.Report;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DevExpress.XtraEditors.Repository;
using System;
using System.Windows.Forms;

namespace PhoenixCI.FormUI.Prefix1
{
    public partial class W10012 : W1xxx
    {
        public W10012(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();
            _DB_TYPE = "fut";
        }
    }
}