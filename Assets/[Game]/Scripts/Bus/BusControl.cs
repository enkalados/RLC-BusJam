using Base.Global.Enums;
using Base.Pool;
using Base.Utilities.Events;
using DG.Tweening;
using MeshColorSetter;
using Stickman;
using System.Collections.Generic;
using UnityEngine;
namespace BusSystem
{
	public class BusControl : MonoBehaviour
	{
		#region Variables
		[SerializeField] GameObject[] seats;
		int arrivedPassengers = 0;
		int totalPassenger = 0;
		Colors color;
		const float MOVE_FORWARD = 20;
		const float MOVE_DURAITON = .5f;
		float distanceBetweenBus;
		float startXPos;

		List<GameObject> loadedPassengers = new List<GameObject>();
		#endregion
		#region Properties 
		#endregion
		#region MonoBehaviour Methods
		#endregion
		#region Methods
		internal int GetTotalPassenger() { return totalPassenger; }
		internal bool HaveEmptySeat()
		{
			return totalPassenger < seats.Length;
		}
		internal void TakeSeat()
		{
			totalPassenger++;
		}
		internal void SetBusColor(Colors color)
		{
			this.color = color;
		}
		internal void SetCreateValues(float distance, float startX)
		{
			distanceBetweenBus = Mathf.Abs(distance);
			startXPos = startX;
		}
		internal Colors GetBusColor()
		{
			return color;
		}
		internal Vector3 GetEmptySeat()
		{
			return seats[totalPassenger - 1].transform.position;
		}
		internal void MoveFullBus()
		{
			transform.DOMoveX(transform.position.x + MOVE_FORWARD, MOVE_DURAITON);
		}
		internal void MoveNextBusPos()
		{
			transform.DOMoveX(transform.position.x + distanceBetweenBus, MOVE_DURAITON);
		}
		internal void PassengerArrived()
		{
			arrivedPassengers++;
			if (totalPassenger == seats.Length && totalPassenger == arrivedPassengers)
			{
				EventManager.OnBusFull.Invoke();
			}
		}
		internal void ResetBus()
		{
			arrivedPassengers = 0;
			totalPassenger = 0;
			transform.localPosition = new Vector3(0, 0, startXPos);
			ClearData();
		}
		#region Save Load
		void ClearData()
		{
			if (loadedPassengers.Count > 0)
			{
				for (int i = 0; i < loadedPassengers.Count; i++)
				{
					PoolingManager.Instance.DestroyPoolObject(loadedPassengers[i].GetComponent<PoolObject>());
				}
			}
		}
		internal void LoadPassengers(int passenger)
		{
			totalPassenger = 0;
			arrivedPassengers = passenger;
			for (int i = 0; i < passenger; i++)
			{
				totalPassenger++;
				GameObject loadedPassenger = PoolingManager.Instance.Instantiate(PoolID.Stickman, null).gameObject;
				loadedPassenger.GetComponent<MeshColorSet>().SetColor(color);
				loadedPassenger.transform.SetLocalPositionAndRotation(seats[totalPassenger - 1].transform.position, Quaternion.identity);
				loadedPassenger.transform.SetParent(seats[totalPassenger - 1].transform);
				loadedPassengers.Add(loadedPassenger);
			}
		}
		#endregion
		#endregion
	}
}