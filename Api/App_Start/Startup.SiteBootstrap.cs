﻿using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using AllAcu.Authentication;
using Domain;
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
            var allAcuBiller = Settings.Get<AllAcuBiller>();

            // Set up system login
            var userManager = container.Resolve<ApplicationUserManager>();
            if (await userManager.FindByNameAsync(allAcuBiller.Username) == null)
            {
                var result = await userManager.CreateAsync(new ApplicationUser
                {
                    UserId = allAcuBiller.UserId,
                    UserName = allAcuBiller.Username,
                    Email = allAcuBiller.Email
                }, allAcuBiller.Password);

                if (!result.Succeeded)
                {
                    Trace.Write(result.Errors);
                    throw new ConfigurationErrorsException(
                        "Could not create default system user account due to configuration problems.\r\n" +
                        string.Join("\r\n", result.Errors));
                }
            }

            if ((await container.Resolve<IEventSourcedRepository<Domain.User.User>>().GetLatest(allAcuBiller.UserId)) == null)
            {
                var user = new Domain.User.User(new Domain.User.User.CreateSystemUser
                {
                    AggregateId = allAcuBiller.UserId,
                    Username = allAcuBiller.Username,
                    Email = allAcuBiller.Email
                });
                await container.Resolve<IEventSourcedRepository<Domain.User.User>>().Save(user);
            }

            // Set up biller
            var billerRepo = container.Resolve<IEventSourcedRepository<Domain.Biller.Biller>>();
            var biller = await billerRepo.GetLatest(allAcuBiller.BillerId);

            if (biller == null)
            {
                biller = new Domain.Biller.Biller(new Domain.Biller.Biller.InitializeBiller
                {
                    Name = "AllAcu Services",
                    SystemUserId = allAcuBiller.UserId,
                    AggregateId = allAcuBiller.BillerId
                });
                await billerRepo.Save(biller);
            }
        }
    }
}
