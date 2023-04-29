using App.Scripts.Mics;

namespace App.Scripts.Domains.Models
{
    public enum ItemType { StrawBerry, BlueBerry, Tomato, Cow, Plot}
    public class Item
    {
        public int Id { get; set; }
        public ItemType ItemType { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public Category Category { get; set; }
        public int TimePerProduct { get; set; } // (millisecond) time for each product
        public int ProductCapacity { get; set; } // (millisecond) total product for each item could be collected

        private static ItemType ConvertShopEventType(ShareData.ShopEventType shopEventType)
        {
            return shopEventType switch
            {
                ShareData.ShopEventType.BBlueBerry => ItemType.BlueBerry,
                ShareData.ShopEventType.BTomato => ItemType.Tomato,
                ShareData.ShopEventType.BStrawBerry => ItemType.StrawBerry,
                ShareData.ShopEventType.BCow => ItemType.Cow,
            };
        }

        public static Item ConvertItemType(ItemType itemType)
        {
            return itemType switch
            {
                ItemType.BlueBerry => Define.BlueBerryItem,
                ItemType.Tomato => Define.TomatoItem,
                ItemType.StrawBerry => Define.StrawBerryItem,
                ItemType.Cow => Define.CowItem,
            };
        }

        public static Item ConvertItemType(ShareData.ShopEventType shopEventType)
        {
            var itemType = ConvertShopEventType(shopEventType);
            return ConvertItemType(itemType);
        }
    }
    
}