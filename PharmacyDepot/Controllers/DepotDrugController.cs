using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PharmacyDepot.Controllers
{
    public class DepotDrugController : ApiController
    {
        BLL.DepotInBLL depotInBLL = new BLL.DepotInBLL();
        List<models.tb_DepotIn> tb_depotins = new List<models.tb_DepotIn>();
        BLL.DepotDrugBLL depotDrugBLL = new BLL.DepotDrugBLL();
        List<models.tb_DepotDrug> tb_depotdrugs = new List<models.tb_DepotDrug>();
        BLL.DepotBLL depotBLL = new BLL.DepotBLL();
        BLL.DepotOutBLL depotOutBLL = new BLL.DepotOutBLL();
        List<models.tb_DepotOut> tb_depotouts = new List<models.tb_DepotOut>();
        List<models.tb_DepotInfo> tb_depotinfos = new List<models.tb_DepotInfo>();
        BLL.DrugBLL drugBLL = new BLL.DrugBLL();
        List<models.tb_DrugInfo> tb_druginfos = new List<models.tb_DrugInfo>();
        BLL.SupplierBLL supBLL = new BLL.SupplierBLL();
        List<models.tb_SupplierInfo> tb_suppliers = new List<models.tb_SupplierInfo>();

        [HttpPost]
        public dynamic GetDepotDrugData(dynamic data)
        {
            //设置标志变量，标志返回的页码的状态
            //1普通页面/2超出范围/3第一页（第0页）/4最后一页/5只有一页
            int[] flag = { 1, 2, 3, 4 ,5 };
            //初始化传过来的参数
            int page = 0; 
            //由服务器决定的每页显示的条数
            int pageSize = 10;
            if (data.index != null ) 
            {//数据存在才赋值
                page = data.index;
            }
            //查询出所有的数据
            //var list = depotDrugBLL.GetList(e=>true,e=>e.tb_DepotIn);
            int listCount = depotDrugBLL.GetCount(en=>true);
            //检查是否超过page的范围
            if( page < 0 || page > listCount / pageSize - 1)
            {//请求的页index小于第一页（第0页）或 请求的页index大于最后一页
                return new{ f = flag[1] };
            }
            else
            {//请求的页index正常
                var f = 0;
                //设置标志
                //
                if ( page == 0) { f = flag[2]; }//第一页
                else if ( page == listCount / pageSize - 1) { f = flag[3]; }//最后一页
                else if (page == 0 && page == listCount / pageSize - 1) { f = flag[4]; }//只有一页
                else { f = flag[0]; }//普通页面
                //查询数据
                //虽然查出来了，但是由于某种原因，需要投影出来属性
                var result = depotDrugBLL.GetPageList(page,pageSize,en=>true,en=>en.Id,en=>en.tb_DepotIn).Select(
                        en => new
                        {
                            id = en.Id,
                            depotinId=en.tb_DepotIn.Id,
                            depotId = en.DepotId,
                            drugId = en.DrugId,
                            num = en.Num,
                            proDate = en.ProductionDate

                        }
                    );
                return new { items = result, page = page , f = f };
            }
        }
        [HttpGet]
        public dynamic GetLastDepotDrug() 
        {//获取最后一页数据
            //由服务器决定的每页显示的条数
            int pageSize = 10;
            //查询出所有的数据
            //var list = depotDrugBLL.GetList(e => true, e => e.tb_DepotIn);
            int listCount = depotDrugBLL.GetCount(en=>true);
            //设置为查询最后一页
            int page = listCount / pageSize - 1;
            var result = depotDrugBLL.GetPageList(page,pageSize,en=>true,en=>en.Id,en=>en.tb_DepotIn).Select(
                        en => new
                        {
                            id = en.Id,
                            depotinId = en.tb_DepotIn.Id,
                            depotId = en.DepotId,
                            drugId = en.DrugId,
                            num = en.Num,
                            proDate = en.ProductionDate
                        }
                    );
            return new { items = result ,page=page };
        }
        [HttpPost]
        public dynamic AddDepotDrug(dynamic data) 
        {//用来接收ajax传来的数据
            //获取数据写入数据库
            if (data.result != null)
            {
                try 
                {
                    List<models.tb_DepotIn> depotInList = new List<models.tb_DepotIn>();
                    var drugList=drugBLL.GetList(en=>true);
                    
                    foreach(var d in data.result)
                    {
                        //初始化对象
                        models.tb_DepotIn tb_depotin = new models.tb_DepotIn();
                        models.tb_DepotDrug tb_depotdrug = new models.tb_DepotDrug();
                        models.tb_Warning warn = new models.tb_Warning();
                        int depotId = d.depotId;
                        int drugId = d.drugId;
                        int supId = d.supId;
                        int num = d.num;
                        DateTime inDate = d.inDate;
                        DateTime proDate = d.proDate;
                        //为每一条仓库药品对象赋值
                        tb_depotdrug.DepotId = depotId;
                        tb_depotdrug.DrugId = drugId;
                        tb_depotdrug.Num = num;
                        tb_depotdrug.ProductionDate = proDate;
                        tb_depotin.tb_DepotDrug.Add(tb_depotdrug);
                        //添加预警信息
                        double days =drugList.FirstOrDefault(en => en.Id == depotId).Shelf * 0.8;
                        warn.WarningDays = proDate.AddDays(days);
                        tb_depotdrug.tb_Warning.Add(warn);
                        //为每一条入库记录对象赋值
                        tb_depotin.DepotId = depotId;
                        tb_depotin.DrugId = drugId;
                        tb_depotin.SupId = supId;
                        tb_depotin.Num = num;
                        tb_depotin.InDate = inDate;
                        tb_depotin.ProductionDate = proDate;
                        depotInList.Add(tb_depotin);
                    }
                    depotInBLL.AddRange(depotInList);

                }
                catch(Exception e)
                {
                    return e.ToString();
                }
            }
            return new{ };
        }
        [HttpPost]
        public dynamic OutStock(dynamic data)
        {//用来接收ajax传来的数据
            //获取数据写入数据库
            if (data.result != null)
            {
                try
                {
                    foreach (var d in data.result)
                    {
                        
                        //取出值
                        int depotdrugId = d.depotdrugId;
                        int num = d.num;
                        DateTime outDate = d.outDate;
                        //初始化对象
                        models.tb_DepotDrug depotdrug = depotDrugBLL.SingleContextGetList(e => e.Id == depotdrugId).FirstOrDefault();
                        models.tb_DepotOut depotout = new models.tb_DepotOut();
                        //赋值
                        depotout.DepotId = depotdrug.DepotId;
                        depotout.DrugId = depotdrug.DrugId;
                        depotout.SupId = depotdrug.tb_DepotIn.SupId;
                        depotout.Num = num;
                        depotout.OutDate = outDate;
                        //////出库
                        depotdrug.Num -= num;
                        depotdrug.tb_DepotOut.Add(depotout);
                        depotDrugBLL.SingleContextModify(depotdrug);
                    }
                }
                catch (Exception e)
                {
                    return e.ToString();
                }
            }
            return new { };
        }
        [HttpPost]
        public dynamic ModifyDepotDrug(dynamic data)
        {
            ///修改规则
            ///单修改仓库药品
            ///修改失败f=0，成功f=1；
            if ( data == null)
            {
                return new { f = 0 };
            }
            else
            {
                int id = data.id;
                //depotinId
                int depotId = data.depotId;
                int drugId = data.drugId;
                int num = data.num;
                DateTime proDate = data.proDate;
                models.tb_DepotDrug depotdrug = depotDrugBLL.SingleContextGetList(en => en.Id == id).FirstOrDefault();
                //depotdrug.Id = id;
                depotdrug.DepotId = depotId;
                depotdrug.DrugId = drugId;
                depotdrug.Num = num;
                depotdrug.ProductionDate = proDate;
                depotDrugBLL.SingleContextModify(depotdrug);
                //修改成功返回data
                return new { items = data, f = 1 };  
            }   
        }
        [HttpPost]
        public dynamic TransferDepotDrug(dynamic data) 
        {
            if (data == null)
            {
                return new { f = 0 };
            }
            else
            { 
                //midify
                int outId = data.outId;
                int transNum = data.transNum;
                int inId = data.inId;
                //转出：原来的num减去转出的num
                models.tb_DepotDrug depotdrug1 = depotDrugBLL.SingleContextGetList(en => en.Id == outId).FirstOrDefault();
                depotdrug1.Num = depotdrug1.Num - transNum;
                //转入：原来的num加上转入的num
                models.tb_DepotDrug depotdrug2 = depotDrugBLL.SingleContextGetList(en => en.Id == inId).FirstOrDefault();
                depotdrug2.Num = depotdrug2.Num + transNum;
                //保存修改
                depotDrugBLL.SingleContextModify(depotdrug1);
                depotDrugBLL.SingleContextModify(depotdrug2);
                //修改成功返回data
                return new { items = data, f = 1 };  
            }
        }
        [HttpPost]
        public dynamic ModifyInstock(dynamic data) 
        {
            if (data == null)
            {
                return new { f = 0 };
            }
            else
            {
                int id = data.id;
                int depotId = data.depotId;
                int drugId = data.drugId;
                int supId = data.supId;
                int num = data.num;
                DateTime inDate = data.inDate;
                DateTime proDate = data.proDate;
                models.tb_DepotIn depotin= depotInBLL.SingleContextGetList(en => en.Id == id).FirstOrDefault();
                //depotdrug.Id = id;
                depotin.DepotId = depotId;
                depotin.DrugId = drugId;
                depotin.SupId = supId;
                depotin.Num = num;
                depotin.InDate = inDate;
                depotin.ProductionDate = proDate;
                depotInBLL.SingleContextModify(depotin);
                //修改成功返回data
                return new { items = data, f = 1 };
            }   
        }
        [HttpPost]
        public dynamic GetInstockData(dynamic data)
        {
            //设置标志变量，标志返回的页码的状态
            //1普通页面/2超出范围/3第一页（第0页）/4最后一页/5只有一页
            int[] flag = { 1, 2, 3, 4, 5 };
            //初始化传过来的参数
            int page = 0;
            //由服务器决定的每页显示的条数
            int pageSize = 10;
            if (data.index != null)
            {//数据存在才赋值
                page = data.index;
            }
            //查询出所有的数据
            //var list = depotDrugBLL.GetList(e=>true,e=>e.tb_DepotIn);
            int listCount = depotInBLL.GetCount(en => true);
            //检查是否超过page的范围
            if (page < 0 || page > listCount / pageSize - 1)
            {//请求的页index小于第一页（第0页）或 请求的页index大于最后一页
                return new { f = flag[1] };
            }
            else
            {//请求的页index正常
                var f = 0;
                //设置标志
                //
                if (page == 0) { f = flag[2]; }//第一页
                else if (page == listCount / pageSize - 1) { f = flag[3]; }//最后一页
                else if (page == 0 && page == listCount / pageSize - 1) { f = flag[4]; }//只有一页
                else { f = flag[0]; }//普通页面
                //查询数据
                //虽然查出来了，但是由于某种原因，需要投影出来属性
                var result = depotInBLL.GetPageList(page, pageSize, en => true, en => en.Id).Select(
                        en => new
                        {
                            id = en.Id,
                            depotId = en.DepotId,
                            drugId = en.DrugId,
                            supId = en.SupId,
                            num = en.Num,
                            inDate = en.InDate,
                            proDate = en.ProductionDate
                        }
                    );
                return new { items = result, page = page, f = f };
            }
        }
        [HttpGet]
        public dynamic GetLastDepotIn()
        {//获取最后一页数据
            //由服务器决定的每页显示的条数
            int pageSize = 10;
            //查询出所有的数据
            //var list = depotInBLL.GetList(e => true);
            int listCount = depotInBLL.GetCount(en=>true);
            //设置为查询最后一页
            int page = listCount / pageSize - 1;
            var result = depotInBLL.GetPageList(page,pageSize,en=>true,en=>en.Id).Select(
                        en => new
                        {
                            id = en.Id,
                            depotId = en.DepotId,
                            drugId = en.DrugId,
                            supId = en.SupId,
                            num = en.Num,
                            inDate = en.InDate,
                            proDate = en.ProductionDate
                        }
                    );
            return new { items = result, page = page };
        }
        [HttpPost]
        public dynamic ModifyOutstock(dynamic data)
        {
            if (data == null)
            {
                return new { f = 0 };
            }
            else
            {
                int id = data.id;
                int depotId = data.depotId;
                int drugId = data.drugId;
                int supId = data.supId;
                int num = data.num;
                DateTime outDate = data.outDate;
                models.tb_DepotOut depotout = depotOutBLL.SingleContextGetList(en => en.Id == id).FirstOrDefault();
                //depotdrug.Id = id;
                depotout.DepotId = depotId;
                depotout.DrugId = drugId;
                depotout.SupId = supId;
                depotout.Num = num;
                depotout.OutDate = outDate;
                depotOutBLL.SingleContextModify(depotout);
                //修改成功返回data
                return new { items = data, f = 1 };
            }
        }
        [HttpPost]
        public dynamic GetOutstockData(dynamic data)
        {
            //设置标志变量，标志返回的页码的状态
            //1普通页面/2超出范围/3第一页（第0页）/4最后一页/5只有一页
            int[] flag = { 1, 2, 3, 4, 5 };
            //初始化传过来的参数
            int page = 0;
            //由服务器决定的每页显示的条数
            int pageSize = 10;
            if (data.index != null)
            {//数据存在才赋值
                page = data.index;
            }
            //查询出所有的数据
            //var list = depotDrugBLL.GetList(e=>true,e=>e.tb_DepotIn);
            int listCount = depotOutBLL.GetCount(en => true);
            //检查是否超过page的范围
            if (page < 0 || page > listCount / pageSize - 1)
            {//请求的页index小于第一页（第0页）或 请求的页index大于最后一页
                return new { f = flag[1] };
            }
            else
            {//请求的页index正常
                var f = 0;
                //设置标志
                //
                if (page == 0) { f = flag[2]; }//第一页
                else if (page == listCount / pageSize - 1) { f = flag[3]; }//最后一页
                else if (page == 0 && page == listCount / pageSize - 1) { f = flag[4]; }//只有一页
                else { f = flag[0]; }//普通页面
                //查询数据
                //虽然查出来了，但是由于某种原因，需要投影出来属性
                var result = depotOutBLL.GetPageList(page, pageSize, en => true, en => en.Id).Select(
                        en => new
                        {
                            id = en.Id,
                            depotId = en.DepotId,
                            drugId = en.DrugId,
                            supId = en.SupId,
                            num = en.Num,
                            outDate = en.OutDate
                        }
                    );
                return new { items = result, page = page, f = f };
            }
        }
        [HttpGet]
        public dynamic GetLastDepotOut()
        {//获取最后一页数据
            //由服务器决定的每页显示的条数
            int pageSize = 10;
            //查询出所有的数据
            //var list = depotInBLL.GetList(e => true);
            int listCount = depotOutBLL.GetCount(en => true);
            //设置为查询最后一页
            int page = listCount / pageSize - 1;
            var result = depotOutBLL.GetPageList(page, pageSize, en => true, en => en.Id).Select(
                        en => new
                        {
                            id = en.Id,
                            depotId = en.DepotId,
                            drugId = en.DrugId,
                            supId = en.SupId,
                            num = en.Num,
                            outDate = en.OutDate
                        }
                    );
            return new { items = result, page = page };
        }
        [HttpPost]
        public dynamic QueryDepotDrug(dynamic data)
        {
            //0，9没有查到是最后一页
            //页index
            int page = 0;
            //一页显示的数量
            int pageSize = 10;
            //页数
            //查所有的第一页
            var result = depotDrugBLL.GetPageList(page,pageSize,e=>true,e=>e.Id,e=>e.tb_DepotIn).Select(
                    e => new
                    {
                        id = e.Id,
                        depotInId = e.DepotInId,
                        num = e.Num
                    }
                );
            int pageCount = 0;
            //查数量
            int count = depotDrugBLL.GetCount(e=>true) ;
            if(data!=null)
            {
                int depotId = data.depotId;
                int drugId = data.drugId;
                
                if (depotId == -1 && drugId == -1)
                {
                    //初始化
                }
                if( depotId == -1 && drugId != -1 )
                {
                    result = depotDrugBLL.GetPageList(page,pageSize,e => e.DrugId == drugId,e=>e.Id,e=>e.tb_DepotIn).Select(
                            e => new
                            {
                                id = e.Id,
                                depotInId = e.DepotInId,
                                num = e.Num
                            }
                        );
                    count = depotDrugBLL.GetCount(e=>e.DrugId==drugId);
                }
                if (depotId != -1 && drugId == -1)
                {
                    result = depotDrugBLL.GetPageList(page,pageSize,e => e.DepotId == depotId,e=>e.Id,e=>e.tb_DepotIn).Select(
                            e => new
                            {
                                id = e.Id,
                                depotInId = e.DepotInId,
                                num = e.Num
                            }
                        );
                    count = depotDrugBLL.GetCount(e=>e.DepotId==depotId);
                }
                if (depotId != -1 && drugId != -1)
                {
                    result = depotDrugBLL.GetPageList(page,pageSize,e => e.DepotId == depotId && e.DrugId == drugId,e=>e.Id,e=>e.tb_DepotIn).Select(
                            e => new
                            {
                                id = e.Id,
                                depotInId = e.DepotInId,
                                num = e.Num
                            }
                        );
                    count = depotDrugBLL.GetCount(e=>e.DepotId==depotId&&e.DrugId==drugId);
                }
                pageCount=(int)Math.Ceiling((double)count / (double)pageSize);
                //如果pageCount是1就是只有一页
            }
            return new { items = result ,pageCount = pageCount};
        }
        [HttpPost]
        public dynamic QueryPrePage(dynamic data)
        {
            //是否第0页，是：1；否：0
            int flag=0;
            //页数
            int FirstIndex = 0;
            int pageSize=10;
            int index = data.index;
            //查所有的第一页
            var result = depotDrugBLL.GetPageList(0, pageSize, e => true, e => e.Id, e => e.tb_DepotIn).Select(
                    e => new
                    {
                        id = e.Id,
                        depotInId = e.DepotInId,
                        num = e.Num
                    }
                );
            //查数量
            int count = depotDrugBLL.GetCount(e=>true) ;
            if(data!=null)
            {
                int depotId = data.depotId;
                int drugId = data.drugId;
                
                if (depotId == -1 && drugId == -1)
                {
                    result = depotDrugBLL.GetPageList(index, pageSize, e => true, e => e.Id, e => e.tb_DepotIn).Select(
                        e => new
                        {
                            id = e.Id,
                            depotInId = e.DepotInId,
                            num = e.Num
                        }
                    );
                }
                if( depotId == -1 && drugId != -1 )
                {
                    result = depotDrugBLL.GetPageList(index,pageSize,e => e.DrugId == drugId,e=>e.Id,e=>e.tb_DepotIn).Select(
                            e => new
                            {
                                id = e.Id,
                                depotInId = e.DepotInId,
                                num = e.Num
                            }
                        );
                    count = depotDrugBLL.GetCount(e=>e.DrugId==drugId);
                }
                if (depotId != -1 && drugId == -1)
                {
                    result = depotDrugBLL.GetPageList(index,pageSize,e => e.DepotId == depotId,e=>e.Id,e=>e.tb_DepotIn).Select(
                            e => new
                            {
                                id = e.Id,
                                depotInId = e.DepotInId,
                                num = e.Num
                            }
                        );
                    count = depotDrugBLL.GetCount(e=>e.DepotId==depotId);
                }
                if (depotId != -1 && drugId != -1)
                {
                    result = depotDrugBLL.GetPageList(index,pageSize,e => e.DepotId == depotId && e.DrugId == drugId,e=>e.Id,e=>e.tb_DepotIn).Select(
                            e => new
                            {
                                id = e.Id,
                                depotInId = e.DepotInId,
                                num = e.Num
                            }
                        );
                    count = depotDrugBLL.GetCount(e=>e.DepotId==depotId&&e.DrugId==drugId);
                }
                if (index == FirstIndex) 
                {
                    flag = 1;
                }
            }
            return new { items = result, flag=flag };
        }
        [HttpPost]
        public dynamic QueryNextPage(dynamic data)
        {
            //是否最后一页，是：1；否：0
            int flag = 0;
            //页数
            int pageSize = 10;
            int index = data.index;
            //查所有的第一页
            var result = depotDrugBLL.GetPageList(0, pageSize, e => true, e => e.Id, e => e.tb_DepotIn).Select(
                    e => new
                    {
                        id = e.Id,
                        depotInId = e.DepotInId,
                        num = e.Num
                    }
                );
            //查数量
            int count = depotDrugBLL.GetCount(e => true);
            if (data != null)
            {
                int depotId = data.depotId;
                int drugId = data.drugId;

                if (depotId == -1 && drugId == -1)
                {
                    result = depotDrugBLL.GetPageList(index, pageSize, e => true, e => e.Id, e => e.tb_DepotIn).Select(
                        e => new
                        {
                            id = e.Id,
                            depotInId = e.DepotInId,
                            num = e.Num
                        }
                    );
                }
                if (depotId == -1 && drugId != -1)
                {
                    result = depotDrugBLL.GetPageList(index, pageSize, e => e.DrugId == drugId, e => e.Id, e => e.tb_DepotIn).Select(
                            e => new
                            {
                                id = e.Id,
                                depotInId = e.DepotInId,
                                num = e.Num
                            }
                        );
                    count = depotDrugBLL.GetCount(e => e.DrugId == drugId);
                }
                if (depotId != -1 && drugId == -1)
                {
                    result = depotDrugBLL.GetPageList(index, pageSize, e => e.DepotId == depotId, e => e.Id, e => e.tb_DepotIn).Select(
                            e => new
                            {
                                id = e.Id,
                                depotInId = e.DepotInId,
                                num = e.Num
                            }
                        );
                    count = depotDrugBLL.GetCount(e => e.DepotId == depotId);
                }
                if (depotId != -1 && drugId != -1)
                {
                    result = depotDrugBLL.GetPageList(index, pageSize, e => e.DepotId == depotId && e.DrugId == drugId, e => e.Id, e => e.tb_DepotIn).Select(
                            e => new
                            {
                                id = e.Id,
                                depotInId = e.DepotInId,
                                num = e.Num
                            }
                        );
                    count = depotDrugBLL.GetCount(e => e.DepotId == depotId && e.DrugId == drugId);
                }
                //假如4页，最大的index==3
                if (index == Math.Ceiling((double)count / (double)pageSize) - 1)
                {
                    flag = 1;
                }
            }
            return new { items = result, flag = flag };
        }
    }
}
