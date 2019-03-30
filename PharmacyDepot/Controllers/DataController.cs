using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PharmacyDepot.ToolClass;
using System.Web;

namespace PharmacyDepot.Controllers
{
    public class SearchModel
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public int drugId { get; set; }
        public int depotId { get; set; }
    }
    public class IdsModel
    {
        public int depotId { get; set; }
        public int drugId { get; set; }
    }
    /// <summary>
    /// 扩展类，比较两个Id是否相等
    /// </summary>
    public static class ExtList
    {
        public static bool Contains(this List<dynamic> Ids, int depotId, int drugId)
        {
            return Ids.Count(en => en.depotId == depotId && en.drugId == drugId) > 0;
        }
        public static bool Contains(this List<dynamic> names, string depotName, string drugName)
        {
            return names.Count(en => en.depotName == depotName && en.drugName == drugName) > 0;
        }
    }
    public class DataController : ApiController
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
        int[] months = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        #endregion
        #region 盘存统计
        [HttpPost]
        public dynamic PostSearchStock(dynamic data)
        {
            int page = 0;
            int pageSize = 20;
            int depotId = 1;
            int drugId = 1;
            Expression<Func<models.StockView, bool>> where = en => true;
            try
            {
                if (data != null)
                {
                    page = data.page;
                    pageSize = data.pageSize;
                    if (data.depotId != null)
                    {
                        depotId = data.depotId;
                        where = where.And(en => en.depotId == depotId);
                    }
                    if (data.drugId != null)
                    {
                        drugId = data.drugId;
                        where = where.And(en => en.drugId == drugId);
                    }
                }
                var t = stockViewBLL.GetList(where);
                //var l = depotDrugBLL.GetList(where, en => en.tb_DepotOut, en => en.tb_DepotIn, en => en.tb_DepotInfo, tb => tb.tb_DrugInfo);
                //var t = from s in l
                //        group s by new { s.tb_DrugInfo, s.tb_DepotInfo } into g
                //        orderby g.Key.tb_DepotInfo.Name
                //        select new
                //        {

                //            depotName = g.Key.tb_DepotInfo.Name,
                //            drugName = g.Key.tb_DrugInfo.Name,
                //            outNum = g.Sum(en => en.tb_DepotOut.Sum(x => x.Num)),
                //            inNum = g.Sum(en => en.tb_DepotIn.Num),
                //            Num = g.Sum(en => en.Num)

                //        };
                var result = t.Skip(page * pageSize).Take(pageSize);
                int count = t.Count();
                return new { dataItem = result, count = count };
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        [HttpPost]
        public dynamic DownSearchStock(dynamic data)
        {
            int depotId = 1;
            int drugId = 1;
            Expression<Func<models.StockView, bool>> where = en => true;
            try
            {
                if (data != null)
                {
                   
                    if (data.depotId != null)
                    {
                        depotId = data.depotId;
                        where = where.And(en => en.depotId == depotId);
                    }
                    if (data.drugId != null)
                    {
                        drugId = data.drugId;
                        where = where.And(en => en.drugId == drugId);
                    }
                }
                var t = stockViewBLL.GetList(where);
                HttpContext.Current.Session["stockList"] = t;
                return true;
             
            }
            catch (Exception e)
            {
                return false;
            }
        }
        [HttpGet]
        public dynamic GetDepotOutYears()
        {
            var list = depotOutBLL.GetList(en => true);
            var years = list.Select(en => en.OutDate.Year).Distinct();
            return new { dataItem = years };
        }
        /// <summary>
        /// 查询出库
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public dynamic PostStockOut(dynamic data)
        {
            int page = 0;
            int pageSize = 20;
            int depotId = 1;
            int drugId = 1;
            Expression<Func<models.tb_DepotOut, bool>> where = en => true;
            if (data != null)
            {
                page = data.page;
                pageSize = data.pageSize;
                if (((int?)data.depotId) != null)
                {
                    depotId = data.depotId;
                    where = where.And(en => en.DepotId == depotId);
                 }
                if (((int?)data.drugId) != null)
                {
                    drugId = data.drugId;
                    where = where.And(en => en.DrugId == drugId);
                }
                if (((int?)data.year) != null)
                {
                    //DateTime 
                    //where=where.And(en=>en.OutDate>)
                }
            }
            var l = depotOutBLL.GetList(where, en => en.tb_DepotInfo, tb => tb.tb_DrugInfo);
            var t = from s in l
                    group s by new { s.tb_DrugInfo, s.tb_DepotInfo, date = DateToYearMonth(s.OutDate) } into g
                    orderby g.Key.tb_DepotInfo.Name
                    select new
                    {
                        depotName = g.Key.tb_DepotInfo.Name,
                        drugName = g.Key.tb_DrugInfo.Name,
                        outNum = g.Sum(en => en.Num),
                        date = g.Key.date


                    };
            var result = t.Skip(page * pageSize).Take(pageSize);
            int count = t.Count();
            return new { dataItem = result, count = count };

        }
        /// <summary>
        /// 下载出库
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public dynamic DownStockOut(dynamic data)
        {
            int depotId = 1;
            int drugId = 1;
            try
            {
                Expression<Func<models.tb_DepotOut, bool>> where = en => true;
                if (data != null)
                {
                    if (((int?)data.depotId) != null)
                    {
                        depotId = data.depotId;
                        where = where.And(en => en.DepotId == depotId);
                    }
                    if (((int?)data.drugId) != null)
                    {
                        drugId = data.drugId;
                        where = where.And(en => en.DrugId == drugId);
                    }
                    if (((int?)data.year) != null)
                    {
                        //DateTime 
                        //where=where.And(en=>en.OutDate>)
                    }
                }
                var l = depotOutBLL.GetList(where, en => en.tb_DepotInfo, tb => tb.tb_DrugInfo);
                var t = from s in l
                        group s by new { s.tb_DrugInfo, s.tb_DepotInfo, date = DateToYearMonth(s.OutDate) } into g
                        orderby g.Key.tb_DepotInfo.Name
                        select new
                        {
                            depotName = g.Key.tb_DepotInfo.Name,
                            drugName = g.Key.tb_DrugInfo.Name,
                            outNum = g.Sum(en => en.Num),
                            date = g.Key.date


                        };
                List<stockout> list = new List<stockout>();
                foreach(var x in t)
                {
                    stockout so = new stockout()
                    {
                        depotName = x.depotName,
                        drugName = x.drugName,
                        outNum=x.outNum,
                        date=x.date
                    };
                    list.Add(so);
                }
                HttpContext.Current.Session["stockOutList"] = list;
                return true;
            }
            catch (Exception e)
            {

                return false;
            }
         

        }
        #endregion
        #region 采购计划
        /// <summary>
        /// 获取需要采购的列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public dynamic GetNotEnoughStock(dynamic data)
        {
            int page = 0;
            int pageSize = 20;
            int depotId = 1;
            int drugId = 1;
            int num = 500;
            Expression<Func<models.tb_DepotDrug, bool>> where = en => true;
            if (data != null)
            {
                page = data.page;
                pageSize = data.pageSize;
                if (data.depotId != null)
                {
                    depotId = data.depotId;
                    where = where.And(en => en.DepotId == depotId);
                }
                if (data.drugId != null)
                {
                    drugId = data.drugId;
                    where = where.And(en => en.DrugId == drugId);
                }
                if (data.num != null)
                {
                    num = data.num;
                }
            }
            var purchaseList = purchaseBLL.GetList(en => en.Finish == false);
            var PIds = purchaseList.Select(en => en.Id);
            var purchaseDrugsList = purchaseDrugsBLL.GetList(en => PIds.Contains(en.PId), en => en.tb_DrugInfo, en => en.tb_DepotInfo);
            List<dynamic> IdsList = new List<dynamic>();
            foreach (var pd in purchaseDrugsList)
            {
                var Ids = new
                {
                    depotId = pd.DepotId,
                    drugId = pd.DrugId
                };
                IdsList.Add(Ids);
            }
            //depotDrugBLL.GetList(en => !IdList.Contains(en.DepotId, en.DrugId));
            var depotList = stockViewBLL.GetList(en => true);
            var finallist = depotList.Where(en => !IdsList.Contains(en.depotId, en.drugId));
            //var list = from s in depotList
            //           group s by new { s.tb_DrugInfo, s.tb_DepotInfo } into g
            //           select new
            //           {
            //               depotId = g.Key.tb_DepotInfo.Id,
            //               drugId = g.Key.tb_DrugInfo.Id,
            //               depotName = g.Key.tb_DepotInfo.Name,
            //               drugName = g.Key.tb_DrugInfo.Name,
            //               num = g.Sum(en => en.Num)

            //           };
            var slist = finallist.Where(en => en.Num <= 500);
            var result = slist.Skip(page * pageSize).Take(pageSize);
            return new { dataItem = result, count = slist.Count() };
        }
        [HttpPost]
        public dynamic GetPurchasePlan(dynamic data)
        {
            int page = 0;
            int pageSize = 20;
            Expression<Func<models.tb_Purchase, bool>> where = en => true;
            if (data != null)
            {
                page = data.page;
                pageSize = data.pageSize;

            }
            var purchaseList = purchaseBLL.GetPageList(page, pageSize, where, en => en.Id, en => en.tb_PurchaseDrugs);
            var count = purchaseList.Count;
            var list = purchaseList.Select(en => new
            {
                Id = en.Id,
                Finish = en.Finish,
                CreateDate = en.CreateDate.ToShortDateString(),
                CloseDate = en.CloseDate.HasValue ? en.CloseDate.Value.ToShortDateString() : "",
                Total = en.tb_PurchaseDrugs.Count
            });
            return new { dataItem = list, count = count };
        }
        [HttpPost]
        public dynamic PostBuildPurhasePlan(dynamic data)
        {
            List<int> depotIdList = new List<int>();
            List<int> drugIdList = new List<int>();
            Dictionary<int, List<int>> ind = new Dictionary<int, List<int>>();
            List<dynamic> Idlist = new List<dynamic>();
            //List<models.tb_PurchaseDrugs> purchaseDrugsList = new List<models.tb_PurchaseDrugs>();
            int depotId = 0;
            int drugId = 0;
            int num = 0;
            int supId = 0;
            if (data.purchase != null)
            {
                foreach (var d in data.purchase)
                {
                    if (d.depotId != null)
                        depotId = d.depotId;
                    if (d.drugId != null )
                        drugId = d.drugId;
                    if (d.pnum != null)
                        num = d.pnum;
                    if (d.sup != null)
                        supId = d.sup;
                    if (depotId == 0 || drugId == 0 || num == 0 || supId == 0)
                    {
                        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Conflict));
                    }
                    var Ids = new
                    {
                        depotId = depotId,
                        drugId = drugId,
                        num = num,
                        supId = supId
                    };
                    Idlist.Add(Ids);

                }

            }
            else
            {
                return null;
            }
            models.tb_Purchase purchase = new models.tb_Purchase()
            {
                CreateDate = DateTime.Now,
                Finish = false
            };
            foreach (var ids in Idlist)
            {
                models.tb_PurchaseDrugs purchaseDrugs = new models.tb_PurchaseDrugs()
                {
                    DrugId = ids.drugId,
                    DepotId = ids.depotId,
                    Num = ids.num,
                    SupId = ids.supId

                };
                purchase.tb_PurchaseDrugs.Add(purchaseDrugs);
            }
            int count = 0;
            if (purchase.tb_PurchaseDrugs.Count > 0)
            {
                count = purchaseBLL.Add(purchase);
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Conflict));
            }

            return new { count = count };
        }
        [HttpPost]
        public dynamic GetPurchaseDrug(dynamic data)
        {
            int page = 0;
            int pageSize = 20;
            int depotId = 1;
            int drugId = 1;
            int num = 500;
            int pId = 1;
            Expression<Func<models.tb_PurchaseDrugs, bool>> where = en => true;
            if (data != null)
            {
                page = data.page;
                pageSize = data.pageSize;
                if (data.depotId != null)
                {
                    depotId = data.depotId;
                    where = where.And(en => en.DepotId == depotId);
                }
                if (data.PId != null)
                {
                    pId = data.PId;
                    where = where.And(en => en.PId == pId);
                }
                if (data.drugId != null)
                {
                    drugId = data.drugId;
                    where = where.And(en => en.DrugId == drugId);
                }
            }
            var count = purchaseDrugsBLL.GetCount(where);
            var purchaseDrugList = purchaseDrugsBLL.GetPageList(page, pageSize, where, en => en.PId, en => en.tb_DepotInfo, en => en.tb_DrugInfo, en => en.tb_SupplierInfo);
            var list = purchaseDrugList.Select(en => new
            {
                depotId = en.DepotId,
                drugId = en.DrugId,
                drugName = en.tb_DrugInfo.Name,
                depotName = en.tb_DepotInfo.Name,
                pId = en.PId,
                num = en.Num,
                supName = en.tb_SupplierInfo.Name
            });
            return new { dataItem = list, count = count };
        }
        [HttpPost]
        public dynamic PostModifyPurchaseDrug(dynamic data)
        {
            //List<models.tb_PurchaseDrugs> purchaseDrugsList = new List<models.tb_PurchaseDrugs>();
            int depotId = 0;
            int drugId = 0;
            int pid = 0;
            int num = 0;
            if (data != null)
            {
                if (data.depotid != null)
                    depotId = data.depotid;
                if (data.drugid != null)
                    drugId = data.drugid;
                if (data.pid != null)
                    pid = data.pid;
                if (data.num != null)
                    num = data.num;
                if (depotId == 0 || drugId == 0 || num == 0 || pid == 0)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Conflict));
                }
                var purchaseDrug = purchaseDrugsBLL.GetList(en => en.DepotId == depotId && en.DrugId == drugId && en.PId == pid).FirstOrDefault();
                if (purchaseDrug == null)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Conflict));
                }
                purchaseDrug.Num = num;
                purchaseDrugsBLL.Modify(purchaseDrug);
                return true;
            }
            else
            {
                return null;
            }
            ;
        }
        [HttpPost]
        public dynamic PostBuildPurchaseDrug(dynamic data)
        {
            List<models.tb_DepotIn> depotInList = new List<models.tb_DepotIn>();
            List<int> pids = new List<int>();
            if (data != null)
            {
                foreach (var d in data)
                {
                    int pid = d;
                    pids.Add(pid);
                }

            }
            else
            {
                return null;
            }
            var purchaseList=purchaseBLL.GetList(en => pids.Contains(en.Id));
            if (purchaseList != null)
            {
                foreach (var purchase in purchaseList)
                {
                    purchase.CloseDate = DateTime.Now;
                    purchase.Finish = true;
                    foreach (var drug in purchase.tb_PurchaseDrugs)
                    {
                        models.tb_DepotIn depotin = new models.tb_DepotIn()
                        {
                            DepotId = drug.DepotId,
                            DrugId = drug.DrugId,
                            InDate = DateTime.Now,
                            Num = drug.Num,
                            ProductionDate = DateTime.Now,
                            SupId = drug.SupId
                        };
                        models.tb_DepotDrug depotDrug = new models.tb_DepotDrug()
                        {
                            DepotId = drug.DepotId,
                            DrugId = drug.DrugId,
                            ProductionDate = DateTime.Now,
                            Num = drug.Num
                        };
                        depotin.tb_DepotDrug.Add(depotDrug);
                        depotInList.Add(depotin);
                    }
                }
                depotInBLL.AddRange(depotInList);
                purchaseBLL.Modify(purchaseList);
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Conflict));
            }

            return true;
        }
       
        #endregion
        #region 药品查询
        [HttpPost]
        public dynamic GetDrugCheckData(dynamic data)
        {
            int page = 0;
            int pageSize = 20;
            int depotId = 1;
            int drugId = 1;
            int stockMore=0;
            int stockLess=0;
            string depotVague = "";
            string drugVague = "";
            DateTime earlyMore;
            DateTime earlyLess;
            DateTime lateMore;
            DateTime lateLess;
            Expression<Func<models.drugCheck, bool>> where = en => true;
            try
            {
                if (data != null)
                {
                    page = data.page;
                    pageSize = data.pageSize;
                    if(data.drugVague!=null)
                    {
                        drugVague = data.drugVague;
                        where = where.And(en => en.drugName.Contains(drugVague));
                    }
                    if (data.depotVague != null)
                    {
                        depotVague = data.depotVague;
                        where = where.And(en => en.depotName.Contains(depotVague));
                    }
                    if (data.depotId != null)
                    {
                        depotId = data.depotId;
                        where = where.And(en => en.depotId == depotId);
                    }
                    if (data.drugId != null)
                    {
                        drugId = data.drugId;
                        where = where.And(en => en.drugId == drugId);
                    }
                    if (data.stockMore != null)
                    {
                        stockMore = data.stockMore;
                        where = where.And(en => en.num >= stockMore);
                    }
                    if (data.stockLess != null)
                    {
                        stockLess = data.stockLess;
                        where = where.And(en => en.num <= stockLess);
                    }
                    if (data.earlyMore != null)
                    {
                        earlyMore = data.earlyMore;
                        where = where.And(en => en.earlyDate >= earlyMore);
                    }
                    if (data.earlyLess != null)
                    {
                        earlyLess = data.earlyLess;
                        where = where.And(en => en.earlyDate <= earlyLess);
                    }
                    if (data.lateMore != null)
                    {
                        lateMore = data.lateMore;
                        where = where.And(en => en.lateDate >= lateMore);
                    }
                    if (data.lateLess != null)
                    {
                        lateLess = data.lateLess;
                        where = where.And(en => en.lateDate <= lateLess);
                    }

                }
                var t = drugCheckBLL.GetPageList(page,pageSize,where,en=>en.depotId);
                //var l = depotDrugBLL.GetList(where, en => en.tb_DepotOut, en => en.tb_DepotIn, en => en.tb_DepotInfo, tb => tb.tb_DrugInfo);
                //var t = from s in l
                //        group s by new { s.tb_DrugInfo, s.tb_DepotInfo } into g
                //        orderby g.Key.tb_DepotInfo.Name
                //        select new
                //        {

                //            depotName = g.Key.tb_DepotInfo.Name,
                //            drugName = g.Key.tb_DrugInfo.Name,
                //            outNum = g.Sum(en => en.tb_DepotOut.Sum(x => x.Num)),
                //            inNum = g.Sum(en => en.tb_DepotIn.Num),
                //            Num = g.Sum(en => en.Num)

                //        };
                var result = t.AsEnumerable();
                int count = drugCheckBLL.GetCount(where);
                return new { dataItem = result, count = count };
            }catch
            {
                return new { };

            }
        }
        [HttpPost]
        public dynamic DownDrugCheck(dynamic data)
        {
            int depotId = 1;
            int drugId = 1;
            int stockMore=0;
            int stockLess=0;
            string depotVague = "";
            string drugVague = "";
            DateTime earlyMore;
            DateTime earlyLess;
            DateTime lateMore;
            DateTime lateLess;
            Expression<Func<models.drugCheck, bool>> where = en => true;
            try
            {
                if (data != null)
                {
                    if (data.drugVague != null)
                    {
                        drugVague = data.drugVague;
                        where = where.And(en => en.drugName.Contains(drugVague));
                    }
                    if (data.depotVague != null)
                    {
                        depotVague = data.depotVague;
                        where = where.And(en => en.depotName.Contains(depotVague));
                    }
                    if (data.depotId != null)
                    {
                        depotId = data.depotId;
                        where = where.And(en => en.depotId == depotId);
                    }
                    if (data.drugId != null)
                    {
                        drugId = data.drugId;
                        where = where.And(en => en.drugId == drugId);
                    }
                    if (data.stockMore != null)
                    {
                        stockMore = data.stockMore;
                        where = where.And(en => en.num >= stockMore);
                    }
                    if (data.stockLess != null)
                    {
                        stockLess = data.stockLess;
                        where = where.And(en => en.num <= stockLess);
                    }
                    if (data.earlyMore != null)
                    {
                        earlyMore = data.earlyMore;
                        where = where.And(en => en.earlyDate >= earlyMore);
                    }
                    if (data.earlyLess != null)
                    {
                        earlyLess = data.earlyLess;
                        where = where.And(en => en.earlyDate <= earlyLess);
                    }
                    if (data.lateMore != null)
                    {
                        lateMore = data.lateMore;
                        where = where.And(en => en.lateDate >= lateMore);
                    }
                    if (data.lateLess != null)
                    {
                        lateLess = data.lateLess;
                        where = where.And(en => en.lateDate <= lateLess);
                    }

                }
                var list = drugCheckBLL.GetList(where);
                HttpContext.Current.Session["DrugCheckList"] = list;
                return true;
            }
            catch
            {
                return false;
            }
                

        }
        #endregion
        #region 获取普通数据
        [HttpGet]
        public dynamic GetDepot()
        {
            var list = depotBLL.GetList(en => true);
            List<dynamic> result = new List<dynamic>(); ;
            foreach (var l in list)
            {
                var r = new
                {
                    Name = l.Name,
                    Id = l.Id
                };
                result.Add(r);
            }

            return new { dataItem = result, count = list.Count };
        }
        [HttpGet]
        public dynamic GetDrug()
        {
            var list = drugBLL.GetList(en => true);
            List<dynamic> result = new List<dynamic>(); ;
            foreach (var l in list)
            {
                var r = new
                {
                    Name = l.Name,
                    Id = l.Id
                };
                result.Add(r);
            }

            return new { dataItem = result, count = list.Count };
        }
        [HttpGet]
        public dynamic GetSup()
        {
            var list = supplierBLL.GetList(en => true);
            List<dynamic> result = new List<dynamic>(); ;
            foreach (var l in list)
            {
                var r = new
                {
                    Name = l.Name,
                    Id = l.Id
                };
                result.Add(r);
            }
            return new { dataItem = result, count = list.Count };
        }

        private string DateToYearMonth(DateTime d)
        {
            return d.ToString("yyyy-MM");
        }
        #endregion
    }
}
