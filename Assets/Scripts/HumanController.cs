using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class HumanController : MonoBehaviour
   
{
    public AudioSource walkSound;
    public AudioSource jumpSound;
    public AudioSource attackSound;
    public AudioSource landSound;
    public AudioSource dashSound;


    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpHeight = 2f;
    public float gravity = -20f;

    [Header("Camera Relative Movement")]
    public Transform cameraTransform;

    private bool falconIsWithHuman = true;

    [Header("Dash")]
    public float dashSpeed = 14f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.5f;
    

    [Header("Double Jump")]
    public int maxJumpsWithFalcon = 2;
    public int maxJumpsWithoutFalcon = 1;

    private int jumpsUsed = 0;  

    private CharacterController controller;

    private Vector2 moveInput;
    private Vector3 velocity;

    private bool isActiveCharacter = true;
    private bool isDashing = false;
    private bool hasTouchedGroundSinceDash = true;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    private Vector3 dashDirection;
    public Animator animator;
    private bool wasGrounded = false;
    //private bool isWalking;
    //private bool isJumping;
    private int isWalkingHash;
    private int isJumpingHash;

    private bool wasGroundedLastFrame = false;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
        
    }

    private void Update()
    {
        HandleGravityAlways();

        if (!isActiveCharacter)
        {
            ApplyGravityOnly();
            return;
        }

        HandleCooldowns();
        HandleMovement();

        float inputMagnitude = moveInput.magnitude;

        if (inputMagnitude > 0.1f)
        {
            animator.SetBool("isWalking", true);
            if (!walkSound.isPlaying && controller.isGrounded)
            {

            walkSound.Play();
            }

        }
        else if (!controller.isGrounded && walkSound.isPlaying)
        {
            walkSound.Stop();
        }
        else
        {
            animator.SetBool("isWalking", false);

        }

    }

    private void HandleGravityAlways()
    {
        if (isDashing)
        {
            velocity.y = 0f;
            return;
        }

        bool isGroundedNow = controller.isGrounded;

    
        if (wasGrounded && !isGroundedNow)
        {
        
            if (jumpsUsed == 0)
            {
                jumpsUsed = 1;
            }

            if (animator != null)
            {
                animator.SetBool("isJumping", true);
            }
        }

        if (isGroundedNow && velocity.y < 0f)
        {
            if (!wasGrounded)
            {
                if (!dashSound.isPlaying)
                {
                    landSound.Play();
                }

                jumpSound.Stop();
                walkSound.Stop(); // optional reset
            }

            velocity.y = -2f;

            jumpsUsed = 0;

            hasTouchedGroundSinceDash = true;

            if (animator != null)
            {
                jumpSound.Stop();
                animator.SetBool("isJumping", false);
            }
        }

        velocity.y += gravity * Time.deltaTime;

        wasGrounded = isGroundedNow;
    }

    private void ApplyGravityOnly()
    {
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleCooldowns()
    {
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    private void HandleMovement()
    {
        Vector3 moveDirection;

        if (cameraTransform != null)
        {
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;

            cameraForward.Normalize();
            cameraRight.Normalize();

            moveDirection = cameraForward * moveInput.y + cameraRight * moveInput.x;
        }
        else
        {
            moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        }

        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                12f * Time.deltaTime
            );
        }

        if (isDashing)
        {
            // Dash ignores WASD and only moves in the saved dash direction.
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);

            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0f)
            {
                isDashing = false;
            }

           
            return; //This is so gravity does not apply during dash
        }
        else
        {
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);
            
        }

        // Gravity only applies when NOT dashing.
        controller.Move(velocity * Time.deltaTime);
    }

    public void SetActiveCharacter(bool active)
    {
        isActiveCharacter = active;

        if (!active)
        {
            moveInput = Vector2.zero;
            isDashing = false;
        }
    }

    public bool IsActiveCharacter()
    {
        return isActiveCharacter;
    }

    public void setFalconWithHuman(bool withHuman)
    {
        falconIsWithHuman = withHuman;

        if(!falconIsWithHuman)
        {
            isDashing = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!isActiveCharacter)
            return;
        if (isDashing)
            return;

      
        
        
        moveInput = context.ReadValue<Vector2>();

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!isActiveCharacter)
            return;
        if (isDashing)
            return;
        if (!context.performed)
            return;
        

        int allowedJumps = falconIsWithHuman ? maxJumpsWithFalcon : maxJumpsWithoutFalcon;

        if(jumpsUsed < allowedJumps)
        {
            if (!jumpSound.isPlaying)
            {
                jumpSound.Play();
                walkSound.Stop();
            }
            if(animator != null)
            {
                animator.SetBool("isJumping", true);
            }

            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpsUsed++;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!isActiveCharacter)
            return;

        if (!context.performed)
            return;

        if(!falconIsWithHuman)
            return;

        if (dashCooldownTimer > 0f)
            return;

        if (!hasTouchedGroundSinceDash)
            return;

        Vector3 moveDirection;

        if (cameraTransform != null)
        {
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;

            cameraForward.Normalize();
            cameraRight.Normalize();

            moveDirection = cameraForward * moveInput.y + cameraRight * moveInput.x;
            dashSound.Play();
        }
        else
        {
            moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        }

        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        if (moveDirection.magnitude < 0.1f)
        {
            moveDirection = transform.forward;
            
        }

        dashDirection = moveDirection.normalized;
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;
        hasTouchedGroundSinceDash = false;
    }
}