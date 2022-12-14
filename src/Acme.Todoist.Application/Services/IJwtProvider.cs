using Acme.Todoist.Domain.Models;
using Acme.Todoist.Domain.ValueObjects;

namespace Acme.Todoist.Application.Services
{
    public interface IJwtProvider
    {
        JwtToken Generate(User user);
    }
}
