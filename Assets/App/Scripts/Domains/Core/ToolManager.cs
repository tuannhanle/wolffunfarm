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
            var gold = _statManager.Gold;
            var result = _tool.BeBoughtBy(gold);
            if (result == null)
                return false;
            //TODO: save new <tool level> to storage
            _tool.UpLevel();
            return true;
        }

        public int? GetToolLevel => _tool.Level;
    }
}