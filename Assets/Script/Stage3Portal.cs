using UnityEngine;

public class Stage3Portal : MonoBehaviour
{
    public GameObject Stage3;
    public GameObject shop; // Shop 오브젝트를 직접 할당
    private Timer timer; // Timer 인스턴스
    private bool isPlayerInRange = false;
    private bool stageChanged = false;

    void Start()
    {
        // Stage3 비활성화, Shop 활성화
        Stage3.SetActive(false);

        if (shop != null)
        {
            shop.SetActive(true); // 초기에는 Shop 활성화
        }

        timer = FindObjectOfType<Timer>();
        if (timer != null)
        {
            timer.OnTimerEnd += OnTimerEnd; // 타이머 종료 시 Shop 활성화 이벤트 등록
        }
    }

    void Update()
    {
        if (isPlayerInRange && !stageChanged)
        {
            HandleStageTransition(); // 포탈을 통해 Stage3로 이동
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            stageChanged = false;

            // 포탈에 닿으면 Shop 비활성화
            if (shop != null)
            {
                shop.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private void HandleStageTransition()
    {
        if (timer != null)
        {
            timer.PauseTimer(); // 타이머 일시정지
        }

        TransitionToStage(Stage3);
    }

    private void TransitionToStage(GameObject nextStage)
    {
        Debug.Log("Activating Stage 3");
        nextStage.SetActive(true); // Stage 3 활성화
        stageChanged = true; // 스테이지 변경 완료

        if (timer != null)
        {
            timer.ResetTimer(10f);
            timer.StartTimer(); // 타이머 시작
        }
    }

    private void OnTimerEnd()
    {
        Debug.Log("Timer ended, returning to shop.");

        Stage3.SetActive(false);

        if (shop != null)
        {
            shop.SetActive(true); // Shop 활성화
        }

        if (timer != null)
        {
            timer.PauseTimer(); // 타이머 일시정지
        }

        stageChanged = false;
    }
}
