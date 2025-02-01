using Base.Global.Enums;
using Base.Pool;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace GridSystem.Editor
{
	public class GridEditorWindow : EditorWindow
	{
		#region Grid Params
		private int gridX = 10;
		private int gridZ = 10;
		private float cellSize = 1f;
		private PoolID[,] gridData;
		#endregion
		#region Veriables
		string poolPath = "PoolObjects";
		PoolData poolDatabase;

		List<GridData> gridDataList = new List<GridData>();
		string gridDatasPath = "Assets/[Game]/Data/GridData";
		int selectedGridDataIndex;
		#endregion
		#region Dictionary
		
		public static Dictionary<PoolID, Color> PoolColors = new Dictionary<PoolID, Color>();
		#endregion
		#region Editor Methods
		[MenuItem("Tools/Grid Editor")]
		public static void ShowWindow()
		{
			GetWindow<GridEditorWindow>("Grid Editor");
		}
		private void OnEnable()
		{
			InitializeGrid();
			LoadGridData();
			LoadColors();
			poolDatabase = Resources.Load<PoolData>(poolPath);
		}
		private void OnGUI()
		{
			GUILayout.Label("Grid Settings", EditorStyles.boldLabel);
			selectedGridDataIndex = EditorGUILayout.Popup("Selected Grid:", selectedGridDataIndex, GetGridDataNames());
			gridX = EditorGUILayout.IntField("Grid Width (X)", gridX);
			gridZ = EditorGUILayout.IntField("Grid Depth (Z)", gridZ);
			cellSize = EditorGUILayout.FloatField("Cell Size", cellSize);

			EditorGUILayout.Space(15);

			if (gridDataList.Count == 0)
			{
				EditorGUILayout.HelpBox("Not found GridData!", MessageType.Warning);
				return;
			}

			if (gridDataList.Count > 0 && selectedGridDataIndex < gridDataList.Count)
			{
				GridData selectedGrid = gridDataList[selectedGridDataIndex];

				EditorGUILayout.Space();

				if (GUILayout.Button("Clear & Preview Level"))
				{

				}
				//if (GUILayout.Button("Generate Grid"))
				//{
				//	InitializeGrid();
				//}
				if (GUILayout.Button("Save Level"))
				{

				}

				EditorGUILayout.Space(15);

				GUILayout.BeginHorizontal();
				if (GUILayout.Button("Go to Grid data"))
				{
					EditorGUIUtility.PingObject(selectedGrid);
				}
				if (GUILayout.Button("Go to pool data"))
				{
					EditorGUIUtility.PingObject(poolDatabase);
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.Space(20);

			if (GUILayout.Button("Edit Pool Colors", GUILayout.Height(25)))
			{
				PoolColorWindow.ShowWindow();
			}

			GUILayout.Space(20);

			DrawGrid();
		}
		#endregion
		#region Grid Methods
		private void InitializeGrid()
		{
			if (gridData == null || gridData.GetLength(0) != gridX || gridData.GetLength(1) != gridZ)
			{
				PoolID[,] newGrid = new PoolID[gridX, gridZ];
				if (gridData != null)
				{
					for (int x = 0; x < Mathf.Min(gridX, gridData.GetLength(0)); x++)
					{
						for (int y = 0; y < Mathf.Min(gridZ, gridData.GetLength(1)); y++)
						{
							newGrid[x, y] = gridData[x, y];
						}
					}
				}

				gridData = newGrid;
			}
		}
		private void DrawGrid()
		{
			GUILayout.Label("Set Grid", EditorStyles.boldLabel);
			EditorGUILayout.Space(10);

			Handles.BeginGUI();


			for (int y = 0; y < gridX; y++)
			{
				EditorGUILayout.BeginHorizontal();
				for (int x = 0; x < gridZ; x++)
				{
					Rect rect = GUILayoutUtility.GetRect(120, 30);
					EditorGUI.DrawRect(rect, GetTileColor(gridData[x, y]));

					gridData[x, y] = (PoolID)EditorGUI.EnumPopup(rect, gridData[x, y]);
				}
				EditorGUILayout.EndHorizontal();
			}


			Handles.EndGUI();
		}
		private void LoadGridData()
		{
			gridDataList.Clear();

			string[] levels = AssetDatabase.FindAssets("t:GridData", new[] { gridDatasPath });

			foreach (string item in levels)
			{
				string path = AssetDatabase.GUIDToAssetPath(item);
				GridData data = AssetDatabase.LoadAssetAtPath<GridData>(path);

				if (data != null)
					gridDataList.Add(data);
			}

			if (selectedGridDataIndex >= gridDataList.Count)
				selectedGridDataIndex = 0;
		}
		private string[] GetGridDataNames()
		{
			string[] names = new string[gridDataList.Count];
			for (int i = 0; i < gridDataList.Count; i++)
			{
				names[i] = gridDataList[i].name;
			}
			return names;
		}
		#endregion
		#region Color Control
		public static void SaveColor(PoolID id, Color color)
		{
			PoolColors[id] = color;
			EditorPrefs.SetString("PoolColor_" + id.ToString(), JsonUtility.ToJson(color));
		}
		private void LoadColors()
		{
			foreach (PoolID id in System.Enum.GetValues(typeof(PoolID)))
			{
				string key = "PoolColor_" + id.ToString();
				if (EditorPrefs.HasKey(key))
				{
					Color color = JsonUtility.FromJson<Color>(EditorPrefs.GetString(key));
					PoolColors[id] = color;
				}
				else
				{
					PoolColors[id] = Color.white;
				}
			}
		}
		Color GetTileColor(PoolID poolID)
		{
			if (PoolColors.TryGetValue(poolID, out Color color))
			{
				return color;
			}
			return Color.white;
		}
		#endregion

	}
	public class PoolColorWindow : EditorWindow
	{

		public static void ShowWindow()
		{
			GetWindow<PoolColorWindow>("Pool Objects Colors");
		}

		private void OnGUI()
		{
			EditorGUILayout.LabelField("Set Colors for Pool Objects", EditorStyles.boldLabel);

			foreach (PoolID id in System.Enum.GetValues(typeof(PoolID)))
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(id.ToString(), GUILayout.Width(100));

				if (!GridEditorWindow.PoolColors.TryGetValue(id, out Color color))
				{
					color = Color.white;
				}

				Color newColor = EditorGUILayout.ColorField(color);
				if (newColor != color)
				{
					GridEditorWindow.SaveColor(id, newColor);
				}

				EditorGUILayout.EndHorizontal();
			}
		}
		
	}
}