using Base.Managers;
using UnityEngine;

namespace Base.UI
{
    public class StartPanelControl : MonoBehaviour
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
            UiPanel.ShowPanel();
        }
        #endregion
        #region Button Methods
        public void TapToStart()
        {
            if (LevelManager.Instance.GetLevelIsStarted())
            {
                return;
            }
            LevelManager.OnLevelStart.Invoke();
            UiPanel.HidePanel();
            UIManager.Instance.ShowPanel(Global.Enums.PanelID.InGamePanel);
        }
        #endregion
    }
}