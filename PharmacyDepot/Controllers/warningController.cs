using models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace PharmacyDepot.Controllers
{
    public class warningController : Controller
    {
        //
        // GET: /warning/

        BLL.DepotDrugBLL DepotDrugBLL = new BLL.DepotDrugBLL();
        BLL.DrugBLL DrugBLL = new BLL.DrugBLL();
        BLL.WarningBLL warningBLL = new BLL.WarningBLL();



        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Warning()
        {
            ViewData["pageIndex"] = ViewData["pageIndex"] != null ? ViewData["pageIndex"] : 1;
            int pageIndex = (int)ViewData["pageIndex"];
            return View(selectwa(pageIndex));
        }
        public ActionResult page(int pageIndex, string str)
        {
            if (str == "privous")
            {
                pageIndex = pageIndex - 1;
            }
            if (str == "next")
            {
                pageIndex = pageIndex + 1;
            }
            ViewData["pageIndex"] = pageIndex;
            return View("Warning", selectwa(pageIndex));
        }
        public List<tb_Warning> selectwa(int pageIndex)
        {
            int pageMax = Convert.ToInt32(Math.Ceiling(warningBLL.GetList(en => true).Count() / 20.0));
            ViewData["count"] = pageMax;
            ViewData["privousEnabled"] = "";
            ViewData["nextEnabled"] = "";
            if (pageIndex < 2)
            {
                ViewData["privousEnabled"] = "disabled";
            }
            if (pageIndex > pageMax - 1)
            {
                ViewData["nextEnabled"] = "disabled";
            }
            pageIndex = pageIndex - 1;
            DateTime now = DateTime.Now;
            var wa = warningBLL.GetPageList(pageIndex, 20, en => en.WarningDays>=now, en => en.WarningDays);
            return wa;
        }
        
        

    }
}
