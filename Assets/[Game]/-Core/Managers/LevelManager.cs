using Base.Global.Enums;
using Base.Pool;
using Base.Utilities;
using Base.Utilities.GlobalVariable;
using Base.Utilities.SaveLoadManager;
using LevelSaveSystem;
using LevelUI;
using System.Collections.Generic;
using System.Linq;
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
		LevelTransformData currentLevelData = null;
		string levelEnvironmentsPath = "Assets/[Game]/Data/LevelEnvironmentDatas";
		GameObject levelParent = null;
		string levelParentName = "LEVEL";

		PoolObject getObject;

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
			CreateLevel();

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
				if (currentLevel > levelDataList.Count)
				{
					currentLevel = 1;
				}
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
			SelectCurrentLevel();
			CreateLevel();
		}
		void RestartLevel()
		{
			SelectCurrentLevel();
			ReloadLevel();
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
		void CreateLevel()
		{
			levelParent = GameObject.Find(levelParentName);
			if(levelParent != null)
			{
				Destroy(levelParent);
			}

			levelParent = new GameObject(levelParentName);
			levelParent.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

			currentLevelData = levelDataList.First(data => data.Level == currentLevel);

			for (int i = 0; i < currentLevelData.LevelTransforms.Count; i++)
			{
				try
				{
					PoolObject item = PoolingManager.Instance.Instantiate(currentLevelData.LevelTransforms[i].PoolID, currentLevelData.LevelTransforms[i].Position, currentLevelData.LevelTransforms[i].Rotation);
					item.transform.SetParent(levelParent.transform);
					item.transform.localScale = currentLevelData.LevelTransforms[i].Scale;
				}
				catch (System.Exception)
				{
					Debug.LogWarning("Level Object not found in pool");
					throw;
				}
			}
		}
		void ReloadLevel()
		{
			for (int i = 0; i < currentLevelData.LevelTransforms.Count; i++)
			{
				getObject = GetLevelObject(currentLevelData.LevelTransforms[i].PoolID);
				getObject.transform.SetPositionAndRotation(currentLevelData.LevelTransforms[i].Position, currentLevelData.LevelTransforms[i].Rotation);
				getObject.transform.localScale = currentLevelData.LevelTransforms[i].Scale;
			}
		}
		PoolObject GetLevelObject(PoolID poolID)
		{
			foreach (Transform item in levelParent.transform)
			{
				if (item.gameObject.TryGetComponent(out PoolObject poolObject))
				{
					if (poolObject.PoolID == poolID)
					{
						return poolObject;
					}
				}
			}
			return null;
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