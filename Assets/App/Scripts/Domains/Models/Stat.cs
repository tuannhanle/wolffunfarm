namespace App.Scripts.Domains.Models
{
    public class Stat
    {
        public long GoldAmount;
        public int ToolLevel;
        public int PercentPerLevel;
        public int JobDuration;
        public int ExtentTimeToDestroy;
        
        // public int IdleWorkerAmount { get;  set; }
        // public int WorkingWorkerAmount { get;  set; }
        // public int UnusedBlueberryAmount { get;  set; }
        // public int UnusedStrawberryAmount { get;  set; }
        // public int UnusedTomatoAmount { get;  set; }
        // public int UnusedCowAmount { get;  set; }
        // public int UsingPlotAmount { get;  set; }
        // public int UnusedPlotAmount { get;  set; }
        // public int BlueberryProductAmount { get;  set; }
        // public int StrawberryProductAmount { get;  set; }
        // public int TomotoProductAmount { get;  set; }
        // public int MilkProductAmount { get;  set; }
        //
        // public int GetSumUnusedSeeds => UnusedBlueberryAmount + UnusedTomatoAmount + UnusedStrawberryAmount;

        
        public Stat(){}

        public Stat(long g, int t, int p, int j, int e)
        {
            GoldAmount = g;
            ToolLevel = t;
            PercentPerLevel = p;
            JobDuration = j;
            ExtentTimeToDestroy = e;
        }
    }
}