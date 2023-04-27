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
        private Queue<Worker> _idleWorkers = new();
        private Queue<Worker> _workingWorkers = new();

        private Progress _progress;

        private bool _isInit = false;
        private int _amountWorker = 0;
        private const float DURATION_WORKER = 120f;
        
        // TODO: Get amount of worker from db
        public int GetAmountWorkerDB { get { return 1; } }

        public WorkerManager()
        {
            _idleWorkers = new Queue<Worker>();
            _workingWorkers = new Queue<Worker>();
            _progress = new Progress();
            _isInit = true;
            _amountWorker = _amountWorker == 0 ? GetAmountWorkerDB : _amountWorker;
            GainWorker(_amountWorker);            
        }

        private void GainWorker(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                _idleWorkers.Enqueue(new ());
            }
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
            if (_idleWorkers.Count > 0)
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

        // TODO: get it from DB
        private int _amountMoney = 1000;
        public int AmountMoney => _amountMoney;
        
        public void RentWorker(ShareData.InteractEventType? EInteractEvent)
        {
            if (EInteractEvent != ShareData.InteractEventType.RentWorker)
                return;
            if (_amountMoney < Worker.Price)
                return;
            _amountMoney -= Worker.Price;
            //TODO: save new <worker> to storage
            GainWorker(1);
            
        }

        public void InitVeryFirstLogin()
        {
            TakeProceedingAsync(new () { EProceeding = ProceedingType.Seeding }).Forget();
            TakeProceedingAsync(new () { EProceeding = ProceedingType.Seeding }).Forget();
            TakeProceedingAsync(new () { EProceeding = ProceedingType.Seeding }).Forget();
        }
    }
    
}