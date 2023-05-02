using System;
using System.Collections.Generic;
using App.Scripts.Domains.Models;

namespace App.Scripts.Domains.Core
{
    public class JobManager : Dependency<JobManager>, IDependency
    {
        private List<long> _executingJobs = new();
        private const string JOB = "Job";

        public Job CreateJob(int workerId, int plotId, JobType jobType, string itemName = null)
        {
            var jobId = TimeStamp.SecondUTC()+plotId;
            var job = new Job(jobId, workerId, plotId, jobType, itemName, TimeStamp.SecondUTC());
            if (_dataLoader.JobStorage.ContainsKey(jobId) == false)
            {
                
                _dataLoader.JobStorage.Add(jobId, job);
                _dataLoader.Push<Job>();
            }
            return job;
        }

        public bool RemoveJob(long jobId)
        {
            var result = _dataLoader.JobStorage.Remove(jobId);
            _executingJobs.Remove(jobId);
            _dataLoader.Push<Job>();
            return result;
        }

        public Job GetJob(int workerId)
        {
            foreach (var job in _dataLoader.JobStorage)
            {
                if (job.Value.WorkerId != workerId)
                    continue;
                if (IsExecutingJob(job.Value.JobId))
                    continue;
                _executingJobs.Add(job.Key);
                return job.Value;
            }
            return null;
        }

        private bool IsExecutingJob(long jobId)
        {
            foreach (var executingJobId in _executingJobs)
            {
                if (jobId != executingJobId)
                    continue;
                return true;
            }
            return false;
        }
    }
}