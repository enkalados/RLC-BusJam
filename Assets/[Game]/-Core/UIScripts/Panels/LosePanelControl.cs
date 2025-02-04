using Base.Managers;
using UnityEngine;
namespace Base.UI
{
    public class LosePanelControl : MonoBehaviour
    {
        #region Variables

        #endregion
        #region Properties 
        EasyPanel uiPanel;
        EasyPanel UiPanel { get { return (uiPanel == null) ? uiPanel = GetComponent<EasyPanel>() : uiPanel; } }
        #endregion
        #region MonoBehaviour Methods
        private void OnEnable()
        {
            UiPanel.HidePanel();
        }
		#endregion
		#region Button Methods
		public void RestartLevelButton()
        {
            LevelManager.OnRestartLevel.Invoke();
            UiPanel.HidePanel();
        }
        #endregion
    }
}