using Microsoft.EntityFrameworkCore;

namespace MemeSource.DAL.Interfaces
{
    /// <summary>
    /// UnitOfWork interface
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// DB Context
        /// </summary>
        DbContext Context { get; }

        /// <summary>
        /// Saves the change
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangeAsync();
    }
}
