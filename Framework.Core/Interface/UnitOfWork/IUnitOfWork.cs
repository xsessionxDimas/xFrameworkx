using System;

namespace Framework.Core.Interface.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void Rollback();
        void SaveChanges();
        void AutoDetectChangesEnabled(bool option);
        void LazyLoadingEnabled(bool option);
    }
}
