using Base.Global.Enums;
using GridSystem;
using System.Collections.Generic;
using UnityEngine;
namespace LevelDataSystem
{
	[CreateAssetMenu(fileName = "LevelData-", menuName = "Level Data")]
	public class LevelData : ScriptableObject
	{
		public int Level;
		public int Timer;
		public int PlaceHoldersCount;
		public List<Colors> BusColorList = new List<Colors>();
		public LevelTransformData LevelTransformData;
		public GridData TilesData;
		public GridData StickmansTileData;
	}
}