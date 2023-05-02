using System;
using System.Collections.Generic;
using App.Scripts.Domains.Core;
using App.Scripts.Domains.Models;
using App.Scripts.Domains.Services;
using UnityEditor.Experimental.GraphView;

namespace App.Scripts.Domains.GameObjects
{
    public class Plot : IHasItemName, IBuyable
    {
        public int Id;
        public string ItemName { get; set; }
        public int ProductAmount;
        public long? CreateAt { get; set; } 
        public long? UpdatedAt { get; set; }
        public Item Item { get; set; }

        public Queue<DateTime> CollectTimeQueue = new();
        public int Price { get; set; }
        public long TimeUntilHarvest { get; set; } // millisecond
        
        // override int Price { get; private set; } = 500;


        private bool _isGrowable => ItemName == null;
        public bool IsCollectable(ItemType? itemType) =>Item?.ItemType == itemType;

        private const int EXTEND_TIME_TO_SELF_DESTROY = 3600; // second
        public Plot() { }

        public Plot(int Id,string ItemName,int ProductAmount,long TimeUntilHarvest,long CreateAt,long? UpdatedAt)
        {
            this.Id = Id;
            this.ItemName = ItemName;
            this.ProductAmount = ProductAmount;
            this.TimeUntilHarvest = TimeUntilHarvest;
            this.CreateAt=CreateAt;
            this.UpdatedAt = UpdatedAt;
        }
        
        public Plot(int priceAmount) 
        {
            Price = priceAmount;
            ItemName = null;
            TimeUntilHarvest = 0;
        }

        public Plot(string itemName, int priceAmount = 500) 
        {
            Price = priceAmount;
            ItemName = itemName;
        }


        public void TakeAmountProduct()
        {
            var item = Item;
            var timePerProductWasImproved = GetTimePerProductWasImprove();
            var deltaTime = TimeStamp.Second(DateTime.UtcNow) - UpdatedAt??CreateAt;
            var productHasBeenCollectable = (int) ((float)deltaTime / timePerProductWasImproved);
            var realProductAmount = productHasBeenCollectable > item.ProductCapacity
                ? item.ProductCapacity
                : productHasBeenCollectable;
            // this.Product = new Product()
            // {
            //     ItemType = item.ItemType, 
            //     Amount = realProductAmount
            // };
            // Crop.UpdatedAt = DateTime.UtcNow;
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
                ItemName = null;
                TimeUntilHarvest = 0;
                return true;
            }
            return false;
        }

        private float GetTimePerProductWasImprove()
        {
            var toolImprovePercent = DependencyProvider.Instance.GetDependency<ToolManager>().GetPercentPerLevel;
            return (float)Item.TimePerProduct * (toolImprovePercent / 100f);
        }

    }
}