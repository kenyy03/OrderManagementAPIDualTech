namespace OrderManagementAPI.Data.Uow.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IQueryable<T> AsQueryable();
        Task<T?> FindByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(params T[] entities);
        Task Delete(int id);
        Task Delete(Guid id);
        void Update(T entity);
        void RemoveRange(params T[] entities);
    }
}
