using Microsoft.EntityFrameworkCore;

namespace Nile.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly NileDbContext _db;

        public GenericRepository(NileDbContext db)
        {
            _db = db;
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _db.Set<T>().FindAsync(id);
        }

        public virtual async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _db.Set<T>().ToListAsync();
        }

        public virtual async Task AddAsync(T entity)
        {
            await _db.Set<T>().AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _db.Set<T>().AddRangeAsync(entities);
        }

        public virtual void Update(T entity)
        {
            _db.Set<T>().Update(entity);
        }

        public virtual void Remove(T entity)
        {
            _db.Set<T>().Remove(entity);
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
