window.onload = function () {
    BuildPurchasePlanController.Init();
}
BuildPurchasePlanController = {
    total: 0,
    url: '../../api/data/getnotenoughstock',
    Init: function () {
        var owner = BuildPurchasePlanController;
        owner.GetData = owner.GetPurchaseData;
        owner.GetData();
        $("#previous").click(owner.Previous);
        $("#next").click(owner.Next);
        $("#search").click(owner.Search);
        $("#jump").click(owner.Jump);
        $("#allcheckbox").click(owner.SelectCheckBox);
        $("#purchase").click(owner.BuildPurchase);
        owner.GetDepot();
        owner.GetDrug();
        $("#lip").addClass("disabled");
        $("#page").html("1");
    },
    validation: function () {
        var owner = BuildPurchasePlanController;
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
        var self = BuildPurchasePlanController;
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
                    str += "<td data-depotid=" + e.depotId + ">" + e.depotName + "</td>";
                    str += "<td data-drugid=" + e.drugId + ">" + e.drugName + "</td>";
                    str += "<td>" + e.Num + "</td>";
                    str += "<td class='pnum'></td>";
                    str += "<td class='sup'></td>";
                    str += "<td><input type='checkbox' class='purcheckbox' name='pur' /></td>";
                    str += "</tr>";
                    $(".table tbody").append(str);
                });
                $("#total").html(total);
                if (total == 1) {
                    $("#lin").addClass("disabled");
                }
                $(".pnum").each(function (index, e) {
                    var text = "<input type='number' min='0' class='form-control' />"
                    $(e).append(text);
                });
                self.validation();
                self.GetSup();
            }
        })
    },
    OnlyInteger:function(){

    },
    Search: function () {
        var owner = BuildPurchasePlanController;
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
        var owner = BuildPurchasePlanController;
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
        var self = BuildPurchasePlanController;
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
        var self = BuildPurchasePlanController;
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
    GetSup:function(){
        var owner = BuildPurchasePlanController;
        $.ajax({
            type: 'GET',
            url: '../../api/data/getsup',
            success: function (d) {
                var result = d.dataItem;
                var count = d.count;
                var select="<select class='form-control'>";
                var op = "<option></option>";
                select += op;
                $(result).each(function (index, e) {
                    var str = "<option value='" + e.Id + "'>";
                    str += e.Name;
                    str += "</option>";
                    select+=str;
                });
                select += "</select>";
                $(".sup").each(function (index, e) {
                    $(e).append(select);
                });
            }
        })
    },
    Next: function () {
        var self = BuildPurchasePlanController;
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
        var self = BuildPurchasePlanController;
        var page = self.SearchModel.page;
        self.SearchModel.page -= 1;
        self.GetData();
        self.validation();
    },
    SelectCheckBox: function () {
        var check = $("#allcheckbox").get(0).checked;
        $('.purcheckbox').prop("checked", check);

    },
    BuildPurchase: function () {
        var owner = BuildPurchasePlanController;
        var trs = $('.purcheckbox:checked').parent().parent();
        if (trs.length === 0)
        {
            alert("请先选择要生成采购计划的仓库药品");
            return;
        }
        var data = {};
        data.purchase = [];

        $(trs).each(function (i, e) {
            var depotId = $(e).children().eq(0).data('depotid');
            var drugId = $(e).children().eq(1).data('drugid');
            var pnum = $(e).children().eq(3).children().eq(0).val();
            if (isNaN(pnum))
            {
                pnum = 0;
            }
            var sup = $(e).children().eq(4).children().eq(0).val();
            if (isNaN(sup))
            {
                sup = 0;
            }
            var Ids = { depotId: depotId, drugId: drugId, pnum: pnum, sup: sup };
            data.purchase[i] = Ids;
        });
        owner.PostPurchaseData(data);
    },
    PostPurchaseData: function (data) {
        url = "../../api/data/postbuildpurhaseplan";
        $.ajax({
            type: 'post',
            url: url,
            data: JSON.stringify(data),
            contentType: 'application/json',
            success: function (d) {
                var str = "成功增加采购计划！";
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
        pageSize: 20,
        drugId: null,
        depotId: null
    },

}