using Base.Utilities.Events;
using TMPro;
using UnityEngine;
namespace MoneyManager
{
    public class MoneyTextSet : MonoBehaviour
    {
        #region Variables
        [SerializeField] TextMeshProUGUI coinText;
		#endregion
		#region Properties 

		#endregion
		#region MonoBehaviour Methods
		private void OnEnable()
		{
			EventManager.OnMoneyUpdated.AddListener(UpdateText);
		}
		private void OnDisable()
		{
			EventManager.OnMoneyUpdated.RemoveListener(UpdateText);
		}
		#endregion
		#region My Methods
		void UpdateText(int value)
        {
            coinText.text = value.ToString();
        }
        #endregion
    }
}