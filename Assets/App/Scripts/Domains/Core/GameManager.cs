using System;
using System.Collections.Generic;
using App.Scripts.Domains.Services;
using App.Scripts.Mics;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Domains.Core
{
    public class GameManager : MiddlewareBehaviour
    {
        private List<IDependency> _dependencies = new();
        private const int  GOLD_TARGET = 1000000;
        private StatManager _statManager;
        private WorkerManager _workerManager;
        private DataLoader _dataLoader;
        private PlotManager _plotMananger;
        private JobManager _jobManager;
        
        private LazyDataInlet<ShareData.PlotUIPassage> _plotInlet = new();

        private void Awake()
        {
            
            _dependencies.AddRange(new List<IDependency>
            {
                new DataLoader(),
                new WorkerManager(),
                new ToolManager(),
                new PlotManager(),
                new ShopManager(),
                new StatManager(),
                new PaymentService(),
                new BroadcastService(),
                new JobManager()
                // add more at above
            } );
     
            foreach (var dependency in _dependencies)
            {
                dependency.Init();
            }
            
            _statManager = DependencyProvider.Instance.GetDependency<StatManager>();
            _plotMananger = DependencyProvider.Instance.GetDependency<PlotManager>();
            _workerManager = DependencyProvider.Instance.GetDependency<WorkerManager>();
            _dataLoader = DependencyProvider.Instance.GetDependency<DataLoader>();
            _jobManager = DependencyProvider.Instance.GetDependency<JobManager>();
        }

        private void Start()
        {

            MainFlow().Forget();
        }

        private async UniTask MainFlow()
        {
            
            while (_dataLoader.stat.GoldAmount < GOLD_TARGET)
            {
                _jobManager.CheckJobToDo();
                var plots = _plotMananger.GetUsingPlots();
                foreach (var plot in plots)
                {
                    plot.SelfCheck();
                    _plotInlet.UpdateValue(new ShareData.PlotUIPassage(){Plot = plot});
                }
                await UniTask.Delay(TimeSpan.FromSeconds(1));
            }
            Debug.Log("Game has been finished");
        }

 
    }
}