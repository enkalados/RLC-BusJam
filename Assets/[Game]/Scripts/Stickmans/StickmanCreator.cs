using Base.Global.Enums;
using Base.Managers;
using Base.Pool;
using GridSystem;
using MeshColorSetter;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace Stickman.Creator
{
	public class StickmanCreator : MonoBehaviour
	{
		#region Variables
		[SerializeField] GameObject stickmanParent;
		GridData stickmanGridData;
		List<GameObject> createdStickmanList = new List<GameObject>();
		#endregion
		#region Properties 
		GridStickmanControl gridStickman;
		GridStickmanControl GridStickmanControl => (gridStickman == null) ? gridStickman = GetComponent<GridStickmanControl>() : gridStickman;
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
			SetStickmanData(LevelManager.Instance.GetCurrentLevelData().StickmansTileData);
			GridStickmanControl.SetStickmans(createdStickmanList);
		}
		void SetStickmanData(GridData gridData)
		{
			if (stickmanGridData == null)
			{
				stickmanGridData = gridData;
				CreateStickmans();
			}
			else if (!stickmanGridData.GridTiles.SequenceEqual(gridData.GridTiles))
			{
				stickmanGridData = gridData;
				DestroyStickmans();
				CreateStickmans();
			}
			else
			{
				ResetStickmans();
			}
		}
		void DestroyStickmans()
		{
			foreach (GameObject item in createdStickmanList)
			{
				PoolingManager.Instance.DestroyPoolObject(item.GetComponent<PoolObject>());
			}
		}
		void ResetStickmans()
		{
            foreach (GameObject item in createdStickmanList)
            {
				item.GetComponent<StickmanControl>().ResetStickman(stickmanParent);
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