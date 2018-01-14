using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace EudiFramework
{
    public abstract class EudiSystemBehaviour : EudiComponentBehaviour
    {         
        private int _arrayLength;
        public EudiMap _linkedMap;

        public EudiStructList<EudiEntity> MatchEntities = new EudiStructList<EudiEntity>(0);
        public int ArrayLength => _arrayLength;

        private JobHandle _lastJobHandle;

        protected internal override void InternalSystemAwake()
        {
            List<EudiMap.TupleItem> tempItems = new List<EudiMap.TupleItem>();
            List<FieldInfo> tempCorrectFields = new List<FieldInfo>();
            
            var type = GetType();
            var fields = type.GetFields();
            var fieldsCount = fields.Length;
            for (int i = 0; i < fieldsCount; i++)
            {
                var field = fields[i];
                var hasTupleAttribute = Attribute.IsDefined(field, typeof(InjectTuplesAttribute));
                if (hasTupleAttribute)
                {                    
                    var item = new EudiMap.TupleItem();
                    item.Attribute = (InjectTuplesAttribute)Attribute.GetCustomAttribute(field, typeof(InjectTuplesAttribute));
                    item.Type = field.FieldType;
                    tempItems.Add(item);
                    tempCorrectFields.Add(field);
                }
            }
            
            _linkedMap = Eudi.EntitiesManager.MapManager.GetMap(tempItems.ToArray());
            MatchEntities = Eudi.EntitiesManager.MapManager.GetEntities(_linkedMap);

            // Set fields value from the map
            foreach (var field in tempCorrectFields)
            {
                var isGeneric = field.FieldType.IsGenericType;
                var wantedType = field.FieldType;

                var obj = _linkedMap.GetAssignedObject(wantedType);
                if (obj == null)
                    Debug.LogError(new Exception($"Field {field.Name} (c:{field.DeclaringType?.Name}) was set to null!"));
                field.SetValue(this, obj);
            }

            if (_linkedMap == null || tempItems.Count == 0)
            {
                Debug.LogWarning($"!!! System {GetType().FullName} got no `InjectTuple` attribute.");
                tempItems?.Clear();
                tempItems = null; //< remove reference
            }
        }

        public virtual void OnEntityReceiveComponent(int entity)
        {
            
        }

        public virtual void OnEntityDeletedComponent(int entity)
        {
            
        }
        
        // -------- -------- -------- -------- -------- -------- -------- -------- 
        // Some supports for jobs (even if we have already multithreading on Eudi!)
        // -------- -------- -------- -------- -------- -------- -------- --------

        public JobHandle GetDependency() => _lastJobHandle;
        public void AddDependency(JobHandle jobHandle) => _lastJobHandle = jobHandle;
        
        internal bool IsInstanceOfGenericType(Type genericType, object instance)
        {
            Type type = instance.GetType();
            while (type != null)
            {
                if (type.IsGenericType &&
                    type.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }
                type = type.BaseType;
            }
            return false;
        }

        /*private void _addEntity(EudiEntity entity, List<AdvancedFieldInfo> tuplesComponents, AdvancedFieldInfo transformAccessArray)
        {
            var mapManager = Eudi.EntitiesManager.MapManager;
            
            
            // Do our tuples got any reference to a component?
            if (tuplesComponents.Count > 0)
            {
                var components = entity.Components;
                var componentsLength = components.Count;
                for (int j = 0; j < componentsLength; j++)
                {
                    var component = components[j];
                    var tuplesLength = tuplesComponents.Count;
                    for (int k = 0; k < tuplesLength; k++)
                    {
                        
                    }
                }
            }
        }

        private void _addComponentFromEntity(AdvancedFieldInfo tupleComponent, Component component)
        {
            
        }*/
    }
}