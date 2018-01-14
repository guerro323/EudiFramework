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
        internal bool HasUnityUpdater = false;
        internal EudiThreadGroupUnityUpdater LinkToUnityUpdater;
        
        public List<EudiComponentBehaviour> ComponentsList = new List<EudiComponentBehaviour>();
        private List<EudiComponentBehaviour> m_ComponentsWithFixedUpdate = new List<EudiComponentBehaviour>();
        private List<EudiComponentBehaviour> m_ComponentsWithUpdate = new List<EudiComponentBehaviour>();
        private List<EudiComponentBehaviour> m_ComponentsWithLateUpdate = new List<EudiComponentBehaviour>();

        public void OnNewEudiComponent(EudiComponentBehaviour component)
        {
            ComponentsList.Add(component);

            if (ReflectionUtility.IsOverride(component, "UnityFixedUpdate"))
                m_ComponentsWithFixedUpdate.Add(component);
            if (ReflectionUtility.IsOverride(component, "UnityUpdate"))
                m_ComponentsWithUpdate.Add(component);
            if (ReflectionUtility.IsOverride(component, "UnityLateUpdate"))
                m_ComponentsWithLateUpdate.Add(component);
        }

        public void OnRemoveEudiComponent(EudiComponentBehaviour component)
        {
            ComponentsList.Remove(component);

            if (m_ComponentsWithFixedUpdate.Contains(component))
                m_ComponentsWithFixedUpdate.Remove(component);
            if (m_ComponentsWithUpdate.Contains(component))
                m_ComponentsWithUpdate.Remove(component);
            if (m_ComponentsWithLateUpdate.Contains(component))
                m_ComponentsWithLateUpdate.Remove(component);

            Eudi.EntitiesManager._removeEntity(component.gameObject); //< we do a blind remove (the 'try remove' function will check if we can remove the entity)
        }

        public void FixedUpdate()
        {
            if (HasUnityUpdater)
                LinkToUnityUpdater.ManagerFixedUpdate();
            
            var listCount = m_ComponentsWithFixedUpdate.Count;
            for (int i = 0; i < listCount; i++)
            {
                var item = m_ComponentsWithFixedUpdate[i];
                item._DoFixedUpdate();
            }
        }

        public void Update()
        {
            if (HasUnityUpdater)
                LinkToUnityUpdater.ManagerUpdate();
            
            var listCount = m_ComponentsWithUpdate.Count;
            for (int i = 0; i < listCount; i++)
            {
                var item = m_ComponentsWithUpdate[i];
                item._DoUpdate();
            }
        }

        public void LateUpdate()
        {
            if (HasUnityUpdater)
                LinkToUnityUpdater.ManagerLateUpdate();
            
            var listCount = m_ComponentsWithLateUpdate.Count;
            for (int i = 0; i < listCount; i++)
            {
                var item = m_ComponentsWithLateUpdate[i];
                item._DoLateUpdate();
            }
        }

        public void OnDestroy()
        {
            
        }
    }
}
