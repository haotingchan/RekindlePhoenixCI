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

/// <summary>
/// Lukas, 2019/5/13
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
    public partial class W40073 : FormParent {
        public W40073(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }
    }
}