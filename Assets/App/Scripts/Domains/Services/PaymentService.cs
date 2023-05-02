using App.Scripts.Domains.Core;
using App.Scripts.Domains.Models;
using App.Scripts.Mics;
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

        private long GoldAmount => Gold.Amount;

        public void Init()
        {
            base.Init();
            Gold = new Gold()
            {
                Amount = _statManager.Stat.GoldAmount,
                Name = "Gold"
            };
        }

        public void Earn(int amount)
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

        public void SellProducts()
        {
            var blueberryStockAmount = _statManager.Stat.BlueberryProductAmount * Define.BlueBerryItem.Stock;
            var tomatoStockAmount = _statManager.Stat.TomotoProductAmount * Define.TomatoItem.Stock;
            var strawberryStockAmount = _statManager.Stat.StrawberryProductAmount * Define.StrawBerryItem.Stock;
            var milkStockAmount = _statManager.Stat.MilkProductAmount * Define.CowItem.Stock;
            var sum = blueberryStockAmount + tomatoStockAmount + strawberryStockAmount + milkStockAmount;
            _statManager.SellAllProduct(sum);
            Earn(sum);
        }
    }
}