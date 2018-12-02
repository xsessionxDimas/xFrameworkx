using System.Data.Entity;
using Framework.Core.Interface.Repository;
using Framework.Core.Interface.UnitOfWork;

namespace Framework.Repository.UnitOfWork
{
    public class UnitOfWorkManager : IUnitOfWorkManager
    {
        private bool isDisposed;
        private readonly DbContext context;
        private readonly IRepository<Core.Model.AuditTrail, int> auditRepository;

        public UnitOfWorkManager(DbContext context, IRepository<Core.Model.AuditTrail, int> auditRepository)
        {
            this.context         = context;
            this.auditRepository = auditRepository;
        }

        /// <summary>
        /// Provides an instance of a unit of work. This wrapping in the manager
        /// class helps keep concerns separated
        /// </summary>
        /// <returns></returns>
        public IUnitOfWork NewUnitOfWork()
        {
            return new UnitOfWork(context, auditRepository);
        }

        /// <summary>
        /// Make sure there are no open sessions.
        /// In the web app this will be called when the injected UnitOfWork manager
        /// is disposed at the end of a request.
        /// </summary>
        public void Dispose()
        {
            if (isDisposed) return;
            context.Dispose();
            isDisposed = true;
        }
    }
}
