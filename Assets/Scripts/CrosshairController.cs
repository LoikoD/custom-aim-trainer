using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    GameManager gm;
    Crosshair crosshair;
    
    [SerializeField] CanvasScaler scaler;

    void Start()
    {
        gm = GameManager.Instance;
        crosshair = new Crosshair(gameObject);

    }
    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("Applying crosshair config");
            ApplyCrosshairCfg();
        }
    }

    private void ApplyCrosshairCfg()
    {
        // Dot size
        crosshair.dot.sizeDelta = Vector2.one * gm.crosshairCfg.dotSize;

        // Length and width of lines
        crosshair.leftLine.sizeDelta = new Vector2(gm.crosshairCfg.lineLength, gm.crosshairCfg.lineWidth);
        crosshair.rightLine.sizeDelta = new Vector2(gm.crosshairCfg.lineLength, gm.crosshairCfg.lineWidth);
        crosshair.topLine.sizeDelta = new Vector2(gm.crosshairCfg.lineWidth, gm.crosshairCfg.lineLength);
        crosshair.bottomLine.sizeDelta = new Vector2(gm.crosshairCfg.lineWidth, gm.crosshairCfg.lineLength);

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
}
