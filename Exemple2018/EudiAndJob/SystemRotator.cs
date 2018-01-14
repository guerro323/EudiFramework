using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using EudiFramework;
using EudiFramework.Replica;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace _P4Debug.EudiAndJob
{
	public class SystemRotator : EudiSystemBehaviour
	{
		[InjectTuples] public TransformAccessArray m_Transforms;

		[InjectTuples] public EudiMatchArray<RotationSpeed> m_Rotators;

		protected override void UnityUpdate()
		{
			base.UnityUpdate();
			
			var job = new Job
			{
				dt = Time.deltaTime,
				entities = MatchEntities,
			};
			
			AddDependency(job.Schedule(m_Transforms, GetDependency()));
		}

		struct Job : IJobParallelForTransform
		{
			public float dt;
			[ReadOnly] public EudiStructList<EudiEntity> entities;

			public void Execute(int i, TransformAccess transform)
			{
				var rotator = entities[i].GetLazyModule<RotationSpeed>();
				
				transform.rotation = transform.rotation *
				                     Quaternion.AngleAxis(dt * rotator.speed, Vector3.up);
			}
		}
	}
}
