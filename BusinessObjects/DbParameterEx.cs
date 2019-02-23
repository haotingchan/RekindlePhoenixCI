using BusinessObjects.Enums;
using System.Data;

namespace BusinessObjects
{
    public class DbParameterEx
    {
        public DbParameterEx()
        {
        }

        public DbParameterEx(string parameterName, object parameterValue)
        {
            Name = parameterName;
            Value = parameterValue;
        }

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private DbTypeEx _DbType = DbTypeEx.None;

        public DbTypeEx DbType
        {
            get { return _DbType; }
            set { _DbType = value; }
        }

        private ParameterDirection _Direction = ParameterDirection.Input;

        public ParameterDirection Direction
        {
            get { return _Direction; }
            set { _Direction = value; }
        }

        private object _Value;

        public object Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
    }
}