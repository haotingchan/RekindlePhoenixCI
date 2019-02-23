using OnePiece;
using System.Data;
using System.Data.Common;


namespace DataObjects.Dao.Together.SpecificDao
{
   public class D50034: DataGate
   {
      public D50034()
      {
      }

//      /// <summary>
//      /// 從16個從UI傳過來的參數+2個根據功能定義的參數,來動態設定where條件
//      /// </summary>
//      /// <param name="is_sbrkno">期貨商代號Start</param>
//      /// <param name="is_ebrkno">期貨商代號End</param>
//      /// <param name="is_prod_category">商品群組</param>
//      /// <param name="is_prod_kind_id_sto">2碼商品</param>
//      /// <param name="is_prod_kind_id">造市商品</param>
//      /// <param name="rb_month_checked">區間:月份</param>
//      /// <param name="rb_date_checked">區間:日期</param>
//      /// <param name="is_sdate">如果區間選擇月份,則傳入起始月份yyyyMM,如果為日期則傳入起始日期yyyyMMdd</param>
//      /// <param name="is_edate">如果區間選擇月份,則傳入結束月份yyyyMM,如果為日期則傳入結束日期yyyyMMdd</param>
//      /// <param name="rb_gall_checked">統計依照:全部</param>
//      /// <param name="rb_gparam_checked">統計依照:商品群組</param>
//      /// <param name="rb_s_checked">統計依照:股票類各群組</param>
//      /// <param name="rb_gkind2_checked">統計依照:商品(2碼)</param>
//      /// <param name="rb_gkind_checked">統計依照:商品(3碼)</param>
//      /// <param name="rb_gprod_checked">統計依照:序列</param>
//      /// <param name="rb_mmk_checked">列印順序:造市者(false=商品)</param>
//      /// <param name="tableName">tableName</param>
//      /// <param name="fieldPrefix">欄位名稱prefix,不一定跟tableName相同</param>
//      /// <returns>AMM0_YMD/AMM0_BRK_NO/AMM0_ACC_NO/BRK_ABBR_NAME/AMM0_PROD_TYPE/AMM0_PROD_ID/AMM0_O_SUBTRACT_QNTY/AMM0_Q_SUBTRACT_QNTY/AMM0_IQM_SUBTRACT_QNTY</returns>
//      protected DataTable ListReport(string is_sbrkno, string is_ebrkno,
//                                  string is_prod_category,
//                                  string is_prod_kind_id_sto,
//                                  string is_prod_kind_id,
//                                  bool rb_month_checked, bool rb_date_checked,
//                                  string is_sdate, string is_edate,
//                                  bool rb_gall_checked, bool rb_gparam_checked, bool rb_s_checked, bool rb_gkind2_checked, bool rb_gkind_checked, bool rb_gprod_checked,
//                                  bool rb_mmk_checked,
//                                  string tableName = "ci.AMM0",
//                                  string fieldPrefix = "AMM0")
//      {

//         //string fieldPrefix = "AMM0";//不是table name,而是欄位名稱prefix,不一定跟tableName相同
//         string is_where = "";//動態where條件,開頭為 and ...

//         /* 日期起迄 */
//         if (!string.IsNullOrEmpty(is_sdate))
//            is_where += System.Environment.NewLine + string.Format(" and {0}{1} >= '{2}'", fieldPrefix, "_YMD", is_sdate);
//         if (!string.IsNullOrEmpty(is_edate))
//            is_where += System.Environment.NewLine + string.Format(" and {0}{1} <= '{2}'", fieldPrefix, "_YMD", is_edate);
//         /* 期貨商代號起迄 */
//         if (!string.IsNullOrEmpty(is_sbrkno))
//            is_where += System.Environment.NewLine + string.Format(" and {0}{1} >= '{2}'", fieldPrefix, "_BRK_NO", is_sbrkno);
//         if (!string.IsNullOrEmpty(is_ebrkno))
//            is_where += System.Environment.NewLine + string.Format(" and {0}{1} <= '{2}'", fieldPrefix, "_BRK_NO", is_ebrkno);

//         //統計類別
//         string is_sum_type = (rb_month_checked ? "M" : "D");
//         //統計子類別
//         string is_sum_subtype = "1";
//         if (rb_gall_checked) {
//            is_sum_subtype = "1";
//         }
//         else if (rb_gparam_checked) {
//            is_sum_subtype = "3";
//         }
//         else if (rb_s_checked) {
//            is_sum_subtype = "S";
//         }
//         else if (rb_gkind2_checked) {
//            is_sum_subtype = "4";
//         }
//         else if (rb_gkind_checked) {
//            is_sum_subtype = "5";
//         }
//         else if (rb_gprod_checked) {
//            is_sum_subtype = "6";
//         }

//         //ken,Sort順序(這邊組SQL沒用到,但是有些dataWindow會用這個做分群,所以這個參數要放在前端CS判斷然後傳給報表UI)
//         string is_sort_type = (rb_mmk_checked ? "F" : "P");

//         //ken,特殊Where條件,這段原PB寫得很亂,不想理解直接照翻
//         if (!rb_gall_checked) {
//            //商品群組
//            if (!string.IsNullOrEmpty(is_prod_category)) {
//               is_where += System.Environment.NewLine + string.Format(" and {0}{1} = '{2}'", fieldPrefix, "_PARAM_KEY", is_prod_category);
//            }

//            //個股商品
//            if (!rb_gparam_checked) {
//               if (!string.IsNullOrEmpty(is_prod_kind_id_sto)) {
//                  is_where += System.Environment.NewLine + string.Format(" and {0}{1} = '{2}'", fieldPrefix, "_KIND_ID2", is_prod_kind_id_sto);
//               }

//               //商品
//               if (!rb_gkind2_checked && !string.IsNullOrEmpty(is_prod_kind_id)) {
//                  is_where += System.Environment.NewLine + string.Format(" and {0}{1} = '{2}'", fieldPrefix, "_KIND_ID", is_prod_kind_id);
//               }
//            }//if (!rb_gparam_checked) {
//         }//if (!rb_gall_checked) {

//         //又另外設定四個參數設定where條件,分兩段不知道在開心什麼
//         object[] parms = {
//                "@as_symd",is_sdate,
//                "@as_eymd",is_edate,
//                "@as_sum_type",is_sum_type,
//                "@as_sum_subtype",is_sum_subtype
//            };

//         string sql = string.Format(@"
//SELECT AMM0_YMD,   
//    AMM0_BRK_NO,
//    AMM0_ACC_NO,
//    (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK WHERE ABRK_NO = AMM0_BRK_NO ) as BRK_ABBR_NAME,  
//    AMM0_PROD_TYPE,
//    (case AMM0_SUM_SUBTYPE 
//        when '1' then AMM0_PROD_TYPE
//        when '2' then AMM0_PROD_SUBTYPE
//        when '3' then (case AMM0_PARAM_KEY when 'STF    ' then 'STF*   ' when 'STC    ' then 'STC*   ' else AMM0_PARAM_KEY end)
//        when 'S' then AMM0_PARAM_KEY
//        when '4' then (case when length(trim(AMM0_KIND_ID2)) = 2 then trim(AMM0_KIND_ID2)||AMM0_PROD_TYPE else trim(AMM0_KIND_ID2) end)
//        when '5' then AMM0_KIND_ID
//        else AMM0_PROD_ID end) as AMM0_PROD_ID,
//    AMM0_O_SUBTRACT_QNTY,
//    AMM0_Q_SUBTRACT_QNTY,
//    AMM0_IQM_SUBTRACT_QNTY
//FROM {0}
//WHERE AMM0_DATA_TYPE = 'Q'
//AND AMM0_YMD >= @as_symd
//AND AMM0_YMD <= @as_eymd
//AND AMM0_SUM_TYPE = @as_sum_type 
//AND AMM0_SUM_SUBTYPE = @as_sum_subtype  
//AND AMM0_O_SUBTRACT_QNTY + AMM0_Q_SUBTRACT_QNTY + AMM0_IQM_SUBTRACT_QNTY > 0
//{1}
//order by amm0_ymd , amm0_brk_no , amm0_acc_no , amm0_prod_type , amm0_prod_id"
// , tableName, is_where);


//         DataTable dtResult = db.GetDataTable(sql, parms);

//         return dtResult;
//      }//public ListReport


