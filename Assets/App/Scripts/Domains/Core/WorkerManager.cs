using System;
using System.Collections.Generic;
using App.Scripts.Domains.Models;
using App.Scripts.Mics;
using Cysharp.Threading.Tasks;
using Progress = App.Scripts.Domains.Models.Progress;

namespace App.Scripts.Domains.Core
{
    public class WorkerManager : Dependency<WorkerManager>, IDependency
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
        
        public async UniTask Assign(Job job)
        {
            if (job.EJob == JobType.Seeding)
            {
                var isCow = job.EItemType == ItemType.Cow && _statManager.Stat.UnusedCowAmount > 0;
                var isTomato = job.EItemType == ItemType.Tomato && _statManager.Stat.UnusedTomatoAmount > 0;
                var isBlueberry = job.EItemType == ItemType.BlueBerry && _statManager.Stat.UnusedBlueberryAmount > 0;
                var isStrawberry = job.EItemType == ItemType.StrawBerry && _statManager.Stat.UnusedStrawberryAmount > 0;
                if (isCow || isTomato || isBlueberry || isStrawberry)
                {
                    _progress.datas.Push(job);
                    TakeProgressAsync(_progress).Forget();
                }
            }
            else if(job.EJob == JobType.Collecting)
            {
                var isCow = job.EItemType == ItemType.Cow && _statManager.Stat.MilkProductAmount > 0;
                var isTomato = job.EItemType == ItemType.Tomato && _statManager.Stat.TomotoProductAmount > 0;
                var isBlueberry = job.EItemType == ItemType.BlueBerry && _statManager.Stat.BlueberryProductAmount > 0;
                var isStrawberry = job.EItemType == ItemType.StrawBerry && _statManager.Stat.StrawberryProductAmount > 0;
                if (isCow || isTomato || isBlueberry || isStrawberry)
                {
                    _progress.datas.Push(job);
                    TakeProgressAsync(_progress).Forget();
                }
            }
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
        private async UniTask<bool> WorkerExecuteAsync(Job job)
        {
            var numberOfProceed = 0;
            if (isWorkerExecutable == false)
                return false;
            switch (job.EJob)
            {
                case JobType.Seeding:
                    numberOfProceed = _plotManager.Attach(job.EItemType);
                    break;
                case JobType.Collecting:
                    numberOfProceed = _plotManager.Collect(job.EItemType);
                    break;
            }
            if (numberOfProceed == 0)   
                return false;
            var delayTime = TimeSpan.FromSeconds(DURATION_WORKER * numberOfProceed) ;
            await UseWorker(delayTime);
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

        private async UniTask UseWorker(TimeSpan delayTime)
        {
            var worker = _idleWorkers.Dequeue();
            _workingWorkers.Enqueue(worker);
            _statManager.Gain<Worker>(-1);
            await UniTask.Delay(delayTime);
            worker = _workingWorkers.Dequeue();
            _idleWorkers.Enqueue(worker);
            _statManager.Gain<Worker>(1);
            await UniTask.Yield();

        }


    }
    
}