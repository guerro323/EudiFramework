using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudiFramework
{
    public interface IEudiGameObjectManager
    {
        void OnNewEudiComponent(EudiComponentBehaviour component);
        void OnRemoveEudiComponent(EudiComponentBehaviour component);
        void FixedUpdate();
        void Update();
        void LateUpdate();
        void OnDestroy();
        
        // TODO: implement searching and finding methods
    }
}
