using App.Scripts.Domains.Models;

namespace App.Scripts.Domains.Core
{
    public class ToolManager : Dependency<ToolManager>, IDependency
    {
        private Tool _tool = new();
        public void Init()
        {
            base.Init();
            _tool.Price = 500;
            _tool.Level = _dataLoader.stat.ToolLevel;
        }
        
        
        public bool UpgradeTool()
        {
            var isPayable = _paymentService.Buy(_tool);
            if (isPayable)
            {
                _tool.UpLevel();
                _statManager.UpdateTool();
                return true;
            }
            return false;
        }

        public int? GetToolLevel => _tool.Level;

        public float GetPercentPerLevel => _tool.GetPercentPerLevel;
    }
}