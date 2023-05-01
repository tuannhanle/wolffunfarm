using App.Scripts.Domains.GameObjects;
using App.Scripts.Domains.Models;

namespace App.Scripts.Mics
{
    public static class Define
    {
        public static Item BlueBerryItem => new ()
        {
            ItemType = ItemType.BlueBerry, 
            Price = 50,
            Stock = 8,
            TimePerProduct = 15*60,
            ProductCapacity = 40,
        };
        public static Item TomatoItem => new ()
        {
            ItemType = ItemType.Tomato, 
            Price = 30,
            Stock = 5,
            TimePerProduct = 10*60,
            ProductCapacity = 40,
        };
        public static Item StrawBerryItem => new ()
        {
            ItemType = ItemType.StrawBerry , 
            Price= 40,
            Stock = 6,
            TimePerProduct = 5*60,
            ProductCapacity = 20,
        };
        public static Item CowItem => new ()
        {
            ItemType = ItemType.Cow , 
            Price= 100,
            Stock = 15,
            TimePerProduct = 30*60,
            ProductCapacity = 100,
        };

        public static  Plot PlotItem => new(COMMON_PRICE);

        public static Worker WorkerItem => new(COMMON_PRICE);
        
        private const int COMMON_PRICE = 500;


    }
}