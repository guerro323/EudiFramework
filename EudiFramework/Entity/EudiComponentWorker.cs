using EudiFramework.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EudiFramework
{
    [Serializable]
    public abstract class EudiComponentWorker
    {
        private int m_threadCount;
        public int ThreadCount => m_threadCount;

        /// <summary>
        /// Tasks to start when finishing the setup, get cleaned after finish
        /// </summary>
        private List<WorkerTask> m_workerTasks = new List<WorkerTask>();
        private List<ThreadGroup> m_threadGroups = new List<ThreadGroup>();

        public EudiComponentBehaviour parent;

        public bool IsEnabled;

        internal void ForceWorkerUpdate(EudiWorkerUpdateEvent ev)
        {
            WorkerUpdate(ev); //< specify threadId for heavy task.
        }

        /// <summary>
        /// Will get call when a worker was added
        /// </summary>
        /// <param name="workerTask">The worker task</param>
        /// <param name="firstCreation">Was the worker task just created?</param>
        protected virtual void OnNewWorkerTask(WorkerTask workerTask, bool firstCreation)
        {

        }

        /// <summary>
        /// Will get called when the worker is enabled
        /// </summary>
        protected virtual void WorkerUpdate(EudiWorkerUpdateEvent ev)
        {

        }

        /// <summary>
        /// Will get called when the worker isn't enabled
        /// </summary>
        protected virtual void OffWorkUpdate(int threadId, int workerId)
        {

        }

        protected virtual bool WorkerOnDestroy() { return true; }

        public void Destroy()
        {
            var cleanFields = WorkerOnDestroy();

            parent = null;

            // No need to perform this if we are ending the application, the initiatorEngine is already removing that for us
            if (!Eudi.ApplicationIsEnding)
            {
                foreach (var workerTask in m_workerTasks)
                {
                    workerTask.Workers.Remove(this);
                    if (workerTask.Workers.Count == 0)
                    {
                        var group = m_threadGroups[workerTask.TaskId];
                        EudiThreading.RemoveThreadGroup(group, group.MetaCreationType, group.MetaCreationGroupId);
                    }
                }
            }

            m_threadGroups.Clear();
            m_threadGroups = null;
            m_workerTasks.Clear();
            m_workerTasks = null;

            if (!cleanFields)
            {
                var type = GetType();
                var fields = type.GetFields();
                var fieldsCount = fields.Length;
                for (int i = 0; i < fieldsCount; i++)
                {
                    var field = fields[i];
                    field.SetValue(this, null);
                }
            }
        }

        public bool ValidContracts()
        {
            var type = GetType();
            var fields = type.GetFields();
            var fieldsCount = fields.Length;
            for (int i = 0; i < fieldsCount; i++)
            {
                var field = fields[i];
                var hasContractAttribute = Attribute.IsDefined(field, typeof(EudiFieldContractAttribute));
                if (hasContractAttribute && field.GetValue(this) == null)
                {
                    return false;
                }
            }
            return true;
        }

        public void SetThreadCount(int newValue)
        {
            m_threadCount = newValue;
        }

        public void SetThreadShareParam(int workId, ThreadGroup threadGroup, EudiSynchronizationType syncType = EudiSynchronizationType.Unity)
        {
            if (threadGroup.UseDefaultTasks)
            {
                var firstCreation = false;
                WorkerTask task = null;
                if (!threadGroup.Tasks.TryGetValue(workId, out task))
                {
                    task = EudiThreading.CreateTask();
                    task.TaskId = workId;

                    threadGroup.Tasks[workId] = task;

                    firstCreation = true;
                }

                if (task != null)
                {
                    task.SynchronizationType = syncType;

                    OnNewWorkerTask(task, firstCreation);

                    m_threadGroups.Add(threadGroup);

                    task.Workers.Add(this);

                    m_workerTasks.Add(task);
                }
            }
            else
            {
                threadGroup.CreateFromWorker(this, workId, syncType);
                m_threadGroups.Add(threadGroup);
            }
        }

        public void FinishSetup()
        {
            foreach (var workerTask in m_workerTasks)
            {
                if (workerTask.IsWaitingForStart)
                {
                    workerTask.IsWaitingForStart = false;
                    workerTask.Task.Start(workerTask.SynchronizationType == EudiSynchronizationType.Unity 
                        ? Eudi.UnityTaskScheduler
                        : TaskScheduler.Default);
                }
            }
        }
    }
}
