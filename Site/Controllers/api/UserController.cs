using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Domain.Authentication;

namespace AllAcu.Controllers.api
{
    [CareProviderIdFilter]
    [RoutePrefix("api")]
    public class UserController : ApiController
    {
        [Route("user")]
        public IEnumerable<UserListItemViewModel> GetUsers()
        {
            return new[]
            {
                new UserListItemViewModel {Name = "Brett Morien"},
                new UserListItemViewModel {Name = "Dr. Smith"}
            };
        }
    }
}
