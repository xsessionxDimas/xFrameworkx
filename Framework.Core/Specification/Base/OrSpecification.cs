using System;
using System.Linq.Expressions;
using Framework.Core.Interface.Specification;

namespace Framework.Core.Specification.Base
{
    public class OrSpecification<T> : SpecificationBase<T>
    {
        private readonly ISpecification<T> leftSpecification;
        private readonly ISpecification<T> rightSpecification;
        private readonly ParameterExpression paramExpr;

        public OrSpecification(ISpecification<T> leftSpecification, ISpecification<T> rightSpecification)
        {
            this.leftSpecification  = leftSpecification;
            this.rightSpecification = rightSpecification;
            paramExpr               = Expression.Parameter(typeof(T));
        }
        
        public override Expression<Func<T, bool>> ToExpression()
        {
            var leftExpression  = leftSpecification.ToExpression();
            var rightExpression = rightSpecification.ToExpression();
            var orExpression    = Expression.Or(leftExpression.Body, rightExpression.Body);
            orExpression        = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(orExpression);
            var lambda          = Expression.Lambda<Func<T, bool>>(orExpression, paramExpr);
            return lambda;
        }
    }
}
