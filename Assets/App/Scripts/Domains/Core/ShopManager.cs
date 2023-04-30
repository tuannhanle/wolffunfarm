using System.Collections.Generic;
using App.Scripts.Domains.Models;
using App.Scripts.Mics;
using App.Scripts.UI;

namespace App.Scripts.Domains.Core
{
    public class ShopManager : Dependency<ShopManager>
    {

        private List<Item> _cart = new();
        
        private int _amountSeedOrder = 0;

        public bool IsCartBuyable => _amountSeedOrder == 10;

        private bool IsPickSeedable(int amount) => amount + _amountSeedOrder <= 10;
        
        
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
            _amountSeedOrder = 0;
        }

        private void PickItem(ItemType? eItemType)
        {
            if (eItemType == ItemType.Plot)
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
            if (eItemType == null)
                return;
            var amount = eItemType == ItemType.BlueBerry ? 10 : 1;
            var item = Item.ConvertItemType(eItemType);
            if (item == null)
                return;
            if (IsPickSeedable(amount) == false)
                return;
            _amountSeedOrder += amount;
            for (int i = 0; i < amount; i++)
            {
                _cart.Add(item);
            }

        }

        private void BuyCow()
        {
            var cowItem = Define.CowItem;
            if( _statManager.Gold.IsPayable(cowItem.Price) == false)
                return;
            _statManager.Gold.Pay(cowItem.Price);
            _statManager.GainItem(cowItem.ItemType);
        }

        private void BuySeedInCart()
        {
            if (IsCartBuyable == false)
                return; //TODO: notify
            var sumPrice = 0;
            foreach (var item in _cart)
            {
                sumPrice += item.Price;
            }
            if( _statManager.Gold.IsPayable(sumPrice) == false)
                return;
            _statManager.Gold.Pay(sumPrice);
            foreach (var item in _cart)
            {
                _statManager.GainItem(item.ItemType);
            }
            _cart = new();
            _amountSeedOrder = 0;

        }

        public void Init()
        {
            throw new System.NotImplementedException();
        }
    }
}