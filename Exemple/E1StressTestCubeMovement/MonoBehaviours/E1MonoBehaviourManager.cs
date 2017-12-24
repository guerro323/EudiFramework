using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Exemple.MonoBehaviours
{
    /// <summary>
    /// Manager to hold the update functions
    /// </summary>
    public class E1MonoBehaviourManager : MonoBehaviour
    {
        public static E1MonoBehaviourManager singleton;

        public List<E1CubeMovementMonoBehaviour> CubeList = new List<E1CubeMovementMonoBehaviour>();

        private void Awake()
        {
            singleton = this;
        }

        private void Update()
        {
            var listCount = CubeList.Count;
            for (int i = 0; i < listCount; i++)
            {
                var item = CubeList[i];
                item.OnUpdate();
            }
        }
    }
}
