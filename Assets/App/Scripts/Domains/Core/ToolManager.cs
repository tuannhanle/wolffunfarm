using App.Scripts.Domains.Models;
using App.Scripts.Mics;
using UnityEngine;

namespace App.Scripts.Domains.Core
{
    public class ToolManager : Dependency<ToolManager>
    {
        private Tool _tool = new();
        
        public void Init()
        {
            base.Init();
            _tool.Level = _statManager.ToolLevel;
        }
        
        
        public bool UpgradeTool()
        {
            var isPayable = _paymentService.Buy(_tool);
            if (isPayable)
            {
                _tool.UpLevel();
                _statManager.Gain<Tool>();
                return true;
            }
            return false;
        }

        public int? GetToolLevel => _tool.Level;

        public float GetPercentPerLevel => _tool.GetPercentPerLevel;
    }
}