using Base.Global.Enums;
using Base.Utilities;
using System.Collections.Generic;
using Base.UI;
namespace Base.Managers
{
    public class UIManager : Singleton<UIManager>
    {
        #region Variables

        #endregion
        #region Properties 
        public Dictionary<PanelID, IPanel> PanelsByID { get; private set; } = new();

        #endregion
        #region MonoBehaviour Methods
        #endregion
        #region My Methods
        public void ShowPanel(PanelID panelID)
        {
            if (!PanelsByID.ContainsKey(panelID))
                return;

            PanelsByID[panelID].ShowPanelAnimated();
        }

        public void HidePanel(PanelID panelID)
        {
            if (!PanelsByID.ContainsKey(panelID))
                return;

            PanelsByID[panelID].HidePanelAnimated();
        }

        public void HideAllPanels()
        {
            foreach (var panel in PanelsByID.Values)
            {
                panel.HidePanelAnimated();
            }
        }

        public void AddPanel(IPanel panel)
        {
            if (PanelsByID.ContainsKey(panel.PanelID))
                return;

            PanelsByID.Add(panel.PanelID, panel);
        }

        public void RemovePanel(IPanel panel)
        {
            if (!PanelsByID.ContainsKey(panel.PanelID))
                return;

            PanelsByID.Remove(panel.PanelID);
        }
        #endregion


    }
}
