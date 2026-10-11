namespace Basket.API.Data
{
    public class CachedBasketRespository(IBasketRepository repository) : IBasketRepository
    {
        public Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken)
        {
            return repository.DeleteBasket(userName, cancellationToken);
        }

        public Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken)
        {
            return repository.GetBasket(userName, cancellationToken);
        }

        public Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken)
        {
            return repository.StoreBasket(basket, cancellationToken);
        }
    }
}
