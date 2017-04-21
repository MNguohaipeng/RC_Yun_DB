
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



//重置ifarme  高度
function resetifarme() {
    var width = (window.screen.availWidth / 8) * 6.60;
    var height = (window.screen.height / 10) * 8.0;
    parent.set_ifarme_size(width, height);
}