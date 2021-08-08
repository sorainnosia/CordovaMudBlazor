/******************************************************************************************************************************/
/*** AUTHOR NAME : JOHN KENEDY                                                                                              ***/
/*** EMAIL       : sorainnosia@gmail.com                                                                                    ***/
/*** History     : 24 Jul 2017 - JOHN KENEDY - Initial Development                                                          ***/
/***             :                                                                                                          ***/
/******************************************************************************************************************************/
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CordovaMudBlazor.Server
{
    public class AllowCORSAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", new Microsoft.Extensions.Primitives.StringValues("*"));
            context.HttpContext.Response.Headers.Add("Access-Control-Allow-Headers", new Microsoft.Extensions.Primitives.StringValues("Content-Type"));
            context.HttpContext.Response.Headers.Add("Access-Control-Allow-Methods", new Microsoft.Extensions.Primitives.StringValues("GET, POST, PUT, DELETE, OPTIONS"));
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}