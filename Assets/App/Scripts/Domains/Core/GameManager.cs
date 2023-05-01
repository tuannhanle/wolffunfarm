using System;
using System.Collections.Generic;
using App.Scripts.Domains.Models;
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
        private void Awake()
        {
            
            _dependencies.AddRange(new List<IDependency>
            {
                new WorkerManager(),
                new ToolManager(),
                new PlotManager(),
                new ShopManager(),
                new StatManager(),
                new PaymentService(),
                // add more at above
            } );
     
            foreach (var dependency in _dependencies)
            {
                dependency.Init();
            }
            
            _statManager = DependencyProvider.Instance.GetDependency<StatManager>();
            _workerManager = DependencyProvider.Instance.GetDependency<WorkerManager>();
        }

        private void Start()
        {
            _statManager.PostcastData();
            MainFlow().Forget();
        }

        private async UniTask MainFlow()
        {
            // per frame
            while (_statManager.Stat.GoldAmount == GOLD_TARGET)
            {
                _workerManager.ExecuteAsync();
            }

            Debug.Log("Game has been finished");
        }
    }
}