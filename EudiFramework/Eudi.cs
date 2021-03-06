using System.Threading;
using System.Threading.Tasks;

namespace EudiFramework
{
    public class Eudi
    {
        /// <summary>
        /// The configuration of Eudi
        /// </summary>
        /// <remarks>
        /// <see cref="Globals"/> is generated from <see cref="EudiInitiatorEngine"/> to keep the binded references alive.
        /// </remarks>
        public static EudiConfiguration<object> Globals { get; internal set; }

        /// <summary>
        /// The components manager of Eudi (WIP)
        /// </summary>
        public static EudiComponents Components { get; internal set; }

        /// <summary>
        /// The entities manager of Eudi (WIP)
        /// </summary>
        public static EudiEntitiesManager EntitiesManager { get; internal set; }

        /// <summary>
        /// Is the application ending?
        /// </summary>
        public static bool ApplicationIsEnding { get; internal set; }

        public static SynchronizationContext UnitySynchronizationContext { get; internal set; }
        public static TaskScheduler UnityTaskScheduler { get; internal set; }
    }
}