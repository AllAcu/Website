using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using AllAcu.Authentication;
using Domain.Authentication;
using Domain.Registration;
using Domain.User;
using Microsoft.Its.Domain;

namespace AllAcu.Controllers.api
{
    [CareProviderIdFilter]
    [RoutePrefix("api/registration")]
    public class RegistrationController : ApiController
    {
        private IEventSourcedRepository<Registration> registrationEventSourcedRepository;
        private AllAcuSiteDbContext dbContext;
        private IEventSourcedRepository<User> userRepository;
        public ApplicationUserManager userManager;

        public RegistrationController(IEventSourcedRepository<Registration> registrationEventSourcedRepository, IEventSourcedRepository<User> userRepository, AllAcuSiteDbContext dbContext)
        {
            this.registrationEventSourcedRepository = registrationEventSourcedRepository;
            this.userRepository = userRepository;
            this.dbContext = dbContext;
        }

        [Route("signup")]
        public Task Signup(Registration.SignUp command)
        {
            var registration = new Registration(command);
            return registrationEventSourcedRepository.Save(registration);
        }

        [Route("invite")]
        [Authorize]
        public async Task Invite(Registration.Invite command)
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
        }

        [Route("confirm"), HttpPost]
        public async Task Confirm(Registration.ConfirmEmail command)
        {
            var confirmation = dbContext.Confirmations.FirstOrDefault(c => c.Token == command.Token && c.Email == command.Email);

            if (confirmation == null)
            {
                NotFound();
            }

            var registration = await registrationEventSourcedRepository.GetLatest(confirmation.RegistrationId);

            await registration.ApplyAsync(command);
        }

        [Route("register"), HttpPost]
        public async Task Register(Registration.Register command)
        {

        }
    }
}
