using System;
using UnityEngine;

namespace EudiFramework
{
    [Serializable]
    public class EudiComponentContract
    {
        [SerializeField]
        private bool m_IsLocked;
        public bool IsLocked => m_IsLocked;

        public void Lock() => m_IsLocked = true;
        public void Unlock() => m_IsLocked = false;
    }
}
