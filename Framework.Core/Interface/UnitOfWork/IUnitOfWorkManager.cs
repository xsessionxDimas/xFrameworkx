namespace Framework.Core.Interface.UnitOfWork
{
    public interface IUnitOfWorkManager
    {
        IUnitOfWork NewUnitOfWork();
    }
}
