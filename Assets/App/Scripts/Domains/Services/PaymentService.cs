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
            var stock = 0;
            foreach (var pair in _dataLoader.ItemStorage)
            {
                var itemStorage = pair.Value;
                if (itemStorage.HarvestedProduct == 0)
                    continue;
                stock += SellProduct(itemStorage);
                itemStorage.HarvestedProduct = 0;
            }
            if (stock == 0)
                return;
            _dataLoader.Push<ItemStorage>();
            _statManager.Gain(stock);
        }

        private int SellProduct(ItemStorage itemStorage)
        {
            var item = _dataLoader.ItemCollection[itemStorage.ItemName];
            var productAmount = itemStorage.HarvestedProduct;
            return productAmount * item.Stock;
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
            return result;
        }
        
  
    }
}