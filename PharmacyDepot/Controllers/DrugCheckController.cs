using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PharmacyDepot.Controllers
{
    public class DrugCheckController : ControllerBase
    {
        //
        // GET: /DrugCheck/
        BLL.DrugCheckBLL drugCheckBLL = new BLL.DrugCheckBLL();
        //public partial class drugCheckT
        //{
        //    public int depotId { get; set; }
        //    public int drugId { get; set; }
        //    public string drugName { get; set; }
        //    public string depotName { get; set; }
        //    public System.DateTime lateDate { get; set; }
        //    public System.DateTime earlyDate { get; set; }
        //    public int num { get; set; }
        //    public int batchs { get; set; }
        //}
        public ActionResult Index()
        {
           
            return View();
        }
        public ActionResult DownList()
        {
            var list = Session["DrugCheckList"] as List<models.drugCheck>;
            string[] columnName = { "仓库ID", "药品ID", "药品名称", "仓库名称", "最晚批次", "最早批次", "数量", "批次" };
            DownFile(list, "仓库药品表", columnName);
            return RedirectToAction("Index");
        }

    }
}
