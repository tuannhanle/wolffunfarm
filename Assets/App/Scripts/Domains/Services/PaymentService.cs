using App.Scripts.Domains.Core;
using App.Scripts.Domains.Models;
using UnityEngine.Playables;

namespace App.Scripts.Domains.Services
{
    public interface IBuyable
    {
        int Price { get; set; }
    }
    public class PaymentService : Dependency<PaymentService>, IDependency
    {
        private Gold Gold { get; set; }

        private int GoldAmount => Gold.Amount;

        public void Init()
        {
            base.Init();
            Gold = new Gold()
            {
                Amount = _statManager.Stat.GoldAmount,
                Name = "Gold"
            };
        }

        public void Gain(int amount)
        {
            Gold.Amount += amount;
        }

        public bool Buy(IBuyable iBuyable)
        {
            var price = iBuyable.Price;
            if (IsPayable(price) == false)
                return false;
            Gold.Amount -= price;
            return true;
        }

        private bool IsPayable(int amount)
        {
            return Gold.Amount - amount >= 0;
        }
    }
}