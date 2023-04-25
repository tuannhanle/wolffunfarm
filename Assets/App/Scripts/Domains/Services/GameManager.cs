using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Scripts.Domains.Models;
using Plot = App.Scripts.Domains.GameObjects.Plot;

namespace App.Scripts.Domains.Services
{
    public class GameManager : MonoBehaviour
    {
        public int money;
        public int day;
        public int plotsOfLand = 3;
        public Crop[] crops;
        public Plot[] plotPrefabs;
        public Transform plotsParent;

        private List<Plot> plots = new List<Plot>();

        // Start is called before the first frame update
        void Start()
        {
            money = 1000;
            day = 1;

            // Create starting plots of land
            for (int i = 0; i < plotsOfLand; i++)
            {
                CreatePlot();
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void CreatePlot()
        {
            // Choose a random plot prefab
            Plot plotPrefab = plotPrefabs[Random.Range(0, plotPrefabs.Length)];

            // Instantiate the plot
            Plot plot = Instantiate(plotPrefab, plotsParent) ;

            // Add the plot to the list of plots
            plots.Add(plot);

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
            foreach (Plot plot in plots)
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


