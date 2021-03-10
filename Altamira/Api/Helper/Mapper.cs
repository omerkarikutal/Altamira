using AutoMapper;
using Core.Dtos;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Helper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<UserPost, User>();
            CreateMap<User, UserPost>();

            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();

            CreateMap<UserPut, User>();
            CreateMap<User, UserPut>();

            CreateMap<UserLogin, User>();
            CreateMap<User, UserLogin>();
        }
    }
}
