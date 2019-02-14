using System.Data.Entity;
using System.Linq;
using Framework.Core.Interface.Repository;
using Framework.Core.Interface.Specification;
using Framework.Repository.Repositories.Base;

namespace Framework.Repository.Repositories
{
    public class BankRepository : BaseRepository<Core.Model.Bank>, IRepository<Core.Model.Bank, int>
    {
        public BankRepository(DbContext context) : base(context)
        {
            this.context.ChangeTracker.DetectChanges();
        }

        private DbSet<Core.Model.Bank> GetEntities()
        {
            return context.Set<Core.Model.Bank>();
        }

        public Core.Model.Bank FindById(int id)
        {
            return GetEntities().Find(id);
        }

        public Core.Model.Bank FindOne(ISpecification<Core.Model.Bank> spec, string[] includes)
        {
            var query = GetEntities().AsQueryable();
            if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));
            return query.Where(spec.ToExpression()).SingleOrDefault();
        }

        public IQueryable<Core.Model.Bank> Find(ISpecification<Core.Model.Bank> spec, string[] includes, bool isAscending = false)
        {
            var query = GetEntities().AsQueryable();
            if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));
            return isAscending ? query.Where(spec.ToExpression()).OrderBy(o => o.Id) : query.Where(spec.ToExpression()).OrderByDescending(o => o.Id);
        }

        public void Add(Core.Model.Bank entity)
        {
            if (entity.Id == 0)
            {
                GetEntities().Add(entity);
                return;
            }
            GetEntities().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public void Remove(Core.Model.Bank entity)
        {
            DbSet<Core.Model.Bank> dbContext = GetEntities();
            dbContext.Attach(entity);
            dbContext.Remove(entity);
        }

        public int Count(ISpecification<Core.Model.Bank> spec, string[] includes)
        {
            return GetEntities().Count(spec.ToExpression());
        }
    }
}
