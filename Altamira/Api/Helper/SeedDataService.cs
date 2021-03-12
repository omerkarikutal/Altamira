using Core.Business;
using Core.Dtos;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Helper
{
    public class SeedDataService : IHostedService
    {
        private readonly IUserService userService;
        public SeedDataService(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var list = await userService.GetAllUser();

            if (list.Count > 0)
                return;

            UserPost userPost = new UserPost
            {
                Username = "test",
                Password = "1234"
            };

            await userService.Add(userPost);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

}
