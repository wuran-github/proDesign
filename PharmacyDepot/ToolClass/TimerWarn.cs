using models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PharmacyDepot.ToolClass
{
    public static class TimerWarn
    {
        public static int  warn()
        {
            BLL.DepotDrugBLL DepotDrugBLL = new BLL.DepotDrugBLL();
            BLL.WarningBLL warningBLL = new BLL.WarningBLL();
            int warnNum = 0;
            var wa = warningBLL.GetList(en => DateTime.Now>= en.WarningDays && en.tb_DepotDrug.Num>0,en=>en.tb_DepotDrug);
            warnNum = wa.Count;
            //var idlist = wa.Select(en => en.DepotDrugId);
            //var dep = DepotDrugBLL.GetList(en => !idlist.Contains(en.Id), en => en.tb_DrugInfo);
            //List<models.tb_Warning> warnList = new List<tb_Warning>();
            //foreach(var d in dep)
            //{
               
            //}
            //foreach (var w in wa)
            //{
            //    idlist.Add(w.DepotDrugId);
            //}
            //foreach (var d in dep)
            //{
            //    if (!idlist.Exists(x => x == d.Id))
            //    {
            //        int wd = Convert.ToInt32(Math.Ceiling(d.tb_DrugInfo.Shelf * 0.8));
            //        tb_Warning Warning = new tb_Warning();
            //        Warning.DepotDrugId = d.Id;
            //        DateTime ProductionDate = d.ProductionDate;
            //        Warning.WarningDays = ProductionDate.AddDays(wd);
            //        warningBLL.Add(Warning);
            //    }
            //}
            //string now = DateTime.Now.ToShortDateString();
            //wa = warningBLL.GetList(en => true);
            //foreach (var w in wa)
            //{
            //    if (w.WarningDays.ToShortDateString() == now)
            //    {
            //        warnNum = warnNum + 1;
            //    }
            //}
            return warnNum;
        }
        public static void expireCreate()
        {
            //BLL.DamagedDrugBLL damagedDrugBLL = new BLL.DamagedDrugBLL();
            //BLL.DepotDrugBLL depotDrugBLL = new BLL.DepotDrugBLL();
            //var dep = depotDrugBLL.GetList(en => true, en => en.tb_DrugInfo);
            //DateTime now = DateTime.Now;
            //foreach (var d in dep)
            //{
            //    if (d.Num != 0)
            //    {
            //        int expire = d.tb_DrugInfo.Shelf;
            //        DateTime expireDate = d.ProductionDate.AddDays(d.tb_DrugInfo.Shelf);
            //        if (expireDate <= now)
            //        {
            //            tb_DamagedDrug expireDamaged = new tb_DamagedDrug();
            //            expireDamaged.DepotDrugId = d.Id;
            //            expireDamaged.DrugId = d.DrugId;
            //            expireDamaged.DamagedDate = Convert.ToDateTime(expireDate);
            //            expireDamaged.Num = d.Num;
            //            expireDamaged.Reason = "药品过期";
            //            d.Num = 0;
            //            depotDrugBLL.Modify(d);
            //            damagedDrugBLL.Add(expireDamaged);
            //        }
            //    }
            //}
        }
    }
}