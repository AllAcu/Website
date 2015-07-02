using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Domain.Authentication;
using Domain.User;
using Microsoft.Its.Domain;

namespace AllAcu.Controllers.api
{
    [CareProviderIdFilter]
    [RoutePrefix("api")]
    public class UserController : ApiController
    {
        private AllAcuSiteDbContext dbContext;

        public UserController(AllAcuSiteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Route("user")]
        public IEnumerable<UserListItemViewModel> GetUsers()
        {
            return dbContext.UserList;
        }
    }
}
