using Microsoft.EntityFrameworkCore;
using Services.Context;
using Services.Models;

namespace Services.Repositories
{
    // --- Interface ---
    public interface ITrashRepository
    {
        Task<IEnumerable<Trash>> GetAllAsync();
        Task<Trash?> GetByIdAsync(int id);
        Task AddAsync(Trash trash);
        Task UpdateAsync(Trash trash);
        Task DeleteAsync(int id);
    }

    // --- Implementasi ---
    public class TrashRepository : ITrashRepository
    {
        private readonly DataContext _context;

        public TrashRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Trash>> GetAllAsync()
            => await _context.Trashs.OrderBy(p => p.TrashNID).ToListAsync();

        public async Task<Trash?> GetByIdAsync(int id)
            => await _context.Trashs.FindAsync(id);

        public async Task AddAsync(Trash trash)
        {
            _context.Trashs.Add(trash);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Trash trash)
        {
            _context.Trashs.Update(trash);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var p = await _context.Trashs.FindAsync(id);
            if (p is null) return;
            _context.Trashs.Remove(p);
            await _context.SaveChangesAsync();
        }
    }
}
