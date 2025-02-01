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
		#region Properties 
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

			GUILayout.Space(30);

			DrawGrid();
		}
		#endregion
		#region Methods
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
			Handles.color = Color.gray;

			for (int y = 0; y < gridX; y++)
			{
				EditorGUILayout.BeginHorizontal();
				for (int x = 0; x < gridZ; x++)
				{
					gridData[x, y] = (PoolID)EditorGUILayout.EnumPopup(gridData[x, y], GUILayout.Width(100));
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
	}
}