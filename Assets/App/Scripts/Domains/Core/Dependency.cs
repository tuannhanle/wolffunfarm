using App.Scripts.Domains.Services;
using UnityEngine;

namespace App.Scripts.Domains.Core
{
    public class Dependency<T>  where T : class 
    {
        protected WorkerManager _workerManager;
        protected ToolManager _toolManager;
        protected PlotManager _plotManager;
        protected ShopManager _shopManager;
        protected StatManager _statManager;
        protected PaymentService _paymentService;
        
        public Dependency()
        {
            DependencyProvider.Instance.RegisterDependency(typeof(T), this);
            Debug.Log($"[{typeof(T)}] has been RegisterDependency");

        }

        public void Init()
        {
            InjectDependencies();
            Debug.Log($"[{typeof(T)}] has been Initialized");
        }

        protected void InjectDependencies()
        {
            if(_statManager == null) _statManager =  DependencyProvider.Instance.GetDependency<StatManager>();
            if(_plotManager == null) _plotManager = DependencyProvider.Instance.GetDependency<PlotManager>();
            if(_workerManager == null) _workerManager = DependencyProvider.Instance.GetDependency<WorkerManager>();
            if(_toolManager == null) _toolManager = DependencyProvider.Instance.GetDependency<ToolManager>();
            if(_shopManager == null) _shopManager = DependencyProvider.Instance.GetDependency<ShopManager>();
            if(_paymentService == null) _paymentService = DependencyProvider.Instance.GetDependency<PaymentService>();

        }
    }
}