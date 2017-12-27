using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudiFramework
{
    public interface IEudiThreadGroupUnityUpdater
    {
        void ManagerFixedUpdate();
        void ManagerUpdate();
        void ManagerLateUpdate();
    }
}
