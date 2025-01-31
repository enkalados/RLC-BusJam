using Base.Managers;
using UnityEngine;

public class DebugMenuControl : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject debugMenuButtons;
    #endregion
    #region Properties 

    #endregion
    #region MonoBehaviour Methods

    #endregion
    #region My Methods
    public void SetActiveDebugMenuButtons()
    {
        debugMenuButtons.SetActive(!debugMenuButtons.activeSelf);
    }
    public void WinButton()
    {
        LevelManager.Instance.CompleteLevel(true);
    }
    public void LoseButton()
    {
		LevelManager.Instance.CompleteLevel(false);
    }
    #endregion
}
