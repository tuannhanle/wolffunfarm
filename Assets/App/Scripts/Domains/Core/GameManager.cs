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

        private List<Item> _items = new List<Item>();

        private WorkerManager _workerManager= new WorkerManager();
        private ToolManager _toolManager = new ToolManager();
        private PlotManager _plotManager = new PlotManager();
        private void Awake()
        {
            _plotManager.InitVeryFirstLogin();
            _workerManager.InitVeryFirstLogin();
            // add init resource
            _items.AddRange(new List<Item>()
            {
                new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},new Item(){Name = "Tomato"},
                new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},new Item(){Name = "Blueberry"},
                new Item(){Name = "Cow"},new Item(){Name = "Cow"}
            });



            this.Subscribe<ShareData.InteractButtonsUIEvent>(OnInteractButtonsUIEventRaised);
            
            
        }

        private void OnInteractButtonsUIEventRaised(ShareData.InteractButtonsUIEvent uiEvent)
        {
            if (uiEvent.EInteractEvent == null)
                return;

            switch (uiEvent.EInteractEvent)
            {
                case ShareData.InteractEventType.RentWorker:
                    _workerManager.RentWorker(uiEvent.EInteractEvent);
                    break;
                case ShareData.InteractEventType.UpgradeTool:
                    _toolManager.UpgradeTool(uiEvent.EInteractEvent);
                    break;
                case ShareData.InteractEventType.ExtendPlot:
                    _plotManager.ExtendPlot(uiEvent.EInteractEvent);
                    break;
                default:
                    break;
            }
        }
    }
}