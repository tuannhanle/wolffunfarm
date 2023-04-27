using System;
using App.Scripts.Domains.Models;

namespace App.Scripts.Mics
{
    public class ShareData
    {
        public class HeadUpDisplayData
        {
            public Currency Golden { get; set; }
            public Currency Workder { get; set; }
            public Currency Plot { get; set; }

        }

        public enum InteractEventType
        {
            OpenShop, 
            RentWorker, 
            UpgradeTool, 
            GetMilk, 
            Sell
            
        }

        public class InteractButtonsUIEvent
        {
            public InteractEventType EInteractEvent { get; set; }
        }

        public enum ShopEventType
        {
            BBlueBerry,
            BTomato,
            BStrawBerry,
            BCow,
            BPlot,
            BuySeedInCart,
            ReleaseSeedInCart
        }
        
        public class ShopUIEvent
        {
            public ShopEventType EShopUIEvent { get; set; } 
        }
    }
}