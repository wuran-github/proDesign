window.onload = function () {
    OutstockModifyController.Init();
    OutstockController.Init();
}
OutstockController = {
    Init: function () {
        var owner = OutstockController;
        $("#Outstock_submit").on("click",owner.Outstock)
    },
    Outstock: function () {
        var tr = $("#Outstock_tb tbody tr");
        var data = {};
        data.result = [];
        tr.each(function (i, e) {
            var depotdrugId = $("input[name='depotdrugid']").eq(i).val();
            var num = $("input[name='num']").eq(i).val();
            var outDate = $("input[name='outdate']").eq(i).val();
            data.result[i] = {
                depotdrugId:depotdrugId,
                num: num,
                outDate: outDate,
            }
        })
        $.ajax({
            url: '../../api/depotdrug/outstock',
            type: 'POST',
            data: JSON.stringify(data),
            contentType: 'application/json',
            datatype: 'json',
            success: function (data) {
                alert("出库成功");
                window.location.href = "../../Outstock/Outstock";
            },
        })
    },
},
OutstockModifyController = {
    Init: function () {
        var owner = OutstockModifyController;
        owner.FirstDepotDrugData();

        $("#lab_firstpage_outstockModify").on("click", owner.FirstDepotDrugData);
        $("#but_nextpage_outstockModify").on("click", owner.NextDepotDrugData);
        $("#but_prepage_outstockModify").on("click", owner.PreDepotDrugData);
        $("#lab_lastpage_outstockModify").on("click", owner.LastDepotDrugData);
        $("#but_gopage_outstockModify").on("click", owner.GotoDeopotDrugData);

    },
    SetPreButEnable: function () {//设置“上一页”按钮可用
        $("#but_prepage_outstockModify").removeAttr("disabled");
        $("#but_prepage_outstockModify").css("background-color", "#f5f5f5");
        $("#but_prepage_outstockModify").css("border-color", "#f5f5f5");
    },
    SetNextButEnable: function () {//设置“下一页”按钮可用
        $("#but_nextpage_outstockModify").removeAttr("disabled");
        $("#but_nextpage_outstockModify").css("background-color", "#f5f5f5");
        $("#but_nextpage_outstockModify").css("border-color", "#f5f5f5");
    },
    SetPreButDisable: function () {//设置“上一页”按钮不可用
        $("#but_prepage_outstockModify").attr("disabled", "true");
        $("#but_prepage_outstockModify").css("background-color", "gray");
        $("#but_prepage_outstockModify").css("border-color", "gray");
    },
    SetNextButDisable: function () {//设置“下一页”按钮不可用
        $("#but_nextpage_outstockModify").attr("disabled", "true");
        $("#but_nextpage_outstockModify").css("background-color", "gray");
        $("#but_nextpage_outstockModify").css("border-color", "gray");
    },
    GotoDeopotDrugData: function () {//跳转第x页
        var owner = OutstockModifyController;
        //获取跳转的index
        var index = $("#text_gopage_outstockModify").val();
        var data = {};
        data = {
            index: index
        };
        $.ajax({
            url: '../../api/depotdrug/getoutstockdata',
            type: 'post',
            data: JSON.stringify(data),
            datatype: 'json',
            contentType: 'application/json',
            success: function (data) {
                var flag = parseInt(data.f);
                var page = parseInt(data.page);
                if (flag == 2) {//超出范围
                    alert("请输入合适范围的页码");
                }
                else if (flag == 4) {//最后一页
                    //最后一页的url不一样
                    //重新发送服务器请求
                    owner.LastDepotDrugData();
                }
                else if (flag == 3 || flag == 5) {//第一页或只有一页
                    owner.FirstData(data);
                }
                else {//普通页面
                    //设置“上一页”“下一页”按钮可用
                    owner.SetPreButEnable();
                    owner.SetNextButEnable();
                    //设置label当前页
                    $("#lab_nowpage_outstockModify").html(page);
                    //先清空原来的列表内容
                    $("#OutstockModify_tb tbody").children().remove();
                    var datas = data.items;
                    $(datas).each(function (index, element) {
                        var d1 = new Date(element.outDate);
                        var year1 = d1.getFullYear() + '年';
                        var month1 = d1.getMonth() + 1 + '月';
                        var day1 = d1.getDate() + '日';
                        var str = "<tr>";
                        str += "<td>" + element.id + "</td>";
                        str += "<td>" + element.depotId + "</td>";
                        str += "<td>" + element.drugId + "</td>";
                        str += "<td>" + element.supId + "</td>";
                        str += "<td>" + element.num + "</td>";
                        str += "<td>" + year1+month1+day1 + "</td>";
                        str += "<td><input name='OutstockModify_modify' type='button' value='修改' /></td>";
                        str += "</tr>";
                        $("#OutstockModify_tb tbody").append(str);
                    })
                }
            },
        })
    },
    FirstData: function (data) {
        var owner = OutstockModifyController;
        //返回的页为第一页，f==3
        var flag = parseInt(data.f);
        if ((flag == 3 || flag == 5) && data != null) {
            //设置“上一页”按钮不可用
            owner.SetPreButDisable();
            //清空原来的内容
            $("#OutstockModify_tb tbody").children().remove();
            var datas = data.items;
            //修改标志当前页的label
            $("#lab_nowpage_outstockModify").html("0");
            if (flag == 5) {//只有一页数据，设置下一页按钮不可用
                //设置“下一页”按钮不可用
                owner.SetNextButDisable();
            }
            else {//设置“下一页”按钮可用
                owner.SetNextButEnable();
            }
            $(datas).each(function (index, element) {
                var d1 = new Date(element.outDate);
                var year1 = d1.getFullYear() + '年';
                var month1 = d1.getMonth() + 1 + '月';
                var day1 = d1.getDate() + '日';
                var str = "<tr>";
                str += "<td>" + element.id + "</td>";
                str += "<td>" + element.depotId + "</td>";
                str += "<td>" + element.drugId + "</td>";
                str += "<td>" + element.supId + "</td>";
                str += "<td>" + element.num + "</td>";
                str += "<td>" + year1 + month1 + day1 + "</td>";
                str += "<td><input name='OutstockModify_modify' type='button' value='修改' /></td>";
                str += "</tr>";
                $("#OutstockModify_tb tbody").append(str);
            })
        }
        else {
            alert("请求页面出错！");
        }
    },
    FirstDepotDrugData: function () {//初始化第一页
        var owner = OutstockModifyController;
        //请求第一页的idx
        var idx = 0;
        var data = {};
        data = {
            index: idx,
        };
        $.ajax({
            url: '../../api/depotdrug/getoutstockdata',
            type: 'post',
            data: JSON.stringify(data),
            datatype: 'json',
            contentType: 'application/json',
            success: function (data) {
                owner.FirstData(data);
            },
        })
    },
    LastData: function (data) {
        var owner = OutstockModifyController;
        if (data != null) {
            //设置“下一页”按钮不可用
            owner.SetNextButDisable();
            //清空原来的内容
            $("#OutstockModify_tb tbody").children().remove();
            var datas = data.items;
            var page = parseInt(data.page);
            //修改标志当前页的label
            $("#lab_nowpage_outstockModify").html(page);
            //检查是否只有一页数据
            if (page == 0) {//只有一页数据，设置上一页按钮不可用
                owner.SetPreButDisable();
            }
            else {//上一页按钮可用
                owner.SetPreButEnable();
            }
            $(datas).each(function (index, element) {
                var d1 = new Date(element.outDate);
                var year1 = d1.getFullYear() + '年';
                var month1 = d1.getMonth() + 1 + '月';
                var day1 = d1.getDate() + '日';
                var str = "<tr>";
                str += "<td>" + element.id + "</td>";
                str += "<td>" + element.depotId + "</td>";
                str += "<td>" + element.drugId + "</td>";
                str += "<td>" + element.supId + "</td>";
                str += "<td>" + element.num + "</td>";
                str += "<td>" + year1 + month1 + day1 + "</td>";
                str += "<td><input name='OutstockModify_modify' type='button' value='修改' /></td>";
                str += "</tr>";
                $("#OutstockModify_tb tbody").append(str);
            })
        }
        else {
            alert("请求页面出错！");
        }
    },
    LastDepotDrugData: function () {
        var owner = OutstockModifyController;
        $.ajax({
            url: '../../api/depotdrug/getlastdepotout',
            type: 'get',
            success: function (data) {
                owner.LastData(data);
            },
        })
    },
    NextDepotDrugData: function () {//下一页按钮事件
        var owner = OutstockModifyController;
        //当前页idx
        var idx = $("#lab_nowpage_outstockModify").text();
        //下一页index
        var index = parseInt(idx) + 1;
        var data = {};
        data = {
            index: index,
        };
        $.ajax({
            url: '../../api/depotdrug/getoutstockdata',
            type: 'post',
            data: JSON.stringify(data),
            datatype: 'json',
            contentType: 'application/json',
            success: function (data) {
                //如果当前页为第0页，将“上一页”按钮设置为可用
                if (parseInt(idx) == 0) {
                    //设置“上一页”按钮可用
                    owner.SetPreButEnable();
                }
                //下一页检查是否是最后一页
                if (parseInt(data.f) == 4) {//设置“下一页”按钮不可用
                    owner.SetNextButDisable();
                }
                //设置标志当前页的label
                $("#lab_nowpage_outstockModify").html(index);
                //先清空原来的列表内容
                $("#OutstockModify_tb tbody").children().remove();
                var datas = data.items;
                $(datas).each(function (index, element) {
                    var d1 = new Date(element.outDate);
                    var year1 = d1.getFullYear() + '年';
                    var month1 = d1.getMonth() + 1 + '月';
                    var day1 = d1.getDate() + '日';
                    var str = "<tr>";
                    str += "<td>" + element.id + "</td>";
                    str += "<td>" + element.depotId + "</td>";
                    str += "<td>" + element.drugId + "</td>";
                    str += "<td>" + element.supId + "</td>";
                    str += "<td>" + element.num + "</td>";
                    str += "<td>" + year1 + month1 + day1 + "</td>";
                    str += "<td><input name='OutstockModify_modify' type='button' value='修改' /></td>";
                    str += "</tr>";
                    $("#OutstockModify_tb tbody").append(str);
                })
            }
        })
    },
    PreDepotDrugData: function () {//上一页按钮事件
        var owner = OutstockModifyController;
        //当前页idx
        var idx = $("#lab_nowpage_outstockModify").text();
        //上一页index
        var index = parseInt(idx) - 1;
        var data = {};
        data = {
            index: index,
        };
        $.ajax({
            url: '../../api/depotdrug/getoutstockdata',
            type: 'post',
            data: JSON.stringify(data),
            datatype: 'json',
            contentType: 'application/json',
            success: function (data) {
                //如果请求的上一页为第一页或者普通页面，将“下一页”按钮设置为可用
                if (parseInt(data.f) == 3 || parseInt(data.f) == 1) {
                    owner.SetNextButEnable();
                }
                //上一页检查是否是第0页
                if (parseInt(data.f) == 3) {//设置“上一页”按钮不可用
                    owner.SetPreButDisable();
                }
                //设置label当前页
                $("#lab_nowpage_outstockModify").html(index);
                //先清空原来的列表内容
                $("#OutstockModify_tb tbody").children().remove();
                var datas = data.items;
                $(datas).each(function (index, element) {
                    var d1 = new Date(element.outDate);
                    var year1 = d1.getFullYear() + '年';
                    var month1 = d1.getMonth() + 1 + '月';
                    var day1 = d1.getDate() + '日';
                    var str = "<tr>";
                    str += "<td>" + element.id + "</td>";
                    str += "<td>" + element.depotId + "</td>";
                    str += "<td>" + element.drugId + "</td>";
                    str += "<td>" + element.supId + "</td>";
                    str += "<td>" + element.num + "</td>";
                    str += "<td>" + year1 + month1 + day1 + "</td>";
                    str += "<td><input name='OutstockModify_modify' type='button' value='修改' /></td>";
                    str += "</tr>";
                    $("#OutstockModify_tb tbody").append(str);
                })
            }
        })
    },
}