
$(function () {
    resetifarme();
})

 //设置ifarme  地址
function set_ifarme_Url(url ) {
    $("#PageContent").attr("src", url);
}

function set_ifarme_size(width,height) {
    $("#PageContent").attr("width", width)
    $("#PageContent").attr("height", height)
}

Table.loadTable = function (table, options) {
	$table = table;

	var rootPath = getRootPath();

	var scripts = [
            rootPath + '/js/bootstrap-table/bootstrap-table.js',
            rootPath + '/js/bootstrap-table/extensions/export/bootstrap-table-export.js',
            rootPath + '/js/bootstrap-table/extensions/export/bootstrap-table-export.js',
            rootPath + '/js/bootstrap-table/master/tableExport.js',
            rootPath + '/js/bootstrap-table/locale/bootstrap-table-zh-CN.min.js',
            rootPath + '/js/bootstrap-table/master/bootstrap-editable.js'
	],
        eachSeries = function (arr, iterator, callback) {
        	callback = callback || function () { };
        	if (!arr.length) {
        		return callback(options);
        	}
        	var completed = 0;
        	var iterate = function () {
        		iterator(arr[completed], function (err) {
        			if (err) {
        				callback(err);
        				callback = function () { };
        			}
        			else {
        				completed += 1;
        				if (completed >= arr.length) {
        					callback(options);
        				}
        				else {
        					iterate();
        				}
        			}
        		});
        	};
        	iterate();
        };

	eachSeries(scripts, Table.getScript, Table.initTable);
};


Table.reloadTable = function (table, options) {
	$table = table;
	$table.bootstrapTable('refresh', options);
};




//重置ifarme  高度
function resetifarme() {
    var width = (window.screen.availWidth / 8) * 6.60;
    var height = (window.screen.height / 10) * 8.0;
    parent.set_ifarme_size(width, height);
}


function GetQueryString(name) {
	var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
	var r = window.location.search.substr(1).match(reg);
	if (r != null) return unescape(r[2]); return null;
}