using Base.Global.Enums;
using System.Collections.Generic;
using UnityEngine;
namespace GridSystem
{
	[CreateAssetMenu(fileName = "GridData-Level-", menuName = "Grid Data")]
	public class GridData : ScriptableObject
	{
		public int GridX;
		public int GridZ;
		public List<GridTile> GridTiles;
	}
	[System.Serializable]
	public class GridTile
	{
		public int X;
		public int Z;
		public PoolID ObjectPoolID;
	}
}