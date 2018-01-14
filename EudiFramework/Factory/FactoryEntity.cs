namespace EudiFramework.Factory
{
    public class FactoryEntity : EudiComponentBehaviour
    {
        protected override void UnityAwake()
        {
            base.UnityAwake();

            CreateComponents();
        }

        protected virtual void CreateComponents()
        {
        }
    }
}