using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Data.Uow.Interfaces;

namespace OrderManagementAPI.Data.Uow
{
    public class Repository<T>(DbContext context) : IRepository<T> where T : class
    {
        private readonly DbContext _context = context;
        private readonly DbSet<T> _entities = context.Set<T>();

        public async Task<T> AddAsync(T entity)
        {
            var entityAdded = await _entities.AddAsync(entity);
            return entityAdded.Entity;
        }

        public async Task AddRangeAsync(params T[] entities)
        {
            await _entities.AddRangeAsync(entities);
        }

        public IQueryable<T> AsQueryable() => _entities;

        public async Task Delete(int id)
        {
            var entity = await FindByIdAsync(id);
            if (entity is not null)
            {
                _entities.Remove(entity);
            }
        }

        public async Task Delete(Guid id)
        {
            var entity = await FindByIdAsync(id);
            if (entity is not null)
            {
                _entities.Remove(entity);
            }
        }

        public async Task<T?> FindByIdAsync(int id) => await _entities.FindAsync(id);
        public async Task<T?> FindByIdAsync(Guid id) => await _entities.FindAsync(id);

        public IEnumerable<T> GetAll() => _entities.AsEnumerable();

        public void RemoveRange(params T[] entities)
        {
            _entities.RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _entities.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
