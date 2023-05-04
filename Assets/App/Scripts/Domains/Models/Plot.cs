using System;
using System.Collections.Generic;
using App.Scripts.Domains.Core;
using App.Scripts.Domains.Models;
using App.Scripts.Domains.Services;
using UnityEngine;

namespace App.Scripts.Domains.GameObjects
{
    public class Plot : IBuyable
    {
        public int Id;
        public string ItemName = null;
        public int HarvestedProduct; // product number that has been collect / harvested
        public DateTime CreateAt; // for crop
        public DateTime UpdatedAt; // for crop
        public int Price { get; set; }
        public long TimeUntilHarvest = -1; // millisecond
        public bool IsUsing = false;

        private Item _item;
        public Item Item
        {
            get
            {
                var collection = DependencyProvider.Instance.GetDependency<DataLoader>().ItemCollection;
                if (_item == null && collection.ContainsKey(ItemName))
                {
                    _item = DependencyProvider.Instance.GetDependency<DataLoader>().ItemCollection[ItemName];
                }
                return _item;
            }
            private set
            {
                _item = value;
            }
        }



        public int ProductOnPlot { get; set; }
        public int ProductCapacity { get; set; }
        
        public bool IsPutOutable(string itemName) => ItemName.Equals(itemName) && ProductOnPlot > 0;
    
        public Plot() { }

        public Plot(int Id,string ItemName,int harvestedProduct,long TimeUntilHarvest,DateTime CreateAt, DateTime UpdatedAt, bool isUsing)
        {
            this.Id = Id;
            this.ItemName = ItemName;
            this.HarvestedProduct = harvestedProduct;
            this.TimeUntilHarvest = TimeUntilHarvest;
            this.CreateAt=CreateAt;
            this.UpdatedAt = UpdatedAt;
            this.IsUsing = isUsing;
        }

        public void SelfCheck()
        {
            if (IsUsing == false)
                return;
            if (IsAliveItem() == false)
            {
                SelfDestroy();
                return;
            }
            var t = GetTimePerProductWasImprove();
            var deltaTime = (DateTime.UtcNow - CreateAt).TotalSeconds;
            var producOnPlot = (int) ((float)deltaTime / t);
            ProductOnPlot = producOnPlot > Item.ProductCapacity // product on the plot (now)
                ? Item.ProductCapacity - HarvestedProduct
                : producOnPlot - HarvestedProduct;
            ProductCapacity = Item.ProductCapacity - HarvestedProduct;
                var nextProductTime = (ProductOnPlot + 1) * Item.TimePerProduct;
            if (ProductOnPlot + 1 > Item.ProductCapacity)
            {
                var extentTimeToDestroy = DependencyProvider.Instance.GetDependency<DataLoader>().stat.ExtentTimeToDestroy;
                nextProductTime += extentTimeToDestroy;
            }
            TimeUntilHarvest = (long)(DateTime.UtcNow - CreateAt.AddSeconds(nextProductTime)).TotalSeconds;
            
        }

        
        private bool IsAliveItem()
        {
            var t = GetTimePerProductWasImprove();
            var extentTimeToDestroy = DependencyProvider.Instance.GetDependency<DataLoader>().stat.ExtentTimeToDestroy;
            int aliveTimeWithinExtendTime = (int)((float)(Item.ProductCapacity * Item.TimePerProduct) * t) + extentTimeToDestroy ;
            DateTime deadTime = CreateAt.AddSeconds(aliveTimeWithinExtendTime);
            return DateTime.UtcNow >= CreateAt && DateTime.UtcNow <= deadTime;
        }

        private bool IsInBeetweenExtendTime()
        {
            var t = GetTimePerProductWasImprove();
            var extentTimeToDestroy = DependencyProvider.Instance.GetDependency<DataLoader>().stat.ExtentTimeToDestroy;
            int aliveTime = (int)((float)(Item.ProductCapacity * Item.TimePerProduct) * t)  ;
            DateTime beingDie = CreateAt.AddSeconds(aliveTime);
            int aliveTimeWithinExtendTime = (int)((float)(Item.ProductCapacity * Item.TimePerProduct) * t)+ extentTimeToDestroy  ;
            DateTime deadTime = CreateAt.AddSeconds(aliveTimeWithinExtendTime);
            return DateTime.UtcNow >= beingDie && DateTime.UtcNow <= deadTime;
        }


        
        public void PutIn(Item item, int extendTime = 0)
        {
            if (IsUsing)
                return;
            IsUsing = true;
            ItemName = item.ItemName;
            CreateAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            ProductCapacity = item.ProductCapacity;
            var timePerProductWasImproved = GetTimePerProductWasImprove();
            TimeUntilHarvest = (long)(timePerProductWasImproved * (float)item.ProductCapacity + (float)extendTime);

        }
        
        public int PutOut(string itemName)
        {
            if (itemName == null)
                return 0;
            UpdatedAt = DateTime.UtcNow;
            HarvestedProduct = ProductOnPlot;
            TimeUntilHarvest = 0;
            IsUsing = true;
            ProductCapacity -= HarvestedProduct;
            var result = ProductOnPlot;
            ProductOnPlot = 0;
            HarvestedProduct = 0;
            if (IsInBeetweenExtendTime())
            {
                SelfDestroy();
            }
            return result;

        }

        private float GetTimePerProductWasImprove()
        {
            if (IsUsing)
                return (float) Item.TimePerProduct;
            var toolImprovePercent = DependencyProvider.Instance.GetDependency<ToolManager>().GetPercentPerLevel;
            return (float)Item.TimePerProduct * (toolImprovePercent / 100f);
            
        }

        private void SelfDestroy()
        {
            UpdatedAt = DateTime.UtcNow;
            HarvestedProduct = 0;
            ProductOnPlot = 0;
            ProductCapacity = 0;
            ItemName = null;
            TimeUntilHarvest = -1;
            IsUsing = false;
        }
    }
}