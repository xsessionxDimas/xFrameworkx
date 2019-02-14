using System.Data.Entity;
using System.Linq;
using Framework.Core.Interface.Repository;
using Framework.Core.Interface.Specification;
using Framework.Repository.Repositories.Base;

namespace Framework.Repository.Repositories
{
    public class AuditTrailRepository : BaseRepository<Core.Model.AuditTrail>, IRepository<Core.Model.AuditTrail, int>
    {
        public AuditTrailRepository(DbContext context) : base(context)
        {
            this.context.ChangeTracker.DetectChanges();
        }

        private DbSet<Core.Model.AuditTrail> GetEntities()
        {
            return context.Set<Core.Model.AuditTrail>();
        }

        public Core.Model.AuditTrail FindById(int id)
        {
            return GetEntities().Find(id);
        }

        public Core.Model.AuditTrail FindOne(ISpecification<Core.Model.AuditTrail> spec, string[] includes)
        {
            var query = GetEntities().AsQueryable();
            query     = includes.Aggregate(query, (current, include) => current.Include(include));
            return query.Where(spec.ToExpression()).SingleOrDefault();
        }

        public IQueryable<Core.Model.AuditTrail> Find(ISpecification<Core.Model.AuditTrail> spec, string[] includes, bool isAscending = false)
        {
            var query = GetEntities().AsQueryable();
            query     = includes.Aggregate(query, (current, include) => current.Include(include));
            return isAscending ? query.Where(spec.ToExpression()).OrderBy(o => o.Date) : query.Where(spec.ToExpression()).OrderByDescending(o => o.Date);
        }

        public void Add(Core.Model.AuditTrail entity)
        {
            if (entity.Id == 0)
            {
                GetEntities().Add(entity);
                return;
            }
            GetEntities().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public void Remove(Core.Model.AuditTrail entity)
        {
            DbSet<Core.Model.AuditTrail> dbContext = GetEntities();
            dbContext.Attach(entity);
            dbContext.Remove(entity);
        }

        public int Count(ISpecification<Core.Model.AuditTrail> spec, string[] includes)
        {
            return GetEntities().Count(spec.ToExpression());
        }
    }
}
