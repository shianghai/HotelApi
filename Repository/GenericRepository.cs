using HotelApi.Data;
using HotelApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelApi.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly HotelDbContext _context;
        private readonly DbSet<T> _db;

        public GenericRepository(HotelDbContext context)
        {
            _context = context;
            _db = _context.Set<T>();
        }
        public async void Delete(int id)
        {
            var entity = await _db.FindAsync(id);
            if (entity is not null) _db.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _db.RemoveRange(entities);
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression, List<string> includes = null)
        {
            IQueryable<T> query = _db;
            if(includes is not null)
            {
                foreach(var include in includes)
                {
                    query.Include(include);
                }
            }

            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<IList<T>> GetAll(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, Expression<Func<T, bool>> expression = null, List<string> includes = null)
        {
            IQueryable<T> query = _db;
            if(includes != null)
            {
                foreach(var include in includes) query.Include(include);
            }
            if (orderBy is not null) orderBy(query);
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<IList<T>> GetAll(Expression<Func<T, bool>> expression = null, List<string> includes = null)
        {
            IQueryable<T> query = _db;
            if (includes != null)
            {
                foreach (var include in includes) query.Include(include);
            }
            return await query.AsNoTracking().ToListAsync();
        }

        public void Insert(T entity)
        {
            _db.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async void InsertRange(IEnumerable<T> entities)
        {
            await _db.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _db.Update(entity);
        }
    }
}
