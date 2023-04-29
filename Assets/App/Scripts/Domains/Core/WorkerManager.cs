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
        private readonly PlotManager _plotManager;
        private readonly Queue<Worker> _idleWorkers = new();
        private readonly Queue<Worker> _workingWorkers = new();
        private readonly Progress _progress = new();

        private const float DURATION_WORKER = 120f;

        private bool isWorkerExecutable => _idleWorkers.Count > 0;
        public WorkerManager()
        {
            _statManager = DependencyProvider.Instance.GetDependency<StatManager>();
            _plotManager = DependencyProvider.Instance.GetDependency<PlotManager>();

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
                if (await WorkerExecuteAsync(proceeding) == false)
                {
                    progress.datas.Push(proceeding);
                }
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
        private async UniTask<bool> WorkerExecuteAsync(Proceeding proceeding)
        {
            var timeSpanDuration = TimeSpan.FromSeconds(DURATION_WORKER);
            if (isWorkerExecutable == false)
                return false;
            var executableWorker = _idleWorkers.Dequeue();
            var isProceedExecutable = false;
            if (proceeding.EProceeding == ProceedingType.Seeding)
            {
                isProceedExecutable = _plotManager.Attach(proceeding.EItemType);
            }

            if (isProceedExecutable == false)   
                return false;
            _workingWorkers.Enqueue(executableWorker);
            await UniTask.Delay(timeSpanDuration);
            executableWorker = _workingWorkers.Dequeue();
            _idleWorkers.Enqueue(executableWorker);
            return true;
        }

        public void RentWorker()
        {
            var worker = new Worker().BeBoughtBy<Worker>(_statManager.Gold);
            if (worker == null)
                return;
            _idleWorkers.Enqueue(worker);
            //TODO: save new <worker> to storage

        }


    }
    
}