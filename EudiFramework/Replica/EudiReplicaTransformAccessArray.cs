using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Bindings;

namespace EudiFramework.Replica
{
    public struct EudiReplicaTransformAccess
    {
        internal static Dictionary<int, Transform> AllTransforms = new Dictionary<int, Transform>();
        
        internal int transformInstanceId;

        public Vector3 position;
        public Vector3 localPosition;
        public Vector3 localScale;
        public Quaternion rotation;
    }
    
  /// <summary>
  /// Temporate class for 2018.1.2b (actual <see cref="TransformAccessArray"/> is crashing everytime :( )
  /// </summary>
  public struct EudiReplicaTransformAccessArray
  {
      private GCHandle _transformArrayHandle;
      
      // no native hashtable :(
      public int Length => _transforms.Length;
      private NativeArray<EudiReplicaTransformAccess> _transforms;

      public EudiReplicaTransformAccessArray(int initialLength = 0)
      {
          _transformArrayHandle = new GCHandle();
          
          _transforms = new NativeArray<EudiReplicaTransformAccess>(0, Allocator.Persistent);
          _transformArrayHandle = GCHandle.Alloc(this);
      }

      /// <summary>
      /// Add.
      /// </summary>
      public void Add(Transform transform)
      {
          var index = _transforms.Length;
          var transformAccess = new EudiReplicaTransformAccess()
          {
              transformInstanceId = transform.GetInstanceID()
          };

          EudiReplicaTransformAccess.AllTransforms[transformAccess.transformInstanceId] = transform;
          
          ModifyModuleArrayLength(index + 1);
          _transforms[index] = transformAccess;
          
          
      }

      public IntPtr GetTransformAccessArrayForSchedule()
      {
          return GCHandle.ToIntPtr(_transformArrayHandle);
      }

      private void ModifyModuleArrayLength(int length)
      {
          var tempArray = new NativeArray<EudiReplicaTransformAccess>(length, Allocator.Persistent);

          _copy(ref _transforms, ref tempArray); //< NativeArray<>.CopyTo() is a bit buggy
          var oldReference = _transforms;

          if (_transforms.IsCreated)
              _transforms.Dispose();
          
          _transforms = tempArray;
      }

      private void _copy(ref NativeArray<EudiReplicaTransformAccess> src, ref NativeArray<EudiReplicaTransformAccess> dst)
      {
          if (src.Length > dst.Length)
              throw new ArgumentException("array.Length does not match the length of this instance");
          
          var length = src.Length;
          
          for (int i = 0; i < length; i++)
          {
              dst[i] = src[i];
          }
      }
      
      public EudiReplicaTransformAccess this[int index]
      {
          get { return _transforms[index]; }
          set { _transforms[index] = value; }
      }

      public Transform GetTransform(int index)
      {
          return EudiReplicaTransformAccess.AllTransforms[_transforms[index].transformInstanceId];
      }
  }
}