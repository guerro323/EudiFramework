using System;
using UnityEngine;

namespace EudiFramework.Replica
{
    /// <summary>
    /// Thread Safe variant of original unity <see cref="UnityEngine.Time"/> class
    /// </summary>
    public static class EudiReplicaTime
    {
        public static float realtimeSinceStartup { get; private set; }
        public static int renderedFrameCount { get; private set; }
        public static int frameCount { get; private set; }
        public static float timeScale { get; private set; }
        public static float smoothDeltaTime { get; private set; }
        public static float maximumDeltaTime { get; private set; }
        public static int captureFramerate { get; private set; }
        public static float fixedDeltaTime { get; private set; }
        public static float unscaledDeltaTime { get; private set; }
        public static float fixedUnscaledTime { get; private set; }
        public static float unscaledTime { get; private set; }
        public static float fixedTime { get; private set; }
        public static float deltaTime { get; private set; }
        public static float timeSinceLevelLoad { get; private set; }
        public static float time { get; private set; }
        public static float fixedUnscaledDeltaTime { get; private set; }
        public static bool inFixedTimeStep { get; private set; }

        internal static void ForceUpdate()
        {
            realtimeSinceStartup = Time.realtimeSinceStartup;
            renderedFrameCount  = Time.renderedFrameCount;
            frameCount = Time.frameCount;
            timeScale = Time.timeScale;
            smoothDeltaTime = Time.smoothDeltaTime;
            maximumDeltaTime = Time.maximumDeltaTime;
            captureFramerate = Time.captureFramerate;
            fixedDeltaTime = Time.fixedDeltaTime;
            unscaledDeltaTime = Time.unscaledDeltaTime;
            fixedUnscaledTime = Time.fixedUnscaledTime;
            fixedTime = Time.fixedTime;
            deltaTime = Time.deltaTime;
            timeSinceLevelLoad = Time.timeSinceLevelLoad;
            time = Time.time;
            fixedUnscaledDeltaTime = Time.fixedUnscaledDeltaTime;
            inFixedTimeStep = Time.inFixedTimeStep;
        }
    }
}
