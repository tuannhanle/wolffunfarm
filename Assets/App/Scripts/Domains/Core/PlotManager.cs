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
                _plots.Add(new());
            }
            for (int i = 0; i < _statManager.UsingPlotAmount ; i++)
            {
                _plots.Add(new());
            }
        }
        
        public void ExtendPlot()
        {
            var plot = new Stuff(500).BeBoughtBy(_statManager.Gold) as Plot;
            if (plot == null)
                return;
            _plots.Add(plot);
            _statManager.GainItem(ItemType.Plot);
            //TODO: save new <plot amount> to storage
            
        }

        public bool Attach(ItemType itemType)
        {
            foreach (var plot in _plots)
            {
                return plot.PlantCrop(itemType);
            }
            return false;
        }





    }
}