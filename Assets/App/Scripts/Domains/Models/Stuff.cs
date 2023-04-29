using System;

namespace App.Scripts.Domains.Models
{
    public class Stuff 
    {
        protected int Price { get; set;}

        protected Stuff(int amount)
        {
            Price = amount;
        }

        public T BeBoughtBy<T>(Gold gold) where T : class
        {
            if (gold == null)
                throw new ArgumentNullException(nameof(gold)); // input validation
        
            if (gold.Amount < Price)
                return null;
        
            gold.Pay(Price);
            return this as T;
        }
        
    }
}