namespace App.Scripts.Domains.Models
{
    public class Crop
    {
        public Item Item { get; set; }
        public int DaysToHarvest { get; set; }
        public int SellPrice { get; set; }
        public int BuyPrice { get; set; }

        public Crop(Item item, int daysToHarvest, int sellPrice, int buyPrice)
        {
            Item = item;
            DaysToHarvest = daysToHarvest;
            SellPrice = sellPrice;
            BuyPrice = buyPrice;
        }

        public Crop()
        {
        }
    }
}