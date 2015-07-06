using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        [Route("user/{userId}/join"), HttpPost]
        public void JoinProvider(Guid userId, User.JoinProvider command)
        {
            var user = userEventSourcedRepository.GetLatest(userId);
            command.ApplyTo(user);
            userEventSourcedRepository.Save(user);
        }

        [Route("user/{userId}/leave"), HttpPost]
        public void LeaveProvider(Guid userId, User.LeaveProvider command)
        {
            var user = userEventSourcedRepository.GetLatest(userId);
            command.ApplyTo(user);
            userEventSourcedRepository.Save(user);
        }
    }
}
