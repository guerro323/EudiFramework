using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace EudiFramework
{
    /// <summary>
    /// A struct that can store some class data in a list! Isn't this beautiful?
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct EudiStructList<T> : IDisposable
    {
        private struct NullStruct
        {
            
        }
        
        private NullStruct _handleReference;
        private GCHandle _listHandle;
        
        // no native hashtable :(
        public int Length => _list.Count;
        private List<T> _list => GetList(_listHandle);

        public EudiStructList(int initialLength)
        {
            _handleReference = new NullStruct();
            _listHandle = CreateTemporaryList(_handleReference);
            _listHandle = SetList(_listHandle, new List<T>());
        }
        
        public bool Contains(T _objectChecked)
        {
            return _list.Contains(_objectChecked);
        }

        public void Replace(T oldObject, T newObject)
        {
            var modulesLength = _list.Count;

            var indexToReplace = -1;
            var equalityComparer = EqualityComparer<T>.Default;
            for (int i = 0; i < modulesLength; i++)
            {
                var item = _list[i];
                if (equalityComparer.Equals(item, oldObject))
                    indexToReplace = i;
            }

            if (indexToReplace != -1)
            {
                _list[indexToReplace] = newObject;
            }
        }

        public void Remove(T module)
        {
            _list.Remove(module);
        }

        public void Add(T module)
        {
            _list.Add(module);
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
        
        public T this[int index]
        {
            get { return _list[index]; }
            set { _list[index] = value; }
        }

        public static GCHandle CreateTemporaryList(object original)
        {
            return GCHandle.Alloc(original);
        }
        
        public static GCHandle SetList(GCHandle original, object array)
        {
            original.Target = array;
            return original;
        }

        public static List<T> GetList(GCHandle handle)
        {
            if (!handle.IsAllocated)
                throw new Exception("Handle isn't allocated!");
                
            return (List<T>)handle.Target;
        }

        /// <summary>
        /// Clear the list and dispose it with the class
        /// </summary> 
        public void Dispose()
        {
            _list.Clear();
            _listHandle.Free();
        }
    }
}