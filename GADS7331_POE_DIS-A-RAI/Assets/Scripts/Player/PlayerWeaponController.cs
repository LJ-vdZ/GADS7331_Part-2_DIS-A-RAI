using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [Header("Weapon")]
    public GameObject playerGun;
    public Transform firePoint;
    public float fireRate = 0.15f;
    public float damage = 25f;
    public float range = 100f;

    [Header("Pickup Settings")]
    public float interactRange = 4f;
    public KeyCode pickupKey = KeyCode.E;

    private bool hasGun = false;
    private float nextFireTime = 0f;
    private Camera playerCamera;

    void Start()
    {
        // Try to find the camera
        playerCamera = Camera.main;
        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
        }

        if (playerCamera == null)
        {
            Debug.LogError("No camera found! Make sure your camera is tagged 'MainCamera' or is a child of the player.");
        }

        if (playerGun != null)
            playerGun.SetActive(false);
        else
            Debug.LogError("Player Gun is not assigned!");
    }

    void Update()
    {
        // Debug ray so you can see it in Scene view
        if (playerCamera != null)
        {
            Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactRange, Color.yellow);
        }

        if (Input.GetKeyDown(pickupKey))
        {
            TryPickupGun();
        }

        if (hasGun && Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void TryPickupGun()
    {
        // Already has a gun - don't allow another pickup
        if (hasGun) return;

        if (playerCamera == null) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            GunPickup pickup = hit.collider.GetComponent<GunPickup>();
            if (pickup == null)
                pickup = hit.collider.GetComponentInParent<GunPickup>();

            if (pickup != null)
            {
                playerGun.SetActive(true);
                hasGun = true;
                GameAudio.PlayEquipGun();
                Destroy(pickup.gameObject);
                Debug.Log("Gun picked up!");
            }
        }
    }

    void Shoot()
    {
        GameAudio.PlayGunShot();
        Vector3 origin = firePoint != null ? firePoint.position : playerCamera.transform.position;
        Vector3 direction = playerCamera.transform.forward;
        Ray ray = new Ray(playerCamera.transform.position, direction);
        Vector3 end = origin + direction * range;
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            end = hit.point;
            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy == null)
                enemy = hit.collider.GetComponentInParent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage, hit.point);
            }
        }

        CombatEffects.SpawnElectricBall(origin, end);
    }
}
