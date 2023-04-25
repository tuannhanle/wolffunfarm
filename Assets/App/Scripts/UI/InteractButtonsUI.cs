using App.Scripts.Mics;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.UI
{
    public class InteractButtonsUI : MonoBehaviour
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

        private LazyDataInlet<ShareData.OpenShop> _openShopEvent = new LazyDataInlet<ShareData.OpenShop>();

        private void Awake()
        {
            if(_upgradeToolButton) _shopButton.onClick.AddListener(OnUIshopButtonClicked);
            if(_upgradeToolButton) _upgradeToolButton.onClick.AddListener(()=>{});
            if(_rentWorkerButton) _rentWorkerButton.onClick.AddListener(()=>{});
            if(_getMilkButton) _getMilkButton.onClick.AddListener(()=>{});
            if(_sellButton) _sellButton.onClick.AddListener(()=>{});

        }

        private void OnUIshopButtonClicked()
        {
            _openShopEvent.UpdateValue(new ShareData.OpenShop());
        }
    }
}