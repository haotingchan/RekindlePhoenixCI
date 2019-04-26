using System;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DataObjects.Dao.Together.SpecificDao {
   public class D50010 : DataGate {
      /// <summary>
      /// 針對不同的grid data source,合併相同的輸入與輸出
      /// </summary>
      /// <param name="name"></param>
      /// <returns></returns>
      public IGridData50010 CreateGridData(Type type, string name) {

         string AssemblyName = type.Namespace.Split('.')[0];//最後compile出來的dll名稱
         string className = type.FullName + name;//完整的class路徑

         // 這裡就是Reflection，直接依照className實體化具體類別
         return (IGridData50010)Assembly.Load(AssemblyName).CreateInstance(className);
      }

   }

   public class Q50010 {
      public string ParamKey { get; set; }
      public string KindIdSt { get; set; }
      public string KindIdO { get; set; }
      public string ProdSort { get; set; }
      public string FcmRange { get; set; }
      public DateTime TxtDate { get; set; }

      public Q50010(DateTime txtDate, string paramKey, string kindIdSt, string kindIdO, string prodSort, string fcmSNo, string fcmENo) {

         if (CheckValue(paramKey)) {
            ParamKey = string.IsNullOrEmpty(paramKey) ? "" :
                  "and ammd_PARAM_KEY ='" + paramKey + "'";
         }

         if (CheckValue(kindIdSt)) {
            KindIdSt = string.IsNullOrEmpty(kindIdSt) ? "" :
                  "and ammd_kind_id2 ='" + kindIdSt + "'";
         }

         if (CheckValue(kindIdO)) {
            KindIdO = string.IsNullOrEmpty(kindIdO) ? "" :
                  "and ammd_KIND_ID = '" + kindIdO + "'";
         }

         if (CheckValue(prodSort)) {
            ProdSort = string.IsNullOrEmpty(prodSort) ? "" :
                  "and AMMD_PROD_ID like '" + prodSort + "%'";
         }

         if (CheckValue(fcmSNo)) {
            fcmSNo = string.IsNullOrEmpty(fcmSNo) ? "" :
               $"and AMMD_BRK_NO >= '{fcmSNo}'";
         }

         if (CheckValue(fcmENo)) {
            fcmENo = string.IsNullOrEmpty(fcmENo) ? "" :
               $"and AMMD_BRK_NO <= '{fcmENo}'";
         }

         FcmRange = fcmSNo + fcmENo;
         TxtDate = txtDate;
      }

      protected virtual bool CheckValue(string str) {

         RegexOptions opt = new RegexOptions();
         opt = RegexOptions.IgnorePatternWhitespace;
         Regex reg = new Regex("[A-Za-z0-9]+", opt);

         if (!string.IsNullOrEmpty(str)) {
            if (!reg.Match(str).Success) {
               throw new Exception("Value Error");
            }
         }

         return true;
      }

      /// <summary>
      /// convert all Properties to object[]
      /// </summary>
      /// <returns></returns>
      public object[] ToParam() {
         object[] aryParam = new object[GetType().GetProperties().Length * 2];
         int pos = 0;

         foreach (var prop in GetType().GetProperties()) {
            aryParam[pos++] = ":" + prop.Name;//":"其實可不用
            aryParam[pos++] = prop.GetValue(this);
         }

         return aryParam;
      }
   }

   public interface IGridData50010 {
      DataTable GetData(Q50010 queryArgs);
   }

   public class D50010O : DataGate, IGridData50010 {
      public DataTable GetData(Q50010 queryArgs) {

         object[] parms = {
                ":as_sort_type", "F",
                ":adt_date", queryArgs.TxtDate,
            };

         string sql = string.Format(@"  SELECT AMMD_BRK_NO, 
            BRK_ABBR_NAME,AMMD_ACC_NO, 
          AMMD_PROD_TYPE,
        TRIM(AMMD_PROD_ID) as AMMD_PROD_ID,   
         AMMD_W_TIME,   
         AMMD_R_TIME, 
         AMMD_M_TIME, 
         r_second,
         m_second, 
         cp_valid,
         AMMD_KEEP_TIME,
         AMMD_WEIGHT_P,
         AMMD_WEIGHT_Q,
         AMMD_WEIGHT_KIND,
         AMMD_RESULT,
         AMMD_OQ_CODE,
         AMMD_Q_NO from (
 SELECT  case when :as_sort_type ='F' then ammd_brk_no || ammd_acc_no else ammd_prod_type || ammd_prod_id end as  cp_group1,  
         case when :as_sort_type ='F' then  ammd_prod_type || ammd_prod_id else ammd_brk_no|| ammd_acc_no end as cp_group2 ,
         ammd_brk_no,
         AMMD_ACC_NO,
        (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK
          WHERE ABRK_NO = AMMD_BRK_NO ) as BRK_ABBR_NAME,  
         AMMD_PROD_ID,   
         AMMD_W_TIME,   
         AMMD_R_TIME, 
         AMMD_M_TIME, 
         AMMD_PROD_TYPE,
         case when AMMD_R_TIME >= AMMD_W_TIME then 0 else round(fut.TIME_DIFF(AMMD_R_TIME,AMMD_W_TIME),0) end   as r_second,
         round(fut.TIME_DIFF(AMMD_W_TIME,AMMD_M_TIME),0)   as m_second,
         case when AMMD_VALID_CNT > 0 then 'Y' else '' end as cp_valid,
         AMMD_KEEP_TIME,
         AMMD_WEIGHT_P,
         AMMD_WEIGHT_Q,
         AMMD_WEIGHT_KIND,
         AMMD_RESULT,
         AMMD_OQ_CODE,
         AMMD_Q_NO  
    FROM ci.AMMD  
   WHERE AMMD_DATE = :adt_date  AND  
         AMMD_DATA_TYPE  IN ('Q'  ,'q')
         {0}
         {1}
         {2}
         {3}
         {4}
         order by cp_group1 , cp_group2 , ammd_w_time)",
         queryArgs.ParamKey, queryArgs.KindIdSt, queryArgs.KindIdO, queryArgs.FcmRange, queryArgs.ProdSort);

         return db.GetDataTable(sql, parms);
      }
   }

   public class D50010AH : DataGate, IGridData50010 {
      public DataTable GetData(Q50010 queryArgs) {

         object[] parms = {
                ":as_sort_type",  "F",
                ":adt_date", queryArgs.TxtDate,
            };

         string sql = string.Format(@"  SELECT AMMD_BRK_NO, 
            BRK_ABBR_NAME, AMMD_ACC_NO,  
          AMMD_PROD_TYPE,
        TRIM(AMMD_PROD_ID) as AMMD_PROD_ID,   
         AMMD_W_TIME,   
         AMMD_R_TIME, 
         AMMD_M_TIME, 
         r_second,
         m_second, 
         cp_valid,
         AMMD_KEEP_TIME,
         AMMD_WEIGHT_P,
         AMMD_WEIGHT_Q,
         AMMD_WEIGHT_KIND,
         AMMD_RESULT,
         AMMD_OQ_CODE,
         AMMD_Q_NO from (
 SELECT case when :as_sort_type ='F' then ammd_brk_no || ammd_acc_no else ammd_prod_type || ammd_prod_id end as  cp_group1,  
         case when :as_sort_type ='F' then  ammd_prod_type || ammd_prod_id else ammd_brk_no|| ammd_acc_no end as cp_group2 ,
         ammd_brk_no,
         AMMD_ACC_NO,
        (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK
          WHERE ABRK_NO = AMMD_BRK_NO ) as BRK_ABBR_NAME,  
         AMMD_PROD_TYPE,
         AMMD_PROD_ID,   
         AMMD_W_TIME,   
         AMMD_R_TIME, 
         AMMD_M_TIME, 
         case when AMMD_R_TIME >= AMMD_W_TIME then 0 else round(fut.TIME_DIFF(AMMD_R_TIME,AMMD_W_TIME),0) end   as r_second,
         round(fut.TIME_DIFF(AMMD_W_TIME,AMMD_M_TIME),0)   as m_second,
         case when AMMD_VALID_CNT > 0 then 'Y' else '' end as cp_valid,
         AMMD_KEEP_TIME,
         AMMD_WEIGHT_P,
         AMMD_WEIGHT_Q,
         AMMD_WEIGHT_KIND,
         AMMD_RESULT,
         AMMD_OQ_CODE,
         AMMD_Q_NO  
    FROM ci.AMMDAH  
   WHERE AMMD_DATE = :adt_date  AND  
         AMMD_DATA_TYPE  IN ('Q'  ,'q')
         {0}
         {1}
         {2}
         {3}
         {4}
         order by cp_group1 , cp_group2 , ammd_w_time)",
         queryArgs.ParamKey, queryArgs.KindIdSt, queryArgs.KindIdO, queryArgs.FcmRange, queryArgs.ProdSort);

         return db.GetDataTable(sql, parms);
      }
   }
}
