using System.Collections.Generic;

namespace Acme.Todoist.Domain.Security;

public sealed record Anonymous : IIdentityContext
{
    public string Id => null;
    public string Name => "anonymous";
    public Role Role => Role.Anonymous;
    public bool IsAuthenticated => false;
    public bool IsAdmin => false;
    public bool IsClientApplication => false;
    public IReadOnlyDictionary<string, IReadOnlyCollection<string>> Claims => null;
}