using System;
using System.Linq.Expressions;
using Framework.Core.Model;
using Framework.Core.Specification.Base;

namespace Framework.Core.Specification
{
    public class ActiveBankSpec : SpecificationBase<Bank>
    {
        public override Expression<Func<Bank, bool>> ToExpression()
        {
            var parameter = Expression.Parameter(typeof(Bank), "bank");
            return Expression.Lambda<Func<Bank, bool>>(Expression.Equal(Expression.Property(parameter, nameof(Bank.IsDelete)), 
            Expression.Constant(false, typeof(bool))), parameter);
        }
    }
}
