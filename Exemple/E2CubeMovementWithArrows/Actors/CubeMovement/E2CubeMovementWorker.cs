using EudiFramework;
using EudiFramework.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class E2CubeMovementWorker : EudiComponentWorker
{
    [EudiFieldContract]
    public E2CubeMovementContractPosition   ContractPosition;
    public E2CubeMovementDataInput          DataInput;

    private E2CubeMovementComponent         m_component;

    public E2CubeMovementWorker(out E2CubeMovementContractPosition contractPosition,
        ref E2CubeMovementDataInput dataInput)
    {
        contractPosition = ContractPosition = new E2CubeMovementContractPosition();
        DataInput = dataInput;

        SetThreadCount(1);
        SetThreadShareParam(0, EudiThreading.GetThreadGroup<E2CubeMovementWorker>());
    }

    protected override async void OnNewWorkerTask(WorkerTask workerTask, bool firstCreation)
    {
        while (m_component == null)
            await Task.Delay(2);
        workerTask.RefreshRate = m_component.UpdateTime;
    }

    protected override void WorkerUpdate(EudiWorkerUpdateEvent ev)
    {
        m_component = parent as E2CubeMovementComponent;

        if (m_component.UseLock)
        {
            if (!ContractPosition.IsLocked)
            {
                ContractPosition.Position.x += DataInput.HorizontalValue * (EudiFramework.Replica.EudiReplicaTime.deltaTime * DataInput.Speed);
                ContractPosition.Lock();
            }
        }
        else
        {
            ContractPosition.Position.x += DataInput.HorizontalValue * (ev.WorkerTask.ReplicaTime.TimeDelta * DataInput.Speed);
        }
    }
}