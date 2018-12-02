using System;
using System.Linq.Expressions;
using Framework.Core.Model;
using Framework.Core.Specification.Base;

namespace Framework.Core.Specification
{
    public class BankByIdSpec : SpecificationBase<Bank>
    {
        private readonly int id;

        public BankByIdSpec(int id)
        {
            this.id = id;
        }

        public override Expression<Func<Bank, bool>> ToExpression()
        {
            var parameter = Expression.Parameter(typeof(Bank), "bank");
            return Expression.Lambda<Func<Bank, bool>>(Expression.Equal(Expression.Property(parameter, nameof(Bank.Id)), Expression.Constant(id, typeof(int))), parameter);
        }
    }
}
