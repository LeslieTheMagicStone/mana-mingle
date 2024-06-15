using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WarriorAnimsFREE
{
	public class WarriorMovementController : SuperStateMachine
	{

		[SerializeField]
		List<AudioClip> m_runStoneSounds = new List<AudioClip>();

		[SerializeField]
		Transform m_leftFoot;

		[SerializeField]
		Transform m_rightFoot;
		[Header("Components")]
		private WarriorController warriorController;

		[Header("Movement")]
		public float movementAcceleration = 90.0f;
		public float runSpeed = 6f;
		public float groundFriction = 50f;
		[HideInInspector] public Vector3 currentVelocity;

		[Header("Jumping")]
		public float gravity = 25.0f;
		public float jumpAcceleration = 5.0f;
		public float jumpHeight = 3.0f;
		public float inAirSpeed = 6f;

		private float rotationX = 0f;
		private float rotationY = 0f;

		private const float MOUSE_SENSITIVITY_X = 0.5f;
		private const float MOUSE_SENSITIVITY_Y = 0.2f;
		AudioSource m_audioSource;

		private void Start()
		{
			warriorController = GetComponent<WarriorController>();
			m_audioSource = GetComponent<AudioSource>();

			// Set currentState to idle on startup.
			currentState = WarriorState.Idle;
		}

		public void PlayFootStepSound(int footIndex)
		{
			PlayRandomSound(m_runStoneSounds); // 直接播放音效，无需检测地形类型
		}

		void PlayRandomSound(List<AudioClip> audioClips)
		{
			int soundIndex = Random.Range(0, audioClips.Count);
			if (m_audioSource && audioClips.Count > soundIndex)
			{
				m_audioSource.PlayOneShot(audioClips[soundIndex]);

				// Debug log
				Debug.Log("Playing sound: " + audioClips[soundIndex].name);
			}
		}

		#region Updates

		/*void Update () {
		 * Update is normally run once on every frame update. We won't be using it in this case, since the SuperCharacterController component sends a callback Update called SuperUpdate. 
		 * SuperUpdate is recieved by the SuperStateMachine, and then fires further callbacks depending on the state.
		}*/

		// Put any code in here you want to run BEFORE the state's update function. 
		// This is run regardless of what state you're in.
		protected override void EarlyGlobalSuperUpdate()
		{
		}

		// Put any code in here you want to run AFTER the state's update function.  
		// This is run regardless of what state you're in.
		protected override void LateGlobalSuperUpdate()
		{
			// Move the player by our velocity every frame.
			transform.position += currentVelocity * warriorController.superCharacterController.deltaTime;

			// Calculate the local velocity
			Vector3 localVelocity = transform.InverseTransformDirection(currentVelocity) / runSpeed;

			// If alive and is moving, set animator.
			if (warriorController.canMove)
			{
				if (localVelocity.magnitude > 0 && warriorController.HasMoveInput())
				{
					warriorController.isMoving = true;
					warriorController.SetAnimatorBool("Moving", true);
					warriorController.SetAnimatorFloat("Horizontal", localVelocity.x);
					warriorController.SetAnimatorFloat("Velocity", localVelocity.z);
				}
				else
				{
					warriorController.isMoving = false;
					warriorController.SetAnimatorBool("Moving", false);
					warriorController.SetAnimatorFloat("Horizontal", 0);
					warriorController.SetAnimatorFloat("Velocity", 0);
				}
			}

			HandleRotation();
		}

		#endregion

		#region Gravity / Jumping

		// public void RotateGravity(Vector3 up)
		// {
		// 	lookDirection = Quaternion.FromToRotation(transform.up, up) * lookDirection;
		// }

		// Calculate the initial velocity of a jump based off gravity and desired maximum height attained.
		private float CalculateJumpSpeed(float jumpHeight, float gravity)
		{
			return Mathf.Sqrt(2 * jumpHeight * gravity);
		}

		#endregion

		#region States

		// Below are the state functions. 
		// Each one is called based on the name of the state, so when currentState = Idle, we call Idle_EnterState. 
		// If currentState = Jump, we call Jump_SuperUpdate()
		private void Idle_EnterState()
		{
			warriorController.superCharacterController.EnableSlopeLimit();
			warriorController.superCharacterController.EnableClamping();
			warriorController.LockJump(false);
			warriorController.SetAnimatorInt("Jumping", 0);
			warriorController.SetAnimatorBool("Moving", false);
		}

		// Run every frame we are in the idle state.
		private void Idle_SuperUpdate()
		{
			// If Jump.
			if (warriorController.canJump && warriorController.inputJump)
			{
				currentState = WarriorState.Jump;
				return;
			}
			// In air.
			if (!warriorController.MaintainingGround())
			{
				currentState = WarriorState.Fall;
				return;
			}
			if (warriorController.HasMoveInput() && warriorController.canMove)
			{
				currentState = WarriorState.Move;
				return;
			}
			// Apply friction to slow to a halt.
			currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, groundFriction
				* warriorController.superCharacterController.deltaTime);
		}

		// Run once when exit the idle state.
		private void Idle_ExitState()
		{
		}

		// Run once when exit the idle state.
		private void Idle_MoveState()
		{
			warriorController.SetAnimatorBool("Moving", true);
		}

		private void Move_SuperUpdate()
		{
			// If Jump.
			if (warriorController.canJump && warriorController.inputJump)
			{
				currentState = WarriorState.Jump;
				return;
			}
			// Fallling.
			if (!warriorController.MaintainingGround())
			{
				currentState = WarriorState.Fall;
				return;
			}
			// Set speed determined by movement type.
			if (warriorController.HasMoveInput() && warriorController.canMove)
			{
				currentVelocity = Vector3.MoveTowards(currentVelocity, warriorController.moveInput
					* runSpeed, movementAcceleration
					* warriorController.superCharacterController.deltaTime);
			}
			else
			{
				currentState = WarriorState.Idle;
			}
		}

		private void Jump_EnterState()
		{
			warriorController.SetAnimatorInt("Jumping", 1);
			warriorController.SetAnimatorTrigger(AnimatorTrigger.JumpTrigger);
			warriorController.superCharacterController.DisableClamping();
			warriorController.superCharacterController.DisableSlopeLimit();
			currentVelocity += warriorController.superCharacterController.up * CalculateJumpSpeed(jumpHeight, gravity);
			warriorController.LockJump(true);
			warriorController.Jump();
		}

		private void Jump_SuperUpdate()
		{
			Vector3 planarMoveDirection = Math3d.ProjectVectorOnPlane(warriorController.superCharacterController.up, currentVelocity);
			Vector3 verticalMoveDirection = currentVelocity - planarMoveDirection;

			// Falling.
			if (currentVelocity.y < 0)
			{
				currentVelocity = planarMoveDirection;
				currentState = WarriorState.Fall;
				return;
			}

			planarMoveDirection = Vector3.MoveTowards(planarMoveDirection, warriorController.moveInput * inAirSpeed, jumpAcceleration * warriorController.superCharacterController.deltaTime);
			verticalMoveDirection -= warriorController.superCharacterController.up * gravity * warriorController.superCharacterController.deltaTime;
			currentVelocity = planarMoveDirection + verticalMoveDirection;
		}

		private void Fall_EnterState()
		{
			warriorController.superCharacterController.DisableClamping();
			warriorController.superCharacterController.DisableSlopeLimit();
			warriorController.LockJump(false);
			warriorController.SetAnimatorInt("Jumping", 2);
			warriorController.SetAnimatorTrigger(AnimatorTrigger.JumpTrigger);
		}

		private void Fall_SuperUpdate()
		{
			// Landing.
			if (warriorController.AcquiringGround())
			{
				currentVelocity = Math3d.ProjectVectorOnPlane(warriorController.superCharacterController.up, currentVelocity);
				currentState = WarriorState.Idle;
				return;
			}

			// Normal gravity.
			currentVelocity -= warriorController.superCharacterController.up * gravity * warriorController.superCharacterController.deltaTime;
		}

		private void Fall_ExitState()
		{
			warriorController.SetAnimatorInt("Jumping", 0);
			warriorController.SetAnimatorTrigger(AnimatorTrigger.JumpTrigger);

			// Landed.
			if (warriorController.AcquiringGround()) { warriorController.Land(); }
		}

		#endregion

		/// <summary>
		/// Rotate towards the direction the Warrior is moving.
		/// </summary>
		private void HandleRotation()
		{
			// transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(warriorController.moveInput), Time.deltaTime * rotationSpeed);
			// Handle mouse look
			if (Cursor.lockState != CursorLockMode.Locked) return;

			float mouseX = Input.GetAxis("Mouse X") * MOUSE_SENSITIVITY_X;
			float mouseY = Input.GetAxis("Mouse Y") * MOUSE_SENSITIVITY_Y;

			rotationX -= mouseY;
			rotationY += mouseX;

			rotationX = Mathf.Clamp(rotationX, -60, 60);

			transform.localRotation = Quaternion.Euler(0, rotationY, 0);
			Camera.main.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
		}
	}
}
