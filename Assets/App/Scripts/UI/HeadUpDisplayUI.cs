using System;
using System.Text;
using App.Scripts.Domains.Core;
using App.Scripts.Domains.Models;
using App.Scripts.Mics;
using UnityEngine;
using UnityEngine.Serialization;
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
        }

        private void OnDataUpdated(Stat Stat)
        {
            _texts[0].text = $"Gold: {Stat.GoldAmount}";
            _texts[1].text  = $"Tool Lv.{Stat.ToolLevel}";
            _texts[2].text = $"Idle Worker: {Stat.IdleWorkerAmount}";
            _texts[3].text = $"Working Worker: {Stat.WorkingWorkerAmount}";
            _texts[4].text = $"Unused Seeds: {Stat.GetSumUnusedSeeds}";
            _texts[5].text = $"Unused Plots: {Stat.UnusedPlotAmount}";
            _texts[6].text = $"Using Plots: {Stat.UsingPlotAmount}";
            _texts[7].text = $"Blueberries: {Stat.BlueberryProductAmount}";
            _texts[8].text = $"Tomatoes: {Stat.TomotoProductAmount}";
            _texts[9].text = $"Strawberries: {Stat.StrawberryProductAmount}";
            _texts[10].text = $"Milk gallons: {Stat.MilkProductAmount}";

        }

    }
}