namespace App.Scripts.Domains.Models
{
    public enum ItemType { StrawBerry, BlueBerry, Tomato, Cow}
    public class Item
    {
        public int Id { get; set; }
        public ItemType Name { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
    }
}