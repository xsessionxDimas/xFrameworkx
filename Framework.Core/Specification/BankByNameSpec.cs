using System;
using System.Linq.Expressions;
using Framework.Core.Model;
using Framework.Core.Specification.Base;

namespace Framework.Core.Specification
{
    public class BankByNameSpec : SpecificationBase<Bank>
    {
        private readonly string name;

        public BankByNameSpec(string name)
        {
            this.name = name;
        }

        public override Expression<Func<Bank, bool>> ToExpression()
        {
            var parameter   = Expression.Parameter(typeof(Bank), "bank");
            var propertyExp = Expression.Property(parameter, nameof(Bank.BankName));
            var method      = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var callMethod  = Expression.Call(propertyExp, method, Expression.Constant(name, typeof(string)));
            return Expression.Lambda<Func<Bank, bool>>(callMethod, parameter);
        }
    }
}
