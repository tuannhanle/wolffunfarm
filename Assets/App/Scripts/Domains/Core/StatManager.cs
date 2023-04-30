using App.Scripts.Domains.GameObjects;
using App.Scripts.Domains.Models;
using App.Scripts.Domains.Services;

namespace App.Scripts.Domains.Core
{
    public class StatManager : Dependency<StatManager>
    {
        public int GoldAmount { get; private set; }
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

        public StatManager() : base()
        {
            SyncFromLocalDB();
        }
        
        public void CheatGoldAmount(ICheat iCheat)
        {
            GoldAmount = 0;
            // Gold =  new() { Amount = iCheat.GoldAmount, Name = "Gold" };
        }

        private void SyncFromLocalDB()
        {
            // Gold = new() { Amount = 0, Name = "Gold" };
            GoldAmount = 0;
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

        public void GainItem(ItemType itemType, bool isUnsed = true)
        {
            switch (itemType)
            {
                case ItemType.Cow: UnusedCowAmount++; break;
                case ItemType.StrawBerry: UnusedStrawberryAmount++; break;
                case ItemType.BlueBerry: UnusedBlueberryAmount++; break;
                case ItemType.Tomato: UnusedTomatoAmount++; break;
                case ItemType.Plot: UnusedPlotAmount++; break;
            }
        }

        public void Gain<T>(bool isUnsed = true) where T : IBuyable
        {
            if (typeof(T) == typeof(Worker)) IdleWorkerAmount++;
            if (typeof(T) == typeof(Tool)) ToolLevel++;
        }
        
        
    }

    public interface ICheat
    {
        int GoldAmount { get; set; }
    }
}