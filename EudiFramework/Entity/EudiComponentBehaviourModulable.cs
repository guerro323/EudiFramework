using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace EudiFramework
{
    [Serializable]
    public abstract class EudiComponentBehaviourModulable : EudiComponentBehaviour
    {
        public List<IModule> AttachedModules = new List<IModule>();
        public List<IShareableModule> SharedModules => EudiEntity.SharedModules;  
    }
    
    /// <summary>
    /// Represent a behaviour with the possibilites to access to his modules
    /// </summary>
    public class EudiComponentBehaviourModulable<TDependencyBase> : EudiComponentBehaviourModulable
        where TDependencyBase : IModule
    {
        /// <summary>
        /// Get the module
        /// </summary>
        /// <typeparam name="ModuleType">The interface that was used to bind the instance</typeparam>
        /// <returns>An instance that was binded to the interface</returns>
        public ModuleType GetModule<ModuleType>() where ModuleType : TDependencyBase
        {
            var type = typeof(ModuleType);
            var index = 0;
            
            if (HasModule<ModuleType>(ref index))
                return (ModuleType) AttachedModules[index];
            if (type.IsSubclassOf(typeof(MonoBehaviour)) && !(typeof(IShareableModule).IsAssignableFrom(type)))
            {
                ModuleType moduleToReturn;
                AttachedModules.Add(moduleToReturn = (ModuleType)(object)GetComponent(type));
                return moduleToReturn;
            }

            return default(ModuleType);
        }

        /// <summary>
        /// Return true if we have this module
        /// </summary>
        /// <typeparam name="ModuleType">The instance type</typeparam>
        /// <returns>Return true if we have the module requested</returns>
        /// <remarks>
        /// For some obvious reasons, we don't return the found type in case it's a struct...
        /// </remarks>
        public bool HasModule<ModuleType>(ref int index) where ModuleType : TDependencyBase
        {
            var type = typeof(ModuleType);
            var modulesLength = AttachedModules.Count;
            for (int i = 0; i < modulesLength; i++)
            {
                var module = AttachedModules[i];
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
            where ModuleType : TDependencyBase
        {
            var type = typeof(ModuleType);

            if (typeof(IShareableModule).IsAssignableFrom(type))
            {
                // REFLECTION STUFF!!!
                MethodInfo method = EudiEntity.GetType().GetMethod("GetLazyModule");
                MethodInfo generic = method.MakeGenericMethod(type);
                return (ModuleType)generic.Invoke(EudiEntity, null);
            }

            if (type.IsValueType && !type.IsEnum)
            {
                return StructGetLazyModule<ModuleType>();
            }

            var existingBinding = GetModule<ModuleType>();
            if (existingBinding != null)
                return existingBinding;

            var instance = (type.IsSubclassOf(typeof(MonoBehaviour))
                ? (ModuleType) (object) gameObject.AddComponent(type)
                : Activator.CreateInstance<ModuleType>());

            AttachedModules.Add(instance);
            return instance;
        }

        /// <summary>
        /// (Struct version) Create the module to the class, if it already exist, return it
        /// </summary>
        /// <typeparam name="ModuleType"></typeparam>
        private ModuleType StructGetLazyModule<ModuleType>()
            where ModuleType : TDependencyBase //< we can't put the struct constraint or else it wouldn't compile
        {
            var index = 0;
            var bindingFound = HasModule<ModuleType>(ref index);
            if (bindingFound)
                return (ModuleType) AttachedModules[index];

            var instance = default(ModuleType);
            AttachedModules.Add(instance);
            return instance;
        }

        public ModuleType SetModule<ModuleType>(ModuleType instance)
            where ModuleType : TDependencyBase
        {
            var type = typeof(ModuleType);
            var instanceType = instance.GetType();

            if (instance is MonoBehaviour)
            {
                var instancedBehaviour = (MonoBehaviour)(object) instance;
                if (instancedBehaviour.gameObject != gameObject)
                    throw new Exception("Adding a module from another gameobject, this is not accepted.");
            }
            
            if (!AttachedModules.Contains(instance))
                AttachedModules.Add(instance);
            
            return instance;
        }

        /// <summary>
        /// WIP METHOD (for now, it's just for removing the binding)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Dispose<ModuleType>() where ModuleType : TDependencyBase
        {
            var type = typeof(ModuleType);
            var index = 0;
            object myInstance;
            if (HasModule<ModuleType>(ref index))
            {
                var module = AttachedModules[index];
                
                UnityEngine.Debug.Log($"[Modules] instance({module.GetType().Name}) was removed from {gameObject.name}!");
                AttachedModules.Remove(module);

                // If it's a gameobject and we removed all binders, destroy the gameobject
                if (module is MonoBehaviour)
                {
                    Destroy(module as MonoBehaviour);
                }
            }
        }
    }
}