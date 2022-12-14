using Acme.Todoist.Application.DataContracts.Requests;
using Acme.Todoist.Application.DataContracts.Responses;
using Acme.Todoist.Application.Features.Commands.Accounts;
using Acme.Todoist.Domain.ValueObjects;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Acme.Todoist.Domain.Commons;

namespace Acme.Todoist.Application.Services;

public sealed class AccountAppService : AppServiceBase
{
    public AccountAppService(ISender sender, IMapper mapper) : base(sender, mapper) { }

    public async ValueTask<Response<TokenResponseData>> LoginAsync(LoginRequest request, OperationContext operationContext, CancellationToken cancellationToken)
    {
        var command = new LoginUser.Command(
            request.Email,
            request.Password,
            operationContext);

        var result = await Dispatcher.Send(command, cancellationToken);

        return Response.From<JwtToken, TokenResponseData>(result, Mapper);
    }

}