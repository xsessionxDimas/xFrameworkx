using System;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.Core.Specification.Base
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<TSource, bool>> MakeCountPredicate<TSource>(string collectionName, string itemName, ExpressionType itemComparison, string itemValue)
        {
            var source              = Expression.Parameter(typeof(TSource), "request");
            Type[] type             = {typeof(TSource)};
            var parameterExpression = Expression.Parameter(type[0], "consent");
            var propertyNames       = "".Split('.');
            Expression propBase     = parameterExpression;
            foreach (var property in propertyNames.Select(propertyName => type[0].GetProperty(propertyName)))
            {
                propBase   = Expression.Property(propBase, property);
                type[0]    = propBase.Type;
            }
            var itemType   = type[0].GetGenericArguments()[0];
            var anyMethod  = typeof(Enumerable).GetMethods()
                            .Single(m => m.Name == "Any" && m.GetParameters().Length == 2)
                            .MakeGenericMethod(itemType);
            var predicate  = Expression.Lambda<Func<TSource, bool>>(Expression.Call(anyMethod, propBase, null), source);
            return predicate;
        }
    }
}
