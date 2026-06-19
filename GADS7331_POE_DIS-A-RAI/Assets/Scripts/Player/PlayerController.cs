using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 7f;
    public float crateCarrySpeed = 2.8f;    
    public float gravity = 25f;

    public float sprintSpeed = 12f;

    [Header("Camera")]
    public Camera playerCamera;
    public float lookSpeed = 2f;

    [Header("Interaction Settings")]
    public float interactRange = 5f;
    public Transform carryAttachPoint;
    public float throwForce = 10f;

    private CharacterController characterController;
    private Vector3 moveVelocity = Vector3.zero;
    private float xRotation = 0f;

    private Transform currentPlatform = null;
    private Rigidbody carriedObject = null;
    private bool isCarrying = false;

    private bool isCameraLocked = false;

    // Crate Handling
    private Transform carriedCrate = null;
    private float crateOriginalHeight;

    //for zero gravity
    private bool isInZeroGravity = false;
    private Rigidbody playerRb;

    private float zeroGDrag = 1.2f;
    private float zeroGAngularDrag = 2f;
    private float normalMass = 1f;
    private float pushForce = 25f;

    private float zeroGMaxSpeed = 8f;        // Reduced from 18f
    private float thrustForce = 28f;         // Main forward thrust when pressing Space
    private float pushOffForce = 22f;        // Force when pushing off walls/floors

    private float surfaceDetectionDistance = 2f;

    private BlackBox carriedBlackBox = null;

    private bool movementLocked = false;

    // === NEW: Interaction Control ===
    private bool isInteractionEnabled = true;


    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>() ?? Camera.main;

        if (carryAttachPoint == null)
            carryAttachPoint = transform.Find("CarryAttachPoint");

        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
            playerRb = rb;
        }
    }

    private void Update()
    {
        //HandleMouseLook();
        HandleMovement();
        HandleInteraction();
        UpdateCarriedCrate();

    }

    private void LateUpdate()
    {
        // Always handle mouse look in LateUpdate for smoothest feel
        HandleMouseLook();
    }

    private void HandleMouseLook()
    {
        if (isCameraLocked) return;   // This line must be here

        // Normal mouse look code...
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        transform.Rotate(Vector3.up * mouseX);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    // Add this method
    public void SetMovementLocked(bool locked)
    {
        movementLocked = locked;
    }

    // ===================== NEW METHODS =====================
    public void SetInteractionEnabled(bool enabled)
    {
        isInteractionEnabled = enabled;
    }

    public bool IsInteractionEnabled() => isInteractionEnabled;
    // =======================================================

    private void HandleMovement()
    {
        if (movementLocked)
        {
            // Still allow mouse look, but no movement
            return;
        }

        if (isInZeroGravity)
        {
            HandleZeroGMovement();
            return;
        }

        // === NORMAL GRAVITY MOVEMENT ===
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        //sprint logic when not carrying crate
        bool isSprinting = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && (h != 0 || v != 0);
        float currentSpeed = (carriedCrate != null) ? crateCarrySpeed : (isSprinting ? sprintSpeed : walkSpeed);

        //float currentSpeed = (carriedCrate != null) ? crateCarrySpeed : walkSpeed;

        Vector3 desiredMove = (forward * v + right * h) * currentSpeed;

        if (characterController.isGrounded)
            moveVelocity.y = -2f;
        else
            moveVelocity.y -= gravity * Time.deltaTime;

        moveVelocity.x = desiredMove.x;
        moveVelocity.z = desiredMove.z;

        characterController.Move(moveVelocity * Time.deltaTime);

        StickToPlatform();
    }

    private void HandleZeroGMovement()
    {
        if (playerRb == null) return;

        // NO constant upward force > only very tiny drift if you want
        playerRb.AddForce(Vector3.up * 0.4f, ForceMode.Acceleration);   // You can lower to 0.2f or 0f

        // SPACEBAR = One-time push forward when near a surface
        if (Input.GetKeyDown(KeyCode.Space) && IsNearSurface())
        {
            Vector3 pushDirection = playerCamera.transform.forward;
            playerRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);

            Debug.Log("Pushed forward!");
        }

        // Limit max speed
        if (playerRb.linearVelocity.magnitude > zeroGMaxSpeed)
        {
            playerRb.linearVelocity = playerRb.linearVelocity.normalized * zeroGMaxSpeed;
        }

        KeepUpright();
    }

    private bool IsNearSurface()
    {
        Vector3 pos = transform.position;

        Vector3[] directions = { -transform.up, transform.up, -playerCamera.transform.forward, playerCamera.transform.forward, transform.right, -transform.right};

        foreach (Vector3 dir in directions)
        {
            if (Physics.Raycast(pos, dir, surfaceDetectionDistance))
            {
                Debug.DrawRay(pos, dir * surfaceDetectionDistance, Color.green, 0.2f);
                return true;
            }
        }
        return false;
    }

    private void KeepUpright()
    {
        Quaternion uprightRot = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, uprightRot, 12f * Time.deltaTime);
        playerRb.angularVelocity = Vector3.zero;
    }

    private void StickToPlatform()
    {
        if (Physics.Raycast(transform.position + Vector3.up * 0.15f, Vector3.down, out RaycastHit hit, 0.6f))
        {
            if (hit.transform.CompareTag("ElevatorPlatform"))
            {
                if (currentPlatform != hit.transform)
                {
                    currentPlatform = hit.transform;
                    transform.SetParent(currentPlatform);
                }
                return;
            }
        }
        if (currentPlatform != null)
        {
            transform.SetParent(null);
            currentPlatform = null;
        }
    }

    private void HandleInteraction()
    {
        if(!isInteractionEnabled) return;   // Prevents interaction when disabled

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isCarrying || carriedCrate != null)
                DropOrThrowObject();
            else
                TryPickupOrInteract();
        }

        if (isCarrying && carriedObject != null && carryAttachPoint != null)
        {
            carriedObject.transform.position = Vector3.Lerp(carriedObject.transform.position, carryAttachPoint.position, 20f * Time.deltaTime);
            carriedObject.transform.rotation = Quaternion.Lerp(carriedObject.transform.rotation, carryAttachPoint.rotation, 12f * Time.deltaTime);
        }
    }

    private void TryPickupOrInteract()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        int layerMask = ~LayerMask.GetMask("Player");

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, layerMask))
        {
            // === TERMINAL INTERACTION ===
            Terminal terminal = hit.collider.GetComponentInParent<Terminal>();
            if (terminal != null)
            {
                Debug.Log("Terminal detected - Calling Interact()");
                terminal.Interact();
                return;
            }

            if (hit.collider.CompareTag("Crate"))
            {
                Rigidbody crateRb = hit.collider.GetComponentInParent<Rigidbody>();
                if (crateRb != null)
                {
                    AttachCrate(hit.transform);
                    return;
                }
            }

            // === BLACK BOX PICKUP ===
            BlackBox blackBox = hit.collider.GetComponent<BlackBox>() ?? hit.collider.GetComponentInParent<BlackBox>();

            if (blackBox != null)
            {
                blackBox.TryPickup(this);
                return;
            }

            Rigidbody rb = hit.collider.GetComponentInParent<Rigidbody>();

            if (rb != null)
            {
                if (!rb.isKinematic)
                {
                    PickupObject(rb);
                    return;
                }
            }
        }
    }

    private void AttachCrate(Transform crate)
    {
        carriedCrate = crate;
        crateOriginalHeight = crate.position.y;

        Rigidbody rb = crate.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        crate.SetParent(transform);
    }

    private void UpdateCarriedCrate()
    {
        if (carriedCrate == null) return;

        Vector3 pos = carriedCrate.position;
        pos.y = crateOriginalHeight;
        carriedCrate.position = pos;
    }

    private void PickupObject(Rigidbody rb)
    {
        carriedObject = rb;
        isCarrying = true;

        rb.isKinematic = true;
        rb.useGravity = false;
        rb.transform.SetParent(carryAttachPoint);
    }

    private void DropOrThrowObject()
    {
        if (carriedCrate != null)
        {
            carriedCrate.SetParent(null);
            Rigidbody rb = carriedCrate.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
            carriedCrate = null;
            return;
        }

        if (carriedObject == null) return;

        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            PowerCellSlot slot = hit.collider.GetComponentInParent<PowerCellSlot>();
            if (slot != null)
            {
                PowerCell cell = carriedObject.GetComponent<PowerCell>();
                if (cell != null && slot.InsertCell(cell))
                {
                    carriedObject = null;
                    isCarrying = false;
                    return;
                }
            }
        }

        carriedObject.transform.SetParent(null);
        carriedObject.isKinematic = false;
        carriedObject.useGravity = true;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            carriedObject.AddForce(playerCamera.transform.forward * throwForce, ForceMode.Impulse);

        carriedObject = null;
        isCarrying = false;
    }

    //for zero gravity puzzle
    public void EnterZeroGravity()
    {
        isInZeroGravity = true;
        characterController.enabled = false;

        playerRb = GetComponent<Rigidbody>();
        if (playerRb == null)
            playerRb = gameObject.AddComponent<Rigidbody>();

        playerRb.isKinematic = false;
        playerRb.useGravity = false;
        playerRb.linearDamping = 1.5f;
        playerRb.angularDamping = 6f;
        playerRb.mass = 1f;
        playerRb.freezeRotation = true;

        // THIS IS KEY for smooth camera
        //playerRb.interpolation = RigidbodyInterpolation.Interpolate;

        playerRb.AddForce(Vector3.up * 5f, ForceMode.Impulse);

        Debug.Log("Zero-G Entered - Smooth Camera Mode");
    }

    public void ExitZeroGravity()
    {
        isInZeroGravity = false;
        characterController.enabled = true;

        if (playerRb != null)
        {
            playerRb.useGravity = true;
            playerRb.isKinematic = true;
            playerRb.linearVelocity = Vector3.zero;
        }

        SetCameraLocked(false);
        moveVelocity = Vector3.zero;

        Debug.Log("Back to normal - Camera should be smooth");
    }

    public void SetCameraLocked(bool locked)
    {
        isCameraLocked = locked;
    }

    public void PickupBlackBox(BlackBox box)
    {
        carriedBlackBox = box;

        Rigidbody rb = box.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        box.transform.SetParent(carryAttachPoint);

        box.transform.localPosition = new Vector3(0, 0.5f, 0f);   // Raised + slightly forward

        // Custom rotation: 260 degrees on X axis
        box.transform.localRotation = Quaternion.Euler(260f, -10f, 0f);

        // Optional: Slightly adjust position if needed
        // box.transform.localPosition = new Vector3(0, 0.2f, 0.3f);
    }
}
