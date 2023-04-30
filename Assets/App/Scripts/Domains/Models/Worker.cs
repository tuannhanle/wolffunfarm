using System;
using App.Scripts.Domains.Services;

namespace App.Scripts.Domains.Models
{
    public class Worker : Unit, IBuyable
    {
        public string Name = "Worker";
        public long RemainTime { get; set; }
        public int Price { get; set; }


        // public T BeBoughtBy<T>(Gold gold) where T : class
        // {
        //     if (gold == null)
        //         throw new ArgumentNullException(nameof(gold)); // input validation
        //
        //     if (gold.Amount < Price)
        //         return null;
        //
        //     gold.Pay(Price);
        //     return this as T;
        // }

        public Worker(int priceAmount=500) : base(priceAmount)
        {
        }

    }
}