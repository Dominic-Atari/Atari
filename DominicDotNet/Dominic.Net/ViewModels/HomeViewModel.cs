namespace Dominic.Net.Models
{
    // ViewModel for the Home page
    public class HomeViewModel
    {
       public IEnumerable<Pie> PiesOfTheWeek { get; }

        public HomeViewModel(IEnumerable<Pie> piesOfTheWeek)
        {
            PiesOfTheWeek = piesOfTheWeek;
        }
    }
}