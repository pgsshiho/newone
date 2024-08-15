using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Timer : MonoBehaviour
{
    public static Timer Instance; // �̱��� �ν��Ͻ�

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
    private bool isPaused = true; // �⺻������ Ÿ�̸Ӱ� �Ͻ����� ���·� ����

    public Action OnTimerEnd;

    public bool TimerEnded
    {
        get { return timerEnded; }
    }

    void Awake()
    {
        // �� ��ȯ �� Ÿ�̸Ӱ� ���������� �ϱ� ���� �̱��� ���� ����
        // DontDestroyOnLoad(gameObject); ����

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

        // �� �ε� �̺�Ʈ ���
        SceneManager.sceneLoaded += OnSceneLoaded;

        StartTimer(); // Ÿ�̸� ����
    }

    void OnDestroy()
    {
        // �� �ε� �̺�Ʈ ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        // Ÿ�̸Ӱ� �Ͻ����� ���°� �ƴϰ� ������ Ȱ��ȭ�Ǿ� ���� ������ Ÿ�̸� ����
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

                    PauseTimer(); // Ÿ�̸� ����
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
        StartTimer(); // Ÿ�̸Ӹ� ������ �� �ڵ����� ���۵ǵ��� ��
    }

    public void StartTimer()
    {
        isPaused = false; // Ÿ�̸� �簳
    }

    public void PauseTimer()
    {
        isPaused = true; // Ÿ�̸� �Ͻ�����
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

    // ���� �ε�� �� ȣ��Ǵ� �޼���
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ÿ�̸Ӱ� ���Ե� ���� ���ƿ��� �� Ÿ�̸Ӹ� �ٽ� ����
        if (scene.name == "YourSceneName") // Ÿ�̸Ӱ� �����ϴ� �� �̸����� ��ü
        {
            ResetTimer(10f); // Ÿ�̸Ӹ� �ٽ� �����ϰ� ����
        }
    }
}
