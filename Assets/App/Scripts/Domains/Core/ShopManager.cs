using System.Collections.Generic;
using App.Scripts.Domains.Models;
using App.Scripts.Mics;

namespace App.Scripts.Domains.Core
{
    public class ShopManager
    {
        private readonly PlotManager _plotManager;
        private readonly StatManager _statManager;
        
        private List<Item> _cart = new();
        
        private int _amountSeedOrder = 0;

        public bool IsCartBuyable => _amountSeedOrder == 10;

        public ShopManager()
        {
            DependencyProvider.Instance.RegisterDependency(typeof(ShopManager), this);
            _statManager = DependencyProvider.Instance.GetDependency<StatManager>();
            _plotManager = DependencyProvider.Instance.GetDependency<PlotManager>();

        }

        private bool IsPickSeedable(int amount) => amount + _amountSeedOrder <= 10;
        
        
        public void OnShopUIEventRaised(ShareData.ShopUIEvent shopUIEvent)
        {
            switch (shopUIEvent.EShopUIEvent)
            {
                case ShareData.ShopEventType.BBlueBerry:
                    this.PickSeed(Item.ConvertItemType(shopUIEvent.EShopUIEvent),1);
                    break;
                case ShareData.ShopEventType.BTomato:
                    this.PickSeed(Item.ConvertItemType(shopUIEvent.EShopUIEvent),1);
                    break;
                case ShareData.ShopEventType.BStrawBerry:
                    this.PickSeed(Item.ConvertItemType(shopUIEvent.EShopUIEvent),10);
                    break;
                case ShareData.ShopEventType.BCow:
                    this.BuyCow(Item.ConvertItemType(shopUIEvent.EShopUIEvent));
                    break;
                case ShareData.ShopEventType.BPlot:
                    _plotManager.ExtendPlot(shopUIEvent.EShopUIEvent);
                    _statManager.GainItem(ItemType.Plot);
                    break;
                case ShareData.ShopEventType.BuySeedInCart:
                    this.BuySeedInCart();
                    break;
                case ShareData.ShopEventType.ReleaseSeedInCart:
                    this.ReleaseSeedInCart();
                    break;
            }
        }

        private void ReleaseSeedInCart()
        {
            _cart = new();
            _amountSeedOrder = 0;
        }

        private void PickSeed(Item item, int amount)
        {
            if (IsPickSeedable(amount) == false)
                return;
            _amountSeedOrder += amount;
            for (int i = 0; i < amount; i++)
            {
                _cart.Add(item);
            }

        }

        private void BuyCow(Item item)
        {
            if( _statManager.Gold.IsPayable(item.Price) == false)
                return;
            _statManager.Gold.Pay(item.Price);
            _statManager.GainItem(item.ItemType);
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
    }
}