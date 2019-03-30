 
 window.onload = function () {
     PurchaseDrugController.Init();
 }
 PurchaseDrugController = {
     total: 0,
     url: '../../api/data/getpurchasedrug',
     Init: function () {
         var owner = PurchaseDrugController;
         owner.GetData = owner.GetPurchaseDrugData;
         owner.GetData();
         $("#previous").click(owner.Previous);
         $("#next").click(owner.Next);
         $("#search").click(owner.Search);
         $("#jump").click(owner.Jump);
         $("#allcheckbox").click(owner.SelectCheckBox);
         $(".table tbody").on("click", '.updatedrug', owner.ModifyDrug);
         owner.GetDepot();
         owner.GetDrug();
         $("#lip").addClass("disabled");
         $("#page").html("1");
     },
     GetUrlParms: function (parmname) {
         parmname.toLowerCase
         var reg = new RegExp("(^|&)" + parmname.toLowerCase() + "=([^&]*)(&|$)");
         var r = window.location.search.toLowerCase().substr(1).match(reg);
         if (r != null) return r[2]; return null;
     },
     validation: function () {
         var owner = PurchaseDrugController;
         var page = owner.SearchModel.page + 1;
         if (page <= 1) {
             $("#lip").addClass("disabled");
         }
         else {
             $("#lip").removeClass("disabled");
         }
         if (page >= owner.total) {
             $("#lin").addClass("disabled");
         }
         else {
             $("#lin").removeClass("disabled");
         }
         $("#page").html(page);
     },
     GetPurchaseDrugData: function () {
         var self = PurchaseDrugController;
         var Id = self.GetUrlParms("Id");
         self.SearchModel.PId = Id===null?0:Id;
         $(".table tr:not(:first)").remove();
         $.ajax({
             type: 'post',
             url: self.url,
             data: JSON.stringify(self.SearchModel),
             contentType: 'application/json',
             success: function (d) {
                 var result = d.dataItem;
                 var count = d.count;
                 var total = Math.ceil(count / (self.SearchModel.pageSize + 0.0));
                 self.total = total;
                 $(result).each(function (index, e) {

                     var str = "<tr>";
                     str+="<td data-pid=" + e.pId + ">"+e.pId+"</td>";
                     str += "<td data-depotid=" + e.depotId + ">" + e.depotName + "</td>";
                     str += "<td data-drugid=" + e.drugId + ">" + e.drugName + "</td>";
                     str += "<td>" + e.supName + "</td>";
                     str += "<td class='pnum'>" + "<input type='number' min='0' class='form-control' value='" + e.num + "' /></td>";
                     str += "<td><input type='button' value='修改' class='btn btn-default updatedrug' /></td>";
                     str += "</tr>";
                     $(".table tbody").append(str);
                 });
                 $("#total").html(total);
                 if (total == 1) {
                     $("#lin").addClass("disabled");
                 }
                 self.validation();
             }
         })
     },
     OnlyInteger:function(){

     },
     Search: function () {
         var owner = PurchaseDrugController;
         var drug = $("#drugselect").val();
         var depot = $("#depotselect").val();
         if (drug && !isNaN(drug)) {

             owner.SearchModel.drugId = drug;
         }
         else {
             owner.SearchModel.drugId = null;
         }
         if (depot && !isNaN(depot)) {
             owner.SearchModel.depotId = depot;
         }
         else {
             owner.SearchModel.depotId = null;

         }
         owner.SearchModel.page = 0;
         owner.GetData();
         owner.validation();
     },
     Jump: function () {
         var owner = PurchaseDrugController;
         var pageindex = $("#pageIndex").val();
         if (!isNaN(pageindex)) {
             if (pageindex > owner.total) {
                 alert("页码不正确!");
             }
             else {
                 owner.SearchModel.page = pageindex - 1;
                 owner.GetData();
                 owner.validation();
             }
         }
         else {
             alert("输入不正确");
         }
     },
     GetDepot: function () {
         var self = PurchaseDrugController;
         $.ajax({
             type: 'GET',
             url: '../../api/data/getdepot',
             success: function (d) {
                 var result = d.dataItem;
                 var count = d.count;
                 var op = "<option></option>";
                 $("#depotselect").append(op);
                 $(result).each(function (index, e) {
                     var str = "<option value='" + e.Id + "'>";
                     str += e.Name;
                     str += "</option>";
                     $("#depotselect").append(str);
                 });
             }
         })
     },
     GetDrug: function () {
         var self = PurchaseDrugController;
         $.ajax({
             type: 'GET',
             url: '../../api/data/getdrug',
             success: function (d) {
                 var result = d.dataItem;
                 var count = d.count;
                 var op = "<option></option>";
                 $("#drugselect").append(op);
                 $(result).each(function (index, e) {
                     var str = "<option value='" + e.Id + "'>";
                     str += e.Name;
                     str += "</option>";
                     $("#drugselect").append(str);
                 });
             }
         })
     },
     Next: function () {
         var self = PurchaseDrugController;
         if ($("#lin").hasClass("disabled")) {
             return;
         }
         var page = self.SearchModel.page + 2;
         self.SearchModel.page += 1;
         $(".table tr:not(:first)").remove();
         self.GetData();
         self.validation();

     },
     Previous: function () {
         if ($("#lip").hasClass("disabled")) {
             return;
         }
         var self = PurchaseDrugController;
         var page = self.SearchModel.page;
         self.SearchModel.page -= 1;
         self.GetData();
         self.validation();
     },
     SelectCheckBox: function () {
         var check = $("#allcheckbox").get(0).checked;
         $('.purcheckbox').prop("checked", check);

     },
     ModifyDrug:function(){
         var self = this;
         var td = $(self).parent().parent();
         var data={};
         data.pid = td.children().eq(0).data("pid");
         data.depotid = td.children().eq(1).data("depotid");
         data.drugid = td.children().eq(2).data("drugid");
         data.num = td.children().eq(4).children().eq(0).val();

         url = "../../api/data/PostModifyPurchaseDrug";
         $.ajax({
             type: 'post',
             url: url,
             data: JSON.stringify(data),
             contentType: 'application/json',
             success: function (d) {
                 var str = "成功修改采购单！";
                 alert(str);
                 window.location.reload();
             },
             error: function (xhr) {
                 var str = "错误信息：" + xhr.status + " " + xhr.statusText;
                 alert(str);
             }
         });
     },
     GetData: null,
     SearchModel: {
         page: 0,
         pageSize: 10,
         PId:1,
         drugId: null,
         depotId: null
     },

 }