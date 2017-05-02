



function submitUpdate() {

	if (GetQueryString("ID")) {
		$("input[name='Action']").val("Update");
	} else {
		$("input[name='Action']").val("Insert");
	}

	$("#user").ajaxSubmit({
		type: 'post',
		url: '/User/Edit',
		success: function (data) {
			if (data.start == "success") {
				alert("保存成功");
				parent.hide_dig();
			} else {
				alert(data.msg);
			}
		},
		error: function (XmlHttpRequest, textStatus, errorThrown) {
			console.log(XmlHttpRequest);
			console.log(textStatus);
			console.log(errorThrown);
		}
	});
}

 