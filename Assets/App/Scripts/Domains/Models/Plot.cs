using System;
using App.Scripts.Domains.Core;
using App.Scripts.Domains.Models;
using App.Scripts.Domains.Services;
using UnityEditor.Experimental.GraphView;

namespace App.Scripts.Domains.GameObjects
{
    public class Plot : Unit, IBuyable
    {
        public Crop Crop { get; private set; }
        public int Price { get; set; }
        public long TimeUntilHarvest { get; set; } // millisecond
        
        // override int Price { get; private set; } = 500;


        private bool _isGrowable => Crop == null;
        public bool IsCollectable(ItemType? itemType) => Crop?.Item?.ItemType == itemType;

        private const int EXTEND_TIME_TO_SELF_DESTROY = 3600; // second
        public Plot(int priceAmount) : base(priceAmount)
        {
            Price = priceAmount;
            Crop = null;
            TimeUntilHarvest = 0;
        }

        public Plot(Crop crop, int priceAmount = 500) : base(priceAmount)
        {
            Price = priceAmount;
            Crop = crop;
        }
        
        public void TakeAmountProduct()
        {
            var item = Crop.Item;
            var timePerProductWasImproved = GetTimePerProductWasImprove();
            var deltaTime = TimeStamp.Second(DateTime.UtcNow) - TimeStamp.Second(Crop.UpdatedAt??Crop.CreateAt);
            var productHasBeenCollectable = (int) ((float)deltaTime / timePerProductWasImproved);
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
                var timePerProductWasImproved = GetTimePerProductWasImprove();
                TimeUntilHarvest = (long)(timePerProductWasImproved * (float)item.ProductCapacity + (float)EXTEND_TIME_TO_SELF_DESTROY);
                return true;
            }
            return false;
        }
        
        public bool Harvest(ItemType? itemType)
        {
            if (itemType == null)
                return false;
            if (!_isGrowable && TimeUntilHarvest == 0)
            {
                Crop = null;
                TimeUntilHarvest = 0;
                return true;
            }
            return false;
        }

        private float GetTimePerProductWasImprove()
        {
            var toolImprovePercent = DependencyProvider.Instance.GetDependency<ToolManager>().GetPercentPerLevel;
            return (float)Crop.Item.TimePerProduct * (toolImprovePercent / 100f);
        }

    }
}