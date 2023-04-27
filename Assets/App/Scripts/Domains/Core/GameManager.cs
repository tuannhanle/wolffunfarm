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

        private List<Item> _items = new();

        private WorkerManager _workerManager= new();
        private ToolManager _toolManager = new();
        private PlotManager _plotManager = new();
        private ShopManager _shopManager;
        
        private void Awake()
        {
            
            _plotManager.InitVeryFirstLogin();
            _shopManager = new(_plotManager);
            _workerManager.InitVeryFirstLogin();
            
            // add init resource
            _items.AddRange(new List<Item>()
            {
                // new (){Name = "Tomato"},new (){Name = "Tomato"},new (){Name = "Tomato"},new (){Name = "Tomato"},new (){Name = "Tomato"},new (){Name = "Tomato"},new (){Name = "Tomato"},new (){Name = "Tomato"},new (){Name = "Tomato"},new (){Name = "Tomato"},
                // new (){Name = "Blueberry"},new (){Name = "Blueberry"},new (){Name = "Blueberry"},new (){Name = "Blueberry"},new (){Name = "Blueberry"},new (){Name = "Blueberry"},new (){Name = "Blueberry"},new (){Name = "Blueberry"},new (){Name = "Blueberry"},new (){Name = "Blueberry"},
                // new (){Name = "Cow"},new (){Name = "Cow"}
            });



            this.Subscribe<ShareData.InteractButtonsUIEvent>(OnInteractButtonsUIEventRaised);
            this.Subscribe<ShareData.ShopUIEvent>(_shopManager.OnShopUIEventRaised);
            
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
                case ShareData.InteractEventType.GetMilk:
                    break;
                case ShareData.InteractEventType.Sell:
                    break;
           
                default:
                    break;
            }
        }


    }
}