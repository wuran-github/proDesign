using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DataInsert
{
    class Program
    {
        static string[] Snames = { "云南白药", "白云山", "同仁堂","三九医药","德众","花城","九芝堂" };
        static string[] drugNames = {"复方感冒灵","维C银翘片","复方黄连素","腹可安","咽炎片","藿香正气丸","牛黄解毒片","西瓜霜","清火片","健胃消食片" };
        static BLL.DamagedDrugBLL damagedDrugBLL = new BLL.DamagedDrugBLL();
        static BLL.DepotBLL depotBLL = new BLL.DepotBLL();
        static BLL.DepotDrugBLL depotDrugBLL = new BLL.DepotDrugBLL();
        static BLL.DepotInBLL depotInBLL = new BLL.DepotInBLL();
        static BLL.DepotOutBLL depotOutBLL = new BLL.DepotOutBLL();
        static BLL.DrugBLL drugBLL = new BLL.DrugBLL();
        static BLL.SupplierBLL supplierBLL = new BLL.SupplierBLL();
        static BLL.WarningBLL warningBLL = new BLL.WarningBLL();
        static BLL.PurchaseBLL purchaseBLL=new BLL.PurchaseBLL();
        static BLL.PurchaseDrugsBLL purchaseDrugsBLL=new BLL.PurchaseDrugsBLL();
        static BLL.StockViewBLL stockViewBLL = new BLL.StockViewBLL();

        static string[] depotNames = { "东仓库", "西仓库", "南仓库", "北仓库" };
        static void Main(string[] args)
        {
           
            DateTime time = new DateTime(2016, 5, 4);
            int[] months = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            selectStock();
            //insertPur();
            //List<models.tb_SupplierInfo> slist = new List<models.tb_SupplierInfo>();
            //for (int i = 0; i < 7; i++)
            //{
            //    models.tb_SupplierInfo supp = new models.tb_SupplierInfo()
            //    {
            //        Name = Snames[i],
            //        CompanyPhone = "1234567891" + i,
            //        ContactName = "张三",
            //        Email = "test@123.com",
            //        Fax = "1234567-321",
            //        LegalPersons = "李四",
            //        OrganizationCode = "00000000-0",
            //        PostalCode = "300000",
            //        Address = "中国",
            //        ContactPhone = "1234567891" + 1

            //    };
            //    slist.Add(supp);
            //}
            //supplierBLL.AddRange(slist);
            //List<models.tb_DrugInfo> dlist = new List<models.tb_DrugInfo>();
            //int l = 0;
            //foreach (var s in drugNames)
            //{
            //    models.tb_DrugInfo drug = new models.tb_DrugInfo()
            //    {
            //        LicenseNumber = "国药准字Z1002021" + l,
            //        Name = s,
            //        spec = "盒",
            //        Shelf = 365,
            //        Uses = "感冒,上火"
            //    };
            //    l++;
            //    dlist.Add(drug);
            //}
            //drugBLL.AddRange(dlist);
            //List<models.tb_DepotInfo> ddlist = new List<models.tb_DepotInfo>();
            //for (int i = 0; i < 4; i++)
            //{
            //    models.tb_DepotInfo d = new models.tb_DepotInfo()
            //    {
            //        Address = "天津",
            //        Area = 400.5,
            //        Name = depotNames[i],
            //        Type = i
            //    };
            //    ddlist.Add(d);
            //}
            ////depotBLL.AddRange(ddlist);
            //List<models.tb_DepotIn> ilist = new List<models.tb_DepotIn>();
            //List<models.tb_DepotDrug> dddlist = new List<models.tb_DepotDrug>();
            //for (int i = 0; i < 100; i++)
            //{
            //    Random r = new Random(Guid.NewGuid().GetHashCode());
            //    models.tb_DepotIn din = new models.tb_DepotIn();
            //    din.DepotId = r.Next(1, 5);
            //    r = new Random(Guid.NewGuid().GetHashCode());
            //    din.DrugId = r.Next(1, 11);
            //    r = new Random(Guid.NewGuid().GetHashCode());
            //    din.Num = r.Next(2, 10) * 100;
            //    r = new Random(Guid.NewGuid().GetHashCode());
            //    din.SupId = r.Next(1, 8);
            //    r = new Random(Guid.NewGuid().GetHashCode());
            //    din.InDate = time.AddMonths(r.Next(0, 12)).AddDays(-r.Next(1, 30));
            //    r = new Random(Guid.NewGuid().GetHashCode());
            //    din.ProductionDate = time.AddMonths(-r.Next(1, 5));
            //    r = new Random(Guid.NewGuid().GetHashCode());
            //    ilist.Add(din);

            //    models.tb_DepotDrug dd = new models.tb_DepotDrug()
            //    {
            //        ProductionDate = din.ProductionDate,
            //        Num = din.Num,
            //        DepotId = din.DepotId,
            //        DrugId = din.DrugId,
            //        DepotInId = i + 1
            //    };
            //    dddlist.Add(dd);
            //}
            //depotInBLL.AddRange(ilist);
            //depotDrugBLL.AddRange(dddlist);
            //List<models.tb_DepotOut> dlist1 = new List<models.tb_DepotOut>();
            //List<models.tb_DepotDrug> ddlist2 = depotDrugBLL.SingleContextGetList(en => true, en => en.tb_DepotIn);
            models.tb_DepotDrug dd = depotDrugBLL.SingleContextGetList(en => en.Id == 1).FirstOrDefault();
            models.tb_DepotOut depotout = new models.tb_DepotOut()
            {
                Num=100
            };
            
            dd.Num -= 100;
            dd.tb_DepotOut.Add(depotout);   
            depotDrugBLL.SingleContextModify(dd);
            //for (int i = 0; i < 100; i++)
            //{
            //    Random r = new Random(Guid.NewGuid().GetHashCode());
            //    models.tb_DepotDrug dd = ddlist2[r.Next(0, ddlist2.Count)];
            //    var depotIn = depotInBLL.GetList(en => en.Id == dd.DepotInId).FirstOrDefault();
            //    int num = r.Next(2, 5) * 10;
            //    models.tb_DepotOut din = new models.tb_DepotOut();
            //    din.DepotId = dd.DepotId;
            //    din.DrugId = dd.DrugId;
            //    r = new Random(Guid.NewGuid().GetHashCode());
            //    dd.Num -= num;
            //    din.Num = num;
            //    din.SupId = 1;
            //    r = new Random(Guid.NewGuid().GetHashCode());
            //    var R = new Random(Guid.NewGuid().GetHashCode());
            //    din.OutDate = dd.tb_DepotIn.InDate.AddMonths(r.Next(0, 10)).AddDays(R.Next(1, 10));
            //    din.tb_DepotDrug.Add(dd);
            //    dlist1.Add(din);
            //}
            //depotOutBLL.SingleContextAddRange(dlist1);
            //int year = 2016;
            //DateTime time2 = new DateTime(year, 1, 1);
            //DateTime time3 = new DateTime(year + 1, 1, 1);
            //var outIds = depotOutBLL.GetList(de => de.OutDate >= time2 && de.OutDate <time3).Select(en => en.Id);
            //var l = depotDrugBLL.GetList(en => en.tb_DepotIn.InDate >= time2 && en.tb_DepotIn.InDate < time3 && en.DepotId==1 && en.DrugId==1, en => en.tb_DepotOut, en => en.tb_DepotIn, en => en.tb_DepotInfo, tb => tb.tb_DrugInfo);
            //var depotdrug = from s in l
            //                group s by new { s.DepotId, s.DrugId } into g
            //                select new
            //                {
            //                    depotId = g.Key.DepotId,
            //                    drugId = g.Key.DrugId,
            //                    sum = g.Sum(en => en.Num)
            //                };
            
            //var c = from m in months
            //        join s in l on m equals  s.tb_DepotIn.InDate.Month into monthDepot
            //        from md in monthDepot.DefaultIfEmpty()
            //        group md by new { md.tb_DrugInfo, md.tb_DepotInfo, month = m } into g
            //        select new
            //        {
            //            depotName ="东仓库",
            //            drugName = "复方感冒灵",
            //            outNum =g!=null?g.Sum(en => en.tb_DepotOut.Where(de => outIds.Contains(de.Id)).Sum(x => x.Num)):0,
            //            inNum =g!=null?g.Sum(en => en.tb_DepotIn.Num):0,
            //            Num = g!= null?g.Sum(en => en.tb_DepotIn.Num)-g.Sum(en => en.tb_DepotOut.Where(de => outIds.Contains(de.Id)).Sum(x => x.Num)):0,
            //            month = g.Key.month

            //        };
            //DefaultIfEmpty()能够为空序列提供一个默认的元素。DefaultIfEmpty使用到了泛型的default关键词，
            //对于引用类型将返回null，而对于值类型将返回0。对于结构体类型，则会根据其成员类型将他们对应的初始化为null或者0.
            //var z = from m in months
            //        join s in t
            //        on m equals s.month into os
            //        from x in os.DefaultIfEmpty()
            //        select new
            //        {
            //            depotName =(x==null)?null:x.depotName,
            //            drugName = (x == null) ? null : x.drugName,
            //            outNum = (x == null) ? 0 : x.outNum,
            //            inNum = (x == null) ? 0 : x.inNum,
            //            Num = (x == null) ? 0 : x.Num,
            //            month = m
            //        }
            //                ;
            //var l = depotDrugBLL.GetList(en => true, en => en.tb_DepotOut, en => en.tb_DepotIn, en => en.tb_DepotInfo, tb => tb.tb_DrugInfo);
            //var t = from s in l
            //        group s by new {s.tb_DrugInfo,s.tb_DepotInfo} into g
            //        orderby g.Key.tb_DepotInfo.Name
            //        select new
            //        {
            //            depotName=g.Key.tb_DepotInfo.Name,
            //            drugName=g.Key.tb_DrugInfo.Name,
            //            outNum=g.Sum(en=>en.tb_DepotOut.Sum(x=>x.Num)),
            //            inNum=g.Sum(en=>en.tb_DepotIn.Num)

            //        };
            //int a;
        }
        static void insertPur()
        {
            //models.tb_Purchase purchase = new models.tb_Purchase()
            //{
            //    CreateDate = DateTime.Now,
            //    Finish = false
            //};
            var purchase = purchaseBLL.SingleContextGetList(en => en.Id == 1,en=>en.tb_PurchaseDrugs).FirstOrDefault();
            models.tb_PurchaseDrugs purchaseDrugs = new models.tb_PurchaseDrugs()
            {
                DrugId = 2,
                DepotId = 1,
                SupId=1,
                Num=10
            };
            purchase.tb_PurchaseDrugs.Add(purchaseDrugs);
            purchaseBLL.SingleContextModify(purchase);

        }
        static void selectStock()
        {
            models.DBDepotEntities db = new models.DBDepotEntities();
            var d = db.Set<models.StockView>().AsNoTracking();
            var t=d.ToList();
        }
    }
}
