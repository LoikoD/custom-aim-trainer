using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    GameManager gm;
    Crosshair crosshair;

    #region SliderFieldPanel fields declaration

    // Dot
    [SerializeField] SliderFieldPanel dotPanel;
    // Line Length
    [SerializeField] SliderFieldPanel lengthPanel;
    // Line Thickness
    [SerializeField] SliderFieldPanel thicknessPanel;
    // Line Gap
    [SerializeField] SliderFieldPanel gapPanel;
    // Color
    [SerializeField] SliderFieldPanel redPanel;
    [SerializeField] SliderFieldPanel greenPanel;
    [SerializeField] SliderFieldPanel bluePanel;
    [SerializeField] SliderFieldPanel opacityPanel;

    #endregion


    void Start()
    {
        gm = GameManager.Instance;
        crosshair = new Crosshair(gameObject);

        ApplyCrosshairCfg();

    }

    private void ApplyCrosshairCfg()
    {
        // Dot size
        crosshair.dot.sizeDelta = Vector2.one * gm.crosshairCfg.dotSize;

        // Length and width of lines
        crosshair.leftLine.sizeDelta = new Vector2(gm.crosshairCfg.lineLength, gm.crosshairCfg.lineThickness);
        crosshair.rightLine.sizeDelta = new Vector2(gm.crosshairCfg.lineLength, gm.crosshairCfg.lineThickness);
        crosshair.topLine.sizeDelta = new Vector2(gm.crosshairCfg.lineThickness, gm.crosshairCfg.lineLength);
        crosshair.bottomLine.sizeDelta = new Vector2(gm.crosshairCfg.lineThickness, gm.crosshairCfg.lineLength);

        // Gap
        crosshair.leftLine.anchoredPosition = Vector2.left * gm.crosshairCfg.gap;
        crosshair.rightLine.anchoredPosition = Vector2.right * gm.crosshairCfg.gap;
        crosshair.topLine.anchoredPosition = Vector2.up * gm.crosshairCfg.gap;
        crosshair.bottomLine.anchoredPosition = Vector2.down * gm.crosshairCfg.gap;

        // Color
        crosshair.dot.GetComponent<Image>().color = gm.crosshairCfg.color;
        crosshair.leftLine.GetComponent<Image>().color = gm.crosshairCfg.color;
        crosshair.rightLine.GetComponent<Image>().color = gm.crosshairCfg.color;
        crosshair.topLine.GetComponent<Image>().color = gm.crosshairCfg.color;
        crosshair.bottomLine.GetComponent<Image>().color = gm.crosshairCfg.color;

    }
    private void ChangeColor()
    {
        crosshair.dot.GetComponent<Image>().color = gm.crosshairCfg.color;
        crosshair.leftLine.GetComponent<Image>().color = gm.crosshairCfg.color;
        crosshair.rightLine.GetComponent<Image>().color = gm.crosshairCfg.color;
        crosshair.topLine.GetComponent<Image>().color = gm.crosshairCfg.color;
        crosshair.bottomLine.GetComponent<Image>().color = gm.crosshairCfg.color;

        redPanel.ChangeSliderColor(gm.crosshairCfg.color);
        greenPanel.ChangeSliderColor(gm.crosshairCfg.color);
        bluePanel.ChangeSliderColor(gm.crosshairCfg.color);
        opacityPanel.ChangeSliderColor(gm.crosshairCfg.color);
    }

    public void UpdatePanels()
    {
        // Sliders
        dotPanel.UpdateSlider(gm.crosshairCfg.dotSize);
        lengthPanel.UpdateSlider(gm.crosshairCfg.lineLength);
        thicknessPanel.UpdateSlider(gm.crosshairCfg.lineThickness);
        gapPanel.UpdateSlider(gm.crosshairCfg.gap);
        redPanel.UpdateSlider(gm.crosshairCfg.color.r);
        greenPanel.UpdateSlider(gm.crosshairCfg.color.g);
        bluePanel.UpdateSlider(gm.crosshairCfg.color.b);
        opacityPanel.UpdateSlider(gm.crosshairCfg.color.a);

        // Fields
        dotPanel.UpdateField(gm.crosshairCfg.dotSize.ToString("R"));
        lengthPanel.UpdateField(gm.crosshairCfg.lineLength.ToString("R"));
        thicknessPanel.UpdateField(gm.crosshairCfg.lineThickness.ToString("R"));
        gapPanel.UpdateField(gm.crosshairCfg.gap.ToString("R"));
        redPanel.UpdateField(gm.crosshairCfg.color.r.ToString("R"));
        greenPanel.UpdateField(gm.crosshairCfg.color.g.ToString("R"));
        bluePanel.UpdateField(gm.crosshairCfg.color.b.ToString("R"));
        opacityPanel.UpdateField(gm.crosshairCfg.color.a.ToString("R"));

    }


    #region DotSize
    void ChangeDotSize(float dotSize)
    {
        // Save to gm
        gm.crosshairCfg.dotSize = dotSize;

        // Update UI
        crosshair.dot.sizeDelta = Vector2.one * gm.crosshairCfg.dotSize;
    }
    public void OnChangeDotSlider(float sliderValue)
    {
        dotPanel.OnChangeSlider(sliderValue);

        ChangeDotSize(sliderValue);
    }
    public void OnChangeDotField(string valueStr)
    {
        float value = dotPanel.OnChangeField(valueStr);

        ChangeDotSize(value);
    }
    public void OnEndEditDotField(string valueStr)
    {
        dotPanel.OnEndEditField(valueStr);
    }
    #endregion

    #region LineLength
    void ChangeLineLength(float lineLength)
    {
        // Save to gm
        gm.crosshairCfg.lineLength = lineLength;

        // Update UI
        crosshair.leftLine.sizeDelta = new Vector2(gm.crosshairCfg.lineLength, gm.crosshairCfg.lineThickness);
        crosshair.rightLine.sizeDelta = new Vector2(gm.crosshairCfg.lineLength, gm.crosshairCfg.lineThickness);
        crosshair.topLine.sizeDelta = new Vector2(gm.crosshairCfg.lineThickness, gm.crosshairCfg.lineLength);
        crosshair.bottomLine.sizeDelta = new Vector2(gm.crosshairCfg.lineThickness, gm.crosshairCfg.lineLength);
    }
    public void OnChangeLengthSlider(float sliderValue)
    {
        lengthPanel.OnChangeSlider(sliderValue);

        ChangeLineLength(sliderValue);
    }
    public void OnChangeLengthField(string valueStr)
    {
        float value = lengthPanel.OnChangeField(valueStr);

        ChangeLineLength(value);
    }
    public void OnEndEditLengthField(string valueStr)
    {
        lengthPanel.OnEndEditField(valueStr);
    }
    #endregion

    #region LineThickness
    void ChangeLineThickness(float lineThickness)
    {
        // Save to gm
        gm.crosshairCfg.lineThickness = lineThickness;

        // Update UI
        crosshair.leftLine.sizeDelta = new Vector2(gm.crosshairCfg.lineLength, gm.crosshairCfg.lineThickness);
        crosshair.rightLine.sizeDelta = new Vector2(gm.crosshairCfg.lineLength, gm.crosshairCfg.lineThickness);
        crosshair.topLine.sizeDelta = new Vector2(gm.crosshairCfg.lineThickness, gm.crosshairCfg.lineLength);
        crosshair.bottomLine.sizeDelta = new Vector2(gm.crosshairCfg.lineThickness, gm.crosshairCfg.lineLength);
    }
    public void OnChangeThicknessSlider(float sliderValue)
    {
        thicknessPanel.OnChangeSlider(sliderValue);

        ChangeLineThickness(sliderValue);
    }
    public void OnChangeThicknessField(string valueStr)
    {
        float value = thicknessPanel.OnChangeField(valueStr);

        ChangeLineThickness(value);
    }
    public void OnEndEditThicknessField(string valueStr)
    {
        thicknessPanel.OnEndEditField(valueStr);
    }
    #endregion

    #region LineGap
    void ChangeLineGap(float lineGap)
    {
        // Save to gm
        gm.crosshairCfg.gap = lineGap;

        // Update UI
        crosshair.leftLine.anchoredPosition = Vector2.left * gm.crosshairCfg.gap;
        crosshair.rightLine.anchoredPosition = Vector2.right * gm.crosshairCfg.gap;
        crosshair.topLine.anchoredPosition = Vector2.up * gm.crosshairCfg.gap;
        crosshair.bottomLine.anchoredPosition = Vector2.down * gm.crosshairCfg.gap;
    }
    public void OnChangeGapSlider(float sliderValue)
    {
        gapPanel.OnChangeSlider(sliderValue);

        ChangeLineGap(sliderValue);
    }
    public void OnChangeGapField(string valueStr)
    {
        float value = gapPanel.OnChangeField(valueStr);

        ChangeLineGap(value);
    }
    public void OnEndEditGapField(string valueStr)
    {
        gapPanel.OnEndEditField(valueStr);
    }
    #endregion

    #region Red
    void ChangeRed(float value)
    {
        // Save to gm
        gm.crosshairCfg.color.r = value / 255;

        // Update UI
        ChangeColor();
    }
    public void OnChangeRedSlider(float sliderValue)
    {
        redPanel.OnChangeSlider(sliderValue);

        ChangeRed(sliderValue);
    }
    public void OnChangeRedField(string valueStr)
    {
        float value = redPanel.OnChangeField(valueStr);

        ChangeRed(value);
    }
    public void OnEndEditRedField(string valueStr)
    {
        redPanel.OnEndEditField(valueStr);
    }
    #endregion

    #region Green
    void ChangeGreen(float value)
    {
        // Save to gm
        gm.crosshairCfg.color.g = value / 255;

        // Update UI
        ChangeColor();
    }
    public void OnChangeGreenSlider(float sliderValue)
    {
        greenPanel.OnChangeSlider(sliderValue);

        ChangeGreen(sliderValue);
    }
    public void OnChangeGreenField(string valueStr)
    {
        float value = greenPanel.OnChangeField(valueStr);

        ChangeGreen(value);
    }
    public void OnEndEditGreenField(string valueStr)
    {
        greenPanel.OnEndEditField(valueStr);
    }
    #endregion

    #region Blue
    void ChangeBlue(float value)
    {
        // Save to gm
        gm.crosshairCfg.color.b = value / 255;

        // Update UI
        ChangeColor();
    }
    public void OnChangeBlueSlider(float sliderValue)
    {
        bluePanel.OnChangeSlider(sliderValue);

        ChangeBlue(sliderValue);
    }
    public void OnChangeBlueField(string valueStr)
    {
        float value = bluePanel.OnChangeField(valueStr);

        ChangeBlue(value);
    }
    public void OnEndEditBlueField(string valueStr)
    {
        bluePanel.OnEndEditField(valueStr);
    }
    #endregion

    #region Opacity
    void ChangeOpacity(float value)
    {
        // Save to gm
        gm.crosshairCfg.color.a = value;

        // Update UI
        ChangeColor();
    }
    public void OnChangeOpacitySlider(float sliderValue)
    {
        float value = opacityPanel.OnChangeSlider(sliderValue);

        ChangeOpacity(value);
    }
    public void OnChangeOpacityField(string valueStr)
    {
        float value = opacityPanel.OnChangeField(valueStr);

        ChangeOpacity(value);
    }
    public void OnEndEditOpacityField(string valueStr)
    {
        opacityPanel.OnEndEditField(valueStr);
    }
    #endregion
}
