using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PharmacyDepot.Controllers
{
    public class stockout
    {
        public string depotName { get; set; }
        public string drugName { get; set; }
        public int outNum { get; set; }
        public string date { get; set; }
    }
    public class StockController : ControllerBase
    {
        //
        // GET: /Stock/
        BLL.DepotDrugBLL depotDrugBLL = new BLL.DepotDrugBLL();
        BLL.DepotBLL depotBLL = new BLL.DepotBLL();
        public ActionResult StockIndex()
        {
            
           
            return View();
        }
        public ActionResult StockOutIndex()
        {
            return View();
        }
        public ActionResult BulidPurchasePlan()
        {
            return View();
        }
        public ActionResult PurchasePlan()
        {
            return View();
        }
        public ActionResult PurchaseDrug()
        {
            return View();
        }
        public ActionResult DownStockList()
        {
            var list = Session["stockList"] as List<models.StockView>;
            DownFile(list, "盘存统计表", PharmacyDepot.ToolClass.ColumnNames.stock);
            return RedirectToAction("StockIndex");
        }
        public ActionResult DownStockOutList()
        {
            var list = Session["stockOutList"] as List<stockout>;
            DownFile(list, "出库量表", PharmacyDepot.ToolClass.ColumnNames.stockout);
            return RedirectToAction("StockOutIndex");
        }
        public ActionResult TestPost()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TestPost(List<models.tb_DepotDrug> depots)
        {
            
            return View();
        }
    
    }
}
