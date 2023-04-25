namespace App.Scripts.Domains.Models
{
    public class Crop
    {
        public string Name { get; set; }
        public int DaysToHarvest { get; set; }
        public int SellPrice { get; set; }
        public int BuyPrice { get; set; }

        public Crop(string name, int daysToHarvest, int sellPrice, int buyPrice)
        {
            Name = name;
            DaysToHarvest = daysToHarvest;
            SellPrice = sellPrice;
            BuyPrice = buyPrice;
        }
    }
}