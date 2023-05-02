using System;
using App.Scripts.Domains.Core;
using App.Scripts.Domains.Services;
using App.Scripts.Mics;

namespace App.Scripts.Domains.Models
{
    public enum ItemType { 
        StrawBerry, BlueBerry, Tomato, Cow, 
        UnusedPlot, UsingPlot
    }
    [System.Serializable]
    public class Item : IBuyable, IHasItemName
    {
        public string ItemName { get; set; }
        public ItemType ItemType;
        public int Price { get; set; }
        public int Stock;
        public Category Category;

        public int TimePerProduct; // (second) time for each product
        public int ProductCapacity; // (second) total product for each item could be collected

        public Item(){}
        public Item(string itemName, int price, int stock, int time, int productCapacity)
        {
            ItemName = itemName;
            Price = price;
            Stock = stock;
            TimePerProduct = time;
            ProductCapacity = productCapacity;
        }

        public static Item ConvertItemType(ItemType? eItemType)
        {
            if (eItemType == null)
                return null;
            return eItemType switch
            {
                ItemType.BlueBerry => Define.BlueBerryItem,
                ItemType.Tomato => Define.TomatoItem,
                ItemType.StrawBerry => Define.StrawBerryItem,
                ItemType.Cow => Define.CowItem
            };
        }

    }
    
}