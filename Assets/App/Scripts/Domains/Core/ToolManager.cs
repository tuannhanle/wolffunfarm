using App.Scripts.Domains.Models;
using App.Scripts.Mics;

namespace App.Scripts.Domains.Core
{
    public class ToolManager
    {
        private readonly StatManager _statManager;
        private readonly Tool _tool = new();



        public ToolManager()
        {
            DependencyProvider.Instance.RegisterDependency(typeof(ToolManager), this);
            _statManager = DependencyProvider.Instance.GetDependency<StatManager>();
            _tool.Level = _statManager.ToolLevel;
        }

        
        public void UpgradeTool()
        {
            var result = _tool.BeBoughtBy<Plot>(_statManager.Gold);
            if (result == null)
                return;
            //TODO: save new <tool level> to storage
            _tool.UpLevel();
        }

        public int? GetToolLevel => _tool.Level;
    }
}