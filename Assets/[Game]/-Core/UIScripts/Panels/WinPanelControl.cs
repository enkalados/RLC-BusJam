using Base.Managers;
using UnityEngine;
namespace Base.UI
{
    public class WinPanelControl : MonoBehaviour
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
		public void NextLevelButton()
        {
			LevelManager.OnNextLevel.Invoke();
			UiPanel.HidePanel();
            UIManager.Instance.HidePanel(Base.Global.Enums.PanelID.InGamePanel);
            UIManager.Instance.ShowPanel(Base.Global.Enums.PanelID.StartPanel);
        }
        #endregion
    }
}