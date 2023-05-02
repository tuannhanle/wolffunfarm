using System;

namespace App.Scripts.Domains.Models
{
    public enum JobType { PutIn, Harvasting}
    public class Job 
    {
        public long JobId;
        public int WorkerId;
        public int PlotId;
        public JobType JobType;
        public string ItemName;
        public long TakenAt;

        public Job(){}

        public Job(long jobId, int workerId, int plotId, JobType jobType, string itemName, long takenAt)
        {
            JobId = jobId;
            WorkerId = workerId;
            PlotId = plotId;
            JobType = jobType;
            ItemName = itemName;
            TakenAt = takenAt;
        }
    }
    
}