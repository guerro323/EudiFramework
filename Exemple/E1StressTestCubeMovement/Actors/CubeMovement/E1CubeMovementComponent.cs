using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EudiFramework;
using UnityEngine;

namespace Assets.Exemple.Actors.CubeMovement
{
    public class E1CubeMovementComponent : EudiComponentBehaviour
    {
        [EudiFieldContract]
        public E1CubeMovementContractPositionScale ContractPositionScale;

        [EudiFieldContract]
        public E1CubeMovementContractHeavyRotation ContractRotation;

        public E1CubeMovementData Data = new E1CubeMovementData();

        protected override void UnityAwake()
        {
            AddWorker(new E1CubeMovementWorker(out ContractPositionScale, out ContractRotation, ref Data));
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
