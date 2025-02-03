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
				bool can = CanReachZero(stickmanList[i].GetComponent<StickmanControl>().GetGridX(), stickmanList[i].GetComponent<StickmanControl>().GetGridZ());
				stickmanList[i].GetComponent<StickmanControl>().SetCanClickable(can);
			}
		}
		#endregion
		#region Methods
		bool CanReachZero(int x, int z)
		{
			return CanReachZero1(x, z) || CanReachZero2(x, z);
		}
		bool CanReachZero1(int x, int z)
		{
			if (z == 0)
			{
				print(x + " " + z + " eriþebilir");
				return true;
			}

			for (int i = x; i < tileGridData.GridX; i++)
			{
				for (int j = z; j > -1; j--)
				{
					if (i == x && j == z) continue;
					if (TileListContains(i, j))
					{
						print(x + "-" + z + "  için kontrol:" + i + " " + j + " tile içinde");
						if (!StickmanListContains(i, j))
						{
							print(i + " " + j + " boþ");
							if (j == 0)
							{
								print(x + "-" + z + "  için kontrol:" + i + " " + j + " eriþebilir");
								return true;
							}
						}
						else
						{
							print(x + "-" + z + "  için kontrol:" + i + " " + j + " dolu");
							continue;
						}
					}
					else
					{
						print(x + "-" + z + "  için kontrol:" + i + " " + j + " tile dýþýnda");
						continue;
					}
				}
			}

			return false;
		}
		bool CanReachZero2(int x, int z)
		{
			if (z == 0)
			{
				print(x + " " + z + " eriþebilir");
				return true;
			}

			for (int i = x; i > -1; i--)
			{
				for (int j = z; j > -1; j--)
				{
					if (i == x && j == z) continue;
					if (TileListContains(i, j))
					{
						print(x + "-" + z + "  için kontrol:" + i + " " + j + " tile içinde");
						if (!StickmanListContains(i, j))
						{
							print(x + "-" + z + "  için kontrol:" + i + " " + j + " boþ");
							if (j == 0)
							{
								print(x + "-" + z + "  için kontrol:" + i + " " + j + " eriþebilir");
								return true;
							}
						}
						else
						{
							print(x + "-" + z + "  için kontrol:" + i + " " + j + " dolu");
							continue;
						}
					}
					else
					{
						print(x + "-" + z + "  için kontrol:" + i + " " + j + " tile dýþýnda");
						continue;
					}
				}
			}

			return false;
		}
		bool StickmanListContains(int x, int z)
		{
			foreach (GridTile item in obstacleTiles)
			{
				if (item.X == x && item.Z == z)
				{
					return true;
				}
			}
			return false;
		}
		bool TileListContains(int x, int z)
		{
			foreach (GridTile item in gridTiles)
			{
				if (item.X == x && item.Z == z)
				{
					return true;
				}
			}
			return false;
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