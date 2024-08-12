using UnityEngine;

public class BossPortal : MonoBehaviour
{
    public GameObject Boss;
    public GameObject shop; // Shop 오브젝트를 직접 할당
    private Timer timer; // Timer 인스턴스
    private bool isPlayerInRange = false;
    private bool stageChanged = false;

    void Start()
    {
        // Boss 비활성화, Shop 활성화
        Boss.SetActive(false);

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
            HandleStageTransition(); // 포탈을 통해 Boss 스테이지로 이동
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

        TransitionToBoss();
    }

    private void TransitionToBoss()
    {
        Debug.Log("Activating Boss Stage");
        Boss.SetActive(true); // 보스 스테이지 활성화
        stageChanged = true; // 스테이지 변경 완료

        if (timer != null)
        {
            timer.PauseTimer(); // 보스 스테이지에서 타이머 비활성화
        }
    }

    private void OnTimerEnd()
    {
        Debug.Log("Timer ended, returning to shop.");

        Boss.SetActive(false);

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
