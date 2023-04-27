using System.Collections.Generic;
using App.Scripts.Domains.Models;
using App.Scripts.Mics;
using UnityEditor.Experimental.GraphView;

namespace App.Scripts.Domains.Core
{
    public class ShopManager
    {
        public PlotManager _plotManager;
        private int _amountSeedOrder = 0;

        private Item _blueBerry = new (){Name = ItemType.BlueBerry, Price = 50 };
        private Item _tomato = new (){Name = ItemType.Tomato, Price = 30};
        private Item _strawBerry = new (){Name = ItemType.StrawBerry , Price= 40};
        private Item _cow = new (){Name = ItemType.Cow , Price= 100};
        private List<Item> _items;
        private Dictionary<ItemType, int> _priceMap = new();
        public ShopManager(PlotManager plotManager)
        {
            _plotManager = plotManager;
            _items = new List<Item>(){_blueBerry, _tomato, _strawBerry, _cow};
            foreach (var item in _items)
            {
                _priceMap.Add(item.Name, item.Price);
            }
        }
        
        private bool IsPickSeedable(int amount)
        {
            return amount + _amountSeedOrder <= 10;
        }
        public void OnShopUIEventRaised(ShareData.ShopUIEvent shopUIEvent)
        {
            if (shopUIEvent.EShopUIEvent == null)
                return;
            
            switch (shopUIEvent.EShopUIEvent)
            {
                case ShareData.ShopEventType.BBlueBerry:
                    this.PickSeed(shopUIEvent.EShopUIEvent,1);
                    break;
                case ShareData.ShopEventType.BTomato:
                    this.PickSeed(shopUIEvent.EShopUIEvent,1);
                    break;
                case ShareData.ShopEventType.BStrawBerry:
                    this.PickSeed(shopUIEvent.EShopUIEvent,10);
                    break;
                case ShareData.ShopEventType.BCow:
                    // this.BuySeed(shopUIEvent.EShopUIEvent);
                    break;
                case ShareData.ShopEventType.BPlot:
                    _plotManager.ExtendPlot(shopUIEvent.EShopUIEvent);
                    break;
                default:
                    break;
            }
        }
        
        public void PickSeed(ShareData.ShopEventType shopEvent, int amount)
        {
            if (IsPickSeedable(amount) == false)
                return;
            if (shopEvent == ShareData.ShopEventType.BBlueBerry)
            {
                
            }
        }
    }
}