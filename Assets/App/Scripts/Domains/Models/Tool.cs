using System;
using App.Scripts.Domains.Services;

namespace App.Scripts.Domains.Models
{
    public class Tool : Unit, IBuyable
    {
        public int? Level { get; set; } = 1;
        public float Percent { get; private set; } = 10;
        public void UpLevel() => Level++;
        public float GetPercentPerLevel => 100f + (Level??1 - 1f) * Percent;

        public Tool(int priceAmount = 500) : base(priceAmount)
        {
            Price = priceAmount;
        }

        public int Price { get; set; }
    }
}