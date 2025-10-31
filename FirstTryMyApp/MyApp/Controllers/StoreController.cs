using Microsoft.AspNetCore.Mvc;
using MyApp.Data;
using MyApp.Model;
using MyApp.Model.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApp.Service;

namespace MyApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        // GET: api/store
        [HttpGet]
        public async Task<IActionResult> GetStores()
        {
            var stores = await _storeService.GetAllAsync();
            return Ok(stores);
        }

        // GET: api/store/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStoreById(int id)
        {
            var store = await _storeService.GetByIdAsync(id);
            if (store == null)
                return NotFound();
            return Ok(store);
        }

        // POST: api/store
        [HttpPost]
        public async Task<IActionResult> PostStore(StoreDto storeDto)
        {
            var created = await _storeService.AddAsync(storeDto);
            return CreatedAtAction(nameof(GetStoreById), new { id = created.Id }, created);
        }

        // PUT: api/store/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStore(int id, StoreDto storeDto)
        {
            var updated = await _storeService.UpdateAsync(id, storeDto);
            if (updated == null) return NotFound();
            return NoContent();
        }

        // DELETE: api/store/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(int id)
        {
            var deleted = await _storeService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
