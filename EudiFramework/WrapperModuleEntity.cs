using UnityEngine;

namespace EudiFramework
{
    public abstract class WrapperModuleEntity : EudiComponentBase
    {
        protected internal virtual IShareableModule _instance { get; }
    }

// Create our entity automatically
    public class WrapperModuleEntity<T> : WrapperModuleEntity
        where T : IShareableModule
    {
        protected internal override IShareableModule _instance => Instance;
        public T Instance;

        protected internal override void InternalAwake()
        {
            int referenceToModule = 0;
            if (!EudiEntity.HasModule<T>(ref referenceToModule))
            {
                EudiEntity._addModule(Instance);
            }
        }

        private void OnValidate()
        {
            if (EudiEntity == null || !Application.isPlaying)
                return;
            
            EudiEntity.SetModule(Instance);
        }

        protected internal override void InternalOnDestroy()
        {
            base.InternalOnDestroy();
            
            
        }
    }
}