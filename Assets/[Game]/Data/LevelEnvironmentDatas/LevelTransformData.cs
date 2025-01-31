using Base.Global.Enums;
using System.Collections.Generic;
using UnityEngine;
namespace LevelSaveSystem
{
	[CreateAssetMenu(fileName = "TransformData-Level-", menuName = "Level Transform Data")]
	public class LevelTransformData : ScriptableObject
	{
		public int Level;
		public List<LevelTransform> LevelTransforms;
	}
	[System.Serializable]
	public class LevelTransform
	{
		public PoolID PoolID;
		public Vector3 Position;
		public Quaternion Rotation;
		public Vector3 Scale;
	}
}