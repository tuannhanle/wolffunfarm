using System;
using System.Collections.Generic;
using App.Scripts.Domains.Core;
using App.Scripts.Domains.Models;
using App.Scripts.Domains.Services;

namespace App.Scripts.Domains.GameObjects
{
    public class Plot : IBuyable
    {
        public int Id;
        public string ItemName = null;
        public int ProductAmount;
        public long? CreateAt;
        public long? UpdatedAt;
        public int Price { get; set; }
        public long TimeUntilHarvest; // millisecond
        public bool IsUsing;
        
        // override int Price { get; private set; } = 500;
        
        public bool IsHarvastable(string itemName) => ItemName.Equals(itemName) && ProductAmount > 0;

        public Plot() { }

        public Plot(int Id,string ItemName,int ProductAmount,long TimeUntilHarvest,long? CreateAt,long? UpdatedAt, bool isUsing)
        {
            this.Id = Id;
            this.ItemName = ItemName;
            this.ProductAmount = ProductAmount;
            this.TimeUntilHarvest = TimeUntilHarvest;
            this.CreateAt=CreateAt;
            this.UpdatedAt = UpdatedAt;
            this.IsUsing = isUsing;
        }

        public void TakeAmountProduct()
        {
            // var item = Item;
            // var timePerProductWasImproved = GetTimePerProductWasImprove();
            // var deltaTime = TimeStamp.Second(DateTime.UtcNow) - UpdatedAt??CreateAt;
            // var productHasBeenCollectable = (int) ((float)deltaTime / timePerProductWasImproved);
            // var realProductAmount = productHasBeenCollectable > item.ProductCapacity
            //     ? item.ProductCapacity
            //     : productHasBeenCollectable;
            // this.Product = new Product()
            // {
            //     ItemType = item.ItemType, 
            //     Amount = realProductAmount
            // };
            // Crop.UpdatedAt = DateTime.UtcNow;
        }

        public bool IsPutInable()
        {
            return TimeUntilHarvest == 0 && (ItemName == null || ItemName.Equals(""));
        }
        
        public void PutIn(int productCapacity, int extendTime)
        {
            if (TimeUntilHarvest != 0)
                return;
            var timePerProductWasImproved = GetTimePerProductWasImprove();
            TimeUntilHarvest = (long)(timePerProductWasImproved * (float) productCapacity + (float)extendTime);
        }
        
        public void Harvest(string itemName)
        {
            if (itemName == null)
                return;
            if (TimeUntilHarvest != 0)
                return;
            ProductAmount = 0;
            ItemName = null;
            TimeUntilHarvest = 0;
        }

        private float GetTimePerProductWasImprove()
        {
            // var toolImprovePercent = DependencyProvider.Instance.GetDependency<ToolManager>().GetPercentPerLevel;
            // return (float)Item.TimePerProduct * (toolImprovePercent / 100f);
            return 0;
        }

    }
}