using Base.Managers;
using System.Collections.Generic;
using UnityEngine;
namespace LevelUI
{
	public class LevelUIControl : MonoBehaviour
	{
		#region Variables
		[SerializeField] GameObject levelsParent;
		[SerializeField] GameObject levelUIPrefab;
		List<GameObject> levels = new List<GameObject>();
		int previousLevel = -1;
		#endregion
		#region Properties 
		#endregion
		#region MonoBehaviour Methods
		private void OnEnable()
		{
			LevelManager.OnLevelFinish.AddListener(DeselectPrevious);
		}
		private void OnDisable()
		{
			LevelManager.OnLevelFinish.RemoveListener(DeselectPrevious);
		}
		#endregion
		#region Methods
		internal void CreateButtons(int totalLevel)
		{
			for (int i = 0; i < totalLevel; i++)
			{
				levels.Add(Instantiate(levelUIPrefab, levelsParent.transform));
				levels[levels.Count - 1].GetComponent<LevelUIIcon>().SetLevelText(totalLevel - i);
			}
		}
		void DeselectPrevious()
		{
			if (previousLevel > 0)
			{
				levels[levels.Count - previousLevel].GetComponent<LevelUIIcon>().DeselectLevel();
			}
		}
		internal void SelecetCurrentLevel(int currentLevel)
		{
			levels[(levels.Count - currentLevel)].GetComponent<LevelUIIcon>().SelectLevel();
			previousLevel = currentLevel;
		}
		#endregion
	}
}