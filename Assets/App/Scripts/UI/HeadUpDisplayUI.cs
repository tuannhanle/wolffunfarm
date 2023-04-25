using App.Scripts.Mics;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.UI
{
    public class HeadUpDisplayUI : MiddlewareBehaviour
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
            this.Subscribe<ShareData.HeadUpDisplayData>(OnDataUpdated);
        }

        private void OnDataUpdated(ShareData.HeadUpDisplayData headUpDisplayData)
        {
            _goldText.text = $"{headUpDisplayData.Golden.Name}: {headUpDisplayData.Golden.Amount}";
            _workerText.text = $"{headUpDisplayData.Workder.Name}: {headUpDisplayData.Workder.Amount}/{headUpDisplayData.Workder.Capacity}";
            _plotText.text = $"{headUpDisplayData.Plot.Name}: {headUpDisplayData.Plot.Amount}/{headUpDisplayData.Plot.Capacity}";

        }

    }
}