using Base.Managers;
using Base.Utilities.GlobalVariable;
using Base.Utilities.SaveLoadManager;
using GridSystem;
using Stickman.Creator;
using System.Collections.Generic;
using UnityEngine;
namespace GameSaveLoad
{
	public class GameSaveLoadControl : MonoBehaviour
	{
		#region Variables
		#endregion
		#region Properties 
		//StickmanCreator stickmanCreator;
		//StickmanCreator StickmanCreator => (stickmanCreator == null) ? stickmanCreator = GetComponent<StickmanCreator>() : stickmanCreator;
		#endregion
		#region MonoBehaviour Methods
		private void OnEnable()
		{
			LevelManager.OnLevelStart.AddListener(LoadDatas);
		}
		private void OnDisable()
		{
			LevelManager.OnLevelStart.RemoveListener(LoadDatas);
		}
		#endregion
		#region Methods
		void LoadDatas()
		{
			//LoadStickmanList();
		}
		internal void SaveStickmanList(List<GridTile> list)
		{
			SaveLoad.SetList<GridTile>(GlobalVariables.StickmanSaveKey, list);
		}
		internal List<GridTile> LoadStickmanList()
		{
			return SaveLoad.GetList<GridTile>(GlobalVariables.StickmanSaveKey);
		}
		#endregion
	}
}