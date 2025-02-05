using Base.Global.Enums;
using Base.Managers;
using Base.Pool;
using Base.Utilities.Events;
using GameSaveLoad;
using GridSystem.WaitPlace;
using MeshColorSetter;
using Stickman;
using Stickman.Animation;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
		List<Colors> savedWaitingTiles = new List<Colors>();
		List<GameObject> waitingPassengersList = new List<GameObject>();
		#endregion
		#region Properties 
		GameSaveLoadControl gameSaveLoad;
		GameSaveLoadControl GameSaveLoad => (gameSaveLoad == null) ? gameSaveLoad = GetComponent<GameSaveLoadControl>() : gameSaveLoad;
		#endregion
		#region MonoBehaviour Methods
		private void OnEnable()
		{
			EventManager.OnBusFull.AddListener(CheckNewBus);
			LevelManager.OnLevelFinish.AddListener(ClearWaitingPassengersData);
		}
		private void OnDisable()
		{
			EventManager.OnBusFull.RemoveListener(CheckNewBus);
			LevelManager.OnLevelFinish.RemoveListener(ClearWaitingPassengersData);
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
			LoadSavedWaitingPassengers();
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
			SaveWaitTiles();
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
			if (busList.Count > 0)
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
				SaveWaitTiles();
			}
		}
		#region Save Load
		void LoadSavedWaitingPassengers()
		{
			savedWaitingTiles = GameSaveLoad.GetWaitingPassengers().ToList();
            for (int i = 0; i < savedWaitingTiles.Count; i++)
            {
				GameObject waitingPassenger = PoolingManager.Instance.Instantiate(PoolID.Stickman, null).gameObject;
				waitingPassenger.GetComponent<MeshColorSet>().SetColor(savedWaitingTiles[i]);
				waitingPassenger.GetComponent<StickmanControl>().SetStickmanColor(savedWaitingTiles[i]);
				waitingPassenger.transform.SetLocalPositionAndRotation(waitPlaces[i].transform.position, Quaternion.identity);
				waitingPassengersList.Add(waitingPassenger);

				waitingPassenger.GetComponent<StickmanControl>().MoveToTile(waitPlaces.First(tile => tile.GetIsEmpty()).gameObject);
				waitPlaces.First(tile => tile.GetIsEmpty()).SetTileEmpty(false, waitingPassenger.GetComponent<StickmanControl>());
				fullWaitPlaceCount++;
			}
		}
		void SaveWaitTiles()
		{
			savedWaitingTiles.Clear();
			for (int i = 0; i < waitPlaces.Count; i++)
			{
				if (!waitPlaces[i].GetIsEmpty())
				{
					savedWaitingTiles.Add(waitPlaces[i].GetStickman().GetColor());
				}
			}
			GameSaveLoad.SaveWaitingPassengers(savedWaitingTiles);
		}
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
		void ClearWaitingPassengersData()
		{
			for (int i = 0; i < waitingPassengersList.Count; i++)
			{
				PoolingManager.Instance.DestroyPoolObject(waitingPassengersList[i].GetComponent<PoolObject>());
			}
			waitingPassengersList.Clear();
		}
		void ClearBusSaveDatas()
		{
			busSavedColors.Clear();
			busSavedPassengers.Clear();
			savedWaitingTiles.Clear();
        }
		void SaveBusParams()
		{
			ClearBusSaveDatas();
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