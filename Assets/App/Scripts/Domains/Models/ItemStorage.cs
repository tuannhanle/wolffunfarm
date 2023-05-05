using App.Scripts.Domains.Core;

namespace App.Scripts.Domains.Models
{
    public class ItemStorage 
    {
        public string ItemName;
        public int UnusedAmount;
        public int HarvestedProduct;
        
        public ItemStorage(){}

        public ItemStorage(string itemName, int usingAmount, int unusedAmount, int harvestedProduct)
        {
            ItemName = itemName;
            UnusedAmount = unusedAmount;
            HarvestedProduct = harvestedProduct;
        }

    }
}