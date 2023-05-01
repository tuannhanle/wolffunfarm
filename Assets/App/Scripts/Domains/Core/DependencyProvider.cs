using System;
using System.Collections.Generic;

namespace App.Scripts.Domains.Core
{
    public class DependencyProvider : Singleton<DependencyProvider>
    {
        Dictionary<Type, object> _dependencies = new Dictionary<Type, object>();

        public void RegisterDependency(Type contract, object instance, bool replace = true)
        {
            if (!contract.IsAssignableFrom(instance.GetType()))
            {
                throw new ArgumentException("Provided instance does not fulfill the contract, " + contract.Name);
            }
            if (HasDependency(contract) && !replace)
            {
                // Log.Error("Instance for contract " + contract.Name + " is already registered and replace is set to false.");
                return;
            }
            _dependencies[contract] = instance;
        }
        
        public bool HasDependency(Type contract)
        {
            return _dependencies.ContainsKey(contract);
        }
        
        public T GetDependency<T>() where T : IDependency
        {
            Type type = typeof (T);

            if (_dependencies.ContainsKey(type))
            {
                //Note: Due to RegisterDep interface it should be impossible for this cast to fail.  
                return (T) _dependencies[type];
            }
            else
                throw new MissingDependencyException(typeof(T));
        }
    }

    public class MissingDependencyException : Exception
    {
        public MissingDependencyException(Type requestedDependency) : 
            base("Unable to fetch dependency of type: " + requestedDependency.Name + ".")
        { }
    }
}