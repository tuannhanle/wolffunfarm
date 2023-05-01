using System;
using System.Collections.Generic;
using App.Scripts.Domains.Models;
using App.Scripts.Mics;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Domains.Core
{
    public class WorkerManager : Dependency<WorkerManager>, IDependency
    {
        private readonly Queue<Worker> _idleWorkers = new();
        private readonly Queue<Worker> _workingWorkers = new();
        private Stack<Job> _jobs { get; set; } = new();


        private bool isWorkerExecutable => _idleWorkers.Count > 0;
        private const int AMOUNT_EACH_WORKER_FOR_RENT = 1;
        private const float DURATION_WORKER = 120f;



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
        
        public async UniTask ExecuteAsync()
        {
            if (_jobs.Count <= 0)
                return;
            var job = _jobs.Pop();
            await UniTask.WaitUntil(() => isWorkerExecutable == true);
            var idleWorker = GetIdleWorker();
            var hasExecuted = idleWorker.Execute(job);
            
            if (hasExecuted)
            {
                if (job.EJob == JobType.PutIn)
                    _plotManager.Attach(job.EItemType);
                if (job.EJob == JobType.Harvesting)
                    _plotManager.Collect(job.EItemType);
                var delayTime = TimeSpan.FromSeconds(DURATION_WORKER);
                await UniTask.Delay(delayTime);
            }
            ReturnIdleWorker();
        }
        
        public async UniTask Assign(Job job)
        {
            // dieu kien seeding
            // seed > 0 với loại seed muốn seeding
            // unused plot > 0
            
            if (job.EJob == JobType.PutIn)
            {
                var isCow = job.EItemType == ItemType.Cow && _statManager.Stat.UnusedCowAmount > 0;
                var isTomato = job.EItemType == ItemType.Tomato && _statManager.Stat.UnusedTomatoAmount > 0;
                var isBlueberry = job.EItemType == ItemType.BlueBerry && _statManager.Stat.UnusedBlueberryAmount > 0;
                var isStrawberry = job.EItemType == ItemType.StrawBerry && _statManager.Stat.UnusedStrawberryAmount > 0;
                if (isCow || isTomato || isBlueberry || isStrawberry)
                {
                    _jobs.Push(job);
                }
            }
            else if(job.EJob == JobType.Harvesting)
            {
                // TODO: this is being wrong, fix later
                // check for each plot on field
                var isCow = job.EItemType == ItemType.Cow && _statManager.Stat.MilkProductAmount > 0;
                var isTomato = job.EItemType == ItemType.Tomato && _statManager.Stat.TomotoProductAmount > 0;
                var isBlueberry = job.EItemType == ItemType.BlueBerry && _statManager.Stat.BlueberryProductAmount > 0;
                var isStrawberry = job.EItemType == ItemType.StrawBerry && _statManager.Stat.StrawberryProductAmount > 0;
                if (isCow || isTomato || isBlueberry || isStrawberry)
                {
                    _jobs.Push(job);
                }
            }
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

        private Worker GetIdleWorker()
        {
            var worker = _idleWorkers.Dequeue();
            _workingWorkers.Enqueue(worker);
            _statManager.Gain<Worker>(-1);
            return worker;
        }

        private void ReturnIdleWorker()
        {
            var worker = _workingWorkers.Dequeue();
            _idleWorkers.Enqueue(worker);
            _statManager.Gain<Worker>(1);
        }
        
        // private async UniTask TakeProgressAsync()
        // {
        //     while (_jobs.Count > 0)
        //     {
        //         var proceeding = _jobs.Pop();
        //         if (await WorkerExecuteAsync(proceeding) == false)
        //         {
        //             _jobs.Push(proceeding);
        //         }
        //         await UniTask.Yield();
        //     }
        //     
        // }

        //TODO: worker has still not execute the proceed
        // private async UniTask<bool> WorkerExecuteAsync(Job job)
        // {
        //     var numberOfProceed = 0;
        //     if (isWorkerExecutable == false)
        //         return false;
        //
        //     if (numberOfProceed == 0)   
        //         return false;
        //     var delayTime = TimeSpan.FromSeconds(DURATION_WORKER * numberOfProceed) ;
        //     return true;
        // }



    }
    
}