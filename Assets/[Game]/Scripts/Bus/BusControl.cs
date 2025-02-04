using Base.Global.Enums;
using DG.Tweening;
using UnityEngine;
namespace BusSystem
{
	public class BusControl : MonoBehaviour
	{
		#region Variables
		[SerializeField] GameObject[] seats;
		int totalPassenger = 0;
		Colors color;
		const float MOVE_FORWARD = 20;
		const float MOVE_DURAITON = .5f;
		float distanceBetweenBus;
		float startXPos;
		#endregion
		#region Properties 
		#endregion
		#region MonoBehaviour Methods
		#endregion
		#region Methods
		internal bool HaveEmptySeat()
		{
			return totalPassenger < seats.Length;
		}
		internal void TakeSeat() { totalPassenger++; }
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
			return seats[totalPassenger-1].transform.position;
		}
		internal void MoveFullBus()
		{
			if(totalPassenger == seats.Length)
			{
				transform.DOMoveX(transform.position.x + MOVE_FORWARD, MOVE_DURAITON);
			}
		}
		internal void MoveNextBusPos()
		{
			transform.DOMoveX(transform.position.x + distanceBetweenBus, MOVE_DURAITON);
		}
		internal void ResetBus()
		{
			totalPassenger = 0;
			transform.localPosition = new Vector3(0, 0, startXPos);
		}
		#endregion
	}
}