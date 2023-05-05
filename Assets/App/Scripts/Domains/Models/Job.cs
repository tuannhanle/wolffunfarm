using System;

namespace App.Scripts.Domains.Models
{
    public enum JobType { PutIn, PutOut}
    public class Job 
    {
        public long JobId;
        public string ItemName;
        public int WorkerId;
        public int PlotId;
        public JobType JobType;
        public DateTime TakenAt;
        // public bool IsDone;

        public Job(){}

        public Job(long jobId, int workerId, int plotId, JobType jobType, string itemName, DateTime takenAt)
        {
            JobId = jobId;
            WorkerId = workerId;
            PlotId = plotId;
            JobType = jobType;
            ItemName = itemName;
            TakenAt = takenAt;
            // IsDone = isDone;
        }
    }
    
}