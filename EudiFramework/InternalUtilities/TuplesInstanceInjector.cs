using System;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

namespace EudiFramework
{
    public static class TuplesInstanceInjector
    {
#if OLDCONCEPT
        public static object AddObjectToNativeArray(object originalObject, EudiMap map)
        {
            //Debug.LogException(new NotImplementedException("Waiting for NativeList<> :)"));
            /*var nativeArrayType = typeof(NativeArray<>).MakeGenericType(originalType);
            
            var obj = map.GetAssignedObject(nativeArrayType);
            if (obj != null)
            {
                var type = obj.GetType();
                
            }*/
            return null;
        }

        public static object AddObjectToDataArray(object originalObject, EudiMap map)
        {
            var originalObjectType = originalObject.GetType();
            if (!originalObjectType.IsValueType)
                return null;
            
            var dataArrayType = typeof(EudiModuleDataArray<>).MakeGenericType(originalObjectType);
            
            var obj = map.GetAssignedObject(dataArrayType);
            if (obj != null)
            {
                var castedObj = (IEudiModuleDataArray) obj;
                castedObj.AddModule(originalObject);
            }

            return null;
        }

        public static void ReplaceObjectToDataArray(object oldObject, object originalObject, EudiMap map)
        {
            var originalObjectType = originalObject.GetType();
            if (!originalObjectType.IsValueType)
                return;
            
            var dataArrayType = typeof(EudiModuleDataArray<>).MakeGenericType(originalObjectType);
            
            var obj = map.GetAssignedObject(dataArrayType);
            if (obj != null)
            {
                var castedObj = (IEudiModuleDataArray) obj;
                castedObj.ReplaceModule(oldObject, originalObject);
            }
        }

        public static void RemoveObjectFromDataArray(object originalObject, EudiMap map)
        {
            var originalObjectType = originalObject.GetType();
            if (!originalObjectType.IsValueType)
                return;
            
            var dataArrayType = typeof(EudiModuleDataArray<>).MakeGenericType(originalObjectType);
            
            var obj = map.GetAssignedObject(dataArrayType);
            if (obj != null)
            {
                var castedObj = (IEudiModuleDataArray) obj;
                if (castedObj.Contains(originalObject))
                    castedObj.RemoveModule(originalObject);
            }
        }
    #endif
        
        public static bool IsValid(Type originalType, EudiMap.TupleItem item)
        {
            if (item.Type.IsGenericType)
            {
                if (item.Type.GetGenericTypeDefinition() == typeof(EudiMatchArray<>))
                {
                    var newType = originalType;
                    if (typeof(WrapperModuleEntity).IsAssignableFrom(newType))
                    {
                        Type baseType = newType.BaseType;
                        while (baseType != null)
                        {
                            if (baseType.IsGenericType &&
                                baseType.GetGenericTypeDefinition() == typeof(WrapperModuleEntity<>))
                            {
                                newType = baseType.GetGenericArguments()[0];
                                break;
                            }

                            baseType = baseType.BaseType;

                            if (baseType == typeof(System.Object))
                            {
                                return false;
                            }
                        }
                    }

                    var dataArrayType = typeof(EudiMatchArray<>).MakeGenericType(newType);
                    return dataArrayType == item.Type;
                }
            }
            else
            {
                if (item.Type == typeof(TransformAccessArray)
                    && originalType == typeof(Transform))
                    return true;
            }

            return false;
        }
    }
}