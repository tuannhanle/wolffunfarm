using System;
using App.Scripts.Domains.Core;
using App.Scripts.Domains.Models;
using App.Scripts.Mics;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.UI
{
    
    public enum ShopEventType
    {
        Buy,
        BuySeedInCart,
        ReleaseSeedInCart
    }
    
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
        [SerializeField] private Button _buySeedInCart;
        [SerializeField] private Button _releaseSeedInCart;

        // State
        private bool _isClose = true;
        
        private ShareData.OpenShopEvent _openShopEvent = new() { };
        
        private ShopManager _shopManager;

        private void Awake()
        {
            OnUIOpenStateUpdate(_openShopEvent);
            
            if(_closeButton) _closeButton.onClick.AddListener(()=>OnUIOpenStateUpdate(_openShopEvent));
            
            if(_buyBlueberryButton)_buyBlueberryButton.onClick.AddListener(
                delegate { OnUIButtonClicked(ShopEventType.Buy, ItemType.BlueBerry); });
            if(_buyTomatoButton)_buyTomatoButton.onClick.AddListener(
                delegate { OnUIButtonClicked(ShopEventType.Buy, ItemType.Tomato); });
            if(_buyStrawberry)_buyStrawberry.onClick.AddListener(
                delegate { OnUIButtonClicked(ShopEventType.Buy, ItemType.StrawBerry); });
            if(_buyCowButton)_buyCowButton.onClick.AddListener(
                delegate { OnUIButtonClicked(ShopEventType.Buy, ItemType.Cow); });
            if(_buyPlot)_buyPlot.onClick.AddListener(
                delegate { OnUIButtonClicked(ShopEventType.Buy, ItemType.UnusedPlot); });
            if(_buySeedInCart)_buyPlot.onClick.AddListener(
                delegate { OnUIButtonClicked(ShopEventType.BuySeedInCart); });
            if(_releaseSeedInCart)_buyPlot.onClick.AddListener(
                delegate { OnUIButtonClicked(ShopEventType.ReleaseSeedInCart); });

            this.Subscribe<ShareData.OpenShopEvent>(OnUIOpenStateUpdate);

        }

        private void Start()
        {
            _shopManager = DependencyProvider.Instance.GetDependency<ShopManager>();
        }
        
        private void OnUIButtonClicked(ShopEventType eShopEvent, ItemType? eItemType =null)
        {
            _shopManager.OnShopUIEventRaised(eShopEvent, eItemType);
        }

        private void OnUIOpenStateUpdate(ShareData.OpenShopEvent openShopEvent)
        {
            _isClose = !_isClose;
            _canvasGroup.alpha = _isClose ? 1 : 0;
            _canvasGroup.interactable = _isClose;
            _canvasGroup.blocksRaycasts = _isClose;        
        }

  
    }
}