
function canvasDraw(e) {
    var c = document.getElementById("canvasPlace");
    var ctx = c.getContext("2d");
    console.log(e);
    ctx.beginPath();
    var bounds = c.getBoundingClientRect();
    ctx.moveTo(e[e.length - 1].x-bounds.left, e[e.length - 1].y-bounds.top);
    for (var i = 0; i < e.length; i++) {
        ctx.lineTo(e[i].x - bounds.left, e[i].y - bounds.top);
    }
    ctx.stroke();
}
 