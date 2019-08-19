
using BaseGround;
using BusinessObjects;
using PhoenixCI.BusinessLogic.Prefix7;
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
        private B700xxFunc b700xxFunc;
        public W10023(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();
            _DB_TYPE = "opt";
            b700xxFunc = new B700xxFunc();
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
                    //全部
                    wf_70010("%");

                    //日盤
                    wf_70010("0");
                    break;
            }
            base.RunBeforeEveryItem(args);

            return rtn;
        }

        private void wf_70010(string marketCode) {
            string saveFilePath;
            string fileName;
            string fileFlag="";
            string symd = txtOcfDate.DateTimeValue.ToString("yyyyMM01");
            string eymd = txtOcfDate.FormatValue;
            switch (marketCode) {
                case "0":
                    fileFlag = "_day";
                    break;
                case "1":
                    fileFlag = "_night";
                    break;
                case "%":
                    fileFlag = "";
                    break;
            }
            #region 日
            fileName = $@"\Daily_OPT{fileFlag}.csv";
            saveFilePath = GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH + fileName;
            b700xxFunc.F70010YmdByMarketCode(saveFilePath, symd, eymd, "D", "O", marketCode);

            //英文版
            fileName = $@"\Daily_OPT{fileFlag}_eng.csv";
            saveFilePath = GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH + fileName;
            b700xxFunc.F70010YmdByMarketCode(saveFilePath, symd, eymd, "D", "O", marketCode,true);
            #endregion

            #region 月
            //string nextYmd = ;
            eymd = txtOcfDate.DateTimeValue.ToString("yyyyMM");
            symd = symd.Substring(0, 6);
            // 當下營業日換月份時,才產出月檔
            if (symd.IndexOf(eymd)<0)
            {
                fileName = $@"Monthly_OPT{fileFlag}.csv";
                saveFilePath = GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH + fileName;
                b700xxFunc.F70010YmdByMarketCode(saveFilePath, symd, eymd, "M", "O", marketCode);

                fileName = $@"Monthly_OPT{fileFlag}_eng.csv";
                saveFilePath = GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH + fileName;
                b700xxFunc.F70010YmdByMarketCode(saveFilePath, symd, eymd, "M", "O", marketCode,true);
            }
            #endregion
        }

    }
}