      ///// <summary>
      ///// 
      ///// </summary>
      ///// <param name="is_sbrkno">期貨商代號Start</param>
      ///// <param name="is_ebrkno">期貨商代號End</param>
      ///// <param name="is_prod_category">商品群組</param>
      ///// <param name="is_prod_kind_id_sto">2碼商品</param>
      ///// <param name="is_prod_kind_id">造市商品</param>
      ///// <param name="rb_month_checked">區間:月份</param>
      ///// <param name="rb_date_checked">區間:日期</param>
      ///// <param name="is_sdate">如果區間選擇月份,則傳入起始月份yyyyMM,如果為日期則傳入起始日期yyyyMMdd</param>
      ///// <param name="is_edate">如果區間選擇月份,則傳入結束月份yyyyMM,如果為日期則傳入結束日期yyyyMMdd</param>
      ///// <param name="rb_gall_checked">統計依照:全部</param>
      ///// <param name="rb_gparam_checked">統計依照:商品群組</param>
      ///// <param name="rb_s_checked">統計依照:股票類各群組</param>
      ///// <param name="rb_gkind2_checked">統計依照:商品(2碼)</param>
      ///// <param name="rb_gkind_checked">統計依照:商品(3碼)</param>
      ///// <param name="rb_gprod_checked">統計依照:序列</param>
      ///// <param name="rb_mmk_checked">列印順序:造市者(false=商品)</param>
      ///// <returns>AMM0_YMD/AMM0_BRK_NO/AMM0_ACC_NO/BRK_ABBR_NAME/AMM0_PROD_TYPE/AMM0_PROD_ID/AMM0_O_SUBTRACT_QNTY/AMM0_Q_SUBTRACT_QNTY/AMM0_IQM_SUBTRACT_QNTY</returns>
      //public DataTable ListAMMO(string is_sbrkno, string is_ebrkno,
      //                            string is_prod_category,
      //                            string is_prod_kind_id_sto,
      //                            string is_prod_kind_id,
      //                            bool rb_month_checked, bool rb_date_checked,
      //                            string is_sdate, string is_edate,
      //                            bool rb_gall_checked, bool rb_gparam_checked, bool rb_s_checked, bool rb_gkind2_checked, bool rb_gkind_checked, bool rb_gprod_checked,
      //                            bool rb_mmk_checked)
      //{
      //   return ListReport(is_sbrkno, is_ebrkno,
      //                       is_prod_category,
      //                       is_prod_kind_id_sto,
      //                       is_prod_kind_id,
      //                       rb_month_checked, rb_date_checked,
      //                       is_sdate, is_edate,
      //                       rb_gall_checked, rb_gparam_checked, rb_s_checked, rb_gkind2_checked, rb_gkind_checked, rb_gprod_checked,
      //                       rb_mmk_checked,
      //                       "ci.AMM0",
      //                       "AMM0");
      //}//public DataTable ListAMMO

