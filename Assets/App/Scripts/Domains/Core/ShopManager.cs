using App.Scripts.Domains.Models;
using App.Scripts.UI;

namespace App.Scripts.Domains.Core
{
    public class ShopManager : Dependency<ShopManager>, IDependency
    {
        private Cart _cart = new();

        public void OnShopUIEventRaised(ShopEventType eShopEvent, string itemName =null)
        {
            switch (eShopEvent)
            {
                case ShopEventType.Buy:
                    this.PickItem(itemName);
                    break;
                case ShopEventType.BuySeedInCart:
                    this.BuySeedInCart();
                    break;
                case ShopEventType.ReleaseSeedInCart:
                    this.ReleaseSeedInCart();
                    break;
            }
        }

        private void ReleaseSeedInCart()
        {
            _cart = new();
        }

        private void PickItem(string itemName)
        {
            if (itemName == null || itemName.Equals(""))
                return;
            if (_dataLoader.ItemCollection.TryGetValue(itemName, out var item))
            {
                if (itemName.Equals("Plot"))  
                    _plotManager.ExtendPlot();            
                else if (itemName.Equals("Cow"))  
                    this.BuyCow(itemName);
                else
                    this.PickSeed(itemName);
            }
        }
        
        private void PickSeed(string itemName)
        {
            if (_dataLoader.ItemCollection.TryGetValue(itemName, out var item))
            {
                if (_cart.IsPickSeedable(item.BuyUnit) == false)
                    return;
                for (int i = 0; i < item.BuyUnit; i++)
                {
                    _cart.Pick(item);
                }
            }
        }

        private void BuyCow(string itemName)
        {
            var cowItem = _dataLoader.ItemCollection[itemName];
            if (_paymentService.Buy(cowItem))
            {
                if (_dataLoader.ItemStorage.TryGetValue(itemName, out var itemStorage))
                {
                    itemStorage.UnusedAmount++;
                    _dataLoader.Push<ItemStorage>();
                }
            }
            // {
            //     _workerManager.Assign(JobType.PutIn, itemName);
            // }
        }

        private void BuySeedInCart()
        {
            if (_cart.IsCartBuyable == false)
                return; //TODO: notify
            var isPayable = _paymentService.Buy(_cart);
            if (isPayable)
            {
                _cart.StorageItems();
                _cart = new();
            }
        }
    }
}