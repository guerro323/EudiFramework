using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudiFramework.Threading
{
    public class EudiWorkerTaskUpdater
    {
        protected int refreshRate;
        protected Func<Task> waitAction;

        /// <summary>
        /// Refresh rate of the updater (a non-null value for <see cref="WaitAction"/> will ignore this variable)
        /// </summary>
        public virtual int RefreshRate
        {
            get { return refreshRate; }
            set { refreshRate = value; }
        }
        /// <summary>
        /// Set a custom action for the worker task upate. (a null value will use <see cref="RefreshRate"/>)
        /// </summary>
        public virtual Func<Task> WaitAction
        {
            get { return WaitAction; }
            set { WaitAction = value; }
        }

        public EudiWorkerTaskUpdater(int refreshRate = 6, Func<Task> waitAction = null)
        {
            RefreshRate = refreshRate;
            WaitAction = waitAction;
        }
    }
}
