

//提交数据
function analysis() {
 
	if (!formvalidation()) {
		return;
	}
	$("input[name='import']").val("false");
	$("#size_form").ajaxSubmit({
		type: 'post',
		url: '/Execl/import_trousers',
		success: function (data) {
			$(".headings th").remove();
			var json = data.rows;
			var th_html = '<th class="column-title">编码</th><th class="column-title">单褶成品臀围</th><th class="column-title">双褶成品臀围 </th><th class="column-title">横档</th><th class="column-title">腿肥 浪下10CM</th><th class="column-title">前浪连腰</th><th class="column-title">后浪连腰</th><th class="column-title">净臀围</th><th class="column-title">成品腰围</th><th class="column-title">身高</th><th class="column-title">裤长</th><th class="column-title">净腰围</th> '
			$(".headings").append(th_html);
			show_dig();
			$(".insert_excel_list").remove();
			for (var i = 0; i < json.length; i++) {
				var html = "<tr class='even pointer insert_excel_list' >"
				html += "<td>" + json[i]["Code"] + "</td>";
				html += "<td>" + json[i]["DZ_HipLength_CP"] + "</td>";
				html += "<td>" + json[i]["SZ_HipLength_CP"] + "</td>";
				html += "<td>" + json[i]["Crosspiece"] + "</td>";
				html += "<td>" + json[i]["LegWidth_UnderTheWaves"] + "</td>";
				html += "<td>" + json[i]["FrontRise_EvenWaist"] + "</td>";
				html += "<td>" + json[i]["AfterTheWaves_EvenWaist"] + "</td>";
				html += "<td>" + json[i]["NetHip"] + "</td>";
				html += "<td>" + json[i]["CP_WaistWidth"] + "</td>";
				html += "<td>" + json[i]["Height"] + "</td>";
				html += "<td>" + json[i]["LongPants"] + "</td>";
				html += "<td>" + json[i]["NetWaist"] + "</td>";
 
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
		url: '/Execl/import_trousers',
		success: function (data) {
			alert(JSON.stringify(data))

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