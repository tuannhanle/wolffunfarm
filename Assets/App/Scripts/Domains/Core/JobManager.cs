using System;
using System.Collections.Generic;
using App.Scripts.Domains.Models;

namespace App.Scripts.Domains.Core
{
    public class JobManager : Dependency<JobManager>, IDependency
    {
        private HashSet<long> _executingJobs = new();

        public Job CreateJob(int workerId, int plotId, JobType jobType, string itemName = null)
        {
            var jobId = TimeStamp.SecondUTC()+plotId;
            var job = new Job(jobId, workerId, plotId, jobType, itemName, DateTime.UtcNow);
            if (_dataLoader.JobStorage.ContainsKey(jobId) == false)
            {
                _dataLoader.JobStorage.Add(jobId, job);
                _dataLoader.Push<Job>();
            }
            _executingJobs.Add(jobId);
            return job;
        }

        public bool RemoveJob(long jobId)
        {
            var result = _dataLoader.JobStorage.Remove(jobId);
            _executingJobs.Remove(jobId);
            _dataLoader.Push<Job>();
            return result;
        }

        /// <summary>
        /// Get a job that is excutable to work
        /// </summary>
        /// <returns>First job</returns>
        public Job GetJob()
        {
            foreach (var job in _dataLoader.JobStorage)
            {
                if (_executingJobs.Contains(job.Value.JobId))
                    continue;
                _executingJobs.Add(job.Key);
                return job.Value;
            }
            return null;
        }

        public Job GetJob(int workerId)
        {
            foreach (var job in _dataLoader.JobStorage)
            {
                if (job.Value.WorkerId != workerId)
                    continue;
                if (_executingJobs.Contains(job.Value.JobId))
                    continue;
                _executingJobs.Add(job.Key);
                return job.Value;
            }
            return null;
        }
        
    }
}