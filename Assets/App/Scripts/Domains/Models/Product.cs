using System;

namespace App.Scripts.Domains.Models
{
    public class Product
    {
        public ItemType ItemType { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsActive => Amount > 0;

    }
}