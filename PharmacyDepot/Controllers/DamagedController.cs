using models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PharmacyDepot.Controllers
{
    public class DamagedController : Controller
    {
        //
        // GET: /Damaged/Damaged
        BLL.DamagedDrugBLL damagedDrugBLL = new BLL.DamagedDrugBLL();
        BLL.DepotDrugBLL depotDrugBLL = new BLL.DepotDrugBLL();
        BLL.DrugBLL drugBLL = new BLL.DrugBLL();

        public ActionResult Index()
        {
            return View("~/Damaged");
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
            return View("Damaged", selectda(pageIndex));
        }
        public List<tb_DamagedDrug> selectda(int pageIndex)
        {
            int pageMax = Convert.ToInt32(Math.Ceiling(damagedDrugBLL.GetList(en => true).Count() / 20.0));
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
            var da = damagedDrugBLL.GetPageListDesc(pageIndex, 20, en => true, en => en.DamagedDate);
            return da.ToList();
        }
        public ActionResult Damaged()
        {
            ViewData["pageIndex"] = ViewData["pageIndex"] != null ? ViewData["pageIndex"] : 1;
            int pageIndex = (int)ViewData["pageIndex"];
            return View(selectda(pageIndex));
        }
        public bool judge(tb_DamagedDrug damagedDrug,int flag)
        {
            var Drug = drugBLL.GetList(en => en.Id == damagedDrug.DrugId).FirstOrDefault();
            if (Drug == null)
            {
                ModelState.AddModelError("DrugId", "药品不存在，请重新输入");
                return false;
            }
            else
            {
                var depotDrug = depotDrugBLL.GetList(en => en.Id == damagedDrug.DepotDrugId).FirstOrDefault();
                if (depotDrug == null)
                {
                    ModelState.AddModelError("DepotDrugId", "药品存放信息不存在，请重新输入");
                    return false;
                }
                else
                {
                    if (depotDrug.DrugId != damagedDrug.DrugId)
                    {
                        ModelState.AddModelError("DepotDrugId", "药品存放信息不存在，请重新输入");
                        return false;
                    }
                    else
                    {
                        int m = damagedDrug.Num;
                        int n = depotDrug.Num;
                        if (flag == 1)
                        {
                            depotDrug.Num = n - m;
                        }
                        else
                        {
                            BLL.DamagedDrugBLL DamagedDrugBLL = new BLL.DamagedDrugBLL();
                            var da = DamagedDrugBLL.GetList(en => en.Id == damagedDrug.Id).FirstOrDefault();
                            int j = da.Num;
                            depotDrug.Num = n + j - m;
                        }
                        if (depotDrug.Num<0)
                        {
                            ModelState.AddModelError("Num", "所报药品库存不足，请重新输入");
                            return false;

                        }
                        else
                        {
                            depotDrugBLL.Modify(depotDrug);
                            return true;
                        }
                    }
                }
            }

        }
        public ActionResult Create()
        {
            models.tb_DamagedDrug da = new models.tb_DamagedDrug();
            return View();
        }
        [HttpPost]
        public ActionResult Create(tb_DamagedDrug damagedDrug, FormCollection form)
        {
            
            if (ModelState.IsValid)
            {
                if (judge(damagedDrug,1) == false)
                {
                    return View("Create", damagedDrug);
                }
                else
                {
                    damagedDrugBLL.Add(damagedDrug);
                    return RedirectToAction("Damaged");
                }
            }
            else
            {
                return View("Create", damagedDrug);
            }
        }
        public ActionResult Edit(int id)
        {
            var da = damagedDrugBLL.GetList(en => en.Id == id).FirstOrDefault();
            return View(da);
        }
        [HttpPost]
        public ActionResult Edit(tb_DamagedDrug damagedDrug, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                if (judge(damagedDrug,0) == false)
                {
                    return View("Edit", damagedDrug);
                }
                else
                {
                    UpdateModel(damagedDrug, new[] { "DrugId", "DepotDrugId", "DamagedDate", "Num", "Reason" });
                    damagedDrugBLL.Modify(damagedDrug);
                    return RedirectToAction("Damaged");
                }
            }
            else
            {
                return View("Edit", damagedDrug);
            }
        }
        public ActionResult Delete(int id)
        {
            var da = damagedDrugBLL.GetList(en => en.Id == id).FirstOrDefault();
            return View(da);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection form)
        {
            var da = damagedDrugBLL.GetList(en => en.Id == id).FirstOrDefault();
            var depotDrug = depotDrugBLL.GetList(en => en.Id == da.DepotDrugId).FirstOrDefault();
            depotDrug.Num = da.Num + depotDrug.Num;
            depotDrugBLL.Modify(depotDrug);
            damagedDrugBLL.Delete(da);
            return RedirectToAction("Damaged");

        }

    }
}
