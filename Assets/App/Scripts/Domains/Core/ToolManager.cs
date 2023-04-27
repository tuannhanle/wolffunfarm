using App.Scripts.Domains.Models;
using App.Scripts.Mics;

namespace App.Scripts.Domains.Core
{
    public class ToolManager
    {
        private Tool _tool = new Tool();

        // TODO: get it from DB
        private int _amountMoney = 1000;
        public int AmountMoney => _amountMoney;

        public void UpgradeTool(ShareData.InteractEventType? uiEventEInteractEvent)
        {
            if (uiEventEInteractEvent != ShareData.InteractEventType.UpgradeTool)
                return;
            if (_amountMoney < Tool.Price)
                return;
            _amountMoney -= Tool.Price;
            //TODO: save new <tool level> to storage
            _tool.UpLevel();
        }
    }
}