using Acme.Todoist.Application.DataContracts.Responses;
using Acme.Todoist.Domain.Models;
using AutoMapper;

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
            CreateMap<TodoComment, TodoCommentResponseData>();
        }
    }
}
