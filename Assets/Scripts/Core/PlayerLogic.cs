using UnityEngine;

public enum PlayerID
{
    _P1,
    _P2,
}

public class PlayerLogic : MonoBehaviour
{
    public PlayerID playerID => _playerID;
    [SerializeField] private PlayerID _playerID;
    private Vector3 direction;
    private Vector3 velocity;
    private const float SPEED = 2f;

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal" + playerID.ToString());
        float verticalInput = Input.GetAxis("Vertical" + playerID.ToString());
        float horizontalInputRaw = Input.GetAxisRaw("Horizontal" + playerID.ToString());
        float verticalInputRaw = Input.GetAxisRaw("Vertical" + playerID.ToString());
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
