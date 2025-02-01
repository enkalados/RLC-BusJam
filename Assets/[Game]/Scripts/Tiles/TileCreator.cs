using Base.Global.Enums;
using Base.Managers;
using Base.Pool;
using UnityEngine;
namespace GridSystem
{
	public class TileCreator : MonoBehaviour
	{
		#region Variables
		[SerializeField] GameObject TilesParent;
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
		#endregion
	}
}