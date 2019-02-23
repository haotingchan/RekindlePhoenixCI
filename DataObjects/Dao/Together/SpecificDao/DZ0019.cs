using OnePiece;
using System;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao
{
    public class DZ0019
    {
        private Db db;

        public DZ0019()
        {
            db = GlobalDaoSetting.DB;
        }

        public DataTable ListUTPByDept(string UPF_DPT_ID)
        {
            object[] parms = {
                "@UPF_DPT_ID",UPF_DPT_ID
            };

            string sql =
                @"
                          SELECT CI.UPF.UPF_DPT_ID AS UPF_DPT_ID,   
                                 CI.DPT.DPT_NAME AS DPT_NAME,   
                                 CI.UPF.UPF_USER_ID AS UPF_USER_ID,   
                                 CI.UPF.UPF_USER_NAME AS UPF_USER_NAME,   
                                 CI.UTP.UTP_TXN_ID AS UTP_TXN_ID,   
                                 CI.TXN.TXN_NAME AS TXN_NAME 
                            FROM CI.UPF,   
                                 CI.UTP,   
                                 CI.TXN,   
                                 CI.DPT  
                           WHERE ( CI.UPF.UPF_USER_ID = CI.UTP.UTP_USER_ID ) AND  
                                 ( CI.UTP.UTP_TXN_ID = CI.TXN.TXN_ID ) AND  
                                 ( CI.UPF.UPF_DPT_ID = CI.DPT.DPT_ID ) AND  
                                 ( ( UPF_DPT_ID = @UPF_DPT_ID ) )   
                            ORDER BY UPF_DPT_ID,UPF_USER_ID,UTP_TXN_ID
                    ";
            DataTable dtResult = db.GetDataTable(sql,parms);

            return dtResult;
        }

    }
}