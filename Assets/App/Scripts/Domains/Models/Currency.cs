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

        public void Gain(int amount)
        {
            this.Amount += amount;
        }

        public bool Pay(int amount)
        {
            if (IsPayable(amount) == false)
                return false;
            this.Amount -= amount;
            return true;
        }

        public bool IsPayable(int amount)
        {
            return this.Amount - amount >= 0;
        }
    }

    public class Plot : Currency
    {
        
    }

    public class Seed : Currency
    {
        
    }
}