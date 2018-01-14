using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace EudiFramework
{
    public class WrapperEudiEntity : MonoBehaviour
    {
        public EudiEntity Instance;

        private void Awake()
        {
            Instance = new EudiEntity(this);
        }

        private void OnDestroy()
        {
            Eudi.EntitiesManager._removeEntity(gameObject);
            
            Instance.Wrapper = null;

            Instance.Components.Clear();
            Instance.Components = null;

            Instance.EudiComponents.Clear();
            Instance.EudiComponents = null;

            Instance.SharedModules.Clear();
            Instance.SharedModules = null;

            Instance = null;
        }
    }

    [Serializable]
    public class EudiEntity
    {
        private static long _lastId = 0;

        public EudiMap ComponentMap;

        [NonSerialized]
        public List<Component> Components;
        [NonSerialized]
        public List<EudiComponentBehaviour> EudiComponents;
        [NonSerialized]
        public List<IShareableModule> SharedModules;

        public WrapperEudiEntity Wrapper;
        public long Id;

        internal EudiEntity(WrapperEudiEntity wrapper)
        {
            Components = new List<Component>();
            EudiComponents = new List<EudiComponentBehaviour>();
            SharedModules = new List<IShareableModule>();
            
            _lastId += 1;
            Id = _lastId;
            Wrapper = wrapper;
            Wrapper.Instance = this;
            
            _initAll();
        }

        public object GetComponent(Type type)
        {
            if (type.IsSubclassOf(typeof(EudiComponentBehaviour)))
            {
                var eComponentsLength = EudiComponents.Count;
                for (int i = 0; i < eComponentsLength; i++)
                {
                    var component = EudiComponents[i];
                    if (component.GetType() == type)
                        return component;
                }

                return null;
            }

            var nullComponentIndex = -1;
            var componentsLength = Components.Count;
            for (int i = 0; i < componentsLength; i++)
            {
                var component = Components[i];
                if (component == null) //< The component is null, maybe we can find another one?
                {
                    nullComponentIndex = i;
                    break;
                }
                if (component.GetType() == type)         
                    return component;
            }

            object toReturn;
            if ((toReturn = Wrapper.GetComponent(type)) != null)
                return toReturn;

            if (nullComponentIndex != 1)
            {
                Components.RemoveAt(nullComponentIndex);
            }

            return null;
        }

        public TComponent GetComponent<TComponent>()
        {
            var type = typeof(TComponent);
            var comp = GetComponent(type);
            return (TComponent)comp;
        }

        internal void _initAll()
        {
            // Get all components
            var components = Wrapper.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                var component = components[i];
                var compAsEudiBehaviour = component as EudiComponentBehaviour;
                var compAsEudiModule = component as WrapperModuleEntity;

                if (compAsEudiBehaviour != null) // Add eudi behaviour component to the list...
                {
                    if (!EudiComponents.Contains(compAsEudiBehaviour))
                    EudiComponents.Add(compAsEudiBehaviour);
                }
                else if (compAsEudiModule != null) // else add module...
                {
                    var instance = compAsEudiModule._instance;
                    _addModule(instance);
                }
                else // else add normal unity component... (monobehaviour, transform, camera, etc...)
                {
                    if (!Components.Contains(component))
                        Components.Add(component);
                }
            }
            
            _updateMap();
        }

        internal void _updateMap()
        {
            var tempList = new EudiStructList<Type>(0);
            
            // Get all components (SPAGHETTI CODE!!!)
            var components = Wrapper.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                var component = components[i];

                tempList.Add(component.GetType());
            }
            
            if (ComponentMap == null)
                ComponentMap = new EudiMap();

            ComponentMap.TuplesItems = new EudiMap.TupleItem[tempList.Length];
            for (int i = 0; i < tempList.Length; i++)
            {
                ComponentMap.TuplesItems[i] = new EudiMap.TupleItem()
                {
                    Type = tempList[i],
                    Attribute = null
                };
            }
            
            tempList.Dispose();
        }
        
        #region Shareable modules

        internal void _addModule(IShareableModule instance)
        {
            SharedModules.Add(instance);
            Eudi.EntitiesManager._initModule(Wrapper.gameObject, instance);
        }

        /// <summary>
        /// Will remove the module from the list but replace it into the tuples maps
        /// </summary>
        /// <param name="instance"></param>
        private void _removeModuleButReplaceIt(IShareableModule oldInstance)
        {
            SharedModules.Remove(oldInstance);
        }

        public bool ContainsModule(IModule module)
        {
            if (module is IShareableModule)
                return SharedModules.Contains((IShareableModule)module);
            var eComponentsCount = EudiComponents.Count;
            for (int i = 0; i < eComponentsCount; i++)
            {
                var component = EudiComponents[i] as EudiComponentBehaviourModulable;
                if (component != null
                    && component.AttachedModules.Contains(module))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Return true if we have this module
        /// </summary>
        /// <typeparam name="ModuleType">The instance type</typeparam>
        /// <returns>Return true if we have the module requested</returns>
        /// <remarks>
        /// For some obvious reasons, we don't return the found type in case it's a struct...
        /// </remarks>
        public bool HasModule<ModuleType>(ref int index) where ModuleType : IShareableModule
        {
            if (SharedModules == null)
            {
                Debug.LogError("Illogical exception, SharedModules was null.");
                SharedModules = new List<IShareableModule>();
            }

            var type = typeof(ModuleType);
            var modulesLength = SharedModules.Count;
            for (int i = 0; i < modulesLength; i++)
            {
                var module = SharedModules[i];
                if (module.GetType() == type)
                {
                    index = i;
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// Create the module to the class, if it already exist, return it
        /// </summary>
        /// <typeparam name="ModuleType"></typeparam>
        public ModuleType GetLazyModule<ModuleType>()
            where ModuleType : IShareableModule
        {
            var type = typeof(ModuleType);

            if (type.IsValueType && !type.IsEnum)
            {
                return StructGetLazyModule<ModuleType>();
            }

            var existingBindingIndex = 0;
            var hasModule = HasModule<ModuleType>(ref existingBindingIndex);
            if (hasModule)
            {
                return (ModuleType)SharedModules[existingBindingIndex];
            }

            var instance = (type.IsSubclassOf(typeof(MonoBehaviour))
                ? (ModuleType) (object) Wrapper.gameObject.AddComponent(type)
                : Activator.CreateInstance<ModuleType>());

            _addModule(instance);
            return instance;
        }

        /// <summary>
        /// (Struct version) Create the module to the class, if it already exist, return it
        /// </summary>
        /// <typeparam name="ModuleType"></typeparam>
        private ModuleType StructGetLazyModule<ModuleType>()
            where ModuleType : IShareableModule //< we can't put the struct constraint or else it wouldn't compile
        {
            var index = 0;
            var bindingFound = HasModule<ModuleType>(ref index);
            if (bindingFound)
                return (ModuleType) SharedModules[index];

            var instance = default(ModuleType);
            _addModule(instance);
            return instance;
        }

        public ModuleType SetModule<ModuleType>(ModuleType instance)
            where ModuleType : IShareableModule
        {
            var type = typeof(ModuleType);
            var instanceType = instance.GetType();

            if (type != instanceType)
            {
                Debug.LogError("ERROR");
                return instance;
            }

            if (instance is MonoBehaviour)
            {
                var instancedBehaviour = (MonoBehaviour)(object) instance;
                if (instancedBehaviour.gameObject != Wrapper.gameObject)
                    throw new Exception("Adding a module from another gameobject, this is not accepted.");
            }
            
            // first, check if we have a module of same type
            IShareableModule moduleToRemove = default(IShareableModule);
            var hadExistingModule = false;
            foreach (var module in SharedModules)
            {
                if (module.GetType() == type)
                {
                    moduleToRemove = module;
                    hadExistingModule = true;
                }
            }

            if (hadExistingModule)
            {
                _removeModuleButReplaceIt(moduleToRemove);
            }
            
            _addModule(instance);
            
            return instance;
        }
        #endregion
    }
}