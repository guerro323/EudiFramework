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
        public static EudiConfiguration Globals { get; internal set; }
        /// <summary>
        /// The components manager of Eudi (WIP)
        /// </summary>
        public static EudiComponents Components { get; internal set; }

        /// <summary>
        /// Is the application ending?
        /// </summary>
        public static bool ApplicationIsEnding { get; internal set; }
    }
}