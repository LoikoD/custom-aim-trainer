using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class SliderFieldPanelController : MonoBehaviour
{
    public Slider slider;
    public TMP_InputField field;

    [SerializeField] float sliderMaxValue;
    [SerializeField] bool normalized;


    void Awake()
    {
        if (sliderMaxValue !=  slider.minValue)
        {
            slider.maxValue = sliderMaxValue;
            if (normalized)
            {
                field.characterLimit = (1f / sliderMaxValue).ToString("R").Length;
            }
            else
            {
                field.characterLimit = sliderMaxValue.ToString().Length;
            }
        }
    }

    public void ChangeSliderColor(Color color)
    {
        slider.fillRect.GetComponent<Image>().color = color;
    }


    public void UpdateSlider(float value)
    {
        float sliderValue = value;

        if (normalized)
        {
            sliderValue *= sliderMaxValue;
        }

        if (!slider.value.Equals(sliderValue))
        {
            // Will not invoke onValueChange callback
            slider.SetValueWithoutNotify(sliderValue);
        }
    }
    public void UpdateField(string value)
    {
        if (!field.text.Equals(value))
        {
            // Will not invoke onValueChange callback
            field.SetTextWithoutNotify(value);
        }
    }
    public float OnChangeSlider(float sliderValue)
    {
        float value = sliderValue;
        if  (normalized)
        {
            value = slider.normalizedValue;
        }

        // Update field
        string valueStr = value.ToString("R");
        UpdateField(valueStr);

        return value;
    }
    private float OnChangeFieldBase(string valueStr)
    {
        // Replace empty string with 0
        if (valueStr == string.Empty)
        {
            valueStr = "0";
        }

        // Parse string to float and clamp between 0 and max value
        float value = float.Parse(valueStr);

        if (normalized)
        {
            value = Mathf.Clamp01(value);
        }
        else
        {
            value = Mathf.Clamp(value, 0, sliderMaxValue);
        }


        return value;
    }
    public float OnChangeField(string valueStr)
    {
        float value = OnChangeFieldBase(valueStr);

        // Update sens slider
        UpdateSlider(value);

        return value;
    }
    public void OnEndEditField(string valueStr)
    {
        float value = OnChangeFieldBase(valueStr);

        // Update field value
        string checkedValueStr = value.ToString("R");
        UpdateField(checkedValueStr);
    }
}