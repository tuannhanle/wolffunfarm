using System.Collections.Generic;

namespace App.Scripts.Domains.Models
{
    public enum JobType { Seeding, Collecting}
    public class Job
    {
        public JobType EJob { get; set; }
        public ItemType? EItemType { get; set; }

    }

    public class Progress
    {
        public Stack<Job> datas { get; set; } = new Stack<Job>();
    }
}