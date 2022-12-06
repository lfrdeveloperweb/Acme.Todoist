using Acme.Todoist.Commons.Models.Security;
using Acme.Todoist.Core.Features.Commands;
using Acme.Todoist.Domain.Models;
using AutoMapper;
using System;

namespace Acme.Todoist.Core.Mappers
{
    /// <summary>
    /// Class that contains configuration to map request, response and models.
    /// </summary>
    public sealed class CoreProfile : Profile
    {
        public CoreProfile()
        {
            CommonMappers();
            TodoMappers();
        }

        private void CommonMappers()
        {
            CreateMap<IIdentityContext, Membership>();
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
        }
    }
}