      ///// <summary>
      ///// 
      ///// </summary>
      ///// <param name="is_sbrkno">期貨商代號Start</param>
      ///// <param name="is_ebrkno">期貨商代號End</param>
      ///// <param name="is_prod_category">商品群組</param>
      ///// <param name="is_prod_kind_id_sto">2碼商品</param>
      ///// <param name="is_prod_kind_id">造市商品</param>
      ///// <param name="rb_month_checked">區間:月份</param>
      ///// <param name="rb_date_checked">區間:日期</param>
      ///// <param name="is_sdate">如果區間選擇月份,則傳入起始月份yyyyMM,如果為日期則傳入起始日期yyyyMMdd</param>
      ///// <param name="is_edate">如果區間選擇月份,則傳入結束月份yyyyMM,如果為日期則傳入結束日期yyyyMMdd</param>
      ///// <param name="rb_gall_checked">統計依照:全部</param>
      ///// <param name="rb_gparam_checked">統計依照:商品群組</param>
      ///// <param name="rb_s_checked">統計依照:股票類各群組</param>
      ///// <param name="rb_gkind2_checked">統計依照:商品(2碼)</param>
      ///// <param name="rb_gkind_checked">統計依照:商品(3碼)</param>
      ///// <param name="rb_gprod_checked">統計依照:序列</param>
      ///// <param name="rb_mmk_checked">列印順序:造市者(false=商品)</param>
      ///// <returns>AMM0_YMD/AMM0_BRK_NO/AMM0_ACC_NO/BRK_ABBR_NAME/AMM0_PROD_TYPE/AMM0_PROD_ID/AMM0_O_SUBTRACT_QNTY/AMM0_Q_SUBTRACT_QNTY/AMM0_IQM_SUBTRACT_QNTY</returns>
      //public DataTable ListAMMOAH(string is_sbrkno, string is_ebrkno,
      //                            string is_prod_category,
      //                            string is_prod_kind_id_sto,
      //                            string is_prod_kind_id,
      //                            bool rb_month_checked, bool rb_date_checked,
      //                            string is_sdate, string is_edate,
      //                            bool rb_gall_checked, bool rb_gparam_checked, bool rb_s_checked, bool rb_gkind2_checked, bool rb_gkind_checked, bool rb_gprod_checked,
      //                            bool rb_mmk_checked)
      //{

