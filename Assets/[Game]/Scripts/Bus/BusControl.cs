using Base.Global.Enums;
using UnityEngine;
namespace BusSystem
{
	public class BusControl : MonoBehaviour
	{
		#region Variables
		Colors color;
		#endregion
		#region Properties 
		#endregion
		#region MonoBehaviour Methods
		#endregion
		#region Methods
		internal void SetBusColor(Colors color)
		{
			this.color = color;
		}
		internal Colors GetBusColor()
		{
			return color;
		}
		#endregion
	}
}