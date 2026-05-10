using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 6f;
    public float crateCarrySpeed = 2.8f;     // Added: Slower speed when carrying crate
    public float gravity = 25f;              // eAdded: Was missing

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

    // Crate Handling
    private Transform carriedCrate = null;
    private float crateOriginalHeight;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>() ?? Camera.main;

        if (carryAttachPoint == null)
            carryAttachPoint = transform.Find("CarryAttachPoint");

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;
    }

    private void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleInteraction();
        UpdateCarriedCrate();
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Use reduced speed when carrying crate
        float currentSpeed = (carriedCrate != null) ? crateCarrySpeed : walkSpeed;

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
            if (hit.collider.CompareTag("Crate"))
            {
                Rigidbody crateRb = hit.collider.GetComponentInParent<Rigidbody>();
                if (crateRb != null)
                {
                    AttachCrate(hit.transform);
                    return;
                }
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
}
