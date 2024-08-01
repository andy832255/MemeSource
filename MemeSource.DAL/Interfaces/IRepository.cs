using System.Linq.Expressions;

namespace MemeSource.DAL.Interfaces
{
    /// <summary>
    /// 泛型Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// 取得全部
        /// </summary>
        /// <param name="filter">查詢條件</param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        /// <summary>
        /// 取得單筆
        /// </summary>
        /// <param name="filter">查詢條件</param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task<T> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null);
        void Add(T entity);
        void Remove(T entity);
        //void RemoveRange(IEnumerable<T> entity);
        void Update(T entity);
    }
}
