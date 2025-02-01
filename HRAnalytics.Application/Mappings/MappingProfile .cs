using AutoMapper;
using HRAnalytics.API.Models.Requests.Employee;
using HRAnalytics.API.Response;
using HRAnalytics.Application.DTOs;
using HRAnalytics.Application.DTOs.Employee;
using HRAnalytics.Application.DTOs.Progress;
using HRAnalytics.Application.Employee;
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
                // Domain'den DTO'ya mapping
                CreateMap<Core.Entities.Employee, EmployeeResponse>()
                    .ForMember(dest => dest.DepartmentName,
                        opt => opt.MapFrom(src => src.Department.Name));

                CreateMap<Core.Entities.Employee, EmployeeResponseV2>()
                    .ForMember(dest => dest.DepartmentName,
                        opt => opt.MapFrom(src => src.Department.Name))
                    .ForMember(dest => dest.FullName,
                        opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                    .ForMember(dest => dest.TotalCompletedModules,
                        opt => opt.MapFrom(src => src.ProgressRecords.Count(p => p.CompletionPercentage == 100)))
                    .ForMember(dest => dest.OverallProgress,
                        opt => opt.MapFrom(src => src.ProgressRecords.Any()
                            ? src.ProgressRecords.Average(p => p.CompletionPercentage)
                            : 0));

                // DTO'dan Domain'e mapping
                CreateMap<CreateEmployeeRequest, Core.Entities.Employee>();
                CreateMap<UpdateEmployeeRequest, Core.Entities.Employee>();
            }
        }
    }

