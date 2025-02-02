using Base.Global.Enums;
using UnityEngine;
namespace Base.Pool
{
	public class PoolObject : MonoBehaviour
	{
		#region Variables
		public PoolID PoolID;
		Vector3 defaultScale;
		#endregion
		#region Properties 
		#endregion
		#region MonoBehaviour Methods
		#endregion
		#region My Methods
		internal void Initialize()
		{
			//transform.localScale = defaultScale;
		}
		#endregion
	}
}