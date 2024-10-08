using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // PlayerInput for switching action maps
    PlayerInput playerInput;

    // Controllers
    UIController uiController;
    PlayerController playerController;

    // Random for target spawn
    System.Random rnd;


    // Objects in scene
    [SerializeField] GameObject area;
    [SerializeField] Transform targetsParent;

    // Prefabs
    [SerializeField] List<GameObject> targetPrefabs; // 0 - sphere, 1 - cube.

    // Configs
    public AreaConfig areaCfg;
    public TargetConfig targetCfg;
    public CrosshairConfig crosshairCfg;

    // Other settings
    public int targetsOnScreen = 3;
    public float targetLifetime = 0;
    public int time = 10;

    // Prefs properties
    public float Sensitivity { get; set; }

    // Current state
    public CurrentGameState currentGameState;
    private Coroutine timerCrtn;

    // Results
    public Results results;

    private const int menuFpsLock = 60;
    private const int gameTimeFpsLock = 300;

    readonly string playerActionMap = "Player";
    readonly string uiActionMap = "UI";

    public GameManager()
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

    private void Awake()
    {
        // To use a dot as a decimal separator
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        // FPS lock at menu/pause
        Application.targetFrameRate = menuFpsLock;

        // Init own variables
        rnd = new System.Random();
        currentGameState = new CurrentGameState();
        results = new Results();

    }

    private void Start()
    {
        // Init this object's components
        playerInput = GetComponent<PlayerInput>();
        // -----------------------------

        // Init other objects and their components
        uiController = UIController.Instance;
        playerController = PlayerController.Instance;

        // Load 
        LoadPrefs();

        // Temporary for dev testing
        area.transform.position = Vector3.forward * areaCfg.minDistance;
        area.transform.localScale = new Vector3(areaCfg.width, areaCfg.height, areaCfg.depth);
        // -------------------------
    }

    public void SavePrefs(bool rewriteAll)
    {
        if (rewriteAll)
        {
            PlayerPrefs.SetFloat(PlayerPrefsProps.Sensitivity.ToString(), Sensitivity);
        }
        PlayerPrefs.Save();
    }
    public void SavePrefsSpecific(List<PlayerPrefsProps> propsToRewrite)
    {
        foreach (PlayerPrefsProps prop in propsToRewrite)
        {
            switch (prop)
            {
                case PlayerPrefsProps.Sensitivity:
                    PlayerPrefs.SetFloat(prop.ToString(), Sensitivity);
                    break;
                case PlayerPrefsProps.Crosshair:
                    string data = JsonUtility.ToJson(crosshairCfg);
                    PlayerPrefs.SetString(prop.ToString(), data);
                    break;
            }

        }
        PlayerPrefs.Save();
    }
    public void LoadPrefs()
    {
        Sensitivity = PlayerPrefs.GetFloat(PlayerPrefsProps.Sensitivity.ToString(), .5f);

        string crosshairCfgJson = PlayerPrefs.GetString(PlayerPrefsProps.Crosshair.ToString(), CrosshairConfig.DefaultJson());
        crosshairCfg = JsonUtility.FromJson<CrosshairConfig>(crosshairCfgJson);
    }

    public void StartGame()
    {
        // FPS lock at game time
        Application.targetFrameRate = gameTimeFpsLock;

        // Hide cursor at game time
        Cursor.lockState = CursorLockMode.Locked;

        // Init current game state
        currentGameState.Init(time);

        // Start timer
        StartTimer();

        // Spawn targets
        for (int i = 0; i < targetsOnScreen; i++)
        {
            SpawnTarget(0);
        }

        // Switch to in-game action map
        playerInput.SwitchCurrentActionMap(playerActionMap);
    }
    public void SpawnTarget(float lifetime)
    {
        // Choose targetPrefab
        GameObject targetPrefab;
        if (targetCfg.shape == Shape.Sphere)
        {
            targetPrefab = targetPrefabs[0];
        }
        else // if (targetCfg.shape == Shape.Cube)
        {
            targetPrefab = targetPrefabs[1];
        }

        // Random position inside rectangle target area
        Vector3 relativePos = new(
            (float)rnd.NextDouble() - 0.5f,
            (float)rnd.NextDouble(),
            (float)rnd.NextDouble()
        );

        // Position in world
        Vector3 pos = new Vector3(
            relativePos.x * area.transform.localScale.x,
            relativePos.y * area.transform.localScale.y,
            relativePos.z * area.transform.localScale.z
        ) + area.transform.position;

        // Instantiate as child of targetsParent object
        GameObject target = Instantiate(targetPrefab, pos, Quaternion.identity, targetsParent);

        // Init target props (color, destroyAfterTime)
        target.GetComponent<Renderer>().material.color = targetCfg.color;
        if (lifetime == 0)
        {
            // Infinite lifetime
            target.GetComponent<DestroyAfterTime>().enabled = false;
        }
        else
        {
            target.GetComponent<DestroyAfterTime>().lifetime = lifetime;
        }
    }
    public void TargetTimedOut()
    {
        // Spawn new target
        SpawnTarget(targetLifetime);
        // ++targetMissed?
    }

    public void ShotHit()
    {
        // Update current game state

        currentGameState.CurrentScore++;
        currentGameState.ShotsCount++;

        currentGameState.LastHitTime = Time.time;

        // Spawn new target
        SpawnTarget(targetLifetime);
    }
    public void ShotMiss()
    {
        // Update current game state
        currentGameState.ShotsCount++;
    }



    public void PauseGame()
    {
        Application.targetFrameRate = menuFpsLock;
        Cursor.lockState = CursorLockMode.None;

        // Freeze all coroutines
        Time.timeScale = 0f;

        // Switch to Menu/Pause action map
        playerInput.SwitchCurrentActionMap(uiActionMap);
    }
    public void ResumeGame()
    {
        Application.targetFrameRate = gameTimeFpsLock;
        Cursor.lockState = CursorLockMode.Locked;

        // Resume all coroutines
        Time.timeScale = 1f;

        // Switch to in-game action map
        playerInput.SwitchCurrentActionMap(playerActionMap);
    }

    public void EndGame()
    {
        Application.targetFrameRate = menuFpsLock;
        Cursor.lockState = CursorLockMode.None;

        // Reset scene to default state (player model, destroy all targets if any, stop timer)
        SceneDefaultState();

        // Calculate results
        results.Calculate(currentGameState);

        // Show Results Screen
        uiController.OpenResultsScreen();

        // Switch to Menu/Pause action map
        playerInput.SwitchCurrentActionMap(uiActionMap);
    }
    public void SceneDefaultState()
    {
        // Destroy the remaining targets
        foreach (Transform child in targetsParent)
        {
            Destroy(child.gameObject);
        }

        // Set player model to default state
        playerController.DefaultState();

        // Stop timer
        StopTimer();

        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }
    private void StartTimer()
    {
        timerCrtn = StartCoroutine(Timer());
    }

    private void StopTimer()
    {
        StopCoroutine(timerCrtn);
    }

    private IEnumerator Timer()
    {
        while (currentGameState.CurrentTime > 0)
        {
            // Wait 1 second
            yield return new WaitForSeconds(1);

            // Update current game timer
            currentGameState.CurrentTime--;
        }

        EndGame();
    }
}

public enum PlayerPrefsProps
{
    Sensitivity,
    Crosshair
}