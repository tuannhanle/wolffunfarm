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

        public int GetAmountSeedOrdered => _amountSeedOrder;
        
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
            var dataloader = DependencyProvider.Instance.GetDependency<DataLoader>();
            var itemStorage =dataloader.ItemStorage;
            foreach (var item in _items)
            {
                itemStorage[item.ItemName].UnusedAmount++;
            }
            dataloader.Push<ItemStorage>();
        }
    }
}