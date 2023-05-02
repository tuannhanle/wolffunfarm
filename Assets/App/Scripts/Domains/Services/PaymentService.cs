using App.Scripts.Domains.Core;
using App.Scripts.Domains.Models;

namespace App.Scripts.Domains.Services
{
    public interface IBuyable
    {
        int Price { get; set; }
    }
    public class PaymentService : Dependency<PaymentService>, IDependency
    {

        public void SellProducts()
        {
            // var blueberryStockAmount = _statManager.Stat.BlueberryProductAmount * _dataLoader.ItemCollection["Blueberry"].Stock;
            // var tomatoStockAmount = _statManager.Stat.TomotoProductAmount * _dataLoader.ItemCollection["Tomato"].Stock;
            // var strawberryStockAmount = _statManager.Stat.StrawberryProductAmount * _dataLoader.ItemCollection["Strawberry"].Stock;
            // var milkStockAmount = _statManager.Stat.MilkProductAmount * _dataLoader.ItemCollection["Cow"].Stock;
            // var sum = blueberryStockAmount + tomatoStockAmount + strawberryStockAmount + milkStockAmount;
            // _statManager.SellAllProduct(sum);
            // Earn(sum);
            Earn(0);
        }


        public bool Buy(IBuyable iBuyable)
        {
            var price = iBuyable.Price;
            if (IsPayable(price) == false)
                return false;
            _statManager.Pay(price);
            return true;
        }

        private bool IsPayable(int amount)
        {
            var result =  _dataLoader.stat.GoldAmount - amount >= 0;
            _dataLoader.Push<Stat>();
            return result;
        }
        
        private void Earn(int amount)
        {
            _statManager.Gain(amount);
        }
    }
}