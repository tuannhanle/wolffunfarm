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
        public string Description { get; set; }
        public Category Category { get; set; }

        public static ItemType ConvertShopEventType(ShareData.ShopEventType shopEventType)
        {
            return shopEventType switch
            {
                ShareData.ShopEventType.BBlueBerry => ItemType.BlueBerry,
                ShareData.ShopEventType.BTomato => ItemType.Tomato,
                ShareData.ShopEventType.BStrawBerry => ItemType.StrawBerry,
                ShareData.ShopEventType.BCow => ItemType.Cow,
            };
        }
    }
    
}