using AutoMapper;
using WebApi.DTO;
using WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserForListDTO>();
            //CreateMap<User, UserForDetailsDTOcs>();
        }
    }
}
