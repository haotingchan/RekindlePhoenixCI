using BaseGround;
using BusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhoenixCI.FormUI.Prefix1
{
    public partial class W10023 : W1xxx
    {

        public W10023(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();
            _DB_TYPE = "opt";
        }

        protected override string RunBeforeEveryItem(PokeBall args)
        {
            string rtn = "";
            switch (args.TXF_TID)
            {
                case "sp_O_gen_T_TPROD_FUT":
                    DataTable dtLogSP = servicePrefix1.ListLogsp(txtOcfDate.DateTimeValue, "sp_F_gen_MTF0", "D");
                    if (dtLogSP.Rows.Count > 0)
                    {
                        if (dtLogSP.Rows[0]["LOGSP_END_TIME"] == null)
                        {
                            rtn = $"{args.TXF_TID} 需待「10012 - sp_F_gen_MTF0」完成才可執行!";
                        }
                    }
                    break;
                case "wf_CI_1002205":
                    //全部wf_70010('%')

                    //日盤wf_70010('0')
                    break;
            }
            base.RunBeforeEveryItem(args);

            return rtn;
        }

    }
}
