using Base.Global.Enums;
using Base.Managers;
using Base.Utilities.GlobalVariable;
using Base.Utilities.SaveLoadManager;
using GridSystem;
using System.Collections.Generic;
using UnityEngine;
namespace GameSaveLoad
{
	public class GameSaveLoadControl : MonoBehaviour
	{
		#region Variables
		#endregion
		#region Properties 

		#endregion
		#region MonoBehaviour Methods
		private void OnEnable()
		{
			LevelManager.OnLevelStart.AddListener(LoadDatas);
			LevelManager.OnLevelFinish.AddListener(ClearDatas);
		}
		private void OnDisable()
		{
			LevelManager.OnLevelStart.RemoveListener(LoadDatas);
			LevelManager.OnLevelFinish.RemoveListener(ClearDatas);
		}
		#endregion
		#region Methods
		void LoadDatas()
		{
			//LoadStickmanList();
		}
		void ClearDatas()
		{
			SaveLoad.DeleteKey(GlobalVariables.StickmanSaveKey);
			SaveLoad.DeleteKey(GlobalVariables.BusColorsSaveKey);
			SaveLoad.DeleteKey(GlobalVariables.BusPassengersSaveKey);
		}
		internal void SaveStickmanListData(List<GridTile> list)
		{
			SaveLoad.SetList<GridTile>(GlobalVariables.StickmanSaveKey, list);
		}
		internal List<GridTile> LoadStickmanListData()
		{
			return SaveLoad.GetList<GridTile>(GlobalVariables.StickmanSaveKey);
		}
		internal void SaveBusDatas(List<Colors> busSavedColors, List<int> busSavedPassengers)
		{
			SaveLoad.SetList<Colors>(GlobalVariables.BusColorsSaveKey, busSavedColors);
			SaveLoad.SetList<int>(GlobalVariables.BusPassengersSaveKey, busSavedPassengers);
		}
		internal List<Colors> GetBusColorsData()
		{
			return SaveLoad.GetList<Colors>(GlobalVariables.BusColorsSaveKey);
		}
		internal List<int> GetBusPassengersData()
		{
			return SaveLoad.GetList<int>(GlobalVariables.BusPassengersSaveKey);
		}
		#endregion
	}
}