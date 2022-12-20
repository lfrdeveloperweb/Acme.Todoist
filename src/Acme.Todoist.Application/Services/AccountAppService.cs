using Acme.Todoist.Application.DataContracts.Requests;
using Acme.Todoist.Application.DataContracts.Responses;
using Acme.Todoist.Application.Features.Accounts;
using Acme.Todoist.Domain.Commons;
using Acme.Todoist.Domain.Security;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Acme.Todoist.Domain.Models;

namespace Acme.Todoist.Application.Services;

public sealed class AccountAppService : AppServiceBase
{
    public AccountAppService(ISender sender, IMapper mapper) : base(sender, mapper) { }

    public async ValueTask<Response> RegisterAccountAsync(RegisterAccountRequest request, OperationContext operationContext, CancellationToken cancellationToken)
    {
        var command = new RegisterAccount.Command(
            request.Name,
            request.BirthDate,
            request.Email?.ToLower(),
            request.PhoneNumber,
            request.Password,
            request.ConfirmPassword,
            operationContext);

        var result = await Dispatcher.Send(command, cancellationToken);

        return Response.From(result);
    }

    public async ValueTask<Response<JwtTokenResponseData>> LoginAsync(LoginRequest request, OperationContext operationContext, CancellationToken cancellationToken)
    {
        var command = new LoginUser.Command(
            request.Email,
            request.Password,
            operationContext);

        var result = await Dispatcher.Send(command, cancellationToken);

        return Response.From<JwtToken, JwtTokenResponseData>(result, Mapper);
    }
    
    public async ValueTask<Response<UserResponseData>> GetProfileAsync(OperationContext operationContext, CancellationToken cancellationToken)
    {
        var query = new GetUserDetails.Query(operationContext.Identity.Id, operationContext);
        var result = await Dispatcher.Send(query, cancellationToken);

        return Response.From<User, UserResponseData>(result, Mapper);
    }

    public async ValueTask<Response> LockAccountAsync(string userId, OperationContext operationContext, CancellationToken cancellationToken)
    {
        var result = await Dispatcher.Send(new LockAccount.Command(userId,operationContext), cancellationToken);

        return Response.From(result);
    }
}