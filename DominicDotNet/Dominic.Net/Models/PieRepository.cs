
using Microsoft.EntityFrameworkCore;

namespace Dominic.Net.Models
{
        public class PieRepository : IPieRepository
        {
             private readonly DominicShopDbContext _DominicShopDbContext;

        public PieRepository(DominicShopDbContext DominicShopDbContext)
        {
            _DominicShopDbContext = DominicShopDbContext;
        }

        public IEnumerable<Pie> AllPies
        {
            get
            {
                return _DominicShopDbContext.Pies.Include(c => c.Category);
            }
        }

        public IEnumerable<Pie> PiesOfWeek
        {
            get
            {
                return _DominicShopDbContext.Pies.Include(c => c.Category).Where(p => p.IsPieOfTheWeek);
            }
        }

        public Pie? GetPieById(int pieId)
        {
            return _DominicShopDbContext.Pies.FirstOrDefault(p => p.PieId == pieId);
        }

        public IEnumerable<Pie> SearchPies(string searchQuery)
        {
            throw new NotImplementedException();
        }
    }
}
