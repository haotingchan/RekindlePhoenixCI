using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Data.Common;

namespace Utils.Util
{
    public static class DataTableUtil
    {
        /// <summary>
        /// 將DataTable轉換成List
        /// </summary>
        /// <typeparam name="T">轉換成的Object</typeparam>
        /// <param name="table">datatable</param>
        /// <returns></returns>
        public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        public static T DataToObject<T>(this DbDataReader reader) where T : class, new()
        {
            try
            {

                T obj = new T();

                foreach (var prop in obj.GetType().GetProperties())
                {
                    try
                    {
                        PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                        propertyInfo.SetValue(obj, Convert.ChangeType(reader[prop.Name], propertyInfo.PropertyType), null);
                    }
                    catch
                    {
                        continue;
                    }
                }

                return obj;
            }
            catch
            {
                return null;
            }
        }
    }
}