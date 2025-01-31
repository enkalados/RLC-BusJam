using UnityEditor;
using UnityEngine;
namespace Base.Managers
{
	[CustomEditor(typeof(LevelManager))]
	public class LevelManagerGetLevelsInEditor : Editor
	{
		#region Variables
		#endregion
		#region Properties 
		#endregion
		#region MonoBehaviour Methods
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			LevelManager levelManager = (LevelManager)target;

			if (GUILayout.Button("Get Levels"))
			{
				levelManager.LoadLevelData();
			}
		}
		#endregion
		#region Methods
		#endregion
	}
}