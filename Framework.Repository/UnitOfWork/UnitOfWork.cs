using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Framework.Core.Interface.Repository;
using Framework.Core.Interface.Service;
using Framework.Core.Interface.UnitOfWork;
using Framework.Repository.AuditTrail;

namespace Framework.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        //http://msdn.microsoft.com/en-us/library/bb738523.aspx
        //http://stackoverflow.com/questions/815586/entity-framework-using-transactions-or-savechangesfalse-and-acceptallchanges

        public readonly DbContext context;
        private readonly IDbTransaction transaction;
        private readonly ObjectContext objectContext;
        private readonly IRepository<Core.Model.AuditTrail, int> auditRepository; 
        
        /// <summary>
        /// Constructor
        /// </summary>
        public UnitOfWork(DbContext context, IRepository<Core.Model.AuditTrail, int> auditRepository)
        {
            this.context = context;
            this.auditRepository = auditRepository;

            // In order to make calls that are overidden in the caching ef-wrapper, we need to use
            // transactions from the connection, rather than TransactionScope.
            // This results in our call e.g. to commit() being intercepted
            // by the wrapper so the cache can be adjusted.
            // This won't work with the dbcontext because it handles the connection itself, so we must use the underlying ObjectContext.
            // http://blogs.msdn.com/b/diego/archive/2012/01/26/exception-from-dbcontext-api-entityconnection-can-only-be-constructed-with-a-closed-dbconnection.aspx
            objectContext = ((IObjectContextAdapter)this.context).ObjectContext;

            // Updating EF timeout taken from
            // http://stackoverflow.com/questions/6232633/entity-framework-timeouts
            //_objectContext.CommandTimeout = 3 * 60; // value in seconds
            if (objectContext.Connection.State == ConnectionState.Open) return;
            objectContext.Connection.Open();
            transaction = objectContext.Connection.BeginTransaction();
        }

        public void AutoDetectChangesEnabled(bool option)
        {
            context.Configuration.AutoDetectChangesEnabled = option;
        }

        public void LazyLoadingEnabled(bool option)
        {
            context.Configuration.LazyLoadingEnabled = option;
        }

        public void SaveChanges()
        {
            try
            {
                var auditFactory = new AuditTrailFactory(context);
                var entityList   = context.ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified);
                IEnumerable<Core.Model.AuditTrail> audit = null;
                foreach (var entity in entityList)
                {
                    audit = auditFactory.GetAudit(entity);
                }
                context.SaveChanges();
                if (audit == null) return;
                foreach (var log in audit)
                {
                    auditRepository.Add(log);
                }
                context.SaveChanges();
            }
            catch (OptimisticConcurrencyException)
            {
                var refreshableObjects = context.ChangeTracker.Entries().Select(c => c.Entity).ToList();
                ((IObjectContextAdapter) context).ObjectContext.Refresh(RefreshMode.ClientWins, refreshableObjects);
            }
        }

        public void Commit()
        {
            SaveChanges();
            transaction.Commit();
        }

        /// <summary>
        /// Commits the transcation and saves changes to the database.. Also clears the long term cache based on the starting cache keys from CacheConstants
        /// </summary>
        /// <param name="cacheStartsWithToClear"></param>
        /// <param name="cacheService"></param>
        public void Commit(List<string> cacheStartsWithToClear, ICacheService cacheService)
        {
            Commit();
            cacheService.ClearStartsWith(cacheStartsWithToClear);
        }

        public void Rollback()
        {
            transaction.Rollback();
            // http://blog.oneunicorn.com/2011/04/03/rejecting-changes-to-entities-in-ef-4-1/
            foreach (var entry in context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        // Note - problem with deleted entities:
                        // When an entity is deleted its relationships to other entities are severed.
                        // This includes setting FKs to null for nullable FKs or marking the FKs as conceptually null (don’t ask!)
                        // if the FK property is not nullable. You’ll need to reset the FK property values to
                        // the values that they had previously in order to re-form the relationships.
                        // This may include FK properties in other entities for relationships where the
                        // deleted entity is the principal of the relationship–e.g. has the PK
                        // rather than the FK. I know this is a pain–it would be great if it could be made easier in the future, but for now it is what it is.
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void Dispose()
        {
            if (objectContext.Connection.State == ConnectionState.Open)
            {
                objectContext.Connection.Close();
            }
        }
    }
}
