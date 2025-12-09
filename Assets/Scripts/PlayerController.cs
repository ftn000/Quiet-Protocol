using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement speeds")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float crouchSpeed = 1.5f;

    [Header("Smoothing")]
    public float speedSmoothTime = 0.08f;

    [Header("Noise radii")]
    public float noiseWalk = 3f;
    public float noiseRun = 10f;
    public float noiseCrouch = 0.5f;
    /*
    [Header("Optional")]
    public Transform spriteRoot;
    public Animator animator;*/

    // Input System
    private Vector2 moveInput;
    private bool sprintHeld;
    private bool crouchHeld;

    // Movement state
    private float currentSpeed;
    private float speedVelocity;

    // Components
    private Rigidbody2D rb;
    private NoiseEmitter noiseEmitter;

    // Animator parameters
    private readonly int animMoveX = Animator.StringToHash("MoveX");
    private readonly int animMoveY = Animator.StringToHash("MoveY");
    private readonly int animSpeed = Animator.StringToHash("Speed");
    private readonly int animSprint = Animator.StringToHash("Sprinting");
    private readonly int animCrouch = Animator.StringToHash("Crouching");

    public float CurrentNoiseRadius { get; private set; }
    public float CurrentSpeed => currentSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        noiseEmitter = GetComponent<NoiseEmitter>();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        Debug.Log("Move: " + moveInput);
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        sprintHeld = ctx.performed && !ctx.canceled;
    }

    public void OnCrouch(InputAction.CallbackContext ctx)
    {
        crouchHeld = ctx.performed && !ctx.canceled;
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            // Interaction logic will be added later
        }
    }

    private void Update()
    {
        UpdateSpeed();
        UpdateNoise();
        /*UpdateAnimator();
        OrientSprite();*/
    }

    private void FixedUpdate()
    {
        MovePhysics();
    }

    void UpdateSpeed()
    {
        float targetSpeed = walkSpeed;

        if (crouchHeld)
            targetSpeed = crouchSpeed;
        else if (sprintHeld && moveInput.sqrMagnitude > 0.01f)
            targetSpeed = runSpeed;

        float inputMagnitude = moveInput.magnitude;

        currentSpeed = Mathf.SmoothDamp(
            currentSpeed,
            targetSpeed * inputMagnitude,
            ref speedVelocity,
            speedSmoothTime
        );
    }

    void MovePhysics()
    {
        Vector2 newPos = rb.position + moveInput.normalized * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }

    void UpdateNoise()
    {
        float inputMag = moveInput.magnitude;

        if (crouchHeld)
            CurrentNoiseRadius = noiseCrouch * inputMag;
        else if (sprintHeld)
            CurrentNoiseRadius = noiseRun * inputMag;
        else
            CurrentNoiseRadius = noiseWalk * inputMag;

        if (noiseEmitter != null)
            noiseEmitter.SetNoiseRadius(CurrentNoiseRadius);
    }
    /*
    void UpdateAnimator()
    {
        if (animator == null) return;

        animator.SetFloat(animMoveX, moveInput.x);
        animator.SetFloat(animMoveY, moveInput.y);
        animator.SetFloat(animSpeed, currentSpeed);
        animator.SetBool(animSprint, sprintHeld);
        animator.SetBool(animCrouch, crouchHeld);
    }

    void OrientSprite()
    {
        if (spriteRoot == null) return;

        if (moveInput.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            spriteRoot.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }
    }*/

    public void EmitNoiseBurst(float radius)
    {
        CurrentNoiseRadius = Mathf.Max(CurrentNoiseRadius, radius);
        if (noiseEmitter != null)
            noiseEmitter.SetNoiseRadius(CurrentNoiseRadius);
    }
}
