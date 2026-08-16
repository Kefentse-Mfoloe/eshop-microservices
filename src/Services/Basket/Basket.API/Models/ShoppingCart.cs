namespace Basket.API.Models;

public class ShoppingCart
{
    public string UserName { get; set; } = default!;
    public List<ShoppingCartItem> Items { get; set; } = new();

    public ShoppingCart(string userName)
    {

    }

    // Required for mapping
    public ShoppingCart()
    {
    }
}