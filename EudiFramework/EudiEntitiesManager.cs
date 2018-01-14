using System;
using System.Collections.Generic;
using EudiFramework.Replica;
using UnityEngine;
using UnityEngine.Jobs;
using Object = UnityEngine.Object;

namespace EudiFramework
{
    public class EudiEntitiesManager
    {
        public class EudiMapManager
        {
            public Dictionary<EudiMap, EudiStructList<EudiEntity>> AllMaps =
                new Dictionary<EudiMap, EudiStructList<EudiEntity>>();
 
            public bool Contains(EudiMap mapToCheck) => SearchExisting(mapToCheck) != null;
            
            public EudiMap SearchExisting(EudiMap mapToCheck)
            {
                foreach (var againstMap in AllMaps.Keys)
                {
                    if (againstMap.Equals(mapToCheck))
                        return againstMap;
                }

                return null;
            }

            public EudiMap GetMap(EudiMap.TupleItem[] items)
            {
                var map = new EudiMap { TuplesItems = items };
                var existingMap = SearchExisting(map);
                if (existingMap != null)
                    return existingMap;

                map._createObjects();

                AllMaps.Add(map, new EudiStructList<EudiEntity>(0));
                return map;
            }

            public EudiStructList<EudiEntity> GetEntities(EudiMap map)
            {
                var otherMap = SearchExisting(map);
                if (otherMap == null)
                    throw new Exception("Couldn't find the map!");

                return AllMaps[map];
            }
        }
        
        public readonly Dictionary<long, EudiEntity> AllEntities = new Dictionary<long, EudiEntity>();

        public EudiMapManager MapManager => Eudi.Globals.Bind<EudiMapManager, EudiMapManager>();
        
        public EudiEntity GetEntity(long id)
        {
            return AllEntities[id];
        }

        public EudiEntity GetEntity(IModule module)
        {
            var entitiesCount = AllEntities.Count;
            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = AllEntities[i];
                if (entity.ContainsModule(module))
                    return entity;
            }

            return null;
        }

        internal void _initComponent(GameObject go, EudiComponentBehaviour component)
        {
            var wrapper = component.GetComponent<WrapperEudiEntity>();
            component.EudiEntityId = wrapper.Instance.Id;
            component.EudiEntity = wrapper.Instance;

            if (!wrapper.Instance.EudiComponents.Contains(component))
                wrapper.Instance.EudiComponents.Add(component);

            wrapper.Instance._updateMap();

            // Key is Map, Value is Entity
            foreach (var mapEntities in MapManager.AllMaps)
            {
                if (!_isValidForMapping(go, mapEntities.Key))
                    continue;

                var entitiesList = mapEntities.Value;
                if (!entitiesList.Contains(wrapper.Instance))
                {
                    entitiesList.Add(wrapper.Instance);
                    
                    var obj = mapEntities.Key.GetAssignedObject(typeof(TransformAccessArray));
                    if (obj != null)
                    {                                     
                        var castedObj = (TransformAccessArray)obj;
                        castedObj.Capacity += 1;
                        castedObj.Add(go.transform);
                    }
                }

                // Add the objects
                /* DEPRECATED: We use a shared map now on each entity
                TuplesInstanceInjector.AddObjectToNativeArray(component, map);
                TuplesInstanceInjector.AddObjectToDataArray(component, map);*/

            }
        }

        internal void _initModule(GameObject go, IShareableModule module)
        {
            var wrapper = go.GetComponent<WrapperEudiEntity>();
            wrapper.Instance._updateMap();
            
            // Key is Map, Value is Entity
            foreach (var mapEntities in MapManager.AllMaps)
            {
                if (!_isValidForMapping(go, mapEntities.Key))
                    continue;
                
                var entitiesList = mapEntities.Value;
                if (!entitiesList.Contains(wrapper.Instance))
                {
                    entitiesList.Add(wrapper.Instance);

                    var obj = mapEntities.Key.GetAssignedObject(typeof(TransformAccessArray));
                    if (obj != null)
                    {
                        var castedObj = (TransformAccessArray) obj;
                        castedObj.Capacity += 1;
                        castedObj.Add(go.transform);
                    }
                }
            }            
        }

        internal void _addEntity(GameObject go)
        {
            var wrapper = go.AddComponent<WrapperEudiEntity>();
            AllEntities[wrapper.Instance.Id] = wrapper.Instance;
        }

        internal void _removeEntity(GameObject go)
        {
            var wrapper = go.GetComponent<WrapperEudiEntity>();
            if (wrapper.Instance.SharedModules.Count > 0)
                return;
            
            // Remove the transform access array
            // Key is Map, Value is Entity
            foreach (var mapEntities in MapManager.AllMaps)
            {
                var obj = mapEntities.Key.GetAssignedObject(typeof(TransformAccessArray));
                if (obj != null)
                {
                    var castedObj = (TransformAccessArray) obj;
                    var indexOfTransform = -1;
                    for (int i = 0; i < castedObj.Length; i++)
                    {
                        if (castedObj[i] == go.transform)
                            indexOfTransform = i;
                    }

                    /* TODO: Remove transform from array, if (indexOfTransform != -1)
                        castedObj.RemoveAtSwapBack(indexOfTransform);*/
                }

                foreach (var map in MapManager.AllMaps)
                {
                    if (map.Value.Contains(wrapper.Instance))
                        map.Value.Remove(wrapper.Instance);
                }
                
            }
        }

        internal bool _isValidForMapping(GameObject go, EudiMap map)
        {
            var entity = go.GetComponent<WrapperEudiEntity>().Instance;
            var entityMap = entity.ComponentMap;
            
            /*var validCount = 0;
            
            // we have a transform, so add it
            validCount++;
            
            foreach (var item in map.TuplesItems)
            {
                if (entity == null)
                {
                    if (typeof(Component).IsAssignableFrom(item.Type)
                        && go.GetComponent(item.Type) != null)
                        validCount++;
                }
                else
                {
                    if (typeof(Component).IsAssignableFrom(item.Type)
                        && entity.GetComponent(item.Type) != null)
                        validCount++;
                    foreach (var module in entity.SharedModules)
                    {
                        if (TuplesInstanceInjector.IsValid(module, item))
                            validCount++;
                    }
                }
            }

            return validCount == map.TuplesItems.Length;*/
            var isValid = map.ContainsTypes(entityMap, false);
            return isValid;
        }
    }
}