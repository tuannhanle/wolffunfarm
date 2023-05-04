using System;
using App.Scripts.Domains.GameObjects;
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
        
        private const string PRODUCT_AMOUNT =  "Product: {0}/{1}";
        private const string REMAIN_TIME = "Next: {0}s left";
        private const string PLOT_NAME = "<b>Plot {0}</b>";
        
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

        public void SetUp(Plot plot)
        {                
            var plotName = String.Format(PLOT_NAME, plot.Id);
            this.gameObject.name = plotName;
            
            this.Id = plotName;
            
            this.ItemName = 
                plot.IsUsing ? 
                    plot.ItemName
                    : "<i>ready</i>";
            
            this.ProductAmount = 
                plot.IsUsing ? 
                    String.Format(PRODUCT_AMOUNT, plot.ProductOnPlot, plot.ProductCapacity)
                    :null;
            
            this.TimeRemain = 
                plot.IsUsing ?
                    String.Format(REMAIN_TIME, FormatTime(plot.TimeUntilHarvest))
                    : null; 
            
            this.gameObject.SetActive(true);
        }

        private string FormatTime(long seconds)
        {
            return TimeStamp.GetTimeString(Math.Abs(seconds));
        }
        
    }
}