using System;
using App.Scripts.Domains.Core;
using App.Scripts.Domains.Models;
using App.Scripts.Domains.Services;
using App.Scripts.Mics;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.UI
{
    public class InteractButtonsUI : MiddlewareBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _upgradeToolButton;
        [SerializeField] private Button _rentWorkerButton;
        [SerializeField] private Button _sellButton;

        [Header("Seed Buttons")]
        [SerializeField] private Button _seedBlueBerry;
        [SerializeField] private Button _seedStrawBerry;
        [SerializeField] private Button _seedTomato;
        [SerializeField] private Button _breedCow;
        [Header("Collect Buttons")]
        [SerializeField] private Button _collectBlueBerry;
        [SerializeField] private Button _collectStrawBerry;
        [SerializeField] private Button _collectTomato;
        [SerializeField] private Button _getMilkButton;

        private WorkerManager _workerManager;
        private ToolManager _toolManager;
        private PaymentService _paymentService;
        
        private readonly LazyDataInlet<ShareData.OpenShopEvent> _shopEventInlet = new();
        
        private void Awake()
        {
            
            if(_shopButton) _shopButton.onClick.AddListener(
                delegate { OnUIButtonClicked(InteractEventType.OpenShop); });
            if(_upgradeToolButton) _upgradeToolButton.onClick.AddListener(
                delegate { OnUIButtonClicked(InteractEventType.UpgradeTool); });
            if(_rentWorkerButton) _rentWorkerButton.onClick.AddListener(
                delegate { OnUIButtonClicked(InteractEventType.RentWorker); });
            if(_sellButton) _sellButton.onClick.AddListener(
                delegate { OnUIButtonClicked(InteractEventType.Sell); });
            
            if(_seedBlueBerry) _seedBlueBerry.onClick.AddListener(
                delegate { OnUIButtonClicked(InteractEventType.Seeding, ItemType.BlueBerry); });
            if(_seedStrawBerry) _seedStrawBerry.onClick.AddListener(
                delegate { OnUIButtonClicked(InteractEventType.Seeding,ItemType.StrawBerry); });
            if(_seedTomato) _seedTomato.onClick.AddListener(
                delegate { OnUIButtonClicked(InteractEventType.Seeding, ItemType.Tomato); });
            if(_breedCow) _breedCow.onClick.AddListener(
                delegate { OnUIButtonClicked(InteractEventType.Seeding, ItemType.Cow); });

            // collect product from plant or animal
            if(_collectBlueBerry) _collectBlueBerry.onClick.AddListener(
                delegate { OnUIButtonClicked(InteractEventType.CollectProduct, ItemType.BlueBerry); });
            if(_collectStrawBerry) _collectStrawBerry.onClick.AddListener(
                delegate { OnUIButtonClicked(InteractEventType.CollectProduct, ItemType.StrawBerry); });
            if(_collectTomato) _collectTomato.onClick.AddListener(
                delegate { OnUIButtonClicked(InteractEventType.CollectProduct, ItemType.Tomato); });
            if(_getMilkButton) _getMilkButton.onClick.AddListener(
                delegate { OnUIButtonClicked(InteractEventType.CollectProduct, ItemType.Cow); });

        }

        private void Start()
        {
            _workerManager = DependencyProvider.Instance.GetDependency<WorkerManager>();
            _toolManager = DependencyProvider.Instance.GetDependency<ToolManager>();
            _paymentService = DependencyProvider.Instance.GetDependency<PaymentService>();
        }

        private void OnUIButtonClicked(InteractEventType eInteractEvent, ItemType? eItemType = null)
        {
            switch (eInteractEvent)
            {
                case InteractEventType.Sell:
                    _paymentService.SellProducts();
                    break;
                case InteractEventType.OpenShop:
                    _shopEventInlet.UpdateValue(new());
                    break;
                case InteractEventType.RentWorker:
                    _workerManager.RentWorker( );
                    break;
                case InteractEventType.UpgradeTool:
                    _toolManager.UpgradeTool();
                    break;
                case InteractEventType.Seeding:
                    if (eItemType == null)
                        break;
                    // execute here
                    _workerManager.Assign(new ()
                    {
                        EJob = JobType.Seeding,
                        EItemType = eItemType
                    }).Forget();
                    break;
                case InteractEventType.CollectProduct:
                    if (eItemType == null)
                        break;
                    // execute here
                    _workerManager.Assign(new ()
                    {
                        EJob = JobType.Collecting,
                        EItemType = eItemType
                    }).Forget();
                    break;
            }
        }

    }
    
    public enum InteractEventType
    {
        OpenShop, 
        RentWorker, 
        UpgradeTool, 
        Sell,
        Seeding,
        CollectProduct,
 
    }
    
}