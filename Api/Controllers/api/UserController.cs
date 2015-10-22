using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AllAcu.Authentication;
using Domain.User;
using Microsoft.Ajax.Utilities;
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
        private readonly IEventSourcedRepository<Domain.User.User> userEventSourcedRepository;
        private readonly ApplicationUserManager userManager;

        public UserController(AllAcuSiteDbContext dbContext, IEventSourcedRepository<Domain.User.User> userEventSourcedRepository, ApplicationUserManager userManager)
        {
            this.dbContext = dbContext;
            this.userEventSourcedRepository = userEventSourcedRepository;
            this.userManager = userManager;
        }

        [Route("{userId}")]
        public async Task<User> GetUser(Guid userId)
        {
            return await dbContext.Users.FindAsync(userId);
        }

        [Route("")]
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await dbContext.Users.Where(u => u.Confirmed).ToArrayAsync();
        }

        [Route("signup"), HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Signup(Domain.User.User.SignUp command)
        {
            if (command.Email.IsNullOrWhiteSpace())
            {
                return NotFound();
            }
            if (await dbContext.Users.AnyAsync(u => command.Email.Equals(u.Email, StringComparison.OrdinalIgnoreCase)))
            {
                return Ok();
            }

            var user = new Domain.User.User();
            await user.ApplyAsync(command);
            await userEventSourcedRepository.Save(user);

            return Ok();
        }

        [Route("invite"), HttpPost]
        public async Task<IHttpActionResult> Invite(Domain.User.User.Invite command)
        {
            command.Role = command.Role ?? Domain.CareProvider.CareProvider.Roles.Practitioner;

            var userDetails = dbContext.Users.FirstOrDefault(u => u.Email == command.Email);
            var user = userDetails == null ?
                new Domain.User.User(new Domain.User.User.SignUp { Email = command.Email }) :
                await userEventSourcedRepository.GetLatest(userDetails.UserId);

            if (user.HasBeenInvited(command.OrganizationId, command.Role))
            {
                return Ok();
            }

            await user.ApplyAsync(command);
            await userEventSourcedRepository.Save(user);

            return Ok();
        }

        [Route("inviteToBiller"), HttpPost]
        public Task<IHttpActionResult> InviteToBiller(Domain.User.User.Invite command)
        {
            command.OrganizationId = Domain.Biller.Biller.AllAcuBillerId;
            return Invite(command);
        }

        [Route("{userId}/invites")]
        [AllowAnonymous]
        public IHttpActionResult GetInvites(Guid userId)
        {
            return Ok(dbContext.ProviderInvitations.Where(i => i.User.UserId == userId));
        }

        [Route("{userId}/accept")]
        public async Task<IHttpActionResult> Accept(Guid userId, Domain.User.User.AcceptInvite command)
        {
            var user = await userEventSourcedRepository.GetLatest(userId);
            if (user == null)
            {
                return NotFound();
            }

            await user.ApplyAsync(command);
            await userEventSourcedRepository.Save(user);

            return Ok();
        }

        [Route("register"), HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Register(Domain.User.User.Register command)
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

        [Route("confirmations"), HttpGet]
        public async Task<IHttpActionResult> Confirmations()
        {
            return Ok(await dbContext.Confirmations.ToListAsync());
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
