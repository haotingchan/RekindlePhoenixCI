using ActionService;
using ActionService.DbDirect;
using ActionService.Extensions;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace PhoenixCI.Widget
{
    public class DropDownList
    {
        public static void SetCommonLookUp(RepositoryItemLookUpEdit repLookUp)
        {
            var ds = repLookUp.DataSource;
            if (ds is DataTable)
            {
                repLookUp.DropDownRows = ((DataTable)ds).Rows.Count;
            }
            else if (ds is IList)
            {
                repLookUp.DropDownRows = ((IList)ds).Count;
            }
            repLookUp.NullText = "";
            repLookUp.ShowFooter = false;
            repLookUp.ShowHeader = false;
            repLookUp.ShowLines = false;
            repLookUp.BestFitMode = BestFitMode.BestFitResizePopup;
            repLookUp.PopulateColumns();
            repLookUp.PopupFormMinSize = new System.Drawing.Size(10, 10);
        }

        public static RepositoryItemLookUpEdit Get(List<DictionaryEntry> list)
        {
            RepositoryItemLookUpEdit repLookUp = new RepositoryItemLookUpEdit();

            repLookUp.ValueMember = "Value";
            repLookUp.DisplayMember = "Key";
            repLookUp.DataSource = list;
            DropDownList.SetCommonLookUp(repLookUp);
            repLookUp.Columns["Value"].Visible = false;

            return repLookUp;
        }

        public static RepositoryItemLookUpEdit CurrencyMapping(string COL_CURRENCY_NAME, string COL_CURRENCY_VALUE)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(COL_CURRENCY_NAME);
            dt.Columns.Add(COL_CURRENCY_VALUE);

            DataRow newRow = dt.NewRow();
            newRow[COL_CURRENCY_NAME] = "臺幣";
            newRow[COL_CURRENCY_VALUE] = "1";
            dt.Rows.Add(newRow);

            newRow = dt.NewRow();
            newRow[COL_CURRENCY_NAME] = "美元";
            newRow[COL_CURRENCY_VALUE] = "2";
            dt.Rows.Add(newRow);

            newRow = dt.NewRow();
            newRow[COL_CURRENCY_NAME] = "歐元";
            newRow[COL_CURRENCY_VALUE] = "3";
            dt.Rows.Add(newRow);

            newRow = dt.NewRow();
            newRow[COL_CURRENCY_NAME] = "日幣";
            newRow[COL_CURRENCY_VALUE] = "4";
            dt.Rows.Add(newRow);

            newRow = dt.NewRow();
            newRow[COL_CURRENCY_NAME] = "英鎊";
            newRow[COL_CURRENCY_VALUE] = "5";
            dt.Rows.Add(newRow);

            newRow = dt.NewRow();
            newRow[COL_CURRENCY_NAME] = "澳幣";
            newRow[COL_CURRENCY_VALUE] = "6";
            dt.Rows.Add(newRow);

            newRow = dt.NewRow();
            newRow[COL_CURRENCY_NAME] = "港幣";
            newRow[COL_CURRENCY_VALUE] = "7";
            dt.Rows.Add(newRow);

            newRow = dt.NewRow();
            newRow[COL_CURRENCY_NAME] = "人民幣";
            newRow[COL_CURRENCY_VALUE] = "8";
            dt.Rows.Add(newRow);

            newRow = dt.NewRow();
            newRow[COL_CURRENCY_NAME] = "南非幣";
            newRow[COL_CURRENCY_VALUE] = "A";
            dt.Rows.Add(newRow);

            newRow = dt.NewRow();
            newRow[COL_CURRENCY_NAME] = "紐幣";
            newRow[COL_CURRENCY_VALUE] = "G";
            dt.Rows.Add(newRow);

            RepositoryItemLookUpEdit repLookUp = new RepositoryItemLookUpEdit();
            repLookUp.DisplayMember = COL_CURRENCY_NAME;
            repLookUp.ValueMember = COL_CURRENCY_VALUE;
            repLookUp.DataSource = dt;
            SetCommonLookUp(repLookUp);
            repLookUp.Columns[COL_CURRENCY_VALUE].Visible = false;

            return repLookUp;
        }

        public static RepositoryItemLookUpEdit TxnType()
        {
            List<DictionaryEntry> txnTypeList = new List<DictionaryEntry>();
            txnTypeList.Add(new DictionaryEntry("新增", "I"));
            txnTypeList.Add(new DictionaryEntry("查詢，刪除", "D"));
            txnTypeList.Add(new DictionaryEntry("變更", "U"));
            txnTypeList.Add(new DictionaryEntry("維護", "A"));
            txnTypeList.Add(new DictionaryEntry("其他", "O"));
            txnTypeList.Add(new DictionaryEntry("PROC", "S"));
            txnTypeList.Add(new DictionaryEntry("報表", "R"));

            RepositoryItemLookUpEdit repLookUp = Get(txnTypeList);

            return repLookUp;
        }

        public static ComboBox CreateComboBoxDptIdAndName(ComboBox cbx)
        {
         ServiceCommon serviceCommon = new ServiceCommon();
         DataTable dt = serviceCommon.ListDPT().Trim();
            cbx.DisplayMember = "DPT_ID_NAME";
            cbx.ValueMember = "DPT_ID";
            cbx.DataSource = dt;
            cbx.SelectedItem = null;

            return cbx;
        }

        public static RepositoryItemLookUpEdit CreateRepositoryItemDptIdAndName(ComboBox cbx)
        {
            RepositoryItemLookUpEdit repLookUp = new RepositoryItemLookUpEdit();
            repLookUp.DisplayMember = cbx.DisplayMember;
            repLookUp.ValueMember = cbx.ValueMember;
            repLookUp.DataSource = cbx.DataSource;
            SetCommonLookUp(repLookUp);
            repLookUp.Columns[cbx.ValueMember].Visible = false;
            repLookUp.Columns["DPT_NAME"].Visible = false;
            repLookUp.Columns["DPT_SEQ_NO"].Visible = false;

            return repLookUp;
        }

        public static ComboBox ComboBoxUserIdAndName(ComboBox cbx)
        {
         ServiceCommon serviceCommon = new ServiceCommon();
         DataTable dt = serviceCommon.ListDataForUserIDAndUserName().Trim();
            cbx.DisplayMember = "UserIDAndUserName";
            cbx.ValueMember = "UPF_USER_ID";
            cbx.DataSource = dt;
            cbx.SelectedItem = null;

            return cbx;
        }

        public static ComboBox ComboBoxTxnIdAndName(ComboBox cbx)
        {
         ServiceCommon serviceCommon = new ServiceCommon();
         DataTable dt = serviceCommon.ListDataForTxnIdAndName();
            cbx.DisplayMember = "TxnIdAndName";
            cbx.ValueMember = "TXN_ID";
            cbx.DataSource = dt;
            cbx.SelectedItem = null;

            return cbx;
        }
    }
}