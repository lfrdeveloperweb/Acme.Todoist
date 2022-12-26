using System;
using System.Linq.Expressions;

namespace Acme.Todoist.Domain.Specs.Core;

internal sealed record IdentitySpecification<T> : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression() => _ => true;
}