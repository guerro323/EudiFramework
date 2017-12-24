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
    public class MonoBehaviourManager : MonoBehaviour
    {
        public static MonoBehaviourManager singleton;

        public List<CubeMovementMonoBehaviour> CubeList = new List<CubeMovementMonoBehaviour>();

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
