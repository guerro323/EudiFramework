using System;
using System.Collections;
using System.Collections.Generic;
using EudiFramework;
using UnityEngine;

namespace _P4Debug.EudiAndJob
{
	[Serializable]
	public struct RotationSpeed : IShareableModule, IEquatable<RotationSpeed>
	{
		public float speed;

		public RotationSpeed(float speed)
		{
			this.speed = speed;
		}

		public bool Equals(RotationSpeed other)
		{
			return speed.Equals(other.speed);
		}
	}

	public class RotationSpeedDataComponent : WrapperModuleEntity<RotationSpeed>
	{
	}
}
