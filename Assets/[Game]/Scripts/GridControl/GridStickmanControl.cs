using Base.Global.Enums;
using Base.Managers;
using BusSystem;
using GameSaveLoad;
using Stickman;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace GridSystem
{
	public class GridStickmanControl : MonoBehaviour
	{
		#region Variables
		List<StickmanControl> stickmanControlList = new List<StickmanControl>();
		GridData tileGridData;
		GridData stickmanGridData;
		List<GridTile> gridTiles = new List<GridTile>();
		List<GridTile> obstacleTiles = new List<GridTile>();
		List<GridTile> stickmanSaved = new List<GridTile>();

		#endregion
		#region Properties 
		BusPassengerControl busPassengerControl;
		BusPassengerControl BusPassengerControl => (busPassengerControl == null) ? busPassengerControl = GetComponent<BusPassengerControl>() : busPassengerControl;
		GameSaveLoadControl gameSaveLoad;
		GameSaveLoadControl GameSaveLoad => (gameSaveLoad == null) ? gameSaveLoad = GetComponent<GameSaveLoadControl>() : gameSaveLoad;
		#endregion
		#region MonoBehaviour Methods
		private void OnEnable()
		{
			LevelManager.OnLevelStart.AddListener(GetGridDatas);
			LevelManager.OnLevelFinish.AddListener(ClearSavedStickmans);
		}
		private void OnDisable()
		{
			LevelManager.OnLevelStart.RemoveListener(GetGridDatas);
			LevelManager.OnLevelFinish.AddListener(ClearSavedStickmans);
		}
		#endregion
		#region Grid Methods
		internal void CheckClickedStickman(int x, int z)
		{
			if (CanReachZ0(x, z))
			{
				if (stickmanSaved.Count == 0)
				{
					CreateStickmanSave();
				}
				stickmanControlList.First(stckmn => stckmn.GetGridX() == x && stckmn.GetGridZ() == z).Clicked();
				BusPassengerControl.CheckPassenger(stickmanControlList.First(stckmn => stckmn.GetGridX() == x && stckmn.GetGridZ() == z));
				stickmanControlList.Remove(stickmanControlList.First(stckmn => stckmn.GetGridX() == x && stckmn.GetGridZ() == z));
				obstacleTiles.Remove(obstacleTiles.First(stckmn => stckmn.X == x && stckmn.Z == z));
				CheckAllStickmans();
				RemoveFromSavedList(x, z);
			}
		}
		void CheckAllStickmans()
		{
			for (int i = 0; i < stickmanControlList.Count; i++)
			{
				stickmanControlList[i].SetCanClickable(CanReachZ0(stickmanControlList[i].GetGridX(), stickmanControlList[i].GetGridZ()));
			}
		}
		void GetGridDatas()
		{
			ResetDatas();

			GetGridTileData();
			gridTiles = tileGridData.GridTiles.Where(tile => tile.ObjectPoolID == PoolID.Tile).ToList();
			if (GameSaveLoad.LoadStickmanListData().Count == 0)
			{
				GetStickmanTileData();
				obstacleTiles = stickmanGridData.GridTiles.Where(tile => tile.ObjectPoolID == PoolID.Stickman).ToList();
				CheckAllStickmans();
			}

		}
		void ResetDatas()
		{
			obstacleTiles.Clear();
			gridTiles.Clear();
		}
		private bool CanReachZ0(int startX, int startZ)
		{
			if (!gridTiles.Any(tile => tile.X == startX && tile.Z == startZ))
			{
				return false;
			}

			HashSet<string> visited = new HashSet<string>();
			Queue<(int x, int z)> queue = new Queue<(int x, int z)>();

			queue.Enqueue((startX, startZ));
			visited.Add($"{startX},{startZ}");

			int[] dx = { 0, 0, 1, -1 };
			int[] dz = { 1, -1, 0, 0 };

			while (queue.Count > 0)
			{
				var (currentX, currentZ) = queue.Dequeue();

				if (currentZ == 0)
				{
					return true;
				}
				for (int i = 0; i < 4; i++)
				{
					int newX = currentX + dx[i];
					int newZ = currentZ + dz[i];
					string key = $"{newX},{newZ}";

					if (IsValidPosition(newX, newZ) && !visited.Contains(key))
					{
						queue.Enqueue((newX, newZ));
						visited.Add(key);
					}
				}
			}
			return false;
		}
		private bool IsValidPosition(int x, int z)
		{
			// Grid üzerinde var mý kontrol et
			bool isOnGrid = gridTiles.Any(tile => tile.X == x && tile.Z == z);
			// Engel var mý kontrol et
			bool isObstacle = obstacleTiles.Any(tile => tile.X == x && tile.Z == z);
			return isOnGrid && !isObstacle;
		}
		void GetGridTileData()
		{
			tileGridData = LevelManager.Instance.GetCurrentLevelData().TilesData;
		}
		void GetStickmanTileData()
		{
			stickmanGridData = LevelManager.Instance.GetCurrentLevelData().StickmansTileData;
		}
		internal void SetStickmans(List<GameObject> stickmanList)
		{
			stickmanControlList.Clear();
			for (int i = 0; i < stickmanList.Count; i++)
			{
				stickmanControlList.Add(stickmanList[i].GetComponent<StickmanControl>());
			}
			CheckAllStickmans();
		}
		#region	Save Load
		internal void LoadSavedStickman(List<GameObject> stickmanList, List<GridTile> tileList)
		{
			ClearSavedStickmans();
			obstacleTiles.Clear();
			stickmanControlList.Clear();
			stickmanSaved = tileList.ToList();
			for (int i = 0; i < stickmanList.Count; i++)
			{
				stickmanControlList.Add(stickmanList[i].GetComponent<StickmanControl>());
			}
			obstacleTiles = tileList.Where(tile => tile.ObjectPoolID == PoolID.Stickman).ToList();
			CheckAllStickmans();
		}
		void ClearSavedStickmans()
		{
			stickmanSaved.Clear();
		}
		void CreateStickmanSave()
		{
			obstacleTiles.Clear();
			for (int i = 0; i < stickmanControlList.Count; i++)
			{
				GridTile stickmanTile = new GridTile();
				stickmanTile.X = stickmanControlList[i].GetGridX();
				stickmanTile.Z = stickmanControlList[i].GetGridZ();
				stickmanTile.ObjectPoolID = PoolID.Stickman;
				stickmanTile.Color = stickmanControlList[i].GetColor();
				stickmanSaved.Add(stickmanTile);
			}
			obstacleTiles = stickmanGridData.GridTiles.Where(tile => tile.ObjectPoolID == PoolID.Stickman).ToList();
			GameSaveLoad.SaveStickmanListData(stickmanSaved);
		}
		void RemoveFromSavedList(int x, int z)
		{
			stickmanSaved.Remove(stickmanSaved.First(stckmn => stckmn.X == x && stckmn.Z == z));
			GameSaveLoad.SaveStickmanListData(stickmanSaved);
		}
		#endregion
		#endregion
	}
}