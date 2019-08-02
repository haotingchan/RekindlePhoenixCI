using BaseGround;
using BaseGround.Report;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using PhoenixCI.Widget;
using System;
using System.Data;

namespace PhoenixCI.FormUI.PrefixZ
{
    public partial class WZ0998 : FormParent
    {
        private ReportHelper _ReportHelper;


        public WZ0998(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();

            GridHelper.SetCommonGrid(gvMain);
            
            PrintableComponent = gcMain;
            this.Text = _ProgramID + "─" + _ProgramName;


        }

        public override ResultStatus BeforeOpen()
        {
            base.BeforeOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus Open()
        {
            base.Open();

            DropDownList.LookUpItemTxnIdAndName(ddlTxnId);

            return ResultStatus.Success;
        }

        protected override ResultStatus AfterOpen()
        {
            base.AfterOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus ActivatedForm()
        {
            base.ActivatedForm();

            _ToolBtnSave.Enabled = true;
            _ToolBtnRetrieve.Enabled = true;
            _ToolBtnInsert.Enabled = true;
            _ToolBtnDel.Enabled = true;

            return ResultStatus.Success;
        }

        protected override ResultStatus Retrieve()
        {
            base.Retrieve(gcMain);

            gcMain.DataSource = new RPT().ListData(ddlTxnId.EditValue.AsString()+"%",txtTxdId.Text+"%");
            gcMain.Focus();

            return ResultStatus.Success;
        }

        protected override ResultStatus CheckShield()
        {
            base.CheckShield(gcMain);
            if (!IsDataModify(gcMain)) { return ResultStatus.Fail; }
            if (string.IsNullOrEmpty(ddlTxnId.EditValue.AsString()))
            {
                MessageDisplay.Warning("作業代號不可為空白!");
                return ResultStatus.Fail;
            }

            return ResultStatus.Success;
        }

        protected override ResultStatus Save(PokeBall pokeBall)
        {
            try
            {
                DataTable dtCurrent = (DataTable)gcMain.DataSource;
                gvMain.CloseEditor();
                gvMain.UpdateCurrentRow();

                DataTable dtChange = dtCurrent.GetChanges();
                DataTable dtForAdd = dtCurrent.GetChanges(DataRowState.Added);
                DataTable dtForModified = dtCurrent.GetChanges(DataRowState.Modified);
                DataTable dtForDeleted = dtCurrent.GetChanges(DataRowState.Deleted);

                if (dtChange == null)
                {
                    MessageDisplay.Warning("沒有變更資料,不需要存檔!", GlobalInfo.WarningText);
                    return ResultStatus.Fail;
                }
                if (dtChange.Rows.Count == 0)
                {
                    MessageDisplay.Warning("沒有變更資料,不需要存檔!", GlobalInfo.WarningText);
                    return ResultStatus.Fail;
                }

                ResultData result = new RPT().UpdateData(dtChange);

                
                if (result.Status == ResultStatus.Fail)
                {
                    return ResultStatus.Fail;
                }

            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
            return ResultStatus.Success;
        }

        protected override ResultStatus Run(PokeBall args)
        {
            base.Run(gcMain);

            return ResultStatus.Success;
        }

        protected override ResultStatus Import()
        {
            base.Import(gcMain);

            return ResultStatus.Success;
        }

        protected override ResultStatus Export(ReportHelper reportHelper)
        {
            reportHelper = _ReportHelper;
            base.Export(reportHelper);

            return ResultStatus.Success;
        }

        protected override ResultStatus Print(ReportHelper reportHelper)
        {
            base.Print(_ReportHelper);
            return ResultStatus.Success;
        }

        protected override ResultStatus InsertRow()
        {
            int focusIndex = gvMain.GetFocusedDataSourceRowIndex();
            gvMain.CloseEditor();//必須先做close edit, like dt.AcceptChanges();

            //新增一行並做初始值設定
            DataTable dt = (DataTable)gcMain.DataSource;
            DataRow drNew = dt.NewRow();

            drNew["RPT_TXN_ID"] = dt.Rows[focusIndex]["RPT_TXN_ID"];
            drNew["RPT_TXD_ID"] = dt.Rows[focusIndex]["RPT_TXD_ID"];
            drNew["RPT_SEQ_NO"] = dt.Rows[focusIndex]["RPT_SEQ_NO"];

            dt.Rows.InsertAt(drNew, focusIndex);
            gcMain.DataSource = dt;
            gvMain.FocusedRowHandle = focusIndex;//原本的focusRowHandle會記住之前的位置,其實只是往上一行
            gvMain.FocusedColumn = gvMain.Columns[0];

            SetOrder( RPT_SEQ_NO,"I");
            return ResultStatus.Success;
        }

        protected override ResultStatus DeleteRow()
        {
            base.DeleteRow(gvMain);
            SetOrder( RPT_SEQ_NO,"D");


            return ResultStatus.Success;
        }

        protected override ResultStatus BeforeClose()
        {
            return base.BeforeClose();
        }

        protected override ResultStatus COMPLETE()
        {
            MessageDisplay.Info(MessageDisplay.MSG_OK);
            Retrieve();
            return ResultStatus.Success;
        }

        public void SetOrder( GridColumn SEQ_NO,string type)
        {
            DataTable dtCurrent = (DataTable)gvMain.GridControl.DataSource;
            gvMain.CloseEditor();
            gvMain.UpdateCurrentRow();

            int focusIndex = gvMain.GetFocusedDataSourceRowIndex();

            //int pos = 0;

            for (int i = 0; i < dtCurrent.Rows.Count; i++)
            {
                if (i > focusIndex)
                {
                    if (type =="I")
                    {
                        dtCurrent.Rows[i][RPT_SEQ_NO.Name] = i == dtCurrent.Rows.Count -1 ? dtCurrent.Rows[i][RPT_SEQ_NO.Name].AsInt() +1 : dtCurrent.Rows[i+1][RPT_SEQ_NO.Name, DataRowVersion.Original];
                    }
                    else if (type == "D")
                    {
                        dtCurrent.Rows[i][RPT_SEQ_NO.Name] = dtCurrent.Rows[i - 1][RPT_SEQ_NO.Name, DataRowVersion.Original];
                    }
                }
            }
            gvMain.Focus();
            gvMain.FocusedColumn = gvMain.Columns[0];
        }

    }
}