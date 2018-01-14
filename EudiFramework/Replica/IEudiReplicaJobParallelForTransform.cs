using Unity.Jobs;
using UnityEngine.Jobs;

namespace EudiFramework.Replica
{
    public interface IEudiReplicaJobParallelForTransform
    {
        /// <summary>
        ///   <para>Execute.</para>
        /// </summary>
        /// <param name="index">Index.</param>
        /// <param name="transform">TransformAccessArray.</param>
        void Execute(int index, EudiReplicaTransformAccess transform);
    }
}