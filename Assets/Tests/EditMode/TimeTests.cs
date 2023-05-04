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
        public void TestDateTime()
        {
            string dateString = "1/1/0001 12:00:00 AM";
            var firstDayDT = DateTime.Parse(dateString);
            Assert.AreEqual(true, TimeStamp.IsFirstDay(firstDayDT));
        }
    }
}