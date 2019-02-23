using DevExpress.XtraGrid;
using System;
using System.Collections.Generic;
using System.Data;

namespace BusinessObjects
{
    public class PokeBall
    {
        public List<DataTable> TrackedDataTables = new List<DataTable>();

        public string TXF_TID;

        public GridControl GridControlMain;

        public GridControl GridControlSecond;

        public DateTime OcfDate;
    }
}