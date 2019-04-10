using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Shared;

/// <summary>
/// Lukas, 2019/4/9
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
    /// <summary>
    /// 30223 個股類部位限制公告表
    /// </summary>
    public partial class W30223 : FormParent {
        public W30223(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {
            base.Open();
            //日期
            txtSDate.EditValue = PbFunc.f_ocf_date(0);
            txtEDate.EditValue = txtSDate.EditValue;

#if DEBUG
            txtSDate.EditValue = "2018/12/28";
            txtEDate.EditValue = txtSDate.EditValue;
#endif

            return ResultStatus.Success;
        }
    }
}