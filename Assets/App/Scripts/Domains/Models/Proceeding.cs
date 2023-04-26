using System.Collections.Generic;

namespace App.Scripts.Domains.Models
{
    public enum ProceedingType { Seeding, Collecting, Milking}
    public class Proceeding
    {
        public ProceedingType EProceeding { get; set; }

    }

    public class Progress
    {
        public Stack<Proceeding> datas { get; set; } = new Stack<Proceeding>();
    }
}