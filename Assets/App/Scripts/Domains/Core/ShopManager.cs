using App.Scripts.Domains.Models;
using App.Scripts.Mics;
using App.Scripts.UI;

namespace App.Scripts.Domains.Core
{
    public class ShopManager : Dependency<ShopManager>, IDependency
    {
        private Cart _cart = new();
        private LazyDataInlet<ShareData.CartEvent> _cartEventInlet = new();

        public void OnShopUIEventRaised(ShopEventType eShopEvent, string itemName =null)
        {
            switch (eShopEvent)
            {
                case ShopEventType.Buy:
                    this.PickItem(itemName);
                    break;
                case ShopEventType.BuyCart:
                    this.BuySeedInCart();
                    break;
                case ShopEventType.RenewCart:
                    this.RenewCart();
                    break;
            }
        }

        private void RenewCart()
        {
            _cart = new();
        }

        private void PickItem(string itemName)
        {
            if (itemName == null || itemName.Equals(""))
                return;
            if (_dataLoader.ItemCollection.TryGetValue(itemName, out var item) == false)
                return;

            if (itemName.Equals(Define.PLOT))  
                _plotManager.ExtendPlot();
            else
            {
                if (item.IsAnimal)  
                    this.BuyCow(itemName);
                else
                    this.PickSeed(itemName);
            }

        }
        
        private void PickSeed(string itemName)
        {
            if (_dataLoader.ItemCollection.TryGetValue(itemName, out var item) == false)
                return;
            if (_cart.IsPickSeedable(item.BuyUnit) == false)
                return;
            for (int i = 0; i < item.BuyUnit; i++) { _cart.Pick(item); }
            _cartEventInlet.UpdateValue(new ShareData.CartEvent()
            {
                itemNamePicked = itemName,
                amountPick = item.BuyUnit,
                amountTotalOrder = _cart.GetAmountSeedOrdered
            });
        }

        private void BuyCow(string itemName)
        {
            var cowItem = _dataLoader.ItemCollection[itemName];
            if (_paymentService.Buy(cowItem) == false)
                return;
            if (_dataLoader.ItemStorage.TryGetValue(itemName, out var itemStorage) == false)
                return;
            itemStorage.UnusedAmount++;
            _dataLoader.Push<ItemStorage>();
        }

        private void BuySeedInCart()
        {
            if (_cart.IsCartBuyable == false)
                return; 
            var isPayable = _paymentService.Buy(_cart);
            if (isPayable == false)
                return;
            _cartEventInlet.UpdateValue(new ShareData.CartEvent() { isBuy = true });
            _cart.StorageItems();
            RenewCart();
        }
    }
}