using UnityEngine;
using EudiFramework;
using Assets.Exemple.Actors.CubeMovement;
using System.Collections.Generic;
using System.Collections;
using Assets.Exemple.MonoBehaviours;
using EudiFramework.Threading;

public class E1CubeMovementOnStart : MonoBehaviour
{
    public static int HeavyWorkIteration => singleton.HeavyRotationIteration;
    public static EudiSynchronizationType SyncType => singleton.SynchronizationType;

    private static E1CubeMovementOnStart singleton;

    public enum TestType
    {
        EudiActorBehaviour,
        UnityStandardMonoBehaviour
    }

    public GameObject OriginalCube;
    [Header("Properties")]
    public int Cubes = 10000;
    public int HeavyRotationIteration = 1000;
    public EudiSynchronizationType SynchronizationType = EudiSynchronizationType.TrueMultiThreading;
    private List<GameObject> CubesToBind = new List<GameObject>();

    public TestType BehaviourTestType = TestType.EudiActorBehaviour;

    private IEnumerator Start()
    {
        singleton = this;

        if (BehaviourTestType == TestType.EudiActorBehaviour)
        {
            Eudi.Globals.Bind<ISuperLogger, MyCustomLogger>();
            Eudi.Globals.Bind<MonoBehaviour, MyCustomLogger>(); //< You shouldn't try that at home.
            Eudi.Globals.Bind<GameObject, MyCustomLogger>(); //< same; (Instead, you should only bind to the main interface of the class)

            for (int i = 0; i < Cubes; i++)
            {
                var newGo = Instantiate(OriginalCube);
                newGo.AddComponent<E1CubeMovementComponent>().Data.Offset = i * 0.1f;
                CubesToBind.Add(newGo);

                if (i % 100 == 1)
                    yield return null;
            }

            Eudi.Globals.Dispose<ISuperLogger>(); //output: [IoC] instance(MyCustomLogger) from ISuperLogger was disposed!
            Eudi.Globals.DisposeFromBinders<MyCustomLogger>(); //output: [IoC] instance(MyCustomLogger) from MonoBehaviour/GameObject was disposed!

            yield return null;

            System.GC.Collect(); //< collect the garbage generated from MyCustomLogger
            //^ output one time: MyCustomLogger was destroyed from the GC
        }
        else
        {
            new GameObject("MonobehaviourManager").AddComponent<E1MonoBehaviourManager>();

            for (int i = 0; i < Cubes; i++)
            {
                var newGo = Instantiate(OriginalCube);
                newGo.AddComponent<E1CubeMovementMonoBehaviour>().Offset = i * 0.1f;
                CubesToBind.Add(newGo);

                if (i % 100 == 1)
                    yield return null;
            }
        }
    }
}