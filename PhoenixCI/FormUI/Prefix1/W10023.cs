
using BaseGround;
using BaseGround.Shared;
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

            DateTime edate = txtOcfDate.DateTimeValue;
            #region 日
            string symd = edate.ToString("yyyyMM01");
            string eymd = txtOcfDate.FormatValue;
            createFile(symd, eymd, "D", "Daily", marketCode);
            #endregion

            string nextYmd = PbFunc.f_ocf_date(2, _DB_TYPE);
            DateTime nextDate = DateTime.ParseExact(nextYmd, "yyyyMMdd", null);
            DayOfWeek nextYmdDayNum = nextDate.DayOfWeek;

            #region 月
            nextYmd = nextYmd.Substring(0, 6);
            symd = edate.ToString("yyyy01");
            eymd = edate.ToString("yyyyMM");
            // 當下營業日換月份時,才產出月檔
            if (eymd != nextYmd)
            {
                createFile(symd, eymd, "M", "Monthly", marketCode);
            }
            #endregion

            #region 週
            DayOfWeek dayNum = edate.DayOfWeek;
            eymd = txtOcfDate.FormatValue;
            symd = "";
            if (dayNum > nextYmdDayNum && Math.Abs(new TimeSpan(edate.Ticks - nextDate.Ticks).Days) >6)
            {
                createFile(symd, eymd, "D", "Weekly", marketCode);
            }
            #endregion

            #region 年
            eymd = edate.ToString("yyyy");
            nextYmd = nextYmd.Substring(0,4);
            if (eymd != nextYmd)
            {
                symd = (int.Parse(eymd) - 9).ToString();
                createFile(symd, eymd, "Y", "Yearly", marketCode);
            }
            #endregion
        }

        public void createFile(string symd,string eymd,string sumType,string dataType,string marketCode) {
            string fileFlag = "";
            string param = "1002205";
            switch (marketCode)
            {
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


            string fileName = $@"\{dataType}_OPT{fileFlag}.csv";
            string saveFilePath = GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH + fileName;

            //中文版
            if (dataType == "Weekly")
            {
                b700xxFunc.F70010WeekByMarketCode(saveFilePath, symd, eymd, sumType, "O", marketCode);
            }
            else
            {
                b700xxFunc.F70010YmdByMarketCode(saveFilePath, symd, eymd, sumType, "O", marketCode);
            }
            ExecuteFile(param, fileName);
            ExecuteFile(param, $@"\{dataType}_OPT{fileFlag}_OpenData.csv");
            if (dataType == "Daily")
            {
                ExecuteFile(param, $@"\{dataType}_OPT{fileFlag}_W_OpenData.csv");
            }

            //英文版
            fileName = $@"\{dataType}_OPT{fileFlag}_eng.csv";
            saveFilePath = GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH + fileName;
            if (dataType == "Weekly")
            {
                b700xxFunc.F70010WeekByMarketCode(saveFilePath, symd, eymd, sumType, "O", marketCode, true);
            }
            else
            {
                b700xxFunc.F70010YmdByMarketCode(saveFilePath, symd, eymd, sumType, "O", marketCode, true);
            }
            ExecuteFile(param, fileName);
        }

    }
}
