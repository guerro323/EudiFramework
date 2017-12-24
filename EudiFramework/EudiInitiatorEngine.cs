using EudiFramework.Replica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
            var go = new GameObject("#RuntimeInitiator");
            DontDestroyOnLoad(go);
            go.AddComponent<EudiInitiatorEngine>();

            var instance = Eudi.Globals.Bind<IEudiGameObjectManager, EudiGameObjectManager>();
            instance.transform.SetParent(go.transform);
        }

        public void Awake()
        {
            Eudi.Globals = new EudiConfiguration();
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
