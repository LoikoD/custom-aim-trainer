using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralSettingsController : MonoBehaviour
{
    private GameManager gm;

    [SerializeField] private SliderFieldPanelController sensitivityPanel;

    void Awake()
    {
        gm = GameManager.Instance;
        UpdatePanels();

        // Init UI
        UpdatePanels();

    }

    private void UpdatePanels()
    {
        sensitivityPanel.UpdateSlider(gm.Sensitivity);
        sensitivityPanel.UpdateField(gm.Sensitivity.ToString("R"));
    }

    #region Sensitivity
    private void ChangeSensitivity(float value)
    {
        gm.Sensitivity = value;
    }
    public void OnChangeSensitivitySlider(float sliderValue)
    {
        float value = sensitivityPanel.OnChangeSlider(sliderValue);

        ChangeSensitivity(value);
    }
    public void OnChangeSensitivityField(string valueStr)
    {
        float value = sensitivityPanel.OnChangeField(valueStr);

        ChangeSensitivity(value);
    }
    public void OnEndEditSensitivityField(string valueStr)
    {
        sensitivityPanel.OnEndEditField(valueStr);
    }
    // -------------------
    #endregion
}
