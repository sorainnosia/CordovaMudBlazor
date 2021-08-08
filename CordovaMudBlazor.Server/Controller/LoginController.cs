using System;
using Microsoft.AspNetCore.Mvc;

namespace CordovaMudBlazor.Server
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [AllowCORS]
    [DisableRequestSizeLimit]
    public class LoginController : ControllerBase
    {
        public LoginController()
        {
        }
    }
}
