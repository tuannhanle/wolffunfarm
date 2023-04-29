namespace App.Scripts.Domains.Models
{
    public class Crop
    {
        public Item Item { get; set; }


        public Crop(Item item)
        {
            Item = item;

        }

    }
}