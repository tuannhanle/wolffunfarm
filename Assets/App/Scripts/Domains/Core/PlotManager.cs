using System.Collections.Generic;
using App.Scripts.Domains.GameObjects;
using App.Scripts.Mics;

namespace App.Scripts.Domains.Core
{
    public class PlotManager
    {
        public int UsingPlotAmount { get; set; }
        public int UnusedPlotAmount { get; set; }
        private List<Plot> _plots = new();

        // TODO: get it from DB
        private int _amountMoney = 1000;
        public int AmountMoney => _amountMoney;

        public void Init()
        {
            for (int i = 0; i < UnusedPlotAmount + UsingPlotAmount ; i++)
            {
                _plots.Add(new());
            }
        }
        
        public void ExtendPlot(ShareData.ShopEventType? uiEventEInteractEvent)
        {
            if (uiEventEInteractEvent != ShareData.ShopEventType.BPlot)
                return;
            if (_amountMoney < Plot.Price)
                return;
            _amountMoney -= Plot.Price;
            //TODO: save new <plot amount> to storage
            _plots.Add(new Plot());
        }
    }
}