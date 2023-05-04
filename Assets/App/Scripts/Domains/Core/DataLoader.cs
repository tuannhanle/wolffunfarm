using System;
using System.Collections.Generic;
using App.Scripts.Domains.GameObjects;
using App.Scripts.Domains.Models;
using App.Scripts.Mics;
using Sinbad;
using UnityEngine;

namespace App.Scripts.Domains.Core
 {
   public class DataLoader : Dependency<DataLoader>, IDependency
    {
        public const string ITEM_DATA_FILE = "ItemData.csv";
        public const string ITEM_STORAGE_DATA_FILE = "ItemStorageData.csv";
        public const string PLOT_DATA_FILE = "PlotData.csv";
        public const string WORKER_DATA_FILE = "WorkerData.csv";
        public const string STAT_DATA_FILE = "StatData.csv";
        public const string CONTAIN_DATA_FILE = "ConstantData.csv";
        public const string JOB_DATA_FILE = "JobData.csv";
        public Dictionary<string, Item> ItemCollection = new ();
        public Dictionary<string, ItemStorage> ItemStorage = new();
        public Dictionary<int, Plot> PlotStorage = new ();
        public Dictionary<int, Worker> WorkerStorage = new();
        public Dictionary<long, Job> JobStorage = new();
        public Stat stat => _stats[0];
        public Constant constant => _constants[0];
        private List<Stat> _stats = new ();
        private List<Constant> _constants = new ();

        private const string DATA_FOLDER_PREFIX = "Data/{0}";

        public void Init()
        {
            base.Init();
            Fetch();
        }
        
        public void Fetch()
        {
            List<Item> objs1 = new ();
            objs1 = CsvUtil.LoadObjects<Item>(String.Format(DATA_FOLDER_PREFIX,ITEM_DATA_FILE));
            foreach (var obj in objs1)
            {
                if (ItemCollection.ContainsKey(obj.ItemName))
                    continue;
                ItemCollection.Add(obj.ItemName, obj);
   
            }
            
            List<ItemStorage> objs2 = new ();
            objs2 = CsvUtil.LoadObjects<ItemStorage>(String.Format(DATA_FOLDER_PREFIX,ITEM_STORAGE_DATA_FILE));
            foreach (var obj in objs2)
            {
                if (ItemStorage.ContainsKey(obj.ItemName))
                    continue;
                ItemStorage.Add(obj.ItemName, obj);
   
            }

            // _dataLoader.LoadObjects(String.Format(DATA_FOLDER_PREFIX,ITEM_DATA_FILE), _dataLoader.ItemCollection);
            // _dataLoader.LoadObjects(String.Format(DATA_FOLDER_PREFIX,ITEM_STORAGE_DATA_FILE), _dataLoader.ItemStorage);
            // _dataLoader.LoadObjects(String.Format(DATA_FOLDER_PREFIX,UNUSED_DATA_FILE), _dataLoader.UnusedCollection);
            // _dataLoader.LoadObjects(String.Format(DATA_FOLDER_PREFIX,USING_DATA_FILE), _dataLoader.UsingCollection);
            _dataLoader.LoadPlotStorage();
            _dataLoader.LoadWorkerStorage();
            _dataLoader.LoadJobStorage();
            _stats = CsvUtil.LoadObjects<Stat>(String.Format(DATA_FOLDER_PREFIX,STAT_DATA_FILE));
            _constants = CsvUtil.LoadObjects<Constant>(String.Format(DATA_FOLDER_PREFIX,CONTAIN_DATA_FILE));

        }

        public void Push()
        {
            _dataLoader.SaveObjects(_dataLoader.PlotStorage,String.Format(DATA_FOLDER_PREFIX,PLOT_DATA_FILE));
            _dataLoader.SaveObjects(_dataLoader.WorkerStorage,String.Format(DATA_FOLDER_PREFIX,WORKER_DATA_FILE));
            _dataLoader.SaveObjects(_dataLoader.JobStorage,String.Format(DATA_FOLDER_PREFIX,JOB_DATA_FILE));
            _dataLoader.SaveObjects(_dataLoader.ItemStorage,String.Format(DATA_FOLDER_PREFIX,ITEM_STORAGE_DATA_FILE));
            _dataLoader.SaveObjects(_dataLoader._stats,String.Format(DATA_FOLDER_PREFIX,STAT_DATA_FILE));
        }

        public void Push<T>()
        {
            if (typeof(T) == typeof(Worker))
            {
                _dataLoader.SaveObjects(_dataLoader.WorkerStorage,String.Format(DATA_FOLDER_PREFIX,WORKER_DATA_FILE));
                _broadcastService.BroadcastWorkerPassager();
            }

            if (typeof(T) == typeof(Plot))
            {
                _broadcastService.BroadcastPlotPassager();
                _dataLoader.SaveObjects(_dataLoader.PlotStorage,String.Format(DATA_FOLDER_PREFIX,PLOT_DATA_FILE));
            }             
            if(typeof(T) == typeof(Job))             
                _dataLoader.SaveObjects(_dataLoader.JobStorage,String.Format(DATA_FOLDER_PREFIX,JOB_DATA_FILE));           
            if(typeof(T) == typeof(Stat))             
                _dataLoader.SaveObjects(_dataLoader._stats,String.Format(DATA_FOLDER_PREFIX,STAT_DATA_FILE));
            if (typeof(T) == typeof(ItemStorage))
            {
                _dataLoader.SaveObjects(_dataLoader.ItemStorage,String.Format(DATA_FOLDER_PREFIX,ITEM_STORAGE_DATA_FILE));
                _broadcastService.BroadcastItemStoragePassager();
            }             
        }
        
        private void LoadJobStorage()
        {
            var filename = String.Format(DATA_FOLDER_PREFIX, JOB_DATA_FILE);
            List<Job> objs = new List<Job>();
            try
            { 
                objs = CsvUtil.LoadObjects<Job>(filename);
                foreach (var obj in objs)
                {
                    if (_dataLoader.JobStorage.ContainsKey(obj.JobId))
                        continue;
                    _dataLoader.JobStorage.Add(obj.JobId, obj);
                }
            }
            catch (Exception e)
            {
                Debug.Log("[LoadJobStorage]: "+e);
            }
        }
        
        private void LoadWorkerStorage()
        {
            var filename = String.Format(DATA_FOLDER_PREFIX, WORKER_DATA_FILE);
            List<Worker> objs = new List<Worker>();
            try
            { 
                objs = CsvUtil.LoadObjects<Worker>(filename);
                foreach (var obj in objs)
                {
                    if (_dataLoader.WorkerStorage.ContainsKey(obj.Id))
                        continue;
                    _dataLoader.WorkerStorage.Add(obj.Id, obj);
                }
            }
            catch (Exception e)
            {
                Debug.Log("[LoadWorkerStorage]: "+e);

            }
        }

        private void LoadPlotStorage()
        {
            var filename = String.Format(DATA_FOLDER_PREFIX, PLOT_DATA_FILE);
            List<Plot> objs = new List<Plot>();
            try
            { 
                objs = CsvUtil.LoadObjects<Plot>(filename);
                foreach (var obj in objs)
                {
                    if (_dataLoader.PlotStorage.ContainsKey(obj.Id))
                        continue;
                    _dataLoader.PlotStorage.Add(obj.Id, obj);
                }
            }
            catch (Exception e)
            {
                Debug.Log("[LoadPlotStorage]: "+e);

            }
        }
        
        // private void LoadObjects<T>(string filename, Dictionary<string,T> maps) where T: new()
        // {
        //     List<T> objs = new List<T>();
        //     try
        //     {
        //         objs = CsvUtil.LoadObjects<T>(filename);
        //         foreach (var obj in objs)
        //         {
        //             if (maps.ContainsKey(obj.ItemName))
        //                 continue;
        //             maps.Add(obj.ItemName, (T)obj);
        //
        //         }
        //     }
        //     catch (Exception e)
        //     {
        //         Debug.Log("[LoadObjects]: "+filename+e);
        //     }
        //
        // }

        private void LoadObject<T>(string filename, ref T destObject)
        {
            try
            {
                CsvUtil.LoadObject(filename, ref destObject);
            }
            catch (Exception e)
            {
                Debug.Log("[LoadObject]: "+filename+e);
            }
        }

        private void SaveObject<T>(T obj, string filename)
        {
            CsvUtil.SaveObject(obj, filename);
        }

        private void SaveObjects<T>(IEnumerable<T> objs, string filename)
        {
            CsvUtil.SaveObjects(objs, filename);
        }
        
        private void SaveObjects<Y,T>(Dictionary<Y,T> maps, string filename)
        {
            List<T> objs = new List<T>();
            foreach (var pair in maps)
            {
                objs.Add(pair.Value);
            }
            CsvUtil.SaveObjects(objs, filename);
        }



    }
}