using System.Linq.Expressions;

namespace Framework.Core.Specification.Base
{
    internal class ParameterReplacer : ExpressionVisitor
    {
        private readonly ParameterExpression parameter;

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return base.VisitParameter(parameter);
        }

        internal ParameterReplacer(ParameterExpression parameter)
        {
            this.parameter = parameter;
        }
    }
}
