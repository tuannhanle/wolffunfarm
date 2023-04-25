namespace App.Scripts.Domains.Models
{
    public class Currency
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public int Capacity { get; set; }
    }

    public class Gold : Currency
    {
        
    }

    public class Worker : Currency
    {
        
    }

    public class Plot : Currency
    {
        
    }

    public class Seed : Currency
    {
        
    }
}