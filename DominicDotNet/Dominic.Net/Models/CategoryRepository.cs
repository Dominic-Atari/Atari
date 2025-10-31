namespace Dominic.Net.Models
{
    public class CategoryRepository : ICategoryRepository
    {
       private readonly DominicShopDbContext _DominicShopDbContext;

        public CategoryRepository(DominicShopDbContext DominicShopDbContext)
        {
            _DominicShopDbContext = DominicShopDbContext;
        }

        public IEnumerable<Category> AllCategories => _DominicShopDbContext.Categories.OrderBy(p => p.CategoryName);
    }
}
