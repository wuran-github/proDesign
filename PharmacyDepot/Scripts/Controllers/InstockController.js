window.onload = function ()
{
    DepotDrugmodifyController.Init();
    InstockController.Init();
    InstockModifyController.Init();
}
InstockController = {
    Init: function () {
        var owner = InstockController;
        $("#instock_submit").on("click", owner.AddDepotDrug);
    },
    AddDepotDrug: function () {
        var tr = $("#Instock_tb tbody tr");
        var data = {};
        data.result = [];
        tr.each(function (i, e) {
            var depotId = $("select[name='depotid']").eq(i).val();
            var drugId = $("select[name='drugid']").eq(i).val();
            var supId = $("select[name='supid']").eq(i).val();
            var num = $("input[name='num']").eq(i).val();
            var inDate = $("input[name='indate']").eq(i).val();
            var proDate = $("input[name='prodate']").eq(i).val();
            data.result[i] = {
                depotId: depotId,
                drugId: drugId,
                supId: supId,
                num: num,
                inDate: inDate,
                proDate: proDate
            }
        })
        $.ajax({
            url: '../../api/depotdrug/adddepotdrug',
            type: 'POST',
            data: JSON.stringify(data),
            contentType: 'application/json',
            datatype: 'json',
            success: function (data) {
                alert("添加成功");
                window.location.href = "../../Instock/Instock";
            },
        })
    },
}
InstockModifyController = {
    Init: function () {
        var owner = InstockModifyController;
        owner.FirstDepotDrugData();

        $("#lab_firstpage_instockModify").on("click", owner.FirstDepotDrugData);
        $("#but_nextpage_instockModify").on("click", owner.NextDepotDrugData);
        $("#but_prepage_instockModify").on("click", owner.PreDepotDrugData);
        $("#lab_lastpage_instockModify").on("click", owner.LastDepotDrugData);
        $("#but_gopage_instockModify").on("click", owner.GotoDeopotDrugData);

    },
    SetPreButEnable: function () {//设置“上一页”按钮可用
        $("#but_prepage_instockModify").removeAttr("disabled");
        $("#but_prepage_instockModify").css("background-color", "#f5f5f5");
        $("#but_prepage_instockModify").css("border-color", "#f5f5f5");
    },
    SetNextButEnable: function () {//设置“下一页”按钮可用
        $("#but_nextpage_instockModify").removeAttr("disabled");
        $("#but_nextpage_instockModify").css("background-color", "#f5f5f5");
        $("#but_nextpage_instockModify").css("border-color", "#f5f5f5");
    },
    SetPreButDisable: function () {//设置“上一页”按钮不可用
        $("#but_prepage_instockModify").attr("disabled", "true");
        $("#but_prepage_instockModify").css("background-color", "gray");
        $("#but_prepage_instockModify").css("border-color", "gray");
    },
    SetNextButDisable: function () {//设置“下一页”按钮不可用
        $("#but_nextpage_instockModify").attr("disabled", "true");
        $("#but_nextpage_instockModify").css("background-color", "gray");
        $("#but_nextpage_instockModify").css("border-color", "gray");
    },
    GotoDeopotDrugData: function () {//跳转第x页
        var owner = InstockModifyController;
        //获取跳转的index
        var index = $("#text_gopage_instockModify").val();
        var data = {};
        data = {
            index: index
        };
        $.ajax({
            url: '../../api/depotdrug/getinstockdata',
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
                    $("#lab_nowpage_instockModify").html(page);
                    //先清空原来的列表内容
                    $("#InstockModify_tb tbody").children().remove();
                    var datas = data.items;
                    $(datas).each(function (index, element) {
                        var d1 = new Date(element.inDate);
                        var year1 = d1.getFullYear()+'年';
                        var month1 = d1.getMonth()+1+'月';
                        var day1 = d1.getDate() + '日';
                        var d2 = new Date(element.proDate);
                        var year2 = d2.getFullYear() + '年';
                        var month2 = d2.getMonth() + 1 + '月';
                        var day2 = d2.getDate() + '日';
                        var str = "<tr>";
                        str += "<td>" + element.id + "</td>";
                        str += "<td>" + element.depotId + "</td>";
                        str += "<td>" + element.drugId + "</td>";
                        str += "<td>" + element.supId + "</td>";
                        str += "<td>" + element.num + "</td>";
                        str += "<td>" + year1+month1+day1 + "</td>";
                        str += "<td>" + year2+month2+day2 + "</td>";
                        str += "<td><input name='InstockModify_modify' type='button' value='修改' /></td>";
                        str += "</tr>";
                        $("#InstockModify_tb tbody").append(str);
                    })
                }
            },
        })
    },
    FirstData: function (data) {
        var owner = InstockModifyController;
        //返回的页为第一页，f==3
        var flag = parseInt(data.f);
        if ((flag == 3 || flag == 5) && data != null) {
            //设置“上一页”按钮不可用
            owner.SetPreButDisable();
            //清空原来的内容
            $("#InstockModify_tb tbody").children().remove();
            var datas = data.items;
            //修改标志当前页的label
            $("#lab_nowpage_instockModify").html("0");
            if (flag == 5) {//只有一页数据，设置下一页按钮不可用
                //设置“下一页”按钮不可用
                owner.SetNextButDisable();
            }
            else {//设置“下一页”按钮可用
                owner.SetNextButEnable();
            }
            $(datas).each(function (index, element) {
                var d1 = new Date(element.inDate);
                var year1 = d1.getFullYear() + '年';
                var month1 = d1.getMonth() + 1 + '月';
                var day1 = d1.getDate() + '日';
                var d2 = new Date(element.proDate);
                var year2 = d2.getFullYear() + '年';
                var month2 = d2.getMonth() + 1 + '月';
                var day2 = d2.getDate() + '日';
                
                var str = "<tr>";
                str += "<td>" + element.id + "</td>";
                str += "<td>" + element.depotId + "</td>";
                str += "<td>" + element.drugId + "</td>";
                str += "<td>" + element.supId + "</td>";
                str += "<td>" + element.num + "</td>";
                str += "<td>" + year1 + month1 + day1 + "</td>";
                str += "<td>" + year2 + month2 + day2 + "</td>";
                str += "<td><input name='InstockModify_modify' type='button' value='修改' /></td>";
                str += "</tr>";
                $("#InstockModify_tb tbody").append(str);
            })
        }
        else {
            alert("请求页面出错！");
        }
    },
    FirstDepotDrugData: function () {//初始化第一页
        var owner = InstockModifyController;
        //请求第一页的idx
        var idx = 0;
        var data = {};
        data = {
            index: idx,
        };
        $.ajax({
            url: '../../api/depotdrug/getinstockdata',
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
        var owner = InstockModifyController;
        if (data != null) {
            //设置“下一页”按钮不可用
            owner.SetNextButDisable();
            //清空原来的内容
            $("#InstockModify_tb tbody").children().remove();
            var datas = data.items;
            var page = parseInt(data.page);
            //修改标志当前页的label
            $("#lab_nowpage_instockModify").html(page);
            //检查是否只有一页数据
            if (page == 0) {//只有一页数据，设置上一页按钮不可用
                owner.SetPreButDisable();
            }
            else {//上一页按钮可用
                owner.SetPreButEnable();
            }
            $(datas).each(function (index, element) {
                var d1 = new Date(element.inDate);
                var year1 = d1.getFullYear() + '年';
                var month1 = d1.getMonth() + 1 + '月';
                var day1 = d1.getDate() + '日';
                var d2 = new Date(element.proDate);
                var year2 = d2.getFullYear() + '年';
                var month2 = d2.getMonth() + 1 + '月';
                var day2 = d2.getDate() + '日';
                var str = "<tr>";
                str += "<td>" + element.id + "</td>";
                str += "<td>" + element.depotId + "</td>";
                str += "<td>" + element.drugId + "</td>";
                str += "<td>" + element.supId + "</td>";
                str += "<td>" + element.num + "</td>";
                str += "<td>" + year1 + month1 + day1 + "</td>";
                str += "<td>" + year2 + month2 + day2 + "</td>";
                str += "<td><input name='InstockModify_modify' type='button' value='修改' /></td>";
                str += "</tr>";
                $("#InstockModify_tb tbody").append(str);
            })
        }
        else {
            alert("请求页面出错！");
        }
    },
    LastDepotDrugData: function () {
        var owner = InstockModifyController;
        $.ajax({
            url: '../../api/depotdrug/getlastdepotin',
            type: 'get',
            success: function (data) {
                owner.LastData(data);
            },
        })
    },
    NextDepotDrugData: function () {//下一页按钮事件
        var owner = InstockModifyController;
        //当前页idx
        var idx = $("#lab_nowpage_instockModify").text();
        //下一页index
        var index = parseInt(idx) + 1;
        var data = {};
        data = {
            index: index,
        };
        $.ajax({
            url: '../../api/depotdrug/getinstockdata',
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
                $("#lab_nowpage_instockModify").html(index);
                //先清空原来的列表内容
                $("#InstockModify_tb tbody").children().remove();
                var datas = data.items;
                $(datas).each(function (index, element) {
                    var d1 = new Date(element.inDate);
                    var year1 = d1.getFullYear() + '年';
                    var month1 = d1.getMonth() + 1 + '月';
                    var day1 = d1.getDate() + '日';
                    var d2 = new Date(element.proDate);
                    var year2 = d2.getFullYear() + '年';
                    var month2 = d2.getMonth() + 1 + '月';
                    var day2 = d2.getDate() + '日';
                    var str = "<tr>";
                    str += "<td>" + element.id + "</td>";
                    str += "<td>" + element.depotId + "</td>";
                    str += "<td>" + element.drugId + "</td>";
                    str += "<td>" + element.supId + "</td>";
                    str += "<td>" + element.num + "</td>";
                    str += "<td>" + year1 + month1 + day1 + "</td>";
                    str += "<td>" + year2 + month2 + day2 + "</td>";
                    str += "<td><input name='InstockModify_modify' type='button' value='修改' /></td>";
                    str += "</tr>";
                    $("#InstockModify_tb tbody").append(str);
                })
            }
        })
    },
    PreDepotDrugData: function () {//上一页按钮事件
        var owner = InstockModifyController;
        //当前页idx
        var idx = $("#lab_nowpage_instockModify").text();
        //上一页index
        var index = parseInt(idx) - 1;
        var data = {};
        data = {
            index: index,
        };
        $.ajax({
            url: '../../api/depotdrug/getinstockdata',
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
                $("#lab_nowpage_instockModify").html(index);
                //先清空原来的列表内容
                $("#InstockModify_tb tbody").children().remove();
                var datas = data.items;
                $(datas).each(function (index, element) {
                    var d1 = new Date(element.inDate);
                    var year1 = d1.getFullYear() + '年';
                    var month1 = d1.getMonth() + 1 + '月';
                    var day1 = d1.getDate() + '日';
                    var d2 = new Date(element.proDate);
                    var year2 = d2.getFullYear() + '年';
                    var month2 = d2.getMonth() + 1 + '月';
                    var day2 = d2.getDate() + '日';
                    var str = "<tr>";
                    str += "<td>" + element.id + "</td>";
                    str += "<td>" + element.depotId + "</td>";
                    str += "<td>" + element.drugId + "</td>";
                    str += "<td>" + element.supId + "</td>";
                    str += "<td>" + element.num + "</td>";
                    str += "<td>" + year1 + month1 + day1 + "</td>";
                    str += "<td>" + year2 + month2 + day2 + "</td>";
                    str += "<td><input name='InstockModify_modify' type='button' value='修改' /></td>";
                    str += "</tr>";
                    $("#InstockModify_tb tbody").append(str);
                })
            }
        })
    },
}
DepotDrugmodifyController = {
    Init:function(){
        var owner=DepotDrugmodifyController;
        owner.FirstDepotDrugData();
        
        $("#lab_firstpage").on("click",owner.FirstDepotDrugData);
        $("#but_nextpage").on("click", owner.NextDepotDrugData);
        $("#but_prepage").on("click", owner.PreDepotDrugData);
        $("#lab_lastpage").on("click", owner.LastDepotDrugData);
        $("#but_gopage").on("click", owner.GotoDeopotDrugData);
        
    },
    SetPreButEnable:function(){//设置“上一页”按钮可用
        $("#but_prepage").removeAttr("disabled");
        $("#but_prepage").css("background-color", "#f5f5f5");
        $("#but_prepage").css("border-color", "#f5f5f5");
    },
    SetNextButEnable: function () {//设置“下一页”按钮可用
        $("#but_nextpage").removeAttr("disabled");
        $("#but_nextpage").css("background-color", "#f5f5f5");
        $("#but_nextpage").css("border-color", "#f5f5f5");
    },
    SetPreButDisable: function () {//设置“上一页”按钮不可用
        $("#but_prepage").attr("disabled", "true");
        $("#but_prepage").css("background-color", "gray");
        $("#but_prepage").css("border-color", "gray");
    },
    SetNextButDisable: function () {//设置“下一页”按钮不可用
        $("#but_nextpage").attr("disabled", "true");
        $("#but_nextpage").css("background-color", "gray");
        $("#but_nextpage").css("border-color", "gray");
    },
    GotoDeopotDrugData: function () {//跳转第x页
        var owner = DepotDrugmodifyController;
        //获取跳转的index
        var index = $("#text_gopage").val();
        var data = {};
        data = {
            index:index
        };
        $.ajax({
            url: '../../api/depotdrug/getdepotdrugdata',
            type: 'post',
            data: JSON.stringify(data),
            datatype: 'json',
            contentType: 'application/json',
            success: function (data) {
                var flag = parseInt(data.f);
                var page = parseInt(data.page);
                if ( flag== 2){//超出范围
                    alert("请输入合适范围的页码");
                }
                else if ( flag == 4 ){//最后一页
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
                    $("#lab_nowpage").html(page);
                    //先清空原来的列表内容
                    $("#depotDrugModify_tb tbody").children().remove();
                    var datas = data.items;
                    $(datas).each(function (index, element) {
                        
                        var d2 = new Date(element.proDate);
                        var year2 = d2.getFullYear() + '年';
                        var month2 = d2.getMonth() + 1 + '月';
                        var day2 = d2.getDate() + '日';
                        var str = "<tr>";
                        str += "<td>" + element.id + "</td>"; 
                        str += "<td>" + element.depotinId + "</td>";
                        str += "<td>" + element.depotId + "</td>";
                        str += "<td>" + element.drugId + "</td>";
                        str += "<td>" + element.num + "</td>";
                        str += "<td>" + year2 + month2 + day2 + "</td>";
                        str += "<td><input name='instock_modify' type='button' value='修改' /></td>";
                        str += "</tr>";
                        $("#depotDrugModify_tb tbody").append(str);
                    })
                }
            },
        })
    },
    FirstData: function (data) {
        var owner = DepotDrugmodifyController;
        //返回的页为第一页，f==3
        var flag = parseInt(data.f);
        if ( (flag== 3||flag==5) && data != null){
            //设置“上一页”按钮不可用
            owner.SetPreButDisable();
            //清空原来的内容
            $("#depotDrugModify_tb tbody").children().remove();
            var datas = data.items;
            //修改标志当前页的label
            $("#lab_nowpage").html("0");
            if (flag == 5) {//只有一页数据，设置下一页按钮不可用
                //设置“下一页”按钮不可用
                owner.SetNextButDisable();
            }
            else{//设置“下一页”按钮可用
                owner.SetNextButEnable();
            }
            $(datas).each(function (index, element) {
                var d2 = new Date(element.proDate);
                var year2 = d2.getFullYear() + '年';
                var month2 = d2.getMonth() + 1 + '月';
                var day2 = d2.getDate() + '日';
                var str = "<tr>";
                str += "<td>" + element.id + "</td>";
                str += "<td>" + element.depotinId + "</td>";
                str += "<td>" + element.depotId + "</td>";
                str += "<td>" + element.drugId + "</td>";
                str += "<td>" + element.num + "</td>";
                str += "<td>" + year2 + month2 + day2 + "</td>";
                str += "<td><input name='instock_modify' type='button' value='修改' /></td>";
                str += "</tr>";
                $("#depotDrugModify_tb tbody").append(str);
            })
        }
        else{
            alert("请求页面出错！");
        }
    },
    FirstDepotDrugData:function(){//初始化第一页
        var owner=DepotDrugmodifyController;
        //请求第一页的idx
        var idx = 0 ;
        var data = {};
        data = {
            index: idx,
        };
        $.ajax({
            url: '../../api/depotdrug/getdepotdrugdata',
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
        var owner = DepotDrugmodifyController;
        if( data != null ){
            //设置“下一页”按钮不可用
            owner.SetNextButDisable();
            //清空原来的内容
            $("#depotDrugModify_tb tbody").children().remove();
            var datas = data.items;
            var page = parseInt(data.page);
            //修改标志当前页的label
            $("#lab_nowpage").html(page);
            //检查是否只有一页数据
            if (page == 0) {//只有一页数据，设置上一页按钮不可用
                owner.SetPreButDisable();
            }
            else{//上一页按钮可用
                owner.SetPreButEnable();
            }
            $(datas).each(function (index, element) {
                var d2 = new Date(element.proDate);
                var year2 = d2.getFullYear() + '年';
                var month2 = d2.getMonth() + 1 + '月';
                var day2 = d2.getDate() + '日';
                var str = "<tr>";
                str += "<td>" + element.id + "</td>";
                str += "<td>" + element.depotinId + "</td>";
                str += "<td>" + element.depotId + "</td>";
                str += "<td>" + element.drugId + "</td>";
                str += "<td>" + element.num + "</td>";
                str += "<td>" + year2 + month2 + day2 + "</td>";
                str += "<td><input name='instock_modify' type='button' value='修改' /></td>";
                str += "</tr>";
                $("#depotDrugModify_tb tbody").append(str);
            })
        }
        else {
            alert("请求页面出错！");
        }
    },
    LastDepotDrugData:function(){
        var owner=DepotDrugmodifyController;
        $.ajax({
            url: '../../api/depotdrug/getlastdepotdrug',
            type: 'get',
            success: function (data){
                owner.LastData(data);
            },
        })
    },
    NextDepotDrugData: function () {//下一页按钮事件
        var owner = DepotDrugmodifyController;
        //当前页idx
        var idx = $("#lab_nowpage").text();
        //下一页index
        var index = parseInt(idx) + 1;
        var data = {};
        data = {
            index: index,
        };
        $.ajax({
            url: '../../api/depotdrug/getdepotdrugdata',
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
                if (parseInt(data.f) == 4){//设置“下一页”按钮不可用
                    owner.SetNextButDisable();
                }
                //设置标志当前页的label
                $("#lab_nowpage").html(index);
                //先清空原来的列表内容
                $("#depotDrugModify_tb tbody").children().remove();
                var datas = data.items;
                $(datas).each(function (index, element) {
                    var d2 = new Date(element.proDate);
                    var year2 = d2.getFullYear() + '年';
                    var month2 = d2.getMonth() + 1 + '月';
                    var day2 = d2.getDate() + '日';
                    var str = "<tr>";
                    str += "<td>" + element.id + "</td>";
                    str += "<td>" + element.depotinId + "</td>";
                    str += "<td>" + element.depotId + "</td>";
                    str += "<td>" + element.drugId + "</td>";
                    str += "<td>" + element.num + "</td>";
                    str += "<td>" + year2 + month2 + day2 + "</td>";
                    str += "<td><input name='instock_modify' type='button' value='修改' /></td>";
                    str += "</tr>";
                    $("#depotDrugModify_tb tbody").append(str);
                })
            }
        })
    },
    PreDepotDrugData: function () {//上一页按钮事件
        var owner = DepotDrugmodifyController;
        //当前页idx
        var idx = $("#lab_nowpage").text();
        //上一页index
        var index = parseInt(idx) - 1;
        var data = {};
        data = {
            index: index,
        };
        $.ajax({
            url: '../../api/depotdrug/getdepotdrugdata',
            type: 'post',
            data: JSON.stringify(data),
            datatype: 'json',
            contentType: 'application/json',
            success: function (data) {
                //如果请求的上一页为第一页或者普通页面，将“下一页”按钮设置为可用
                if ( parseInt(data.f) == 3|| parseInt(data.f) ==1 ) {
                    owner.SetNextButEnable();
                }
                //上一页检查是否是第0页
                if (parseInt(data.f) == 3) {//设置“上一页”按钮不可用
                    owner.SetPreButDisable();
                }
                //设置label当前页
                $("#lab_nowpage").html(index);
                //先清空原来的列表内容
                $("#depotDrugModify_tb tbody").children().remove();
                var datas = data.items;
                $(datas).each(function (index, element) {
                    var d2 = new Date(element.proDate);
                    var year2 = d2.getFullYear() + '年';
                    var month2 = d2.getMonth() + 1 + '月';
                    var day2 = d2.getDate() + '日';
                    var str = "<tr>";
                    str += "<td>" + element.id + "</td>";
                    str += "<td>" + element.depotinId + "</td>";
                    str += "<td>" + element.depotId + "</td>";
                    str += "<td>" + element.drugId + "</td>";
                    str += "<td>" + element.num + "</td>";
                    str += "<td>" + year2 + month2 + day2 + "</td>";
                    str += "<td><input name='instock_modify' type='button' value='修改' /></td>";
                    str += "</tr>";
                    $("#depotDrugModify_tb tbody").append(str);
                })
            }
        })
    },
  
}
