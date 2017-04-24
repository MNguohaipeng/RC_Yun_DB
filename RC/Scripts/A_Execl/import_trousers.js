




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
			loadthead();
			show_dig();
			$('#tb_departments').bootstrapTable('load', data);
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

	$("thead").width($("#exli01").height());
}

//隐藏弹出框
function hide_dig() {

	$("#myModal").modal('hide');

}

//表单验证
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

function loadthead() {
	var oTable = new TableInit();
	oTable.Init();

}

var TableInit = function () {




	var oTableInit = new Object();
	//初始化Table
	oTableInit.Init = function () {
		$('#tb_departments').bootstrapTable({
			url: '#',         //请求后台的URL（*）
			method: 'post',                      //请求方式（*）
			toolbar: '#toolbar',                //工具按钮用哪个容器
			striped: true,                      //是否显示行间隔色
			cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
			pagination: false,                   //是否显示分页（*）
			sortable: true,                     //是否启用排序
			sortOrder: "asc",                   //排序方式
			queryParams: oTableInit.queryParams,//传递参数（*）
			sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
			pageNumber: 1,                       //初始化加载第一页，默认第一页
			pageSize: 10,                       //每页的记录行数（*）
			pageList: [10, 25, 50],        //可供选择的每页的行数（*）
			search: false,                       //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
			strictSearch: true,
			showColumns: true,                  //是否显示所有的列
			showRefresh: false,                  //是否显示刷新按钮
			minimumCountColumns: 2,             //最少允许的列数
			clickToSelect: true,                //是否启用点击选中行
			uniqueId: "ID",                     //每一行的唯一标识，一般为主键列
			showToggle: false,                    //是否显示详细视图和列表视图的切换按钮
			cardView: false,                    //是否显示详细视图
			detailView: false,                   //是否显示父子表
			height: document.body.clientHeight / 3,
			columns: [{
				checkbox: true
			}, {
				field: 'Code',
				title: '编码'
			}, {
				field: 'DZ_HipLength_CP',
				title: '单褶成品臀围',
				sortable: true
			}, {
				field: 'SZ_HipLength_CP',
				title: '双褶成品臀围',
				sortable: true
			}, {
				field: 'Crosspiece',
				title: '横档',
				sortable: true
			}, {
				field: 'LegWidth_UnderTheWaves',
				title: '腿肥 浪下10CM',
				sortable: true
			}, {
				field: 'FrontRise_EvenWaist',
				title: '前浪连腰',
				sortable: true
			}, {
				field: 'AfterTheWaves_EvenWaist',
				title: '后浪连腰',
				sortable: true
			}, {
				field: 'NetHip',
				title: '净臀围',
				sortable: true
			}, {
				field: 'CP_WaistWidth',
				title: '成品腰围',
				sortable: true
			}, {
				field: 'Height',
				title: '身高',
				sortable: true
			}, {
				field: 'LongPants',
				title: '裤长',
				sortable: true
			}, {
				field: 'NetWaist',
				title: '净腰围',
				sortable: true
			}]
		});
	};

	//得到查询的参数
	oTableInit.queryParams = function (params) {
		var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
			limit: params.limit,   //页面大小
			offset: params.offset  //页码

		};
		return temp;
	};
	return oTableInit;
};



