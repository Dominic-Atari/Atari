using System.Collections.Generic;
using System.Threading.Tasks;
using MyApp.Model;
using MyApp.Model.Dtos;

namespace MyApp.Service
{
    public interface IStoreService
    {
        // IStoreService is used to manage store operations from the controller and interact with the data layer from the service
        Task<IEnumerable<StoreDto>> GetAllAsync();
        Task<StoreDto?> GetByIdAsync(int id);
        Task<Store> AddAsync(StoreDto storeDto);
        Task<StoreDto?> UpdateAsync(int id, StoreDto storeDto);
        Task<bool> DeleteAsync(int id);
    }
}