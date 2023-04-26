using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Scripts.Domains.Models;
using App.Scripts.Mics;
using Plot = App.Scripts.Domains.GameObjects.Plot;

namespace App.Scripts.Domains.Core
{
    public class GameManager : MiddlewareBehaviour
    {
        public int money;
        public int day;
        public int plotsOfLand = 3;
        public Crop[] crops;
        public Plot[] plotPrefabs;

        private List<Plot> _plots = new List<Plot>();
        private List<Item> _items = new List<Item>();
        private Tool _tool = new Tool();

        private Progress _progress = new Progress();

        private WorkerProgress _workerProgress = new WorkerProgress();
        
        // Start is called before the first frame update
        void Start()
        {
            
            // add init resource
            _tool = new Tool();
            _plots.AddRange(new List<Plot>(){new Plot(), new Plot(), new Plot()});
            _items.AddRange(new List<Item>()
            {
                new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},
                new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},
                new Item(){Name = "Cow"},new Item(){Name = "Cow"}
            });

            _progress.datas.Push(new Proceeding(){EProceeding = ProceedingType.Seeding});
            _progress.datas.Push(new Proceeding(){EProceeding = ProceedingType.Seeding});
            _progress.datas.Push(new Proceeding(){EProceeding = ProceedingType.Seeding});

            _workerProgress.Init();
            _workerProgress.GetProgress(_progress);
            
            money = 1000;
            day = 1;

            // Create starting plots of land
            for (int i = 0; i < plotsOfLand; i++)
            {
                // CreatePlot();
            }
        }
        

        // public void CreatePlot()
        // {
        //
        //
        //     // Set the plot's crop
        //     Crop crop = GetRandomCrop();
        //     
        //     plot.PlantCrop(crop);
        //
        //     // Subtract the plot's cost from the player's money
        //     money -= plot.Crop.SellPrice;
        // }

        public void EndDay()
        {
            // Increase the day counter
            day++;

            // Update each plot's crop
            foreach (Plot plot in _plots)
            {
                plot.UpdateCrop();
            }
        }

        private Crop GetRandomCrop()
        {
            return crops[Random.Range(0, crops.Length)];
        }
    
        
        private Player _player;

        public GameManager(Player player)
        {
            _player = player;
        }

        public void IncreaseScore(int amount)
        {
            _player.Score += amount;
        }
        
        
        
    }
}


