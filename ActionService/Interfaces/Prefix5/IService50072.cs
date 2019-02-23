using System;
using System.Data;

namespace ActionService.Interfaces.Prefix5 {

    public interface IService50072 : IService {

        DataTable ListData(string as_symd, string as_eymd);
        DataTable ListData_etf(string as_sym, string as_eym);
        DataTable ListData_mtx(string as_symd, string as_eymd);
        DataTable ListData_txf(string as_sym, string as_eym);

    }
}
