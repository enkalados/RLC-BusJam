using Base.Global.Enums;
using Base.Pool;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace LevelSaveSystem
{
	public class LevelSaveLoadEditorWindow : EditorWindow
	{
		#region Variables
		List<LevelTransformData> levelDataList = new List<LevelTransformData>();
		GameObject levelParent = null;
		string levelParentName = "LEVEL";
		int selectedIndex = 0;
		string levelEnvironmentsPath = "Assets/[Game]/Data/LevelEnvironmentDatas";
		string poolPath = "PoolObjects";
		PoolData poolDatabase;
		#endregion
		#region Properties 

		#endregion
		#region Editor Methods
		[MenuItem("Tools/Level Editor Manager")]
		public static void ShowWindow()
		{
			LevelSaveLoadEditorWindow window = GetWindow<LevelSaveLoadEditorWindow>("Level Editor Window");
			window.minSize = new Vector2(350, 200);

			GetWindow<LevelSaveLoadEditorWindow>("Level Selector");
		}
		private void OnEnable()
		{
			LoadLevelData();
			poolDatabase = Resources.Load<PoolData>(poolPath);
		}
		private void OnGUI()
		{
			if (levelDataList.Count == 0)
			{
				EditorGUILayout.HelpBox("Not found LevelTransformData!", MessageType.Warning);
				return;
			}

			selectedIndex = EditorGUILayout.Popup("Selected Environment:", selectedIndex, GetLevelNames());

			if (levelDataList.Count > 0 && selectedIndex < levelDataList.Count)
			{
				LevelTransformData selectedLevel = levelDataList[selectedIndex];

				EditorGUILayout.Space();

				if (GUILayout.Button("Clear & Preview Level"))
				{
					ClearEndPreviewLevel(selectedLevel);
				}
				if (GUILayout.Button("Clear Level"))
				{
					ClearLevel();
				}
				if (GUILayout.Button("Save Level"))
				{
					ShowWarningPopup(selectedLevel);
				}

				EditorGUILayout.Space(15);

				GUILayout.BeginHorizontal();
				if (GUILayout.Button("Go to level data"))
				{
					EditorGUIUtility.PingObject(selectedLevel);
				}
				if (GUILayout.Button("Go to pool data"))
				{
					EditorGUIUtility.PingObject(poolDatabase);
				}
				GUILayout.EndHorizontal();
			}
		}
		#endregion
		#region Methods
		private void ClearLevel()
		{
			levelParent = GameObject.Find(levelParentName);
			if (levelParent != null)
			{
				DestroyImmediate(levelParent);
			}
			else
			{
				Debug.LogWarning("There is no Level in the scene");
			}
		}
		private void SaveLevel(LevelTransformData selectedLevel)
		{
			levelParent = GameObject.Find(levelParentName);
			if (levelParent != null)
			{
				selectedLevel.LevelTransforms.Clear();
				LevelTransform levelTransform = new LevelTransform();

				for (int i = 0; i < levelParent.transform.childCount; i++)
				{
					if (levelParent.transform.GetChild(i).TryGetComponent(out PoolObject element))
					{
						LevelTransform newLevelTransform = new LevelTransform();
						newLevelTransform.PoolID = element.PoolID;
						newLevelTransform.Position = element.transform.position;
						newLevelTransform.Rotation = element.transform.localRotation;
						newLevelTransform.Scale = element.transform.localScale;
						selectedLevel.LevelTransforms.Add(newLevelTransform);
					}
				}
			}
			else
			{
				Debug.LogWarning("There is no Level in the scene");
			}
		}
		private void ClearEndPreviewLevel(LevelTransformData selectedLevel)
		{
			ClearLevel();

			levelParent = GameObject.Find(levelParentName);
			if (levelParent == null)
			{
				levelParent = new GameObject(levelParentName);
				levelParent.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
			}

			poolDatabase = Resources.Load<PoolData>(poolPath);

			for (int i = 0; i < selectedLevel.LevelTransforms.Count; i++)
			{
				try
				{
					PoolObject item = (PoolObject)PrefabUtility.InstantiatePrefab(GetPoolObject(selectedLevel.LevelTransforms[i].PoolID, poolDatabase));

					item.transform.SetParent(levelParent.transform);
					item.transform.SetPositionAndRotation(selectedLevel.LevelTransforms[i].Position, selectedLevel.LevelTransforms[i].Rotation);
					item.transform.localScale = selectedLevel.LevelTransforms[i].Scale;
				}
				catch (System.Exception)
				{
					Debug.LogWarning("Level Object not found in pool");
					throw;
				}
			}
		}
		private void ShowWarningPopup(LevelTransformData selectedLevel)
		{
			bool continueAction = EditorUtility.DisplayDialog(
				"WARNING",
				"There is data for the selected level. If you continue, existing data will be deleted and new data will be created. Do you want to continue?",
				"Clear And Save",     
				"Cancel" 
			);

			if (continueAction)
			{
				SaveLevel(selectedLevel);
			}
		}
		PoolObject GetPoolObject(PoolID poolID, PoolData poolDatabase)
		{
			foreach (Pool item in poolDatabase.PoolHolder)
			{
				if (item.Prefab.PoolID == poolID)
				{
					return item.Prefab;
				}
			}
			return null;
		}
		private void LoadLevelData()
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

			if (selectedIndex >= levelDataList.Count)
				selectedIndex = 0;
		}
		private string[] GetLevelNames()
		{
			string[] names = new string[levelDataList.Count];
			for (int i = 0; i < levelDataList.Count; i++)
			{
				names[i] = levelDataList[i].name;
			}
			return names;
		}
		#endregion
	}
}