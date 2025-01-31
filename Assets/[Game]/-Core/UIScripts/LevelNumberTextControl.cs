using Base.Managers;
using Base.Utilities.GlobalVariable;
using Base.Utilities.SaveLoadManager;
using TMPro;
using UnityEngine;

public class LevelNumberTextControl : MonoBehaviour
{
    #region Variables

    #endregion
    #region Properties 
    TextMeshProUGUI levelNumberText;
    TextMeshProUGUI LevelNumberText { get { return (levelNumberText == null) ? levelNumberText = GetComponent<TextMeshProUGUI>() : levelNumberText; } }
    #endregion
    #region MonoBehaviour Methods
    private void OnEnable()
    {
        UpdateLevelText();
        LevelManager.OnLevelSuccess.AddListener(UpdateLevelText);
        LevelManager.OnLevelFail.AddListener(UpdateLevelText);
    }
    private void OnDisable()
    {
        LevelManager.OnLevelSuccess.RemoveListener(UpdateLevelText);
        LevelManager.OnLevelFail.RemoveListener(UpdateLevelText);
    }
    #endregion
    #region My Methods
    void UpdateLevelText()
    {
        LevelNumberText.text="LEVEL " + SaveLoad.GetInt(GlobalVariables.LastLevelNumberSaveKey, 1);
    }
    #endregion
}
