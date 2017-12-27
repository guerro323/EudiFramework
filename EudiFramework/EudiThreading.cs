using EudiFramework.Replica;
using EudiFramework.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EudiFramework
{
    /// <summary>
    /// If you want a simple way to use threading in workers, use this utility class instead of manually instanting <see cref="ThreadGroup"/>.
    /// </summary>
    public static class EudiThreading
    {
        private static Dictionary<Type, Dictionary<int, ThreadGroup>> m_ThreadGroups
            = new Dictionary<Type, Dictionary<int, ThreadGroup>>();

        /// <summary>
        /// Get the group of a specific class
        /// </summary>
        /// <typeparam name="T">The class</typeparam>
        /// <param name="groupId">The group id (default is 0)</param>
        /// <param name="existingGroup">If you created a group, you can use it here</param>
        /// <returns></returns>
        public static ThreadGroup GetThreadGroup<T>(int groupId = 0, ThreadGroup existingGroup = null)
        {
            var type = typeof(T);
            Dictionary<int, ThreadGroup> dicoGroups = null;
            ThreadGroup finalValue = null;

            if (!m_ThreadGroups.TryGetValue(type, out dicoGroups))
            {
                dicoGroups = new Dictionary<int, ThreadGroup>();
                m_ThreadGroups[type] = dicoGroups;
            }
            if (dicoGroups.TryGetValue(groupId, out finalValue))
            {
                return finalValue;
            }
            else
            {
                finalValue = existingGroup ?? new ThreadGroup(type, groupId);
                dicoGroups[groupId] = finalValue;
                return finalValue;
            }
        }

        public static void RemoveThreadGroup(ThreadGroup threadGroup, Type type = null, int groupId = -1)
        {
            if (type != null)
            {
                if (groupId >= 0)
                {
                    var dico = m_ThreadGroups[type];

                    dico[groupId].Dispose();
                    dico.Remove(groupId);

                    if (dico.Count == 0)
                        m_ThreadGroups.Remove(type);
                }
            }
        }

        public static WorkerTask CreateTask()
        {
            var workerTask = new WorkerTask();
            workerTask.cancelToken = new CancellationTokenSource();
            workerTask.Task = new Task(async () =>
            {
                try
                {
                    var updateEvent = new EudiWorkerUpdateEvent() { WorkerTask = workerTask };

                    while (!workerTask.cancelToken.IsCancellationRequested)
                    {
                        updateEvent.Group = workerTask.Group;
                        updateEvent.threadId = workerTask.Group?.MetaCreationGroupId ?? -1;
                        updateEvent.workId = workerTask.TaskId;

                        workerTask.ReplicaTime.Update();

                        var count = workerTask.Workers.Count;
                        for (int i = 0; i < count; i++)
                        {
                            var worker = workerTask.Workers[i];
                            worker.ForceWorkerUpdate(updateEvent);
                        }

                        if (workerTask.Updater.WaitAction == null)
                            await Task.Delay(workerTask.Updater.RefreshRate);
                        else
                        {
                            // init task...
                            await workerTask.Updater.WaitAction();
                        }
                    }
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError("[THREADING] TASK ERROR!\n" + ex.ToString());
                }
            }, workerTask.cancelToken.Token);
            workerTask.IsWaitingForStart = true;
            return workerTask;
        }
    }
}
