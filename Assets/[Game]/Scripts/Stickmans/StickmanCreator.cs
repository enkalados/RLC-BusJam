using Base.Global.Enums;
using Base.Managers;
using Base.Pool;
using GameSaveLoad;
using GridSystem;
using MeshColorSetter;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Stickman.Creator
{
	public class StickmanCreator : MonoBehaviour
	{
		#region Variables
		GameObject stickmanParent;
		string parentName = "StickmansParent";
		GridData stickmanGridData;
		List<GameObject> createdStickmanList = new List<GameObject>();
		#endregion
		#region Properties 
		GridStickmanControl gridStickman;
		GridStickmanControl GridStickmanControl => (gridStickman == null) ? gridStickman = GetComponent<GridStickmanControl>() : gridStickman;
		GameSaveLoadControl gameSaveLoad;
		GameSaveLoadControl GameSaveLoad => (gameSaveLoad == null) ? gameSaveLoad = GetComponent<GameSaveLoadControl>() : gameSaveLoad;
		#endregion
		#region MonoBehaviour Methods
		private void OnEnable()
		{
			LevelManager.OnLevelStart.AddListener(GetSticmanData);
		}
		private void OnDisable()
		{
			LevelManager.OnLevelStart.RemoveListener(GetSticmanData);
		}
		#endregion
		#region Methods
		void GetSticmanData()
		{
			stickmanParent = GameObject.Find(parentName);
			ResetStickmans();
			if (GameSaveLoad.LoadStickmanListData().Count > 0)
			{
				LoadStickmanFromSave(GameSaveLoad.LoadStickmanListData());
			}
			else
			{
				SetStickmanData(LevelManager.Instance.GetCurrentLevelData().StickmansTileData);
				GridStickmanControl.SetStickmans(createdStickmanList);
			}
		}
		void SetStickmanData(GridData gridData)
		{
			stickmanGridData = gridData;
			CreateStickmans();
		}
		void ResetStickmans()
		{
			if (createdStickmanList.Count > 0)
			{
				foreach (GameObject item in createdStickmanList)
				{
					PoolingManager.Instance.DestroyPoolObject(item.GetComponent<PoolObject>());
				}
				createdStickmanList.Clear();
			}
		}
		void CreateStickmans()
		{
			createdStickmanList.Clear();
			for (int i = 0; i < stickmanGridData.GridTiles.Count; i++)
			{
				if (stickmanGridData.GridTiles[i].ObjectPoolID == PoolID.Stickman)
				{
					PoolObject createdStickman;
					createdStickman = PoolingManager.Instance.Instantiate(PoolID.Stickman, stickmanParent.transform);
					createdStickman.transform.SetLocalPositionAndRotation(new Vector3(stickmanGridData.GridTiles[i].X, 0, -stickmanGridData.GridTiles[i].Z), Quaternion.identity);
					createdStickman.GetComponent<MeshColorSet>().SetColor(stickmanGridData.GridTiles[i].Color);
					createdStickman.GetComponent<StickmanControl>().SetStickmanColor(stickmanGridData.GridTiles[i].Color);
					createdStickman.GetComponent<StickmanControl>().SetGridInfo(stickmanGridData.GridTiles[i].X, stickmanGridData.GridTiles[i].Z);
					createdStickmanList.Add(createdStickman.gameObject);
				}
			}
		}
		#region	Save Load
		internal void LoadStickmanFromSave(List<GridTile> stickmanList)
		{
			for (int i = 0; i < stickmanList.Count; i++)
			{
				PoolObject createdStickman;
				createdStickman = PoolingManager.Instance.Instantiate(PoolID.Stickman, stickmanParent.transform);
				createdStickman.transform.SetLocalPositionAndRotation(new Vector3(stickmanList[i].X, 0, -stickmanList[i].Z), Quaternion.identity);
				createdStickman.GetComponent<MeshColorSet>().SetColor(stickmanList[i].Color);
				createdStickman.GetComponent<StickmanControl>().SetStickmanColor(stickmanList[i].Color);
				createdStickman.GetComponent<StickmanControl>().SetGridInfo(stickmanList[i].X, stickmanList[i].Z);
				createdStickmanList.Add(createdStickman.gameObject);
			}
			GridStickmanControl.LoadSavedStickman(createdStickmanList, stickmanList);

		}
		#endregion
		#endregion
#if UNITY_EDITOR
		public void SetStickmanDataEditor(GridData gridData, PoolObject stickmanObj)
		{
			CreateStickmanEditor(gridData, stickmanObj);
		}
		void CreateStickmanEditor(GridData gridData, PoolObject stickmanObj)
		{
			stickmanParent = GameObject.Find(parentName);
			for (int i = 0; i < gridData.GridTiles.Count; i++)
			{
				if (gridData.GridTiles[i].ObjectPoolID == PoolID.Stickman)
				{
					PoolObject item = (PoolObject)PrefabUtility.InstantiatePrefab(stickmanObj, stickmanParent.transform);
					item.transform.SetLocalPositionAndRotation(new Vector3(gridData.GridTiles[i].X, 0, -gridData.GridTiles[i].Z), Quaternion.identity);
					item.GetComponent<MeshColorSet>().SetColor(gridData.GridTiles[i].Color);
				}
			}
		}
#endif
	}
}