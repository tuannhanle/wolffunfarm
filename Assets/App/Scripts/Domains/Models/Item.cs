using App.Scripts.Domains.Core;
using App.Scripts.Domains.Services;

namespace App.Scripts.Domains.Models
{

    [System.Serializable]
    public class Item :  IBuyable
    {
        public string ItemName = null;
        public int Price { get; set; }
        public int BuyUnit;

        public int Stock;

        public int TimePerProduct; // (second) time for each product
        public int ProductCapacity; // (second) total product for each item could be collected

        public bool IsSeedingable;
        public bool IsAnimal;
        public bool IsBuyInShop;
        public Item(){}
        public Item(string itemName, int price, int stock, int time, int productCapacity, bool isSeedingable, bool isAnimal, bool isBuyInShop, int buyUnit)
        {
            ItemName = itemName;
            Price = price;
            Stock = stock;
            TimePerProduct = time;
            ProductCapacity = productCapacity;
            IsSeedingable = isSeedingable;
            IsAnimal = isAnimal; 
            IsBuyInShop = isBuyInShop;
            BuyUnit = buyUnit;
        }

    }
    
}