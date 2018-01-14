using System;
using System.Collections.Generic;

namespace EudiFramework.Factory
{
	public class FactorySystem
	{
		private static Dictionary<Type, Factory> _factories;
		
		public static Factory GetFactory<T>() where T : Factory, new()
		{
			var type = typeof(T);
			
			if (_factories == null)
				_factories = new Dictionary<Type, Factory>();
			if (!_factories.ContainsKey(type))
			{
				return _factories[type] = new T();
			}
			return _factories[type];
		}
	}
}
