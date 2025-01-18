using AutoMapper;
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
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.DepartmentName,
                    opt => opt.MapFrom(src => src.Department.Name));

            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<UpdateEmployeeDto, Employee>();

            CreateMap<EmployeeProgress, ModuleProgressDto>()
                .ForMember(dest => dest.ModuleName,
                    opt => opt.MapFrom(src => src.Module.Name));

            CreateMap<Department, DepartmentDto>();
            CreateMap<LearningModule, LearningModuleDto>();
        }
    }
}
