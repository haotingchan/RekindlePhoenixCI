using System;
using System.Data;
using System.Web;

namespace ActionService.Extensions
{
    public static class ExtensionService
    {
        public static Uri AddQuery(this Uri uri, string name, string value)
        {
            var httpValueCollection = HttpUtility.ParseQueryString(uri.Query);

            httpValueCollection.Remove(name);
            httpValueCollection.Add(name, value);

            var ub = new UriBuilder(uri);
            ub.Query = httpValueCollection.ToString();

            return ub.Uri;
        }

        public static DataTable Trim(this DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    if (col.DataType == typeof(String))
                    {
                        dr[col] = dr[col].ToString().Trim();
                    }
                }
            }

            dt.AcceptChanges();

            return dt;
        }
    }
}