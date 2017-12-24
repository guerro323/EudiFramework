using UnityEngine;

public class E2CubeMovementBehaviour : MonoBehaviour
{
    public float Speed = 2.0f; //< The movement speed factor
    private Vector3 m_position;

    private void Awake()
    {
        E2CubeMovementManager.BehavioursToUpdate.Add(this);
    }

    public void OnUpdate()
    {
        m_position.x += Input.GetAxis("Horizontal") * (Time.deltaTime * Speed);

        m_position.y = -2f;

        transform.position = m_position;
    }
}