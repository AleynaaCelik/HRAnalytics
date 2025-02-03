using AutoMapper;
using HRAnalytics.Core.Common;
using HRAnalytics.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Application.Common
{
   
        public abstract class BaseHandler
        {
            protected readonly IUnitOfWork _unitOfWork;
            protected readonly IMapper _mapper;

            protected BaseHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            protected async Task<T> GetEntityByIdAsync<T>(int id) where T : BaseEntity
            {
                var entity = await _unitOfWork.Repository<T>().GetByIdAsync(id);
                if (entity == null)
                {
                    throw new NotFoundException(typeof(T).Name, id);
                }
                return entity;
            }

            protected async Task SaveChangesAsync()
            {
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
