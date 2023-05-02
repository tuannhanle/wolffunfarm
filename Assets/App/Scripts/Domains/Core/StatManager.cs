using App.Scripts.Domains.Models;

namespace App.Scripts.Domains.Core
{
    public class StatManager : Dependency<StatManager>, IDependency
    {
        private LazyDataInlet<Stat> _statsUpdatedInlet = new();

        public void Init()
        {
            base.Init();
            PostcastData();
        }
        
        public void PostcastData()
        {
            var stat = _dataLoader.stat;
            _statsUpdatedInlet.UpdateValue(stat);
        }
        
        public void CheatGoldAmount(ICheat iCheat)
        {
            _dataLoader.stat.GoldAmount = iCheat.GoldAmount;
        }

        public void UpdateTool()
        {
            _dataLoader.stat.ToolLevel+=1;
            _dataLoader.Push<Stat>();
            PostcastData();
        }

        public void Pay(int amount)
        {
            _dataLoader.stat.GoldAmount-=amount;
            _dataLoader.Push<Stat>();
            PostcastData();
        }        
        
        public void Gain(int amount)
        {
            _dataLoader.stat.GoldAmount+=amount;
            _dataLoader.Push<Stat>();
            PostcastData();
        }
        
        
        
        public void SellAllProduct(int amountStock)
        {
            // _stat.BlueberryProductAmount = 0;
            // _stat.TomotoProductAmount = 0;
            // _stat.StrawberryProductAmount = 0;
            // _stat.MilkProductAmount = 0;
            // _stat.GoldAmount += amountStock;
        }
    }

    public interface ICheat
    {
        int GoldAmount { get; set; }
    }
}