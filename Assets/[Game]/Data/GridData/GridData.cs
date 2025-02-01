using Base.Global.Enums;
using UnityEngine;
namespace GridSystem
{
	[CreateAssetMenu(fileName = "GridData-Level-", menuName = "Grid Data")]
	public class GridData : ScriptableObject
	{
		public int GridX;
		public int GridZ;
		public GridTile[] GridTiles;
	}
	public class GridTile
	{
		public float X;
		public float Z;
		public PoolID ObjectPoolID;
	}
}