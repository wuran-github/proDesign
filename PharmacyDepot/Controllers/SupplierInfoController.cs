using models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PharmacyDepot.Controllers
{
   
      
    public class SupplierInfoController : Controller
    {
        //
        // GET: /Depot/
        BLL.SupplierBLL supplierBLL = new BLL.SupplierBLL();
        public ActionResult Index()
        {
            return View("/~SuoolierInfo");
        }
        public ActionResult SupplierInfo() 
        {
            var da = supplierBLL.GetList(en => true);
            return View(da);
        }
        public ActionResult details(int id)
        {
            var da = supplierBLL.GetList(en => en.Id == id).FirstOrDefault();
            return View(da);
        }
       
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(tb_SupplierInfo supplierinfo, FormCollection form)
        {
            supplierBLL.Add(supplierinfo);
            return RedirectToAction("SupplierInfo");
                
        }
        public ActionResult Edit(int id)
        {
            var da = supplierBLL.GetList(en => en.Id == id).FirstOrDefault();
            return View(da);
        }
        [HttpPost]
        public ActionResult Edit(tb_SupplierInfo supplierinfo, FormCollection form)
        {
            if (ModelState.IsValid)
            {

                UpdateModel(supplierinfo, new[] { "Name", "LegalPersons", "Address", "ContactName", "ContactPhone", "CompanyPhone", "Email", "OrganizationCode", "PostalCode", "Fax" });
                supplierBLL.Modify(supplierinfo);
                    return RedirectToAction("SupplierInfo");
                
            }
            else
            {
                return View("Edit", supplierinfo);
            }
        }
        public ActionResult Delete(int id)
        {
            var da = supplierBLL.GetList(en => en.Id == id).FirstOrDefault();
            return View(da);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection form)
        {
            var da = supplierBLL.GetList(en => en.Id == id).FirstOrDefault();


            supplierBLL.Delete(da);
            return RedirectToAction("SupplierInfo");

        }

    }
}
