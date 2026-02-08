using System.Data.Common;

namespace OrderManagementAPI.Data.Uow.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        void Save();
        Task SaveAsync();
        void Commit();
        Task CommitAsync();
        void RollBack();
        Task RollBackAsync();
        void BeginTransaction();
        Task BeginTransactionAsync();
        List<T> RawSqlQuery<T>(string query, Func<DbDataReader, T> map, int timeOut = 30, params object[] parameters) where T : class;
        void SetCommandTimeout(int seconds);
    }
}
