using App.Scripts.Domains.Core;
using App.Scripts.Domains.Models;
using App.Scripts.Mics;

namespace App.Scripts.Domains.Services
{
    public class BroadcastService: Dependency<BroadcastService>, IDependency
    {
        public void Init()
        {
            base.Init();
            BroadcastData(_dataLoader.stat);
            BroadcastWorkerPassager();
            BroadcastPlotPassager();
            BroadcastItemStoragePassager();
        }
        
        public void BroadcastPlotPassager()
        {
            var plotPassage = new ShareData.PlotPassage();
            foreach (var pair in _dataLoader.PlotStorage)
            {
                plotPassage.UnusedAmount += pair.Value.IsUsing ? 0 : 1;
                plotPassage.UsingWorkerAmount += pair.Value.IsUsing ? 1 : 0;
            }
            BroadcastData(plotPassage);
            
        }

        public void BroadcastItemStoragePassager()
        {
            var itemStoragePassenger = new ShareData.ItemStoragePassage();
            foreach (var pair in _dataLoader.ItemStorage)
            {
                if (pair.Key.Equals("Blueberry"))
                {
                    itemStoragePassenger.GetSumUnusedSeeds += pair.Value.UnusedAmount;
                    itemStoragePassenger.BlueberryProductAmount += pair.Value.HarvestedProduct;
                }
                if (pair.Key.Equals("Tomato"))
                {
                    itemStoragePassenger.GetSumUnusedSeeds += pair.Value.UnusedAmount;
                    itemStoragePassenger.TomotoProductAmount += pair.Value.HarvestedProduct;
                }
                if (pair.Key.Equals("Strawberry"))
                {
                    itemStoragePassenger.GetSumUnusedSeeds += pair.Value.UnusedAmount;
                    itemStoragePassenger.StrawberryProductAmount += pair.Value.HarvestedProduct;
                }
                if (pair.Key.Equals(Define.COW))
                {
                    itemStoragePassenger.MilkProductAmount += pair.Value.HarvestedProduct;
                }
            }
            BroadcastData(itemStoragePassenger);
        }

        public void BroadcastWorkerPassager()
        {
            var workerPassager = new ShareData.WorkerPassage();
            foreach (var pair in _dataLoader.WorkerStorage)
            {
                workerPassager.IdleWorkerAmount += pair.Value.IsUsing ? 0 : 1;
                workerPassager.WorkingWorkerAmount += pair.Value.IsUsing ? 1 : 0;
            }
            BroadcastData(workerPassager);
        }
        
        private void BroadcastData<T>(T data) where T : class
        { 
            LazyDataInlet<T> _inlet = new();
            _inlet.UpdateValue(data);
        }
    }
}