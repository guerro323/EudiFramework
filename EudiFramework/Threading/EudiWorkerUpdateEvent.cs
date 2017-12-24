using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudiFramework.Threading
{
    public class EudiWorkerUpdateEvent
    {
        public int threadId;
        public int workId;
        public ThreadGroup Group;
        public WorkerTask WorkerTask;
    }
}
