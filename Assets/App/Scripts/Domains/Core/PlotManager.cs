using System.Collections.Generic;
using App.Scripts.Domains.Models;
using Plot = App.Scripts.Domains.GameObjects.Plot;

namespace App.Scripts.Domains.Core
{
    public class PlotManager : Dependency<PlotManager>, IDependency
    {
        private const string PLOT = "Plot";
        
        public void ExtendPlot()
        {
            var plot = _dataLoader.ItemCollection[PLOT];
            var isPayable = _paymentService.Buy(plot);
            if( isPayable == false)
                return;
            var plotId = _dataLoader.PlotStorage.Count;
            _dataLoader.PlotStorage.Add(plotId, new Plot(plotId,"",0,0,null,null,false));
            _dataLoader.Push<Plot>();
        }

        public bool IsPutInable(int plotId, string itemName)
        {
            if (_dataLoader.PlotStorage.TryGetValue(plotId, out var plot) == false)
                return false;
            if (_dataLoader.ItemCollection.TryGetValue(itemName, out var item) == false)
                return false;
            return plot.IsPutInable();
        }
        
        public void PutIn(int plotId, string itemName)
        {
            if (_dataLoader.PlotStorage.TryGetValue(plotId, out var plot) == false)
                return;
            if (_dataLoader.ItemCollection.TryGetValue(itemName, out var item) == false)
                return;
            var extentTime = _dataLoader.stat.ExtentTimeToDestroy;
            plot.PutIn(item.ProductCapacity, extentTime);
        }

        public bool IsHarvastable(int plotId, string itemName)
        {
            if (_dataLoader.PlotStorage.TryGetValue(plotId, out var plot) == false)
                return false;
            if (_dataLoader.ItemCollection.TryGetValue(itemName, out var item) == false)
                return false;
            return plot.IsHarvastable(itemName);
        }
        

        public void Harvast(int plotId, string itemName)
        {
            if (_dataLoader.PlotStorage.TryGetValue(plotId, out var plot) == false)
                return;
            if (_dataLoader.ItemCollection.TryGetValue(itemName, out var item) == false)
                return;
            plot.Harvest(itemName);
        }

        public List<Plot> GetPlots(JobType jobType, string itemName)
        {
            List<Plot> plots = new();
            if (jobType == JobType.Harvasting)
            {
                foreach (var plot in _dataLoader.PlotStorage)
                {
                    if (plot.Value.IsUsing == false)
                        continue;
                    if (plot.Value.ItemName.Equals(itemName) == false)
                        continue;
                    plots.Add(plot.Value);
                }
            }
            else
            {
                foreach (var plot in _dataLoader.PlotStorage)
                {
                    if (plot.Value.IsUsing)
                        continue;
                    plots.Add(plot.Value);
                }
            }
            return plots.Count != 0 ? plots : null;
        }


    }
}