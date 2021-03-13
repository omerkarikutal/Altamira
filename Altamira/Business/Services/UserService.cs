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
            dbUser.CreateDate = DateTime.Now;
            dbUser.HashPassword = hashService.HashPassword(model.Password);
            User result = await userRepository.AddAsync(dbUser);
            await cacheService.Remove(CacheConsts.UserList);
            return mapper.Map<UserDto>(result);
        }
        public async Task AddRangeAsync(List<User> users)
        {
            await userRepository.AddRangeAsync(users);
        }
        public async Task Delete(string id)
        {
            UserDto userDto = await GetUserById(id);
            await userRepository.DeleteAsync(mapper.Map<User>(userDto));
            await cacheService.Remove(CacheConsts.UserList);
        }
        public async Task<List<UserDto>> GetAllUser()
        {
            string cacheKey = CacheConsts.UserList;

            List<User> result;

            if (await cacheService.Get<List<User>>(cacheKey) != null)
            {
                result = await cacheService.Get<List<User>>(cacheKey);
            }
            else
            {
                result = await userRepository.Get();
                if (result.Count > 0)
                    await cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(1));
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
            User result = await userRepository.GetAsync(s => s.Id == id);
            return mapper.Map<UserDto>(result);
        }
        public async Task<UserDto> Update(UserPut model)
        {
            User dbUser = await userRepository.GetAsync(s => s.Id == model.Id);

            if (!string.IsNullOrEmpty(model.Password))
                dbUser.HashPassword = hashService.HashPassword(model.Password);
            if (!string.IsNullOrEmpty(model.Name))
                dbUser.Name = model.Name;
            if (!string.IsNullOrEmpty(model.Surname))
                dbUser.Surname = model.Surname;
            if (!string.IsNullOrEmpty(model.Username))
                dbUser.Username = model.Username;
            dbUser.CreateDate = DateTime.Now;
            User result = await userRepository.UpdateAsync(dbUser);

            await cacheService.Remove(CacheConsts.UserList);
            return mapper.Map<UserDto>(result);
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
    }
}
