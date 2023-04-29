using System;
using App.Scripts.Domains.Models;
using NUnit.Framework;
using Plot = App.Scripts.Domains.GameObjects.Plot;

namespace Tests.EditMode
{
    [TestFixture]
    public class TimeTests
    {
        [Test]
        public void TestProductCouldBeCollectedAfterTime()
        {
            var productAmountExpected = 3;
            var item = new Item() {ItemType = ItemType.BlueBerry, TimePerProduct = 60, ProductCapacity = 20 };
            var crop = new Crop(item);
            crop.CreateAt = TimeStamp.DateTimeFromSeconds(TimeStamp.Second(DateTime.UtcNow ) - (long)60*productAmountExpected) ;
            var plot = new Plot(crop);
            plot.TakeAmountProduct();
            Assert.AreEqual(productAmountExpected,plot.Crop.Product.Amount);
        }
    }
}