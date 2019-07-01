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
            _DB_TYPE = "opt";
        }

        protected override string RunBeforeEveryItem(PokeBall args)
        {
            base.RunBeforeEveryItem(args);
            switch (args.TXF_TID)
            {
                case "sp_O_gen_T_TPROD_FUT":

                    break;
                case "wf_CI_1002205":
                    break;
                case "sp_H_upd_AMM0_Month":
                        
                    break;
            }

            return "";
        }

    }
}
