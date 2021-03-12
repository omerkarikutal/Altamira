using Core.Business;
using Core.Dtos;
using Core.Entities;
using Core.Hash;
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
        private readonly IHashService hashService;
        public SeedDataService(IUserService userService,IHashService hashService)
        {
            this.userService = userService;
            this.hashService = hashService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var list = await userService.GetAllUser();

            if (list.Count > 0)
                return;

            List<User> userPosts = new List<User>();

            userPosts.Add(new User
            {
                Username = "Bret",
                HashPassword = hashService.HashPassword("Bret")
            });
            userPosts.Add(new User
            {
                Username = "Antonette",
                HashPassword = hashService.HashPassword("Antonette")
            });
            userPosts.Add(new User
            {
                Username = "Samantha",
                HashPassword = hashService.HashPassword("Samantha")
            });
            await userService.AddRangeAsync(userPosts);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

}
