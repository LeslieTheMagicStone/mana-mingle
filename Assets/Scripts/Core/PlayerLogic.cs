using UnityEngine;

public class PlayerLogic : MonoBehaviour
{

    private CharacterController characterController;

    private Vector3 direction;
    private Vector3 velocity;
    private const float SPEED = 2f;


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInputRaw = Input.GetAxisRaw("Horizontal");
        float verticalInputRaw = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontalInputRaw, 0, verticalInputRaw);
        velocity.x = horizontalInput * SPEED / direction.magnitude;
        velocity.z = verticalInput * SPEED / direction.magnitude;
    }

    private void FixedUpdate()
    {
        characterController.Move(velocity * Time.fixedDeltaTime * SPEED);
    }
}
