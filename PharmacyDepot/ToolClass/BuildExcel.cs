using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PharmacyDepot.ToolClass
{
    public static class BuildExcel
    {

        public static string FillExcel<T>(string Path,string[] columnName, string name, List<T> list)
        {

            System.Data.DataTable dt = ListToDataTable.ListToTable<T>(list, columnName);
            Microsoft.Office.Interop.Excel.Application ex = new Microsoft.Office.Interop.Excel.Application();


            Workbooks wbk2 = ex.Workbooks;
            _Workbook _wbk2 = wbk2.Add(true);
            //复制完关闭excel模板

            Worksheet ws = _wbk2.Sheets[1];
            //整体写入方法
            object[,] objData = new object[dt.Rows.Count + 1, dt.Columns.Count];
            //首先将数据写入到一个二维数组中  
            for (int m = 0; m < dt.Columns.Count; m++)
            {
                //if (!dt.Columns[m].ColumnName.StartsWith("F"))
                    objData[0, m] = dt.Columns[m].ColumnName;
                //else
                //    objData[0, m] = null;
            }
            if (dt.Rows.Count > 0)
            {
                for (int m = 0; m < dt.Rows.Count; m++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        objData[m + 1, j] = dt.Rows[m][j];
                    }
                }
            }
            string startCol = "A";
            int iCnt = (dt.Columns.Count / 26);
            string endColSignal = (iCnt == 0 ? "" : ((char)('A' + (iCnt - 1))).ToString());
            string endCol = endColSignal + ((char)('A' + dt.Columns.Count - iCnt * 26 - 1)).ToString();

            Microsoft.Office.Interop.Excel.Range range = ws.get_Range(startCol + "1", endCol + (dt.Rows.Count - iCnt * 26 + 1).ToString());
            //Microsoft.Office.Interop.Excel.Range range = ws.get_Range(startCol + "1", endCol + dt.Rows.Count.ToString());

            range.Value = objData;

            //逐个单元格写入方法

            // for (int r = 0; r < dt.Rows.Count; r++)
            // {
            //    for (int l = 0; l < dt.Columns.Count; l++)
            //    {
            //       ws.Cells[r + 2, l + 1] = dt.Rows[r][l].ToString();
            //    }
            //   }
            //保存
            ex.DisplayAlerts = false;
            ex.AlertBeforeOverwriting = false;
            try
            {
                _wbk2.SaveAs(Path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }
            catch (Exception e)
            {

            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(ws);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(_wbk2);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(wbk2);

                ex.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(ex);


            }
            return Path;

        }
    }
}