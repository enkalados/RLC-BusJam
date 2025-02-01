using GridSystem;
using UnityEngine;
namespace LevelDataSystem
{
	[CreateAssetMenu(fileName = "LevelData-", menuName = "Level Data")]
	public class LevelData : ScriptableObject
	{
		public int Level;
		public LevelTransformData LevelTransformData;
		public GridData TilesData;
		public GridData StickmansTileData;
	}
}