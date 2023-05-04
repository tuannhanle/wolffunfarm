using App.Scripts.Domains.GameObjects;
using App.Scripts.Domains.Models;

namespace App.Scripts.Mics
{
    public class ShareData
    {
        public class OpenShopEvent
        { }

        public class CartEvent
        {
            public string itemNamePicked = null;
            public bool isBuy = false;
            public bool isRelease = false;
            public int amountPick = 0;
            public int amountTotalOrder = 0;

        }

        public class WorkerPassage
        {
            public int IdleWorkerAmount = 0;
            public int WorkingWorkerAmount = 0;
        }       
        
        public class ItemStoragePassage
        {
            public int GetSumUnusedSeeds = 0;
            public int BlueberryProductAmount = 0;
            public int TomotoProductAmount = 0;
            public int StrawberryProductAmount = 0;
            public int MilkProductAmount = 0;
        }

        public class PlotPassage
        {
            public int UnusedAmount = 0;
            public int UsingWorkerAmount = 0;
        }

        public class PlotUIPassage
        {
            public Plot Plot { get; set; }
            public bool IsExtend { get; set; } = false;
        }
    }
}