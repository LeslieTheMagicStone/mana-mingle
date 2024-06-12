using UnityEngine;
using Unity.Netcode;

public class PlayerLogic : NetworkBehaviour
{
    [SerializeField] private Damager bulletPrefab;
    private Vector3 direction;
    private Vector3 velocity;
    private float verticalRotation = 0;
    private const float SPEED = 2f;
    private const float SENSITIVITY_X = 2.0f;
    private const float SENSITIVITY_Y = 2.0f;
    private const float Y_ROTATION_MAX = 90f;
    private const float Y_ROTATION_MIN = -90f;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) enabled = false;
    }

    private void Update()
    {
        // HandleMovement();
        // HandleRotation();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootBulletServerRpc(NetworkManager.LocalClientId);
        }
    }

    [ServerRpc]
    private void ShootBulletServerRpc(ulong shooter)
    {
        ShootBulletClientRpc(shooter);
    }

    [ClientRpc]
    private void ShootBulletClientRpc(ulong shooter)
    {
        var bullet = Instantiate(bulletPrefab, transform.position + transform.forward + transform.up, transform.rotation);
        bullet.SetOwnerId(shooter);
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInputRaw = Input.GetAxisRaw("Horizontal");
        float verticalInputRaw = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontalInputRaw, 0, verticalInputRaw);
        if (direction.magnitude == 0)
        {
            velocity.x = 0;
            velocity.z = 0;
            return;
        }
        velocity.x = horizontalInput * SPEED / direction.magnitude;
        velocity.z = verticalInput * SPEED / direction.magnitude;
    }

    private void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * SENSITIVITY_X;
        float mouseY = Input.GetAxis("Mouse Y") * SENSITIVITY_Y;

        transform.Rotate(0, mouseX, 0);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, Y_ROTATION_MIN, Y_ROTATION_MAX);

        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    private void FixedUpdate()
    {
        transform.Translate(velocity * Time.fixedDeltaTime * SPEED);
    }

    public void Teleport(Transform destination)
    {
        transform.position = destination.position;
        transform.rotation = destination.rotation;
    }
}
