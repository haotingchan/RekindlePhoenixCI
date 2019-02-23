using ActionService.Interfaces.Prefix5;
using ActionService.Interfaces.PrefixC;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using System.Data;

namespace ActionService.DbDirect.Prefix5 {

    public class Service50072 : IService50072 {

        private D50072 dao50072;

        public Service50072() {
            dao50072 = new D50072();
        }

        public DataTable ListData(string as_symd, string as_eymd) {

            return dao50072.ListData(as_symd, as_eymd);
        }

        public DataTable ListData_etf(string as_sym, string as_eym) {

            return dao50072.ListData_etf(as_sym, as_eym);
        }

        public DataTable ListData_mtx(string as_symd, string as_eymd) {

            return dao50072.ListData_mtx(as_symd, as_eymd);
        }

        public DataTable ListData_txf(string as_sym, string as_eym) {

            return dao50072.ListData_txf(as_sym, as_eym);
        }
    }
}
