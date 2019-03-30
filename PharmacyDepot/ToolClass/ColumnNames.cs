using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PharmacyDepot.ToolClass
{
    public static class ColumnNames
    {
        public static string[] stock = { "现存数量", "仓库名称", "药品名称", "入库数量", "出库数量", "仓库ID", "药品ID" };
        public static string[] stockout = { "仓库名称", "药品名称","出库量",  "出库年月" };
    }
}