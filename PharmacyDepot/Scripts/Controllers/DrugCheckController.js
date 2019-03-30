window.onload = function () {
    DrugCheckController.Init();
}
DrugCheckController = {
    total: 0,
    url: '../../api/data/getdrugcheckdata',
    Init: function () {
        var owner = DrugCheckController;
        owner.GetData = owner.GetDrugCheckData;
        owner.GetData();
        owner.DatePickerInit();
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
    DatePickerInit:function(){
        $("#earlyMore").datetimepicker({
            minView: "month", //选择日期后，不会再跳转去选择时分秒 
            language: 'zh-CN',
            format: 'yyyy-mm-dd',
            todayBtn: 1,
            autoclose: 1,
        });
        $("#earlyLess").datetimepicker({
            minView: "month", //选择日期后，不会再跳转去选择时分秒 
            language: 'zh-CN',
            format: 'yyyy-mm-dd',
            todayBtn: 1,
            autoclose: 1,
        });
        $("#lateMore").datetimepicker({
            minView: "month", //选择日期后，不会再跳转去选择时分秒 
            language: 'zh-CN',
            format: 'yyyy-mm-dd',
            todayBtn: 1,
            autoclose: 1,
        });
        $("#lateLess").datetimepicker({
            minView: "month", //选择日期后，不会再跳转去选择时分秒 
            language: 'zh-CN',
            format: 'yyyy-mm-dd',
            todayBtn: 1,
            autoclose: 1,
        });
    },
    validation: function () {
        var owner = DrugCheckController;
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
    GetDrugCheckData: function () {
        var self = DrugCheckController;
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
                    str += "<td>" + e.depotName + "</td>";
                    str += "<td>" + e.drugName + "</td>";
                    str += "<td>" + e.num + "</td>";
                    str += "<td>" + e.batchs + "</td>";
                    str += "<td>" + (new Date(e.earlyDate)).toLocaleDateString() + "</td>";
                    str += "<td>" + (new Date(e.lateDate)).toLocaleDateString() + "</td>";
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
        var owner = DrugCheckController;
        var drug = $("#drugselect").val();
        var drugVague = $("#drugVague").val();
        var depotVague = $("#depotVague").val();
        var depot = $("#depotselect").val();
        var stockMore = $("#stockMore").val();
        var stockLess = $("#stockLess").val();
        var earlyMore = $("#earlyMore").val();
        var earlyLess = $("#earlyLess").val();
        var lateMore = $("#lateMore").val();
        var lateLess = $("#lateLess").val();
        if (drugVague)
        {
            owner.SearchModel.drugVague = drugVague;
        } else
        {
            owner.SearchModel.drugVague = null;
        }
        if (depotVague)
        {
            owner.SearchModel.depotVague = depotVague;

        } else {
            owner.SearchModel.depotVague = null;

        }
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
        if (stockMore)
        {
            owner.SearchModel.stockMore = stockMore;
        }
        else
        {
            owner.SearchModel.stockMore = null;
        }
        
        if (stockLess) {
            owner.SearchModel.stockLess = stockLess;
        }
        else {
            owner.SearchModel.stockLess = null;
        }
        if (owner.IsDate(earlyMore)) {
            owner.SearchModel.earlyMore =new Date(earlyMore);
        } else{
            owner.SearchModel.earlyMore = null;
        }
        if (owner.IsDate(earlyLess)) {
            owner.SearchModel.earlyLess = new Date(earlyLess);
        } else {
            owner.SearchModel.earlyLess = null;
        }
        if (owner.IsDate(lateMore)) {
            owner.SearchModel.lateMore = new Date(lateMore);
        } else {
            owner.SearchModel.lateMore = null;
        }
        if (owner.IsDate(lateLess)) {
            owner.SearchModel.lateLess = new Date(lateLess);
        } else {
            owner.SearchModel.lateLess = null;
        }
        owner.SearchModel.page = 0;
        owner.GetData();
        owner.validation();
    },
    IsDate:function(val){
        var date = new Date(val);
        return (date.getDate() == val.substring(val.length - 2));
    },
    Jump: function () {
        var owner = DrugCheckController;
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
        var self = DrugCheckController;
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
        var self = DrugCheckController;
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
        var self = DrugCheckController;
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
        var self = DrugCheckController;
        var page = self.SearchModel.page;
        self.SearchModel.page -= 1;
        self.GetData();
        self.validation();
    },
    DownFlie:function(){
        var self = DrugCheckController;
        $.ajax({
            type: 'post',
            url: '../../api/data/downdrugcheck',
            data: JSON.stringify(self.SearchModel),
            contentType: 'application/json',
            success: function (d) {
                if(d){
                    location.href = "../../drugcheck/DownList";
                }
            }});
                
            
    },
    GetData: null,
    SearchModel: {
        page: 0,
        pageSize: 10,
        drugId: null,
        depotId: null,
        drugVague: null,
        depotVague:null,
        stockMore: null,
        stockLess: null,
        earlyMore: null,
        earlyLess: null,
        lateMore: null,
        lateLess:null
    },

}