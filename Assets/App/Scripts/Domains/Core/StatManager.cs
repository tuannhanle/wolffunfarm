using App.Scripts.Domains.GameObjects;
using App.Scripts.Domains.Models;
using App.Scripts.Domains.Services;

namespace App.Scripts.Domains.Core
{
    public class StatManager : Dependency<StatManager>
    {
        private Stat _stat = new Stat();
        public Stat Stat => _stat;

        private LazyDataInlet<Stat> _statsUpdatedInlet = new();

        public StatManager() : base()
        {
            SyncFromLocalDB();
        }

        public void PostcastData()
        {
            _statsUpdatedInlet.UpdateValue(_stat);
        }
        
        public void CheatGoldAmount(ICheat iCheat)
        {
            _stat.GoldAmount = 0;
            // Gold =  new() { Amount = iCheat.GoldAmount, Name = "Gold" };
        }

        private void SyncFromLocalDB()
        {
            // Gold = new() { Amount = 0, Name = "Gold" };
            _stat.GoldAmount = 0;
            _stat.ToolLevel = 1;
            _stat.IdleWorkerAmount = 2;
            _stat.WorkingWorkerAmount = 0;
            _stat.UnusedBlueberryAmount = 10;
            _stat.UnusedTomatoAmount = 10;
            _stat.UnusedStrawberryAmount = 0;
            _stat.UnusedCowAmount = 2;
            _stat.UnusedPlotAmount = 3;
            _stat.UsingPlotAmount = 0;
            _stat.BlueberryProductAmount = 0;
            _stat.StrawberryProductAmount = 0;
            _stat.TomotoProductAmount = 0;
            _stat.MilkProductAmount = 0;
        }



        public void GainItem(ItemType? itemType, int amount, bool isUnsed = true)
        {
            if (isUnsed)
            {
                switch (itemType)
                {
                    case ItemType.Cow: _stat.UnusedCowAmount+=amount; break;
                    case ItemType.StrawBerry: _stat.UnusedStrawberryAmount+=amount; break;
                    case ItemType.BlueBerry: _stat.UnusedBlueberryAmount+=amount; break;
                    case ItemType.Tomato: _stat.UnusedTomatoAmount+=amount; break;
                    case ItemType.Plot: _stat.UnusedPlotAmount+=amount; break;
                } 
            }
            else
            {
                switch (itemType)
                {
                    case ItemType.Plot: _stat.UsingPlotAmount+=amount; break;
                } 
            }


            PostcastData();
        }

        public void Gain<T>( int amount, bool isUnsed = true) where T : IBuyable
        {
            if (typeof(T) == typeof(Worker)) _stat.IdleWorkerAmount+=amount;
            if (typeof(T) == typeof(Tool)) _stat.ToolLevel+=amount;
            PostcastData();

        }
        
        
    }

    public interface ICheat
    {
        int GoldAmount { get; set; }
    }
}