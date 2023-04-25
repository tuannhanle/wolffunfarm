using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Scripts.Domains.Models;
using App.Scripts.Mics;
using Plot = App.Scripts.Domains.GameObjects.Plot;

namespace App.Scripts.Domains.Services
{
    public class GameManager : MiddlewareBehaviour
    {
        public int money;
        public int day;
        public int plotsOfLand = 3;
        public Crop[] crops;
        public Plot[] plotPrefabs;
        public Transform plotsParent;

        private List<Plot> _plots = new List<Plot>();
        private List<Worker> _workers = new List<Worker>();
        private List<Item> _items = new List<Item>();
        private Tool _tool = new Tool();

        // Start is called before the first frame update
        void Start()
        {

            // add init resource
            _tool = new Tool();
            _plots.AddRange(new List<Plot>(){new Plot(), new Plot(), new Plot()});
            _workers.Add(new Worker());
            _items.AddRange(new List<Item>()
            {
                new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},
                new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},
                new Item(){Name = "Cow"},new Item(){Name = "Cow"}
            });
            
            
            
            money = 1000;
            day = 1;

            // Create starting plots of land
            for (int i = 0; i < plotsOfLand; i++)
            {
                CreatePlot();
            }
        }
        

        public void CreatePlot()
        {
            // Choose a random plot prefab
            Plot plotPrefab = plotPrefabs[Random.Range(0, plotPrefabs.Length)];

            // Instantiate the plot
            Plot plot = Instantiate(plotPrefab, plotsParent) ;

            // Add the plot to the list of plots
            _plots.Add(plot);

            // Set the plot's crop
            Crop crop = GetRandomCrop();
            plot.PlantCrop(crop);

            // Subtract the plot's cost from the player's money
            money -= plot.Crop.SellPrice;
        }

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


