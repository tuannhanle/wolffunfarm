using System;

namespace App.Scripts.Domains.Models
{
    public class Stuff 
    {
        protected int Price { get; set;}

        public Stuff(int amount)
        {
            Price = amount;
        }

        public Stuff BeBoughtBy(Gold gold)
        {
            if (gold == null)
                throw new ArgumentNullException(nameof(gold)); // input validation
        
            if (gold.Amount < Price)
                return null;
        
            gold.Pay(Price);
            return this;
        }
        
    }
}