      //   return ListReport(is_sbrkno, is_ebrkno,
      //                       is_prod_category,
      //                       is_prod_kind_id_sto,
      //                       is_prod_kind_id,
      //                       rb_month_checked, rb_date_checked,
      //                       is_sdate, is_edate,
      //                       rb_gall_checked, rb_gparam_checked, rb_s_checked, rb_gkind2_checked, rb_gkind_checked, rb_gprod_checked,
      //                       rb_mmk_checked,
      //                       "ci.AMM0AH",
      //                       "AMM0");
      //}//public DataTable ListAMMOAH
      public DbDataAdapter ListAMMOAH(string is_sdate, string is_edate, string is_sum_type, string is_sum_subtype)
      {
         object[] parms = {
                ":as_symd",is_sdate,
                ":as_eymd",is_edate,
                ":as_sum_type",is_sum_type,
                ":as_sum_subtype",is_sum_subtype
            };
         string sql = @"
SELECT AMM0_YMD,   
      AMM0_BRK_NO,AMM0_ACC_NO,
      (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK
         WHERE ABRK_NO = AMM0_BRK_NO ) as BRK_ABBR_NAME,  
      AMM0_PROD_TYPE,
      case AMM0_SUM_SUBTYPE 
            when '1' then AMM0_PROD_TYPE
            when '2' then AMM0_PROD_SUBTYPE
            when '3' then case AMM0_PARAM_KEY  when 'STF    ' then 'STF*   ' when 'STC    ' then 'STC*   ' else AMM0_PARAM_KEY end
            when 'S' then  AMM0_PARAM_KEY
            when '4' then case when length(trim(AMM0_KIND_ID2)) = 2 then trim(AMM0_KIND_ID2)||AMM0_PROD_TYPE else trim(AMM0_KIND_ID2) end
            when '5' then AMM0_KIND_ID
            else AMM0_PROD_ID end as AMM0_PROD_ID,
      AMM0_O_SUBTRACT_QNTY,
      AMM0_Q_SUBTRACT_QNTY,
      AMM0_IQM_SUBTRACT_QNTY
   FROM ci.AMM0AH
WHERE AMM0_DATA_TYPE = 'Q'
   AND AMM0_YMD >= :as_symd
   AND AMM0_YMD <= :as_eymd
   AND AMM0_SUM_TYPE = :as_sum_type 
   AND AMM0_SUM_SUBTYPE = :as_sum_subtype  
   AND AMM0_O_SUBTRACT_QNTY + AMM0_Q_SUBTRACT_QNTY + AMM0_IQM_SUBTRACT_QNTY > 0
ORDER BY amm0_ymd , amm0_brk_no , amm0_acc_no , amm0_prod_type , amm0_prod_id 
";
         DbDataAdapter adapter = db.GetDataAdapter(sql, parms);
         return adapter;
      }//public DbDataAdapter ListAMMOAH
      public DbDataAdapter ListAMMO(string is_sdate, string is_edate, string is_sum_type, string is_sum_subtype)
      {
         object[] parms = {
                ":as_symd",is_sdate,
                ":as_eymd",is_edate,
                ":as_sum_type",is_sum_type,
                ":as_sum_subtype",is_sum_subtype
            };
         string sql = @"
SELECT AMM0_YMD,   
         AMM0_BRK_NO,AMM0_ACC_NO,
        (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK
          WHERE ABRK_NO = AMM0_BRK_NO ) as BRK_ABBR_NAME,  
         AMM0_PROD_TYPE,
         case AMM0_SUM_SUBTYPE 
              when '1' then AMM0_PROD_TYPE
              when '2' then AMM0_PROD_SUBTYPE
              when '3' then case AMM0_PARAM_KEY  when 'STF    ' then 'STF*   ' when 'STC    ' then 'STC*   ' else AMM0_PARAM_KEY end
              when 'S' then  AMM0_PARAM_KEY
              when '4' then case when length(trim(AMM0_KIND_ID2)) = 2 then trim(AMM0_KIND_ID2)||AMM0_PROD_TYPE else trim(AMM0_KIND_ID2) end
              when '5' then AMM0_KIND_ID
              else AMM0_PROD_ID end as AMM0_PROD_ID,
         AMM0_O_SUBTRACT_QNTY,
         AMM0_Q_SUBTRACT_QNTY,
         AMM0_IQM_SUBTRACT_QNTY
    FROM ci.AMM0
   WHERE AMM0_DATA_TYPE = 'Q'
     AND AMM0_YMD >= :as_symd
     AND AMM0_YMD <= :as_eymd
     AND AMM0_SUM_TYPE = :as_sum_type 
     AND AMM0_SUM_SUBTYPE = :as_sum_subtype  
     AND AMM0_O_SUBTRACT_QNTY + AMM0_Q_SUBTRACT_QNTY + AMM0_IQM_SUBTRACT_QNTY > 0
   ORDER BY amm0_ymd , amm0_brk_no , amm0_acc_no , amm0_prod_type , amm0_prod_id 
";
         DbDataAdapter adapter = db.GetDataAdapter(sql, parms);
         return adapter;
      }//public DbDataAdapter ListAMMO
      public DataTable AMMOAH(string is_sdate, string is_edate, string is_sum_type, string is_sum_subtype)
      {
         object[] parms = {
                ":as_symd",is_sdate,
                ":as_eymd",is_edate,
                ":as_sum_type",is_sum_type,
                ":as_sum_subtype",is_sum_subtype
            };
         string sql = @"
SELECT AMM0_YMD,   
      AMM0_BRK_NO,AMM0_ACC_NO,
      (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK
         WHERE ABRK_NO = AMM0_BRK_NO ) as BRK_ABBR_NAME,  
      AMM0_PROD_TYPE,
      case AMM0_SUM_SUBTYPE 
            when '1' then AMM0_PROD_TYPE
            when '2' then AMM0_PROD_SUBTYPE
            when '3' then case AMM0_PARAM_KEY  when 'STF    ' then 'STF*   ' when 'STC    ' then 'STC*   ' else AMM0_PARAM_KEY end
            when 'S' then  AMM0_PARAM_KEY
            when '4' then case when length(trim(AMM0_KIND_ID2)) = 2 then trim(AMM0_KIND_ID2)||AMM0_PROD_TYPE else trim(AMM0_KIND_ID2) end
            when '5' then AMM0_KIND_ID
            else AMM0_PROD_ID end as AMM0_PROD_ID,
      AMM0_O_SUBTRACT_QNTY,
      AMM0_Q_SUBTRACT_QNTY,
      AMM0_IQM_SUBTRACT_QNTY
   FROM ci.AMM0AH
WHERE AMM0_DATA_TYPE = 'Q'
   AND AMM0_YMD >= :as_symd
   AND AMM0_YMD <= :as_eymd
   AND AMM0_SUM_TYPE = :as_sum_type 
   AND AMM0_SUM_SUBTYPE = :as_sum_subtype  
   AND AMM0_O_SUBTRACT_QNTY + AMM0_Q_SUBTRACT_QNTY + AMM0_IQM_SUBTRACT_QNTY > 0
ORDER BY amm0_ymd , amm0_brk_no , amm0_acc_no , amm0_prod_type , amm0_prod_id 
";
         DataTable dt = db.GetDataTable(sql, parms);
         return dt;
      }//public dt ListAMMOAH
      public DataTable AMMO(string is_sdate, string is_edate, string is_sum_type, string is_sum_subtype)
      {
         object[] parms = {
                ":as_symd",is_sdate,
                ":as_eymd",is_edate,
                ":as_sum_type",is_sum_type,
                ":as_sum_subtype",is_sum_subtype
            };
         string sql = @"
SELECT AMM0_YMD,   
         AMM0_BRK_NO,AMM0_ACC_NO,
        (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK
          WHERE ABRK_NO = AMM0_BRK_NO ) as BRK_ABBR_NAME,  
         AMM0_PROD_TYPE,
         case AMM0_SUM_SUBTYPE 
              when '1' then AMM0_PROD_TYPE
              when '2' then AMM0_PROD_SUBTYPE
              when '3' then case AMM0_PARAM_KEY  when 'STF    ' then 'STF*   ' when 'STC    ' then 'STC*   ' else AMM0_PARAM_KEY end
              when 'S' then  AMM0_PARAM_KEY
              when '4' then case when length(trim(AMM0_KIND_ID2)) = 2 then trim(AMM0_KIND_ID2)||AMM0_PROD_TYPE else trim(AMM0_KIND_ID2) end
              when '5' then AMM0_KIND_ID
              else AMM0_PROD_ID end as AMM0_PROD_ID,
         AMM0_O_SUBTRACT_QNTY,
         AMM0_Q_SUBTRACT_QNTY,
         AMM0_IQM_SUBTRACT_QNTY
    FROM ci.AMM0
   WHERE AMM0_DATA_TYPE = 'Q'
     AND AMM0_YMD >= :as_symd
     AND AMM0_YMD <= :as_eymd
     AND AMM0_SUM_TYPE = :as_sum_type 
     AND AMM0_SUM_SUBTYPE = :as_sum_subtype  
     AND AMM0_O_SUBTRACT_QNTY + AMM0_Q_SUBTRACT_QNTY + AMM0_IQM_SUBTRACT_QNTY > 0
   ORDER BY amm0_ymd , amm0_brk_no , amm0_acc_no , amm0_prod_type , amm0_prod_id 
";
         DataTable dt = db.GetDataTable(sql, parms);
         return dt;
      }//public dt ListAMMO

   }
}