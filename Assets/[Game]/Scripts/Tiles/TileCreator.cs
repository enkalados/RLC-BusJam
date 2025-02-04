using Base.Global.Enums;
using Base.Managers;
using Base.Pool;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace GridSystem
{
	public class TileCreator : MonoBehaviour
	{
		#region Variables
		GameObject tilesParent;
		GameObject gridParent;
		string tilesParentName = "TilesParent";
		string gridParentName = "Grid";
		GridData tileGridData = null;
		PoolObject createdTile;
		#endregion
		#region Properties 
		#endregion
		#region MonoBehaviour Methods
		private void OnEnable()
		{
			LevelManager.OnLevelStart.AddListener(GetTileGridData);
		}
		private void OnDisable()
		{
			LevelManager.OnLevelStart.RemoveListener(GetTileGridData);
		}
		#endregion
		#region Methods
		void GetTileGridData()
		{
			tilesParent = GameObject.Find(tilesParentName);
			gridParent = GameObject.Find(gridParentName); 
			ResetTiles();
			SetTileGridData(LevelManager.Instance.GetCurrentLevelData().TilesData);
		}
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
					createdTile.transform.SetLocalPositionAndRotation(new Vector3(tileGridData.GridTiles[i].X, 0, -tileGridData.GridTiles[i].Z), Quaternion.identity);
				}
			}
			SetGridParent(tileGridData);
		}
		void SetGridParent(GridData gridData)
		{
			if (gridData.GridX % 2 == 0)
			{
				gridParent.transform.position = new Vector3((-gridData.GridX / 2) + .5f, gridParent.transform.position.y, gridParent.transform.position.z);
			}
			else
			{
				gridParent.transform.position = new Vector3((-gridData.GridX / 2), gridParent.transform.position.y, gridParent.transform.position.z);
			}
		}
		void ResetTiles()
		{
			if (tilesParent.transform.childCount > 0)
			{
				for (int i = 0; i < tilesParent.transform.childCount; i++)
				{
					PoolingManager.Instance.DestroyPoolObject(tilesParent.transform.GetChild(i).GetComponent<PoolObject>());
				}
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
			tilesParent = GameObject.Find(tilesParentName);
			gridParent = GameObject.Find(gridParentName);
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