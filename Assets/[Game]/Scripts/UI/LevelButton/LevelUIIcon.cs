using TMPro;
using UnityEngine;
namespace LevelUI
{
	public class LevelUIIcon : MonoBehaviour
	{
		#region Variables
		[SerializeField] GameObject selectedImage;
		[SerializeField] TextMeshProUGUI levelText;
		#endregion
		#region Properties 
		#endregion
		#region MonoBehaviour Methods
		#endregion
		#region Methods
		internal void SetLevelText(int level)
		{
			levelText.text = "" + level;
		}
		internal void DeselectLevel()
		{
			selectedImage.SetActive(false);
		}
		internal void SelectLevel()
		{
			selectedImage.SetActive(true);
		}
		#endregion
	}
}