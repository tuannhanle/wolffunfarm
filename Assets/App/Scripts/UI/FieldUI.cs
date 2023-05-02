using App.Scripts.Domains.Core;
using App.Scripts.Mics;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.UI
{
    public class FieldUI : MiddlewareBehaviour
    {
        [Header("Refs")] 
        [SerializeField] private PlotUI _plotPrefab;
        [SerializeField] private Transform _root;

        private DataLoader _dataLoader;

        private void Start()
        {
            _dataLoader = DependencyProvider.Instance.GetDependency<DataLoader>();
            CreateFieldAsync();
        }
        
        private void CreateFieldAsync()
        {
            // var items = _dataLoader.ItemCollection;
            // Item item = items["Plot"] ;
            var plots = _dataLoader.PlotStorage;
            foreach (var plotPair in plots)
            {
                var field = Instantiate(_plotPrefab, _root);
                var name = $"<b>Plot {plotPair.Value.Id}</b>";
                field.gameObject.name = name;
                field.Id = name;
                field.ItemName = plotPair.Value.IsUsing == false ? "<i>ready</i>":plotPair.Value.ItemName;
                field.ProductAmount =  $"Product: {plotPair.Value.ProductAmount}";
                field.TimeRemain = $"Next: {plotPair.Value.TimeUntilHarvest}s left";
                field.gameObject.SetActive(true);
            }
        }   
    }
}