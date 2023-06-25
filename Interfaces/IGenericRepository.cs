using System.Collections;
using System.Linq.Expressions;

namespace HotelApi.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IList<T>> GetAll(
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            Expression<Func<T, bool>> expression = null,
            List<string> includes = null
        );

        Task<IList<T>> GetAll(
            Expression<Func<T, bool>> expression = null,
            List<string> includes = null
            );

        Task<T> Get(Expression<Func<T, bool>> expression, List<string> includes = null);

        void Insert(T entity);

        void InsertRange(IEnumerable<T> entities);

        void Update(T entity);

        void DeleteRange(IEnumerable<T> entities);

        void Delete(int id);
    }
}
