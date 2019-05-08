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
using DataObjects.Dao.Together.TableDao;
using Common;
using BusinessObjects.Enums;
using BaseGround.Shared;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;

/// <summary>
/// Lukas, 2019/5/8
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {

    public partial class W40072 : FormParent {

        /// <summary>
        /// 調整類型 0一般 1長假 2處置股票 3股票
        /// </summary>
        protected string is_adj_type { get; set; }
        /// <summary>
        /// 交易日期
        /// </summary>
        protected string ymd { get; set; }
        private D40071 dao40071;
        private MGD2 daoMGD2;
        private MGD2L daoMGD2L;
        private RepositoryItemLookUpEdit rateLookUpEdit;

        public W40072(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            dao40071 = new D40071();
            daoMGD2 = new MGD2();
            daoMGD2L = new MGD2L();
            GridHelper.SetCommonGrid(gvMain);
            GridHelper.SetCommonGrid(gvDetail);
            gvDetail.AppearancePrint.BandPanel.Font = new Font("Microsoft YaHei", 10);
            gvDetail.AppearancePrint.BandPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
        }

        protected override ResultStatus Open() {
            base.Open();
            //設定日期和全域變數
            txtSDate.DateTimeValue = DateTime.Now;
            ymd = txtSDate.DateTimeValue.ToString("yyyyMMdd");
            is_adj_type = "2";
#if DEBUG
            txtSDate.EditValue = "2018/12/28";
#endif
            //取得table的schema，因為程式開啟會預設插入一筆空資料列
            DataTable dtMGD2 = dao40071.d_40071();
            gcMain.DataSource = dtMGD2;

            #region 下拉選單設定
            //調整倍數下拉選單
            List<LookupItem> rateList = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "1.5", DisplayMember = "1.5"},
                                        new LookupItem() { ValueMember = "2", DisplayMember = "2"},
                                        new LookupItem() { ValueMember = "3", DisplayMember = "3" }};
            rateLookUpEdit = new RepositoryItemLookUpEdit();
            rateLookUpEdit.SetColumnLookUp(rateList, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, null);
            RATE_INPUT.ColumnEdit = rateLookUpEdit;

            #endregion

            //預設新增一筆設定資料
            InsertRow();


            //txtSDate.Focus();
            return ResultStatus.Success;
        }

        protected override ResultStatus InsertRow() {
            base.InsertRow(gvMain);

            return ResultStatus.Success;
        }
    }
}