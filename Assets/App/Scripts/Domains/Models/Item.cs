using System;
using App.Scripts.Domains.Services;
using App.Scripts.Mics;

namespace App.Scripts.Domains.Models
{
    public enum ItemType { 
        StrawBerry, BlueBerry, Tomato, Cow, 
        UnusedPlot, UsingPlot
    }
    public class Item : IBuyable
    {
        public int Name { get; set; }
        public ItemType ItemType { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public Category Category { get; set; }


        public long TimePerProduct { get; set; } // (second) time for each product
        public int ProductCapacity { get; set; } // (second) total product for each item could be collected
        

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