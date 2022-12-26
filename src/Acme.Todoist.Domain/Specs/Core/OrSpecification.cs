using System;
using System.Linq.Expressions;

namespace Acme.Todoist.Domain.Specs.Core;

internal sealed record OrSpecification<T>(Specification<T> Left, Specification<T> Right) : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        Expression<Func<T, bool>> expression = Left.ToExpression();
        InvocationExpression right = Expression.Invoke(Right.ToExpression(), expression.Parameters);

        return (Expression<Func<T, bool>>)Expression.Lambda(Expression.OrElse(expression.Body, right), expression.Parameters);
    }
}