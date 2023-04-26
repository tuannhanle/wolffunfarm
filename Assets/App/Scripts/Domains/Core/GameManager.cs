using System.Collections.Generic;
using App.Scripts.Domains.Models;
using App.Scripts.Mics;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Domains.Core
{
    public class GameManager : MiddlewareBehaviour
    {
        public Crop[] crops;
        public Plot[] plotPrefabs;

        private List<Plot> _plots = new List<Plot>();
        private List<Item> _items = new List<Item>();
        private Tool _tool = new Tool();


        private WorkerManager _workerManager= new WorkerManager();
        
        private void Awake()
        {
            // add init resource
            _plots.AddRange(new List<Plot>(){new Plot(), new Plot(), new Plot()});
            _items.AddRange(new List<Item>()
            {
                new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},
                new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},
                new Item(){Name = "Cow"},new Item(){Name = "Cow"}
            });

            _workerManager.TakeProceedingAsync(new Proceeding(){EProceeding = ProceedingType.Seeding}).Forget();
            _workerManager.TakeProceedingAsync(new Proceeding(){EProceeding = ProceedingType.Seeding}).Forget();
            _workerManager.TakeProceedingAsync(new Proceeding(){EProceeding = ProceedingType.Seeding}).Forget();
            
            this.Subscribe<ShareData.InteractButtonsUIEvent>(
                e => _workerManager.RentWorker(e.EInteractEvent));

        }
    }
}