using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudiFramework.Threading
{
    public enum EudiSynchronizationType
    {
        /// <summary>
        /// Use the unity synchronization context
        /// </summary>
        Unity = 0,
        /// <summary>
        /// Use a custom multithreading context that don't use unity thread.
        /// </summary>
        TrueMultiThreading = 1
    }
}
