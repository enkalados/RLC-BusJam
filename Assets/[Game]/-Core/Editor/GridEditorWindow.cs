using Base.Pool;
using LevelSaveSystem;
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
		#endregion
		#region Veriables
		string poolPath = "PoolObjects";
		PoolData poolDatabase;
		int selectedPoolIndex = 0;

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

			GUILayout.Label("Select Spawn Object", EditorStyles.boldLabel);
			selectedPoolIndex = EditorGUILayout.Popup("Selected Object:", selectedPoolIndex, GetPoolObjectNames());

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
				if (GUILayout.Button("Refresh Grid"))
				{
					Repaint();
				}
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

			GUILayout.Space(10);

			Rect gridArea = GUILayoutUtility.GetRect(400, 400);
			DrawGrid(gridArea);
		}
		#endregion
		#region Methods
		private void DrawGrid(Rect area)
		{
			Handles.BeginGUI();
			Handles.color = Color.gray;

			float startX = area.x;
			float startY = area.y;
			float gridWidth = area.width;
			float gridHeight = area.height;

			float cellWidth = gridWidth / gridX;
			float cellHeight = gridHeight / gridZ;

			for (int x = 0; x <= gridX; x++)
			{
				float xPos = startX + x * cellWidth;
				Handles.DrawLine(new Vector3(xPos, startY), new Vector3(xPos, startY + gridHeight));
			}
			for (int z = 0; z <= gridZ; z++)
			{
				float yPos = startY + z * cellHeight;
				Handles.DrawLine(new Vector3(startX, yPos), new Vector3(startX + gridWidth, yPos));
			}

			Handles.EndGUI();
		}
		private string[] GetPoolObjectNames()
		{
			string[] names = new string[poolDatabase.PoolHolder.Count];
			for (int i = 0; i < poolDatabase.PoolHolder.Count; i++)
			{
				names[i] = poolDatabase.PoolHolder[i].Prefab.PoolID.ToString();
			}
			return names;
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