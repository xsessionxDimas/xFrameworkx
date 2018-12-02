using System;
using System.Linq.Expressions;
using Framework.Core.Interface.Specification;

namespace Framework.Core.Specification.Base
{
    public class AndSpecification<T> : SpecificationBase<T>
    {
        private readonly ISpecification<T> leftSpecification;
        private readonly ISpecification<T> rightSpecification;
        private readonly ParameterExpression paramExpr;

        public AndSpecification(ISpecification<T> leftSpecification, ISpecification<T> rightSpecification)
        {
            this.leftSpecification  = leftSpecification;
            this.rightSpecification = rightSpecification;
            paramExpr               = Expression.Parameter(typeof(T));
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var leftExpression  = leftSpecification.ToExpression();
            var rightExpression = rightSpecification.ToExpression();
            var andExpression   = Expression.And(leftExpression.Body, rightExpression.Body);
            andExpression       = (BinaryExpression) new ParameterReplacer(paramExpr).Visit(andExpression);
            var lambda          = Expression.Lambda<Func<T, bool>>(andExpression, paramExpr);
            return lambda;
        }
    }
}
