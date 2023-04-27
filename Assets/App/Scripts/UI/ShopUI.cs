using App.Scripts.Mics;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.UI
{
    public class ShopUI : MiddlewareBehaviour
    {
        [Header("Refs")]
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Buttons")]
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _buyBlueberryButton;
        [SerializeField] private Button _buyTomatoButton;
        [SerializeField] private Button _buyStrawberry;
        [SerializeField] private Button _buyCowButton;
        [SerializeField] private Button _buyPlot;
        
        // State
        private bool _isClose = true;
        
        private LazyDataInlet<ShareData.ShopUIEvent> _shopEventInlet = new();

        private ShareData.InteractButtonsUIEvent _openShopEvent = new() { EInteractEvent = ShareData.InteractEventType.OpenShop };
        private void Awake()
        {
            OnUIOpenStateUpdate(_openShopEvent);
            if(_closeButton) _closeButton.onClick.AddListener(()=>OnUIOpenStateUpdate(_openShopEvent));
            if(_buyBlueberryButton)_buyBlueberryButton.onClick.AddListener(delegate { OnUIButtonClicked(ShareData.ShopEventType.BBlueBerry); });
            if(_buyTomatoButton)_buyTomatoButton.onClick.AddListener(delegate { OnUIButtonClicked(ShareData.ShopEventType.BTomato); });
            if(_buyStrawberry)_buyStrawberry.onClick.AddListener(delegate { OnUIButtonClicked(ShareData.ShopEventType.BStrawBerry); });
            if(_buyCowButton)_buyCowButton.onClick.AddListener(delegate { OnUIButtonClicked(ShareData.ShopEventType.BCow); });
            if(_buyPlot)_buyPlot.onClick.AddListener(delegate { OnUIButtonClicked(ShareData.ShopEventType.BPlot); });
            
            this.Subscribe<ShareData.InteractButtonsUIEvent>(OnUIOpenStateUpdate);

        }
        
        
        private void OnUIButtonClicked(ShareData.ShopEventType shopEvent)
        {
            _shopEventInlet.UpdateValue(new(){ EShopUIEvent = shopEvent});
        }

        private void OnUIOpenStateUpdate(ShareData.InteractButtonsUIEvent interactButtonsUIEvent)
        {
            if (interactButtonsUIEvent.EInteractEvent != ShareData.InteractEventType.OpenShop) 
                return;
            
            _isClose = !_isClose;
            _canvasGroup.alpha = _isClose ? 1 : 0;
            _canvasGroup.interactable = _isClose;
            _canvasGroup.blocksRaycasts = _isClose;        
        }

  
    }
}