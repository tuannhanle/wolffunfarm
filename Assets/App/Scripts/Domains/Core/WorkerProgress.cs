using System;
using System.Collections.Generic;
using App.Scripts.Domains.Models;
using Cysharp.Threading.Tasks;
using Progress = App.Scripts.Domains.Models.Progress;

namespace App.Scripts.Domains.Core
{
    public class WorkerProgress 
    {
        private Queue<Worker> _idleWorkers = new Queue<Worker>();
        private Queue<Worker> _workingWorkers = new Queue<Worker>();

        private bool _isInit = false;
        private int _amountWorker = 0;
        private const float DURATION_WORKER = 120f;
        
        // TODO: Get amount of worker from db
        public int GetAmountWorkerDB { get { return 1; } }

        
        public void Init()
        {
            _isInit = true;
            _amountWorker = _amountWorker == 0 ? GetAmountWorkerDB : _amountWorker;
            for (int i = 0; i < _amountWorker; i++)
            {
                _idleWorkers.Enqueue(new Worker());
            }
        }
        
        public async UniTask TakeProgressAsync(Progress progress)
        {
            if (_isInit == false) Init();
            while (progress.datas.Count > 0)
            {
                var proceeding = progress.datas.Pop();
                await UniTask.Yield();
            }
            
        }

        private float GetDuration()
        {
            var toolLevel = 1f;
            var toolPercent = 100f + (toolLevel - 1f) * 10f;
            return toolPercent;
        }
        
        private async UniTask WorkerExecuteAsync(Proceeding proceeding)
        {
            var executableWorker = _idleWorkers.Dequeue();
            _workingWorkers.Enqueue(executableWorker);
            var timeSpanDuration = TimeSpan.FromSeconds(GetDuration());
            await UniTask.Delay(timeSpanDuration);
            executableWorker = _workingWorkers.Dequeue();
            _idleWorkers.Enqueue(executableWorker);
        }

    }
}