using Acme.Todoist.Application.DataContracts.Responses;
using Acme.Todoist.Application.Features.Commands.Todos;
using Acme.Todoist.Commons.Models.Security;
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
            CreateMap<IIdentityContext, Membership>();
            CreateMap<Membership, IdentityNamedResponse>();
        }

        private void TodoMappers()
        {
            CreateMap<CreateTodo.Command, Todo>();
            //.ForMember(
            //    target => target.DueDate,
            //    option =>
            //    {
            //        option.AllowNull();
            //        option.MapFrom(source => DateOnly.FromDateTime(source.DueDate.GetValueOrDefault()));
            //    });


            CreateMap<CreateTodoComment.Command, TodoComment>();

            CreateMap<Todo, TodoResponseData>();
            CreateMap<TodoComment, TodoCommentResponseData>();
        }
    }
}
