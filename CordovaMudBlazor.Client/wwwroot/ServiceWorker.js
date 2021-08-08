const baseURL = '/';
const indexURL = '/index.html';
const networkFetchEvent = 'fetch';
const swInstallEvent = 'install';
const swInstalledEvent = 'installed';
const swActivateEvent = 'activate';
const staticCachePrefix = 'blazor-cache-v1';
const staticCacheName = 'blazor-cordovamudblazor-v1';
const requiredFiles = [
"/_framework/blazor.boot.json",
"/_framework/blazor.webassembly.js",
"/_framework/dotnet.5.0.8.js",
"/_framework/dotnet.wasm",
"/_framework/Microsoft.AspNetCore.Components.dll",
"/_framework/Microsoft.AspNetCore.Components.Forms.dll",
"/_framework/Microsoft.AspNetCore.Components.Web.dll",
"/_framework/Microsoft.AspNetCore.Components.WebAssembly.dll",
"/_framework/Microsoft.CSharp.dll",
"/_framework/Microsoft.Extensions.Configuration.Abstractions.dll",
"/_framework/Microsoft.Extensions.Configuration.dll",
"/_framework/Microsoft.Extensions.Configuration.Json.dll",
"/_framework/Microsoft.Extensions.DependencyInjection.Abstractions.dll",
"/_framework/Microsoft.Extensions.DependencyInjection.dll",
"/_framework/Microsoft.Extensions.Logging.Abstractions.dll",
"/_framework/Microsoft.Extensions.Logging.dll",
"/_framework/Microsoft.Extensions.Options.dll",
"/_framework/Microsoft.Extensions.Primitives.dll",
"/_framework/Microsoft.JSInterop.dll",
"/_framework/Microsoft.JSInterop.WebAssembly.dll",
"/_framework/MudBlazor.dll",
"/_framework/CordovaMudBlazor.Client.dll",
"/_framework/CordovaMudBlazor.Library.dll",
"/_framework/CordovaMudBlazor.Shared.dll",
"/_framework/System.Collections.Concurrent.dll",
"/_framework/System.Collections.dll",
"/_framework/System.Collections.Immutable.dll",
"/_framework/System.Collections.NonGeneric.dll",
"/_framework/System.Collections.Specialized.dll",
"/_framework/System.ComponentModel.Annotations.dll",
"/_framework/System.ComponentModel.dll",
"/_framework/System.ComponentModel.Primitives.dll",
"/_framework/System.ComponentModel.TypeConverter.dll",
"/_framework/System.Console.dll",
"/_framework/System.Drawing.Primitives.dll",
"/_framework/System.IO.Pipelines.dll",
"/_framework/System.Linq.dll",
"/_framework/System.Linq.Expressions.dll",
"/_framework/System.Linq.Queryable.dll",
"/_framework/System.Memory.dll",
"/_framework/System.Net.Http.dll",
"/_framework/System.Net.Primitives.dll",
"/_framework/System.ObjectModel.dll",
"/_framework/System.Private.CoreLib.dll",
"/_framework/System.Private.Runtime.InteropServices.JavaScript.dll",
"/_framework/System.Private.Uri.dll",
"/_framework/System.Runtime.CompilerServices.Unsafe.dll",
"/_framework/System.Runtime.InteropServices.RuntimeInformation.dll",
"/_framework/System.Runtime.Serialization.Primitives.dll",
"/_framework/System.Text.Encodings.Web.dll",
"/_framework/System.Text.Json.dll",
"/_content/MudBlazor/MudBlazor.min.css",
"/_content/MudBlazor/MudBlazor.min.js",

"/css/open-iconic/font/css/open-iconic-bootstrap.min.css",
"/css/open-iconic/font/fonts/open-iconic.eot",
"/css/open-iconic/font/fonts/open-iconic.otf",
"/css/open-iconic/font/fonts/open-iconic.svg",
"/css/open-iconic/font/fonts/open-iconic.ttf",
"/css/open-iconic/font/fonts/open-iconic.woff",
"/css/bootstrap/bootstrap.min.css",
"/css/animation.css",
"/css/app.css",
"/css/pull-to-refresh.css",
"/font/roboto.css",
"/font/woff/KFOlCnqEu92Fr1MmEU9fABc4EsA.woff2",
"/font/woff/KFOlCnqEu92Fr1MmEU9fBBc4.woff2",
"/font/woff/KFOlCnqEu92Fr1MmEU9fBxc4EsA.woff2",
"/font/woff/KFOlCnqEu92Fr1MmEU9fCBc4EsA.woff2",
"/font/woff/KFOlCnqEu92Fr1MmEU9fChc4EsA.woff2",
"/font/woff/KFOlCnqEu92Fr1MmEU9fCRc4EsA.woff2",
"/font/woff/KFOlCnqEu92Fr1MmEU9fCxc4EsA.woff2",
"/font/woff/KFOlCnqEu92Fr1MmSU5fABc4EsA.woff2",
"/font/woff/KFOlCnqEu92Fr1MmSU5fBBc4.woff2",
"/font/woff/KFOlCnqEu92Fr1MmSU5fBxc4EsA.woff2",
"/font/woff/KFOlCnqEu92Fr1MmSU5fCBc4EsA.woff2",
"/font/woff/KFOlCnqEu92Fr1MmSU5fChc4EsA.woff2",
"/font/woff/KFOlCnqEu92Fr1MmSU5fCRc4EsA.woff2",
"/font/woff/KFOlCnqEu92Fr1MmSU5fCxc4EsA.woff2",
"/font/woff/KFOlCnqEu92Fr1MmWUlfABc4EsA.woff2",
"/font/woff/KFOlCnqEu92Fr1MmWUlfBBc4.woff2",
"/font/woff/KFOlCnqEu92Fr1MmWUlfBxc4EsA.woff2",
"/font/woff/KFOlCnqEu92Fr1MmWUlfCBc4EsA.woff2",
"/font/woff/KFOlCnqEu92Fr1MmWUlfChc4EsA.woff2",
"/font/woff/KFOlCnqEu92Fr1MmWUlfCRc4EsA.woff2",
"/font/woff/KFOlCnqEu92Fr1MmWUlfCxc4EsA.woff2",
"/font/woff/KFOmCnqEu92Fr1Mu4mxK.woff2",
"/font/woff/KFOmCnqEu92Fr1Mu4WxKOzY.woff2",
"/font/woff/KFOmCnqEu92Fr1Mu5mxKOzY.woff2",
"/font/woff/KFOmCnqEu92Fr1Mu7GxKOzY.woff2",
"/font/woff/KFOmCnqEu92Fr1Mu7mxKOzY.woff2",
"/font/woff/KFOmCnqEu92Fr1Mu7WxKOzY.woff2",
"/font/woff/KFOmCnqEu92Fr1Mu72xKOzY.woff2",
"/js/app.js",
"/js/browser-resize.js",
"/js/pull-to-refresh.js",
"/favicon.ico",
"/icon-32.png",
"/icon-512.png",
"/index.html",
"/ServiceWorkerRegister.js",
"/manifest.json"
];

