using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EudiFramework;
using UnityEngine;

namespace Assets.Exemple.Actors.CubeMovement
{
    [Serializable]
    public class E1CubeMovementContractPositionScale : EudiComponentContract
    {
        public Vector3 Position;
        public Vector3 Scale;
    }
}
