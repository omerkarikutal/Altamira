using Core.Dtos;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUser();
        Task<UserDto> GetUserById(string id);
        Task<UserDto> Add(UserPost model);
        Task<UserDto> Update(UserPut model);
        Task Delete(string id);
        Task<UserDto> GetUser(UserLogin userLogin);
        string GenerateToken(UserDto result);
        Task AddRangeAsync(List<User> userDtos);//Seed method olduğu için direk user entity'si kullanıldı.
    }
}
