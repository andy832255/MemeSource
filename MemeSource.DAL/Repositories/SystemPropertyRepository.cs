using MemeSource.DAL.Interfaces;
using MemeSource.Models;
using Microsoft.EntityFrameworkCore;
using MemeRepository.Db.Models;

namespace MemeSource.Repositories
{
    public class SystemPropertyRepository : ISystemPropertyRepository
    {
        private readonly MemeRepositoryContext _context;

        public SystemPropertyRepository(MemeRepositoryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SystemProperty>> GetAllAsync()
        {
            return await _context.SystemProperty.ToListAsync();
        }

        public async Task<SystemProperty> GetByIdAsync(int id)
        {
            return await _context.SystemProperty.FindAsync(id);
        }

        public async Task AddAsync(SystemProperty systemProperty)
        {
            await _context.SystemProperty.AddAsync(systemProperty);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SystemProperty systemProperty)
        {
            _context.SystemProperty.Update(systemProperty);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var systemProperty = await _context.SystemProperty.FindAsync(id);
            if (systemProperty != null)
            {
                _context.SystemProperty.Remove(systemProperty);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.SystemProperty.AnyAsync(e => e.ID == id);
        }

        public ISystemProperty GetTokenAsync(string name)
        {
            return _context.SystemProperty.Where(e => e.SP_Name == name).FirstOrDefault();
        }
    }
}
