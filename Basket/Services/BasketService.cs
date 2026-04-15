using Basket.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Services;

public class BasketService(IDistributedCache cache)
{
    public async Task<ShoppingCart?> GetBasket(string userName)
    {
        var basketData = await cache.GetStringAsync(userName);
        return string.IsNullOrEmpty(basketData) ? null : JsonSerializer.Deserialize<ShoppingCart>(basketData);
    }

    public async Task UpdateBasket(ShoppingCart basket)
    {
        var basketData = JsonSerializer.Serialize(basket);
        await cache.SetStringAsync(basket.UserName, basketData);
    }
    public async Task DeleteBasket(string userName)
    {
        await cache.RemoveAsync(userName);
    }
}
