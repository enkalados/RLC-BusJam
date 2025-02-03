using Base.Managers;
using DG.Tweening;
using Stickman;
using System.Collections.Generic;
using UnityEngine;
namespace BusSystem
{
	public class BusPassengerControl : MonoBehaviour
	{
		#region Variables
		Queue<BusControl> busControls = new Queue<BusControl>();
		List<GameObject> waitPlaces = new List<GameObject>();
		int fullWaitPlaceCount = 0;
		#endregion
		#region Properties 
		#endregion
		#region MonoBehaviour Methods
		private void OnEnable()
		{
			LevelManager.OnLevelStart.AddListener(() => fullWaitPlaceCount = 0);
		}
		private void OnDisable()
		{
			
		}
		#endregion
		#region Methods

		internal void SetWaitPlacesList(List<GameObject> places)
		{
			waitPlaces.Clear();
			waitPlaces = places;
		}
		internal void SetBusControlsPlacesList(List<GameObject> busList)
		{
			busControls.Clear();
			for (int i = 0; i < busList.Count; i++)
			{
				busControls.Enqueue(busList[i].GetComponent<BusControl>());
			}
		}
		internal void CheckPassenger(StickmanControl passenger)
		{
			if (passenger.GetColor() == busControls.Peek().GetBusColor())
			{
				passenger.transform.DOMove(busControls.Peek().transform.position, .5f);
			}
			else if (fullWaitPlaceCount < waitPlaces.Count)
			{
				passenger.transform.DOMove(waitPlaces[fullWaitPlaceCount].transform.position, .5f);
				fullWaitPlaceCount++;
			}
		}
		#endregion
	}
}