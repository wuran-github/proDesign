using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace PharmacyDepot.Controllers
{
    public class ControllerBase : Controller
    {
        #region 私有字段
        BLL.DamagedDrugBLL damagedDrugBLL = new BLL.DamagedDrugBLL();
        BLL.DepotBLL depotBLL = new BLL.DepotBLL();
        BLL.DepotDrugBLL depotDrugBLL = new BLL.DepotDrugBLL();
        BLL.DepotInBLL depotInBLL = new BLL.DepotInBLL();
        BLL.DepotOutBLL depotOutBLL = new BLL.DepotOutBLL();
        BLL.DrugBLL drugBLL = new BLL.DrugBLL();
        BLL.SupplierBLL supplierBLL = new BLL.SupplierBLL();
        BLL.WarningBLL warningBLL = new BLL.WarningBLL();
        BLL.PurchaseDrugsBLL purchaseDrugsBLL = new BLL.PurchaseDrugsBLL();
        BLL.PurchaseBLL purchaseBLL = new BLL.PurchaseBLL();
        BLL.StockViewBLL stockViewBLL = new BLL.StockViewBLL();
        BLL.DrugCheckBLL drugCheckBLL = new BLL.DrugCheckBLL();
        #endregion
        public partial class drugCheckT
        {
            public int depotId { get; set; }
            public int drugId { get; set; }
            public string drugName { get; set; }
            public string depotName { get; set; }
            public System.DateTime lateDate { get; set; }
            public System.DateTime earlyDate { get; set; }
            public int num { get; set; }
            public int batchs { get; set; }
        }
        public void DownFile<T>(List<T> list, string name, string[] columnName)
        {
            string path = @"~/report/";
            string fullname =name+ DateTime.Now.ToString("yyyyMMddHHmmss")+".xlsx";
            string pathname=Server.MapPath(path+fullname);
            ToolClass.BuildExcel.FillExcel<T>(pathname, columnName, null, list);
            FileInfo fileInfo = new FileInfo(pathname);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fullname);
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            Response.AddHeader("Content-Transfer-Encoding", "binary");
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            Response.WriteFile(fileInfo.FullName);
            Response.Flush();
            //System.IO.File.Delete(path);

            Response.End();
        }
       
    }
}
