using App.Scripts.Domains.Models;
using App.Scripts.Mics;
using UnityEngine;

namespace App.Scripts.Domains.Core
{
    public class ToolManager : Dependency<ToolManager>
    {
        private Tool _tool = new();
        private const int AMOUNT_EACH_UPDATE_TOOL_LEVEL = 1;
        public void Init()
        {
            base.Init();
            _tool.Level = _statManager.Stat.ToolLevel;
        }
        
        
        public bool UpgradeTool()
        {
            var isPayable = _paymentService.Buy(_tool);
            if (isPayable)
            {
                _tool.UpLevel();
                _statManager.Gain<Tool>(AMOUNT_EACH_UPDATE_TOOL_LEVEL);
                return true;
            }
            return false;
        }

        public int? GetToolLevel => _tool.Level;

        public float GetPercentPerLevel => _tool.GetPercentPerLevel;
    }
}