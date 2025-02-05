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
		private PoolID[,] gridData;
		#endregion
		#region Veriables
		string poolPath = "PoolObjects";
		PoolData poolDatabase;

		public static GridData editingData = null;
		List<GridData> gridDataList = new List<GridData>();
		string gridDatasPath = "Assets/[Game]/Data/GridData";
		int selectedGridDataIndex;
		#endregion
		#region Dictionary
		#endregion
		#region Editor Methods
		public static void ShowWindow(GridData gridData)
		{
			editingData = gridData;
			GetWindow<GridEditorWindow>("Grid Editor");
		}
		private void OnEnable()
		{
			InitializeGrid(gridX, gridZ);
			LoadGridData();
			poolDatabase = Resources.Load<PoolData>(poolPath);
			if (editingData != null)
			{
				ClearAndPreviewGrid(editingData);
				for (int i = 0; i < gridDataList.Count; i++)
				{
					if (string.Equals(gridDataList[i].name, editingData.name))
					{
						selectedGridDataIndex = i;
					}
				}
			}
		}
		private void OnDisable()
		{
			editingData = null;
		}
		private void OnGUI()
		{
			GUILayout.Label("Grid Settings", EditorStyles.boldLabel);

			int newGridX = EditorGUILayout.IntField("Grid Width (X)", gridX);
			int newGridZ = EditorGUILayout.IntField("Grid Depth (Z)", gridZ);

			if (newGridX != gridX || newGridZ != gridZ)
			{
				ResizeGrid(newGridX, newGridZ);
			}
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

				if (GUILayout.Button("Save Grid"))
				{
					ShowWarningPopup(selectedGrid);
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
			if(gridDataList[selectedGridDataIndex].GridType == GridTypes.Stickman)
			{
				if (GUILayout.Button("Edit Stickman Color Data", GUILayout.Height(25)))
				{
					StickmanDataColorSetEditorWindow.ShowWindowGridDataColor(selectedGridDataIndex);
				}
			}
	
			GUILayout.Space(10);

			DrawGrid();
		}
		#endregion
		#region Grid Methods
		private void InitializeGrid(int x, int z)
		{
			gridData = new PoolID[x, z];
		}
		private void ResizeGrid(int newX, int newZ)
		{
			PoolID[,] newGridData = new PoolID[newX, newZ];
			for (int x = 0; x < Mathf.Min(gridX, newX); x++)
			{
				for (int z = 0; z < Mathf.Min(gridZ, newZ); z++)
				{
					newGridData[x, z] = gridData[x, z];
				}
			}
			gridData = newGridData;
			gridX = newX;
			gridZ = newZ;

			Repaint();
		}
		private void DrawGrid()
		{
			GUILayout.Label("Set Grid", EditorStyles.boldLabel);
			EditorGUILayout.Space(10);

			Handles.BeginGUI();
			for (int z = 0; z < gridZ; z++) 
			{
				EditorGUILayout.BeginHorizontal();
				for (int x = 0; x < gridX; x++)
				{
					if (x < gridData.GetLength(0) && z < gridData.GetLength(1))
					{
						Rect rect = GUILayoutUtility.GetRect(50, 30);
						//EditorGUI.DrawRect(rect, GetTileColor(gridData[x, z]));

						gridData[x, z] = (PoolID)EditorGUI.EnumPopup(rect, gridData[x, z]);
					}
				}
				EditorGUILayout.EndHorizontal();
			}
			Handles.EndGUI();
		}
		private void SaveGrid(GridData selectedGrid)
		{
			if (selectedGrid.GridTiles.Count > 0)
			{
				selectedGrid.GridTiles.Clear();
			}
			selectedGrid.GridX = gridX;
			selectedGrid.GridZ = gridZ;
			for (int x = 0; x < gridX; x++)
			{
				for (int z = 0; z < gridZ; z++)
				{
					GridTile tile = new GridTile();
					tile.X = x;
					tile.Z = z;
					tile.ObjectPoolID = gridData[x, z];
					selectedGrid.GridTiles.Add(tile);
				}
			}
			EditorUtility.SetDirty(selectedGrid);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
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
		private void ClearAndPreviewGrid()
		{
			if (gridDataList[selectedGridDataIndex].GridTiles.Count > 0)
			{
				gridX = gridDataList[selectedGridDataIndex].GridX;
				gridZ = gridDataList[selectedGridDataIndex].GridZ;
				InitializeGrid(gridX, gridZ);

				for (int i = 0; i < gridDataList[selectedGridDataIndex].GridTiles.Count; i++)
				{
					gridData[gridDataList[selectedGridDataIndex].GridTiles[i].X, gridDataList[selectedGridDataIndex].GridTiles[i].Z] = gridDataList[selectedGridDataIndex].GridTiles[i].ObjectPoolID;
				}
			}
		}
		private void ClearAndPreviewGrid(GridData selectedGrid)
		{
			if (selectedGrid.GridTiles.Count > 0)
			{
				gridX = selectedGrid.GridX;
				gridZ = selectedGrid.GridZ;
				InitializeGrid(gridX, gridZ);

				for (int i = 0; i < selectedGrid.GridTiles.Count; i++)
				{
					gridData[selectedGrid.GridTiles[i].X, selectedGrid.GridTiles[i].Z] = selectedGrid.GridTiles[i].ObjectPoolID;
				}
			}
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
	
		#region Warning Popup
		private void ShowWarningPopup(GridData selectedGrid)
		{
			bool continueAction = EditorUtility.DisplayDialog(
				"WARNING",
				"There is data for the selected grid. If you continue, existing data will be deleted and new data will be created. Do you want to continue?",
				"Clear And Save",
				"Cancel"
			);

			if (continueAction)
			{
				SaveGrid(selectedGrid);
			}
		}
		#endregion
	}	
}