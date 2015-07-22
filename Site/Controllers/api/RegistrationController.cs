using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AllAcu.Authentication;
using Domain.Registration;
using Microsoft.Its.Domain;

namespace AllAcu.Controllers.api
{
    [CareProviderIdFilter]
    [RoutePrefix("api/registration")]
    public class RegistrationController : ApiController
    {
        private IEventSourcedRepository<Registration> registrationEventSourcedRepository;
        private AllAcuSiteDbContext dbContext;
        public ApplicationUserManager userManager;

        public RegistrationController(IEventSourcedRepository<Registration> registrationEventSourcedRepository, AllAcuSiteDbContext dbContext)
        {
            this.registrationEventSourcedRepository = registrationEventSourcedRepository;
            this.dbContext = dbContext;
        }

        [Route("signup"), HttpPost]
        public async Task<IHttpActionResult> Signup(Registration.SignUp command)
        {
            var registration = new Registration(command);
            await registrationEventSourcedRepository.Save(registration);

            return Ok();
        }

        [Route("invite")]
        [Authorize]
        public async Task<IHttpActionResult> Invite(Registration.Invite command)
        {
            var confirmation = dbContext.Confirmations.FirstOrDefault(c => c.Email == command.Email);

            Registration registration;
            if (confirmation != null)
            {
                registration = await registrationEventSourcedRepository.GetLatest(confirmation.RegistrationId);
                await registration.ApplyAsync(command);
            }
            else
            {
                registration = new Registration(command);
            }

            await registrationEventSourcedRepository.Save(registration);

            return Ok();
        }

        [Route("register"), HttpPost]
        public async Task<IHttpActionResult> Register(Registration.Register command)
        {
            var confirmation = dbContext.Confirmations.FirstOrDefault(c => c.Token == command.Token);

            if (confirmation == null)
            {
                return NotFound();
            }

            var registration = await registrationEventSourcedRepository.GetLatest(confirmation.RegistrationId);
            await registration.ApplyAsync(command);

            return Ok();
        }
    }
}
