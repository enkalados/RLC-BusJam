using Base.Global.Enums;
using Base.Pool;
using GridSystem;
using GridSystem.Editor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace LevelDataSystem.Editor
{
	public class LevelSaveLoadEditorWindow : EditorWindow
	{
		#region Variables
		List<LevelData> levelData = new List<LevelData>();
		GameObject levelParent = null;
		string levelParentName = "LEVEL";
		int selectedLevelIndex = 0;
		string levelDataPath = "Assets/[Game]/Data/LevelData";

		PoolID[] dontSaveWithEnvironment = { PoolID.Tile, PoolID.Stickman };

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
			if (levelData.Count == 0)
			{
				EditorGUILayout.HelpBox("Not found Level Data!", MessageType.Warning);
				return;
			}
			selectedLevelIndex = EditorGUILayout.Popup("Selected Level:", selectedLevelIndex, GetLevelNames());

			if (levelData.Count > 0 && selectedLevelIndex < levelData.Count)
			{
				LevelTransformData selectedLevel = levelData[selectedLevelIndex].LevelTransformData;

				EditorGUILayout.Space();

				if (GUILayout.Button("Clear & Preview Level"))
				{
					ClearEndPreviewLevel(selectedLevel);
				}
				EditorGUILayout.Space(15);
				if (GUILayout.Button("Save Level"))
				{
					ShowWarningPopup(selectedLevel);
				}
				GUILayout.BeginHorizontal();
				if (GUILayout.Button("Edit Tiles"))
				{
					GridEditorWindow.ShowWindow(levelData[selectedLevelIndex].TilesData);
				}
				if (GUILayout.Button("Edit Stickman Area"))
				{
					GridEditorWindow.ShowWindow(levelData[selectedLevelIndex].StickmansTileData);
				}
				GUILayout.EndHorizontal();

				EditorGUILayout.Space(15);

				if (GUILayout.Button("Clear Level"))
				{
					ClearLevel();
				}

				EditorGUILayout.Space(15);

				GUILayout.BeginHorizontal();
				if (GUILayout.Button("Go to level data"))
				{
					EditorGUIUtility.PingObject(levelData[selectedLevelIndex]);
				}
				if (GUILayout.Button("Go to pool data"))
				{
					EditorGUIUtility.PingObject(poolDatabase);
				}
				GUILayout.EndHorizontal();
			}
		}
		#endregion
		#region Level Data Methods
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
						if (!dontSaveWithEnvironment.Contains(element.PoolID))
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
				EditorUtility.SetDirty(selectedLevel);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
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
			FindObjectOfType<TileCreator>().SetTileGridDataEditor(levelData[selectedLevelIndex].TilesData, GetPoolObject(PoolID.Tile, poolDatabase));
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
			levelData.Clear();
			string[] levelDatas = AssetDatabase.FindAssets("t:LevelData", new[] { levelDataPath });

			foreach (string item in levelDatas)
			{
				string path = AssetDatabase.GUIDToAssetPath(item);
				LevelData data = AssetDatabase.LoadAssetAtPath<LevelData>(path);

				if (data != null)
					levelData.Add(data);
			}

			if (selectedLevelIndex >= levelData.Count)
				selectedLevelIndex = 0;
		}
		private string[] GetLevelNames()
		{
			string[] names = new string[levelData.Count];
			for (int i = 0; i < levelData.Count; i++)
			{
				names[i] = "Level " + levelData[i].Level;
			}
			return names;
		}
		#endregion
		#region Warning Popup
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
		#endregion
	}
}