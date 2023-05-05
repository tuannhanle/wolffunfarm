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
        public string ItemName;
        public int HarvestedProduct; // product number that has been collect / harvested
        public DateTime CreateAt; // for crop
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

        public bool IsPutOutable(string itemName)
        {
            if (ItemName == null)
                return false;
            var condition1 = ItemName.Equals(itemName) && ProductOnPlot > 0;
            var condition2 = ProductOnPlot > 0;
            return condition1 && condition2;

        }

        public Plot() { }

        public Plot(int Id,string ItemName,int harvestedProduct,long TimeUntilHarvest,DateTime CreateAt, bool isUsing)
        {
            this.Id = Id;
            this.ItemName = ItemName;
            this.HarvestedProduct = harvestedProduct;
            this.TimeUntilHarvest = TimeUntilHarvest;
            this.CreateAt=CreateAt;
            this.IsUsing = isUsing;
        }

        public void SelfCheck()
        {
            if (IsUsing == false)
                return;
            if (IsAliveItem() == false)
            {
                Clear();
                return;
            }
            //
            // var now = DateTime.UtcNow;
            // var t = GetToolPercent(); // percent of tool that improve the speed of grow up of the product
            // var timePerProduct = (float)Item.TimePerProduct / t;
            // var deltaTime = (float)(Math.Round((now - CreateAt).TotalSeconds)) / t;
            // var producOnPlot = (int) Math.Round(deltaTime / timePerProduct); // calculated product on plot
            // ProductOnPlot = producOnPlot > Item.ProductCapacity // product on the plot (if it is greater than capacity)
            //     ? Item.ProductCapacity - HarvestedProduct
            //     : producOnPlot - HarvestedProduct;
            // ProductCapacity = Item.ProductCapacity - HarvestedProduct;
            // var nextProductTime = (ProductOnPlot + 1) * timePerProduct;
            // if (ProductOnPlot + 1 > Item.ProductCapacity)
            // {
            //     var extentTimeToDestroy = DependencyProvider.Instance.GetDependency<DataLoader>().stat.ExtentTimeToDestroy;
            //     nextProductTime += extentTimeToDestroy;
            // }
            //
            // var remainTimeAt = CreateAt.AddSeconds(nextProductTime);
            // var remainTime = (long)(now - remainTimeAt).TotalSeconds;
            // TimeUntilHarvest = remainTime;

            var now = DateTime.UtcNow;
            var t = GetToolPercent(); // percent of tool that improves the speed of product growth
            var timePerProduct = Item.TimePerProduct / t;
            var deltaTime = (float)Math.Floor((now - CreateAt).TotalSeconds / t);
            var productsElapsed = (int)Math.Floor(deltaTime / timePerProduct);
            var productOnPlot = Math.Min(productsElapsed - HarvestedProduct, Item.ProductCapacity - HarvestedProduct);
            ProductOnPlot = productOnPlot;
            ProductCapacity = Item.ProductCapacity - HarvestedProduct;
            var nextProductTime = (ProductOnPlot + 1) * timePerProduct;
            if (ProductOnPlot + 1 > Item.ProductCapacity)
            {
                var extentTimeToDestroy = DependencyProvider.Instance.GetDependency<DataLoader>().stat.ExtentTimeToDestroy;
                nextProductTime += extentTimeToDestroy;
            }

            var remainTimeAt = CreateAt.AddSeconds(nextProductTime);
            var remainTime = (long)(remainTimeAt - now).TotalSeconds;
            TimeUntilHarvest = remainTime;


        }

        
        private bool IsAliveItem()
        {
            var t = GetToolPercent();
            var extentTimeToDestroy = DependencyProvider.Instance.GetDependency<DataLoader>().stat.ExtentTimeToDestroy;
            int aliveTimeWithinExtendTime = (int)((float)(Item.ProductCapacity * Item.TimePerProduct) / t) + extentTimeToDestroy ;
            DateTime deadTime = CreateAt.AddSeconds(aliveTimeWithinExtendTime);
            return DateTime.UtcNow >= CreateAt && DateTime.UtcNow <= deadTime;
        }

        private bool IsInBeetweenExtendTime()
        {
            var t = GetToolPercent();
            var extentTimeToDestroy = DependencyProvider.Instance.GetDependency<DataLoader>().stat.ExtentTimeToDestroy;
            int aliveTime = (int)((Item.ProductCapacity * Item.TimePerProduct) / t)  ;
            DateTime beingDie = CreateAt.AddSeconds(aliveTime);
            int aliveTimeWithinExtendTime = aliveTime + extentTimeToDestroy  ;
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
        }
        
        public void PutOut()
        {
            var dataloader = DependencyProvider.Instance.GetDependency<DataLoader>();
            dataloader.ItemStorage[ItemName].HarvestedProduct += ProductOnPlot;
            dataloader.Push<ItemStorage>();
            HarvestedProduct += ProductOnPlot;
            ProductOnPlot = 0;
            if (IsInBeetweenExtendTime())
            {
                Clear();
            }

        }

        private float GetToolPercent()
        {
            var toolImprovePercent = DependencyProvider.Instance.GetDependency<ToolManager>().GetPercentPerLevel;
            return (toolImprovePercent / 100f);
            
        }

        public void Clear()
        {
            CreateAt = TimeStamp.FirstDay;
            HarvestedProduct = 0;
            ProductOnPlot = 0;
            ProductCapacity = 0;
            ItemName = "";
            TimeUntilHarvest = -1;
            IsUsing = false;
        }
    }
}