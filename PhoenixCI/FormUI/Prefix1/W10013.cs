using BaseGround;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
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
    public partial class W10013 : W1xxx
    {
        public W10013(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();
            _DB_TYPE = "fut";
        }
        protected override ResultData ExecuteForm(PokeBall args)
        {
            ResultData resultData = new ResultData();
            switch (args.TXF_TID)
            {
                case "w_30053":
                    break;
                case "w_30055":

                    break;
            }

            resultData = base.ExecuteForm(args);

            return resultData;
        }
    }
}
