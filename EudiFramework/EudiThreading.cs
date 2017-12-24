﻿using EudiFramework.Replica;
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
        /// <returns></returns>
        public static ThreadGroup GetThreadGroup<T>(int groupId = 0)
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
                finalValue = new ThreadGroup(type, groupId);
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
                while(!workerTask.cancelToken.IsCancellationRequested)
                {
                    var count = workerTask.Workers.Count;
                    for (int i = 0; i < count; i++)
                    {
                        var worker = workerTask.Workers[i];
                        worker.ForceWorkerUpdate(workerTask.TaskId);
                    }
                    await Task.Delay(6);
                }
            }, workerTask.cancelToken.Token);
            workerTask.IsWaitingForStart = true;
            return workerTask;
        }
    }
}
