using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBall : MonoBehaviour
{
    [SerializeField] private float speed = 10f;

    private Rigidbody   rb;
    private Vector2     moveInput;
    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled  += ctx => moveInput = Vector2.zero;
    }

    private void OnEnable()  { controls.Player.Enable(); }
    private void OnDisable() { controls.Player.Disable(); }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 force = new Vector3(moveInput.x, 0f, moveInput.y);
        rb.AddForce(force * speed);
    }
}

