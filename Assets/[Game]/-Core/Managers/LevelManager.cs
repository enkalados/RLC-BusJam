using Base.Global.Enums;
using Base.Pool;
using Base.Utilities;
using Base.Utilities.GlobalVariable;
using Base.Utilities.SaveLoadManager;
using LevelDataSystem;
using LevelUI;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Base.Managers
{
	public class LevelManager : Singleton<LevelManager>
	{
		#region Parameters
		[SerializeField] LevelUIControl levelUIControl;
		[SerializeField] TextMeshProUGUI levelPlayText;
		[SerializeField] LevelData[] levelDatas;
		LevelTransformData currentLevelData = null;

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


			OnLevelStart.AddListener(StartLevel);
			OnNextLevel.AddListener(NextLevel);
			OnRestartLevel.AddListener(RestartLevel);
		}
		private void Start()
		{
			CreateLevelUI();
			SelectCurrentLevel();
			CheckCreateLevelEnvironment();
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
				if (currentLevel > levelDatas.Length)
				{
					currentLevel = 1;
				}
				SaveLoad.SetInt(GlobalVariables.LastLevelNumberSaveKey, currentLevel);

				OnLevelSuccess.Invoke();
				UIManager.Instance.ShowPanel(PanelID.WinPanel);
			}
			else
			{
				OnLevelFail.Invoke();
				UIManager.Instance.ShowPanel(PanelID.LosePanel);
			}

			OnLevelFinish.Invoke();
		}
		void NextLevel()
		{
			SelectCurrentLevel();
			CheckCreateLevelEnvironment();

			OnLevelStart.Invoke();
			UIManager.Instance.HideAllPanels();
			UIManager.Instance.ShowPanel(PanelID.InGamePanel);
		}
		void RestartLevel()
		{
			SelectCurrentLevel();
			CheckCreateLevelEnvironment();

			OnLevelStart.Invoke();
			UIManager.Instance.HideAllPanels();
			UIManager.Instance.ShowPanel(PanelID.InGamePanel);
		}
		void CheckCreateLevelEnvironment()
		{
			levelParent = GameObject.Find(levelParentName);
			if (levelParent == null)
			{
				CreateLevelEnvironment();
			}
			else
			{
                for (int i = 0; i < levelParent.transform.childCount; i++)
                {
					PoolingManager.Instance.DestroyPoolObject(levelParent.transform.GetChild(i).GetComponent<PoolObject>());
				}
				Destroy(levelParent);
				CreateLevelEnvironment();
			}
		}
		void CreateLevelEnvironment()
		{
			levelParent = new GameObject(levelParentName);
			levelParent.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

			currentLevelData = levelDatas.First(data => data.Level == currentLevel).LevelTransformData;

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
			levelUIControl.CreateButtons(levelDatas.Length);
		}
		void SelectCurrentLevel()
		{
			levelUIControl.SelecetCurrentLevel(currentLevel);
			levelPlayText.text = "Level " + currentLevel;
		}
		public LevelData GetCurrentLevelData()
		{
			return levelDatas[currentLevel-1];
		}
		#endregion
	}
}