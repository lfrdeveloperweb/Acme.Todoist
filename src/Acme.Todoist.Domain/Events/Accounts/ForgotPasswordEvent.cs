namespace Acme.Todoist.Domain.Events.Accounts
{
    public sealed record ForgotPasswordEvent(string SocialSecurityNumber, string Token) : IEvent;
}
