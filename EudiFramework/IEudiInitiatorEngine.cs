using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudiFramework
{
    public interface IEudiInitiatorEngine
    {
        void Awake();
        void Update();
        void LateUpdate();
        void FixedUpdate();
    }
}
