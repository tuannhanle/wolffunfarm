namespace App.Scripts.Domains.Models
{
    public class Worker : IBuyable
    {
        public string Name = "Worker";
        public static int Price { get; private set; } = 500;

    }
}