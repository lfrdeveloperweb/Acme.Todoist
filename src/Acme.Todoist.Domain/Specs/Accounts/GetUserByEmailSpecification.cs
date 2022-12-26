using Acme.Todoist.Domain.Models;
using Acme.Todoist.Domain.Specs.Core;
using System;
using System.Linq.Expressions;

namespace Acme.Todoist.Domain.Specs.Accounts
{
    public sealed record GetUserByEmailSpecification(string Email) : Specification<User>
    {
        public override Expression<Func<User, bool>> ToExpression() => user => user.Email.Equals(Email);
    }
}
