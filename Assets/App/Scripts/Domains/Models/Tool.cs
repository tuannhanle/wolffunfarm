namespace App.Scripts.Domains.Models
{
    public class Tool : IBuyable
    {
        public int? Level { get; private set; }
        public float Percent { get; private set; } = 10;
        public void UpLevel() => Level++;
        public float GetPercentPerLevel => 100f + (Level??1 - 1f) * Percent;
        public static int Price { get; private set; } = 500;
    }
}