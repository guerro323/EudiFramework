using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EudiFramework;
using UnityEngine;

namespace Assets.Exemple.Actors.CubeMovement
{
    /// <summary>
    /// We will do a heavy work just to rotate the object
    /// </summary>
    [Serializable]
    public class E1CubeMovementContractHeavyRotation : EudiComponentContract
    {
        public Quaternion Rotation;
    }
}
