using EudiFramework.Threading;
using System.Collections.Generic;
using UnityEngine;

namespace EudiFramework
{
    public class EudiThreadGroupUnityUpdater : MonoBehaviour, IEudiThreadGroupUnityUpdater
    {
        private List<UnityThreadGroup> m_groupsWithFixedUpdate = new List<UnityThreadGroup>();
        private List<UnityThreadGroup> m_groupsWithUpdate = new List<UnityThreadGroup>();
        private List<UnityThreadGroup> m_groupsWithLateUpdate = new List<UnityThreadGroup>();

        private void Awake()
        {
            var manager = Eudi.Globals.GetBinding<IEudiGameObjectManager>() as EudiGameObjectManager;
            manager.HasUnityUpdater = true;
            manager.LinkToUnityUpdater = this;
        }

        public void OnNewThreadGroup(UnityThreadGroup group)
        {
            if (group.UpdateType == UnityUpdateType.FixedUpdate)
                m_groupsWithFixedUpdate .Add(group);
            if (group.UpdateType == UnityUpdateType.Update)
                m_groupsWithUpdate      .Add(group);
            if (group.UpdateType == UnityUpdateType.LateUpdate)
                m_groupsWithLateUpdate  .Add(group);
        }

        public void ManagerFixedUpdate()
        {
            var listCount = m_groupsWithFixedUpdate.Count;
            for (int i = 0; i < listCount; i++)
            {
                var item = m_groupsWithFixedUpdate[i];
                item._DoFixedUpdate();
            }
        }

        public void ManagerUpdate()
        {
            var listCount = m_groupsWithUpdate.Count;
            for (int i = 0; i < listCount; i++)
            {
                var item = m_groupsWithUpdate[i];
                item._DoUpdate();
            }
        }

        public void ManagerLateUpdate()
        {
            var listCount = m_groupsWithLateUpdate.Count;
            for (int i = 0; i < listCount; i++)
            {
                var item = m_groupsWithLateUpdate[i];
                item._DoLateUpdate();
            }
        }
    }
}
