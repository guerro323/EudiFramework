using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Collections;
using System;

namespace EudiFramework
{
    /// <summary>
    /// Standard class that work like a standard <see cref="MonoBehaviour"/> component.
    /// </summary>
    public class EudiComponentBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Get the list of the workers of this component
        /// </summary>
        public List<EudiComponentWorker> Workers = new List<EudiComponentWorker>();

        private void Awake()
        {
            Eudi.Globals.GetBinding<IEudiGameObjectManager>().OnNewEudiComponent(this);

            UnityAwake();
            if (!ValidContracts())
            {
                Debug.LogError("[BEHAVIOUR] Invalid contracts");
            }
        }

        private void Start()
        {
            UnityStart();
        }

        internal void _DoUpdate()
        {
            UnityUpdate();
        }

        internal void _DoLateUpdate()
        {
            UnityLateUpdate();
        }

        internal void _DoFixedUpdate()
        {
            UnityFixedUpdate();
        }

        private void OnDestroy()
        {
            Eudi.Globals.GetBinding<IEudiGameObjectManager>().OnRemoveEudiComponent(this);

            var cleanFields = UnityOnDestroy();

            var workersCount = Workers.Count;
            for (int i = 0; i < workersCount; i++)
            {
                var worker = Workers[i];
                worker.Destroy();
            }

            Workers.Clear();

            if (!cleanFields)
            {
                var type = GetType();
                var fields = type.GetFields();
                var fieldsCount = fields.Length;
                for (int i = 0; i < fieldsCount; i++)
                {
                    var field = fields[i];
                    field.SetValue(this, null);
                }
            }
            else
            {
                Workers = null;
            }
        }

        /// <summary>
        /// Get called by the unity event 'Awake'
        /// </summary>
        protected virtual void UnityAwake() { }
        /// <summary>
        /// Get called by the unity event 'Start'
        /// </summary>
        protected virtual void UnityStart() { }
        /// <summary>
        /// Get called by the unity event 'Update'
        /// </summary>
        protected virtual void UnityUpdate() { }
        /// <summary>
        /// Get called by the unity event 'LateUpdate'
        /// </summary>
        protected virtual void UnityLateUpdate() { }
        /// <summary>
        /// Get called by the unity event 'FixedUpdate'
        /// </summary>
        protected virtual void UnityFixedUpdate() { }
        /// <summary>
        /// Get called by the unity event 'OnDestroy'. The return value indicate if the fields should be automatically set to null.
        /// </summary>
        /// <returns>Indicate if the fields should be automatically set to null.</returns>
        protected virtual bool UnityOnDestroy() { return true; }

        /// <summary>
        /// Add a worker to the component.
        /// </summary>
        /// <param name="worker">The worker</param>
        /// <param name="enable">Enable the worker after this function was called</param>
        public void AddWorker(EudiComponentWorker worker, bool enable = true)
        {
            Workers.Add(worker);
            if (worker.ValidContracts())
            {
                worker.parent = this;
                worker.IsEnabled = enable;
            }
            else
            {
                Debug.LogError("[WORKER] Invalid contracts");
            }

            worker.FinishSetup();
        }

        /// <summary>
        /// Return true if the contracts of this component are valid.
        /// </summary>
        /// <returns></returns>
        public bool ValidContracts()
        {
            var type = GetType();
            var fields = type.GetFields();
            var fieldsCount = fields.Length;
            for (int i = 0; i < fieldsCount; i++)
            {
                var field = fields[i];
                var hasContractAttribute = Attribute.IsDefined(field, typeof(EudiFieldContractAttribute));
                if (hasContractAttribute && field.GetValue(this) == null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}