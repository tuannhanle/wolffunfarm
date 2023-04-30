using System;

namespace App.Scripts.Domains.Models
{
    public class Unit 
    {
        protected int Price { get; set;}
        
        public Unit(int priceAmount)
        {
            Price = priceAmount;
        }
    }
}