namespace App.Scripts.Domains.Models
{
    public class Worker : IBuyable
    {
        public string Name = "Worker";
        public int Price { get; private set; } = 500;

    }
}