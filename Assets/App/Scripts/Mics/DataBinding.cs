using System;
using System.Collections.Generic;
using App.Scripts.Mics;

public interface IDataInlet<in T> : IDisposable
{
    void UpdateValue(T value);
}

public class DataBindingProvider<T> : IDataInlet<T>
{
    /// <summary>
    /// is there currently an inlet claiming this data
    /// </summary>
    private bool IsClaimed;

    /// <summary>
    /// callbacks to call whenever a new value is published
    /// </summary>
    private Action<T> ListenerCallbacks;

    /// <summary>
    /// this is the last value that was published for type T.
    /// Anybody who registers as a listener is guaranteed to receive it 
    /// </summary>
    private T LastValue;

    /// <summary>
    /// track whether value for this data type has ever been set- if not, we do not automatically dispatch anything to listeners who bind.
    /// however, if the value _has_ been set, even if the LastValue is default(T), it is still published immediately to listeners.
    /// (note this value is necessary because otherwise there is no way to to distinguish between 
    /// </summary>
    private bool HasValueEverBeenUpdated;

    public void Claim()
    {
        // Log.AssumeThrow(!IsClaimed, 
        //     "It is unsafe to claim DataBindingProvider for {0} because it is already claimed by another inlet", typeof(T));
        IsClaimed = true;
    }

    public void Dispose()
    {
        // when disposed, we are no longer claimed
        IsClaimed = false;
    }

    public IDisposable Bind(Action<T> handler)
    {
        if (HasValueEverBeenUpdated)
        {
            handler(LastValue);
        }

        // add the handler _after_ we call it, so we don't leak the delegate in case the call blows up
        ListenerCallbacks += handler;
        return new DisposableAction(() => ListenerCallbacks -= handler);
    }

    public void UpdateValue(T value)
    {
        HasValueEverBeenUpdated = true;
        LastValue = value;
        ListenerCallbacks?.Invoke(value);
    }
}

public class DataBindingRegistry : IDisposable
{
    // DataBindingRegistry is explicitly Disposed during logout, which clears this instance
    [OnLogoutPersist] private static DataBindingRegistry _Instance;

    public static DataBindingRegistry Instance
    {
        get { return _Instance ?? (_Instance = new DataBindingRegistry()); }
    }

    private readonly Dictionary<Type, object> BindingProviders = new Dictionary<Type, object>();

    private bool IsAlreadyDisposed;

    private DataBindingRegistry()
    {
    }

    ~DataBindingRegistry()
    {
        Dispose();
    }

    public void Dispose()
    {
        if (IsAlreadyDisposed)
        {
            return;
        }

        _Instance = null;
        IsAlreadyDisposed = true;

        GC.SuppressFinalize(this);
    }

    public DataBindingProvider<T> RegisterDataProvider<T>()
    {
        DataBindingProvider<T> provider = GetOrCreateProvider<T>();
        provider.Claim();
        return provider;
    }

    public IDisposable Bind<T>(Action<T> handler)
    {
        DataBindingProvider<T> provider = GetOrCreateProvider<T>();
        return provider.Bind(handler);
    }

    private DataBindingProvider<T> GetOrCreateProvider<T>()
    {
        Type typeKey = typeof(T);

        // Lookup existing value
        if (BindingProviders.TryGetValue(typeKey, out object existingProvider))
        {
            return (DataBindingProvider<T>) existingProvider;
        }

        // Make new and register
        DataBindingProvider<T> provider = new DataBindingProvider<T>();
        BindingProviders.Add(typeKey, provider);
        return provider;
    }
}

/// <summary>
/// An inlet which will register or claim its own provider upon first value
/// assignment. Can be used conveniently as a member variable, but remember
/// to call Dispose if you need to return the provider to the pool.
/// </summary>
/// <typeparam name="T"></typeparam>
public struct LazyDataInlet<T> : IDataInlet<T> where T : class
{
    private DataBindingProvider<T> Provider;
    public void UpdateValue(T value)
    {
        if (Provider == null)
        {
            Provider = DataBindingRegistry.Instance.RegisterDataProvider<T>();
        }
        Provider.UpdateValue(value);
    }

    public void Dispose()
    {
        if (Provider == null)
        {
            return;
        }
        Provider.Dispose();
        Provider = null;
    }
}