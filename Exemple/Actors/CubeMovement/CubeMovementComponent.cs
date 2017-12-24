using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EudiFramework;
using UnityEngine;

namespace Assets.Exemple.Actors.CubeMovement
{
    public class CubeMovementComponent : EudiComponentBehaviour
    {
        [EudiFieldContract]
        public CubeMovementContractPositionScale ContractPositionScale;

        [EudiFieldContract]
        public CubeMovementContractHeavyRotation ContractRotation;

        public CubeMovementData Data = new CubeMovementData();

        protected override void UnityAwake()
        {
            AddWorker(new CubeMovementWorker(out ContractPositionScale, out ContractRotation, ref Data));
        }

        protected override void UnityUpdate()
        {
            transform.position = ContractPositionScale.Position;
            transform.localScale = ContractPositionScale.Scale;
            if (ContractRotation.IsLocked)
            {
                transform.rotation = ContractRotation.Rotation;

                ContractRotation.Unlock();
            }
        }
    }
}
