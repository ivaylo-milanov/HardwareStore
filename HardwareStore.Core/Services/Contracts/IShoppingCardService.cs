namespace HardwareStore.Core.Services.Contracts
{
    using HardwareStore.Core.ViewModels.ShoppingCard;

    public interface IShoppingCardService
    {
        Task<List<ShoppingCardModel>> AddToShoppingCardAsync(List<ShoppingCardModel> shoppings, int id, int quantity);

        Task<List<ShoppingCardModel>> RemoveFromShoppingCardAsync(List<ShoppingCardModel> shoppings, int id);
    }
}
