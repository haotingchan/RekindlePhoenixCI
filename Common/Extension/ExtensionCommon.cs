using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;

namespace Common {
    public static class ExtensionCommon {
        #region GridView

        /// <summary>
        /// 把Grid裡面的Cell的空白都去除掉
        /// </summary>
        public static void TrimAllCell(this GridView gv) {
            gv.CustomColumnDisplayText += (object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e) => {
                if (e.Column.ColumnType == typeof(string))
                    e.DisplayText = e.Value.ToString().Trim();
            };
        }

        public static DataView Find(this GridView gv, string filterStr) {
            DataTable dt = ((DataView)gv.DataSource).Table;

            DataView dv = new DataView(dt);
            dv.RowFilter = filterStr;

            return dv;
        }

        public static DataRow AddRow(this GridView gv) {
            gv.AddNewRow();
            int rowIndex = gv.GetVisibleIndex(gv.GetRowHandle(gv.DataRowCount));
            gv.UpdateCurrentRow();

            return ((DataRowView)gv.GetRow(rowIndex)).Row;
        }

        #endregion

        #region object

        // transform object into Identity data type (integer).
        public static int AsId(this object item, int defaultId = -1) {
            if (item == null)
                return defaultId;

            int result;
            if (!int.TryParse(item.ToString(), out result))
                return defaultId;

            return result;
        }

        // transform object into integer data type.
        public static int AsInt(this object item, int defaultInt = default(int)) {
            if (item == null)
                return defaultInt;

            int result;
            if (!int.TryParse(item.ToString(), out result))
                return defaultInt;

            return result;
        }

        // transform object into double data type
        public static double AsDouble(this object item, double defaultDouble = default(double)) {
            if (item == null)
                return defaultDouble;

            double result;
            if (!double.TryParse(item.ToString(), out result))
                return defaultDouble;

            return result;
        }

        public static decimal AsDecimal(this object item, decimal defaultDecimal = default(decimal)) {
            if (item == null)
                return defaultDecimal;

            decimal result;
            if (!decimal.TryParse(item.ToString(), out result))
                return defaultDecimal;

            return result;
        }

        // transform object into string data type
        public static string AsString(this object item, string defaultString = default(string)) {
            if (item == null || item.Equals(System.DBNull.Value))
                return defaultString;

            return item.ToString().Trim();
        }

        // transform object into DateTime data type.
        public static DateTime AsDateTime(this object item, DateTime defaultDateTime = default(DateTime)) {
            if (item == null || string.IsNullOrEmpty(item.ToString()))
                return defaultDateTime;

            DateTime result;
            if (!DateTime.TryParse(item.ToString(), out result))
                return defaultDateTime;

            return result;
        }

        // transform object into DateTime data type format.
        public static DateTime AsDateTime(this object item, string format, DateTime defaultDateTime = default(DateTime)) {
            if (item == null || string.IsNullOrEmpty(item.ToString()))
                return defaultDateTime;

            DateTime result;
            if (!DateTime.TryParseExact(item.ToString(), format, null, DateTimeStyles.AllowWhiteSpaces, out result))
                return defaultDateTime;
            return result;
        }

        // transform object into bool data type
        public static bool AsBool(this object item, bool defaultBool = default(bool)) {
            if (item == null)
                return defaultBool;

            return new List<string>() { "yes", "y", "true" }.Contains(item.ToString().ToLower());
        }

        public static string AsBase64String(this object item) {
            if (item == null)
                return null;
            ;

            return Convert.ToBase64String((byte[])item);
        }

        // transform object into Guid data type
        public static Guid AsGuid(this object item) {
            try { return new Guid(item.ToString()); }
            catch { return Guid.Empty; }
        }

        //number convert to Percent
        public static string AsPercent(this object item, int digits) {
            if (item == null) {
                return "0%";
            }
            try {
                double input = item.AsDouble();
                input = Math.Round((input * 100), digits, MidpointRounding.AwayFromZero);
                return (input.ToString()) + "%";
            }
            catch {
                return "0%";
            }
        }

        #endregion

        #region string

