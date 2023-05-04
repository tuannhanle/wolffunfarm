using System;
using System.Collections.Generic;
using App.Scripts.Domains.Core;
using App.Scripts.Domains.GameObjects;
using App.Scripts.Mics;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.UI
{
    public class FieldUI : MiddlewareBehaviour
    {
        [Header("Refs")] 
        [SerializeField] private PlotUI _plotPrefab;
        [SerializeField] private Transform _root;

        private DataLoader _dataLoader;
        private Dictionary<int, PlotUI> _plotUImaps = new();


        
        public void Start()
        {
            _dataLoader = DependencyProvider.Instance.GetDependency<DataLoader>();
            CreateField();
            this.Subscribe<ShareData.PlotUIPassage>(o =>
            {
                var plot = o.Plot;
                if (o.IsExtend)
                {
                    CreatePlot(plot);
                }
                else
                {
                    var plotUI = GetPlot(plot.Id);
                    plotUI.SetUp(plot);
                }

            });
        }

        private PlotUI GetPlot(int plotId)
        {
            if (_plotUImaps.TryGetValue(plotId, out var plotUI) == false)
                return null;
            return plotUI;
        }
        
        private void CreateField()
        {
            var plots = _dataLoader.PlotStorage;
            foreach (var plot in plots)
            {
                var plotUI = CreatePlot(plot.Value);
            }
        }

        private PlotUI CreatePlot(Plot plot)
        {
            var plotUI = Instantiate(_plotPrefab, _root);
            plotUI.SetUp(plot);
            _plotUImaps.Add(plot.Id, plotUI);
            return plotUI;
        }
        
        
    }
}