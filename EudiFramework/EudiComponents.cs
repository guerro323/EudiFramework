using UnityEngine;

namespace EudiFramework
{
    public class EudiComponents
    {
        public void GOAddComponent<T>(GameObject bindVictim)
        {
            
        }

        public void ForceUnityUpdate(EudiComponentBehaviour componentBehaviour)
        {
            componentBehaviour._DoUpdate();
        }
    }
}
