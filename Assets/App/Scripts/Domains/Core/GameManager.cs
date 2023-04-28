using App.Scripts.Mics;

namespace App.Scripts.Domains.Core
{
    public class GameManager : MiddlewareBehaviour
    {
        

        private WorkerManager _workerManager;
        private ToolManager _toolManager;
        private PlotManager _plotManager;
        private ShopManager _shopManager;
        private StatManager _statManager = new();
        
        
        private void Awake()
        {
            _statManager.SyncFromLocalDB();
            _plotManager = new(_statManager);
            _toolManager = new(_statManager);
            _workerManager = new(_statManager);
            _shopManager = new(_statManager, _plotManager);
            

            this.Subscribe<ShareData.InteractButtonsUIEvent>(OnInteractButtonsUIEventRaised);
            this.Subscribe<ShareData.ShopUIEvent>(_shopManager.OnShopUIEventRaised);
            
        }

        private void OnInteractButtonsUIEventRaised(ShareData.InteractButtonsUIEvent uiEvent)
        {

            switch (uiEvent.EInteractEvent)
            {
                case ShareData.InteractEventType.RentWorker:
                    _workerManager.RentWorker(uiEvent.EInteractEvent);
                    break;
                case ShareData.InteractEventType.UpgradeTool:
                    _toolManager.UpgradeTool(uiEvent.EInteractEvent);
                    break;
                case ShareData.InteractEventType.GetMilk:
                    break;
                case ShareData.InteractEventType.Sell:
                    break;

            }
        }


    }
}