using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EudiFramework
{
    [DefaultExecutionOrder(-9998)]
    public class EudiGameObjectManager : MonoBehaviour, IEudiGameObjectManager
    {
        public List<EudiComponentBehaviour> ComponentsList = new List<EudiComponentBehaviour>();

        public void OnNewEudiComponent(EudiComponentBehaviour component)
        {
            ComponentsList.Add(component);
        }

        public void OnRemoveEudiComponent(EudiComponentBehaviour component)
        {
            ComponentsList.Remove(component);
        }

        public void FixedUpdate()
        {
            var listCount = ComponentsList.Count;
            for (int i = 0; i < listCount; i++)
            {
                var item = ComponentsList[i];
                item._DoFixedUpdate();
            }
        }

        public void Update()
        {
            var listCount = ComponentsList.Count;
            for (int i = 0; i < listCount; i++)
            {
                var item = ComponentsList[i];
                item._DoUpdate();
            }
        }

        public void LateUpdate()
        {
            var listCount = ComponentsList.Count;
            for (int i = 0; i < listCount; i++)
            {
                var item = ComponentsList[i];
                item._DoLateUpdate();
            }
        }

        public void OnDestroy()
        {
            
        }
    }
}
