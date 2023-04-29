using System;

namespace App.Scripts.Domains.Models
{
    public class Tool : Stuff
    {
        public int? Level { get;  set; }
        public float Percent { get; private set; } = 10;
        public void UpLevel() => Level++;
        public float GetPercentPerLevel => 100f + (Level??1 - 1f) * Percent;

        public Tool(int amount = 500) : base(amount)
        {
            Price = amount;
        }
    }
}