using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace PharmacyDepot.ToolClass
{
    public static class ListToDataTable
    {
        public static DataTable ListToTable<T>(List<T> list,string[] columnName, bool isStoreDB = true)
        {
            Type tp = typeof(T);
            PropertyInfo[] proInfos = tp.GetProperties();
            DataTable dt = new DataTable();
            int i = 0;
            foreach (var item in proInfos)
            {
                dt.Columns.Add(columnName[i], typeof(object)); //添加列明及对应类型  
                i++;
            }
            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();
                i = 0;
                foreach (var proInfo in proInfos)
                {
                    object obj = proInfo.GetValue(item);
                    if (obj == null)
                    {
                        continue;
                    }
                    //if (obj != null)  
                    // {  
                    if (isStoreDB && proInfo.PropertyType == typeof(DateTime) && Convert.ToDateTime(obj) < Convert.ToDateTime("1753-01-01"))
                    {
                        continue;
                    }
                    // dr[proInfo.Name] = proInfo.GetValue(item);  
                    dr[columnName[i]] = obj;
                    // }  
                    i++;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }  
    }
}