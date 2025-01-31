using Base.Utilities;
using Base.Utilities.GlobalVariable;
using Base.Utilities.SaveLoadManager;
using LevelSaveSystem;
using LevelUI;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Base.Managers
{
	public class LevelManager : Singleton<LevelManager>
	{
		#region Parameters
		[SerializeField] LevelUIControl levelUIControl;
		[SerializeField] TextMeshProUGUI levelPlayText;
		[SerializeField] List<LevelTransformData> levelDataList = new List<LevelTransformData>();
		string levelEnvironmentsPath = "Assets/[Game]/Data/LevelEnvironmentDatas";

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
			LoadLevelData();
			CreateLevelUI();
			SelectCurrentLevel();

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
		public void LoadLevelData()
		{
			levelDataList.Clear();

			string[] levels = AssetDatabase.FindAssets("t:LevelTransformData", new[] { levelEnvironmentsPath });

			foreach (string item in levels)
			{
				string path = AssetDatabase.GUIDToAssetPath(item);
				LevelTransformData data = AssetDatabase.LoadAssetAtPath<LevelTransformData>(path);

				if (data != null)
					levelDataList.Add(data);
			}
		}
		void CreateLevelUI()
		{
			levelUIControl.CreateButtons(levelDataList.Count);
		}
		void SelectCurrentLevel()
		{
			levelUIControl.SelecetCurrentLevel(currentLevel);
			levelPlayText.text = "Level " + currentLevel;
		}
		#endregion
	}
}