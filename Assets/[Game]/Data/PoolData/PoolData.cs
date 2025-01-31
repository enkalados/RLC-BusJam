using System.Collections.Generic;
using UnityEngine;
namespace Base.Pool
{
	[CreateAssetMenu(fileName = "PoolObjects", menuName = "Pool Object Data")]
	public class PoolData : ScriptableObject
	{
		public List<Pool> PoolHolder = new ();

	}
}