        // transform string into byte array
        public static byte[] AsByteArray(this string s) {
            if (string.IsNullOrEmpty(s))
                return null;

            return Convert.FromBase64String(s);
        }

        // transform object into base64 string.


        // concatenates SQL and ORDER BY clauses into a single string
        public static string OrderBy(this string sql, string sortExpression) {
            if (string.IsNullOrEmpty(sortExpression))
                return sql;

            return sql + " ORDER BY " + sortExpression;
        }

        #endregion

        #region DateTime

        /// <summary>
        /// 將日期轉成英文的月份短名，像是Jan,Feb,Mar這樣
        /// </summary>
        public static string ToShortEnglishMonthWithDot(this DateTime item) {
            return item.ToString("MMM", CultureInfo.InvariantCulture) + ".";
        }

        /// <summary>
        /// 將日期轉成英文的月份長名，像是January,February,March這樣
        /// </summary>
        public static string ToLongEnglishMonth(this DateTime item) {
            return item.ToString("MMMM", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 換成兩位數的月份，例如1變成01,2變成02,3變成03,12還是12
        /// </summary>
        public static string ToTwoNumMonth(this DateTime item) {
            return item.Month.ToString("d2");
        }

        #endregion

        #region DataTable
        /// <summary>
        /// "INDEX"
        /// </summary>
        public static readonly string rowindex = "cp_row";
        /// <summary>
        /// 在DataTable中添加一筆數列，編號從1依次遞增
        /// </summary>
        /// <param >DataTable</param>
        /// <returns></returns>
        public static DataTable AddSeriNumToDataTable(DataTable dt) {
            //需要返回的值
            DataTable dtNew;
            if (dt.Columns.IndexOf(rowindex) >= 0) {
                dtNew = dt;
            }
            else //添加一筆數列,並且在第一列
            {
                int rowLength = dt.Rows.Count;
                int colLength = dt.Columns.Count;
                DataRow[] newRows = new DataRow[rowLength];

                dtNew = new DataTable();
                //在第一列添加“筆數”列
                dtNew.Columns.Add(rowindex);
                for (int i = 0; i < colLength; i++) {
                    dtNew.Columns.Add(dt.Columns[i].ColumnName);
                    //複製dt中的數據
                    for (int j = 0; j < rowLength; j++) {
                        if (newRows[j] == null)
                            newRows[j] = dtNew.NewRow();
                        //將其他數據填充到第二列之後，因為第一列為新增的序號列
                        newRows[j][i + 1] = dt.Rows[j][i];
                    }
                }
                foreach (DataRow row in newRows) {
                    dtNew.Rows.Add(row);
                }

            }
            //對序號列填充，從1遞增
            for (int i = 0; i < dt.Rows.Count; i++) {
                dtNew.Rows[i][rowindex] = i + 1;
            }

            return dtNew;
        }
        /// <summary>
        /// Data欄位最大長度
        /// </summary>
        /// <param name="table">DataTable</param>
        /// <param name="colName">欄位名稱</param>
        /// <returns>int</returns>
        public static int GetMaxColLen(DataTable table, string colName) {
            if (table.Rows.Count == 0)
                return 0;

            DataTable tmpTable = table.Copy();
            tmpTable.Columns.Add("ColLength", typeof(int));
            tmpTable.Columns["ColLength"].Expression = string.Format("len({0})", colName);
            tmpTable.DefaultView.Sort = "ColLength desc";

            return tmpTable.DefaultView[0]["ColLength"].AsInt();
        }
        #endregion
        /// <summary>
        /// 得到Enum的中文說明(Description)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDesc(this Enum value) {
            Type type = value.GetType();

            //// 利用反射找出相對應的欄位
            var field = type.GetField(value.ToString());
            //// 取得欄位設定DescriptionAttribute的值
            var customAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            //// 無設定Description Attribute, 回傳Enum欄位名稱
            if (customAttribute == null || customAttribute.Length == 0) {
                return value.ToString();
            }

            //// 回傳Description Attribute的設定
            return ((DescriptionAttribute)customAttribute[0]).Description;
        }
    }
}