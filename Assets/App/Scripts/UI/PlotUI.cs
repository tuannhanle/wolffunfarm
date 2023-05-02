using App.Scripts.Mics;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.UI
{
    public class PlotUI : MiddlewareBehaviour
    {
        [SerializeField] private Text _idText;
        [SerializeField] private Text _itemNameText;
        [SerializeField] private Text _productAmountText;
        [SerializeField] private Text _timeRemainText;
        
        public string Id
        {
            get { return _idText.text;}
            set { _idText.text = value; }
        }
        public string ItemName
        {
            get { return _itemNameText.text;}
            set { _itemNameText.text = value; }
        }
        public string ProductAmount
        {
            get { return _productAmountText.text;}
            set { _productAmountText.text = value; }
        }
        public string TimeRemain
        {
            get { return _timeRemainText.text;}
            set { _timeRemainText.text = value; }
        }
        
    }
}