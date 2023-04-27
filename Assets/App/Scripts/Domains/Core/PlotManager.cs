using System.Collections.Generic;
using App.Scripts.Domains.GameObjects;
using App.Scripts.Mics;

namespace App.Scripts.Domains.Core
{
    public class PlotManager
    {
        private List<Plot> _plots = new List<Plot>();
        

        public void InitVeryFirstLogin()
        {
            _plots.AddRange(new List<Plot>(){new Plot(), new Plot(), new Plot()});

        }
        
        // TODO: get it from DB
        private int _amountMoney = 1000;
        public int AmountMoney => _amountMoney;
        
        public void ExtendPlot(ShareData.InteractEventType? uiEventEInteractEvent)
        {
            if (uiEventEInteractEvent != ShareData.InteractEventType.ExtendPlot)
                return;
            if (_amountMoney < Plot.Price)
                return;
            _amountMoney -= Plot.Price;
            //TODO: save new <plot amount> to storage
            _plots.Add(new Plot());
        }
    }
}