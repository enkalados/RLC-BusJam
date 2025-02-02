using Base.Global.Enums;
using UnityEngine;
namespace Stickman
{
	public class StickmanControl : MonoBehaviour
	{
		#region Variables
		Colors color;
		#endregion
		#region Properties 
		#endregion
		#region MonoBehaviour Methods
		#endregion
		#region Methods
		internal void SetStickmanColor(Colors color)
		{
			this.color = color;
		}
		#endregion
	}
}