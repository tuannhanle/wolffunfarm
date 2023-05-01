using System;
using System.Collections.Generic;
using App.Scripts.Domains.Models;
using App.Scripts.Mics;
using Cysharp.Threading.Tasks;
using Progress = App.Scripts.Domains.Models.Progress;

namespace App.Scripts.Domains.Core
{
    public class WorkerManager : Dependency<WorkerManager>
    {
        private readonly Queue<Worker> _idleWorkers = new();
        private readonly Queue<Worker> _workingWorkers = new();
        private readonly Progress _progress = new();

        private const float DURATION_WORKER = 120f;

        private bool isWorkerExecutable => _idleWorkers.Count > 0;
        private const int AMOUNT_EACH_WORKER_FOR_RENT = 1;



        public void Init()
        {
            base.Init();
            for (int i = 0; i < _statManager.Stat.IdleWorkerAmount; i++)
            {
                _idleWorkers.Enqueue(new ());
            }
            for (int i = 0; i < _statManager.Stat.WorkingWorkerAmount; i++)
            {
                _workingWorkers.Enqueue(new ());
            }
        }
        
        public async UniTask TakeProceedingAsync(Proceeding proceeding)
        {
            _progress.datas.Push(proceeding);
            TakeProgressAsync(_progress).Forget();
        }
        
        private async UniTask TakeProgressAsync(Progress progress)
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

        //TODO: worker has still not execute the proceed
        private async UniTask<bool> WorkerExecuteAsync(Proceeding proceeding)
        {
            var numberOfProceed = 0;
            if (isWorkerExecutable == false)
                return false;
            var executableWorker = _idleWorkers.Dequeue();
            switch (proceeding.EProceeding)
            {
                case ProceedingType.Seeding:
                    numberOfProceed = _plotManager.Attach(proceeding.EItemType);
                    break;
                case ProceedingType.Collecting:
                    numberOfProceed = _plotManager.Collect(proceeding.EItemType);
                    break;
            }
            if (numberOfProceed == 0)   
                return false;
            _workingWorkers.Enqueue(executableWorker);
            await UniTask.Delay(TimeSpan.FromSeconds(DURATION_WORKER) * numberOfProceed);
            executableWorker = _workingWorkers.Dequeue();
            _idleWorkers.Enqueue(executableWorker);
            return true;
        }

        public void RentWorker()
        {
            var worker = Define.WorkerItem;
            var isPayable = _paymentService.Buy(worker);
            if (isPayable)
            {
                _idleWorkers.Enqueue(worker);
                _statManager.Gain<Worker>(AMOUNT_EACH_WORKER_FOR_RENT);
            }
        }


    }
    
}