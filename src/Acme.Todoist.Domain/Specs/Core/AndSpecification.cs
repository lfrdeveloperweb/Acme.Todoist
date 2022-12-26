using System;
using System.Linq.Expressions;

namespace Acme.Todoist.Domain.Specs.Core;

internal sealed record AndSpecification<T>(Specification<T> Left, Specification<T> Right) : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        Expression<Func<T, bool>> expression = Left.ToExpression();
        InvocationExpression right = Expression.Invoke(Right.ToExpression(), expression.Parameters);
        return (Expression<Func<T, bool>>)Expression.Lambda(Expression.AndAlso(expression.Body, right), expression.Parameters);
    }
}