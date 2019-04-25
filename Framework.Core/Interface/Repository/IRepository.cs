using System.Collections.Generic;
using System.Linq;
using Framework.Core.Interface.Specification;

namespace Framework.Core.Interface.Repository
{
    public interface IRepository<TEntity, in T> where TEntity : class
    {
        TEntity FindById(T id);
        TEntity FindOne(ISpecification<TEntity> spec, string[] includes = null);
        IQueryable<TEntity> Find(ISpecification<TEntity> spec, string[] includes  = null, bool isAscending = false);
        void Add(TEntity entity);
        void Remove(TEntity entity);
        int Count(ISpecification<TEntity> spec, string[] includes = null);

        IEnumerable<TEntity> QueryProcedure(string procedureName, IDictionary<string, object> parameter);
        int QueryProcedureRecordCount(string procedureName, IDictionary<string, object> parameter);
        void NonQueryProcedure(string procedureName, IDictionary<string, object> parameter);
    }
}
