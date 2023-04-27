using App.Scripts.Domains.Models;
using App.Scripts.Mics;

namespace App.Scripts.Domains.Core
{
    public class ToolManager
    {
        private readonly StatManager _statManager;
        private readonly Tool _tool = new();



        public ToolManager(StatManager statManager)
        {
            _statManager = statManager;
            _tool.Level = statManager.ToolLevel;
        }

        
        public void UpgradeTool(ShareData.InteractEventType? uiEventEInteractEvent)
        {
            if (uiEventEInteractEvent != ShareData.InteractEventType.UpgradeTool)
                return;
            if(_statManager.Gold.IsPayable(_tool.Price) == false)
                return;
            _statManager.Gold.Pay(_tool.Price);
            //TODO: save new <tool level> to storage
            _tool.UpLevel();
        }
    }
}