using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyApp.Model;
using MyApp.Data;
using MyApp.Model.Dtos;

namespace MyApp.Service.Sirvices
{
    public class StoreService : IStoreService
    {
        private readonly AppDbContext _context;
        public StoreService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StoreDto>> GetAllAsync()
        {
            return await _context.Stores
                .Select(s => new StoreDto { Name = s.Name, Location = s.Location })
                .ToListAsync();
        }

        public async Task<StoreDto?> GetByIdAsync(int id)
        {
            var store = await _context.Stores.FindAsync(id);
            if (store == null) return null;
            return new StoreDto { Name = store.Name, Location = store.Location };
        }

        public async Task<Store> AddAsync(StoreDto storeDto)
        {
            var store = new Store { Name = storeDto.Name, Location = storeDto.Location };
            _context.Stores.Add(store);
            await _context.SaveChangesAsync();
            return store;
        }

        public async Task<StoreDto?> UpdateAsync(int id, StoreDto storeDto)
        {
            var store = await _context.Stores.FindAsync(id);
            if (store == null) return null;
            store.Name = storeDto.Name;
            store.Location = storeDto.Location;
            await _context.SaveChangesAsync();
            return new StoreDto { Name = store.Name, Location = store.Location };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var store = await _context.Stores.FindAsync(id);
            if (store == null) return false;
            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
