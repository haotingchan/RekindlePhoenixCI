using System;
using System.Data;
using System.Data.Common;

/// <summary>
/// Winni, 2019/04/17
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   public class D20232 : DataGate {
      /// <summary>
      /// get ci.pls3 data retuen 7 field (d_20232)
      /// </summary>
      /// <param name="as_ym">yyyyMM</param>
      /// <param name="as_pid">1 or 2</param>
      /// <returns></returns>
      public DataTable GetDataList(string as_ym , string as_pid) {
         object[] parms = {
                ":as_ym", as_ym,
                ":as_pid", as_pid
            };

         string sql = @"
select 
    pls3_ym, 
    pls3_sid, 
    pls3_kind, 
    pls3_amt, 
    pls3_qnty,

    pls3_cnt, 
    pls3_pid
from ci.pls3
where pls3_ym = :as_ym
and pls3_pid = :as_pid
order by pls3_pid,pls3_sid
";
         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// 匯入資料前,先把pls3該年月刪除
      /// </summary>
      /// <param name="as_ym">yyyyMM</param>
      /// <param name="as_pls3_pid">1 or 2</param>
      /// <returns></returns>
      public int DeletePls3(string as_ym , string as_pls3_pid = "1") {
         object[] parms = {
                ":as_ym", as_ym,
                ":as_pls3_pid",as_pls3_pid
            };

         string sql = @"
delete ci.pls3
where pls3_ym = :as_ym
and pls3_pid = :as_pls3_pid
";

         int res = db.ExecuteSQL(sql , parms);

         return res;
      }


      /// <summary>
      /// ImportDataToPls3 , return success row count
      /// </summary>
      /// <param name="dtSource"></param>
      /// <param name="as_ym">yyyyMM</param>
      /// <param name="as_pls3_pid">1 or 2</param>
      /// <returns></returns>
      public int ImportDataToPls3(DataTable dtSource, string as_ym , string as_pls3_pid = "1") {


         DbConnection connection = db.CreateConnection();
         DbTransaction tran = connection.BeginTransaction();


         try {
            //2.create temp table in oracle
            string sql = @"
create global temporary table ci.temp_20232_2 
(
  pls3_ym    char(6 byte)                       not null,
  pls3_sid   char(6 byte)                       not null,
  pls3_kind  char(4 byte)                       not null,
  pls3_amt   number(14)                         not null,
  pls3_qnty  number(11)                         not null,
  pls3_cnt   number(8)                          not null,
  pls3_pid   char(1 byte)                       not null
) on commit delete rows
";

            DbCommand command = db.CreateCommand(sql , connection , CommandType.Text , null);
            command.Transaction = tran;
            command.ExecuteNonQuery();

            //3.foreach insert
            foreach (DataRow dr in dtSource.Rows) {
               object[] parms = {
                                 ":pls3_ym",dr[0],
                                 ":pls3_sid",dr[1],
                                 ":pls3_kind",dr[2],
                                 ":pls3_amt",dr[3],
                                 ":pls3_qnty",dr[4],
                                 ":pls3_cnt",dr[5],
                                 ":pls3_pid",dr[6],
                                };

               sql = @"
                        insert into ci.temp_20232_2 
                        select :pls3_ym,
                           :pls3_sid,
                           :pls3_kind,
                           :pls3_amt,
                           :pls3_qnty,
                           :pls3_cnt,
                           :pls3_pid
                        from dual";

               command = db.CreateCommand(sql , connection , CommandType.Text , parms);
               command.ExecuteNonQuery();
            }

            //4.delete pls3 yyyyMM data
            object[] deleteParam = {
                ":as_ym", as_ym,
                ":as_pls3_pid",as_pls3_pid
            };

            sql = @"
delete ci.pls3
where pls3_ym = :as_ym
and pls3_pid = :as_pls3_pid
";
            command = db.CreateCommand(sql , connection , CommandType.Text , deleteParam);
            command.ExecuteNonQuery();

            //4.temp table data insert to pls3
            sql = @"
insert into ci.pls3(pls3_ym , pls3_sid , pls3_kind , pls3_amt , pls3_qnty , pls3_cnt , pls3_pid)
select pls3_ym,
    pls3_sid,
    pls3_kind,
    sum(pls3_amt ) as pls3_amt,
    sum(pls3_qnty) as pls3_qnty,
    sum(pls3_cnt) as pls3_cnt,
    pls3_pid
from ci.temp_20232_2
group by pls3_ym, pls3_sid, pls3_kind, pls3_pid";

            command = db.CreateCommand(sql , connection , CommandType.Text , null);
            command.ExecuteNonQuery();

            //5.drop temp table
            sql = @"drop table ci.temp_20232_2";
            command = db.CreateCommand(sql , connection , CommandType.Text , null);
            command.ExecuteNonQuery();

            //6. select pls3 , get insert count
            object[] tempParam = { ":as_ym" , as_ym };
            sql = @"select count(0) from ci.pls3 where pls3_ym=:as_ym";
            command = db.CreateCommand(sql , connection , CommandType.Text , tempParam);
            int res = (int)command.ExecuteScalar();

            //7.commit
            tran.Commit();

            return res;
         } catch(Exception ex) {
            tran.Rollback();
            throw ex;
         } finally {
            connection.Close();
         }
      }


   }
}
