using App.Scripts.Domains.Core;
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

        // State
        private bool _isClose = true;
        
        private ShareData.OpenShopEvent _openShopEvent = new() { };
        
        private ShopManager _shopManager;
        private DataLoader _dataLoader;

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
                button.name = name;
                button.GetComponentInChildren<Text>().text = name;
                button.onClick.AddListener(
                    delegate { OnUIButtonClicked(ShopEventType.Buy, item.Key); });
                button.gameObject.SetActive(true);
            }
        }
        
        private void OnUIButtonClicked(ShopEventType eShopEvent, string itemName = null)
        {
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