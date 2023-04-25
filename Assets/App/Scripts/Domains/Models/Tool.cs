namespace App.Scripts.Domains.Models
{
    public class Tool
    {
        public int Level { get; set; } = 1;
        public int Price { get; set; } = 500;
        public int Percent { get; set; } = 10;

        public void UpLevel()
        {
            Level++;
        }
    }
}