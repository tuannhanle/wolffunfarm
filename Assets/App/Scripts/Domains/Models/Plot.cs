using System;
using App.Scripts.Domains.Core;
using App.Scripts.Domains.Models;
using UnityEditor.Experimental.GraphView;

namespace App.Scripts.Domains.GameObjects
{
    public class Plot : Stuff
    {
        public Crop Crop { get; private set; }
        public long TimeUntilHarvest { get; set; } // millisecond
        
        // override int Price { get; private set; } = 500;


        private bool _isGrowable => Crop == null;

        private const int EXTEND_TIME_TO_SELF_DESTROY = 3600; // second
        public Plot(int amount = 500) : base(amount)
        {
            Price = amount;
            Crop = null;
            TimeUntilHarvest = 0;
        }

        public Plot(Crop crop, int amount = 500) : base(amount)
        {
            Price = amount;
            Crop = crop;
        }
        
        public void TakeAmountProduct()
        {
            var item = Crop.Item;
            var deltaTime = TimeStamp.Second(DateTime.UtcNow) - TimeStamp.Second(Crop.UpdatedAt??Crop.CreateAt);
            var productHasBeenCollectable = (int) (deltaTime / item.TimePerProduct);
            var realProductAmount = productHasBeenCollectable > item.ProductCapacity
                ? item.ProductCapacity
                : productHasBeenCollectable;
            this.Crop.Product = new Product()
            {
                ItemType = item.ItemType, 
                Amount = realProductAmount
            };
            Crop.UpdatedAt = DateTime.UtcNow;
        }

        public bool PlantCrop(ItemType? itemType)
        {
            if (itemType == null)
                return false;
            if (_isGrowable && TimeUntilHarvest == 0)
            {
                var item = Item.ConvertItemType(itemType);
                if (itemType == null)
                    return false;
                Crop = new Crop(item);
                TimeUntilHarvest = item.TimePerProduct * item.ProductCapacity + EXTEND_TIME_TO_SELF_DESTROY;
                return true;
            }
            return false;
        }

        
        public bool Harvest()
        {
            if (!_isGrowable && TimeUntilHarvest == 0)
            {
                Crop = null;
                TimeUntilHarvest = 0;
                return true;
            }
            return false;
        }
        
    }
}