using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudiFramework.Threading
{
    public class WorkerTaskReplicaTime
    {
        public float TimeDelta { get; internal set; }
        public int IntegerTimeDelta { get; internal set; }

        public long CurrentTick;

        public void Create()
        {
            CurrentTick = DateTime.UtcNow.Ticks;
        }

        public void Update()
        {
            var tickDelta = DateTime.UtcNow.Ticks - CurrentTick;
            if (tickDelta > short.MaxValue)
                tickDelta = short.MaxValue;
            IntegerTimeDelta = (int)tickDelta;
            TimeDelta = IntegerTimeDelta * 0.0000001f;

            CurrentTick = DateTime.UtcNow.Ticks;
        }
    }
}
