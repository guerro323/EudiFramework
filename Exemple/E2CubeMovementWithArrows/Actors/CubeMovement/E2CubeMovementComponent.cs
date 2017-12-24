using EudiFramework;
using UnityEngine;

public class E2CubeMovementComponent : EudiComponentBehaviour
{
    [EudiFieldContract]
    public E2CubeMovementContractPosition contractPosition;

    public E2CubeMovementDataInput DataInput = new E2CubeMovementDataInput();

    private E2CubeMovementWorker worker;

    public float Speed = 2.0f;
    public int UpdateTime = 6;
    public bool UseLock = false;

    protected override void UnityAwake()
    {
        AddWorker(worker = new E2CubeMovementWorker(out contractPosition, ref DataInput));
    }

    protected override void UnityUpdate()
    {
        Debug.Log("hi");

        DataInput.HorizontalValue = Input.GetAxis("Horizontal");
        DataInput.Speed = Speed;

        worker.DataInput = DataInput;

        transform.position = contractPosition.Position;

        if (UseLock)
            contractPosition.Unlock();
    }
}
