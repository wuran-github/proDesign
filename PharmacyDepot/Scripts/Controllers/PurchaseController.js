window.onload = function () {
    PurchaseController.Init();
}
PurchaseController = {
    total: 0,
    url: '../../api/data/getpurchaseplan',
    Init: function () {
        var owner = PurchaseController;
        owner.GetData = owner.GetPurchaseData;
        owner.GetData();
        $("#previous").click(owner.Previous);
        $("#next").click(owner.Next);
        $("#search").click(owner.Search);
        $("#jump").click(owner.Jump);
        $("#allcheckbox").click(owner.SelectCheckBox);
        $("#purchase").click(owner.BuildPurchaseDrug);
        $("#lip").addClass("disabled");
        $("#page").html("1");
        $(".table tbody").on('dblclick', "tr",  owner.CheckPlanDrug);
    },
    validation: function () {
        var owner = PurchaseController;
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
    GetPurchaseData: function () {
        var self = PurchaseController;
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
                    str += "<td data-pid='"+e.Id+"' >" + e.Id + "</td>";
                    str += "<td >" + e.CreateDate + "</td>";
                    str += "<td>" + e.CloseDate + "</td>";
                    str += "<td>" + e.Total + "</td>";
                    str += "<td>" + e.Finish + "</td>";
                    if(!e.Finish)
                    str += "<td><input type='checkbox' class='purcheckbox' name='pur' /></td>";
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
    Search: function () {
        var owner = PurchaseController;
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
        var owner = PurchaseController;
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
    CheckPlanDrug:function(){
        var self = $(this);
        var Id = self.children().eq(0).text();
        var url = "/stock/purchasedrug?Id=" + Id;
        window.open(url);

    },
    Next: function () {
        var self = PurchaseController;
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
        var self = PurchaseController;
        var page = self.SearchModel.page;
        self.SearchModel.page -= 1;
        self.GetData();
        self.validation();
    },
    SelectCheckBox:function(){
        var check = $("#allcheckbox").get(0).checked;
        $('.purcheckbox').prop("checked", check);
       
    },
    BuildPurchaseDrug:function(){
        var owner = PurchaseController;
        var trs = $('.purcheckbox:checked').parent().parent();
        if (trs.length === 0) {
            alert("请先选择要生成采购计划的仓库药品");
            return;
        }
        var data = new Array();
        $(trs).each(function (i, e) {
            var pid = $(e).children().eq(0).data('pid');
            data[i] = pid;
        });
        owner.PostPurchaseDrugData(data);
    },
    PostPurchaseDrugData:function(data){
        url = "../../api/data/PostBuildPurchaseDrug";
        $.ajax({
            type: 'post',
            url: url,
            data: JSON.stringify(data),
            contentType: 'application/json',
            success: function (d) {
                var str = "成功进行采购！";
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
        pageSize: 10
    },

}