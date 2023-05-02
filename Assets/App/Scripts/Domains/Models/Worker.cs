using App.Scripts.Domains.Services;

namespace App.Scripts.Domains.Models
{
    public class Worker : IBuyable
    {
        public int Id;
        public string Name = "Worker";
        // public long RemainTime { get; set; } = 0;// second
        public int Price { get; set; }

        // private DateTime StartExecuteAt;
        // private DateTime EndExecuteAt;
        public bool IsUsing;

        public Worker() { }

        public Worker(int id, bool isUsing)
        {
            Id = id;
            IsUsing = isUsing;
        }

        public bool Execute()
        {
            IsUsing = true;
            // StartExecuteAt = DateTime.UtcNow;
            // EndExecuteAt = TimeStamp.DateTimeFromSeconds(StartExecuteAt.Second + 60 * 2);
            return true;
        }
    }
}