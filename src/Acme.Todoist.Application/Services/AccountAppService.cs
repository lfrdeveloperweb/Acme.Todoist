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
            request.DocumentNumber,
            request.Name,
            request.BirthDate,
            request.Email?.ToLower(),
            request.PhoneNumber,
            request.UserName?.ToLower(),
            request.Password,
            request.ConfirmPassword,
            operationContext);

        var result = await Sender.Send(command, cancellationToken);

        return Response.From(result);
    }

    public async ValueTask<Response<JwtTokenResponseData>> LoginAsync(LoginRequest request, OperationContext context, CancellationToken cancellationToken)
    {
        var command = new LoginUser.Command(
            request.Email,
            request.Password,
            context);

        var result = await Sender.Send(command, cancellationToken);

        return Response.From<JwtToken, JwtTokenResponseData>(result, Mapper);
    }
    
    public async ValueTask<Response<UserResponseData>> GetProfileAsync(OperationContext context, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetUserDetails.Query(context.Identity.Id, context), cancellationToken);

        return Response.From<User, UserResponseData>(result, Mapper);
    }

    public async ValueTask<Response> LockAccountAsync(string userId, OperationContext context, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new LockAccount.Command(userId,context), cancellationToken);

        return Response.From(result);
    }

    public async ValueTask<Response> UnlockAccountAsync(string userId, OperationContext context, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new UnlockAccount.Command(userId,context), cancellationToken);

        return Response.From(result);
    }

    public async ValueTask<Response> ForgotPasswordAsync(ForgotPasswordRequest request, OperationContext context, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new ForgotPassword.Command(request.DocumentNumber, context), cancellationToken);

        return Response.From(result);
    }

    public async Task<Response> ResetPasswordAsync(ResetPasswordRequest request, OperationContext context, CancellationToken cancellationToken)
    {
        var command = new ResetPassword.Command(
            request.DocumentNumber, 
            request.Password,
            request.NewPassword,
            request.ConfirmPassword,
            request.Token,
            context);
        
        var result = await Sender.Send(command, cancellationToken);

        return Response.From(result);
    }

    public async Task<Response> ChangePasswordAsync(ChangePasswordRequest request, OperationContext context, CancellationToken cancellationToken)
    {
        var command = new ChangePassword.Command(
            request.CurrentPassword,
            request.NewPassword,
            request.ConfirmNewPassword,
            context);

        var result = await Sender.Send(command, cancellationToken);

        return Response.From(result);
    }

    public async Task<Response> ConfirmEmailAsync(ConfirmEmailRequest request, OperationContext context, CancellationToken cancellationToken)
    {
        var command = new ConfirmEmail.Command(
            request.Email,
            request.Token,
            context);

        var result = await Sender.Send(command, cancellationToken);

        return Response.From(result);
    }
}