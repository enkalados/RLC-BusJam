using Base.Utilities;
using Base.Utilities.GlobalVariable;
using Base.Utilities.SaveLoadManager;
using UnityEngine;
using UnityEngine.Events;

namespace Base.Managers
{
	public class LevelManager : Singleton<LevelManager>
	{
		#region Parameters
		[SerializeField] LevelData[] levels;
		int currentLevel;
		bool isLevelStarted = false;

		#endregion
		#region Events
		public static UnityEvent OnLevelSuccess = new UnityEvent();
		public static UnityEvent OnLevelFail = new UnityEvent();
		public static UnityEvent OnLevelStart = new UnityEvent();
		public static UnityEvent OnLevelFinish = new UnityEvent();
		public static UnityEvent OnNextLevel = new UnityEvent();
		public static UnityEvent OnRestartLevel = new UnityEvent();
		#endregion
		#region Variables

		#endregion
		#region MonoBehaviour Methods
		private void OnEnable()
		{
			currentLevel = SaveLoad.GetInt(GlobalVariables.LastLevelNumberSaveKey, 1);

			OnLevelStart.AddListener(StartLevel);
			OnNextLevel.AddListener(NextLevel);
			OnRestartLevel.AddListener(RestartLevel);
		}
		private void OnDisable()
		{
			OnLevelStart.RemoveListener(StartLevel);
			OnNextLevel.RemoveListener(NextLevel);
			OnRestartLevel.RemoveListener(RestartLevel);
		}
		#endregion
		#region My Methods
		public bool GetLevelIsStarted()
		{
			return isLevelStarted;
		}
		void StartLevel()
		{
			if (isLevelStarted) return;
			isLevelStarted = true;
		}
		public void CompleteLevel(bool isSuccess)
		{
			if (!isLevelStarted) return;
			isLevelStarted = false;
			UIManager.Instance.HideAllPanels();

			if (isSuccess)
			{
				currentLevel += 1;
				SaveLoad.SetInt(GlobalVariables.LastLevelNumberSaveKey, currentLevel);

				OnLevelSuccess.Invoke();
				UIManager.Instance.ShowPanel(Global.Enums.PanelID.WinPanel);
			}
			else
			{
				OnLevelFail.Invoke();
				UIManager.Instance.ShowPanel(Global.Enums.PanelID.LosePanel);
			}

			OnLevelFinish.Invoke();
		}

		void NextLevel()
		{
			//
		}
		void RestartLevel()
		{
			//
		}
		#endregion
	}
	[System.Serializable]
	public class LevelData
	{
		public int LevelNumber;
		public int SceneIndex;

	}
}