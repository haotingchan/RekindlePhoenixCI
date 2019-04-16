using BusinessObjects.Enums;
using Common;
using DevExpress.Data;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
         string[] YMD = inputText.Split('/');
         string year = YMD[0].PadLeft(4, '0');
         string month = YMD[1].PadLeft(2, '0');
         string date = YMD[2].PadLeft(2, '0');
         inputText = $"{year}/{month}/{date}";
         if (!DateTime.TryParse(inputText, out dateTime)) {
            MessageDisplay.Error(errorText, GlobalInfo.ErrorText);
            textEdit.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
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
      /// <summary>
      /// datatable排序
      /// </summary>
      /// <param name="sortcondition">排序條件</param>
      /// <returns></returns>
      public static DataTable Sort(this DataTable dataTable, string sortcondition) {
         DataView dv = dataTable.AsDataView();
         dv.Sort = sortcondition;
         DataTable newDT = dv.ToTable();
         return newDT;
      }

      #region Grid欄位相關設定(例如欄位標題/置中/標題自動折行/內容自動折行)

      /// <summary>
      /// 設定column caption (如果沒有該column也不會出錯)
      /// </summary>
      /// <param name="gv"></param>
      /// <param name="colName"></param>
      /// <param name="colCaption"></param>
      public static void SetColumnCaption(this DevExpress.XtraGrid.Views.Grid.GridView gv,
                                              string colName,
                                              string colCaption) {
         if (gv.Columns[colName] != null) {
            gv.Columns[colName].Caption = colCaption;
         }
      }

      /// <summary>
      /// 設定column HAlignment (如果沒有該column也不會出錯)
      /// </summary>
      /// <param name="gv"></param>
      /// <param name="colName"></param>
      /// <param name="colCaption"></param>
      public static void SetColumnHAlignment(this DevExpress.XtraGrid.Views.Grid.GridView gv,
                                                  string colName,
                                                  DevExpress.Utils.HorzAlignment alignment) {
         if (gv.Columns[colName] != null) {
            gv.Columns[colName].AppearanceCell.Options.UseTextOptions = true;
            gv.Columns[colName].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default;
         }
      }

      /// <summary>
      /// 設定column header自動折行 (如果沒有該column也不會出錯)
      /// </summary>
      /// <param name="gv"></param>
      /// <param name="colName"></param>
      /// <param name="colWidth"></param>
      public static void SetColumnHeaderWrap(this DevExpress.XtraGrid.Views.Grid.GridView gv,
                                                  string colName,
                                                  int colWidth) {
         if (gv.Columns[colName] != null) {
            gv.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;//必須先設定全體grid的此屬性,再來才是根據每個欄位限制寬度
            gv.Columns[colName].Width = colWidth;
            gv.Columns[colName].OptionsColumn.FixedWidth = true;
         }
      }

      /// <summary>
      /// 設定column 自動折行 (如果沒有該column也不會出錯)
      /// </summary>
      /// <param name="gv"></param>
      /// <param name="colName"></param>
      /// <param name="colWidth"></param>
      /// <param name="memoEdit">如果該欄位不可編輯,直接帶null即可</param>
      public static void SetColumnWrap(this DevExpress.XtraGrid.Views.Grid.GridView gv,
                                          string colName,
                                          int colWidth,
                                          DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit memoEdit = null) {
         if (gv.Columns[colName] != null) {
            gv.OptionsView.RowAutoHeight = true;//必須先設定全體grid的此屬性,再來才是根據每個欄位限制寬度
            gv.Columns[colName].Width = colWidth;
            gv.Columns[colName].OptionsColumn.FixedWidth = true;
            if (memoEdit == null)
               memoEdit = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            gv.Columns[colName].ColumnEdit = memoEdit;
         }
      }

      /// <summary>
      /// 設定column Summary (如果沒有該column也不會出錯)
      /// </summary>
      /// <param name="gv"></param>
      /// <param name="showColFieldName">要在哪個欄位顯示Summary Value</param>
      /// <param name="summaryFieldName">要Summary 哪個欄位</param>
      /// <param name="DisplayFormat"></param>
      /// <param name="summaryItemType"></param>
      public static void SetGridSummary(this DevExpress.XtraGrid.Views.Grid.GridView gv,
          string showColFieldName,
          string summaryFieldName,
          string DisplayFormat,
          SummaryItemType summaryItemType) {
         if (gv.Columns[showColFieldName] != null && gv.Columns[summaryFieldName] != null) {
            GridColumnSummaryItem columnSummary = new GridColumnSummaryItem();
            columnSummary.FieldName = summaryFieldName;
            columnSummary.SummaryType = summaryItemType;
            columnSummary.DisplayFormat = DisplayFormat;
            gv.Columns[showColFieldName].Summary.Add(columnSummary);
         }
      }

      /// <summary>
      /// 設定column group Summary (如果沒有該column也不會出錯)
      /// </summary>
      /// <param name="gv"></param>
      /// <param name="summaryFieldName">要Summary 哪個欄位</param>
      /// <param name="DisplayFormat"></param>
      /// <param name="summaryItemType"></param>
      /// <param name="showInFooter">要不要顯示在 group footer</param>
      /// <param name="footerColFieldName">group footer 欄位</param>
      public static void SetGridGroupSummary(this DevExpress.XtraGrid.Views.Grid.GridView gv,
          string summaryFieldName,
          string DisplayFormat,
          SummaryItemType summaryItemType,
          bool showInFooter = false,
          string footerColFieldName = "") {

         if (gv.Columns[summaryFieldName] == null) return;

         GridGroupSummaryItem groupSummary = new GridGroupSummaryItem();
         groupSummary.FieldName = summaryFieldName;
         groupSummary.SummaryType = summaryItemType;
         groupSummary.DisplayFormat = DisplayFormat;

         if (showInFooter) {
            groupSummary.ShowInGroupColumnFooter = gv.Columns[footerColFieldName];
         }
         gv.GroupSummary.Add(groupSummary);
      }

      #endregion

      #region rtf 相關設定

      /// <summary>
      /// Cell 邊框設定
      /// </summary>
      /// <param name="cell"></param>
      /// <param name="TopStyle"></param>
      /// <param name="LeftStyle"></param>
      /// <param name="RightStyle"></param>
      public static void CellSetBorders(this TableCell cell, TableBorderLineStyle TopStyle, 
         TableBorderLineStyle LeftStyle, TableBorderLineStyle RightStyle) {

         cell.Borders.Top.LineStyle = TopStyle;
         cell.Borders.Left.LineStyle = LeftStyle;
         cell.Borders.Right.LineStyle = RightStyle;
      }

      #endregion
   }
}



