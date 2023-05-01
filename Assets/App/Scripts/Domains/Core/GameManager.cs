using System;
using System.Collections.Generic;
using App.Scripts.Domains.Models;
using App.Scripts.Domains.Services;
using App.Scripts.Mics;

namespace App.Scripts.Domains.Core
{
    public class GameManager : MiddlewareBehaviour
    {
        private List<IDependency> _dependencies = new();

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
        }

        private void Start()
        {
            DependencyProvider.Instance.GetDependency<StatManager>().PostcastData();
        }
    }
}