
$("input[name='file_name']").click(function () {
	$("input[name='up_file']").click();

})

$("input[name='up_file']").change(function () {
	$("input[name='file_name']").val($(this).val());
})

$(function () {




	UpdateSelect();
})




	//提交数据
	function analysis() {

		//if (!formvalidation()) {
		//	return;
		//}
		$("input[name='import']").val("false");
		$("#size_form").ajaxSubmit({
			type: 'post',
			url: '/OriginalData/embalmed',
			data: { "action": "N_XF_KZ_001" },
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



	var seleloadcount = 0;

	function UpdateSelect() {
		if (seleloadcount > 0) {
			return;
		}

		$.ajax({
			type: "post",
			url: "/Handler/Select.ashx",
			data: { "action": "ExcelType" },
			dataType: "json",
			success: function (data) {
		 
				$("select[name='excel_type'] option").remove();
				$("select[name='excel_type']").append("<option>--请选择文件类型--</option>");
 
				for (var i = 0; i < data.length; i++) {

					$("select[name='excel_type']").append("<option>" + data[i]["values"] + "</option>");
				}
				
				$("select[name='excel_type']").attr("onclick","")
			}, error: function (errMsg) {
				console.log(JSON.stringify(errMsg))
			}
		});
	}
