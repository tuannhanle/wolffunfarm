using App.Scripts.Domains.Models;

namespace App.Scripts.Domains.GameObjects
{
    public class Plot : IBuyable
    {
        public Crop Crop { get; set; }
        public bool IsFertilized { get; set; }
        public int TimeUntilHarvest { get; set; }
        public int FertilizerLevel { get; set; }
        
        public static int Price { get; private set; } = 500;

        public Plot()
        {
            Crop = null;
            IsFertilized = false;
            TimeUntilHarvest = 0;
            FertilizerLevel = 0;
        }

        public bool PlantCrop(Crop crop)
        {
            if (Crop == null)
            {
                Crop = crop;
                TimeUntilHarvest = crop.DaysToHarvest;
                return true;
            }
            return false;
        }



        public bool Fertilize()
        {
            if (!IsFertilized)
            {
                IsFertilized = true;
                FertilizerLevel++;
                return true;
            }
            return false;
        }

        public bool Harvest()
        {
            if (Crop != null && TimeUntilHarvest == 0)
            {
                Crop = null;
                IsFertilized = false;
                TimeUntilHarvest = 0;
                FertilizerLevel = 0;
                return true;
            }
            return false;
        }

        public void UpdateCrop()
        {
            if (TimeUntilHarvest > 0)
            {
                TimeUntilHarvest--;
                if (IsFertilized)
                {
                    TimeUntilHarvest--;
                }
            }
        }
    }
}