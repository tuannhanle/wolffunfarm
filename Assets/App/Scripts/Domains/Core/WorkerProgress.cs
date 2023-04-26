using System.Collections.Generic;
using App.Scripts.Domains.Models;

namespace App.Scripts.Domains.Core
{
    public class WorkerProgress 
    {
        private List<Worker> _workers = new List<Worker>();
        private bool _isInit = false;
        private int _amountWorker = 0;
        
        // TODO: Get amount of worker from db
        public int GetAmountWorkerDB { get { return 1; } }

        
        public void Init()
        {
            _isInit = true;
            _amountWorker = _amountWorker == 0 ? GetAmountWorkerDB : _amountWorker;
            for (int i = 0; i < _amountWorker; i++)
            {
                _workers.Add(new Worker());
            }
        }
        
        public void GetProgress(Progress progress)
        {
            if (_isInit == false) Init();
            foreach (var data in progress.datas)
            {
                
            }
            
        }

    }
}