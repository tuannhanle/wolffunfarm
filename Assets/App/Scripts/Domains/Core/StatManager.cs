using App.Scripts.Domains.Models;

namespace App.Scripts.Domains.Core
{
    public class StatManager
    {
        public Gold Gold { get; private set; }
        public int ToolLevel { get; private set; }
        public int IdleWorkerAmount { get; private set; }
        public int WorkingWorkerAmount { get; private set; }
        public int UnusedBlueberryAmount { get; private set; }
        public int UnusedStrawberryAmount { get; private set; }
        public int UnusedTomatoAmount { get; private set; }
        public int UnusedCowAmount { get; private set; }
        public int UsingPlotAmount { get; private set; }
        public int UnusedPlotAmount { get; private set; }
        public int BlueberryProductAmount { get; private set; }
        public int StrawberryProductAmount { get; private set; }
        public int TomotoProductAmount { get; private set; }
        public int MilkProductAmount { get; private set; }

        public void SyncFromLocalDB()
        {
            Gold = new() { Amount = 0, Name = "Gold" };
            ToolLevel = 1;
            IdleWorkerAmount = 2;
            WorkingWorkerAmount = 0;
            UnusedBlueberryAmount = 10;
            UnusedTomatoAmount = 10;
            UnusedStrawberryAmount = 0;
            UnusedCowAmount = 2;
            UnusedPlotAmount = 3;
            UsingPlotAmount = 0;
            BlueberryProductAmount = 0;
            StrawberryProductAmount = 0;
            TomotoProductAmount = 0;
            MilkProductAmount = 0;
        }
        
        
    }   
}