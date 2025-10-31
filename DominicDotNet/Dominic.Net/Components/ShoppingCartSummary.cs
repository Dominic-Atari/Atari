using Dominic.Net.Models;
using Dominic.Net.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dominic.Net.Components
{
    //main purpose of this component is to show a summary of the shopping cart in the layout view
    public class ShoppingCartSummary: ViewComponent
    {
        private readonly IShoppingCart _shoppingCart;

        public ShoppingCartSummary(IShoppingCart shoppingCart)
        {
            _shoppingCart = shoppingCart;
        }

        public IViewComponentResult Invoke()
        {
            // Get the actual shopping cart items from the database
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            var shoppingCartViewModel = new ShoppingCartViewModel(_shoppingCart, _shoppingCart.GetShoppingCartTotal());

            return View(shoppingCartViewModel);
        }
    }
}
