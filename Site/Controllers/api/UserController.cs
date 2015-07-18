using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using AllAcu.Authentication;
using Domain.User;
using Microsoft.Its.Domain;

namespace AllAcu.Controllers.api
{
    [CareProviderIdFilter]
    [RoutePrefix("api")]
    [Authorize]
    public class UserController : ApiController
    {
        private readonly AllAcuSiteDbContext dbContext;
        private readonly IEventSourcedRepository<User> userEventSourcedRepository; 

        public UserController(AllAcuSiteDbContext dbContext, IEventSourcedRepository<User> userEventSourcedRepository)
        {
            this.dbContext = dbContext;
            this.userEventSourcedRepository = userEventSourcedRepository;
        }

        [Route("user/{userId}")]
        public async Task<UserDetailsViewModel> GetUser(Guid userId)
        {
            return await dbContext.UserDetails.FindAsync(userId);
        }

        [Route("user")]
        public IEnumerable<UserListItemViewModel> GetUsers()
        {
            return dbContext.UserList;
        }
    }
}
