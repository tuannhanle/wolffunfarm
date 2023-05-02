using App.Scripts.Domains.Models;
using App.Scripts.Mics;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.UI
{
    public class HeadUpDisplayUI : MiddlewareBehaviour
    {
        [Header("Refs")] 
        [SerializeField] private Text _textPrefab;
        [SerializeField] private Transform _root;
        
        // private StringBuilder[] _displayContentSbs = new StringBuilder[12];
        private Text[] _texts = new Text[11];
        
        private void Awake()
        {
            for (int i = 0; i < _texts.Length; i++)
            {
                var textUI = Instantiate(_textPrefab,_root);
                textUI.gameObject.SetActive(true);
                _texts[i] = textUI;
            }
            this.Subscribe<Stat>(OnDataUpdated);
            this.Subscribe<ShareData.WorkerPassage>(OnDataUpdated);
            this.Subscribe<ShareData.ItemStoragePassage>(OnDataUpdated);
      }

        private void OnDataUpdated<T>(T data)
        {
            if (typeof(T) == typeof(Stat))
            {
                var passager =  data as Stat;
                _texts[0].text = $"Gold: {passager.GoldAmount}";
                _texts[1].text  = $"Tool Lv.{passager.ToolLevel}";
            }
            else if (typeof(T) == typeof(ShareData.WorkerPassage))
            {
                var passager =  data as ShareData.WorkerPassage;
                _texts[2].text = $"Idle Worker: {passager.IdleWorkerAmount}";
                _texts[3].text = $"Working Worker: {passager.WorkingWorkerAmount}";
            }
            else if (typeof(T) == typeof(ShareData.ItemStoragePassage))
            {
                var passager =  data as ShareData.ItemStoragePassage;
                _texts[4].text = $"Unused Seeds: {passager.GetSumUnusedSeeds}";
                _texts[5].text = $"Unused Plots: {passager.UnusedPlotAmount}";
                _texts[6].text = $"Using Plots: {passager.UsingPlotAmount}";
                _texts[7].text = $"Blueberries: {passager.BlueberryProductAmount}";
                _texts[8].text = $"Tomatoes: {passager.TomotoProductAmount}";
                _texts[9].text = $"Strawberries: {passager.StrawberryProductAmount}";
                _texts[10].text = $"Milk gallons: {passager.MilkProductAmount}";
            }
        }

    }
}