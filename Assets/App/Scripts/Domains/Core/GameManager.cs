using System.Collections.Generic;
using App.Scripts.Domains.Models;
using App.Scripts.Mics;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Domains.Core
{
    public class GameManager : MiddlewareBehaviour
    {
        
        private List<Item> _items = new();

        private WorkerManager _workerManager;
        private ToolManager _toolManager;
        private PlotManager _plotManager;
        private ShopManager _shopManager;
        private StatManager _statManager = new();
        
        
        private void Awake()
        {
            _statManager.SyncFromLocalDB();
            _plotManager = new()
            {
                UsingPlotAmount = _statManager.UsingPlotAmount,
                UnusedPlotAmount = _statManager.UnusedPlotAmount,
            };
            _plotManager.Init();
            _toolManager = new() { ToolLevel = _statManager.ToolLevel };
            _shopManager = new(_plotManager);
            _workerManager = new()
            {
                IdleWorkerAmount = _statManager.IdleWorkerAmount,
                WorkingWorkerAmount = _statManager.WorkingWorkerAmount
            };
            _workerManager.Init();
            
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