using TMPro;
using UnityEngine;
using System;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public GameObject[] stages;    // 모든 스테이지 오브젝트들을 배열로 관리
    public GameObject[] shops;     // 모든 상점 오브젝트들을 배열로 관리
    public TextMeshProUGUI enemyCountText; // 적의 수를 나타내는 텍스트
    public Transform playerTransform; // 플레이어의 Transform 컴포넌트

    private int currentStageIndex = 0;  // 현재 스테이지의 인덱스
    private int currentShopIndex = 0;   // 현재 상점의 인덱스
    private float timeRemaining = 10f;
    private bool timerEnded = false;
    private bool isPaused = false;

    public Action OnTimerEnd; // 타이머가 끝났을 때 실행될 이벤트

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

        StartTimer(); // 타이머 시작
    }

    void Update()
    {
        if (isPaused)
        {
            return; // 타이머가 일시정지된 상태라면 Update를 진행하지 않음
        }

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                timerEnded = true;

                timerText?.gameObject.SetActive(false);

                OnTimerEnd?.Invoke(); // 타이머 종료 시 등록된 이벤트 실행

                DestroyAllEnemies(); // 모든 적을 제거하고 적 카운트를 0으로 설정
                MovePlayerToOrigin(); // 플레이어를 (0,0)으로 이동
                GoToShop(); // 타이머가 0이 되면 스테이지를 비활성화하고 해당 샵을 활성화

                ResetTimer(10f); // 타이머 자동 리셋 및 시작
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
        // 현재 스테이지 비활성화
        if (currentStageIndex < stages.Length)
        {
            stages[currentStageIndex].SetActive(false);
        }

        // 다음 스테이지를 활성화
        currentStageIndex++;
        if (currentStageIndex < stages.Length)
        {
            stages[currentStageIndex].SetActive(true);
        }
        else
        {
            Debug.LogError("No more stages left to activate.");
        }

        // 모든 상점을 비활성화
        foreach (var shop in shops)
        {
            shop.SetActive(false);
        }

        // 다음 상점을 활성화
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
