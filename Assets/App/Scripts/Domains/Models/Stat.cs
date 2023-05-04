namespace App.Scripts.Domains.Models
{
    public class Stat
    {
        public long GoldAmount;
        public int ToolLevel;
        public int PercentPerLevel;
        public int JobDuration;
        public int ExtentTimeToDestroy;
        public int ToolPrice;
        
        
        public Stat(){}

        public Stat(long g, int t, int p, int j, int e, int tp)
        {
            GoldAmount = g;
            ToolLevel = t;
            PercentPerLevel = p;
            JobDuration = j;
            ExtentTimeToDestroy = e;
            ToolPrice = tp;
        }
    }
}