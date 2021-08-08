using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace CordovaMudBlazor.Library
{
    public class BrowserService
    {
        // Compile to APK (Cordova)
        public static string wwwasset = "/android_asset/www/";
        // Compile to Web
        //public static string wwwasset = "/";
        public static Uri BaseAddress { get; set; }
        public static Action Refresh;
        public static Action StateHasChanged;
        public static event Func<Task> OnResize;
        public static IJSRuntime JRT;
        public static NavigationManager _navMan;
        public static NavigationManager navMan
        {
            get
            {
                return _navMan;
            }
            set
            {
                _navMan = value;
            }
        }

        public static int Height { get; set; }
        public static int Width { get; set; }
        public static bool mobile { get; set; }
        private static bool _MenuLeft = false;
        public static bool MenuLeft
        {
            get
            {
                return _MenuLeft;
            }
            set
            {
                _MenuLeft = value;
                if (StateHasChanged != null) StateHasChanged();
            }
        }

        private static bool _MenuRight = false;
        public static bool MenuRight
        {
            get
            {
                return _MenuRight;
            }
            set
            {
                _MenuRight = value;
                if (StateHasChanged != null) StateHasChanged();
            }
        }
        public static string token { get; set; }
        static BrowserService()
        {
            OnResize += BrowserService_OnResize;
        }

        public static string GetToken()
        {
            //DO THIS INSTEAD : Read from browser cookies and return the JWT token
            return token;
        }

        private static async Task BrowserService_OnResize()
        {
            if (JRT == null) return;

            try
            {
                mobile = await JRT.InvokeAsync<bool>("isMobile");
            }
            catch { }

            var x = await GetInnerWidth();
            var y = await GetInnerHeight();
            if (x != 0) Width = x;
            if (y != 0) Height = y;

            if (StateHasChanged != null) StateHasChanged();
        }

        public static async Task BrowserRefresh()
        {
            if (Refresh != null) await Task.Run(Refresh);
            if (StateHasChanged != null) StateHasChanged();
        }

        [JSInvokable]
        public static async Task OnBrowserResize()
        {
            await OnResize?.Invoke();
        }

        [JSInvokable]
        public static Task BlazorHref(string url)
        {
            string url2 = BrowserService.wwwasset + url.TrimStart('/');
            if (navMan != null) navMan.NavigateTo(url2, false);
            return Task.CompletedTask;
        }

        public static async Task<bool> LocationHref(string url)
        {
            string url2 = BrowserService.wwwasset + url.TrimStart('/');
            try
            {
                return await JRT.InvokeAsync<bool>("locationhref", url2);
            }
            catch { }
            return false;
        }

        public static async Task<bool> OpenBlank(string url)
        {
            try
            {
                return await JRT.InvokeAsync<bool>("openblank", url);
            }
            catch { }
            return false;
        }

        public static async Task<int> GetInnerHeight()
        {
            try
            {
                return await JRT.InvokeAsync<int>("browserResize.getInnerHeight");
            }
            catch { }
            return 0;
        }

        public static async Task<int> GetInnerWidth()
        {
            try
            {
                return await JRT.InvokeAsync<int>("browserResize.getInnerWidth");
            }
            catch { }
            return 0;
        }

        public static bool Xs()
        {
            int width = Width;
            return width < 600;
        }

        public static bool Sm()
        {
            int width = Width;
            return width >= 600 && width < 960;
        }

        public static bool Md()
        {
            int width = Width;
            return width >= 960 && width < 1280;
        }

        public static bool Lg()
        {
            int width = Width;
            return width >= 1280 && width < 1920;
        }

        public static bool Xl()
        {
            int width = Width;
            return width >= 1920;
        }
    }
}
