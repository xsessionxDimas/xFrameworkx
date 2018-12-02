using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace Framework.Repository.Repositories.Base
{
    public abstract class BaseRepository<TEntity> where TEntity : class 
    {
        protected readonly DbContext context;

        protected BaseRepository(DbContext context)
        {
            this.context = context;
        }

        public IEnumerable<TEntity> QueryProcedure(string procedureName, IDictionary<string, object> parameter)
        {
            var sqlQuery  = procedureName + " " + string.Join(", ", parameter.Keys);
            var sqlParams = new SqlParameter[parameter.Keys.Count];
            var index     = 0;
            foreach (var pair in parameter)
            {
                sqlParams[index++] = new SqlParameter
                {
                    ParameterName = pair.Key,
                    Value         = pair.Value ?? "null",
                    Direction     = System.Data.ParameterDirection.Input
                };
            }
            context.Database.CommandTimeout = 120;
            var result                      = context.Database.SqlQuery<TEntity>(sqlQuery, sqlParams).AsEnumerable();
            return result;
        }

        public int QueryProcedureRecordCount(string procedureName, IDictionary<string, object> parameter)
        {
            var sqlQuery  = procedureName + " " + string.Join(", ", parameter.Keys);
            var sqlParams = new SqlParameter[parameter.Keys.Count];
            var index     = 0;
            foreach (var pair in parameter)
            {
                sqlParams[index++] = new SqlParameter
                {
                    ParameterName = pair.Key,
                    Value         = pair.Value ?? "null",
                    Direction     = System.Data.ParameterDirection.Input
                };
            }
            var result = context.Database.SqlQuery<int>(sqlQuery, sqlParams).First();
            return result;
        }

        public void NonQueryProcedure(string procedureName, IDictionary<string, object> parameter)
        {
            var sqlQuery  = "EXEC " + procedureName + " " + string.Join(", ", parameter.Keys);
            var sqlParams = new object[parameter.Keys.Count];
            var index     = 0;
            foreach (var pair in parameter)
            {
                sqlParams[index++] = new SqlParameter
                {
                    ParameterName = pair.Key,
                    Value         = pair.Value,
                    Direction     = System.Data.ParameterDirection.Input
                };
            }
            context.Database.ExecuteSqlCommand(sqlQuery, sqlParams);
        }
    }
}
