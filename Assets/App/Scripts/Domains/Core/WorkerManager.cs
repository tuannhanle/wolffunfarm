using System;
using System.Collections.Generic;
using App.Scripts.Domains.Models;
using App.Scripts.Mics;
using Cysharp.Threading.Tasks;
using Progress = App.Scripts.Domains.Models.Progress;

namespace App.Scripts.Domains.Core
{
    public class WorkerManager
    {

        private readonly StatManager _statManager;
        private readonly Queue<Worker> _idleWorkers = new();
        private readonly Queue<Worker> _workingWorkers = new();
        private readonly Progress _progress = new();
        private readonly Worker _worker = new();

        private const float DURATION_WORKER = 120f;

        private bool isWorkerExecutable => _idleWorkers.Count > 0;
        public WorkerManager()
        {
            _statManager = DependencyProvider.Instance.GetDependency<StatManager>();
            DependencyProvider.Instance.RegisterDependency(typeof(WorkerManager), this);

            for (int i = 0; i < _statManager.IdleWorkerAmount; i++)
            {
                _idleWorkers.Enqueue(new ());
            }
            for (int i = 0; i < _statManager.WorkingWorkerAmount; i++)
            {
                _workingWorkers.Enqueue(new ());
            } 
            TakeProceedingAsync(new () { EProceeding = ProceedingType.Seeding }).Forget();
            TakeProceedingAsync(new () { EProceeding = ProceedingType.Seeding }).Forget();
            TakeProceedingAsync(new () { EProceeding = ProceedingType.Seeding }).Forget();
        }
        
        private async UniTask TakeProceedingAsync(Proceeding proceeding)
        {
            _progress.datas.Push(proceeding);
            TakeProgressAsync(_progress).Forget();
        }
        
        public async UniTask TakeProgressAsync(Progress progress)
        {
            while (progress.datas.Count > 0)
            {
                var proceeding = progress.datas.Pop();
                WorkerExecuteAsync(proceeding).Forget();
                await UniTask.Yield();
            }
            
        }
        
        // private float GetDuration()
        // {
        //     var toolLevel = 1f;
        //     var toolPercent = 100f + (toolLevel - 1f) * 10f;
        //     return DURATION_WORKER + DURATION_WORKER*(toolPercent/100f);
        // }
        
        //TODO: worker has still not execute the proceed
        private async UniTask WorkerExecuteAsync(Proceeding proceeding)
        {
            var timeSpanDuration = TimeSpan.FromSeconds(DURATION_WORKER);
            if (isWorkerExecutable)
            {
                var executableWorker = _idleWorkers.Dequeue();
                _workingWorkers.Enqueue(executableWorker);
                await UniTask.Delay(timeSpanDuration);
                executableWorker = _workingWorkers.Dequeue();
                _idleWorkers.Enqueue(executableWorker);
                return;
            }
            await UniTask.Delay(timeSpanDuration);
            
        }

        public void RentWorker(ShareData.InteractEventType? EInteractEvent)
        {
            if (EInteractEvent != ShareData.InteractEventType.RentWorker)
                return;
            if(_statManager.Gold.IsPayable(_worker.Price) == false)
                return;
            _statManager.Gold.Pay(_worker.Price);
            //TODO: save new <worker> to storage
            _idleWorkers.Enqueue(new ());
            
        }


    }
    
}