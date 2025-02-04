using Stickman;
using UnityEngine;
namespace GridSystem.WaitPlace
{
	public class WaitingTile : MonoBehaviour
	{
		#region Variables
		bool isEmpty = true;
		StickmanControl stickman;
		#endregion
		#region Properties 
		#endregion
		#region MonoBehaviour Methods
		#endregion
		#region Methods
		internal void SetTileEmpty(bool isEmpty, StickmanControl stickman)
		{
			this.isEmpty = isEmpty;
			this.stickman = stickman;
		}
		internal bool GetIsEmpty()
		{
			return isEmpty;
		}
		internal StickmanControl GetStickman() { return stickman; }
		#endregion
	}
}