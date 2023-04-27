using App.Scripts.Mics;
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
        [SerializeField] private Button _getMilkButton;
        [SerializeField] private Button _sellButton;

        [Header("Seed Buttons")]
        [SerializeField] private Button _seedBlueBerry;
        [SerializeField] private Button _seedStrawBerry;
        [SerializeField] private Button _seedTomato;
        [Header("Collect Buttons")]
        [SerializeField] private Button _collectBlueBerry;
        [SerializeField] private Button _collectStrawBerry;
        [SerializeField] private Button _collectTomato;

        private LazyDataInlet<ShareData.InteractButtonsUIEvent> _interactEventInlet = new();

        private void Awake()
        {
            if(_shopButton) _shopButton.onClick.AddListener(delegate { OnUIButtonClicked(ShareData.InteractEventType.OpenShop); });
            if(_upgradeToolButton) _upgradeToolButton.onClick.AddListener(delegate { OnUIButtonClicked(ShareData.InteractEventType.UpgradeTool); });
            if(_rentWorkerButton) _rentWorkerButton.onClick.AddListener(delegate { OnUIButtonClicked(ShareData.InteractEventType.RentWorker); });
            if(_getMilkButton) _getMilkButton.onClick.AddListener(delegate { OnUIButtonClicked(ShareData.InteractEventType.GetMilk); });
            if(_sellButton) _sellButton.onClick.AddListener(delegate { OnUIButtonClicked(ShareData.InteractEventType.Sell); });
        }

        private void OnUIButtonClicked(ShareData.InteractEventType interactEvent)
        {
            _interactEventInlet.UpdateValue(new(){ EInteractEvent = interactEvent});
        }
        

    }
}