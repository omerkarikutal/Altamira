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
        public SeedDataService(IUserService userService, IHashService hashService)
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
                Name = "Bret",
                Surname = "Bret",
                Username = "Bret",
                HashPassword = hashService.HashPassword("Bret"),
                CreateDate = DateTime.Now
            });
            userPosts.Add(new User
            {
                Name = "Antonette",
                Surname = "Antonette",
                Username = "Antonette",
                HashPassword = hashService.HashPassword("Antonette"),
                CreateDate = DateTime.Now
            });
            userPosts.Add(new User
            {
                Name = "Samantha",
                Surname = "Samantha",
                Username = "Samantha",
                HashPassword = hashService.HashPassword("Samantha"),
                CreateDate = DateTime.Now
            });
            await userService.AddRangeAsync(userPosts);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

}
