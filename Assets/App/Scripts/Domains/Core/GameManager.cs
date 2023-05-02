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
                new JobManager()
                // add more at above
            } );
     
            foreach (var dependency in _dependencies)
            {
                dependency.Init();
            }
            
            _statManager = DependencyProvider.Instance.GetDependency<StatManager>();
            _workerManager = DependencyProvider.Instance.GetDependency<WorkerManager>();
            _dataLoader = DependencyProvider.Instance.GetDependency<DataLoader>();
            // _statManager.PostcastData();
        }

        private void Start()
        {

            MainFlow().Forget();
        }

        private async UniTask MainFlow()
        {
            // per frame
            while (_dataLoader.stat.GoldAmount < GOLD_TARGET)
            {
                _workerManager.ExecuteAsync().Forget();
                await UniTask.Yield();

            }

            Debug.Log("Game has been finished");
        }
    }
}