
$(function () {
	//1.初始化Table


	$(".columns").append('<button class="btn btn-info" type="button" name="refresh" aria-label="refresh" title="Refresh">添加尺码</button>');
	$(".columns").append('<button class="btn btn-warning" type="button" name="refresh" aria-label="refresh" title="Refresh">修改尺码</button>');
	$(".columns").append('<button class="btn btn-danger" type="button" name="refresh" aria-label="refresh" title="Refresh">删除尺码</button>');

})

 
var TableInit = function () {
	var oTableInit = new Object();
	//初始化Table
	oTableInit.Init = function () {
		$('#tb_departments').bootstrapTable({
			url: '/SizeManage/GetCoatSize',         //请求后台的URL（*）
			method: 'post',                      //请求方式（*）
			toolbar: '#toolbar',                //工具按钮用哪个容器
			striped: true,                      //是否显示行间隔色
			cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
			pagination: true,                   //是否显示分页（*）
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
			height: document.body.clientHeight,
			columns: [{
				checkbox: true
			}, {
				field: 'Height',
				title: '身高'
			}, {
				field: 'FrontLength',
				title: '前身长'
			}, {
				field: 'NetBust',
				title: '净胸围'
			}, {
				field: 'FinishedBust',
				title: '成品胸围'
			}, {
				field: 'InWaist',
				title: '中腰'
			}, {
				field: 'FinishedHem_NoFork',
				title: '成品下摆【不开叉】'
			}, {
				field: 'FinishedHem_SplitEnds',
				title: '成品下摆【开叉】'
			}, {
				field: 'ShoulderWidth',
				title: '肩宽'
			}]
		});
	};

	//得到查询的参数
	oTableInit.queryParams = function (params) {
		var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
			limit: params.limit,   //页面大小
			offset: params.offset,  //页码
			size_code: $("select[name='size_code']").val(),

			statu: $("#txt_search_statu").val()
		};
		return temp;
	};
	return oTableInit;
};


//加载下拉框
function QueryCode() {


	$("#size_form").ajaxSubmit({
		type: 'post',
		url: '/SizeManage/Query_Code',
		dataType: "json",
		success: function (data) {
			$(".scclass").remove();
			for (var i = 0; i < data.msg.length; i++) {
				if (data.msg[i]) {
					$("#scselect").append("<option class='scclass' value='" + data.msg[i] + "'>" + data.msg[i] + "</option>");
				}
			}

		},
		error: function (XmlHttpRequest, textStatus, errorThrown) {
			console.log(XmlHttpRequest);
			console.log(textStatus);
			console.log(errorThrown);
		}
	});

}


//重新加载
function reLoad() {
	$("#tb_departments th").remove();
 
	var oTable = new TableInit();
	oTable.Init();

}