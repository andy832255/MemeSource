using MemeSource.Models;

namespace MemeSource.DAL.Interfaces
{
    public interface ISystemPropertyRepository
    {
        Task<IEnumerable<SystemProperty>> GetAllAsync();
        Task<SystemProperty> GetByIdAsync(int id);
        Task AddAsync(SystemProperty systemProperty);
        Task UpdateAsync(SystemProperty systemProperty);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
