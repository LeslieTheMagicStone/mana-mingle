using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    private Vector3 direction;
    private Vector3 velocity;
    private const float SPEED = 2f;

    private void Update()
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
