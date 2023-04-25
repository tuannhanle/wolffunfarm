using App.Scripts.Domains.Models;
using App.Scripts.Mics;
using UnityEngine;

namespace App.Scripts.Domains.GameObjects
{
    public class Plot : MiddlewareBehaviour
    {
        public Crop Crop { get; set; }
        public bool IsWatered { get; set; }
        public bool IsFertilized { get; set; }
        public int DaysUntilHarvest { get; set; }
        public int WaterLevel { get; set; }
        public int FertilizerLevel { get; set; }
        
        public Plot()
        {
            Crop = null;
            IsWatered = false;
            IsFertilized = false;
            DaysUntilHarvest = 0;
            WaterLevel = 0;
            FertilizerLevel = 0;
        }

        public bool PlantCrop(Crop crop)
        {
            if (Crop == null)
            {
                Crop = crop;
                DaysUntilHarvest = crop.DaysToHarvest;
                return true;
            }
            return false;
        }

        public bool Water()
        {
            if (!IsWatered)
            {
                IsWatered = true;
                WaterLevel++;
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
            if (Crop != null && DaysUntilHarvest == 0)
            {
                Crop = null;
                IsWatered = false;
                IsFertilized = false;
                DaysUntilHarvest = 0;
                WaterLevel = 0;
                FertilizerLevel = 0;
                return true;
            }
            return false;
        }

        public void UpdateCrop()
        {
            if (DaysUntilHarvest > 0)
            {
                DaysUntilHarvest--;
                if (IsWatered && IsFertilized)
                {
                    DaysUntilHarvest--;
                }
            }
        }
    }
}