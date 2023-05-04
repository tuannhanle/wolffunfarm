using System;
using System.Collections.Generic;
using App.Scripts.Domains.Core;
using App.Scripts.Domains.Models;
using App.Scripts.Mics;
using Cysharp.Threading.Tasks;
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
        [SerializeField] private Button _buttonPrefab;
        [SerializeField] private Transform _root;
        
        [Header("Buttons")]
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _buySeedInCart;
        [SerializeField] private Button _releaseSeedInCart;

        [Header("Display")] 
        [SerializeField] private Text _prefabText;        
        [SerializeField] private Transform _cartRoot;   
        
        // State
        private bool _isClose = true;
        
        private ShareData.OpenShopEvent _openShopEvent = new() { };
        
        private ShopManager _shopManager;
        private DataLoader _dataLoader;

        private Dictionary<string, Text> _textCartMaps = new();
        private Dictionary<string, int> _amountCartMaps = new();

        private void Start()
        {
            _shopManager = DependencyProvider.Instance.GetDependency<ShopManager>();
            _dataLoader = DependencyProvider.Instance.GetDependency<DataLoader>();
            Init();
        }
        
        private void Init()
        {
            OnUIOpenStateUpdate(_openShopEvent);
            
            if(_closeButton) _closeButton.onClick.AddListener(
                ()=>OnUIOpenStateUpdate(_openShopEvent));
            if(_buySeedInCart)_buySeedInCart.onClick.AddListener(
                delegate { OnUIButtonClicked(ShopEventType.BuySeedInCart); });
            if(_releaseSeedInCart)_releaseSeedInCart.onClick.AddListener(
                delegate { OnUIButtonClicked(ShopEventType.ReleaseSeedInCart); });

            CreateButtonAsync("Buy");

            this.Subscribe<ShareData.OpenShopEvent>(OnUIOpenStateUpdate);
            this.Subscribe<ShareData.CartEvent>(OnCartEventUpdated);

        }

        private void OnCartEventUpdated(ShareData.CartEvent cartEvent)
        {
            if (cartEvent.isBuy)
            {
                
            }
            else if (cartEvent.isRelease)
            {

            }
            else
            {
                var itemName = cartEvent.itemNamePicked;
                var textUI = _textCartMaps[itemName];
                textUI.text = String.Format(textUI.text, itemName, _amountCartMaps[itemName]++);
              
            }
        }

        private void CreateCartUI(Item item)
        {
            if (item.IsSeedingable == false && item.IsAnimal)
                return;
            var text = Instantiate(_prefabText, _cartRoot);
            text.text = String.Format("{0} amount: {1}", item.ItemName, 0);
            _textCartMaps.Add(item.ItemName, text);
            _amountCartMaps.Add(item.ItemName, 0);
        }

        private void CreatSumPickUI()
        {
            var text = Instantiate(_prefabText, _cartRoot);
            text.text = String.Format("Total seed amount: {0}",0);

        }
        
        
        private void CreateButtonAsync(string interactName)
        {
            var items = _dataLoader.ItemCollection;
            foreach (var item in items)
            {
                if (item.Value.IsBuyInShop == false)
                    continue;
                var button = Instantiate(_buttonPrefab, _root);
                var name = $"{interactName} {item.Value.BuyUnit}" + " " + item.Key;
                if (item.Value.IsSeedingable  && item.Value.IsAnimal == false)
                {
                    name = $"Pick {item.Value.BuyUnit}" + " " + item.Key;
                }
                button.name = name;
                button.GetComponentInChildren<Text>().text = name;
                button.onClick.AddListener(
                    delegate { OnUIButtonClicked(ShopEventType.Buy, item.Key); });
                button.gameObject.SetActive(true);

                CreateCartUI(item.Value);
            }

            CreatSumPickUI();
        }
        
        private void OnUIButtonClicked(ShopEventType eShopEvent, string itemName = null)
        {
            Debug.Log($"InteractEvent: {eShopEvent} of {itemName} ");

            _shopManager.OnShopUIEventRaised(eShopEvent, itemName);
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