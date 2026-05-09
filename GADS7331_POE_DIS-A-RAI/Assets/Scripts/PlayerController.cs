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

    private CharacterController characterController;
    private Vector3 moveVelocity = Vector3.zero;
    private float xRotation = 0f;

    private Transform currentPlatform = null;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>() ?? Camera.main;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;
    }

    private void Update()
    {
        HandleMouseLook();
        HandleMovement();
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

        // Gravity
        if (characterController.isGrounded)
            moveVelocity.y = -2f;
        else
            moveVelocity.y -= gravity * Time.deltaTime;

        moveVelocity.x = desiredMove.x;
        moveVelocity.z = desiredMove.z;

        characterController.Move(moveVelocity * Time.deltaTime);

        // Stick to platform
        StickToPlatform();
    }

    private void StickToPlatform()
    {
        // Raycast down to detect platform
        if (Physics.Raycast(transform.position + Vector3.up * 0.15f, Vector3.down, out RaycastHit hit, 0.5f))
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

        // Release if no longer on platform
        if (currentPlatform != null)
        {
            transform.SetParent(null);
            currentPlatform = null;
        }
    }

}
