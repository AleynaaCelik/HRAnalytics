using AutoMapper;
using HRAnalytics.API.Models.Requests.Employee;
using HRAnalytics.API.Response;
using HRAnalytics.Application.DTOs;
using HRAnalytics.Application.DTOs.Employee;
using HRAnalytics.Application.DTOs.Progress;
using HRAnalytics.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeResponse>()
                 .ForMember(dest => dest.DepartmentName,
                     opt => opt.MapFrom(src => src.Department.Name));

            CreateMap<CreateEmployeeRequest, Employee>();
            CreateMap<UpdateEmployeeRequest, Employee>();
        }
    }
}
