using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;

namespace EudiFramework
{
    public interface IEudiModuleDataArray
    {
#if OLDCONCEPT
        void ReplaceModule(object oldModule, object newModule);
        void RemoveModule(object module);
        void AddModule(object module);
        bool Contains(object module);
#endif
    }

    public struct EudiMatchArray<TModule> : IEudiModuleDataArray
    {
#if OLDCONCEPT
        private struct NullStruct
        {
            
        }
        
        private NullStruct _handleReference;
        private GCHandle _modulesHandle;
        
        // no native hashtable :(
        public int Length => _modules.Count;
        public List<TModule> _modules => GetModuleArray<TModule>(_modulesHandle);

        public EudiModuleDataArray(int initialLength)
        {
            _handleReference = new NullStruct();
            _modulesHandle = CreateTemporaryModuleArray(_handleReference);
            _modulesHandle = SetModuleArray(_modulesHandle, new List<TModule>());
        }
        
        internal void ReplaceModule(TModule oldModule, TModule newModule)
        {
            var oldObj = (object) oldModule;
            var newObj = (object) newModule;
            ((IEudiModuleDataArray)this).ReplaceModule(oldModule, newModule);
        }

        internal void RemoveModule(TModule module)
        {
            var obj = (object) module;
            ((IEudiModuleDataArray)this).RemoveModule(obj);
        }

        internal void AddModule(TModule module)
        {
            var obj = (object)module;
            ((IEudiModuleDataArray)this).AddModule(obj);
        }

        bool IEudiModuleDataArray.Contains(object _moduleChecked)
        {
            /*var moduleChecked = (TModule)_moduleChecked;   
            for (int i = 0; i < _modules.Count; i++)
            {
                var module = _modules[i];
                if (module.Equals(moduleChecked))
                    return true;
            }

            return false;*/
            return _modules.Contains((TModule)_moduleChecked);
        }

        void IEudiModuleDataArray.ReplaceModule(object oldModule, object newModule)
        {
            var modulesLength = _modules.Count;

            var indexToReplace = -1;
            var equalityComparer = EqualityComparer<TModule>.Default;
            for (int i = 0; i < modulesLength; i++)
            {
                var item = _modules[i];
                if (equalityComparer.Equals((TModule) item, (TModule) oldModule))
                    indexToReplace = i;
            }

            if (indexToReplace != -1)
            {
                _modules[indexToReplace] = (TModule) newModule;
            }
        }

        void IEudiModuleDataArray.RemoveModule(object module)
        {
            _modules.Remove((TModule) module);
        }

        void IEudiModuleDataArray.AddModule(object module)
        {
            /*var index = _modules.Length;
            
            ModifyModuleArrayLength(index + 1);

            var modules = _modules;
            modules[index] = (TModule)module;
            _modulesHandle = SetModuleArray(_modulesHandle, modules);*/
            _modules.Add((TModule)module);
            Debug.Log(module.GetType().Name);
            Debug.Log(_modules.Count);
        }

        /*private void ModifyModuleArrayLength(int length)
        {
            var tempArray = new NativeArray<TModule>(length, Allocator.Persistent);

            var modules = _modules;
            _copy(ref modules, ref tempArray); //< NativeArray<>.CopyTo() is a bit buggy
            
            _modulesHandle = SetModuleArray(_modulesHandle, modules = new NativeArray<TModule>(tempArray, Allocator.Persistent));
        }

        private void _copy(ref NativeArray<TModule> src, ref NativeArray<TModule> dst)
        {
            if (src.Length > dst.Length)
                throw new ArgumentException("array.Length does not match the length of this instance");
            
            var length = src.Length;
            
            for (int i = 0; i < length; i++)
            {
                dst[i] = src[i];
            }
        }*/
        
        public TModule this[int index]
        {
            get { return _modules[index]; }
            set
            {
                var modules = _modules;
                modules[index] = value;
                _modulesHandle = SetModuleArray(_modulesHandle, modules);
            }
        }

        public static GCHandle CreateTemporaryModuleArray(object original)
        {
            return GCHandle.Alloc(original);
        }
        
        public static GCHandle SetModuleArray(GCHandle original, object array)
        {
            original.Target = array;
            return original;
        }

        public static List<T> GetModuleArray<T>(GCHandle handle) where T : struct
        {
            if (!handle.IsAllocated)
                throw new Exception("Handle isn't allocated!");
                
            return (List<T>)handle.Target;
        }
#endif
    }
}