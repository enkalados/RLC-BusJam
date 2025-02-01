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
		[SerializeField] GameObject TilesParent;
		[SerializeField] GameManager GridParent;
		GridData tileGridData;
		PoolObject createdTile;
		#endregion
		#region Properties 
		#endregion
		#region MonoBehaviour Methods
		private void OnEnable()
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
					createdTile = PoolingManager.Instance.Instantiate(PoolID.Tile, TilesParent.transform);
					createdTile.transform.SetLocalPositionAndRotation(new Vector3(tileGridData.GridTiles[i].X, 0, tileGridData.GridTiles[i].Z), Quaternion.identity);
				}
			}
		}
		void SetGridParent(GridData gridData)
		{
			//TilesParent.transform.position=new Vector3(gridData.GridX)
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
					PoolObject item = (PoolObject)PrefabUtility.InstantiatePrefab(tileObject);
					item.transform.SetParent(TilesParent.transform);
					item.transform.SetLocalPositionAndRotation(new Vector3(gridData.GridTiles[i].X, 0, -gridData.GridTiles[i].Z), Quaternion.identity);
				}
			}
		}
#endif
	}
}