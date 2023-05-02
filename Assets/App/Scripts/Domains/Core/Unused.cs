namespace App.Scripts.Domains.Core
{
    public class Unused : IHasItemName
    {
        public string ItemName { get; set; }
        public int Amount;
        
        public Unused() { }

        public Unused(string itemName, int amount)
        {
            ItemName = itemName;
            Amount = amount;
        }

    }
    public class Using: IHasItemName
    {
        public string ItemName { get; set; }
        public int Amount;
        
        public Using() { }

        public Using(string itemName, int amount)
        {
            ItemName = itemName;
            Amount = amount;
        }
        
    }
}