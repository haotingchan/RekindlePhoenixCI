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
using DataObjects.Dao.Together.SpecificDao;
using BusinessObjects.Enums;
using BaseGround.Shared;
using DevExpress.XtraEditors.Controls;
using Common;
using DevExpress.XtraEditors.Repository;
using DevExpress.Utils;

/// <summary>
/// Lukas, 2019/4/17
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
    /// <summary>
    /// 40070
    /// </summary>
    public partial class W40070 : FormParent {

        private D40070 dao40070;

        public W40070(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {
            base.Open();
            //日期
            txtSDate.DateTimeValue = DateTime.Now;
            txtDateG1.DateTimeValue = DateTime.MinValue;
            txtDateG5.DateTimeValue = DateTime.MinValue;
            txtDateG7.DateTimeValue = DateTime.MinValue;

            //設定調整商品條件下拉選單
            List<LookupItem> modelType = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "S", DisplayMember = "SMA達調整標準"},
                                        new LookupItem() { ValueMember = "a", DisplayMember = "任一model達調整標準" },
                                        new LookupItem() { ValueMember = "%", DisplayMember = "全部商品" }};
            Extension.SetDataTable(ddlModel, modelType, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, "");
            ddlModel.EditValue = "S";

            //設定群組
            GRP_NAME.GroupIndex = 0;
            GRP_NAME.Caption = "群組";

            #region RadioGroup
            //設定結算保證金調整金額項目RadioGroup
            // creating and initializing the radio group items 
            RadioGroupItem item1 = new RadioGroupItem();
            item1.Description = "SMA";
            item1.Value = "S";
            RadioGroupItem item2 = new RadioGroupItem();
            item2.Description = "EWMA";
            item2.Value = "E";
            RadioGroupItem item3 = new RadioGroupItem();
            item3.Description = "MaxVol";
            item3.Value = "M";
            RadioGroupItem item4 = new RadioGroupItem();
            item4.Description = "使用者自訂";
            item4.Value = "U";

            RepositoryItemRadioGroup repositoryItemRadioGroup = new RepositoryItemRadioGroup();
            repositoryItemRadioGroup.Items.Add(item1);
            repositoryItemRadioGroup.Items.Add(item2);
            repositoryItemRadioGroup.Items.Add(item3);
            repositoryItemRadioGroup.Items.Add(item4);
            repositoryItemRadioGroup.Columns = 4;
            ADJ_RSN.ColumnEdit = repositoryItemRadioGroup;
            //ADJ_RSN.ColumnEdit.Appearance.TextOptions.HAlignment = HorzAlignment.Near;

            //不處理/觀察/調整 RadioGroup
            RadioGroupItem item5 = new RadioGroupItem();
            item5.Description = "　　";
            item5.Value = "N";
            RadioGroupItem item6 = new RadioGroupItem();
            item6.Description = "　　";
            item6.Value = " ";
            RadioGroupItem item7 = new RadioGroupItem();
            item7.Description = "　　";
            item7.Value = "Y";

            RepositoryItemRadioGroup repositoryItemRadioGroup2 = new RepositoryItemRadioGroup();
            repositoryItemRadioGroup2.Items.Add(item5);
            repositoryItemRadioGroup2.Items.Add(item6);
            repositoryItemRadioGroup2.Items.Add(item7);
            repositoryItemRadioGroup2.Columns = 3;
            ADJ_CODE.ColumnEdit = repositoryItemRadioGroup2;
            ADJ_CODE.ColumnEdit.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
            #endregion

#if DEBUG
            txtSDate.EditValue = "2018/10/11";
#endif

            return ResultStatus.Success;
        }

        protected override ResultStatus ActivatedForm() {
            base.ActivatedForm();

            _ToolBtnInsert.Enabled = false;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
            _ToolBtnSave.Enabled = true;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
            _ToolBtnDel.Enabled = false;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

            _ToolBtnRetrieve.Enabled = true;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
            _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

            _ToolBtnImport.Enabled = false;//匯入
            _ToolBtnExport.Enabled = false;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
            _ToolBtnPrintAll.Enabled = true;//列印

            return ResultStatus.Success;
        }

        protected override ResultStatus Retrieve() {

            try {
                //讀取資料
                dao40070 = new D40070();
                DataTable dt40070 = dao40070.d_40070_scrn(txtSDate.DateTimeValue.ToString("yyyyMMdd"),ddlModel.EditValue.AsString());
                //排序
                dt40070 = dt40070.Sort("OSW_GRP, SEQ_NO, PROD_TYPE, KIND_ID");
                //過濾
                DataView dv = dt40070.AsDataView();
                dv.RowFilter = " ab_type in ('-','A')";
                dt40070 = dv.ToTable();

                gcMain.DataSource = dt40070;
                //預設展開群組
                gvMain.ExpandAllGroups();

                //複製
                //dw_1.RowsCopy(1, dw_1.rowcount(), primary!, ids_tmp, 1, primary!)

                //設定三個Group的生效日期
                string validDateG1, validDateG5, validDateG7;
                int found;
                //Group1
                found = dt40070.Rows.IndexOf(dt40070.Select("osw_grp='1' and issue_begin_ymd is not null ").FirstOrDefault());
                if (found > -1) {
                    txtDateG1.DateTimeValue = dt40070.Rows[found]["ISSUE_BEGIN_YMD"].AsDateTime("yyyyMMdd");
                }
                else {
                    txtDateG1.DateTimeValue = PbFunc.f_get_ocf_next_n_day(txtSDate.DateTimeValue, 1);
                }
                validDateG1 = txtDateG1.Text;
                //Group2
                found = dt40070.Rows.IndexOf(dt40070.Select("osw_grp='5' and issue_begin_ymd is not null ").FirstOrDefault());
                if (found > -1) {
                    txtDateG5.DateTimeValue = dt40070.Rows[found]["ISSUE_BEGIN_YMD"].AsDateTime("yyyyMMdd");
                }
                else {
                    txtDateG5.DateTimeValue = PbFunc.f_get_ocf_next_n_day(txtSDate.DateTimeValue, 2);
                }
                validDateG5 = txtDateG5.Text;
                //Group2
                found = dt40070.Rows.IndexOf(dt40070.Select("osw_grp='7' and issue_begin_ymd is not null ").FirstOrDefault());
                if (found > -1) {
                    txtDateG7.DateTimeValue = dt40070.Rows[found]["ISSUE_BEGIN_YMD"].AsDateTime("yyyyMMdd");
                }
                else {
                    txtDateG7.DateTimeValue = PbFunc.f_get_ocf_next_n_day(txtSDate.DateTimeValue, 2);
                }
                validDateG7 = txtDateG7.Text;
            }
            catch (Exception ex) {
                MessageDisplay.Error("讀取錯誤");
                throw ex;
            }

            return ResultStatus.Success;
        }
    }
}