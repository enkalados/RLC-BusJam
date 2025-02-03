using Base.Global.Enums;
using Base.Managers;
using Stickman;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace GridSystem
{
	public class GridStickmanControl : MonoBehaviour
	{
		#region Variables
		List<GameObject> stickmanList = new List<GameObject>();
		GridData tileGridData;
		GridData stickmanGridData;
		List<GridTile> gridTiles = new List<GridTile>();
		List<GridTile> obstacleTiles = new List<GridTile>();

		#endregion
		#region Properties 
		#endregion
		#region MonoBehaviour Methods
		private void Start()
		{
			GetGridTileData();
			GetStickmanTileData();

			for (int i = 0; i < stickmanGridData.GridTiles.Count; i++)
			{
				if (stickmanGridData.GridTiles[i].ObjectPoolID == PoolID.Stickman)
				{
					GridTile tile = new GridTile();
					tile.X = stickmanGridData.GridTiles[i].X;
					tile.Z = stickmanGridData.GridTiles[i].Z;
					obstacleTiles.Add(tile);
				}
			}
			for (int i = 0; i < tileGridData.GridTiles.Count; i++)
			{
				if (tileGridData.GridTiles[i].ObjectPoolID == PoolID.Tile)
				{
					GridTile tile = new GridTile();
					tile.X = tileGridData.GridTiles[i].X;
					tile.Z = tileGridData.GridTiles[i].Z;
					gridTiles.Add(tile);
				}
			}



			for (int i = 0; i < stickmanList.Count; i++)
			{
				bool can = CanReachZ0(stickmanList[i].GetComponent<StickmanControl>().GetGridX(), stickmanList[i].GetComponent<StickmanControl>().GetGridZ());
				stickmanList[i].GetComponent<StickmanControl>().SetCanClickable(can);
			}
		}
		#endregion
		#region Methods
		public bool CanReachZ0(int startX, int startZ)
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
			this.stickmanList = stickmanList.ToList();
		}
		//public bool CanEscape(int characterX, int characterZ)
		//{

		//}
		#endregion
	}
}