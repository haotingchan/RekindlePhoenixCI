using BusinessObjects.Enums;
using Common;
using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

/// <summary>
/// ken,2019/1/3
/// </summary>
namespace BaseGround.Shared {
    /// <summary>
    /// LookUpEdit擴充方法
    /// </summary>
    public static class Extension {

        /// <summary>
        /// 列舉出enum所有值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetValues<T>() {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        /// <summary>
        /// 直接設定DataTable給lookupEdit
        /// </summary>
        /// <param name="cbx"></param>
        /// <param name="dataSource">dataTable or Dictionary(k,v)</param>
        /// <param name="valueMember">valueMember</param>
        /// <param name="displayMember">displayMember</param>
        /// <param name="textEditStyles">Standard=可輸入  DisableTextEditor=不可輸入,單純的下拉選單</param>
        /// <param name="nullText">nullText</param>
        public static void SetDataTable(this DevExpress.XtraEditors.LookUpEdit cbx,
                                          object dataSource,
                                          string valueMember,
                                          string displayMember = "CP_DISPLAY",
                                          TextEditStyles textEditStyles = TextEditStyles.Standard,
                                          string nullText = "--請選擇--") {

            //一些基本參數設定
            cbx.EnterMoveNextControl = true;
            cbx.Properties.NullText = nullText;
            cbx.Properties.DropDownRows = 10;//一次顯示的下拉選單項目數量(太多或太少都不好)
            cbx.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            cbx.Properties.SearchMode = SearchMode.AutoFilter;//SearchMode.AutoFilter=輸入框每次輸入,都直接過濾下拉選項
            cbx.Properties.AutoSearchColumnIndex = 1;
            cbx.Properties.TextEditStyle = textEditStyles;//Standard=可輸入  DisableTextEditor=不可輸入,單純的下拉選單

            cbx.Properties.ShowHeader = false;
            cbx.Properties.ShowFooter = false;

            cbx.Properties.DataSource = dataSource;
            cbx.Properties.ForceInitialize();
            cbx.Properties.PopulateColumns();
            cbx.Properties.DisplayMember = displayMember;
            cbx.Properties.ValueMember = valueMember;

            //把不需要顯示的欄位隱藏
            //ken,如果是Dictionary<Key,Value>,則Key在PopulateColumns時其Visible就直接為false
            foreach (LookUpColumnInfo col in cbx.Properties.Columns) {
                if (!string.IsNullOrEmpty(displayMember))
                    if (col.FieldName.ToUpper() != displayMember.ToUpper())
                        col.Visible = false;
            }

        }
        /// <summary>
        /// 直接設定DataTable給RepositoryItemLookUpEdit
        /// </summary>
        /// <param name="cbx"></param>
        /// <param name="dataSource">dataTable or Dictionary(k,v)</param>
        /// <param name="valueMember">valueMember</param>
        /// <param name="displayMember">displayMember</param>
        /// <param name="textEditStyles">Standard=可輸入  DisableTextEditor=不可輸入,單純的下拉選單</param>
        /// <param name="nullText">nullText</param>
        public static void SetColumnLookUp(this DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cbx,
                                          object dataSource,
                                          string valueMember,
                                          string displayMember = "CP_DISPLAY",
                                          TextEditStyles textEditStyles = TextEditStyles.Standard,
                                          string nullText = "--請選擇--"
                                          ) {

            //一些基本參數設定
            //cbx.EnterMoveNextControl = true;
            cbx.NullText = nullText;
            cbx.DropDownRows = 10;//一次顯示的下拉選單項目數量(太多或太少都不好)
            cbx.BestFitMode = BestFitMode.BestFitResizePopup;
            cbx.SearchMode = SearchMode.AutoFilter;//SearchMode.AutoFilter=輸入框每次輸入,都直接過濾下拉選項
            cbx.AutoSearchColumnIndex = 1;
            cbx.TextEditStyle = textEditStyles;//Standard=可輸入  DisableTextEditor=不可輸入,單純的下拉選單

            cbx.ShowHeader = false;
            cbx.ShowFooter = false;

            cbx.DataSource = dataSource;
            cbx.ForceInitialize();
            cbx.PopulateColumns();
            cbx.DisplayMember = displayMember;
            cbx.ValueMember = valueMember;
            //把不需要顯示的欄位隱藏
            foreach (LookUpColumnInfo col in cbx.Columns) {
                if (!string.IsNullOrEmpty(displayMember))
                    if (col.FieldName.ToUpper() != displayMember.ToUpper())
                        col.Visible = false;
            }

        }


        /// <summary>
        /// 改良過的substring,可以避開長度不夠導致錯誤的問題
        /// </summary>
        /// <param name="strData"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string SubStr(this string strData, int startIndex, int length) {
            int intLen = strData.Length;
            int intSubLen = intLen - startIndex;
            string strReturn;
            if (length == 0)
                strReturn = "";
            else {
                if (intLen <= startIndex)
                    strReturn = "";
                else {
                    if (length > intSubLen)
                        length = intSubLen;

                    strReturn = strData.Substring(startIndex, length);
                }
            }
            return strReturn;
        }
        /// <summary>
        /// TextEdit驗證DateTime格式
        /// </summary>
        /// <param name="textEdit"></param>
        /// <param name="inputText"></param>
        /// <param name="errorText"></param>
        /// <returns></returns>
        public static bool IsDate(this DevExpress.XtraEditors.TextEdit textEdit, string inputText, string errorText) {
            DateTime dateTime;
            if (!DateTime.TryParse(inputText, out dateTime)) {
                MessageDisplay.Error(errorText, GlobalInfo.ErrorText);
                textEdit.Text = GlobalInfo.OCF_DATE.ToShortDateString();
                textEdit.Focus();
                return false;
            }
            return true;
        }
        /// <summary>
        /// datatable過濾條件
        /// </summary>
        /// <param name="setfilter">過濾條件</param>
        /// <returns></returns>
        public static DataTable Filter(this DataTable dataTable, string setfilter) {
            DataView dv = dataTable.AsDataView();
            dv.RowFilter = setfilter;
            DataTable newDT = dv.ToTable();
            return newDT;
        }
    }
}



