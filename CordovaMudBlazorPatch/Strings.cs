using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CordovaMudBlazorPatch
{
    class Strings
    {
        public static string BaseTag = "<base";
        public static string CordovaBase = "<base href=\"file:///android_asset/www/\" target=\"_blank\">";
        public static string ScriptTag = "<script";
        public static string SourceCordovaJs = "src=\"cordova.js\"";
        public static string IncludeCordovaJs = "<script " + SourceCordovaJs + "></script>";
        public static string AaptOptionsIgnoreProperty = "ignoreAssetsPattern";
        public static string AaptOptionsIgnorePattern = "!.svn:!.git:!.ds_store:!*.scc:.*:!CVS:!thumbs.db:!picasa.ini:!*~";
        public static string AaptOptionsIgnore = @""+ AaptOptionsIgnoreProperty + @" """ + AaptOptionsIgnorePattern + @"""";
        public static string AaptOption = @"aaptOptions {
        " + AaptOptionsIgnore + @"
    }";

        public static string DefaultConfig = @"defaultConfig {";

        public static string MBMinifyCenter = ".mud-table-cell{display:flex;justify-content:space-between;align-items:center;";
        public static string MBMinifyCenterComment = ".mud-table-cell{display:flex;/*justify-content:space-between;align-items:center;*/";
        public static string MBMinifyNo50 = ".mud-table-cell:before{content:attr(data-label);font-weight:500;padding-right:16px;padding-inline-end:16px;padding-inline-start:unset;}";
        public static string MBMinify50 = ".mud-table-cell:before{content:attr(data-label);font-weight:500;padding-right:16px;width:50%;min-width:50%;}";

        public static string MBNoMinifyCenter = ".mud-table-cell:before {\n    content: attr(data-label);\n    font-weight: 500;\n    padding-right: 16px;";
        public static string MBNoMinifyCenterComment = @".mud-xs-table .mud-table-cell:before {
    content: attr(data-label);
    font-weight: 500;
    padding-right: 16px;
    width: 50%;
    min-width: 50%;";
        public static string MBNoMinifyNo50 = ".mud-xs-table .mud-table-cell {\n    display: flex;\n    justify-content: space-between;\n    align-items: center;";
        public static string MBNoMinify50 = @".mud-xs-table .mud-table-cell {
    display: flex;
    /*justify-content: space-between;
    align-items: center;*/";

        public static string Application = "<application ";
        public static string PermissionRead = "android.permission.READ_EXTERNAL_STORAGE";
        public static string AndroidPermissionRead = @"android:name=""" + PermissionRead + @"""";
        public static string UsePermissionRead = @"<uses-permission "+ AndroidPermissionRead + " />";
        public static string PermissionWrite = "android.permission.WRITE_EXTERNAL_STORAGE";
        public static string AndroidPermissionWrite = @"android:name=""" + PermissionWrite + @"""";
        public static string UsePermissionWrite = @"<uses-permission " + AndroidPermissionWrite + " />";

        public static string LegacyStorage = @"android:requestLegacyExternalStorage=""true""";

        public static string EndBody = "</body>";
        public static string InstantiateStreaming = @"<script>
        var ori = window.fetch;

        window.fetch = (url, info) => {
            if (url !== undefined) {
                console.log(""Fetch:"" + url);
                if (url.startsWith(""http"")) return ori(url, info);
            }
            else return;

            return new Promise(function (resolve, reject) {
                let xhr = new XMLHttpRequest();
                xhr.open(info.method || ""GET"", url);
                if (url.endsWith("".wasm"") || url.endsWith("".dat"") || url.endsWith("".dll"") || url.endsWith("".woff2""))
                    xhr.responseType = ""arraybuffer"";
                xhr.onload = function () {
                    if (this.status >= 200 && this.status < 300) {
                        var h = { get: () => undefined, clone: () => Object.assign({}, h) };
                        var obj = {
                            json: () => JSON.parse(xhr.response),
                            headers: h,
                            ok: true,
                            arrayBuffer: () => xhr.response,
                            clone: () => Object.assign({}, obj)
                        };
                        resolve(obj);
                    } else {
                        reject({
                            status: this.status,
                            statusText: xhr.statusText
                        });
                    }
                };
                xhr.onerror = function () {
                    reject({
                        status: this.status,
                        statusText: xhr.statusText
                    });
                };
                xhr.send();
            })
        };
        WebAssembly.instantiateStreaming = undefined;
    </script>";

        public static string ScriptPermission = @"<script>
        function popitup(url, windowName) {
            newwindow = window.open(url, windowName, 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,width=500,height=400');
            if (window.focus) { newwindow.focus() }
            return false;
        }

        var url = """";
        function DownloadToFolder(urx) {
            url = urx
            window.resolveLocalFileSystemURL(cordova.file.externalRootDirectory, onFileSystemSuccess, onError);
        }

        function onError(e) {
            alert(""Error : Downloading Failed"");
        }

        function onFileSystemSuccess(fileSystem) {
            var entry = """";

            entry = fileSystem;
            //entry = fileSystem.root; // ios = sessionStorage.platform.toLowerCase() == ""ios""

            entry.getDirectory(""Download"", {
                create: true,
                exclusive: false
            }, onGetDirectorySuccess, onError);
        };

        function GetCookie() {
            //DO HERE : return your authentication cookies
        }

        function onGetDirectorySuccess(dir) {
            cdr = dir;

            var fileTransfer = new FileTransfer();
            var uri = encodeURI(url);
            var token = window.blazorExtensions.ReadCookie(""Token"");
            var filename = url.split(""/"").pop().split('?')[0] + "".pdf"";

            fileTransfer.download(uri, cdr.nativeURL + filename,
                function (entry) {
                    alert(""Success : Download\\"" + filename);
                },
                function (error) {
                    alert(""Error : "" + error.code);
                },
                false,
                {
                    headers: {
                        ""token"": token
                    }
                }
            );
        };

        var url = """";
        function gotFileEntry(fileEntry) {
            var fileTransfer = new FileTransfer();

            var uri = encodeURI(url);
            var token = GetCookie();
            fileTransfer.download(uri, cdr.nativeURL,
                function (entry) {
                    // Logic to open file using file opener plugin
                },
                function (error) {
                    alert(""Error : "" + error.code);
                },
                false,
                {
                    headers: {
                        ""token"": token
                    }
                }
            );
        };

        function openblank(urx) {
            realFilename = """";
            if (IsCordovaExist()) {
                CheckFilePermission(function (perm) {
                    if (perm == false) {
                        alert(""Please allow Storage permission"");
                        return false;
                    }
                    DownloadToFolder(urx);
                });

                return false;
            }
            window.open(urx, ""_blank"");
            return false;
        }

        function locationhref(url) {
            window.location.href = url;
            return false;
        }

        function redirect(url) {
            window.location.href = url;
        }

        function alertmsg(msg) {
            alert(msg);
            return true;
        }

        function confirmdelete() {
            return confirm(""Are you sure you want to delete this record?"");
        }

        function isMobile() {
            var check = false;
            (function (a) { if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4))) check = true; })(navigator.userAgent || navigator.vendor || window.opera);
            return check;
        }

        function IsCordovaExist() {
            try {
                if (cordova == null || cordova.plugins == null || cordova.plugins.permissions == null) {
                    return false;
                }
            }
            catch (err) {
                return false;
            }
            return true;
        }

        function CheckFilePermission(callback) {
            if (IsCordovaExist() == false) callback(true);

            var permissions = cordova.plugins.permissions;

            permissions.hasPermission(permissions.READ_EXTERNAL_STORAGE, function (status) {
                if (status.hasPermission) {
                    callback(true);
                    return;
                }
                else {
                    permissions.requestPermission(permissions.READ_EXTERNAL_STORAGE, success, error);
                }
            });

            function error() {
                callback(false);
            }

            function success(status) {
                if (!status.hasPermission) {
                    callback(false);
                    return;
                }
                callback(true);
            }
        }

        window._callbacker = function (callbackObjectInstance, callbackMethod, callbackId, cmd, args) {
            var parts = cmd.split('.');
            var targetFunc = window;
            var parentObject = window;
            for (var i = 0; i < parts.length; i++) {
                if (i == 0 && part == 'window') continue;
                var part = parts[i];
                parentObject = targetFunc;
                targetFunc = targetFunc[part];
            }
            args = JSON.parse(args);
            args.push(function (e, d) {
                var args = [];
                for (var i in arguments) args.push(JSON.stringify(arguments[i]));
                callbackObjectInstance.invokeMethodAsync(callbackMethod, callbackId, args);
            });
            targetFunc.apply(parentObject, args);
        };
    </script>";
    }
}
