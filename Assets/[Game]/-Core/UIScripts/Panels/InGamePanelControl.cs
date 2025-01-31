using Base.Managers;
using UnityEngine;
namespace Base.UI
{
    public class InGamePanelControl : MonoBehaviour
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
        #region My Methods

        #endregion
    }
}