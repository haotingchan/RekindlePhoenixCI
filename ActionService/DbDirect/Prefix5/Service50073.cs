using ActionService.Interfaces.Prefix5;
using DataObjects.Dao.Together.TableDao;
using DataObjects.Dao.Together.TableDao.REWARD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionService.DbDirect.Prefix5 {
    public class Service50073 : IService50073 {

        private RWD_REF_OMNI daoRWD_REF_OMNI;
        private COD daoCOD;

        public Service50073(){
            daoRWD_REF_OMNI = new RWD_REF_OMNI();
            daoCOD = new COD();
        }

        public DataTable GetData() {
            return daoRWD_REF_OMNI.GetData();
        }

        public DataTable dddw_ommi_act_id() {
            return daoCOD.dddw_ommi_act_id();
        }
    }
}
