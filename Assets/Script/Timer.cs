using TMPro;
using UnityEngine;
using System;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public GameObject[] stages;    // ��� �������� ������Ʈ���� �迭�� ����
    public GameObject[] shops;     // ��� ���� ������Ʈ���� �迭�� ����
    public TextMeshProUGUI enemyCountText; // ���� ���� ��Ÿ���� �ؽ�Ʈ
    public Transform playerTransform; // �÷��̾��� Transform ������Ʈ

    private int currentStageIndex = 0;  // ���� ���������� �ε���
    private int currentShopIndex = 0;   // ���� ������ �ε���
    private float timeRemaining = 10f;
    private bool timerEnded = false;
    private bool isPaused = false;

    public Action OnTimerEnd; // Ÿ�̸Ӱ� ������ �� ����� �̺�Ʈ

    public bool TimerEnded
    {
        get { return timerEnded; }
    }

    void Start()
    {
        if (timerText != null)
        {
            UpdateTimerDisplay();
        }

        StartTimer(); // Ÿ�̸� ����
    }

    void Update()
    {
        if (isPaused)
        {
            return; // Ÿ�̸Ӱ� �Ͻ������� ���¶�� Update�� �������� ����
        }

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                timerEnded = true;

                timerText?.gameObject.SetActive(false);

                OnTimerEnd?.Invoke(); // Ÿ�̸� ���� �� ��ϵ� �̺�Ʈ ����

                DestroyAllEnemies(); // ��� ���� �����ϰ� �� ī��Ʈ�� 0���� ����
                MovePlayerToOrigin(); // �÷��̾ (0,0)���� �̵�
                GoToShop(); // Ÿ�̸Ӱ� 0�� �Ǹ� ���������� ��Ȱ��ȭ�ϰ� �ش� ���� Ȱ��ȭ

                ResetTimer(10f); // Ÿ�̸� �ڵ� ���� �� ����
            }

            if (timerText != null)
            {
                UpdateTimerDisplay();
            }
        }
    }

    public void ResetTimer(float newTime)
    {
        timeRemaining = newTime;
        timerEnded = false;
        if (timerText != null)
        {
            timerText.gameObject.SetActive(true);
            UpdateTimerDisplay();
        }
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
        // ���� �������� ��Ȱ��ȭ
        if (currentStageIndex < stages.Length)
        {
            stages[currentStageIndex].SetActive(false);
        }

        // ���� ���������� Ȱ��ȭ
        currentStageIndex++;
        if (currentStageIndex < stages.Length)
        {
            stages[currentStageIndex].SetActive(true);
        }
        else
        {
            Debug.LogError("No more stages left to activate.");
        }

        // ��� ������ ��Ȱ��ȭ
        foreach (var shop in shops)
        {
            shop.SetActive(false);
        }

        // ���� ������ Ȱ��ȭ
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
        int enemyCount = 0;

        foreach (var obj in allObjects)
        {
            if (obj.layer == enemyLayer)
            {
                Destroy(obj);
                enemyCount++;
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
            playerTransform.position = Vector3.zero;
        }
        else
        {
            Debug.LogError("Player Transform is not assigned!");
        }
    }
}
