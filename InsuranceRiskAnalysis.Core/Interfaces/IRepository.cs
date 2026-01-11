using InsuranceRiskAnalysis.Core.Entities;
using System.Linq.Expressions;

namespace InsuranceRiskAnalysis.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity); // Async olmasa da Task döndürmek pattern gereği iyidir
        Task DeleteAsync(T entity);
    }
}
