using System;
using System.Collections.Generic;
using App.Scripts.Domains.Core;
using App.Scripts.Domains.Models;
using App.Scripts.Mics;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace App.Scripts.UI
{
    
    public enum ShopEventType
    {
        Buy,
        BuyCart,
        RenewCart
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

        // private Dictionary<string, Text> _textCartMaps = new();
        private Dictionary<string, CategoryUI> _categoryMaps = new();
        private Text _sumCart;

        private const string TOTAL = "Total seed amount: {0}";
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
                delegate
                {
                    OnUIOpenStateUpdate(_openShopEvent);
                    OnUIButtonClicked(ShopEventType.RenewCart);
                    OnCartEventUpdated(new ShareData.CartEvent() { isRelease = true });
                });
            if(_buySeedInCart)_buySeedInCart.onClick.AddListener(
                delegate { OnUIButtonClicked(ShopEventType.BuyCart); });
            if(_releaseSeedInCart)_releaseSeedInCart.onClick.AddListener(
                delegate
                {
                    OnUIButtonClicked(ShopEventType.RenewCart);
                    OnCartEventUpdated(new ShareData.CartEvent() { isRelease = true });
                });

            CreateButtonAsync("Buy");

            this.Subscribe<ShareData.OpenShopEvent>(OnUIOpenStateUpdate);
            this.Subscribe<ShareData.CartEvent>(OnCartEventUpdated);

        }

        private void OnCartEventUpdated(ShareData.CartEvent cartEvent)
        {
            if (cartEvent.isBuy || cartEvent.isRelease)
            {
                foreach (var caterogy in _categoryMaps)
                {
                    caterogy.Value.Clear();
                }
                _sumCart.text = String.Format(TOTAL, cartEvent.amountTotalOrder);
            }
            else
            {
                if(cartEvent.amountPick <= 0)
                    return;
                _sumCart.text = String.Format(TOTAL, cartEvent.amountTotalOrder);
                var itemName = cartEvent.itemNamePicked;
                var textUI = _categoryMaps[itemName];
                textUI.Gain(cartEvent.amountPick);
              
            }
        }

        private void CreateCartUI(Item item)
        {
            if (item.IsSeedingable && item.IsAnimal == false)
            {
                var text = Instantiate(_prefabText, _cartRoot);
                var cartTextComponent = text.gameObject.AddComponent<CategoryUI>();
                cartTextComponent.SetUp(text, item.ItemName, 0);
                _categoryMaps.Add(item.ItemName, cartTextComponent);
            }
        }

        private void CreatSumPickUI()
        {
            var text = Instantiate(_prefabText, _cartRoot);
            text.text = String.Format(TOTAL,0);
            _sumCart = text;

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

    public class CategoryUI : MiddlewareBehaviour
    {

        public string ItemName { get; set; }
        public int Amount { get; set; }
        public Text CategoryText { get; set; }

        private const string ITEM_AMOUNT = "{0} amount: {1}";

        public void SetUp(Text text, string itemName, int amount)
        {
            ItemName = itemName;
            Amount = amount;
            this.CategoryText = text;
            CategoryText.text = String.Format(ITEM_AMOUNT, ItemName, Amount);

        }
        
        public void Gain(int amount)
        {
            Amount += amount;
            CategoryText.text = String.Format(ITEM_AMOUNT, ItemName, Amount);

        }

        public void Clear()
        {
            Amount = 0;
            CategoryText.text = String.Format(ITEM_AMOUNT, ItemName, Amount);

        }
    }
}