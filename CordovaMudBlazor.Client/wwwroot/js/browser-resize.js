window.browserResize = {
    getInnerHeight: function () {
        return window.innerHeight;
    },
    getInnerWidth: function () {
        return window.innerWidth;
    },
    registerResizeCallback: function () {
        window.addEventListener("resize", browserResize.resized);
    },
    resized: function () {
        if (DotNet == null || DotNet == undefined || DotNet.invokeMethodAsync == null || DotNet.invokeMethodAsync == undefined) return;
        try {
            DotNet.invokeMethodAsync("CordovaMudBlazor.Library", 'OnBrowserResize').then(data => data);
        }
        catch (e) {}
    }
};

window.blazorHref = function (url) {
    if (DotNet == null || DotNet.invokeMethodAsync == null || DotNet.invokeMethodAsync == undefined) return;
    try {
        DotNet.invokeMethodAsync("CordovaMudBlazor.Library", 'BlazorHref', url);
    }
    catch (e) { }
}


window.addEventListener("resize", window.browserResize.resized);

window.locationhref = function (url) {
    var url2 = url.toLowerCase();
    if (url2.startsWith("file:///")) {
        url2 = url2.substring(8, url2.length - 1);
        if (url2.indexOf("/") < 0) {
            window.blazorHref("/" + url2);
            return true;
        }
    }
    window.location.href = url2;
    return true;
}