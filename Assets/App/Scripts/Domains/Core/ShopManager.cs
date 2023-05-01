using System.Collections.Generic;
using App.Scripts.Domains.Models;
using App.Scripts.Mics;
using App.Scripts.UI;

namespace App.Scripts.Domains.Core
{
    public class ShopManager : Dependency<ShopManager>, IDependency
    {
        private Cart _cart = new();
        private const int AMOUNT_EACH_COW = 1;

        
        public void OnShopUIEventRaised(ShopEventType eShopEvent, ItemType? eItemType =null)
        {
            switch (eShopEvent)
            {
                case ShopEventType.Buy:
                    this.PickItem(eItemType);
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

        private void PickItem(ItemType? eItemType)
        {
            if (eItemType == null)
                return;
            if (eItemType == ItemType.UnusedPlot)
            {
                _plotManager.ExtendPlot();
            }
            else if (eItemType == ItemType.Cow)
            {
                this.BuyCow();
            }
            else
            {
                this.PickSeed(eItemType);
            }
        }
        
        private void PickSeed(ItemType? eItemType)
        {
            var amount = eItemType == ItemType.StrawBerry ? 10 : 1;
            var item = Item.ConvertItemType(eItemType);
            if (_cart.IsPickSeedable(amount) == false)
                return;
            for (int i = 0; i < amount; i++)
            {
                _cart.Pick(item);
            }
        }

        private void BuyCow()
        {
            var cowItem = Define.CowItem;
            if(_paymentService.Buy(cowItem))
                _statManager.Gain(cowItem.ItemType, AMOUNT_EACH_COW);
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