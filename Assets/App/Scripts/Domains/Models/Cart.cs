using System.Collections.Generic;
using App.Scripts.Domains.Core;
using App.Scripts.Domains.Services;

namespace App.Scripts.Domains.Models
{
    public class Cart : IBuyable
    {
        public int Price { get; set; }

        public bool IsCartBuyable => _amountSeedOrder == 10;


        private List<Item> _items = new();
        private int _amountSeedOrder = 0;

        private const int AMOUNT_EACH_ITEM = 1;

        public void Pick(Item item)
        {
            _items.Add(item);
            Price += item.Price;
        }
        
        public bool IsPickSeedable(int amount)
        {
            if (amount + _amountSeedOrder <= 10)
            {
                _amountSeedOrder += amount;
                return true;
            }
            return false;
        }

        public void StorageItems()
        {
            var statManager = DependencyProvider.Instance.GetDependency<StatManager>();
            foreach (var item in _items)
            {
                statManager.GainUsing(item.ItemType, AMOUNT_EACH_ITEM );
            }
        }
    }
}