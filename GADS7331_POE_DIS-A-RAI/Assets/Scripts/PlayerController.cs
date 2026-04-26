using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 6f;

    public float lookSpeed = 2f;

    public float gravity = 20f;

    [Header("Camera")]
    public Camera playerCamera;

    private CharacterController characterController;

    private Vector3 moveDir = Vector3.zero;

    private float xRotation = 0f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        
        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();

            if (playerCamera == null) 
            {
                playerCamera = Camera.main;
            }
                
        }
    }

    private void Update()
    {
        if (playerCamera == null) return;

        //mouse Look
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;

        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        transform.Rotate(Vector3.up * mouseX);                    //rotates player left or right 

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);   //rotates camera up or down

        //player movement
        float h = Input.GetAxis("Horizontal");

        float v = Input.GetAxis("Vertical");

        Vector3 forward = transform.TransformDirection(Vector3.forward);

        Vector3 right = transform.TransformDirection(Vector3.right);

        moveDir = (forward * v + right * h) * walkSpeed;

        if (!characterController.isGrounded) 
        {
            moveDir.y -= gravity * Time.deltaTime;
        }
            

        characterController.Move(moveDir * Time.deltaTime);
    }
}
