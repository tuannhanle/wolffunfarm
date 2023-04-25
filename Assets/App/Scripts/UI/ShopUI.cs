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
        
        private void Awake()
        {
            OnUIOpenStateUpdate();
            if(_closeButton) _closeButton.onClick.AddListener(OnUIOpenStateUpdate);
            if(_buyBlueberryButton)_buyBlueberryButton.onClick.AddListener(()=>{});
            if(_buyTomatoButton)_buyTomatoButton.onClick.AddListener(()=>{});
            if(_buyStrawberry)_buyStrawberry.onClick.AddListener(()=>{});
            if(_buyCowButton)_buyCowButton.onClick.AddListener(()=>{});
            if(_buyPlot)_buyPlot.onClick.AddListener(()=>{});
            
            this.Subscribe<ShareData.OpenShop>(_ => OnUIOpenStateUpdate());

        }

        private void OnUIOpenStateUpdate()
        {
            _isClose = !_isClose;
            _canvasGroup.alpha = _isClose ? 1 : 0;
            _canvasGroup.interactable = _isClose;
            _canvasGroup.blocksRaycasts = _isClose;        
        }

  
    }
}