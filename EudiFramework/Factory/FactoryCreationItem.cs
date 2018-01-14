using System;
using UnityEngine;

namespace EudiFramework.Factory
{
	public abstract class FactoryCreationItem<TComponent>
		where TComponent : EudiComponentBehaviour
	{
		public virtual Type MonoBehaviourComponentType => typeof(TComponent);

		public virtual void FinalizeItem()
		{
			
		}
		
		public virtual void ModifyComponent(TComponent component)
		{
			
		}
	}
}