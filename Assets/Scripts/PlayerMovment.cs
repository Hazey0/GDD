using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovment : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed;

    private Vector3 moveDir;
    public InputActionReference move;

    void Update()
    {
        Vector2 input = move.action.ReadValue<Vector2>();
        moveDir = new Vector3(input.x, 0, input.y);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(moveDir.x * moveSpeed, 0, moveDir.z * moveSpeed);
    }
}