using System.Collections.Generic;
using App.Scripts.Domains.Models;
using App.Scripts.Mics;

namespace App.Scripts.Domains.Core
{
    public class GameManager : MiddlewareBehaviour
    {
        private List<IDependency> _dependencies = new();

        private void Awake()
        {
            
            _dependencies.Add(new WorkerManager());
            _dependencies.Add(new ToolManager());
            _dependencies.Add(new PlotManager());
            _dependencies.Add(new ShopManager());
            _dependencies.Add(new StatManager());
            foreach (var dependency in _dependencies)
            {
                dependency.Init();
            }

        }

        private void Start()
        {

        }
    }
}