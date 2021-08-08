using System.Net.Http;
using System;

namespace CordovaMudBlazor.Library
{
    public class MyHttp : HttpClient
    {
        public static Uri MyBaseAddress = new Uri("https://pharga.johnkenedy.com"); 

        public static int PageSize { get; set; } = 40;

    }
}