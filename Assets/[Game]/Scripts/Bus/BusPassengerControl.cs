using Base.Global.Enums;
using Base.Managers;
using Base.Utilities.Events;
using GameSaveLoad;
using GridSystem.WaitPlace;
using Stickman;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace BusSystem
{
	public class BusPassengerControl : MonoBehaviour
	{
		#region Variables
		Queue<BusControl> busList = new Queue<BusControl>();
		List<WaitingTile> waitPlaces = new List<WaitingTile>();
		WaitingTile selectedPassengerTile;
		int fullWaitPlaceCount = 0;
		const float DIST_BETWEEEN_BUS = 7;
		
		List<Colors> busSavedColors = new List<Colors>();
		List<int> busSavedPassengers = new List<int>();
		#endregion
		#region Properties 
		GameSaveLoadControl gameSaveLoad;
		GameSaveLoadControl GameSaveLoad => (gameSaveLoad == null) ? gameSaveLoad = GetComponent<GameSaveLoadControl>() : gameSaveLoad;
		#endregion
		#region MonoBehaviour Methods
		private void OnEnable()
		{
			EventManager.OnBusFull.AddListener(CheckNewBus);
		}
		private void OnDisable()
		{
			EventManager.OnBusFull.RemoveListener(CheckNewBus);
		}
		#endregion
		#region Methods
		internal void SetBusControlsPlacesList(List<GameObject> busList)
		{
			this.busList.Clear();
			for (int i = 0; i < busList.Count; i++)
			{
				this.busList.Enqueue(busList[i].GetComponent<BusControl>());
			}
			ResetDataAndBus();
		}
		void ResetDataAndBus()
		{
			fullWaitPlaceCount = 0;
			foreach (var bus in busList)
			{
				bus.ResetBus();
				bus.MoveNextBusPos();
			}
			foreach (WaitingTile tile in waitPlaces)
			{
				tile.SetTileEmpty(true, null);
			}
		}
		internal void SetWaitPlacesList(List<WaitingTile> places)
		{
			waitPlaces.Clear();
			waitPlaces = places.ToList();
		}

		internal void CheckPassenger(StickmanControl passenger)
		{
			if (passenger.GetColor() == busList.Peek().GetBusColor() && busList.Peek().HaveEmptySeat())
			{
				busList.Peek().TakeSeat();
				passenger.MoveToBus(busList.Peek().GetEmptySeat(), this, busList.Peek());
				SaveBusParams();
			}
			else if (fullWaitPlaceCount < waitPlaces.Count)
			{
				FillTheEmptyWaitPlace(passenger);
			}
		}
		void FillTheEmptyWaitPlace(StickmanControl passenger)
		{
			passenger.MoveToTile(waitPlaces.First(tile => tile.GetIsEmpty()).gameObject);
			waitPlaces.First(tile => tile.GetIsEmpty()).SetTileEmpty(false, passenger);
			fullWaitPlaceCount++;
			FailCheck();
		}
		void FailCheck()
		{
			if (fullWaitPlaceCount == waitPlaces.Count)
			{
				LevelManager.Instance.CompleteLevel(false);
			}
		}
		void CheckWin()
		{
			if (busList.Count == 0)
			{
				LevelManager.Instance.CompleteLevel(true);
			}
		}
		internal void CheckNewBus()
		{
			if(busList.Count > 0)
			{
				busList.Peek().MoveFullBus();
				busList.Dequeue();
				foreach (var bus in busList)
				{
					bus.MoveNextBusPos();
				}
				StartCoroutine(CheckWaitingpassengersCO());
				CheckWin();
				SaveBusParams();
			}
		}
		IEnumerator CheckWaitingpassengersCO()
		{
			yield return new WaitForSeconds(1);
			CheckWaitingPassengers();
		}
		void CheckWaitingPassengers()
		{
			if (busList.Count > 0)
			{
				for (int i = 0; i < waitPlaces.Count; i++)
				{
					if (!waitPlaces[i].GetIsEmpty() && waitPlaces[i].GetStickman().GetColor() == busList.Peek().GetBusColor())
					{
						busList.Peek().TakeSeat();
						waitPlaces[i].GetStickman().MoveToBus(busList.Peek().GetEmptySeat(), this, busList.Peek());
						waitPlaces[i].SetTileEmpty(true, null);
						fullWaitPlaceCount--;
					}
				}
			}
		}
		#region Save Load
		internal void LoadBusFromData(List<GameObject> busList, List<int> busSavedPassengers)
		{
            for (int i = 0; i < busList.Count; i++)
            {
				busList[i].GetComponent<BusControl>().LoadPassengers(busSavedPassengers[i]);
			}

			this.busList.Clear();
			for (int i = 0; i < busList.Count; i++)
			{
				this.busList.Enqueue(busList[i].GetComponent<BusControl>());
			}
			foreach (var bus in this.busList)
			{
				bus.MoveNextBusPos();
			}
		}
		void ClearSave()
		{
			busSavedColors.Clear();
			busSavedPassengers.Clear();
		}
		void SaveBusParams()
		{
			ClearSave();
			foreach (var item in busList)
            {
				busSavedColors.Add(item.GetBusColor());
				busSavedPassengers.Add(item.GetTotalPassenger());
			}
			GameSaveLoad.SaveBusDatas(busSavedColors, busSavedPassengers);
		}
		#endregion
		#endregion
	}
}