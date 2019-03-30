using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PharmacyDepot.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            BLL.DepotBLL depotBLL = new BLL.DepotBLL();
            BLL.DepotDrugBLL ddb = new BLL.DepotDrugBLL();
            return View();
            
        }

    }
}
