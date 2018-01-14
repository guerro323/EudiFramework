using EudiFramework.Replica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.LowLevel;

namespace EudiFramework
{
    /// <summary>
    /// This class is only used to initiate the engine
    /// </summary>
    [DefaultExecutionOrder(-9999)]
    public class EudiInitiatorEngine : MonoBehaviour, IEudiInitiatorEngine
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            Eudi.UnitySynchronizationContext = SynchronizationContext.Current;
            Eudi.UnityTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            var go = new GameObject("#RuntimeInitiator");
            DontDestroyOnLoad(go);
            go.AddComponent<EudiInitiatorEngine>();

            var instance = Eudi.Globals.Bind<IEudiGameObjectManager, EudiGameObjectManager>();
            instance.transform.SetParent(go.transform);
            
            // Search for all systems and initialize them
            var systems = GameObject.FindObjectsOfType<EudiSystemBehaviour>();
            foreach (var system in systems)
            {
                system.InternalSystemAwake();
            }
        }

        public void Awake()
        {
            Eudi.Globals = new EudiConfiguration<object>();
            Eudi.EntitiesManager = new EudiEntitiesManager();
            Eudi.Components = new EudiComponents();
        }

        public void LateUpdate()
        {
            EudiReplicaTime.ForceUpdate();   
        }

        public void Update()
        {
            EudiReplicaTime.ForceUpdate();
        }

        public void FixedUpdate()
        {
            EudiReplicaTime.ForceUpdate();
        }

        public void OnApplicationQuit()
        {
            Eudi.ApplicationIsEnding = true;
            Threading.ThreadGroup.DestroyEverything();
        }
    }
}
