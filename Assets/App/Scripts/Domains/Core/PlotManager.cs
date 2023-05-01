using System.Collections.Generic;
using App.Scripts.Domains.Models;
using App.Scripts.Mics;
using Cysharp.Threading.Tasks;
using Plot = App.Scripts.Domains.GameObjects.Plot;

namespace App.Scripts.Domains.Core
{
    public class PlotManager : Dependency<PlotManager>, IDependency
    {
        private readonly List<Plot> _plots = new();
        private const int AMOUNT_EACH_PLOT = 1;
        public void Init()
        {
            base.Init();
            for (int i = 0; i < _statManager.Stat.UnusedPlotAmount  ; i++)
            {
                _plots.Add(Define.PlotItem);
            }
            for (int i = 0; i < _statManager.Stat.UsingPlotAmount ; i++)
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
            _statManager.GainUnused(ItemType.UnusedPlot, AMOUNT_EACH_PLOT);
        }

        
        public void Attach(ItemType? itemType)
        {
            if (itemType == null)
                return;
            foreach (var plot in _plots)
            {
                var isPlanCropable = plot.PlantCrop(itemType);
                if (isPlanCropable)
                { 
                    _statManager.GainUsing(itemType, -1);
                    _statManager.GainUsing(ItemType.UnusedPlot, -1);
                    _workerManager.Assign(new Job() { EItemType = itemType, EJob = JobType.PutIn });
                    break;
                    
                }
            }
        }

        /// <summary>
         /// 
         /// </summary>
         /// <param name="itemType"></param>
         /// <returns>Wait a time for each proceed for Worker</returns>
        public void Collect(ItemType? itemType)
        {
            if (itemType == null)
                return;
            foreach (var plot in _plots)
            {
                var isCollectable = plot.IsCollectable(itemType);
                if (isCollectable)
                {
                    plot.Harvest(itemType);
                    break;
                }
            }
        }
    }
}