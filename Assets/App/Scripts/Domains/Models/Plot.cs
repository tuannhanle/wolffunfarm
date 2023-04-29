using App.Scripts.Domains.Core;
using App.Scripts.Domains.Models;

namespace App.Scripts.Domains.GameObjects
{
    public class Plot : IBuyable
    {
        public Crop Crop { get; private set; }
        public int TimeUntilHarvest { get; set; } // millisecond
        
        public static int Price { get; private set; } = 500;
        private bool _isGrowable => Crop == null;

        private int _extendTimeToSelfDestroy = 3600; // millisecond
        public Plot()
        {
            Crop = null;
            TimeUntilHarvest = 0;
        }

        public bool PlantCrop(ItemType itemType)
        {
            if (_isGrowable && TimeUntilHarvest == 0)
            {
                var item = Item.ConvertItemType(itemType);
                Crop = new Crop(item);
                TimeUntilHarvest = item.TimePerProduct * item.ProductCapacity + _extendTimeToSelfDestroy;
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