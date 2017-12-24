using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Exemple.MonoBehaviours
{
    /// <summary>
    /// Show how to use the same exemple as a normal monobehaviour without multithreading
    /// </summary>
    public class E1CubeMovementMonoBehaviour : MonoBehaviour
    {
        // ContractPositionScale
        public Vector3 Position;
        public Vector3 Scale;
        // ContractHeavyRotation
        public Quaternion Rotation;
        // ...
        
        private float m_yRot;
        private Vector3 m_eulerRotCached;

        public float Offset;

        private void Awake()
        {
            E1MonoBehaviourManager.singleton.CubeList.Add(this);
        }

        public void OnUpdate()
        {
            // From Contract position part
            // ------------------------------
            // Position...
            Position.x = Mathf.PingPong(Time.time, 5) - 1000 + Offset * 5;
            Position.y = Offset * 0.08f;

            // Scale...
            Scale.x = 0.25f + (Mathf.PingPong((Time.time + Offset) * 2, 5) * 0.25f);
            Scale.y = Scale.x;
            Scale.z = Scale.x;
            // ...

            // From Contract rotation part
            // ------------------------------
            for (var i = 0; i < E1CubeMovementOnStart.HeavyWorkIteration; i++) //< heavy work right here
            {
                m_yRot += (Time.deltaTime * 0.008f) + Offset * 0.0001f;
            }

            m_eulerRotCached.y = Mathf.Lerp(m_eulerRotCached.y, m_yRot, Time.deltaTime * 5f);
            Rotation.eulerAngles = m_eulerRotCached;
            // ...

            // Apply variables to the gameobject
            // ------------------------------
            transform.position = Position;
            transform.localScale = Scale;
            transform.rotation = Rotation;
        }
    }
}