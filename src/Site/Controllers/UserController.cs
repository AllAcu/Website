using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AllAcu.Authentication;
using Domain.User;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Microsoft.Its.Domain;

namespace AllAcu.Controllers
{
    [CareProviderIdFilter]
    [Route("user")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly AllAcuSiteDbContext dbContext;
        private readonly IEventSourcedRepository<Domain.User.User> userEventSourcedRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public UserController(AllAcuSiteDbContext dbContext, IEventSourcedRepository<Domain.User.User> userEventSourcedRepository, UserManager<ApplicationUser> userManager)
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
        public async Task<ActionResult> Signup(Domain.User.User.SignUp command)
        {
            if (string.IsNullOrWhiteSpace(command.Email))
            {
                return HttpNotFound();
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
        public async Task<ActionResult> Invite(Domain.User.User.Invite command)
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
        public Task<ActionResult> InviteToBiller(Domain.User.User.Invite command)
        {
            command.OrganizationId = Domain.Biller.Biller.AllAcuBillerId;
            return Invite(command);
        }

        [Route("obsolete/{userId}/invites")]
        [AllowAnonymous]
        public ActionResult GetInvites(Guid userId)
        {
            return Ok(dbContext.ProviderInvitations.Where(i => i.User.UserId == userId));
        }

        [Route("{userId}/accept")]
        public async Task<ActionResult> Accept(Guid userId, Domain.User.User.AcceptInvite command)
        {
            var user = await userEventSourcedRepository.GetLatest(userId);
            if (user == null)
            {
                return HttpNotFound();
            }

            await user.ApplyAsync(command);
            await userEventSourcedRepository.Save(user);

            return Ok();
        }

        [Route("obsolete/register"), HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(Domain.User.User.Register command)
        {
            var confirmation = dbContext.Confirmations.FirstOrDefault(c => c.Token == command.Token);

            if (confirmation == null)
            {
                return HttpNotFound();
            }

            var applicationUser = new ApplicationUser { UserName = confirmation.Email, Email = confirmation.Email} ;//, UserId = confirmation.UserId };
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

        [Route("obsolete/confirmations"), HttpGet]
        public async Task<ActionResult> Confirmations()
        {
            return Ok(await dbContext.Confirmations.ToListAsync());
        }

        private ActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                throw new Exception();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return HttpBadRequest();
                }

                return HttpBadRequest(ModelState);
            }

            return null;
        }
    }
}
