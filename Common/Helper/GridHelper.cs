using Common.Helper.FormatHelper;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraVerticalGrid;
using DevExpress.XtraVerticalGrid.Rows;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Common {
   public static class GridHelper {
      public static void SetCommonGrid(GridView gv) {
         // 關掉Group的Panel
         gv.OptionsView.ShowGroupPanel = false;

         // 置中
         for (int i = 0; i < gv.Columns.Count; i++) {
            gv.Columns[i].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
         }

         // 關閉某些功能
         foreach (GridColumn column in gv.Columns) {
            // 關閉排序欄位
            column.OptionsColumn.AllowSort = DefaultBoolean.False;
            // 關閉移動欄位
            column.OptionsColumn.AllowMove = false;
            // 關閉過濾欄位
            column.OptionsFilter.AllowFilter = false;
         }

         gv.Appearance.HeaderPanel.Options.UseTextOptions = true;
         gv.Appearance.HeaderPanel.TextOptions.WordWrap = WordWrap.Wrap;

         // 設定列印出來Grid的Header的字體和大小
         gv.OptionsPrint.UsePrintStyles = true;
         gv.AppearancePrint.HeaderPanel.Font = new Font("Microsoft YaHei", gv.Appearance.HeaderPanel.Font.Size);
         gv.Appearance.Empty.BackColor = Color.FromArgb(192, 220, 192);
         gv.OptionsView.ColumnHeaderAutoHeight = DefaultBoolean.True;
         gv.AppearancePrint.HeaderPanel.Options.UseTextOptions = true;
         gv.AppearancePrint.Row.Options.UseTextOptions = true;
         gv.AppearancePrint.HeaderPanel.TextOptions.WordWrap = WordWrap.Wrap;

         gv.OptionsPrint.AllowMultilineHeaders = true;

         // 設定欄位Trim
         TrimWhenEditCell(gv.GridControl);

         //隱藏Popup Menu
         gv.OptionsMenu.EnableColumnMenu = false;

         // 如果Grid被Disabled的話，不要變成一片灰色
         gv.GridControl.UseDisabledStatePainter = false;

         gv.BestFitColumns();

         // 當點擊GrivView的Editor時，無法用滑鼠滾輪滾，所以加這個讓我們可以點到欄位內容後還可以滾輪
         gv.MouseWheel += delegate (object sender, MouseEventArgs e) {
            gv.CloseEditor();
         };
      }

      public static void SetCommonGrid(VGridControl gridControl) {
         gridControl.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         gridControl.Appearance.RowHeaderPanel.BackColor = Color.LightGray;
         gridControl.Appearance.VertLine.BackColor = Color.Gray;
         gridControl.Appearance.HorzLine.BackColor = Color.Gray;
         gridControl.Appearance.BandBorder.BackColor = Color.Gray;

         // 欄位置中
         gridControl.Appearance.RowHeaderPanel.TextOptions.HAlignment = HorzAlignment.Center;

         // 關閉某些功能
         foreach (var row in gridControl.Rows) {
            // 關閉移動欄位
            row.OptionsRow.AllowMove = false;
            row.Properties.OptionsFilter.AllowFilter = false;
         }

         // 設定欄位Trim
         TrimWhenEditCell(gridControl);

         //隱藏Popup Menu
         gridControl.OptionsMenu.EnableContextMenu = false;

         // 如果Grid被Disabled的話，不要變成一片灰色
         gridControl.UseDisabledStatePainter = false;
      }

      public static GridControl CloneGrid(GridControl sourceGrid) {
         GridControl resultGrid = new GridControl();

         resultGrid.MainView = new GridView(resultGrid);

         resultGrid.MainView.Assign(sourceGrid.MainView, true);

         resultGrid.Visible = false;

         return resultGrid;
      }

      public static void AddModifyMark(GridControl gridControl, GridColumn MODIFY_MARK) {
         GridView gv = (GridView)gridControl.MainView;

         MODIFY_MARK.OptionsColumn.AllowEdit = false;
            gv.DataSourceChanged += delegate (object sender, EventArgs e) {
                DataTable dt = (DataTable)gridControl.DataSource;
                if (!dt.Columns.Contains(MODIFY_MARK.FieldName))
                {
                    DataColumn mark = new DataColumn("MODIFY_MARK");
                    mark.DefaultValue = " ";
                    dt.Columns.Add(mark);
                }
            };
            gv.CellValueChanging += delegate (object sender, CellValueChangedEventArgs e)
            {
                DataTable dt = (DataTable)gridControl.DataSource;
                if (!dt.Columns.Contains(MODIFY_MARK.FieldName))
                {
                    DataColumn mark = new DataColumn("MODIFY_MARK");
                    mark.DefaultValue = " ";
                    dt.Columns.Add(mark);
                }

                if (gv.GetRowCellValue(e.RowHandle, "OP_TYPE").AsString() != "I")
                {
                    if (e.Value.ToString().Trim() != gv.GetRowCellValue(e.RowHandle, e.Column).ToString().Trim())
                    {
                        if (e.Value.ToString().Trim() != gv.GetRowCellDisplayText(e.RowHandle, e.Column).ToString().Trim())
                        {
                            gv.SetRowCellValue(e.RowHandle, MODIFY_MARK, "※");
                        }
                    }
                }
            };
        }



        public static void AddModifyCheckMark(GridControl gridControl, RepositoryItemCheckEdit repCheck, GridColumn MODIFY_MARK) {
         GridView gv = (GridView)gridControl.MainView;
         MODIFY_MARK.OptionsColumn.AllowEdit = false;
         repCheck.CheckedChanged += delegate (object sender, EventArgs e) {
            DataTable dt = (DataTable)gridControl.DataSource;

            if (!dt.Columns.Contains(MODIFY_MARK.FieldName)) {
               DataColumn colMark = new DataColumn("MODIFY_MARK");
               colMark.DefaultValue = " ";
               dt.Columns.Add(colMark);
            }

            int row = gv.FocusedRowHandle;

            string mark = gv.GetRowCellValue(row, MODIFY_MARK).AsString();

            if (String.IsNullOrEmpty(mark)) {
               gv.SetRowCellValue(row, MODIFY_MARK, "※");
            } else {
               gv.SetRowCellValue(row, MODIFY_MARK, "");
            }
         };
      }

      public static void AddRowNumber(GridControl gridControl, GridColumn ROW_NUMBER) {
         ROW_NUMBER.OptionsColumn.AllowEdit = false;
         ROW_NUMBER.AppearanceCell.Options.UseTextOptions = true;
         ROW_NUMBER.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
         ROW_NUMBER.MinWidth = 40;
         ROW_NUMBER.MaxWidth = 45;

         GridView gv = (GridView)gridControl.MainView;
         gv.CustomColumnDisplayText += delegate (object sender, CustomColumnDisplayTextEventArgs e) {
            if (e.Column.FieldName == "ROW_NUMBER") {
               e.DisplayText = (gv.GetRowHandle(e.ListSourceRowIndex) + 1).ToString();
            }
         };
      }

      public static void AddOpType(GridControl gridControl, GridColumn[] readOnlyColArray) {
         string opTypeColumnName = "OP_TYPE";

         GridView gv = (GridView)gridControl.MainView;

         gv.ShowingEditor += delegate (object sender, System.ComponentModel.CancelEventArgs e) {
            DataTable dt = (DataTable)gridControl.DataSource;

            if (!dt.Columns.Contains(opTypeColumnName)) {
               DataColumn opType = new DataColumn(opTypeColumnName);
               opType.DefaultValue = " ";
               dt.Columns.Add(opType);
            }

            DataRow row = (DataRow)gv.GetFocusedDataRow();

            if (string.IsNullOrEmpty(row[opTypeColumnName].ToString().Trim())) {
               foreach (var col in readOnlyColArray) {
                  if (gv.FocusedColumn == col) {
                     e.Cancel = true;
                  }
               }
            }
         };
      }

      public static void TrimWhenEditCell(GridControl gridControl) {
         GridView gv = (GridView)gridControl.MainView;

         TrimFormatter formatter = new TrimFormatter();

         gv.ShownEditor += delegate (object sender, EventArgs e) {
            GridView view = sender as GridView;
            if (view.ActiveEditor is TextEdit && !(view.ActiveEditor is LookUpEdit)) {
               TextEdit myTextEdit = ((TextEdit)view.ActiveEditor);

               if (!string.IsNullOrEmpty(myTextEdit.Text.Trim())) {
                  myTextEdit.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                  myTextEdit.Properties.EditFormat.Format = formatter;
               }
            }
         };
      }

      public static void TrimWhenEditCell(VGridControl gridControl) {
         TrimFormatter formatter = new TrimFormatter();

         gridControl.ShownEditor += delegate (object sender, EventArgs e) {
            VGridControl control = sender as VGridControl;
            if (control.ActiveEditor is TextEdit && !(control.ActiveEditor is LookUpEdit)) {
               TextEdit myTextEdit = ((TextEdit)control.ActiveEditor);

               if (!string.IsNullOrEmpty(myTextEdit.Text.Trim())) {
                  myTextEdit.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                  myTextEdit.Properties.EditFormat.Format = formatter;
               }
            };
         };
      }

      public static bool CheckRequired(GridControl gridControl, string[] allowColumn = null) {
         DataTable dt = (DataTable)gridControl.DataSource;

         GridView gv = (GridView)gridControl.MainView;
         GridColumnCollection columnList = gv.Columns;

         for (int i = 0; i < dt.Rows.Count; i++) {
            foreach (DataColumn col in dt.Columns) {
               bool allowNull = false;

               if (allowColumn != null) {
                  allowNull = Array.IndexOf(allowColumn, col.ColumnName) >= 0;
               }

               if (dt.Rows[i].RowState != DataRowState.Deleted && string.IsNullOrEmpty(dt.Rows[i][col].ToString()) && allowNull == false) {
                  string warnText = "還有第{0}筆資料[{1}]尚未填寫!";
                  GridColumn column = columnList.ColumnByFieldName(col.ColumnName);

                  // 如果在Grid上面找不到這個欄位，就不檢查
                  if (column != null) {
                     string caption = column.Caption.Trim().Replace("\r\n", "");
                     MessageDisplay.Warning(string.Format(warnText, i + 1, caption));

                     gv.FocusedColumn = column;
                     gv.FocusedRowHandle = i;

                     return false;
                  } else {
                     return true;
                  }
               }
            }
         }
         return true;
      }

      public static bool CheckRequired(VGridControl gridControl, string[] allowColumn = null) {
         DataTable dt = (DataTable)gridControl.DataSource;
         VGridRows columnList = gridControl.Rows;

         for (int i = 0; i < dt.Rows.Count; i++) {
            foreach (DataColumn col in dt.Columns) {
               bool allowNull = false;

               if (allowColumn != null) {
                  allowNull = Array.IndexOf(allowColumn, col.ColumnName) >= 0;
               }

               if (dt.Rows[i].RowState != DataRowState.Deleted && string.IsNullOrEmpty(dt.Rows[i][col].ToString()) && allowNull == false) {
                  string warnText = "還有第{0}筆資料[{1}]尚未填寫!";
                  var column = columnList.ColumnByFieldName(col.ColumnName);

                  string caption = column.Properties.Caption.Trim().Replace("\r\n", "");
                  MessageDisplay.Warning(string.Format(warnText, i + 1, caption));

                  gridControl.FocusedRow = columnList.GetRowByFieldName(col.ColumnName);
                  gridControl.FocusedRecord = 2;
                  return false;
               }
            }
         }
         return true;
      }

      public static void AcceptText(Control control) {
         if (control is GridControl) {
            GridControl gridControl = (GridControl)control;
            gridControl.MainView.CloseEditor();
            gridControl.MainView.UpdateCurrentRow();
         } else if (control is VGridControl) {
            VGridControl vGridControl = (VGridControl)control;
            vGridControl.CloseEditor();
            vGridControl.UpdateFocusedRecord();
         }
      }
   }
}