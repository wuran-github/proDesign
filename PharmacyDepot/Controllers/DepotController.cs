using models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PharmacyDepot.Controllers
{
    public class DepotController : Controller
    {
        //
        // GET: /Depot/
        BLL.DepotBLL depotBLL = new BLL.DepotBLL();
        public ActionResult Index()
        {
            return View("/~Depot");
        }
        public ActionResult Depot() 
        {
            var da = depotBLL.GetList(en => true);
            return View(da);
        }


        public ActionResult Create()//接收信息
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(tb_DepotInfo depotinfo, FormCollection form)//传递信息
        {
            depotBLL.Add(depotinfo);
            return RedirectToAction("Depot");
                
        }
        public ActionResult Edit(int id)//接收信息
        {
            var da = depotBLL.GetList(en => en.Id == id).FirstOrDefault();
            return View(da);
        }
        [HttpPost]
        public ActionResult Edit(tb_DepotInfo depotinfo, FormCollection form)//传递信息
        {
            if (ModelState.IsValid)
            {

                UpdateModel(depotinfo, new[] { "Name", "Type", "Address", "Area" });
                depotBLL.Modify(depotinfo);
                    return RedirectToAction("Depot");
                
            }
            else
            {
                return View("Edit", depotinfo);
            }
        }
        public ActionResult Delete(int id)//接收信息
        {
            var da = depotBLL.GetList(en => en.Id == id).FirstOrDefault();
            return View(da);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection form)//传递信息
        {
            var da = depotBLL.GetList(en => en.Id == id).FirstOrDefault();


            depotBLL.Delete(da);
            return RedirectToAction("Depot");

        }

    }
}
