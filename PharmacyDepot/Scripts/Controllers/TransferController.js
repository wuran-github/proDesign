$(document).ready(function () {
    function setPreEnable() {
        $("input[name='prePage']").removeAttr("disabled");
        $("input[name='prePage']").css("background-color", "#f5f5f5");
        $("input[name='prePage']").css("border-color", "#f5f5f5");
    }
    function setPreDisable() {
        $("input[name='prePage']").attr("disabled", "true");
        $("input[name='prePage']").css("background-color", "gray");
        $("input[name='prePage']").css("border-color", "gray");
    }
    function setNextEnable() {
        $("input[name='nextPage']").removeAttr("disabled");
        $("input[name='nextPage']").css("background-color", "#f5f5f5");
        $("input[name='nextPage']").css("border-color", "#f5f5f5");
    }
    function setNextDisable() {
        $("input[name='nextPage']").attr("disabled", "true");
        $("input[name='nextPage']").css("background-color", "gray");
        $("input[name='nextPage']").css("border-color", "gray");
    }
    var depotId;
    var drugId;
    $("#confirmTransfer").click(function () {
        var outId = $("#inp_transoutId").val();
        var transNum = $("#inp_transNum").val();
        var inId = $("#inp_transinId").val();
        data = {
            outId: outId, transNum: transNum, inId: inId
        },
        $.ajax({
            url: '../../api/depotdrug/transferdepotdrug',
            type: 'post',
            data: JSON.stringify(data),
            datatype: 'json',
            contentType: 'application/json',
            success: function (data) {
                if (parseInt(data.f) == 0) {
                    alert("调拨失败！");
                }
                else {
                    alert("调拨成功！");
                    window.location.href = "../../Transfer/Transfer";
                }
            }
        })
    });
    $("input[name='but_query']").click(function () {
        //获取值
        depotId = $("#seldepotid").val();
        drugId = $("#seldrugid").val();
        
        //设置nowpage
        $("#nowpage").html(0);
        if (depotId == "") {
            depotId = -1;
        }
        if (drugId == "") {
            drugId = -1;
        }
        var data = {
            depotId: depotId,
            drugId: drugId,
        };
        $.ajax({
            url: '../../api/depotdrug/querydepotdrug',
            type: 'post',
            data: JSON.stringify(data),
            datatype: 'json',
            contentType: 'application/json',
            success: function (data) {
                //显示 表格
                $("#queryresult").removeAttr("hidden");
                //显示“上一页”“下一页”按钮
                $("input[name='prePage']").removeAttr("hidden");
                $("input[name='nextPage']").removeAttr("hidden");
                //获取页的信息对“上一页”“下一页”按钮控制
                //因为当前是第0页，“上一页”按钮不可用
                setPreDisable();
                //判断是否是最后一页
                if (parseInt(data.pageCount) == 1) {//是，“下一页”按钮不可用
                    setNextDisable();
                }
                else {
                    setNextEnable();
                }
                //清空
                $("#queryresult tbody").children().remove();
                $(data.items).each(function (index, element) {
                    var tr = "<tr>";
                    tr += "<td>" + element.id + "</td>"
                    tr += "<td>" + element.depotInId + "</td>"
                    tr += "<td>" + element.num + "</td>"
                    tr += "</tr>";
                    $("#queryresult tbody").append(tr);
                })
            }
        })
    });
    $("input[name='prePage']").click(function () {
        //获取当前页index
        var idx = $("#nowpage").text();
        //上一页index
        var index = parseInt(idx) - 1;
        var data = {
            depotId: depotId,
            drugId:drugId,
            index:index
        };
        $.ajax({
            url: '../../api/depotdrug/queryprepage',
            type: 'post',
            data: JSON.stringify(data),
            datatype: 'json',
            contentType: 'application/json',
            success: function (data) {
                //设置nowpage
                $("#nowpage").html(index);
                //获取页的信息对“上一页”“下一页”按钮控制
                //设置“下一页”可用
                setNextEnable();
                //判断是否是第0页
                if (parseInt(data.flag) == 1) {
                    setPreDisable();
                }
                else {
                    setPreEnable();
                }
                //清空
                $("#queryresult tbody").children().remove();
                $(data.items).each(function (index, element) {
                    var tr = "<tr>";
                    tr += "<td>" + element.id + "</td>"
                    tr += "<td>" + element.depotInId + "</td>"
                    tr += "<td>" + element.num + "</td>"
                    tr += "</tr>";
                    $("#queryresult tbody").append(tr);
                })
            }
        })
    })
    $("input[name='nextPage']").click(function () {
        //获取当前页index
        var idx = $("#nowpage").text();
        //下一页index
        var index = parseInt(idx) + 1;
        var data = {
            depotId: depotId,
            drugId: drugId,
            index: index
        };
        $.ajax({
            url: '../../api/depotdrug/querynextpage',
            type: 'post',
            data: JSON.stringify(data),
            contentType: 'application/json',
            success: function (data) {
                //设置nowpage
                $("#nowpage").html(index);
                //获取页的信息对“上一页”“下一页”按钮控制
                //设置“上一页”可用
                setPreEnable();
                //判断是否是最后一页
                if (parseInt(data.flag) == 1) {
                    setNextDisable();
                }
                else {
                    setNextEnable();
                }
                //清空
                $("#queryresult tbody").children().remove();
                $(data.items).each(function (index, element) {
                    var tr = "<tr>";
                    tr += "<td>" + element.id + "</td>"
                    tr += "<td>" + element.depotInId + "</td>"
                    tr += "<td>" + element.num + "</td>"
                    tr += "</tr>";
                    $("#queryresult tbody").append(tr);
                })
            }
        })
    })
})