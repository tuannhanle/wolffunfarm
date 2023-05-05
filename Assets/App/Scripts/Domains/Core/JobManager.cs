using System;
using System.Collections.Generic;
using App.Scripts.Domains.Models;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Domains.Core
{
    public class JobManager : Dependency<JobManager>, IDependency
    {
        // private HashSet<long> _executingJobs = new();

        public void CheckJobToDo()
        {
            var clone = new Dictionary<long, Job>(_dataLoader.JobStorage);
            foreach (var pair in clone)
            {
                var job = pair.Value;
                var isJobFinished = IsJobFinish(job);
                if (isJobFinished)
                {
                    _plotManager.ReturnUnusedPlot(job.PlotId);
                    _workerManager.Complete(job.JobId);
                    _workerManager.ReturnIdleWorker(job.WorkerId);
                    RemoveJob(job.JobId);
                }
            }
        }
        
        public bool IsJobFinish(Job job)
        {

            var takenAt = job.TakenAt;
            var jobDuration = _dataLoader.stat.JobDuration;
            var doneAt = takenAt.AddSeconds(jobDuration);
            var isOutOfTime = DateTime.UtcNow <= takenAt || DateTime.UtcNow >= doneAt;
            return isOutOfTime;
        }
        
        public Job CreateJob(int workerId, int plotId, JobType jobType, string itemName = null)
        {
            var jobId = TimeStamp.SecondUTC()+plotId;
            var job = new Job(jobId, workerId, plotId, jobType, itemName, DateTime.UtcNow);
            if (_dataLoader.JobStorage.ContainsKey(jobId) == false)
            {
                _dataLoader.JobStorage.Add(jobId, job);
                _dataLoader.Push<Job>();
            }
            // _executingJobs.Add(jobId);
            return job;
        }

        public bool RemoveJob(long jobId)
        {
            var result = _dataLoader.JobStorage.Remove(jobId);
            // _executingJobs.Remove(jobId);
            _dataLoader.Push<Job>();
            return result;
        }

    }
}