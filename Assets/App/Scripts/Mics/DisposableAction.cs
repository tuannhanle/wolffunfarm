using System;
using JetBrains.Annotations;
using UnityEngine;

namespace App.Scripts.Mics
{
    public class DisposableAction : IDisposable
    {
        public DisposableAction([NotNull] Action a)
        {
            DisposeAction = a;
        }

        private Action DisposeAction;

        public void Dispose()
        {
            DisposeAction?.Invoke();
            // Prevent double dispose
            DisposeAction = null;
        }

        ~DisposableAction()
        {
            if (DisposeAction != null)
            {
                Debug.LogError($"Leaked Disposable Action: ${DisposeAction.Method}");
            }
        }
    }
}
