using Acme.Todoist.Domain.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Acme.Todoist.Infrastructure.Security;

public sealed class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) 
        : base(options) { }

    public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);
        
        policy ??= new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionRequirement(Enum.Parse<PermissionType>(policyName)))
            .Build();
       
        return policy;
    }
}