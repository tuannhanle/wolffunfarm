using System;
using System.Collections.Generic;

namespace App.Scripts.Domains.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}