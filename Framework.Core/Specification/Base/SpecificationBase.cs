using System;
using System.Linq.Expressions;
using Framework.Core.Interface.Specification;

namespace Framework.Core.Specification.Base
{
    public abstract class SpecificationBase<T> : ISpecification<T>
    {
        public bool IsSatisfied(T obj)
        {
            var predicate = ToExpression().Compile();
            return predicate(obj);
        }

        public abstract Expression<Func<T, bool>> ToExpression();

        public ISpecification<T> And(ISpecification<T> otherSpecification)
        {
            return new AndSpecification<T>(this, otherSpecification);
        }

        public ISpecification<T> Or(ISpecification<T> otherSpecification)
        {
            return new OrSpecification<T>(this, otherSpecification);
        }

        public ISpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }
}
