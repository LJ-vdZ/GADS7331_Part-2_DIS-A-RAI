using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 6f;
    public float gravity = 25f;

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
        Vector3 desiredMove = (forward * v + right * h) * walkSpeed;

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
            if (isCarrying)
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
            Debug.Log($"Hit: {hit.collider.gameObject.name} | Tag: {hit.collider.tag}");

            // === CRATE CHECK (Added - does NOT affect power cell logic) ===
            if (hit.collider.CompareTag("Crate"))
            {
                Rigidbody crateRb = hit.collider.GetComponentInParent<Rigidbody>();
                if (crateRb != null)
                {
                    Debug.Log("Crate detected - picking up");
                    PickupObject(crateRb);   // Reuse your existing method
                    return;
                }
            }

            // === YOUR ORIGINAL POWER CELL LOGIC (Completely Unchanged) ===
            Rigidbody rb = hit.collider.GetComponentInParent<Rigidbody>();

            if (rb != null)
            {
                Debug.Log($"Rigidbody found | Kinematic: {rb.isKinematic} | Tag: {hit.collider.tag}");

                if (!rb.isKinematic)
                {
                    Debug.Log("Picking up object!");
                    PickupObject(rb);
                    return;
                }
            }
        }
        else
        {
            Debug.Log("Raycast hit nothing");
        }
    }

    private void PickupObject(Rigidbody rb)
    {
        carriedObject = rb;
        isCarrying = true;

        rb.isKinematic = true;
        rb.useGravity = false;
        rb.transform.SetParent(carryAttachPoint);

        Debug.Log("Object successfully picked up and attached!");
    }

    private void DropOrThrowObject()
    {
        if (carriedObject == null) return;

        // Check if we're looking at a PowerCellSlot
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            PowerCellSlot slot = hit.collider.GetComponentInParent<PowerCellSlot>();
            if (slot != null)
            {
                PowerCell cell = carriedObject.GetComponent<PowerCell>();
                if (cell != null)
                {
                    if (slot.InsertCell(cell))
                    {
                        carriedObject = null;
                        isCarrying = false;
                        return;
                    }
                }
            }
        }

        // Normal drop / throw
        carriedObject.transform.SetParent(null);
        carriedObject.isKinematic = false;
        carriedObject.useGravity = true;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            carriedObject.AddForce(playerCamera.transform.forward * throwForce, ForceMode.Impulse);

        carriedObject = null;
        isCarrying = false;
    }
}
