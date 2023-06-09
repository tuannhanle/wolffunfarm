using System;
using System.Collections.Generic;
using App.Scripts.Domains.Models;
using App.Scripts.Mics;
using Plot = App.Scripts.Domains.GameObjects.Plot;

namespace App.Scripts.Domains.Core
{
    public class PlotManager : Dependency<PlotManager>, IDependency
    {
        private LazyDataInlet<ShareData.PlotUIPassage> _plotInlet = new();
        public void ExtendPlot()
        {
            var plotItem = _dataLoader.ItemCollection[Define.PLOT];
            var isPayable = _paymentService.Buy(plotItem);
            if( isPayable == false)
                return;
            var plotId = _dataLoader.PlotStorage.Count;
            var plot = new Plot(plotId, "", 0, -1, TimeStamp.FirstDay, false);
            _dataLoader.PlotStorage.Add(plotId, plot);
            _dataLoader.Push<Plot>();
            _plotInlet.UpdateValue(new ShareData.PlotUIPassage()
            {
                Plot = plot,
                IsExtend = true
            });
        }
        
        public void PutIn(bool isStart, int plotId, string itemName)
        {
            if (_dataLoader.PlotStorage.TryGetValue(plotId, out Plot plot) == false)
                return;
            if (_dataLoader.ItemCollection.TryGetValue(itemName, out Item item) == false)
                return;
            if(plot.IsUsing)
                return;
            var extentTime = _dataLoader.stat.ExtentTimeToDestroy;
            plot.PutIn(item, extentTime);
            
            _plotInlet.UpdateValue(new ShareData.PlotUIPassage() { Plot = plot });
            
            if (isStart)
            {
                if (_dataLoader
                    .ItemStorage
                    .TryGetValue(itemName, out ItemStorage itemStorage))
                {
                    itemStorage.UnusedAmount--;
                    _dataLoader.Push<ItemStorage>();
                }
            }
    
            _dataLoader.Push<Plot>();
        }
        
        public void PutOut(bool isStart, int plotId, string itemName)
        {
            if (_dataLoader.PlotStorage.TryGetValue(plotId, out Plot plot) == false)
                return;
            if (_dataLoader.ItemCollection.TryGetValue(itemName, out Item item) == false)
                return;
            if (plot.IsPutOutable(itemName) == false)
                return;
            
            _plotInlet.UpdateValue(new ShareData.PlotUIPassage() { Plot = plot });

            if (isStart == false)
            {
                plot.PutOut();
            }

            _dataLoader.Push<Plot>();
        }

        public List<Plot> GetUsingPlots()
        {
            List<Plot> plots = new();
            foreach (var plot in _dataLoader.PlotStorage)
            {
                if (plot.Value.IsUsing)
                {
                    plots.Add(plot.Value);
                }
            }

            return plots;
        }
        
        /// <summary>
        /// Returns a list of plots that match the given job type and item name.
        /// </summary>
        /// <param name="jobType">The job type to match.</param>
        /// <param name="itemName">The item name to match.</param>
        /// <returns>A list of matching plots, or null if no plots were found.</returns>
        public List<Plot> GetPlots(JobType jobType, string itemName)
        {
            // Create an empty list to store matching plots.
            List<Plot> plots = new();

            // Iterate through all plots in the plot storage.
            foreach (var plot in _dataLoader.PlotStorage)
            {
                var isExecutingPlot = false;

                // If the job type is Harvasting, find plots that match the given item name and are currently in use.
                if (jobType == JobType.PutOut)
                {
                    foreach (var pair in _dataLoader.JobStorage)
                    {
                        var job = pair.Value;
                        if (job.PlotId == plot.Value.Id)
                        {
                            isExecutingPlot = true;
                            break;
                        }
                    }

                    // If the plot is not currently in use or does not contain the required item, skip it.
                    if (plot.Value.IsUsing == false || plot.Value.ItemName.Equals(itemName) == false)
                        continue;
                }
                
                else if (jobType == JobType.PutIn)
                {
                    foreach (var pair in _dataLoader.JobStorage)
                    {
                        var job = pair.Value;
                        if (job.PlotId == plot.Value.Id)
                        {
                            isExecutingPlot = true;
                            break;
                        }
                    }
                    
                    // If the plot is currently in use, skip it.
                    if (plot.Value.IsUsing)
                        continue;

                }
                if (isExecutingPlot)
                    continue;
                // If the plot is not in use, add it to the list of matching plots.
                plots.Add(plot.Value);
            }

            // Return the list of matching plots, or null if no plots were found.
            return plots.Count > 0 ? plots : null;
        }

        public void ReturnUnusedPlot(int plotId)
        {
            if (_dataLoader.PlotStorage.TryGetValue(plotId, out Plot plot) == false)
                return;
            plot.Clear();
            _dataLoader.Push<Plot>();
            _plotInlet.UpdateValue(new ShareData.PlotUIPassage() { Plot = plot });

        }
    }
}