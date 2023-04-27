using System.Collections.Generic;
using App.Scripts.Domains.Models;
using App.Scripts.Mics;
using Plot = App.Scripts.Domains.GameObjects.Plot;

namespace App.Scripts.Domains.Core
{
    public class PlotManager
    {

        private readonly StatManager _statManager;
        private readonly List<Plot> _plots = new();
        
        public PlotManager(StatManager statManager)
        {
            _statManager = statManager;
            for (int i = 0; i < _statManager.UnusedPlotAmount + _statManager.UsingPlotAmount ; i++)
            {
                _plots.Add(new());
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
            _plots.Add(new Plot());
        }
    }
}