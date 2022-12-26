using System;
using System.Linq.Expressions;

namespace Acme.Todoist.Domain.Specs.Core
{
    public abstract record Specification<T>
    {
        private static readonly Specification<T> All = new IdentitySpecification<T>();

        public bool IsSatisfiedBy(T entity) => ToExpression().Compile()(entity);

        public abstract Expression<Func<T, bool>> ToExpression();

        public Specification<T> And(Specification<T> specification)
        {
            if (this == All)
                return specification;

            return specification == All ? this : new AndSpecification<T>(this, specification);
        }

        public Specification<T> Or(Specification<T> specification) => 
            this == All || specification == All ? All : new OrSpecification<T>(this, specification);

        public Specification<T> Not() => new NotSpecification<T>(this);
    }
}
