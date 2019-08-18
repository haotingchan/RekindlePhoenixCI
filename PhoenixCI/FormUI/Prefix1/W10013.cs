using BaseGround;
using BaseGround.Shared;
using BaseGround.Widget;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
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
        private D30055 dao30055;
        public W10013(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();
            _DB_TYPE = "fut";
            dao30055 = new D30055();
        }
        protected override ResultData ExecuteForm(PokeBall args)
        {
            DateTime ocfDate = txtOcfDate.DateTimeValue;
            ResultData resultData = new ResultData();
            int oswGrp = 5;
            
            switch (args.TXF_TID)
            {
                case "w_30053":
                    int seq;
                    //判斷統計資料轉檔已完成
                    for (int f = 1; f <= 2; f++)
                    {
                        if (f == 1)
                        {
                            seq = 13;
                        }
                        else
                        {
                            seq = 23;
                        }
                        string rtnStr = PbFunc.f_get_jsw_seq("30053", "E", seq, ocfDate, "0");
                        if (rtnStr != "")
                        {
                            resultData.returnString = $"{txtOcfDate.Text}統計資料未轉入完畢，請確認監視批次「AIG5402」執行完成。";
                            resultData.Status = ResultStatus.Fail;
                            return resultData;
                        }
                    }
                    break;
                case "w_30055":
                    //檢查期貨三大法人資料已轉入
                    if (dao30055.GetBTINOIVL3FCount(ocfDate) == 0)
                    {
                        resultData.returnString = "「期貨三大法人」資料還未轉入\n★★★請確認「監視 AI402 」作業已完成";
                        resultData.Status = ResultStatus.Fail;
                        return resultData;
                    }
                    
                    //檢查選擇權三大法人資料已轉入
                    if (dao30055.GetBTINOIVL4Count(ocfDate)== 0)
                    {
                        resultData.returnString = "「選擇權三大法人」資料還未轉入\n★★★請確認「監視 AI402 」作業已完成";
                        resultData.Status = ResultStatus.Fail;
                        return resultData;
                    }

                    //檢查大額交易人資料已轉入
                    if (dao30055.GetTOI1Count(ocfDate) == 0)
                    {
                        resultData.returnString = "「大額交易人」資料還未轉入!\n★★★請確認「10052－監視轉檔」 作業已完成";
                        resultData.Status = ResultStatus.Fail;
                        return resultData;
                    }

                    //期貨行情
                    if (dao30055.GetAMIFCount(ocfDate,"F", oswGrp) == 0)
                    {
                        resultData.returnString = $"期貨Group:{oswGrp}行情資料(ci.AMIF)還未轉入!\n★★★請確認「10013－期貨轉檔－sp_F_gen_H_AMIF」 作業已完成";
                        resultData.Status = ResultStatus.Fail;
                        return resultData;
                    }
                    
                    //選擇權行情
                    if (dao30055.GetAMIFCount(ocfDate, "O", oswGrp) == 0)
                    {
                        resultData.returnString = $"選擇權Group:{oswGrp}行情資料(ci.AMIF)還未轉入!\n★★★請確認「10023－期貨轉檔－sp_O_gen_H_AMIF」 作業已完成";
                        resultData.Status = ResultStatus.Fail;
                        return resultData;
                    }

                    //期貨未平倉量
                    if (dao30055.GetAI2Count(ocfDate, "F", oswGrp) == 0)
                    {
                        resultData.returnString = $"期貨Group:{oswGrp}行情統計檔(ci.AI2)還未轉入!\n★★★請確認「10013－期貨轉檔－sp_F_stt_H_AI2_day」 作業已完成";
                        resultData.Status = ResultStatus.Fail;
                        return resultData;
                    }

                    //選擇權未平倉量
                    if (dao30055.GetAI2Count(ocfDate, "O", oswGrp) == 0)
                    {
                        resultData.returnString = $"選擇權Group:{oswGrp} 行情統計檔(ci.AI2)還未轉入!\n★★★請確認「10023－選擇權轉檔－sp_F_stt_H_AI2_day」 作業已完成";
                        resultData.Status = ResultStatus.Fail;
                        return resultData;
                    }
                    break;
            }
            resultData = base.ExecuteForm(args);
            

            return resultData;
        }

        protected override void ExecuteFormBefore(FormParent formInstance,PokeBall args)
        {
            ((CheckBox)formInstance.Controls.Find("cbxNews", true)[0]).Checked = true;
            ((TextDateEdit)formInstance.Controls.Find("txtSDate", true)[0]).DateTimeValue = txtOcfDate.DateTimeValue;

            if (args.TXF_TID == "w_30055")
            {
                ((CheckBox)formInstance.Controls.Find("cbxTJF", true)[0]).Checked = true;
            }
        }
    }
}
