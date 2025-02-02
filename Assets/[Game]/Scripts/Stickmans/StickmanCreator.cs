using Base.Global.Enums;
using Base.Managers;
using Base.Pool;
using GridSystem;
using MeshColorSetter;
using UnityEditor;
using UnityEngine;
namespace Stickman.Creator
{
	public class StickmanCreator : MonoBehaviour
	{
		#region Variables
		[SerializeField] GameObject stickmanParent;
		GridData stickmanGridData;
		PoolObject createdStickman;
		#endregion
		#region Properties 
		#endregion
		#region MonoBehaviour Methods
		private void Start()
		{		
			SetStickmanData(LevelManager.Instance.GetCurrentLevelData().StickmansTileData);
		}
		#endregion
		#region Methods
		void SetStickmanData(GridData gridData)
		{
			stickmanGridData = gridData;
			CreateStickmans();
		}
		void CreateStickmans()
		{
			for (int i = 0; i < stickmanGridData.GridTiles.Count; i++)
			{
				if (stickmanGridData.GridTiles[i].ObjectPoolID == PoolID.Stickman)
				{
					createdStickman = PoolingManager.Instance.Instantiate(PoolID.Stickman, stickmanParent.transform);
					createdStickman.transform.SetLocalPositionAndRotation(new Vector3(stickmanGridData.GridTiles[i].X, 0, stickmanGridData.GridTiles[i].Z), Quaternion.identity);
					createdStickman.GetComponent<MeshColorSet>().SetColor(stickmanGridData.GridTiles[i].Color);
				}
			}
		}
		#endregion
#if UNITY_EDITOR
		public void SetStickmanDataEditor(GridData gridData, PoolObject stickmanObj)
		{
			CreateStickmanEditor(gridData, stickmanObj);
		}
		void CreateStickmanEditor(GridData gridData, PoolObject stickmanObj)
		{
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