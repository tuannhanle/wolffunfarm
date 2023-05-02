namespace App.Scripts.Mics
{
    public class ShareData
    {
        public class OpenShopEvent
        { }

        public class WorkerPassage
        {
            public int IdleWorkerAmount = 0;
            public int WorkingWorkerAmount = 0;
        }       
        
        public class ItemStoragePassage
        {
            public int GetSumUnusedSeeds = 0;
            public int UnusedPlotAmount = 0;
            public int UsingPlotAmount = 0;
            public int BlueberryProductAmount = 0;
            public int TomotoProductAmount = 0;
            public int StrawberryProductAmount = 0;
            public int MilkProductAmount = 0;
        }
        
    }
}