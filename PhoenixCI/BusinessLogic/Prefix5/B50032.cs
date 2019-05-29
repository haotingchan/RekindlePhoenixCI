using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PhoenixCI.BusinessLogic.Prefix5
{
   public class B50032
   {
      /// <summary>
      /// 比對不符造市規定,產生一個檢視datatable
      /// </summary>
      /// <param name="_Data">要被過濾的資料</param>
      /// <param name="SleCMthTxt">連續月份</param>
      /// <returns></returns>
      public DataTable CompareData(DataTable _Data, string SleCMthTxt)
      {
         DataTable ids = _Data.Clone();
         string lsYMD = "", brkNo = "", prodID = "";
         int liCnt = 0;
         bool found = false;
         int liMth = SleCMthTxt.AsInt();
         _Data.Rows.Add(_Data.NewRow());
         int lastRowIndex = _Data.Rows.Count - 1;
         _Data.Rows[lastRowIndex]["AMM0_BRK_NO"] = "";
         _Data.Rows[lastRowIndex]["AMM0_PROD_ID"] = "";
         _Data.Rows[lastRowIndex]["CP_INVALID"] = 1;
         for (int k = 0; k < _Data.Rows.Count; k++) {
            DataRow dr = _Data.Rows[k];
            //成績有通過
            if (dr["CP_INVALID"].AsInt() == 0 && k != _Data.Rows.Count - 1)
               continue;
            //判斷是否同brk_no+prod_id+ym,已判斷跳過
            try {
               //found = ids.Select($@"AMM0_BRK_NO='{dr["AMM0_BRK_NO"]}' and AMM0_PROD_ID='{dr["AMM0_PROD_ID"]}' and AMM0_YMD='{dr["AMM0_YMD"]}'").Any();
               found = ids.AsEnumerable().Where(r => r.Field<object>("AMM0_BRK_NO").AsString() == dr["AMM0_BRK_NO"].AsString()
                                                                     && r.Field<object>("AMM0_PROD_ID").AsString() == dr["AMM0_PROD_ID"].AsString()
                                                                     && r.Field<object>("AMM0_YMD").AsString() == dr["AMM0_YMD"].AsString()).Any();
            }
            catch (Exception ex) {
#if DEBUG
               MessageDisplay.Error(ex.Source + ":" + k, "判斷是否同brk_no+prod_id+ym,已判斷跳過");
#endif
            }
            if (found)
               continue;

            //判斷是否同brk_no+prod_id+ym,不同acc_no是否有"成績有通過"
            try {
               //found = _Data.Select($@"AMM0_BRK_NO='{dr["AMM0_BRK_NO"]}' and AMM0_PROD_ID='{dr["AMM0_PROD_ID"]}' and AMM0_YMD='{dr["AMM0_YMD"]}' and AMM0_ACC_NO <>'{dr["AMM0_ACC_NO"]}' and CP_INVALID=0").Any();
               found = _Data.AsEnumerable().Where(r => r.Field<object>("AMM0_BRK_NO").AsString() == dr["AMM0_BRK_NO"].AsString()
                                                   && r.Field<object>("AMM0_PROD_ID").AsString() == dr["AMM0_PROD_ID"].AsString()
                                                   && r.Field<object>("AMM0_YMD").AsString() == dr["AMM0_YMD"].AsString()
                                                   && r.Field<object>("AMM0_ACC_NO").AsString() != dr["AMM0_ACC_NO"].AsString()
                                                   && r.Field<object>("CP_INVALID").AsInt() == 0).Any();
            }
            catch (Exception ex) {
#if DEBUG
               MessageDisplay.Error(ex.Source + ":" + k, "判斷不同acc_no是否有成績有通過");
#endif
            }
            if (found)
               continue;

            if (brkNo != dr["AMM0_BRK_NO"].ToString() || prodID != dr["AMM0_PROD_ID"].ToString()) {
               if (liCnt >= liMth) {
                  ids.Rows.Add(ids.NewRow());
                  int iDsLastRowIndex = ids.Rows.Count - 1;
                  ids.Rows[iDsLastRowIndex]["AMM0_BRK_NO"] = brkNo;
                  ids.Rows[iDsLastRowIndex]["AMM0_PROD_ID"] = prodID;
                  ids.Rows[iDsLastRowIndex]["AMM0_OM_QNTY"] = liCnt;
               }
               brkNo = dr["AMM0_BRK_NO"].ToString();
               prodID = dr["AMM0_PROD_ID"].ToString();
               lsYMD = dr["AMM0_YMD"].AsString();
               liCnt = 1;
            }//if (brkNo != dr["AMM0_BRK_NO"].AsString() || prodID != dr["AMM0_PROD_ID"].AsString())
            else {
               if (k == _Data.Rows.Count - 1)
                  continue;

               string lsDate = (dr["AMM0_YMD"].AsString() + "01").AsDateTime("yyyyMMdd").AddDays(-1).ToString("yyyyMM");
               //連續月份
               if (lsDate == lsYMD) {
                  liCnt = liCnt + 1;
               }
               lsYMD = dr["AMM0_YMD"].AsString();
            }
         }//for (int k = 0; k < _Data.Rows.Count; k++)
         _Data.Rows.RemoveAt(_Data.Rows.Count - 1);

         return ids;
      }

      /// <summary>
      /// 比對條件,刪除不符合的資料行
      /// </summary>
      /// <param name="FilterDt"></param>
      /// <param name="CompareDt"></param>
      /// <returns></returns>
      public DataTable FilterData(DataTable FilterDt, DataTable CompareDt)
      {
         bool found = false;
         DataTable dt = FilterDt;
         string brkNo2 = "", prodID2 = "";
         for (int k = 0; k < dt.Rows.Count; k++) {
            DataRow dr = dt.Rows[k];
            if (brkNo2 != dr["AMM0_BRK_NO"].AsString() || prodID2 != dr["AMM0_PROD_ID"].AsString()) {
               brkNo2 = dr["AMM0_BRK_NO"].AsString();
               prodID2 = dr["AMM0_PROD_ID"].AsString();
               //found = ids.Select($@"AMM0_BRK_NO='{dr["AMM0_BRK_NO"]}' and AMM0_PROD_ID='{dr["AMM0_PROD_ID"]}'").Any();
               found = CompareDt.AsEnumerable().Where(r => r.Field<object>("AMM0_BRK_NO").AsString() == brkNo2 && r.Field<object>("AMM0_PROD_ID").AsString() == prodID2).Any();
            }
            if (!found) {
               FilterDt.Rows.RemoveAt(k);
               k = k - 1;
            }
         }//for (int k = 0; k < dt.Rows.Count; k++)

         return FilterDt.AddSeriNumToDataTable();
      }

      /// <summary>
      /// 比對不符造市規定,產生一個檢視datatable(linq平行處理)
      /// </summary>
      /// <param name="_Data">要被過濾的資料</param>
      /// <param name="SleCMthTxt">連續月份</param>
      /// <returns></returns>
      public DataTable CompareDataByParallel(DataTable _Data, string SleCMthTxt)
      {
         int useCpuCount = Environment.ProcessorCount > 3 ? 2 : 1;//限制cpu使用數目
         DataTable ids = _Data.Clone();
         string lsYMD = "", brkNo = "", prodID = "";
         int liCnt = 0;
         bool found = false;
         int liMth = SleCMthTxt.AsInt();
         _Data.Rows.Add(_Data.NewRow());
         int lastRowIndex = _Data.Rows.Count - 1;
         _Data.Rows[lastRowIndex]["AMM0_BRK_NO"] = "";
         _Data.Rows[lastRowIndex]["AMM0_PROD_ID"] = "";
         _Data.Rows[lastRowIndex]["CP_INVALID"] = 1;

         for (int k = 0; k < _Data.Rows.Count; k++) {
            DataRow dr = _Data.Rows[k];
            //成績有通過
            if (dr["CP_INVALID"].AsInt() == 0 && k != _Data.Rows.Count - 1)
               continue;
            //判斷是否同brk_no+prod_id+ym,已判斷跳過
            try {
               //found = ids.Select($@"AMM0_BRK_NO='{dr["AMM0_BRK_NO"]}' and AMM0_PROD_ID='{dr["AMM0_PROD_ID"]}' and AMM0_YMD='{dr["AMM0_YMD"]}'").Any();
               found = ids.AsEnumerable().AsParallel().WithDegreeOfParallelism(useCpuCount).Where(r => r.Field<object>("AMM0_BRK_NO").AsString() == dr["AMM0_BRK_NO"].AsString()
                                                                     && r.Field<object>("AMM0_PROD_ID").AsString() == dr["AMM0_PROD_ID"].AsString()
                                                                     && r.Field<object>("AMM0_YMD").AsString() == dr["AMM0_YMD"].AsString()).Any();
            }
            catch (Exception ex) {
#if DEBUG
               MessageDisplay.Error($"第{k}筆資料:{ex.Message}", "判斷是否同brk_no+prod_id+ym,已判斷跳過");
#endif
            }
            


            if (found)
               continue;

            //判斷是否同brk_no+prod_id+ym,不同acc_no是否有"成績有通過"
            try {
               //found = _Data.Select($@"AMM0_BRK_NO='{dr["AMM0_BRK_NO"]}' and AMM0_PROD_ID='{dr["AMM0_PROD_ID"]}' and AMM0_YMD='{dr["AMM0_YMD"]}' and AMM0_ACC_NO <>'{dr["AMM0_ACC_NO"]}' and CP_INVALID=0").Any();
               found = _Data.AsEnumerable().AsParallel().WithDegreeOfParallelism(useCpuCount).Where(r => r.Field<object>("AMM0_BRK_NO").AsString() == dr["AMM0_BRK_NO"].AsString()
                                                   && r.Field<object>("AMM0_PROD_ID").AsString() == dr["AMM0_PROD_ID"].AsString()
                                                   && r.Field<object>("AMM0_YMD").AsString() == dr["AMM0_YMD"].AsString()
                                                   && r.Field<object>("AMM0_ACC_NO").AsString() != dr["AMM0_ACC_NO"].AsString()
                                                   && r.Field<object>("CP_INVALID").AsInt() == 0).Any();
            }
            catch (Exception ex) {
#if DEBUG
               MessageDisplay.Error($"第{k}筆資料:{ex.Message}", "判斷不同acc_no是否有成績有通過");
#endif
            }

            if (found)
               continue;

            if (brkNo != dr["AMM0_BRK_NO"].ToString() || prodID != dr["AMM0_PROD_ID"].ToString()) {
               if (liCnt >= liMth) {
                  ids.Rows.Add(ids.NewRow());
                  int iDsLastRowIndex = ids.Rows.Count - 1;
                  ids.Rows[iDsLastRowIndex]["AMM0_BRK_NO"] = brkNo;
                  ids.Rows[iDsLastRowIndex]["AMM0_PROD_ID"] = prodID;
                  ids.Rows[iDsLastRowIndex]["AMM0_OM_QNTY"] = liCnt;
               }
               brkNo = dr["AMM0_BRK_NO"].ToString();
               prodID = dr["AMM0_PROD_ID"].ToString();
               lsYMD = dr["AMM0_YMD"].AsString();
               liCnt = 1;
            }//if (brkNo != dr["AMM0_BRK_NO"].AsString() || prodID != dr["AMM0_PROD_ID"].AsString())
            else {
               if (k == _Data.Rows.Count - 1)
                  continue;

               string lsDate = (dr["AMM0_YMD"].AsString() + "01").AsDateTime("yyyyMMdd").AddDays(-1).ToString("yyyyMM");
               //連續月份
               if (lsDate == lsYMD) {
                  liCnt = liCnt + 1;
               }
               lsYMD = dr["AMM0_YMD"].AsString();
            }

            Thread.Sleep(0);
         }//for (int k = 0; k < _Data.Rows.Count; k++)
         _Data.Rows.RemoveAt(_Data.Rows.Count - 1);
         return ids;
      }

      /// <summary>
      /// 比對條件,刪除不符合的資料行(linq平行處理)
      /// </summary>
      /// <param name="FilterDt"></param>
      /// <param name="CompareDt"></param>
      /// <returns></returns>
      public DataTable FilterDataByParallel(DataTable FilterDt, DataTable CompareDt)
      {
         int useCpuCount = Environment.ProcessorCount > 3 ? 2 : 1;//限制cpu使用數目
         bool found = false;
         DataTable dt = FilterDt;
         string brkNo2 = "", prodID2 = "";
         for (int k = 0; k < dt.Rows.Count; k++) {
            DataRow dr = dt.Rows[k];
            if (brkNo2 != dr["AMM0_BRK_NO"].AsString() || prodID2 != dr["AMM0_PROD_ID"].AsString()) {
               brkNo2 = dr["AMM0_BRK_NO"].AsString();
               prodID2 = dr["AMM0_PROD_ID"].AsString();
               try {
                  //found = ids.Select($@"AMM0_BRK_NO='{dr["AMM0_BRK_NO"]}' and AMM0_PROD_ID='{dr["AMM0_PROD_ID"]}'").Any();
                  found = CompareDt.AsEnumerable().AsParallel().WithDegreeOfParallelism(useCpuCount).Where(r => r.Field<object>("AMM0_BRK_NO").AsString() == brkNo2 && r.Field<object>("AMM0_PROD_ID").AsString() == prodID2).Any();
               }
               catch (Exception ex) {
#if DEBUG
                  MessageDisplay.Error($"第{k}筆資料:{ex.Message}", "FilterDataByParallel");
#endif
               }
            }
            if (!found) {
               FilterDt.Rows.RemoveAt(k);
               k = k - 1;
            }
            Thread.Sleep(0);
         }//for (int k = 0; k < dt.Rows.Count; k++)

         return FilterDt.AddSeriNumToDataTable();
      }

   }
}
