using AutoMapper;
using HRAnalytics.Application.DTOs;
using HRAnalytics.Application.DTOs.Employee.Reguests;
using HRAnalytics.Application.DTOs.Employee.Responses;
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
            // Employee mapping konfigürasyonları
            CreateMap<Core.Entities.Employee, EmployeeDto>()
                .ForMember(dest => dest.DepartmentName,
                    opt => opt.MapFrom(src => src.Department.Name));

            CreateMap<CreateEmployeeDto, Core.Entities.Employee>();
            CreateMap<UpdateEmployeeDto, Core.Entities.Employee>();

            // Employee V2 mapping konfigürasyonları
            CreateMap<Core.Entities.Employee, EmployeeResponseV2>()
                .IncludeBase<Core.Entities.Employee, EmployeeDto>()
                .ForMember(dest => dest.TotalCompletedModules,
                    opt => opt.MapFrom(src => src.ProgressRecords.Count(p => p.CompletionPercentage == 100)))
                .ForMember(dest => dest.OverallProgress,
                    opt => opt.MapFrom(src => src.ProgressRecords.Any()
                        ? src.ProgressRecords.Average(p => p.CompletionPercentage)
                        : 0));
        }
    }
}


