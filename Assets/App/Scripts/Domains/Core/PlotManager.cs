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
            _statManager.GainItem(ItemType.Plot, AMOUNT_EACH_PLOT);
        }

        
        public int Attach(ItemType? itemType)
        {
            var numberOfProceed = 0;
            if (itemType == null)
                return 0;
            foreach (var plot in _plots)
            {
                var isPlanCropable = plot.PlantCrop(itemType);
                if (isPlanCropable)
                { 
                    numberOfProceed++;
                    _statManager.GainItem(itemType, -1);
                    _statManager.GainItem(ItemType.Plot, -1, true);
                    _statManager.GainItem(ItemType.Plot, 1, false);

                    break;
                }
            }
            return numberOfProceed;
        }

        /// <summary>
         /// 
         /// </summary>
         /// <param name="itemType"></param>
         /// <returns>Wait a time for each proceed for Worker</returns>
        public int Collect(ItemType? itemType)
        {
            var numberOfProceed = 0;
            if (itemType == null)
                return 0;
            foreach (var plot in _plots)
            {
                var isCollectable = plot.IsCollectable(itemType);
                if (isCollectable)
                {
                    plot.Harvest(itemType);
                    numberOfProceed++;
                }
            }
            return numberOfProceed;
        }
    }
}