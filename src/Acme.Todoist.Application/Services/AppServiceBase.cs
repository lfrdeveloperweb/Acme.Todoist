using Acme.Todoist.Application.DataContracts.Responses;
using Acme.Todoist.Infrastructure.Commands;
using AutoMapper;
using MediatR;

namespace Acme.Todoist.Application.Services
{
    public abstract class AppServiceBase
    {
        protected AppServiceBase(
            ISender sender,
            IMapper mapper)
        {
            Dispatcher = sender;
            Mapper = mapper;
        }

        protected ISender Dispatcher { get; }
        protected IMapper Mapper { get; }

        protected Response<TResponseData> From<TModel, TResponseData>(CommandResult<TModel> result) => Response.From<TModel, TResponseData>(result, Mapper);
    }
}
