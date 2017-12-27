using System;
using System.Collections.Generic;

namespace EudiFramework.Threading
{
    /// <summary>
    /// Thread group that use the main unity thread (in case you don't want to use multithreading)
    /// </summary>
    public class UnityThreadGroup : ThreadGroup
    {
        protected class WorkerWithWorkId
        {
            public int WorkId;
            public EudiComponentWorker Worker;
        }

        public UnityUpdateType UpdateType;
        public override bool UseDefaultTasks => false;

        protected List<WorkerWithWorkId> Workers = new List<WorkerWithWorkId>();

        private EudiWorkerUpdateEvent defaultUpdateEvent;

        public UnityThreadGroup(UnityUpdateType updateType,Type type, int groupId) : base(type, groupId)
        {
            UpdateType = updateType;
            defaultUpdateEvent = new EudiWorkerUpdateEvent()
            {
                Group = this,
                threadId = MetaCreationGroupId,
                WorkerTask = null,
            };

            var updaterBehaviour = Eudi.Globals.Bind<IEudiThreadGroupUnityUpdater, EudiThreadGroupUnityUpdater>();
            updaterBehaviour.OnNewThreadGroup(this);
        }

        public override void CreateFromWorker(EudiComponentWorker worker, int workId, EudiSynchronizationType syncType)
        {
            Workers.Add(new WorkerWithWorkId() { Worker = worker, WorkId = workId });
        }

        internal void _DoFixedUpdate()
        {
            var listCount = Workers.Count;
            for (int i = 0; i < listCount; i++)
            {
                var item = Workers[i];
                defaultUpdateEvent.workId = item.WorkId;
                item.Worker.ForceWorkerUpdate(defaultUpdateEvent);
            }
        }

        internal void _DoUpdate()
        {
            var listCount = Workers.Count;
            for (int i = 0; i < listCount; i++)
            {
                var item = Workers[i];
                defaultUpdateEvent.workId = item.WorkId;
                item.Worker.ForceWorkerUpdate(defaultUpdateEvent);
            }
        }

        internal void _DoLateUpdate()
        {
            var listCount = Workers.Count;
            for (int i = 0; i < listCount; i++)
            {
                var item = Workers[i];
                defaultUpdateEvent.workId = item.WorkId;
                item.Worker.ForceWorkerUpdate(defaultUpdateEvent);
            }
        }

        protected override void NormalDispose()
        {
            Workers.Clear();
            Workers = null;
        }
    }
}
