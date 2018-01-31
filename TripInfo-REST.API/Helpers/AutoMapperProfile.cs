using System;
using AutoMapper;
using TripInfoREST.API.Entities;
using TripInfoREST.API.Models;

namespace TripInfoREST.API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
