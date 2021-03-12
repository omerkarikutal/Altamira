using AutoMapper;
using AutoMapper.Configuration;
using Core.Business;
using Core.DataAccess;
using Core.Dtos;
using Core.Entities;
using Core.Hash;
using Core.RedisManager;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly ICacheService cacheService;
        private readonly IHashService hashService;
        public UserService(IUserRepository userRepository,
            IMapper mapper,
            ICacheService cacheService,
            IHashService hashService)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.cacheService = cacheService;
            this.hashService = hashService;
        }
        public async Task<UserDto> Add(UserPost model)
        {
            User dbUser = mapper.Map<User>(model);
            dbUser.HashPassword = hashService.HashPassword(model.Password);
            User result = await userRepository.AddAsync(dbUser);
            return mapper.Map<UserDto>(result);
        }

        public async Task Delete(string id)
        {
            UserDto userDto = await GetUserById(id);
            await userRepository.DeleteAsync(mapper.Map<User>(userDto));
        }

        public string GenerateToken(UserDto result)
        {
            var now = DateTime.UtcNow;

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,result.Username),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,now.ToUniversalTime().ToString(),ClaimValueTypes.Integer64)
            };

            string secret = "AltamiraJwtTokenSecretKey";//todo

            var signinKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));

            var jwt = new JwtSecurityToken
            (
                issuer: "omerkarikutal",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
            );

            var encodeJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodeJwt;
        }

        public async Task<List<UserDto>> GetAllUser()
        {
            string cacheKey = "Users";

            List<User> result;

            if (cacheService.Get<List<User>>(cacheKey) != null)
            {
                result = cacheService.Get<List<User>>(cacheKey);
            }
            else
            {
                result = await userRepository.Get();

                if (result.Count > 0)
                    cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(1));
            }

            return mapper.Map<List<UserDto>>(result);
        }

        public async Task<UserDto> GetUser(UserLogin userLogin)
        {
            User dbUser = await userRepository.GetAsync(s => s.Username == userLogin.Username);

            if (dbUser == null || !hashService.VerifyPassword(userLogin.Password, dbUser.HashPassword))
                return null;
            return mapper.Map<UserDto>(dbUser);
        }

        public async Task<UserDto> GetUserById(string id)
        {
            string cacheKey = $"User:{id}";
            User result;

            if (cacheService.Get<User>(cacheKey) != null)
            {
                result = cacheService.Get<User>(cacheKey);
            }
            else
            {
                result = await userRepository.GetAsync(s => s.Id == id);
                cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(5));
            }
            return mapper.Map<UserDto>(result);
        }

        public async Task<UserDto> Update(UserPut model)
        {
            User result = await userRepository.UpdateAsync(mapper.Map<User>(model));
            return mapper.Map<UserDto>(result);
        }
    }
}
