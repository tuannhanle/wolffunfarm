using System.Collections.Generic;

namespace App.Scripts.Domains.Models
{
    public enum JobType { PutIn, Harvesting}
    public class Job
    {
        public JobType EJob { get; set; }
        public ItemType? EItemType { get; set; }

    }
    
}