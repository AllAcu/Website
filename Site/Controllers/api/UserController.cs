﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AllAcu.Authentication;
using Domain.CareProvider;
using Domain.User;
using Microsoft.AspNet.Identity;
using Microsoft.Its.Domain;

namespace AllAcu.Controllers.api
{
    [CareProviderIdFilter]
    [RoutePrefix("api/user")]
    [Authorize]
    public class UserController : ApiController
    {
        private readonly AllAcuSiteDbContext dbContext;
        private readonly IEventSourcedRepository<User> userEventSourcedRepository;
        private ApplicationUserManager userManager;

        public UserController(AllAcuSiteDbContext dbContext, IEventSourcedRepository<User> userEventSourcedRepository, ApplicationUserManager userManager)
        {
            this.dbContext = dbContext;
            this.userEventSourcedRepository = userEventSourcedRepository;
            this.userManager = userManager;
        }

        [Route("{userId}")]
        public async Task<UserDetails> GetUser(Guid userId)
        {
            return await dbContext.UserDetails.FindAsync(userId);
        }

        [Route("")]
        public IEnumerable<UserListItemViewModel> GetUsers()
        {
            return dbContext.UserList;
        }

        [Route("signup"), HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Signup(User.SignUp command)
        {
            if (await dbContext.UserDetails.AnyAsync(u => u.Email == command.Email))
            {
                return Ok();
            }

            var user = new User();
            await user.ApplyAsync(command);
            await userEventSourcedRepository.Save(user);

            return Ok();
        }

        [Route("invite")]
        public async Task<IHttpActionResult> Invite(User.Invite command)
        {
            command.ProviderId = ActionContext.ActionArguments.CurrentProviderId().Value;
            var userDetails = dbContext.UserDetails.FirstOrDefault(u => u.Email == command.Email);
            var user = userDetails == null ?
                new User(new User.SignUp { Email = command.Email }) :
                await userEventSourcedRepository.GetLatest(userDetails.UserId);

            await user.ApplyAsync(command);
            await userEventSourcedRepository.Save(user);

            return Ok();
        }

        [Route("register"), HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Register(User.Register command)
        {
            var confirmation = dbContext.Confirmations.FirstOrDefault(c => c.Token == command.Token);

            if (confirmation == null)
            {
                return NotFound();
            }

            var applicationUser = new ApplicationUser { UserName = confirmation.Email, Email = confirmation.Email, UserId = confirmation.UserId };
            var result = await userManager.CreateAsync(applicationUser, command.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            var user = await userEventSourcedRepository.GetLatest(confirmation.UserId);
            await user.ApplyAsync(command);
            await userEventSourcedRepository.Save(user);

            return Ok();
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
