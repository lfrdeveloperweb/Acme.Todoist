using System;
using System.Linq;
using System.Linq.Expressions;

namespace Acme.Todoist.Domain.Specs.Core;

internal sealed record NotSpecification<T>(Specification<T> Specification) : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        var expression = Specification.ToExpression();

        return Expression.Lambda<Func<T, bool>>(Expression.Not(expression.Body), expression.Parameters.Single());
    }
}