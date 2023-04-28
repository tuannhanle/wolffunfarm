using System.Collections.Generic;
using App.Scripts.Domains.Models;
using App.Scripts.Mics;
using Cysharp.Threading.Tasks;
using Plot = App.Scripts.Domains.GameObjects.Plot;

namespace App.Scripts.Domains.Core
{
    public class PlotManager
    {

        private readonly StatManager _statManager;
        private readonly Queue<Plot> _unusedPlots = new();
        private readonly Queue<Plot> _usingPlots = new();

        private bool isSeedable => _unusedPlots.Count > 0;
        
        public PlotManager()
        {
            _statManager = DependencyProvider.Instance.GetDependency<StatManager>();
            DependencyProvider.Instance.RegisterDependency(typeof(PlotManager), this);

            for (int i = 0; i < _statManager.UnusedPlotAmount   ; i++)
            {
                _unusedPlots.Enqueue(new());
            }

            for (int i = 0; i < _statManager.UsingPlotAmount; i++)
            {
                _usingPlots.Enqueue(new());
            }
        }
        
        public void ExtendPlot(ShareData.ShopEventType? uiEventEInteractEvent)
        {
            if (uiEventEInteractEvent != ShareData.ShopEventType.BPlot)
                return;
            if(_statManager.Gold.IsPayable(Plot.Price) == false)
                return;
            _statManager.Gold.Pay(Plot.Price);
            //TODO: save new <plot amount> to storage
            _unusedPlots.Enqueue(new Plot());
        }

        public void Seeding(ItemType itemType)
        {
            if (isSeedable == false)
                return;
            var unusedPlot = _unusedPlots.Dequeue();
            unusedPlot.Crop = new Crop() { Item = new Item(){ItemType = itemType}};
            _usingPlots.Enqueue(unusedPlot);
            
        }
        
        
    }
}