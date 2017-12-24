using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EudiFramework;
using EudiFramework.Replica;
using EudiFramework.Threading;
using UnityEngine;

namespace Assets.Exemple.Actors.CubeMovement
{
    /// <summary>
    /// A worker that will move the gameObject like a pingpong ball
    /// </summary>
    public class E1CubeMovementWorker : EudiComponentWorker
    {
        [EudiFieldContract]
        public E1CubeMovementContractPositionScale ContractPositionScale;

        [EudiFieldContract]
        public E1CubeMovementContractHeavyRotation ContractRotation;
        private float m_yRot;
        private Vector3 m_eulerRotCached;

        public E1CubeMovementData Data;

        public E1CubeMovementWorker(out E1CubeMovementContractPositionScale contractPosition,
            out E1CubeMovementContractHeavyRotation contractRotation,
            ref E1CubeMovementData data)
        {
            contractPosition = ContractPositionScale = new E1CubeMovementContractPositionScale();
            contractRotation = ContractRotation = new E1CubeMovementContractHeavyRotation();
            Data = data;

            SetThreadCount(2);
            SetThreadShareParam(0, EudiThreading.GetThreadGroup<E1CubeMovementWorker>());
            SetThreadShareParam(1, EudiThreading.GetThreadGroup<E1CubeMovementWorker>(1 + Time.frameCount % 2));
        }

        protected override void WorkerUpdate(EudiWorkerUpdateEvent ev)
        {
            if (ev.workId == 0) //< Work on the main worker thread
            {
                // Position...
                ContractPositionScale.Position.x = Mathf.PingPong(EudiReplicaTime.time, 5) - 1000 + Data.Offset * 5;
                ContractPositionScale.Position.y = Data.Offset * 0.08f;

                // Scale...
                ContractPositionScale.Scale.x = 0.25f + (Mathf.PingPong((EudiReplicaTime.time + Data.Offset) * 2, 5) * 0.25f);
                ContractPositionScale.Scale.y = ContractPositionScale.Scale.x;
                ContractPositionScale.Scale.z = ContractPositionScale.Scale.x;
            }

            if (ev.workId == 1) //< Work on another thread
            {
                if (!ContractRotation.IsLocked)
                {
                    for (var i = 0; i < E1CubeMovementOnStart.HeavyWorkIteration; i++) //< heavy work right here
                    {
                        m_yRot += (ev.WorkerTask.ReplicaTime.TimeDelta * 0.064f) + Data.Offset * 0.0001f;
                    }

                    m_eulerRotCached.y = m_yRot;
                    ContractRotation.Rotation.eulerAngles = m_eulerRotCached;

                    ContractRotation.Lock();
                }
            }
        }
    }
}
