using System;
using System.Linq.Expressions;

namespace Framework.Core.Interface.Specification
{
    public interface ISpecification<T>
    {
        bool IsSatisfied(T obj);
        Expression<Func<T, bool>> ToExpression();
        ISpecification<T> And(ISpecification<T> otherSpecification);
        ISpecification<T> Or(ISpecification<T> otherSpecification);
        ISpecification<T> Not();
    }
}
