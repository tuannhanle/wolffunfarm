using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Mics
{
    /// <summary>
    /// Wraps MonoBehaviour for MetanomyLabs standard functionality, and provides a control layer for
    /// moderating use of typical Unity messages like Awake, Update, and OnDestroy
    /// </summary>
    public abstract class MiddlewareBehaviour : MonoBehaviour
    {
        [NonSerialized]
        private List<IDisposable> OnDestroyDisposables;

        protected void OnDestroy()
        {
            // clean up
            if (OnDestroyDisposables != null)
            {
                foreach (IDisposable disposable in OnDestroyDisposables)
                {
                    disposable.Dispose();
                }
                OnDestroyDisposables.Clear();
                OnDestroyDisposables = null;
            }
        }
        
        
        
        /// <typeparam name="T"></typeparam>
        public void Subscribe<T>(Action<T> handler)
        {
            IDisposable unsubscribe = DataBindingRegistry.Instance.Bind(handler);
            AddCleanup(unsubscribe);
        }
        
        public void AddCleanup(IDisposable disposable)
        {
            if (OnDestroyDisposables == null)
            {
                OnDestroyDisposables = new List<IDisposable>();
            }
            OnDestroyDisposables.Add(disposable);
        }

    }
}
