using Base.Utilities;
using UnityEngine.Events;


namespace Base.Managers
{
	public class GameManager : Singleton<GameManager>
	{
		#region Events
		public static UnityEvent OnGameStart = new UnityEvent();
		public static UnityEvent OnGameFinish = new UnityEvent();
		#endregion
		#region Parameters

		#endregion
		#region Properties 

		#endregion
		#region MonoBehaviour Methods
		private void Start()
		{
			OnGameStart.Invoke();
		}
		#endregion
		#region My Methods

		#endregion
	}
}