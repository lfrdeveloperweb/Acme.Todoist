using Acme.Todoist.Commons.Models.Security;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acme.Todoist.Application.DataContracts.Responses;
using Acme.Todoist.Domain.Models;

namespace Acme.Todoist.Application.Mappers
{
    public sealed class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CommonMappers();
            TodoMappers();
        }
        
        private void CommonMappers()
        {
            
        }

        private void TodoMappers()
        {
            CreateMap<Todo, TodoResponseData>();
        }
    }
}
