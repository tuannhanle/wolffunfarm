using App.Scripts.Mics;

namespace App.Scripts.Domains.Core
{
    public class GameManager : MiddlewareBehaviour
    {
        private DependencyProvider _dependencyProvider = new();

        private WorkerManager _workerManager;
        private ToolManager _toolManager;
        private PlotManager _plotManager;
        private ShopManager _shopManager;
        private StatManager _statManager;
        
        
        private void Awake()
        {
            _statManager = new();
            _workerManager = new();
            _toolManager= new();
            _plotManager= new();
            _shopManager= new();

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