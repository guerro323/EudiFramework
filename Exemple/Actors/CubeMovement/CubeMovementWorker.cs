using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EudiFramework;
using EudiFramework.Replica;
using UnityEngine;

namespace Assets.Exemple.Actors.CubeMovement
{
    /// <summary>
    /// A worker that will move the gameObject like a pingpong ball
    /// </summary>
    public class CubeMovementWorker : EudiComponentWorker
    {
        [EudiFieldContract]
        public CubeMovementContractPositionScale ContractPositionScale;

        [EudiFieldContract]
        public CubeMovementContractHeavyRotation ContractRotation;
        private float m_yRot;
        private Vector3 m_eulerRotCached;

        public CubeMovementData Data;

        public CubeMovementWorker(out CubeMovementContractPositionScale contractPosition,
            out CubeMovementContractHeavyRotation contractRotation,
            ref CubeMovementData data)
        {
            contractPosition = ContractPositionScale = new CubeMovementContractPositionScale();
            contractRotation = ContractRotation = new CubeMovementContractHeavyRotation();
            Data = data;

            SetThreadCount(2);
            SetThreadShareParam(0, EudiThreading.GetThreadGroup<CubeMovementWorker>());
            SetThreadShareParam(1, EudiThreading.GetThreadGroup<CubeMovementWorker>(1 + Time.frameCount % 2));
        }

        protected override void WorkerUpdate(int threadId)
        {
            if (threadId == 0) //< Work on the main worker thread
            {
                // Position...
                ContractPositionScale.Position.x = Mathf.PingPong(EudiReplicaTime.time, 5) - 1000 + Data.Offset * 5;
                ContractPositionScale.Position.y = Data.Offset * 0.08f;

                // Scale...
                ContractPositionScale.Scale.x = 0.25f + (Mathf.PingPong((EudiReplicaTime.time + Data.Offset) * 2, 5) * 0.25f);
                ContractPositionScale.Scale.y = ContractPositionScale.Scale.x;
                ContractPositionScale.Scale.z = ContractPositionScale.Scale.x;
            }

            if (threadId == 1) //< Work on another thread
            {
                if (!ContractRotation.IsLocked)
                {
                    for (var i = 0; i < OnStart.HeavyWorkIteration; i++) //< heavy work right here
                    {
                        m_yRot += (EudiReplicaTime.deltaTime * 0.008f) + Data.Offset * 0.0001f;
                    }

                    m_eulerRotCached.y = m_yRot;
                    ContractRotation.Rotation.eulerAngles = m_eulerRotCached;

                    ContractRotation.Lock();
                }
            }
        }
    }
}
