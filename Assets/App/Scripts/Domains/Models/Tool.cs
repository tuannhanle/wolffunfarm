using App.Scripts.Domains.Services;

namespace App.Scripts.Domains.Models
{
    public class Tool : IBuyable
    {
        public int Level { get; set; } = 1;
        public float Percent { get; private set; } = 10;
        public void UpLevel() => Level++;

        public float GetPercentPerLevel()
        {
            return 100f + (Level- 1) * Percent;
        }

        public int Price { get; set; }
    }
}