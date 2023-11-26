using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
    public int currentScore;
    public int currentTime;
    public int shotsCount;
    public float sumTimeToHit;
    public float lastHitTime;

    // Results
    public float accuracy; // %
    public float avgTimeToHit; // in ms

    Coroutine timerCrtn;

    readonly string playerActionMap = "Player";
    readonly string uiActionMap = "UI";

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
            }

        }
        PlayerPrefs.Save();
    }
    public void LoadPrefs()
    {
        Sensitivity = PlayerPrefs.GetFloat(PlayerPrefsProps.Sensitivity.ToString(), .5f);
    }

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
    public void Awake()
    {

        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        Application.targetFrameRate = 60;

        uiController = UIController.Instance;
        playerController = PlayerController.Instance;

        playerInput = GetComponent<PlayerInput>();
        rnd = new System.Random();
        LoadPrefs();
    }

    public void Start()
    {


        // Temporary for dev testing
        area.transform.position = Vector3.forward * areaCfg.minDistance;
        area.transform.localScale = new Vector3(areaCfg.width, areaCfg.height, areaCfg.depth);
        // -------------------------


    }

    public void StartGame()
    {
        Application.targetFrameRate = 300;
        Cursor.lockState = CursorLockMode.Locked;

        currentScore = 0;
        shotsCount = 0;
        sumTimeToHit = 0;
        lastHitTime = Time.time;

        for (int i = 0; i < targetsOnScreen; i++)
        {
            SpawnTarget(0);
        }

        timerCrtn = StartCoroutine(Timer(time));

        playerInput.SwitchCurrentActionMap(playerActionMap);
    }

    public void ShotHit()
    {
        currentScore++;
        shotsCount++;
        sumTimeToHit += Time.time - lastHitTime;
        lastHitTime = Time.time;
        uiController.UpdateScoreText();
        SpawnTarget(targetLifetime);
    }

    public void TargetTimedOut()
    {
        SpawnTarget(targetLifetime);
        // -score?
    }
    public void ShotMiss()
    {
        shotsCount++;
        // -score?
    }

    public void SpawnTarget(float lifetime)
    {
        GameObject targetPrefab;
        if (targetCfg.shape == Shape.Sphere)
        {
            targetPrefab = targetPrefabs[0];
        } else // if (targetCfg.shape == Shape.Cube)
        {
            targetPrefab = targetPrefabs[1];
        }
        Vector3 relativePos = new(
            (float)rnd.NextDouble() - 0.5f,
            (float)rnd.NextDouble(),
            (float)rnd.NextDouble()
        );
        Vector3 pos = new Vector3(
            relativePos.x * area.transform.localScale.x,
            relativePos.y * area.transform.localScale.y,
            relativePos.z * area.transform.localScale.z
        ) + area.transform.position;

        GameObject target = Instantiate(targetPrefab, pos, Quaternion.identity, targetsParent);
        target.GetComponent<Renderer>().material.color = targetCfg.color;
        if (lifetime == 0)
        {
            target.GetComponent<DestroyAfterTime>().enabled = false;
        } else
        {
            target.GetComponent<DestroyAfterTime>().lifetime = lifetime;
        }
    }

    public void PauseGame()
    {
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        playerInput.SwitchCurrentActionMap(uiActionMap);


    }

    public void ResumeGame()
    {
        Application.targetFrameRate = 300;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        playerInput.SwitchCurrentActionMap(playerActionMap);
    }

    public void EndGame()
    {
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.None;

        SceneDefaultState();

        if (shotsCount > 0)
        {
            accuracy = ((float)currentScore / shotsCount) * 100f;
            avgTimeToHit = (sumTimeToHit / currentScore) * 1000f;
        }
        else {
            accuracy = 0f;
            avgTimeToHit = 0f;
        }

        // Show Results Screen
        uiController.OpenResultsScreen();

        playerInput.SwitchCurrentActionMap(uiActionMap);
    }
    public void SceneDefaultState()
    {
        foreach (Transform child in targetsParent)
        {
            Destroy(child.gameObject);
        }
        playerController.DefaultState();
        StopCoroutine(timerCrtn);
        Time.timeScale = 1f;
    }

    IEnumerator Timer(int seconds)
    {
        currentTime = seconds;
        uiController.UpdateTimerText();
        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1);
            currentTime--;
            uiController.UpdateTimerText();
        }
        EndGame();
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }

}
public enum PlayerPrefsProps
{
    Sensitivity,

}