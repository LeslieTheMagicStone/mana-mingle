using UnityEngine;
using UnityEngine.Events;

namespace WarriorAnimsFREE
{
	public class WarriorCharacterAnimatorEvents : MonoBehaviour
	{
		public UnityEvent OnHit = new UnityEvent();
		public UnityEvent OnFootR = new UnityEvent();
		public UnityEvent OnFootL = new UnityEvent();
		public UnityEvent OnLand = new UnityEvent();
		public UnityEvent OnShoot = new UnityEvent();
		public UnityEvent OnWeaponSwitch = new UnityEvent();

		[HideInInspector] public WarriorController warriorController;
		private WarriorMovementController movementController;

		private void Start()
		{
			movementController = GetComponent<WarriorMovementController>();
		}

		public void Hit()
		{
			OnHit.Invoke();
		}

		public void FootR()
		{
			OnFootR.Invoke();
			movementController?.PlayFootStepSound(1); // Right foot
		}

		public void FootL()
		{
			OnFootL.Invoke();
			movementController?.PlayFootStepSound(0); // Left foot
		}

		public void Land()
		{
			OnLand.Invoke();
		}

		public void Shoot()
		{
			OnShoot.Invoke();
		}
	}
}
