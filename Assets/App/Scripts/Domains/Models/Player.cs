using System;

namespace App.Scripts.Domains.Models
{
    public class Player 
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}