using System;
using System.Collections;
using System.Collections.Generic;
using App.Scripts.Domains.GameObjects;
using App.Scripts.Domains.Models;
using Sinbad;
using UnityEditor;
using UnityEngine;

namespace App.Scripts.Domains.Core
 {
    public interface IHasItemName 
    {
        string ItemName { get; set; }
    }
    public class DataLoader : Dependency<DataLoader>, IDependency
    {
        public const string ITEM_DATA_FILE = "ItemData.csv";
        public const string UNUSED_DATA_FILE = "UnusedData.csv";
        public const string USING_DATA_FILE = "UsingData.csv";
        public const string PLOT_DATA_FILE = "PlotData.csv";
        public const string STAT_DATA_FILE = "StatData.csv";
        public const string CONTAIN_DATA_FILE = "ContainData.csv";
        public Dictionary<string, Item> ItemCollection = new ();
        public Dictionary<string, Unused> UnusedCollection = new ();
        public Dictionary<string, Using> UsingCollection = new ();
        public Dictionary<string, Plot> PlotCollection = new ();
        public Stat stat = new Stat();
        public Constant constant = new Constant();

        private const string DATA_FOLDER_PREFIX = "Data/{0}";
        public void Fetch()
        {
            _dataLoader.LoadObjects(String.Format(DATA_FOLDER_PREFIX,ITEM_DATA_FILE), _dataLoader.ItemCollection);
            _dataLoader.LoadObjects(String.Format(DATA_FOLDER_PREFIX,UNUSED_DATA_FILE), _dataLoader.UnusedCollection);
            _dataLoader.LoadObjects(String.Format(DATA_FOLDER_PREFIX,USING_DATA_FILE), _dataLoader.UsingCollection);
            _dataLoader.LoadObjects(String.Format(DATA_FOLDER_PREFIX,PLOT_DATA_FILE), _dataLoader.PlotCollection);
            LoadObject(String.Format(DATA_FOLDER_PREFIX,STAT_DATA_FILE), ref stat);
            LoadObject(String.Format(DATA_FOLDER_PREFIX,CONTAIN_DATA_FILE), ref constant);
        }

        public void Push()
        {
            _dataLoader.SaveObjects(_dataLoader.ItemCollection,String.Format(DATA_FOLDER_PREFIX,ITEM_DATA_FILE));
            _dataLoader.SaveObjects(_dataLoader.UnusedCollection,String.Format(DATA_FOLDER_PREFIX,UNUSED_DATA_FILE));
            _dataLoader.SaveObjects(_dataLoader.UsingCollection,String.Format(DATA_FOLDER_PREFIX,USING_DATA_FILE));
            _dataLoader.SaveObjects(_dataLoader.PlotCollection,String.Format(DATA_FOLDER_PREFIX,PLOT_DATA_FILE));

        }
        
        private void LoadObjects<T>(string filename, Dictionary<string,T> maps) where T: new()
        {
            List<T> objs = new List<T>();
            try
            {
                objs = CsvUtil.LoadObjects<T>(filename);
                foreach (IHasItemName obj in objs)
                {
                    if (maps.ContainsKey(obj.ItemName))
                        continue;
                    maps.Add(obj.ItemName, (T)obj);
                }
            }
            catch (Exception e)
            {
                CsvUtil.SaveObjects(objs, filename);
            }

        }

        private void LoadObject<T>(string filename, ref T destObject)
        {
            try
            {
                CsvUtil.LoadObject(filename, ref destObject);
            }
            catch (Exception e)
            {
                CsvUtil.SaveObject(destObject, filename);

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
        
        private void SaveObjects<T>(Dictionary<string,T> maps, string filename)
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