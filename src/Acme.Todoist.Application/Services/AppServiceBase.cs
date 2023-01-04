using Acme.Todoist.Application.Core.Commands;
using Acme.Todoist.Application.DataContracts.Responses;
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
            Sender = sender;
            Mapper = mapper;
        }

        protected ISender Sender { get; }
        protected IMapper Mapper { get; }

        protected Response<TResponseData> From<TModel, TResponseData>(CommandResult<TModel> result) => Response.From<TModel, TResponseData>(result, Mapper);
    }
}
