using Api.Helper;
using Core.Business;
using Core.Dtos;
using Core.RedisManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await userService.GetAllUser());
        }
        [HttpGet("{id}")]
        [TypeFilter(typeof(NotFoundAttribute))]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await userService.GetUserById(id));
        }
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody] UserPost userPost)
        {
            return Ok(await userService.Add(userPost));
        }
        [HttpPut]
        [ValidateModel]
        public async Task<IActionResult> Put([FromBody] UserPut userPut)
        {
            return Ok(await userService.Update(userPut));
        }
        [HttpDelete("{id}")]
        [TypeFilter(typeof(NotFoundAttribute))]
        public async Task<IActionResult> Delete(string id)
        {
            await userService.Delete(id);
            return Ok();
        }
        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Login([FromBody] UserLogin model)
        {
            var result = await userService.GetUser(model);
            if (result == null)
                return NotFound();

            string token = userService.GenerateToken(result);
            return Ok(token);
        }
    }
}
