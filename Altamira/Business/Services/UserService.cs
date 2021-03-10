using AutoMapper;
using AutoMapper.Configuration;
using Core.Business;
using Core.DataAccess;
using Core.Dtos;
using Core.Entities;
using Microsoft.IdentityModel.Tokens;
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
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }
        public async Task<UserDto> Add(UserPost model)
        {
            try
            {
                User dbUser = mapper.Map<User>(model);
                User result = await userRepository.AddAsync(dbUser);
                return mapper.Map<UserDto>(result);
            }
            catch (Exception)
            {
                return null;
            }
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

            string iss = "AltamiraJwtTokenSecretKey";//todo

            var signinKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(iss));

            var jwt = new JwtSecurityToken
            (
                issuer: iss,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
            );

            var encodeJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodeJwt;
        }

        public async Task<List<UserDto>> GetAllUser()
        {
            List<User> result = await userRepository.Get();
            return mapper.Map<List<UserDto>>(result);
        }

        public async Task<UserDto> GetUser(UserLogin userLogin)
        {
            try
            {
                User dbUser = await userRepository.GetAsync(s => s.Username == userLogin.Username && s.Password == userLogin.Password);
                return mapper.Map<UserDto>(dbUser);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<UserDto> GetUserById(string id)
        {
            try
            {
                User dbUser = await userRepository.GetAsync(s => s.Id == id);
                return mapper.Map<UserDto>(dbUser);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<UserDto> Update(UserPut model)
        {
            try
            {
                User result = await userRepository.UpdateAsync(mapper.Map<User>(model));
                return mapper.Map<UserDto>(result);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
