using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PharmacyDepot.Controllers
{
    public class TransferController : Controller
    {
        //
        // GET: /Transfer/
        BLL.DepotInBLL depotInBLL = new BLL.DepotInBLL();
        List<models.tb_DepotIn> tb_depotins = new List<models.tb_DepotIn>();
        BLL.DepotDrugBLL depotDrugBLL = new BLL.DepotDrugBLL();
        List<models.tb_DepotDrug> tb_depotdrugs = new List<models.tb_DepotDrug>();
        BLL.DepotBLL depotBLL = new BLL.DepotBLL();
        List<models.tb_DepotInfo> tb_depotinfos = new List<models.tb_DepotInfo>();
        BLL.DrugBLL drugBLL = new BLL.DrugBLL();
        List<models.tb_DrugInfo> tb_druginfos = new List<models.tb_DrugInfo>();
        BLL.SupplierBLL supBLL = new BLL.SupplierBLL();
        List<models.tb_SupplierInfo> tb_suppliers = new List<models.tb_SupplierInfo>();
        public void GetOptItems()
        {
            //初始化select的option
            List<int> depotid = new List<int>();
            List<int> drugid = new List<int>();
            List<int> supid = new List<int>();
            tb_depotinfos = depotBLL.GetList(e => true);
            tb_druginfos = drugBLL.GetList(e => true);
            tb_suppliers = supBLL.GetList(e => true);
            for (int i = 0; i < tb_depotinfos.Count; i++)
            {
                depotid.Add(tb_depotinfos[i].Id);
            }
            for (int i = 0; i < tb_druginfos.Count; i++)
            {
                drugid.Add(tb_druginfos[i].Id);
            }
            for (int i = 0; i < tb_suppliers.Count; i++)
            {
                supid.Add(tb_suppliers[i].Id);
            }
            ViewBag.DepotId = depotid;
            ViewBag.DrugId = drugid;
            ViewBag.SupId = supid;
        }
        public ActionResult Transfer()
        {
            GetOptItems();
            return View();
        }
    }
}
