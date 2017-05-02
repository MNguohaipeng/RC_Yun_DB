

function login() {
	if (!formvalidation())
	{
		return;
	}
	$("#login_form").ajaxSubmit({
		type: 'post',
		url: '/Home/Login',
		success: function (data) {
			if (data.Start == "success") {
				location = "index";

			} else {
				alert(data.Msg);
			}
	 
		},
		error: function (XmlHttpRequest, textStatus, errorThrown) {
			console.log(XmlHttpRequest);
			console.log(textStatus);
			console.log(errorThrown);
		}
	});
}


function formvalidation() {
	if (!$("input[name='username']").val()) {
		alert("请输入用户名。");
		return false;
	}

	if (!$("input[name='password']").val()) {
		alert("请输入密码。");
		return false;
	}


	return true;
}
 