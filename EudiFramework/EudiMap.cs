using System;
using System.Collections.Generic;
using EudiFramework.Replica;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

namespace EudiFramework
{
    public class EudiMap
    {
        private class FakeRelationGenericWantedType
        {
            public Type WantedType;
        }

        public class TupleItem
        {
            public InjectTuplesAttribute Attribute;
            public Type Type;

            public bool Equals(TupleItem other, bool attributeCheck = true) => other.Type == Type;
        }

        public TupleItem[] TuplesItems;

        private Dictionary<FakeRelationGenericWantedType, object> assignedObjects =
            new Dictionary<FakeRelationGenericWantedType, object>();

        /// <summary>
        /// Does this map contains some types that are also contained in this one?
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public bool ContainsTypes(EudiMap map, bool attributeCheck = true)
        {
            if (map == null)
                throw new NullReferenceException(nameof(map) + " is null!");
            
            var itemsChecked = 0;
            var itemsLength = TuplesItems.Length;
            
            for (int i = 0; i < itemsLength; i++)
            {
                var item = TuplesItems[i];
                var otherItemsLength = map.TuplesItems.Length;
                for (int j = 0; j < otherItemsLength; j++)
                {
                    var otherItem = map.TuplesItems[j];
                    if (TuplesInstanceInjector.IsValid(otherItem.Type, item))
                        itemsChecked++;
                }
            }

            return itemsChecked >= itemsLength;
        }
        
        public bool Equals(EudiMap map, bool attributeCheck = true)
        {
            var itemsLength = TuplesItems.Length;

            if (map.TuplesItems.Length != itemsLength)
                return false;

            var equalTypesIn = 0;

            for (int i = 0; i < itemsLength; i++)
            {
                var item = TuplesItems[i];
                var otherItemsLength = map.TuplesItems.Length;
                for (int j = 0; j < otherItemsLength; j++)
                {
                    var otherItem = map.TuplesItems[j];
                    if (otherItem.Equals(item, attributeCheck))
                    {
                        equalTypesIn++;
                    }
                }
            }

            return equalTypesIn == itemsLength;
        }

        public object GetAssignedObject(Type wantedType)
        {
            foreach (var assignedObject in assignedObjects)
            {
                if (assignedObject.Key.WantedType == wantedType)
                    return assignedObject.Value;
            }

            return null;
        }

        internal void _createObjects()
        {
            // clean old objects
            foreach (var obj in assignedObjects.Values)
            {
                var disposable = obj as IDisposable;
                disposable?.Dispose();
            }
            
            // assign some objects
            foreach (var item in TuplesItems)
            {
                if (item.Type.IsGenericType)
                {
                    if (item.Type.GetGenericTypeDefinition() == typeof(NativeArray<>)
                    ) // Create default native array constructor
                    {

                    }

                    if (item.Type.GetGenericTypeDefinition() == typeof(EudiMatchArray<>))
                    {
                        var obj = assignedObjects[new FakeRelationGenericWantedType()
                        {
                            WantedType = item.Type,
                        }] = Activator.CreateInstance(item.Type);
                    }
                }

                if (item.Type == typeof(TransformAccessArray))
                {
                    assignedObjects[new FakeRelationGenericWantedType()
                    {
                        WantedType = item.Type,
                    }] = new TransformAccessArray(0);
                }
            }
        }

        public override bool Equals(object obj)
        {
            var a = obj as EudiMap;
            return a != null && Equals(a);
        }
    }
}