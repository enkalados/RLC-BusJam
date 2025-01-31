using Base.Global.Enums;

namespace Base.UI
{
    public interface IPanel
    {
        PanelID PanelID { get; }
        void ShowPanelAnimated();
        void HidePanelAnimated();
    }

}