using Base.Managers;
using Base.Utilities.Events;
using Base.Utilities.GlobalVariable;
using Base.Utilities.SaveLoadManager;
using UnityEngine;
namespace MoneyManager
{
	public class MoneyManager : MonoBehaviour
	{
		#region Variables
		int currentCoin;
		#endregion
		#region Properties 

		#endregion
		#region MonoBehaviour Methods
		private void Awake()
		{
			GetSavedCoin();
		}
		private void OnEnable()
		{
			EventManager.OnMoneyAdded.AddListener(AddCoin);
			EventManager.OnMoneyRemoved.AddListener(RemoveCoin);
			LevelManager.OnLevelSuccess.AddListener(SaveCoin);
		}
		private void OnDisable()
		{
			EventManager.OnMoneyAdded.RemoveListener(AddCoin);
			EventManager.OnMoneyRemoved.RemoveListener(RemoveCoin);
			LevelManager.OnLevelSuccess.RemoveListener(SaveCoin);
		}
		#endregion
		#region My Methods
		void AddCoin(int amount)
		{
			Debug.Log(amount + " coin added");
			currentCoin += amount;
			UpdateCoin();
		}
		void RemoveCoin(int amount)
		{
			Debug.Log(amount + " coin removed");
			currentCoin -= amount;
			SaveCoin();
			UpdateCoin();
		}
		void UpdateCoin()
		{
			EventManager.OnMoneyUpdated.Invoke(currentCoin);
		}
		void SaveCoin()
		{
			SaveLoad.SetInt(GlobalVariables.MoneySaveKey, currentCoin);
		}
		void GetSavedCoin()
		{
			currentCoin = SaveLoad.GetInt(GlobalVariables.MoneySaveKey);
			UpdateCoin();
		}
		#endregion
	}
}