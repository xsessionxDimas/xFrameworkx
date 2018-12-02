using System;
using System.Linq.Expressions;
using Framework.Core.Model;
using Framework.Core.Specification.Base;

namespace Framework.Core.Specification
{
    public class BankByCodeSpec : SpecificationBase<Bank>
    {
        private readonly string code;

        public BankByCodeSpec(string code)
        {
            this.code = code;
        }

        public override Expression<Func<Bank, bool>> ToExpression()
        {
            var parameter   = Expression.Parameter(typeof(Bank), "bank");
            var propertyExp = Expression.Property(parameter, nameof(Bank.BankCode));
            var method      = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var callMethod  = Expression.Call(propertyExp, method, Expression.Constant(code, typeof(string)));
            return Expression.Lambda<Func<Bank, bool>>(callMethod, parameter);
        }
    }
}
