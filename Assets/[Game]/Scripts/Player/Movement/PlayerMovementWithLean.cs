using UnityEngine;
using Base.Managers;
namespace Player.Movement
{
	public class PlayerMovementWithLean : LeanDragTranslateWithClamp
	{
		#region Variables
		bool isControllable = false;
		float currentSpeed;
		#endregion
		#region Properties 

		#endregion
		#region MonoBehaviour Methods
		private void OnEnable()
		{
			currentSpeed = MoveSpeed;
			LevelManager.OnLevelStart.AddListener(() => SetPlayerControl(true));
			LevelManager.OnLevelFinish.AddListener(() => SetPlayerControl(false));
		}
		private void OnDisable()
		{
			LevelManager.OnLevelStart.RemoveListener(() => SetPlayerControl(true));
			LevelManager.OnLevelFinish.RemoveListener(() => SetPlayerControl(false));
		}
		protected override void Update()
		{
			if (!isControllable)
				return;

			PlayerMove();
			base.Update();
		}
		#endregion
		#region My Methods
		void PlayerMove()
		{
			transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
		}
		public void SetPlayerControl(bool isControllable)
		{
			this.isControllable = isControllable;
		}
		#endregion
	}
}