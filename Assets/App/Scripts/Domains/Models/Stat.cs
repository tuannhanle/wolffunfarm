namespace App.Scripts.Domains.Models
{
    public class Stat 
    {
        public long GoldAmount { get;  set; }
        public int ToolLevel { get;  set; }
        
        public int PercentPerLevel { get;  set; }
        public int IdleWorkerAmount { get;  set; }
        public int WorkingWorkerAmount { get;  set; }
        public int UnusedBlueberryAmount { get;  set; }
        public int UnusedStrawberryAmount { get;  set; }
        public int UnusedTomatoAmount { get;  set; }
        public int UnusedCowAmount { get;  set; }
        public int UsingPlotAmount { get;  set; }
        public int UnusedPlotAmount { get;  set; }
        public int BlueberryProductAmount { get;  set; }
        public int StrawberryProductAmount { get;  set; }
        public int TomotoProductAmount { get;  set; }
        public int MilkProductAmount { get;  set; }
        
        public int GetSumUnusedSeeds => UnusedBlueberryAmount + UnusedTomatoAmount + UnusedStrawberryAmount;

        
        public Stat(){}

        public Stat(long goldAmount, int toolLevel,int percentPerLevel)
        {
            GoldAmount = goldAmount;
            ToolLevel = toolLevel;
            PercentPerLevel = percentPerLevel;
        }
    }
}