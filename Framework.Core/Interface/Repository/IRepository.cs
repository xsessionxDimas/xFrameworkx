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
    }
}
