using System.Collections.Generic;
using System;
using UnityEngine;

namespace EudiFramework
{
    public class EudiConfiguration
    {
        private Dictionary<Type, object> _stockTypes = new Dictionary<Type, object>();
        /// <summary>
        /// Temporary variables used for '<see cref="DisposeFromBinders{BindInstance}"/>'.
        /// </summary>
        private List<Type> gc_BindingRemoval = new List<Type>();

        /// <summary>
        /// Get the binded instance of a specific interface
        /// </summary>
        /// <typeparam name="BindShare">The interface that was used to bind the instance</typeparam>
        /// <returns>An instance that was binded to the interface</returns>
        public BindShare GetBinding<BindShare>()
        {
            var type = typeof(BindShare);
            object myInstance;
            if (_stockTypes.TryGetValue(type, out myInstance))
                return (BindShare)myInstance;
            return default(BindShare);
        }

        /// <summary>
        /// Get all binder attached to the instance
        /// </summary>
        /// <typeparam name="BindInstance">The instance type</typeparam>
        /// <returns>The binders type</returns>
        public List<Type> SearchBindings<BindInstance>()
        {
            var hasFoundAnyValue = false;
            List<Type> list = null;

            foreach (var type in _stockTypes)
            {
                if (type.Value.GetType() == typeof(BindInstance))
                {
                    if (!hasFoundAnyValue)
                    {
                        hasFoundAnyValue = true;
                        list = new List<Type>();
                    }
                    list.Add(type.Key);
                }
            }

            return list;
        }

        /// <summary>
        /// Bind an instance to a class
        /// </summary>
        /// <typeparam name="BindShare"></typeparam>
        /// <typeparam name="BindInstance"></typeparam>
        public BindInstance Bind<BindShare, BindInstance>() where BindInstance : new()
        {
            var type = typeof(BindShare);
            var binderType = typeof(BindInstance);

            return (BindInstance)(_stockTypes[type] = binderType.IsSubclassOf(typeof(MonoBehaviour))
                ? new GameObject("[BINDING]" + binderType.Name, binderType).GetComponent<BindInstance>()
                : new BindInstance()); 
        }

        /// <summary>
        /// WIP METHOD (for now, it's just for removing the binding)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Dispose<BindShare>()
        {
            var type = typeof(BindShare);
            Dispose(type);
        }

        public void Dispose(Type bindShare)
        {
            object myInstance;
            if (_stockTypes.TryGetValue(bindShare, out myInstance))
            {
                UnityEngine.Debug.Log($"[IoC] instance({myInstance.GetType().Name}) from {bindShare.Name} was disposed!");
                _stockTypes.Remove(bindShare);
            }
        }

        /// <summary>
        /// WIP METHOD (for now, it's just for removing the instance binders)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void DisposeFromBinders<BindInstance>()
        {
            if (gc_BindingRemoval.Count >= 0)
                gc_BindingRemoval.Clear();

            var type = typeof(BindInstance);
            // Search for bindings...
            foreach (var stKVP in _stockTypes)
            {
                if (stKVP.Value.GetType() == type)
                    gc_BindingRemoval.Add(stKVP.Key);
            }
            // Remove bindings...
            var count = gc_BindingRemoval.Count;
            for (var i = 0; i < gc_BindingRemoval.Count; i++)
            {
                Dispose(gc_BindingRemoval[i]);
            }
        }
    }
}