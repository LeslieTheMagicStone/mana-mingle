using UnityEngine;
using Unity.Netcode;
using WarriorAnimsFREE;

public class PlayerLogic : NetworkBehaviour
{
    private Vector3 direction;
    private Vector3 velocity;
    private float verticalRotation = 0;
    private AudioSource audioSource;
    private Damageable damageable;
    private const float SPEED = 2f;
    private const float SENSITIVITY_X = 2.0f;
    private const float SENSITIVITY_Y = 2.0f;
    private const float Y_ROTATION_MAX = 90f;
    private const float Y_ROTATION_MIN = -90f;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) enabled = false;

        audioSource = GetComponent<AudioSource>();
        damageable = GetComponent<Damageable>();
        damageable.onDeath.AddListener(OnDeath);
    }

    public void Cast(SpellVariant spellVariant)
    {
        CastServerRpc(NetworkManager.LocalClientId, spellVariant);
    }

    private void OnDeath()
    {
        GetComponent<Animator>().SetTrigger("Death");
        GetComponent<WarriorInputController>().enabled = false;
        GetComponent<WarriorMovementController>().enabled = false;
        GetComponent<WarriorController>().enabled = false;
        GetComponent<SuperCharacterController>().enabled = false;
        GetComponent<AnimatorParentMove>().enabled = false;
        GetComponent<WarriorTiming>().enabled = false;
        GetComponent<PickerLogic>().enabled = false;
    }

    [ServerRpc]
    private void CastServerRpc(ulong shooter, SpellVariant spellVariant)
    {
        print("ServerRPC reached");
        CastClientRpc(shooter, spellVariant);
    }

    [ClientRpc]
    private void CastClientRpc(ulong shooter, SpellVariant spellVariant)
    {
        print("ClientRPC reached");
        var spell = GameLogic.instance.spellLibrary[(int)spellVariant];
        audioSource.PlayOneShot(spell.audioClip);
        if (spell.spellType == SpellType.Stay)
        {
            var proj = (ProjectileSpell)spell;
            var spawnPoint = proj.spawnPoint;
            var pos = transform.TransformPoint(spawnPoint.localPosition);
            var rot = transform.rotation * spawnPoint.localRotation;
            var bullet = Instantiate(proj.projectilePrefab, pos, rot);
            bullet.gameObject.SetActive(true);
            bullet.transform.SetParent(transform);
            bullet.SetOwnerId(shooter);
        }
        if (spell.spellType == SpellType.Shoot)
        {
            var proj = (ProjectileSpell)spell;
            var spawnPoint = proj.spawnPoint;
            var pos = transform.TransformPoint(spawnPoint.localPosition);
            var rot = transform.rotation * spawnPoint.localRotation;
            var bullet = Instantiate(proj.projectilePrefab, pos, rot);
            bullet.gameObject.SetActive(true);
            bullet.transform.SetParent(transform);
            bullet.GetComponent<ProjectileBase>().Init(proj.projectileSpeed);
            bullet.SetOwnerId(shooter);
        }
    }

    private void Update()
    {
        // HandleMovement();
        // HandleRotation();
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
