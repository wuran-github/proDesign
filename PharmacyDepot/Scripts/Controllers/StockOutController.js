window.onload = function () {
    StockOutController.Init();
}
StockOutController = {
    total: 0,
    url: '../../api/data/PostStockOut',
    Init: function () {
        var owner = StockOutController;
        owner.GetData = owner.GetStockData;
        owner.GetData();
        $("#previous").click(owner.Previous);
        $("#next").click(owner.Next);
        $("#search").click(owner.Search);
        $("#jump").click(owner.Jump);
        $("#excel").click(owner.DownFlie);
        owner.GetDepot();
        owner.GetDrug();
        $("#lip").addClass("disabled");
        $("#page").html("1");
    },
    validation: function () {
        var owner = StockOutController;
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
    GetStockData: function () {
        var self = StockOutController;
        $(".table tr:not(:first)").remove();
        $.ajax({
            type: 'POST',
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
                    str += "<td>" + e.depotName + "</td>";
                    str += "<td>" + e.drugName + "</td>";
                    str += "<td>" + e.outNum + "</td>";
                    str += "<td>" + e.date + "</td>";
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
    DownFlie: function () {
        var self = StockOutController;
        $.ajax({
            type: 'post',
            url: '../../api/data/DownStockOut',
            data: JSON.stringify(self.SearchModel),
            contentType: 'application/json',
            success: function (d) {
                if (d) {
                    location.href = "../../stock/DownStockOutList";
                }
            }
        });


    },
    Search: function () {
        var owner = StockOutController;
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
        var owner = StockOutController;
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
        var self = StockOutController;
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
        var self = StockOutController;
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
        var self = StockOutController;
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
        var self = StockOutController;
        var page = self.SearchModel.page;
        self.SearchModel.page -= 1;
        self.GetData();
        self.validation();
    },
    GetData: null,
    SearchModel: {
        page: 0,
        pageSize: 10,
        drugId: null,
        depotId: null
    },

}