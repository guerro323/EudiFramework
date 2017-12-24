using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EudiFramework.Threading
{
    public class WorkerTask : IDisposable
    {
        public bool IsWaitingForStart { get; internal set; }
        public Task Task;
        public ThreadGroup Group;
        public CancellationTokenSource cancelToken;
        public int TaskId;
        public List<EudiComponentWorker> Workers = new List<EudiComponentWorker>();
        public int RefreshRate = 6;
        public WorkerTaskReplicaTime ReplicaTime = new WorkerTaskReplicaTime();
        public EudiSynchronizationType SynchronizationType;

        public void Dispose()
        {
            ReplicaTime = null;
            Group = null;
            cancelToken.Cancel();
            Task.Dispose();
            Workers.Clear();
            Workers = null;
        }
    }
}
