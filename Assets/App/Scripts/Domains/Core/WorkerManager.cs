using System;
using System.Collections.Generic;
using App.Scripts.Domains.Models;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Domains.Core
{
    public class WorkerManager : Dependency<WorkerManager>, IDependency
    {
        private readonly Queue<Worker> _idleWorkers = new();
        private readonly Queue<Worker> _workingWorkers = new();

        private bool isWorkerExecutable => _idleWorkers.Count > 0;
        private const string WORKER = "Worker";

        public void Init()
        {
            base.Init();
            foreach (var worker in _dataLoader.WorkerStorage)
            {
                if (worker.Value.IsUsing)
                {
                    _workingWorkers.Enqueue(new Worker(worker.Value.Id, worker.Value.IsUsing));
                }
                else
                {
                    _idleWorkers.Enqueue(new Worker(worker.Value.Id, worker.Value.IsUsing));
                }
            }
        }
        
        
        //TODO: do not use async to remvoe job, use date time onlhy
        public async UniTask ExecuteAsync()
        {
            if (_dataLoader.JobStorage.Count == 0)
                return;
            if (isWorkerExecutable == false) // next frame, it will be back here to check
                return;
            var idleWorker = GetIdleWorker();
            if (idleWorker == null)
            {
                ReturnIdleWorker();
                return;
            }
            var job = _jobManager.GetJob(idleWorker.Id) ?? _jobManager.GetJob(-1);
            if (job == null) // not any job!
            {
                ReturnIdleWorker();
                return;
            }
            var hasExecuted = idleWorker.Execute();
            if (hasExecuted)
            {
                // executing delay time
                if (job.JobType == JobType.PutIn)
                {
                    if (_plotManager.IsPutInable(job.PlotId, job.ItemName) == false)
                    {
                        ReturnIdleWorker();
                        return;
                    }
                }
                else
                {
                    if (_plotManager.IsHarvastable(job.PlotId, job.ItemName) == false)
                    {
                        ReturnIdleWorker();
                        return;
                    }
                }

                var durationWork = _dataLoader.stat.JobDuration;
                var delayTime = TimeSpan.FromSeconds(durationWork);
                await UniTask.Delay(delayTime);
                
                // after that
                if (job.JobType == JobType.PutIn)
                    _plotManager.PutIn(job.PlotId, job.ItemName);
                if (job.JobType == JobType.Harvasting)
                    _plotManager.Harvast(job.PlotId, job.ItemName);
        
                _jobManager.RemoveJob(job.JobId);
            }
            ReturnIdleWorker();
        }
        
        public void Assign(JobType jobType, string itemName)
        {
            var idleWorker = GetIdleWorker();
            if (idleWorker == null)
                return;
            var plots = _plotManager.GetPlots(jobType, itemName);
            if (plots == null)
                return;
            // if (jobType == JobType.PutIn)
            // {
            //     int workerId = idleWorker != null ? idleWorker.Id : -1;
            //     var job = _jobManager.CreateJob( workerId, plots[0].Id, jobType, itemName);
            //     return;
            // }
            foreach (var plot in plots)
            {
                // if workerId = -1 => no ready worker for this job
                int workerId = idleWorker != null ? idleWorker.Id : -1;
                var job = _jobManager.CreateJob( workerId, plot.Id, jobType, itemName);
            }
        }
        
        public void RentWorker()
        {
            var workerItem = _dataLoader.ItemCollection[WORKER];
            var isPayable = _paymentService.Buy(workerItem);
            if (isPayable == false)
                return;
            var id = _dataLoader.WorkerStorage.Count;
            var worker = new Worker(id, false);
            _idleWorkers.Enqueue(worker);
            _dataLoader.WorkerStorage.Add(id, worker);
            _dataLoader.Push<Worker>();
        }

        private Worker GetIdleWorker()
        {
            if (_idleWorkers.Count <= 0)
                return null;
            var worker = _idleWorkers.Dequeue();
            _workingWorkers.Enqueue(worker);
            worker.IsUsing = true;
            _dataLoader.Push<Worker>();
            return worker;
        }

        private void ReturnIdleWorker()
        {
            if (_workingWorkers.Count <= 0)
                return;
            var worker = _workingWorkers.Dequeue();
            _idleWorkers.Enqueue(worker);
            _dataLoader.Push<Worker>();
            worker.IsUsing = false;
        }
    }
    
}