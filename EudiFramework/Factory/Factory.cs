using System;
using UnityEngine;

namespace EudiFramework.Factory
{
	public abstract class Factory
	{
	}

	public abstract class Factory<TFactoryCreationItem> : Factory
		where TFactoryCreationItem : FactoryCreationItem<EudiComponentBehaviour>, new()
	{
		private readonly Type _baseType = typeof(TFactoryCreationItem);

		public virtual TFactoryCreationItem Construct()
		{
			return new TFactoryCreationItem();
		}

		public virtual T Create<T>(TFactoryCreationItem item) where T : EudiComponentBehaviour
		{
			item.FinalizeItem();
			var go = new GameObject(GetType().Name + "#" + item.GetType().Name);
			var comp = (MonoBehaviour)go.AddComponent(item.MonoBehaviourComponentType);
			//item.ModifyComponent(comp);
			throw new NotImplementedException();
			
			return comp as T;
		}

		public virtual void ReturnToFactory()
		{
			
		}
	}
}