using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    private GameManager gm;

    [SerializeField] private RectTransform dot;
    [SerializeField] private RectTransform leftLine;
    [SerializeField] private RectTransform rightLine;
    [SerializeField] private RectTransform topLine;
    [SerializeField] private RectTransform bottomLine;

    private Image dotImage;
    private Image leftLineImage;
    private Image rightLineImage;
    private Image topLineImage;
    private Image bottomLineImage;

    private void Awake()
    {
        dotImage = dot.GetComponent<Image>();
        leftLineImage = leftLine.GetComponent<Image>();
        rightLineImage = rightLine.GetComponent<Image>();
        topLineImage = topLine.GetComponent<Image>();
        bottomLineImage = bottomLine.GetComponent<Image>();

        gm = GameManager.Instance;
    }

    private void Start()
    {
        ApplyCrosshairCfg();
    }

    private void ApplyCrosshairCfg()
    {
        // Dot size
        UpdateDotSize();

        // Length and width of lines
        UpdateLines();

        // Gap
        UpdateGap();

        // Color
        UpdateColor();

    }
    public void UpdateColor()
    {
        dotImage.color = gm.crosshairCfg.color;
        leftLineImage.color = gm.crosshairCfg.color;
        rightLineImage.color = gm.crosshairCfg.color;
        topLineImage.color = gm.crosshairCfg.color;
        bottomLineImage.color = gm.crosshairCfg.color;
    }
    public void UpdateDotSize()
    {
        dot.sizeDelta = Vector2.one * gm.crosshairCfg.dotSize;
    }
    public void UpdateLines()
    {
        leftLine.sizeDelta = new Vector2(gm.crosshairCfg.lineLength, gm.crosshairCfg.lineThickness);
        rightLine.sizeDelta = new Vector2(gm.crosshairCfg.lineLength, gm.crosshairCfg.lineThickness);
        topLine.sizeDelta = new Vector2(gm.crosshairCfg.lineThickness, gm.crosshairCfg.lineLength);
        bottomLine.sizeDelta = new Vector2(gm.crosshairCfg.lineThickness, gm.crosshairCfg.lineLength);
    }
    public void UpdateGap()
    {
        leftLine.anchoredPosition = Vector2.left * gm.crosshairCfg.gap;
        rightLine.anchoredPosition = Vector2.right * gm.crosshairCfg.gap;
        topLine.anchoredPosition = Vector2.up * gm.crosshairCfg.gap;
        bottomLine.anchoredPosition = Vector2.down * gm.crosshairCfg.gap;
    }
}
