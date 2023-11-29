using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    // UI
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject resultsScreen;
    [SerializeField] GameObject interfaceUi;

    // Confirm screen
    [SerializeField] GameObject confirmScreen;
    [SerializeField] TextMeshProUGUI confirmText;

    // In-game interface
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timerText;

    // Results screen
    [SerializeField] TextMeshProUGUI resultScoreText;
    [SerializeField] TextMeshProUGUI accuracyText;
    [SerializeField] TextMeshProUGUI timeToHitText;

    // Pause panels
    [SerializeField] GameObject pauseMenuPanel;

    // Main menu panels
    [SerializeField] Transform mainMenuLeftArea;
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject settingsPanel;

    // Settings panels
    [SerializeField] GameObject generalSettingsPanel;

    // Sensitivity panel
    [SerializeField] Slider sensSlider;
    [SerializeField] TMP_InputField sensField;
    const int SensSliderMaxValue = 1000;

    // Game Manager
    GameManager gm;

    // UI state
    UIState uiState;
    UIState prevState;

    public UIController()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        gm = GameManager.Instance;

        // Init states
        uiState = UIState.MainMenu;

    }


    // Start button handler
    public void StartGame()
    {
        gm.StartGame();

        uiState = UIState.None;
        mainMenu.SetActive(false);
        ShowInterface(gm.currentScore, gm.currentTime);
    }

    
    
    
    // Pause menu
    public void OpenPauseMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            uiState = UIState.MainPause;
            pauseMenu.SetActive(true);
            gm.PauseGame();
        }
    }
    public void ClosePauseMenu()
    {
        uiState = UIState.None;
        pauseMenu.SetActive(false);
        gm.ResumeGame();
    }
    // ----------




    // Results Screen
    public void OpenResultsScreen()
    {
        uiState = UIState.Results;

        Utilities.UpdateTextMeshPro(resultScoreText, "Score: " + gm.currentScore);
        if (gm.shotsCount > 0)
        {
            Utilities.UpdateTextMeshPro(accuracyText, "Accuracy: " + gm.accuracy.ToString("0.##") + "%");
            Utilities.UpdateTextMeshPro(timeToHitText, "Avg. time to hit: " + gm.avgTimeToHit.ToString("0") + "ms");
        } else
        {
            Utilities.UpdateTextMeshPro(accuracyText, "Accuracy: -");
            Utilities.UpdateTextMeshPro(timeToHitText, "Avg. time to hit: -");
        }
        Debug.Log("Accuracy: " + gm.accuracy);
        Debug.Log("Avg. time to hit: " + gm.avgTimeToHit);

        resultsScreen.SetActive(true);
    }
    //---------------

    // Try again button handler
    public void TryAgain()
    {
        gm.StartGame();

        uiState = UIState.None;
        resultsScreen.SetActive(false);
        ShowInterface(gm.currentScore, gm.currentTime);
    }
    // -------------------




    // Settings panel
    public void OpenSettingsPanel()
    {
        // Change state
        uiState =  UIState.MenuSettings;

        // Change panels
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
    public void CloseSettingsPanel()
    {
        // Change state
        uiState = UIState.MainMenu;

        // Change panels
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    // --------------

    // General settings panel
    public void OpenGeneralSettingsPanel()
    {
        // Change state
        prevState = uiState;
        uiState = UIState.GeneralSettings;

        switch (prevState)
        {
            case UIState.MenuSettings:
                // Deactivate current panel
                settingsPanel.SetActive(false);
                // Move general settings panel to the correct area
                generalSettingsPanel.transform.SetParent(mainMenuLeftArea, false);
                break;
            case UIState.MainPause:
                // Deactivate current panel
                pauseMenuPanel.SetActive(false);
                // Move general settings panel to the correct area
                generalSettingsPanel.transform.SetParent(pauseMenu.transform, false);
                break;

        }

        // Init UI
        InitGeneralSettingsUI();

        // Activate general settings panel
        generalSettingsPanel.SetActive(true);
    }
    public void CloseGeneralSettingsPanel()
    {
        // Change state
        uiState = prevState;

        // Deactivate current panel
        generalSettingsPanel.SetActive(false);

        switch (uiState)
        {
            case UIState.MenuSettings:
                // Activate prev panel
                settingsPanel.SetActive(true);
                break;
            case UIState.MainPause:
                // Activate prev panel
                pauseMenuPanel.SetActive(true);
                break;

        }

        // Create list of props to rewrite
        List<PlayerPrefsProps> props = new()
        {
            PlayerPrefsProps.Sensitivity
        };

        // Save prefs
        gm.SavePrefsSpecific(propsToRewrite: props);
    }
    private void InitGeneralSettingsUI()
    {
        UpdateSensField(gm.Sensitivity.ToString("R"));
        UpdateSensSlider(gm.Sensitivity);
    }
    // --------------


    // In-game interface
    public void ShowInterface(int score, int time)
    {
        Utilities.UpdateTextMeshPro(scoreText, "Score: " + score);
        Utilities.UpdateTextMeshPro(timerText, "Time: " + time);
        interfaceUi.SetActive(true);
    }
    public void UpdateScoreText()
    {
        Utilities.UpdateTextMeshPro(scoreText, "Score: " + gm.currentScore);
    }
    public void UpdateTimerText()
    {
        Utilities.UpdateTextMeshPro(timerText, "Time: " + gm.currentTime);
    }
    // -----------------


    // Main menu button handler
    public void MainMenuBtnHandler()
    {
        prevState = uiState;
        uiState = UIState.MainMenuConfirm;

        if (prevState == UIState.Results)
        {
            resultsScreen.SetActive(false);
        }
        else if (prevState == UIState.MainPause)
        {
            pauseMenu.SetActive(false);
        }

        confirmText.text = "Exit to main menu?";
        confirmScreen.SetActive(true);


    }
    // -------------------

    // Quit button handler
    public void QuitBtnHandler()
    {
        prevState = uiState;
        uiState = UIState.QuitConfirm;


        foreach (Transform child in mainMenuPanel.transform)
        {
            if (child.TryGetComponent(out Button btn))
            {
                btn.interactable = false;
            }
        }

        confirmText.text = "Quit the game?";
        confirmScreen.SetActive(true);


    }
    // -------------------

    // Confirm screen Yes handler
    public void ConfirmYes()
    {
        confirmScreen.SetActive(false);
        switch (uiState)
        {
            case UIState.MainMenuConfirm:
                uiState = UIState.MainMenu;
                interfaceUi.SetActive(false);
                mainMenu.SetActive(true);

                if (prevState == UIState.MainPause)
                {
                    gm.SceneDefaultState();
                }

                break;
            case UIState.QuitConfirm:
                gm.QuitGame();
                break;
        }
    }
    // -------------------------
    // Confirm screen No handler
    public void ConfirmNo()
    {
        confirmScreen.SetActive(false);
        switch (prevState)
        {
            case UIState.Results:
                resultsScreen.SetActive(true);
                break;
            case UIState.MainPause:
                pauseMenu.SetActive(true);
                break;
            case UIState.MainMenu:
                foreach (Transform child in mainMenuPanel.transform)
                {
                    if (child.TryGetComponent(out Button btn))
                    {
                        btn.interactable = true;
                    }
                }
                break;
        }
        uiState = prevState;
    }
    // -------------------------


    // Back action
    public void Back(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            HandleBack();
        }
    }

    public void HandleBack()
    {
        switch (uiState)
        {
            // Main Menu
            case UIState.MainMenu:
                QuitBtnHandler();
                break;
            case UIState.MenuSettings:
                CloseSettingsPanel();
                break;
            case UIState.GeneralSettings:
                CloseGeneralSettingsPanel();
                break;

            // Pause
            case UIState.MainPause:
                ClosePauseMenu();
                break;

            // Results
            case UIState.Results:
                MainMenuBtnHandler();
                break;

            // Confirm
            case UIState.MainMenuConfirm:
                ConfirmNo();
                break;

            case UIState.None:
                break;
        }
    }
    // -----------




    // Sensitivity control
    public void UpdateSensSlider(float value)
    {
        float newValue = value * SensSliderMaxValue;
        // Not sure if the condition is necessary, but it shouldn't make worse
        if (!sensSlider.value.Equals(newValue))
        {
            // Will not invoke onValueChange callback
            sensSlider.SetValueWithoutNotify(newValue);
        }
    }
    public void UpdateSensField(string value)
    {
        // Not sure if the condition is necessary, but it shouldn't make worse
        if (!sensField.text.Equals(value))
        {
            // Will not invoke onValueChange callback
            sensField.SetTextWithoutNotify(value);
        }
    }
    public void OnChangeSensitivitySlider(float sliderValue)
    {
        // Normalize to [0, 1]
        float value = sliderValue / SensSliderMaxValue;

        // Update sens field
        string valueStr = value.ToString("R");
        UpdateSensField(valueStr);

        // Update property of game manager
        gm.Sensitivity = value;

    }
    public void OnChangeSensitivityField(string valueStr)
    {
        // Replace empty value with 0
        if (valueStr == string.Empty)
        {
            valueStr = "0";
        }

        // Parse string to float and clamp between 0 and 1
        float value = float.Parse(valueStr);
        value = Mathf.Clamp01(value);

        // Update sens slider
        UpdateSensSlider(value);

        // Property of game manager will be updated with OnEndEdit
    }
    public void OnEndEditSensitivityField(string valueStr)
    {
        // Replace empty value with 0
        if (valueStr == string.Empty)
        {
            valueStr = "0";
        }

        // Parse string to float and clamp between 0 and 1
        float value = float.Parse(valueStr);
        value = Mathf.Clamp01(value);

        // Update field value
        string checkedValueStr = value.ToString("R");
        UpdateSensField(checkedValueStr);

        // Update property of game manager
        gm.Sensitivity = value;
    }
    // -------------------

}   


public enum UIState
{
    // Main Menu
    MainMenu,
    MenuSettings,

    // Pause
    MainPause,

    // General Settings
    GeneralSettings,

    // Results
    Results,

    // Confirm
    MainMenuConfirm,
    QuitConfirm,
    //RestartConfirm,

    None
}

public static class Utilities
{
    public static void UpdateTextMeshPro(TextMeshProUGUI tmp, string value)
    {
        tmp.text = value;
    }
}