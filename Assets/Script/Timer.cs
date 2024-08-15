using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Timer : MonoBehaviour
{
    public static Timer Instance; // 싱글턴 인스턴스

    public TextMeshProUGUI timerText;
    public GameObject[] stages;
    public GameObject[] shops;
    public TextMeshProUGUI enemyCountText;
    public Transform playerTransform;
    public GameObject boss;

    private int currentStageIndex = 0;
    private int currentShopIndex = 0;
    private float timeRemaining = 10f;
    private bool timerEnded = false;
    private bool isPaused = true; // 기본적으로 타이머가 일시정지 상태로 시작

    public Action OnTimerEnd;

    public bool TimerEnded
    {
        get { return timerEnded; }
    }

    void Awake()
    {
        // 씬 전환 시 타이머가 없어지도록 하기 위해 싱글턴 패턴 제거
        // DontDestroyOnLoad(gameObject); 제거

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (timerText != null)
        {
            UpdateTimerDisplay();
        }

        // 씬 로드 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;

        StartTimer(); // 타이머 시작
    }

    void OnDestroy()
    {
        // 씬 로드 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        // 타이머가 일시정지 상태가 아니고 보스가 활성화되어 있지 않으면 타이머 갱신
        if (!isPaused && (boss == null || !boss.activeSelf))
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                if (timeRemaining <= 0)
                {
                    timeRemaining = 0;
                    timerEnded = true;

                    timerText?.gameObject.SetActive(false);

                    OnTimerEnd?.Invoke();

                    DestroyAllEnemies();
                    MovePlayerToOrigin();
                    GoToShop();

                    PauseTimer(); // 타이머 멈춤
                }

                if (timerText != null)
                {
                    UpdateTimerDisplay();
                }
            }
        }
    }

    public void ResetTimer(float newTime)
    {
        timeRemaining = newTime;
        timerEnded = false;
        StartTimer(); // 타이머를 리셋한 후 자동으로 시작되도록 함
    }

    public void StartTimer()
    {
        isPaused = false; // 타이머 재개
    }

    public void PauseTimer()
    {
        isPaused = true; // 타이머 일시정지
    }

    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component is not assigned!");
        }
    }

    private void GoToShop()
    {
        if (currentStageIndex < stages.Length)
        {
            stages[currentStageIndex].SetActive(false);
        }

        currentStageIndex++;
        if (currentStageIndex < stages.Length)
        {
            stages[currentStageIndex].SetActive(true);
        }
        else
        {
            Debug.LogError("No more stages left to activate.");
        }

        foreach (var shop in shops)
        {
            shop.SetActive(false);
        }

        if (currentShopIndex < shops.Length)
        {
            shops[currentShopIndex].SetActive(true);
            currentShopIndex++;
        }
        else
        {
            Debug.LogError("No more shops left to activate.");
        }
    }

    private void DestroyAllEnemies()
    {
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (var obj in allObjects)
        {
            if (obj.layer == enemyLayer)
            {
                Destroy(obj);
            }
        }

        if (enemyCountText != null)
        {
            enemyCountText.text = "0";
        }
    }

    private void MovePlayerToOrigin()
    {
        if (playerTransform != null)
        {
            playerTransform.position = new Vector2(0, 0);
        }
        else
        {
            Debug.LogError("Player Transform is not assigned!");
        }
    }

    // 씬이 로드될 때 호출되는 메서드
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 타이머가 포함된 씬에 돌아왔을 때 타이머를 다시 시작
        if (scene.name == "YourSceneName") // 타이머가 존재하는 씬 이름으로 대체
        {
            ResetTimer(10f); // 타이머를 다시 설정하고 시작
        }
    }
}
