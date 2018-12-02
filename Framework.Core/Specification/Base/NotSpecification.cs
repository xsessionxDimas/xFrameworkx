using System;
using System.Linq.Expressions;
using Framework.Core.Interface.Specification;

namespace Framework.Core.Specification.Base
{
    public class NotSpecification<T> : SpecificationBase<T>
    {
        private readonly ISpecification<T> specification;
        private readonly ParameterExpression paramExpr;

        public NotSpecification(ISpecification<T> specification)
        {
            this.specification = specification;
            paramExpr          = Expression.Parameter(typeof(T));
        }
        
        public override Expression<Func<T, bool>> ToExpression()
        {
            var expression    = specification.ToExpression();
            var notExpression = Expression.Not(expression.Body);
            notExpression     = (UnaryExpression)new ParameterReplacer(paramExpr).Visit(notExpression);
            return Expression.Lambda<Func<T, bool>>(notExpression, paramExpr);
        }
    }
}
