const pStart = { x: 0, y: 0 };
const pCurrent = { x: 0, y: 0 };

let isLoading = false;

function IsLoading() {
    return isLoading;
}

window.browserRefresh = function () {
    if (DotNet == null || DotNet.invokeMethodAsync == null || DotNet.invokeMethodAsync == undefined) return;
    try {
        DotNet.invokeMethodAsync("CordovaMudBlazor.Library", 'BrowserRefresh');
    }
    catch (e) {}
}

function SetLoading(b) {
    isLoading = b;

    ids2 = document.getElementsByClassName("loading-container");
    var id2 = null;
    if (ids2 != null && ids2.length > 0) id2 = ids2[0];

    if (isLoading && id2 != null) id2.style.display = "";
    else if (id2 != null) id2.style.display = "none";

    return isLoading;
}

function loading() {
    SetLoading(true);
    window.browserRefresh();
    setTimeout(() => {
        SetLoading(false);
    }, 3000);
}

function swipeStart(e) {
    if (typeof e["targetTouches"] !== "undefined") {
        let touch = e.targetTouches[0];
        pStart.x = touch.screenX;
        pStart.y = touch.screenY;
    } else {
        pStart.x = e.screenX;
        pStart.y = e.screenY;
    }
    if (pStart.x === undefined || pStart.y === undefined) { //mouse event
        pStart.x = e.clientX;
        pStart.t = e.clientY;
    }
}

function swipeEnd(e) {
    if (document.body.scrollTop === 0 && !isLoading) {
    }
}

function swipe(e) {
    if (typeof e["changedTouches"] !== "undefined") {
        let touch = e.changedTouches[0];
        pCurrent.x = touch.screenX;
        pCurrent.y = touch.screenY;
    } else {
        pCurrent.x = e.screenX;
        pCurrent.y = e.screenY;
    }
    if (pCurrent.x === undefined || pCurrent.y === undefined) { //mouse event
        pCurrent.x = e.clientX;
        pCurrent.y = e.clientY;
    }
    if (pStart.y != 0) {
        let changeY = pStart.y < pCurrent.y ? Math.abs(pStart.y - pCurrent.y) : 0;
        const rotation = changeY < 100 ? changeY * 30 / 100 : 30;

        ids2 = document.getElementsByClassName("loading-container");
        var id2 = null;
        if (ids2 != null && ids2.length > 0) id2 = ids2[0];

        var st = 0;
        if (id2 != null) st = id2.scrollTop;

        var scroll_top = Math.max(st, document.body.scrollTop);
        if (scroll_top == 0) {
            if (changeY > 100) loading(); 
        }
    }
}
