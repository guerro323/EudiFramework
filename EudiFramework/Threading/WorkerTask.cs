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
        public CancellationTokenSource cancelToken;
        public int TaskId;
        public List<EudiComponentWorker> Workers = new List<EudiComponentWorker>();

        public void Dispose()
        {
            cancelToken.Cancel();
            Task.Dispose();
            Workers.Clear();
            Workers = null;
        }
    }
}
