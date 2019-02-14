using System.Data.Entity;
using System.Linq;
using Framework.Core.Interface.Repository;
using Framework.Core.Interface.Specification;
using Framework.Repository.Repositories.Base;

namespace Framework.Repository.Repositories
{
    public class BankBranchRepository : BaseRepository<Core.Model.BankBranch>, IRepository<Core.Model.BankBranch, int>
    {
        public BankBranchRepository(DbContext context) : base(context)
        {
            this.context.ChangeTracker.DetectChanges();
        }

        private DbSet<Core.Model.BankBranch> GetEntities()
        {
            return context.Set<Core.Model.BankBranch>();
        }

        public Core.Model.BankBranch FindById(int id)
        {
            return GetEntities().Find(id);
        }

        public Core.Model.BankBranch FindOne(ISpecification<Core.Model.BankBranch> spec, string[] includes)
        {
            var query = GetEntities().AsQueryable();
            query     = includes.Aggregate(query, (current, include) => current.Include(include));
            return query.Where(spec.ToExpression()).SingleOrDefault();
        }

        public IQueryable<Core.Model.BankBranch> Find(ISpecification<Core.Model.BankBranch> spec, string[] includes, bool isAscending = false)
        {
            var query = GetEntities().AsQueryable();
            query     = includes.Aggregate(query, (current, include) => current.Include(include));
            return isAscending ? query.Where(spec.ToExpression()).OrderBy(o => o.Id) : query.Where(spec.ToExpression()).OrderByDescending(o => o.Id);
        }

        public void Add(Core.Model.BankBranch entity)
        {
            if (entity.Id == 0)
            {
                GetEntities().Add(entity);
                return;
            }
            GetEntities().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public void Remove(Core.Model.BankBranch entity)
        {
            DbSet<Core.Model.BankBranch> dbContext = GetEntities();
            dbContext.Attach(entity);
            dbContext.Remove(entity);
        }

        public int Count(ISpecification<Core.Model.BankBranch> spec, string[] includes)
        {
            return GetEntities().Count(spec.ToExpression());
        }
    }
}
