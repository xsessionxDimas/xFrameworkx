using System.Collections.Generic;

namespace Framework.Core.Interface.Repository
{
    public interface IProcedureRepository<out TEntity> where TEntity : class
    {
        IEnumerable<TEntity> QueryProcedure(string procedureName, IDictionary<string, object> parameter);
        int QueryProcedureRecordCount(string procedureName, IDictionary<string, object> parameter);
        void NonQueryProcedure(string procedureName, IDictionary<string, object> parameter);
    }
}
