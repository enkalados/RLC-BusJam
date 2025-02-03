using Base.Global.Enums;
using Base.Managers;
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

		#endregion
		#region Properties 
		#endregion
		#region MonoBehaviour Methods
		private void OnEnable()
		{
			LevelManager.OnLevelStart.AddListener(GetGridDatas);
		}
		private void OnDisable()
		{
			LevelManager.OnLevelStart.RemoveListener(GetGridDatas);
		}
		#endregion
		#region Grid Methods
		internal void CheckClickedStickman(int x, int z)
		{
			if (CanReachZ0(x, z))
			{
				stickmanControlList.First(stckmn => stckmn.GetGridX() == x && stckmn.GetGridZ() == z).Clicked();
				obstacleTiles.Remove(obstacleTiles.First(stckmn => stckmn.X == x && stckmn.Z == z));			
				CheckAllStickmans();
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
			GetGridTileData();
			GetStickmanTileData();

			obstacleTiles.Clear();
			gridTiles.Clear();

			obstacleTiles = stickmanGridData.GridTiles.Where(tile => tile.ObjectPoolID == PoolID.Stickman).ToList();
			gridTiles = tileGridData.GridTiles.Where(tile => tile.ObjectPoolID == PoolID.Tile).ToList();
			CheckAllStickmans();
		}
		private bool CanReachZ0(int startX, int startZ)
		{
			if (!gridTiles.Any(tile => tile.X == startX && tile.Z == startZ))
			{
				Debug.Log($"Invalid position ({startX}, {startZ})");
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
			for (int i = 0; i < stickmanList.Count; i++)
			{
				stickmanControlList.Add(stickmanList[i].GetComponent<StickmanControl>());
			}
		}
		#endregion
	}
}