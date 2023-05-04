using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using App.Scripts.Domains.GameObjects;
using App.Scripts.Domains.Models;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Domains.Core
{
    public class WorkerManager : Dependency<WorkerManager>, IDependency
    {
        private const string WORKER = "Worker";

        private async UniTask Execute(Job job, Worker worker)
        {
            // executing delay time
            if (job.JobType == JobType.PutIn)
                _plotManager.PutIn(true, job.PlotId, job.ItemName);
            if (job.JobType == JobType.PutOut)
                _plotManager.PutOut(true, job.PlotId, job.ItemName);
            // wait
            var durationWork = _dataLoader.stat.JobDuration;
            var delayTime = TimeSpan.FromSeconds(durationWork);
            await UniTask.Delay(delayTime);
                
            // after that
            if (job.JobType == JobType.PutIn)
                _plotManager.PutIn(false, job.PlotId, job.ItemName);
            if (job.JobType == JobType.PutOut)
                _plotManager.PutOut(false, job.PlotId, job.ItemName);
            ReturnIdleWorker(worker);
            _jobManager.RemoveJob(job.JobId);
        }
        
        /// <summary> This method assigns a worker to a specific task. </summary>
        public void Assign(JobType jobType, string itemName)
        {
            // Get a list of plots that match the given jobType and itemName.
            var plots = _plotManager.GetPlots(jobType, itemName);

            // If no matching plots are found, return without assigning the task.
            if (plots == null)
                return;
            
            // For each matching plot, create a job and assign it to the idle worker.
            foreach (var plot in plots)
            {
                if(_dataLoader.ItemStorage.TryGetValue(itemName, out var itemStorage) ==false)
                    return;
                if (jobType == JobType.PutIn && itemStorage.UnusedAmount <= 0)
                    return;
                // Get an idle worker.
                var idleWorker = GetIdleWorker();

                // If no idle worker is available, return without assigning the task.
                if (idleWorker == null)
                    return;
                
                // Create a job for the current plot.
                var job = _jobManager.CreateJob(idleWorker.Id, plot.Id, jobType, itemName);
                Execute(job, idleWorker).Forget();
                
                if (jobType == JobType.PutIn)
                    break;
                
            }
        }
        
        public void RentWorker()
        {
            var workerItem = _dataLoader.ItemCollection[WORKER];
            var isPayable = _paymentService.Buy(workerItem);
            if (isPayable == false)
                return;
            var isSuccess = GainIdleWorker();
        }
        
        private Worker GetIdleWorker()
        {
            foreach (var pair in _dataLoader.WorkerStorage)
            {
                var worker = pair.Value;
                if (worker.IsUsing == false)
                {
                    worker.IsUsing = true;
                    _dataLoader.Push<Worker>();
                    return worker;
                }
            }
            return null;
        }


        private void ReturnIdleWorker(Worker workingWorker)
        {
            workingWorker.IsUsing = false;
            _dataLoader.Push<Worker>();
        }
        
        private bool GainIdleWorker()
        {
            var workerId = _dataLoader.WorkerStorage.Count;
            var worker = new Worker(workerId, false);
            if (_dataLoader.WorkerStorage.ContainsKey(workerId))
                return false;
            _dataLoader.WorkerStorage.Add(workerId, worker);
            ReturnIdleWorker(worker);
            return true;
        }
    }
    
}