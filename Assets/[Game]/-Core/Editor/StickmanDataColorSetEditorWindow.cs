using Base.Global.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace GridSystem.Editor
{
	public class StickmanDataColorSetEditorWindow : EditorWindow
	{
		static int selectedGridDataIndex;
		List<GridData> gridDataList = new List<GridData>();
		string gridDatasPath = "Assets/[Game]/Data/GridData";

		private Dictionary<Colors, Color> colorMappings;
		List<GridTile> tiles = new List<GridTile>();
		private Colors[,] gridColorData;

		#region Editor Methods
		public static void ShowWindowGridDataColor(int selectedDataIndex)
		{
			selectedGridDataIndex = selectedDataIndex;
			GetWindow<StickmanDataColorSetEditorWindow>("Set Color Data");
		}
		private void OnEnable()
		{
			LoadData();
			InitializeGrid();
			InitializeColorMappings();
		}
		private void OnGUI()
		{
			EditorGUILayout.LabelField("Set Colors for Stickman Objects", EditorStyles.boldLabel);
			GUILayout.Space(10);

			if (gridDataList[selectedGridDataIndex] == null)
			{
				EditorGUILayout.LabelField("No Grid Data Available", EditorStyles.boldLabel);
				return;
			}

			DrawGrid();

			if (GUILayout.Button("Save Colors"))
			{
				SaveGridData();
			}
		}
		#endregion
		#region Methods
		private void DrawGrid()
		{
			GUILayout.Label("Set Grid", EditorStyles.boldLabel);
			EditorGUILayout.Space(10);

			Handles.BeginGUI();
			for (int z = 0; z < gridDataList[selectedGridDataIndex].GridZ; z++)
			{
				EditorGUILayout.BeginHorizontal();
				for (int x = 0; x < gridDataList[selectedGridDataIndex].GridX; x++)
				{
					if (x < gridColorData.GetLength(0) && z < gridColorData.GetLength(1))
					{
						if (gridDataList[selectedGridDataIndex].GridTiles.First(stkmcn => stkmcn.X == x && stkmcn.Z == z).ObjectPoolID == PoolID.Stickman)
						{
							Rect rect = GUILayoutUtility.GetRect(50, 30);
							Color color;
							if (colorMappings.TryGetValue(gridColorData[x, z], out color))
							{
								EditorGUI.DrawRect(rect, color);

							}
							gridColorData[x, z] = (Colors)EditorGUI.EnumPopup(rect, gridColorData[x, z]);
						}
						else
						{
							Rect rect = GUILayoutUtility.GetRect(50, 30);
							EditorGUI.DrawRect(rect, Color.gray);
						}
					}
				}
				EditorGUILayout.EndHorizontal();
			}
			Handles.EndGUI();
		}
		void InitializeGrid()
		{
            gridColorData = new Colors[gridDataList[selectedGridDataIndex].GridX, gridDataList[selectedGridDataIndex].GridZ];
			for (int x = 0; x < gridDataList[selectedGridDataIndex].GridX; x++)
            {
				for (int z = 0; z < gridDataList[selectedGridDataIndex].GridZ; z++)
				{
					gridColorData[x, z] = gridDataList[selectedGridDataIndex].GridTiles.First(stkmcn => stkmcn.X == x && stkmcn.Z == z).Color;
				}
			}
		}
		void LoadData()
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
		private void InitializeColorMappings()
		{
			colorMappings = new Dictionary<Colors, Color>();
			foreach (Colors color in System.Enum.GetValues(typeof(Colors)))
			{
				colorMappings[color] = GetUnityColor(color);
			}
		}
		private Color GetUnityColor(Colors color)
		{
			switch (color)
			{
				case Colors.Green: return Color.green;
				case Colors.Blue: return Color.blue;
				case Colors.Orange: return new Color(1f, 0.647f, 0f);
				case Colors.Purple: return new Color(0.5f, 0f, 0.5f);
				default: return Color.white;
			}
		}
		private void SaveGridData()
		{
			if (gridDataList[selectedGridDataIndex].GridTiles[0].Color!= Colors.None)
			{

			}
			for (int x = 0; x < gridDataList[selectedGridDataIndex].GridX; x++)
			{
				for (int z = 0; z < gridDataList[selectedGridDataIndex].GridZ; z++)
				{
					gridDataList[selectedGridDataIndex].GridTiles.First(stkmcn => stkmcn.X == x && stkmcn.Z == z).Color = gridColorData[x, z];
				}
			}
			EditorUtility.SetDirty(gridDataList[selectedGridDataIndex]);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
		#endregion
	}
}