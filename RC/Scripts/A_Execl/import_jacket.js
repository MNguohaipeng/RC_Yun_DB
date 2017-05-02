  

//提交数据
function analysis() {
 
    if (!formvalidation()) {
        return;
    }
    $("input[name='import']").val("false");
    $("#size_form").ajaxSubmit({
        type: 'post',
        url: '/Execl/import_jacket',
        success: function (data) {
        	console.log(JSON.stringify(data));
        	if (data.state == "error") {
        		alert(data.msg);
        		return;
        	}
            $(".headings th").remove();
            var json = data.msg;
            var th_html = '<th class="column-title">身高 </th><th class="column-title">前身长</th><th class="column-title">净胸围 </th><th class="column-title">成品胸围</th><th class="column-title">中腰</th><th class="column-title">成品下摆【不开叉】 </th><th class="column-title">成品下摆【开叉】 </th><th class="column-title">肩宽 </th><th class="column-title">袖长 </th>'
            $(".headings").append(th_html);
            show_dig();
            $(".insert_excel_list").remove();
            for (var i = 0; i < json.length; i++) {
                var html = "<tr class='even pointer insert_excel_list' >"
                html += "<td>" + json[i]["Height"] + "</td>";
                html += "<td>" + json[i]["FrontLength"] + "</td>";
                html += "<td>" + json[i]["NetBust"] + "</td>";
                html += "<td>" + json[i]["FinishedBust"] + "</td>";
                html += "<td>" + json[i]["InWaist"] + "</td>";
                html += "<td>" + json[i]["FinishedHem_NoFork"] + "</td>";
                html += "<td>" + json[i]["FinishedHem_SplitEnds"] + "</td>";
                html += "<td>" + json[i]["ShoulderWidth"] + "</td>";
                html += "<td>" + json[i]["Sleecve_Show"] + "</td>";
                html += "</tr>";
                $("#excel_data_list").append(html)
            }
             
        },
        error: function (XmlHttpRequest, textStatus, errorThrown) {
            console.log(XmlHttpRequest);
            console.log(textStatus);
            console.log(errorThrown);
        }
    });
}

//导入数据库
function _import() {
    $("input[name='import']").val("true");
    $("#size_form").ajaxSubmit({
        type: 'post',
        url: '/Execl/import_jacket',
        success: function (data) {
      
 
            if (data.state == "success") {
                alert("导入成功。");
                hide_dig();
            } else {
                alert("导入出错：" + data.msg);
                hide_dig();
            }
        },
        error: function (XmlHttpRequest, textStatus, errorThrown) {
            console.log(XmlHttpRequest);
            console.log(textStatus);
            console.log(errorThrown);
        }
    });
}

//显示弹出框
function show_dig() {

    $("#myModal").modal('show');
    $('#myModal').modal({
        keyboard: true
    })
}

//隐藏弹出框
function hide_dig() {

    $("#myModal").modal('hide');

}


function formvalidation() {
    if (!$("input[name='Size_Code']").val()) {
        alert("请输入尺码表编号。");
        return false;
    }

    if (!$("input[name='file_name']").val()) {
        alert("请输入上传尺码表。");
        return false;
    }
    return true;
}