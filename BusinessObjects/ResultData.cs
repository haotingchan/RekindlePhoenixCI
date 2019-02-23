using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class ResultData
    {
        private object _ReturnObject;
        private DataTable _ReturnData;
        private DataTable _ChangedDataTable = new DataTable();
        private DataView _ChangedDataView = new DataView();
        private string _returnString;

        private DataView _ChangedDataViewForAdded    = new DataView();
        private DataView _ChangedDataViewForDeleted  = new DataView();
        private DataView _ChangedDataViewForModified = new DataView();

        public DbTransaction DbTransaction;

        public ResultStatus Status
        {
            get;set;
        }

        public string returnString {
            get { return _returnString; }
            set { _returnString = value; }
        }

        public object ReturnObject
        {
            get { return _ReturnObject; }
            set { _ReturnObject = value; }
        }

        public DataTable ReturnData
        {
            get { return _ReturnData; }
            set { _ReturnData = value; }
        }

        public DataTable ChangedDataTable
        {
            get { return _ChangedDataTable; }
            set { _ChangedDataTable = value; }
        }

        public DataView ChangedDataView
        {
            get { return _ChangedDataView; }
            set { _ChangedDataView = value; }
        }

        public DataView ChangedDataViewForAdded
        {
            get { return _ChangedDataViewForAdded; }
            set { _ChangedDataViewForAdded = value; }
        }

        public DataView ChangedDataViewForDeleted
        {
            get { return _ChangedDataViewForDeleted; }
            set { _ChangedDataViewForDeleted = value; }
        }

        public DataView ChangedDataViewForModified
        {
            get { return _ChangedDataViewForModified; }
            set { _ChangedDataViewForModified = value; }
        }
    }
}
