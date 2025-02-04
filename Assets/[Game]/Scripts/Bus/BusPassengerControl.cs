using Base.Utilities.Events;
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
		#endregion
		#region Properties 
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
			waitPlaces = places;
		}

		internal void CheckPassenger(StickmanControl passenger)
		{
			if (passenger.GetColor() == busList.Peek().GetBusColor() && busList.Peek().HaveEmptySeat())
			{
				busList.Peek().TakeSeat();
				passenger.MoveToBus(busList.Peek().GetEmptySeat(), this, busList.Peek());

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
		}
		internal void CheckNewBus()
		{
			busList.Peek().MoveFullBus();
			busList.Dequeue();
			foreach (var bus in busList)
			{
				bus.MoveNextBusPos();
			}
			StartCoroutine(CheckWaitingpassengersCO());
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
		#endregion
	}
}