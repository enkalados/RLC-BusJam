using Base.Global.Enums;
using Base.Managers;
using Base.Pool;
using UnityEditor;
using UnityEngine;
namespace GridSystem
{
	public class TileCreator : MonoBehaviour
	{
		#region Variables
		[SerializeField] GameObject tilesParent;
		[SerializeField] GameObject gridParent;
		GridData tileGridData;
		PoolObject createdTile;
		#endregion
		#region Properties 
		#endregion
		#region MonoBehaviour Methods
		private void Start()
		{
			SetTileGridData(LevelManager.Instance.GetCurrentLevelData().TilesData);
		}
		#endregion
		#region Methods

		void SetTileGridData(GridData gridData)
		{
			tileGridData = gridData;
			CreateTiles();
		}
		void CreateTiles()
		{
			for (int i = 0; i < tileGridData.GridTiles.Count; i++)
			{
				if (tileGridData.GridTiles[i].ObjectPoolID == PoolID.Tile)
				{
					createdTile = PoolingManager.Instance.Instantiate(PoolID.Tile, tilesParent.transform);
					createdTile.transform.SetLocalPositionAndRotation(new Vector3(tileGridData.GridTiles[i].X, 0, tileGridData.GridTiles[i].Z), Quaternion.identity);
				}
			}
			SetGridParent(tileGridData);
		}
		void SetGridParent(GridData gridData)
		{
			if (gridData.GridX % 2 == 0)
			{
				gridParent.transform.position = new Vector3((-gridData.GridX / 2)+.5f, gridParent.transform.position.y, gridParent.transform.position.z);
			}
			else
			{
				gridParent.transform.position = new Vector3((-gridData.GridX / 2), gridParent.transform.position.y, gridParent.transform.position.z);
			}
		}
		#endregion
#if UNITY_EDITOR
		public void SetTileGridDataEditor(GridData gridData, PoolObject tileObject)
		{
			CreateTilesEditor(gridData, tileObject);
		}
		void CreateTilesEditor(GridData gridData, PoolObject tileObject)
		{
			for (int i = 0; i < gridData.GridTiles.Count; i++)
			{
				if (gridData.GridTiles[i].ObjectPoolID == PoolID.Tile)
				{
					PoolObject item = (PoolObject)PrefabUtility.InstantiatePrefab(tileObject, tilesParent.transform);
					item.transform.SetLocalPositionAndRotation(new Vector3(gridData.GridTiles[i].X, 0, -gridData.GridTiles[i].Z), Quaternion.identity);
				}
			}
			SetGridParent(gridData);
		}
#endif
	}
}