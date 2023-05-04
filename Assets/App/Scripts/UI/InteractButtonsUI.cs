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
        [Header("Refs")] 
        [SerializeField] private Button _buttonPrefab;
        [SerializeField] private Transform _root;

        [Header("Buttons")]
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _upgradeToolButton;
        [SerializeField] private Button _rentWorkerButton;
        [SerializeField] private Button _sellButton;
        

        private WorkerManager _workerManager;
        private ToolManager _toolManager;
        private PaymentService _paymentService;
        private DataLoader _dataLoader;
        
        private readonly LazyDataInlet<ShareData.OpenShopEvent> _shopEventInlet = new();
        
        private void Start()
        {
            _workerManager = DependencyProvider.Instance.GetDependency<WorkerManager>();
            _toolManager = DependencyProvider.Instance.GetDependency<ToolManager>();
            _paymentService = DependencyProvider.Instance.GetDependency<PaymentService>();
            _dataLoader = DependencyProvider.Instance.GetDependency<DataLoader>();
            Init();
        }
        
        private void Init()
        {
            
            if(_shopButton) _shopButton.onClick.AddListener(
                delegate { OnUIButtonClicked(InteractEventType.OpenShop); });
            if(_upgradeToolButton) _upgradeToolButton.onClick.AddListener(
                delegate { OnUIButtonClicked(InteractEventType.UpgradeTool); });
            if(_rentWorkerButton) _rentWorkerButton.onClick.AddListener(
                delegate { OnUIButtonClicked(InteractEventType.RentWorker); });
            if(_sellButton) _sellButton.onClick.AddListener(
                delegate { OnUIButtonClicked(InteractEventType.Sell); });

            CreateButtonAsync("Seeding", InteractEventType.PutIn);
            CreateButtonAsync("Harvest", InteractEventType.PutOut);
        }
        
        private void CreateButtonAsync(string interactName, InteractEventType interactEventType)
        {
            var items = _dataLoader.ItemCollection;
            foreach (var item in items)
            {
                if (item.Value.IsSeedingable == false)
                    continue;
                if (item.Value.IsAnimal && interactEventType == InteractEventType.PutIn)
                {
                    interactName = "Breed";
                }
                var button = Instantiate(_buttonPrefab, _root);
                var name = interactName + " " + item.Key;
                button.name = name;
                button.GetComponentInChildren<Text>().text = name;
                button.onClick.AddListener(
                    delegate
                    {
                        OnUIButtonClicked(interactEventType, item.Key);
                    });
                button.gameObject.SetActive(true);
            }
        }
        private void OnUIButtonClicked(InteractEventType eInteractEvent, string itemName = null)
        {
            Debug.Log($"InteractEvent: {eInteractEvent} of {itemName} ");

            switch (eInteractEvent)
            {
                case InteractEventType.Sell:
                    _paymentService.SellProducts();
                    break;
                case InteractEventType.OpenShop:
                    _shopEventInlet.UpdateValue(new());
                    break;
                case InteractEventType.RentWorker:
                    _workerManager.RentWorker();
                    break;
                case InteractEventType.UpgradeTool:
                    _toolManager.UpgradeTool();
                    break;
                case InteractEventType.PutIn:
                    if (itemName == null)
                        break;
                    // execute here
                    _workerManager.Assign(JobType.PutIn, itemName);
                    break;
                case InteractEventType.PutOut:
                    if (itemName == null)
                        break;
                    // execute here
                    _workerManager.Assign(JobType.PutOut, itemName);
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
        PutIn,
        PutOut,
 
    }
    
}