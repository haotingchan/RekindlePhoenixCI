using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.Linq;

/// <summary>
/// Winni, 2019/3/18
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 期票期貨每月交易概況統計表
   /// </summary>
   public partial class W30414 : FormParent {

      protected enum SheetNo {
         tradeSum = 0, //每月交易概況
         dailyAvg = 1, //每月日均量
         oint = 2 //每月日均未平倉量
      }

      #region 一般交易查詢條件縮寫
      /// <summary>
      /// yyyyMM
      /// </summary>
      public string StartMon {
         get {
            return txtStartMon.DateTimeValue.ToString("yyyyMM");
         }
      }

      /// <summary>
      /// yyyyMM
      /// </summary>
      public string EndMon {
         get {
            return txtEndMon.DateTimeValue.ToString("yyyyMM");
         }
      }
      #endregion

      public W30414(string programID , string programName) : base(programID , programName) {
         InitializeComponent();

         this.Text = _ProgramID + "─" + _ProgramName;
         txtStartMon.DateTimeValue = GlobalInfo.OCF_DATE;
         txtEndMon.DateTimeValue = GlobalInfo.OCF_DATE;

#if DEBUG
         //winni test
         txtStartMon.DateTimeValue = DateTime.ParseExact("2018/03" , "yyyy/MM" , null);
         txtEndMon.DateTimeValue = DateTime.ParseExact("2018/12" , "yyyy/MM" , null);
         this.Text += "(開啟測試模式),Date=2018/03~2018/12";
#endif

      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Export() {
         try {

            #region 輸入&日期檢核

            if (txtStartMon.Text.SubStr(0 , 4) != txtEndMon.Text.SubStr(0 , 4)) {
               MessageDisplay.Error("不可跨年度查詢!");
               return ResultStatus.Fail;
            }

            if (!txtStartMon.IsDate(txtStartMon.Text + "/01" , CheckDate.Start)
                  || !txtEndMon.IsDate(txtEndMon.Text + "/01" , CheckDate.End)) {
               return ResultStatus.Fail;
            }

            if (string.Compare(txtStartMon.Text , txtEndMon.Text) > 0) {
               MessageDisplay.Error(GlobalInfo.ErrorText , CheckDate.Datedif);
               return ResultStatus.Fail;
            }
            #endregion

            //1. ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "訊息：資料轉出中........";
            this.Refresh();

            //2. copy template xls to target path
            string excelDestinationPath = PbFunc.wf_copy_file(_ProgramID , _ProgramID);

            //3. open xls
            Workbook workbook = new Workbook();
            workbook.LoadDocument(excelDestinationPath);

            //4. 年月表頭
            DataTable dtYm = new D30414().ListTitleByMon(StartMon , EndMon);
            if (dtYm.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0}~{1},{2}-年月,無任何資料!" , StartMon , EndMon , _ProgramID));
               WriteLog(string.Format("{0}~{1},{2}-年月,無任何資料!" , StartMon , EndMon , _ProgramID));
            }//if (dtYm.Rows.Count <= 0)

            //5. 填資料
            bool res1 = false, res2 = false, res3 = false;
            int rowNum;
            rowNum = 2;
            res1 = wf_30414(workbook , SheetNo.tradeSum , rowNum , dtYm);
            rowNum = 2;
            res2 = wf_30415(workbook , SheetNo.dailyAvg , rowNum , dtYm);
            res3 = wf_30416(workbook , SheetNo.oint , rowNum , dtYm);

            //6. save 
            workbook.SaveDocument(excelDestinationPath);
            labMsg.Visible = false;

            if (FlagAdmin)
               System.Diagnostics.Process.Start(excelDestinationPath);

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         } finally {
            panFilter.Enabled = true;
            labMsg.Text = "";
            labMsg.Visible = false;
         }
         return ResultStatus.Fail;

      }

      /// <summary>
      /// wf_30414 (sheet1 data)
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="sheetNo">SheetNo.tradeSum</param>
      /// <param name="rowNum"> 2 </param>
      /// <returns></returns>
      protected bool wf_30414(Workbook workbook , SheetNo sheetNo , int rowNum , DataTable dtTmp) {

         string rptName = "股票期貨每月交易概況統計表"; //報表標題名稱
         labMsg.Text = _ProgramID + "－" + rptName + " 轉檔中...";

         try {
            //1. 切換Sheet
            Worksheet ws = workbook.Worksheets[(int)sheetNo];

            //2. 讀取資料
            DataTable dt = new D30414().ListData(StartMon , EndMon);
            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info(String.Format("{0}~{1},{2} - {3},無任何資料!" , StartMon , EndMon , _ProgramID , rptName));
               return false;
            } //if (dt.Rows.Count <= 0 )

            DataRow insertRow = dt.NewRow();
            insertRow["ai2_ymd"] = "999999";
            dt.Rows.Add(insertRow);

            int pos = 0;
            foreach (DataRow row in dt.Rows) {
               pos++;
               if (pos == dt.Rows.Count) continue; //執行到最後一列執行continue

               string ai2Ymd = row["ai2_ymd"].AsString();
               decimal ai2DayCount = row["ai2_day_count"].AsDecimal();
               decimal ai2MQnty = row["ai2_m_qnty"].AsDecimal();
               decimal ai2Oi = row["ai2_oi"].AsDecimal();
               decimal am10Cnt = row["am10_cnt"].AsDecimal();

               decimal am9AccCnt = row["am9_acc_cnt"].AsDecimal();

               dtTmp.PrimaryKey = new DataColumn[] { dtTmp.Columns["ai2_ymd"] };
               int found = dtTmp.Rows.IndexOf(dtTmp.Rows.Find(row["ai2_ymd"].AsString().Trim())).AsInt();

               if (found >= 0) {
                  rowNum = found + 2;
               }//if(found >= 0)

               ws.Cells[rowNum , 0].Value = ai2DayCount;
               ws.Cells[rowNum , 1].Value = string.Format("{0}年{1}月" , ai2Ymd.SubStr(0 , 4) , ai2Ymd.SubStr(4 , 2));
               ws.Cells[rowNum , 2].Value = ai2MQnty;
               ws.Cells[rowNum , 4].Value = ai2Oi;
               if (row["am10_cnt"] != DBNull.Value)
                  ws.Cells[rowNum , 6].Value = am10Cnt;
               ws.Cells[rowNum , 9].Value = am9AccCnt;

               //年度總計
               string year = ai2Ymd.SubStr(0 , 4);
               decimal sumDayCount = dt.Compute("Sum(ai2_day_count)" , "").AsDecimal(); //取得ai2_day_count欄位的總和
               decimal sumMQnty = dt.Compute("Sum(ai2_m_qnty)" , "").AsDecimal(); //取得ai2_m_qnty欄位的總和
               decimal sumOi = dt.Compute("Sum(ai2_oi)" , "").AsDecimal(); //取得ai2_m_qnty欄位的總和
               decimal sumCnt = dt.Compute("Sum(am10_cnt)" , "").AsDecimal(); //取得am10_cnt欄位的總和
               decimal sumAccCnt = dt.Compute("Sum(am9_acc_cnt)" , "").AsDecimal(); //取得am9_acc_cnt欄位的總和
               string nextYear = dt.Rows[pos]["ai2_ymd"].AsString().SubStr(0 , 4);

               if (year != nextYear) {
                  found = dtTmp.Rows.IndexOf(dtTmp.Rows.Find(row["ai2_ymd"])).AsInt(); //found因C#索引從0開始所以少1
                  if (found >= 0) {
                     int addRow = 12 - (row["ai2_ymd"].AsString().SubStr(4 , 2).AsInt()); //補滿12月的列數
                     rowNum = found + 3 + addRow;
                  }

                  ws.Cells[rowNum , 0].Value = sumDayCount;
                  ws.Cells[rowNum , 1].Value = string.Format("{0}年(至執行當月底)" , year);
                  ws.Cells[rowNum , 2].Value = sumMQnty;
                  ws.Cells[rowNum , 4].Value = sumOi;
                  ws.Cells[rowNum , 6].Value = sumCnt;
                  ws.Cells[rowNum , 9].Value = sumAccCnt;

               }//if (year != nextYear)

            }//foreach (DataRow row in dt.Rows)           

            ws.Range["A1"].Select();
            ws.ScrollToRow(0);

            return true;
         } catch (Exception ex) {
            return false;
            throw ex;
         }
      }

      /// <summary>
      /// wf_30415 (sheet2 data)
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="sheetNo">SheetNo.tradeSum</param>
      /// <param name="rowNum"> 2 </param>
      /// <returns></returns>
      protected bool wf_30415(Workbook workbook , SheetNo sheetNo , int rowNum , DataTable dtTmp) {

         string rptName = "股票期貨各標的每月日均量統計表"; //報表標題名稱
         labMsg.Text = "30415－" + rptName + " 轉檔中...";

         try {
            #region 30415
            //1. 表頭
            DataTable dtProd = new D30414().ListProdByMon(StartMon , EndMon);
            if (dtProd.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0}~{1},30415 - 商品檔,無任何資料!" , StartMon , EndMon));
               return false;
            }//if (dtProd.Rows.Count <= 0)

            //2. 切換Sheet
            Worksheet ws30415 = workbook.Worksheets[(int)sheetNo];

            //3. 讀取資料           
            for (int w = 0 ; w < dtProd.Rows.Count ; w++) {
               rowNum++;
               string apdkName = dtProd.Rows[w]["apdk_name"].AsString();
               string kindId = dtProd.Rows[w]["ai2_kind_id"].AsString();
               int rowTmp = rowNum - 1;

               ws30415.Cells[rowTmp , 0].Value = w + 1;
               ws30415.Cells[rowTmp , 1].Value = string.Format("{0}({1})" , apdkName , kindId);

            }//for (int w = 0 ; w < dtProd.Rows.Count ; w++)

            int colNum = 2;
            foreach (DataRow dr in dtTmp.Rows) {
               colNum++;
               int tmpCol = colNum - 1;
               string tmpYm = dr["ai2_ymd"].AsString();

               if (tmpYm.Length > 4) {
                  tmpYm = string.Format("{0}年{1}月" , tmpYm.SubStr(0 , 4) , tmpYm.SubStr(4 , 2));
               } else {
                  tmpYm = string.Format("{0}年全年" , tmpYm.SubStr(0 , 4));
               }
               ws30415.Cells[1 , tmpCol].Value = tmpYm;

            }//foreach (DataRow dr in dtTmp.Rows)

            DataTable dt30415 = new D30414().ListDataByMon(StartMon , EndMon);

            DataRow insertRow = dt30415.NewRow();
            insertRow["ai2_ymd"] = "999999";
            insertRow["ai2_kind_id"] = "end";
            dt30415.Rows.Add(insertRow);

            int con = 0;
            foreach (DataRow dr in dt30415.Rows) {
               con++;
               string kindId = dr["ai2_kind_id"].AsString();
               string ai2Ymd = dr["ai2_ymd"].AsString();
               decimal avgMQnty = dr["avg_m_qnty"].AsDecimal();

               if (con == dt30415.Rows.Count) continue;

               dtProd.PrimaryKey = new DataColumn[] { dtProd.Columns["ai2_kind_id"] };
               dtTmp.PrimaryKey = new DataColumn[] { dtTmp.Columns["ai2_ymd"] };
               int found = dtProd.Rows.IndexOf(dtProd.Rows.Find(dr["ai2_kind_id"])).AsInt();
               int foundCol = dtTmp.Rows.IndexOf(dtTmp.Rows.Find(dr["ai2_ymd"])).AsInt();

               if (found >= 0 && foundCol >= 0) {
                  ws30415.Cells[found + 2 , foundCol + 2].Value = avgMQnty;
               }//if (found >= 0 && foundCol >= 0)

               //年度
               string year = dr["ai2_ymd"].AsString().SubStr(0 , 4);
               string nextYear = dt30415.Rows[con]["ai2_ymd"].AsString().SubStr(0 , 4);
               string nextKindId = dt30415.Rows[con]["ai2_kind_id"].AsString();

               if (year != nextYear || kindId != nextKindId) {

                  foundCol = dtTmp.Rows.IndexOf(dtTmp.Rows.Find(dr["ai2_ymd"])).AsInt();
                  decimal sumDayCount = dt30415.Compute("Sum(ai2_day_count)" , "ai2_kind_id = '" + kindId + "'").AsDecimal(); //取得ai2_day_count欄位的總和
                  decimal sumMQnty = dt30415.Compute("Sum(ai2_m_qnty)" , "ai2_kind_id = '" + kindId + "'").AsDecimal(); //ai2_m_qnty
                  if (found >= 0 && foundCol >= 0) {
                     decimal yAvgMQnty;
                     if (sumDayCount == 0) {
                        yAvgMQnty = 0;
                     } else {
                        yAvgMQnty = Math.Round(sumMQnty / sumDayCount , 0 , MidpointRounding.AwayFromZero);
                     }
                     int tmp = 12 - ai2Ymd.SubStr(4 , 2).AsInt() + 1;
                     ws30415.Cells[found + 2 , foundCol + 2 + tmp].Value = yAvgMQnty;
                  }//if (found >= 0 && foundCol >= 0)
               }
            }//foreach (DataRow dr in dt30415.Rows)

            #endregion

            ws30415.Range["A1"].Select();
            ws30415.ScrollToRow(0);

            return true;
         } catch (Exception ex) {
            return false;
            throw ex;
         }
      }

      /// <summary>
      /// wf_30416 (sheet3 data)
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="sheetNo">SheetNo.tradeSum</param>
      /// <param name="rowNum"> 2 </param>
      /// <returns></returns>
      protected bool wf_30416(Workbook workbook , SheetNo sheetNo , int rowNum , DataTable dtTmp) {

         string rptName = "股票期貨各標的每月日均未平倉量統計表"; //報表標題名稱
         labMsg.Text = "30416－" + rptName + " 轉檔中...";

         try {

            #region 30416

            //1. 表頭
            DataTable dtProd = new D30414().ListProdByMon(StartMon , EndMon);
            if (dtProd.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0}~{1},30415 - 商品檔,無任何資料!" , StartMon , EndMon));
               return false;
            }//if (dtProd.Rows.Count <= 0)

            //切換Sheet
            Worksheet ws30416 = workbook.Worksheets[(int)sheetNo];

            for (int w = 0 ; w < dtProd.Rows.Count ; w++) {
               rowNum++;
               string apdkName = dtProd.Rows[w]["apdk_name"].AsString();
               string kindId = dtProd.Rows[w]["ai2_kind_id"].AsString();
               int rowTmp = rowNum - 1;

               ws30416.Cells[rowTmp , 0].Value = w + 1;
               ws30416.Cells[rowTmp , 1].Value = string.Format("{0}({1})" , apdkName , kindId);

            }//for (int w = 0 ; w < dtProd.Rows.Count ; w++)

            int colNum = 2;
            foreach (DataRow dr in dtTmp.Rows) {
               colNum++;
               int tmpCol = colNum - 1;
               string tmpYm = dr["ai2_ymd"].AsString();

               if (tmpYm.Length > 4) {
                  tmpYm = string.Format("{0}年{1}月" , tmpYm.SubStr(0 , 4) , tmpYm.SubStr(4 , 2));
               } else {
                  tmpYm = string.Format("{0}年全年" , tmpYm.SubStr(0 , 4));
               }
               ws30416.Cells[1 , tmpCol].Value = tmpYm;

            }//foreach (DataRow dr in dtTmp.Rows)

            DataTable dt30416 = new D30414().ListDataByMon(StartMon , EndMon);
            if (dt30416.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0}~{1},30416 - 股票期貨各標的每月日均未平倉量統計表,無任何資料!" , StartMon , EndMon));
               return false;
            }//if (dt30415.Rows.Count <= 0)

            DataRow insertRow2 = dt30416.NewRow();
            insertRow2["ai2_ymd"] = "999999";
            insertRow2["ai2_kind_id"] = "end";
            dt30416.Rows.Add(insertRow2);

            int pot = 0;
            foreach (DataRow dr in dt30416.Rows) {
               pot++;
               string kindId = dr["ai2_kind_id"].AsString();
               string ai2Ymd = dr["ai2_ymd"].AsString();
               decimal avgOi = dr["avg_oi"].AsDecimal();

               if (pot == dt30416.Rows.Count) continue;

               dtProd.PrimaryKey = new DataColumn[] { dtProd.Columns["ai2_kind_id"] };
               dtTmp.PrimaryKey = new DataColumn[] { dtTmp.Columns["ai2_ymd"] };
               int found = dtProd.Rows.IndexOf(dtProd.Rows.Find(dr["ai2_kind_id"])).AsInt();
               int foundCol = dtTmp.Rows.IndexOf(dtTmp.Rows.Find(dr["ai2_ymd"])).AsInt();

               if (found >= 0 && foundCol >= 0) {
                  ws30416.Cells[found + 2 , foundCol + 2].Value = avgOi;
               }//if (found >= 0 && foundCol >= 0)

               //年度
               string year = dr["ai2_ymd"].AsString().SubStr(0 , 4);
               string nextYear = dt30416.Rows[pot]["ai2_ymd"].AsString().SubStr(0 , 4);
               string nextKindId = dt30416.Rows[pot]["ai2_kind_id"].AsString();

               if (year != nextYear || kindId != nextKindId) {
                  foundCol = dtTmp.Rows.IndexOf(dtTmp.Rows.Find(dr["ai2_ymd"])).AsInt();
                  decimal sumDayCount = dt30416.Compute("Sum(ai2_day_count)" , "ai2_kind_id = '" + kindId + "'").AsDecimal(); //取得ai2_day_count欄位的總和
                  decimal sumOi = dt30416.Compute("Sum(ai2_oi)" , "ai2_kind_id = '" + kindId + "'").AsDecimal(); //ai2_oi
                  if (found >= 0 && foundCol >= 0) {
                     decimal yAvgOi;
                     if (sumDayCount == 0) {
                        yAvgOi = 0;
                     } else {
                        yAvgOi = Math.Round(sumOi / sumDayCount , 0 , MidpointRounding.AwayFromZero);
                     }
                     int tmp = 12 - ai2Ymd.SubStr(4 , 2).AsInt() + 1;
                     ws30416.Cells[found + 2 , foundCol + 2 + tmp].Value = yAvgOi;
                  }//if (found >= 0 && foundCol >= 0)
               }
            }//foreach (DataRow dr in dt30415.Rows)

            #endregion

            ws30416.Range["A1"].Select();
            ws30416.ScrollToRow(0);

            return true;
         } catch (Exception ex) {
            return false;
            throw ex;
         }
      }
   }
}