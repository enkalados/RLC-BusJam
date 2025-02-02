using Base.Global.Enums;
using Base.Pool;
using BusSystem.Creator;
using GridSystem;
using GridSystem.Editor;
using Stickman.Creator;
using System;
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

		PoolID[] dontSaveWithEnvironment = { PoolID.Tile, PoolID.Stickman, PoolID.Bus1, PoolID.PlaceHolderTile };
		string poolPath = "PoolObjects";
		PoolData poolDatabase;

		int placeHoldersCount;
		List<Colors> busColors = new List<Colors>();
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
				#region Environment Settings
				if (GUILayout.Button("Clear & Preview Level"))
				{
					ClearEndPreviewLevel(selectedLevel);
				}

				if (GUILayout.Button("Clear Level"))
				{
					ClearLevel();
				}
				if (GUILayout.Button("Save Level Environment"))
				{
					ShowWarningPopup(selectedLevel);
				}

				EditorGUILayout.Space(10);
				Rect group1Rect = GUILayoutUtility.GetRect(position.width, 8);
				EditorGUI.DrawRect(group1Rect, Color.black);
				EditorGUILayout.Space(10);
				#endregion
				#region Grid Settings
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
				#endregion
				#region Other Settings
				placeHoldersCount = EditorGUILayout.IntField("Place Holder Count", placeHoldersCount);

				EditorGUILayout.LabelField("Bus Colors", EditorStyles.boldLabel);

				for (int i = 0; i < busColors.Count; i++)
				{
					EditorGUILayout.BeginHorizontal();

					busColors[i] = (Colors)EditorGUILayout.EnumPopup($"Bus {i + 1}", busColors[i]);

					if (GUILayout.Button("X", GUILayout.Width(20)))
					{
						busColors.RemoveAt(i);
					}

					EditorGUILayout.EndHorizontal();
				}
				if (GUILayout.Button("+ Add Bus"))
				{
					busColors.Add(Colors.None);
				}
				EditorGUILayout.Space(10);
				if (GUILayout.Button("Save Bus & Place Holders"))
				{
					SaveBusAndPlaceHolder();
				}
				EditorGUILayout.Space(10);
				Rect group2Rect = GUILayoutUtility.GetRect(position.width, 8);
				EditorGUI.DrawRect(group2Rect, Color.black);
				EditorGUILayout.Space(10);
				#endregion
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
		private void SaveLevelEnvironment(LevelTransformData selectedLevel)
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
			FindObjectOfType<StickmanCreator>().SetStickmanDataEditor(levelData[selectedLevelIndex].StickmansTileData, GetPoolObject(PoolID.Stickman, poolDatabase));
			FindAnyObjectByType<BusCreator>().SetBusDataEditor(levelData[selectedLevelIndex].BusColorList, GetPoolObject(PoolID.Bus1, poolDatabase));
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

			placeHoldersCount = levelData[selectedLevelIndex].PlaceHoldersCount;
			busColors = levelData[selectedLevelIndex].BusColorList.ToList();

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
		#region Bus And PlaceHolder Methods
		void SaveBusAndPlaceHolder()
		{
			levelData[selectedLevelIndex].BusColorList = busColors.ToList();
			levelData[selectedLevelIndex].PlaceHoldersCount = placeHoldersCount;

			EditorUtility.SetDirty(levelData[selectedLevelIndex]);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
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
				SaveLevelEnvironment(selectedLevel);
			}
		}
		#endregion
	}
}