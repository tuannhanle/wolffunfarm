using System.Collections.Generic;
using App.Scripts.Domains.Models;
using App.Scripts.Mics;
using Cysharp.Threading.Tasks;
using Plot = App.Scripts.Domains.GameObjects.Plot;

namespace App.Scripts.Domains.Core
{
    public class PlotManager : Dependency<PlotManager>
    {
        private readonly List<Plot> _plots = new();

        public void Init()
        {
            base.Init();
            for (int i = 0; i < _statManager.UnusedPlotAmount  ; i++)
            {
                _plots.Add(Define.PlotItem);
            }
            for (int i = 0; i < _statManager.UsingPlotAmount ; i++)
            {
                _plots.Add(Define.PlotItem);
            }
        }
        
        public void ExtendPlot()
        {
            var plot = Define.PlotItem;
            var isPayable = _paymentService.Buy(plot);
            if(isPayable == false)
                return;
            _plots.Add(plot);
            _statManager.GainItem(ItemType.Plot);
        }

        public bool Attach(ItemType? itemType)
        {
            if (itemType == null)
                return false;
            foreach (var plot in _plots)
            {
                return plot.PlantCrop(itemType);
            }
            return false;
        }





    }
}