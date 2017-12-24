using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Exemple.Actors.CubeMovement
{
    /// <summary>
    /// I used a class to represent data instead of holding the variables into each classes.
    /// It would also be possible to use a struct, but it would require typing more code.
    /// The class got no suffix because it hold 'general' data.
    /// </summary>
    public class CubeMovementData
    {
        public float Offset;
    }
}
