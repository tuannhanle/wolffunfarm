using App.Scripts.Domains.Core;

namespace App.Scripts.Domains.Models
{
    public class ItemStorage 
    {
        public string ItemName;
        public int UsingAmount;
        public int UnusedAmount;
        public int ProductAmount;
        
        public ItemStorage(){}

        public ItemStorage(string itemName, int usingAmount, int unusedAmount, int productAmount)
        {
            ItemName = itemName;
            UsingAmount = usingAmount;
            UnusedAmount = unusedAmount;
            ProductAmount = productAmount;
        }

    }
}