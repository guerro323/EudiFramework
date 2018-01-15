using UnityEngine;

namespace EudiFramework
{
    public class EudiComponentBase : MonoBehaviour
    {
        /// <summary>
        /// The entity id of the gameobject
        /// </summary>
        public long EudiEntityId { get; internal set; }

        public EudiEntity EudiEntity { get; internal set; }

        private void Awake()
        {
            // Check if we have an entity component here, or else create it and give feedback to the manager.
            if (!GetComponent<WrapperEudiEntity>())
            {
                Eudi.EntitiesManager._addEntity(gameObject); // add the entity component.
            }
            else
            {
                var wrapper = GetComponent<WrapperEudiEntity>();
                if (!wrapper.IsOnRightGameObject())
                {
                    wrapper.ThrowFakeException_DifferentGameObject();
                    DestroyImmediate(wrapper); //< We use DestroyImmediate because we need to delete it right now, not after the frame.

                    //Add new wrapper...
                    Eudi.EntitiesManager._addEntity(gameObject);
                }
            }

            EudiEntity = GetComponent<WrapperEudiEntity>().Instance;
            EudiEntityId = EudiEntity.Id;
            
            InternalAwake();
        }

        private void OnDestroy() => InternalOnDestroy();
        
        protected internal virtual void InternalAwake()
        {
            
        }

        protected internal virtual void InternalOnDestroy()
        {
            
        }
    }
}