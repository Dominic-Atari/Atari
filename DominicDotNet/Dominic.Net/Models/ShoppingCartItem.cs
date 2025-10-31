namespace Dominic.Net.Models
{
    public class ShoppingCartItem
    {
        public int ShoppingCartItemId { get; set; }
        public int Amount { get; set; }
        public string ShoppingCartId { get; set; } = string.Empty; // Foreign key from ShoppingCart. each item is linked to a specific cart.
        public Pie Pie { get; set; } = default!;
    }   
}