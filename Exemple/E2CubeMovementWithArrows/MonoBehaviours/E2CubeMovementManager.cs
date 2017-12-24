using System.Collections.Generic;
using UnityEngine;


public class E2CubeMovementManager : MonoBehaviour
{
    public static List<E2CubeMovementBehaviour> BehavioursToUpdate = new List<E2CubeMovementBehaviour>();

    private void Update()
    {
        var listLength = BehavioursToUpdate.Count;
        for (int i = 0; i < BehavioursToUpdate.Count; i++)
        {
            var item = BehavioursToUpdate[i];
            item.OnUpdate();
        }
    }
}
