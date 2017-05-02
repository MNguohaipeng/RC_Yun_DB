
function set_ifarme_size(width, height) {
	$("#PageContent").attr("width", width)
	$("#PageContent").attr("height", height)
}


function edit(id) {

	$("#PageContent").attr("src", "Edit?ID=" + id);
 
	show_dig();


}

function insert() {

	$("#PageContent").attr("src", "Edit");

	show_dig();
 
}


 
function Delete(ID) {
 
	if (!ID) {
		alert("系统出错，无法删除。");
	}
	 

	if (!confirm("是否确认删除此用户，删除后无法恢复！")) {
		return;
	}
	 
	$.ajax({
		type: 'post',
		url: '/User/Delete',
		data: { "strID": ID },
		success: function (data) {
		
			if (data == "success") {
 hide_dig();
			} else {
				alert(data)
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
	location = location.href;
}

