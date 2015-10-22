using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AllAcu.Controllers.api
{
    public class AuthController : ApiController
    {
        [Route("login"), HttpPost]
        public void Login(AuthenticationRequest login)
        {
            Debug.WriteLine($"{login.UserName} {login.Password}");
        }

        public class AuthenticationRequest
        {
            public string UserName { get; set; }
            public string Password { get; set; } 
        }
    }
}
