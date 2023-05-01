using System;
using App.Scripts.Domains.Services;

namespace App.Scripts.Domains.Models
{
    public class Worker : Unit, IBuyable
    {
        public string Name = "Worker";
        // public long RemainTime { get; set; } = 0;// second
        public int Price { get; set; }

        private DateTime StartExecuteAt;
        private DateTime EndExecuteAt;

        public Worker(int priceAmount=500) : base(priceAmount)
        {
            Price = priceAmount;
        }

        public bool Execute(Job job)
        {
            StartExecuteAt = DateTime.UtcNow;
            EndExecuteAt = TimeStamp.DateTimeFromSeconds(StartExecuteAt.Second + 60 * 2);
            return true;
        }
    }
}