// * listen for the install event and pre-cache anything in filesToCache * //
self.addEventListener(swInstallEvent, event => {
    self.skipWaiting();
    event.waitUntil(
        caches.open(staticCacheName)
            .then(cache => {
                var x;
                try {
                    x = cache.addAll(requiredFiles);
                }
                catch (e) { }
                return x;
            })
    );
});

self.addEventListener(swActivateEvent, function (event) {
    event.waitUntil(
        caches.keys().then(function (cacheNames) {
            return Promise.all(
                cacheNames.map(function (cacheName) {
                    if (staticCacheName !== cacheName && cacheName.startsWith(staticCachePrefix)) {
                        var x;
                        try {
                            x = caches.delete(cacheName);
                        } catch (e) { }
                        return x;
                    }
                })
            );
        })
    );
});

self.addEventListener(networkFetchEvent, event => {
    const requestUrl = new URL(event.request.url);
    
    if (requestUrl.origin === location.origin) {
        if (requestUrl.pathname === baseURL) {
            event.respondWith(caches.match(indexURL));
            return;
        }
    }
    if (event.request.url.includes("/api/")) {
        return fetch(event.request).then(response => { return response.clone(); });
    }
    event.respondWith(
        caches.match(event.request)
            .then(response => {
                if (response) {
                    return response;
                }
                return fetch(event.request)
                    .then(response => {
                        if (response.ok) {
                            if (requestUrl.origin === location.origin) {
                                caches.open(staticCacheName).then(cache => {
                                    try {
                                        cache.put(event.request.url, response);
                                    } catch (e) { }
                                });
                            }
                        }
                        return response.clone();
                    });
            }).catch(error => {
                console.error(error);
            })
    );
});
