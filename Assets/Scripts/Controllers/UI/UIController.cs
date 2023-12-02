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
    [SerializeField] GameObject crosshairSettingsPanel;

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

        // Add event handlers
        gm.currentGameState.TimeChanged += UpdateTimerText;
        gm.currentGameState.ScoreChanged += UpdateScoreText;


        // Init states
        uiState = UIState.MainMenu;

    }


    // Start button handler
    public void StartGame()
    {
        gm.StartGame();

        uiState = UIState.None;
        mainMenu.SetActive(false);
        ShowInterface();
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

        Utilities.UpdateTextMeshPro(resultScoreText, "Score: " + gm.results.Score);
        if (gm.results.ShotsFired)
        {
            Utilities.UpdateTextMeshPro(accuracyText, "Accuracy: " + gm.results.Accuracy.ToString("0.##") + "%");
            Utilities.UpdateTextMeshPro(timeToHitText, "Avg. time to hit: " + gm.results.AvgTimeToHit.ToString("0") + "ms");
        } else
        {
            Utilities.UpdateTextMeshPro(accuracyText, "Accuracy: -");
            Utilities.UpdateTextMeshPro(timeToHitText, "Avg. time to hit: -");
        }
        Debug.Log("Accuracy: " + gm.results.Accuracy);
        Debug.Log("Avg. time to hit: " + gm.results.AvgTimeToHit);

        resultsScreen.SetActive(true);
    }
    //---------------

    // Try again button handler
    public void TryAgain()
    {
        gm.StartGame();

        uiState = UIState.None;
        resultsScreen.SetActive(false);
        ShowInterface();
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
    // --------------

    // Crosshair settings panel
    public void OpenCrosshairSettingsPanel()
    {
        // Change state
        uiState = UIState.CrosshairSettings;

        // Deactivate current panel
        settingsPanel.SetActive(false);

        // Activate general settings panel
        crosshairSettingsPanel.SetActive(true);
    }
    public void CloseCrosshairSettingsPanel()
    {
        // Change state
        uiState = UIState.MenuSettings;

        // Deactivate current panel
        crosshairSettingsPanel.SetActive(false);

        settingsPanel.SetActive(true);

        /*
        // Create list of props to rewrite
        List<PlayerPrefsProps> props = new()
        {
            PlayerPrefsProps.Sensitivity
        };

        // Save prefs
        gm.SavePrefsSpecific(propsToRewrite: props);
        */
    }

    // In-game interface
    public void ShowInterface()
    {
        UpdateScoreText();
        UpdateTimerText();
        interfaceUi.SetActive(true);
    }
    private void HideInterface()
    {
        interfaceUi.SetActive(false);
    }
    public void UpdateScoreText()
    {
        Utilities.UpdateTextMeshPro(scoreText, "Score: " + gm.currentGameState.CurrentScore.ToString());
    }
    public void UpdateTimerText()
    {
        Utilities.UpdateTextMeshPro(timerText, "Time: " + gm.currentGameState.CurrentTime.ToString());
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
                HideInterface();
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
            case UIState.CrosshairSettings:
                CloseCrosshairSettingsPanel();
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
}


public enum UIState
{
    // Main Menu
    MainMenu,
    MenuSettings,

    // Pause
    MainPause,

    // Settings
    GeneralSettings,
    CrosshairSettings,

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