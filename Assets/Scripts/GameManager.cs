using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]private int score = 0;
    public TMP_Text scoreText;

    public float baseSpeed = 5f;
    public float speedStep = 1f;
    public float interval = 10f;

    private float currentSpeed;
    private float nextSpeedIncreaseTime;

    public float CurrentSpeed => currentSpeed;
    private bool isPaused = false;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject vfxPrefab; 
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GhostVisualSpawner ghostVisualSpawner;

    public Volume volume;

    private ChromaticAberration chromaticAberration;
    private ColorAdjustments colorAdjustments;

    public float lerpDuration = 1f;
    private float timer = 0f;
    private bool isLerping = false;

    private LensDistortion lensDistortion;

    public float distortionShakeDuration = 0.3f;
    public float distortionShakeMagnitude = 0.4f;
    public float distortionShakeFrequency = 20f;
    public int rippleCount = 3;
    private float distortionShakeTimer = 0f;
    private bool isDistortionShaking = false;

    public GameObject speedVFX;
    private bool isSpeedActive = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
        
    }
    private void Start()
    {
        if (PlayerPrefs.GetInt("IsRetry", 0) == 1)
        {
            StartGame();
            Debug.Log("Prefs 1");
            PlayerPrefs.SetInt("IsRetry", 0);
        }
        else
        {
            Debug.Log("Prefs 0"); 
            startScreen.SetActive(true);
            PauseGame();
        }
        currentSpeed = baseSpeed;
        nextSpeedIncreaseTime = Time.time + interval;

        if (volume != null && volume.profile != null)
        {
            if (volume.profile.TryGet(out chromaticAberration))
            {
                chromaticAberration.intensity.overrideState = true;
                chromaticAberration.intensity.value = 0f;
            }
            else
            {
                Debug.LogError("Chromatic Aberration not found in Volume Profile.");
            }

            if (volume.profile.TryGet(out colorAdjustments))
            {
                colorAdjustments.saturation.overrideState = true;
                colorAdjustments.saturation.value = 0f;
            }
            else
            {
                Debug.LogError("Color Adjustments not found in Volume Profile.");
            }
        }
    }
    public void StartSpeedVFX()
    {
        if (!isSpeedActive) return;
        isSpeedActive = false;
        speedVFX.SetActive(true);
        Invoke("StopSpeedVFX", 2f);
    }

    public void StopSpeedVFX()
    {
        if (speedVFX != null)
        {
            speedVFX.SetActive(false);
        }
    }
    public void StartEffectLerp()
    {
        timer = 0f;
        isLerping = true;
    }
    public void StartLensDistortionShake()
    {
        if (volume.profile.TryGet(out lensDistortion))
        {
            lensDistortion.intensity.overrideState = true;
            lensDistortion.center.overrideState = true;

            distortionShakeTimer = 0f;
            isDistortionShaking = true;
        }
        else
        {
            Debug.LogWarning("Lens Distortion not found in Volume Profile.");
        }
    }


    private void Update()
    {
        if (Time.time >= nextSpeedIncreaseTime)
        {
            currentSpeed += speedStep;
            nextSpeedIncreaseTime += interval;
            isSpeedActive = true;
            StartSpeedVFX();          
        }

        if (!isLerping) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / lerpDuration);

        if (chromaticAberration != null)
        {
            chromaticAberration.intensity.value = Mathf.Lerp(0f, 1f, t);
        }

        if (colorAdjustments != null)
        {
            colorAdjustments.saturation.value = Mathf.Lerp(0f, -100f, t);
        }

        if (t >= 1f)
        {
            isLerping = false;
        }

        if (isDistortionShaking && lensDistortion != null)
        {
            distortionShakeTimer += Time.deltaTime;
            float progress = distortionShakeTimer / distortionShakeDuration;

            if (progress < 1f)
            {
                float sineWave = Mathf.Sin(progress * Mathf.PI * rippleCount);
                float envelope = 1f - progress; 
                float ripple = distortionShakeMagnitude * sineWave * envelope;

                lensDistortion.intensity.value = ripple;
                lensDistortion.center.value = new Vector2(0.5f, 0.5f);
            }
            else
            {
                lensDistortion.intensity.value = 0f;
                lensDistortion.center.value = new Vector2(0.5f, 0.5f);
                isDistortionShaking = false;
            }
        }



    }
    public void PlayBurstEffect()
    {
        GameObject vfxInstance = Instantiate(vfxPrefab, spawnPoint.position, spawnPoint.rotation);
        Destroy(vfxInstance, 2f);

        ghostVisualSpawner.SpawnVFXGhost(spawnPoint.position, spawnPoint.rotation);
    }
    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score.ToString();
        Debug.Log("Score: " + score);
    }
    public void GameOver()
    {
        Debug.Log("Game Over!");
        StartEffectLerp();
        StartLensDistortionShake();
        Invoke("EnableGameOverScreen", 2f);
    }
    public void EnableGameOverScreen()
    {
        gameOverScreen.SetActive(true);
    }
    public void StartGame()
    {
        ResumeGame();
        startScreen.SetActive(false);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void RetryGame()
    {
        ResumeGame();
        PlayerPrefs.SetInt("IsRetry", 1); 
        PlayerPrefs.Save();
        MainMenu();
    }

    public void PauseGame()
    {
        if (isPaused) return;

        Time.timeScale = 0f;
        isPaused = true;

    }

    public void ResumeGame()
    {
        if (!isPaused) return;

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}
