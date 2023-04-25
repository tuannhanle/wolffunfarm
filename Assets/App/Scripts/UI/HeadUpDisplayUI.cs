using System;
using App.Scripts.Domains.Models;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.UI
{
    public class HeadUpDisplayUI : MonoBehaviour
    {
        
        [Header("Texts")]
        [SerializeField] private Text _goldText;
        [SerializeField] private Text _workerText;
        [SerializeField] private Text _plotText;

        [Header("Buttons")]
        [SerializeField] private Button _shopButton;


        private void Awake()
        {
            _shopButton.onClick.AddListener(()=>{});
        }

        private void UpdateTexts(HeadUpDisplayData headUpDisplayData)
        {
            _goldText.text = $"{headUpDisplayData.Golden.Name}: {headUpDisplayData.Golden.Amount}";
            _workerText.text = $"{headUpDisplayData.Workder.Name}: {headUpDisplayData.Workder.Amount}/{headUpDisplayData.Workder.Capacity}";
            _plotText.text = $"{headUpDisplayData.Plot.Name}: {headUpDisplayData.Plot.Amount}/{headUpDisplayData.Plot.Capacity}";

        }
        
        
        public class HeadUpDisplayData
        {
            public Currency Golden { get; set; }
            public Currency Workder { get; set; }
            public Currency Plot { get; set; }

        }
    }
}