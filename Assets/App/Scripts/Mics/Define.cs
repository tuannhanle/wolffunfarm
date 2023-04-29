using App.Scripts.Domains.Models;

namespace App.Scripts.Mics
{
    public static class Define
    {
        public static readonly Item BlueBerryItem = new ()
        {
            ItemType = ItemType.BlueBerry, 
            Price = 50,
            TimePerProduct = 15*60,
            ProductCapacity = 40,
        };
        public static readonly Item TomatoItem = new ()
        {
            ItemType = ItemType.Tomato, 
            Price = 30,
            TimePerProduct = 10*60,
            ProductCapacity = 40,
        };
        public static readonly Item StrawBerryItem = new ()
        {
            ItemType = ItemType.StrawBerry , 
            Price= 40,
            TimePerProduct = 5*60,
            ProductCapacity = 20,
        };
        public static readonly Item CowItem = new ()
        {
            ItemType = ItemType.Cow , 
            Price= 100,
            TimePerProduct = 30*60,
            ProductCapacity = 100,
        };

    }
}