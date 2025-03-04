﻿using ActionService.Extensions;
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
    public partial class W10002 : W1xxx
    {
        public W10002(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();
            _DB_TYPE = "monit";
        }

        protected override ResultStatus Open()
        {
            if (DesignMode) return ResultStatus.Success;
            base.Open();
            txtPrevOcfDate.DateTimeValue = PbFunc.f_ocf_date(5, _DB_TYPE).AsDateTime("yyyyMMdd"); //GlobalInfo.OCF_DATE;
            return ResultStatus.Success;
        }

        protected override ResultStatus RunBefore(PokeBall args)
        {
            base.RunBefore(args);
            if (!servicePrefix1.setPrevOCF(txtPrevOcfDate.DateTimeValue,_DB_TYPE,GlobalInfo.USER_ID))
            {
                return ResultStatus.Fail;
            }

            //主要為monit,但有選擇權的項目
            if (!servicePrefix1.setOCF(txtOcfDate.DateTimeValue, "opt", GlobalInfo.USER_ID))
            {
                return ResultStatus.Fail;
            }
            return ResultStatus.Success;
        }
    }
}