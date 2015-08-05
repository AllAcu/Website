using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using AllAcu.Authentication;
using Domain;
using Domain.Biller;
using Domain.User;
using Its.Configuration;
using Microsoft.Its.Domain;
using Owin;
using Pocket;

namespace AllAcu
{
    public partial class Startup
    {
        internal async Task BootstrapSite(IAppBuilder app, PocketContainer container)
        {
            var system = Settings.Get<SystemUser>();

            // Set up biller
            var billerRepo = container.Resolve<IEventSourcedRepository<Biller>>();
            var biller = await billerRepo.GetLatest(system.BillerId);

            if (biller == null)
            {
                biller = new Biller(new Biller.InitializeBiller
                {
                    Name = "AllAcu Services",
                    AggregateId = system.BillerId
                });
                await billerRepo.Save(biller);
            }

            // Set up system login
            var userManager = container.Resolve<ApplicationUserManager>();
            if (await userManager.FindByIdAsync(system.UserId.ToString()) == null)
            {
                var result = await userManager.CreateAsync(new ApplicationUser
                {
                    UserId = system.UserId,
                    UserName = system.Username,
                    Email = system.Email
                }, system.Password);

                if (!result.Succeeded)
                {
                    Trace.Write(result.Errors);
                    throw new ConfigurationErrorsException(
                        "Could not create default system user account due to configuration problems.\r\n" +
                        string.Join("\r\n", result.Errors));
                }
            }

            if ((await container.Resolve<IEventSourcedRepository<User>>().GetLatest(system.UserId)) == null)
            {
                var user = new User(new User.CreateSystemUser
                {
                    AggregateId = system.UserId,
                    Username = system.Username,
                    Email = system.Email
                });
                await container.Resolve<IEventSourcedRepository<User>>().Save(user);
            }
        }
    }
}
