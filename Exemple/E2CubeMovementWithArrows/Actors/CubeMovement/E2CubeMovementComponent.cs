using EudiFramework;
using UnityEngine;

public class E2CubeMovementComponent : EudiComponentBehaviour
{
    [EudiFieldContract]
    public E2CubeMovementContractPosition contractPosition;

    public E2CubeMovementDataInput DataInput = new E2CubeMovementDataInput();

    public float Speed = 2.0f;
    public int UpdateTime = 6;
    public bool UseLock = false;

    protected override void UnityAwake()
    {
        AddWorker(new E2CubeMovementWorker(out contractPosition, ref DataInput));
    }

    protected override void UnityUpdate()
    {
        DataInput.HorizontalValue = Input.GetAxis("Horizontal");
        DataInput.Speed = Speed;

        transform.position = contractPosition.Position;

        if (UseLock)
            contractPosition.Unlock();
    }
}
