using Application.ViewModels.CategoryViewModels;
using Application.ViewModels.UserViewModel;
using Application.ViewModels.UserViewModels;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            CreateMap<Category, CreateCategoryViewModel>().ReverseMap();
            CreateMap<User, UserViewModel>()
                .ForMember(des => des.Role, src => src.MapFrom(x => x.Role != null ? (string)x.Role.ToString() : (string?)null))
                .ReverseMap();
            CreateMap<User, RegisterCustomerViewModel>().ReverseMap();
            CreateMap<User, RegisterEmployeeViewModel>().ReverseMap();
        }

    }
}
