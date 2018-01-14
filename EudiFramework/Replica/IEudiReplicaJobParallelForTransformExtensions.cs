using System;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine.Jobs;

namespace EudiFramework.Replica
{ 
  /// <summary>
  ///   <para>Extension methods for IJobParallelForTransform.</para>
  /// </summary>
  public static class IEudiReplicaJobParallelForTransformExtensions
  {
    public static JobHandle Schedule<T>(this T jobData, EudiReplicaTransformAccessArray transforms, JobHandle dependsOn = default (JobHandle)) where T : struct, IEudiReplicaJobParallelForTransform
    {
      JobsUtility.JobScheduleParameters parameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), TransformParallelForLoopStruct<T>.Initialize(), dependsOn, ScheduleMode.Batched);
      return JobsUtility.ScheduleParallelForTransform(ref parameters, transforms.GetTransformAccessArrayForSchedule());
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    private struct TransformParallelForLoopStruct<T> where T : struct, IEudiReplicaJobParallelForTransform
    {
      private static ExecuteJobFunction _jobCache;
      public static IntPtr jobReflectionData;

      public static IntPtr Initialize()
      {
        if (TransformParallelForLoopStruct<T>.jobReflectionData == IntPtr.Zero)
        {
          System.Type type = typeof (T);
          // ISSUE: reference to a compiler-generated field
          if (_jobCache == null)
          {
            // ISSUE: reference to a compiler-generated field
            _jobCache= new ExecuteJobFunction(Execute);
          }
          // ISSUE: reference to a compiler-generated field
          ExecuteJobFunction fMgCache0 = _jobCache;
          jobReflectionData = JobsUtility.CreateJobReflectionData(type, (object) fMgCache0);
        }
        return jobReflectionData;
      }

      public static unsafe void Execute(ref T jobData, IntPtr jobData2, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex)
      {
        /*IntPtr output;
        UnsafeUtility.CopyPtrToStructure<IntPtr>(jobData2, out output);
        int* sortedToUserIndex = (int*) (void*) EudiReplicaTransformAccessArray.GetSortedToUserIndex(output);
        TransformAccess* sortedTransformAccess = (TransformAccess*) (void*) EudiReplicaTransformAccessArray.GetSortedTransformAccess(output);*/
        
        IntPtr output;
        UnsafeUtility.CopyPtrToStructure<IntPtr>(jobData2, out output);
        var transformArray = (EudiReplicaTransformAccessArray)GCHandle.FromIntPtr(output).Target;
        
        int beginIndex;
        int endIndex;
        JobsUtility.GetJobRange(ref ranges, jobIndex, out beginIndex, out endIndex);
        for (int index1 = beginIndex; index1 < endIndex; ++index1)
        {
          int index2 = index1;
          //int num = sortedToUserIndex[index2];
          //JobsUtility.PatchBufferMinMaxRanges(bufferRangePatchData, UnsafeUtility.AddressOf<T>(ref jobData), num, 1);
          jobData.Execute(index2, transformArray[index2]);
        }
      }

      public delegate void ExecuteJobFunction(ref T jobData, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex);
    }
  }
}