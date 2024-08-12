using UnityEngine;

public class Stage2Portal : MonoBehaviour
{
    public GameObject Stage2;
    public GameObject shop; // Shop ������Ʈ�� ���� �Ҵ�
    private Timer timer; // Timer �ν��Ͻ�
    private bool isPlayerInRange = false;
    private bool stageChanged = false;

    void Start()
    {
        // Stage2 ��Ȱ��ȭ, Shop Ȱ��ȭ
        Stage2.SetActive(false);

        if (shop != null)
        {
            shop.SetActive(true); // �ʱ⿡�� Shop Ȱ��ȭ
        }

        timer = FindObjectOfType<Timer>();
        if (timer != null)
        {
            timer.OnTimerEnd += OnTimerEnd; // Ÿ�̸� ���� �� Shop Ȱ��ȭ �̺�Ʈ ���
        }
    }

    void Update()
    {
        if (isPlayerInRange && !stageChanged)
        {
            HandleStageTransition(); // ��Ż�� ���� Stage2�� �̵�
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            stageChanged = false;

            // ��Ż�� ������ Shop ��Ȱ��ȭ
            if (shop != null)
            {
                shop.SetActive(false);
                HandleStageTransition();
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
            timer.PauseTimer(); // Ÿ�̸� �Ͻ�����
        }

        TransitionToStage(Stage2);
    }

    private void TransitionToStage(GameObject nextStage)
    {
        Debug.Log("Activating Stage 2");
        nextStage.SetActive(true); // Stage 2 Ȱ��ȭ
        stageChanged = true; // �������� ���� �Ϸ�

        if (timer != null)
        {
            timer.ResetTimer(10f);
            timer.StartTimer(); // Ÿ�̸� ����
        }
    }

    private void OnTimerEnd()
    {
        Debug.Log("Timer ended, returning to shop.");

        Stage2.SetActive(false);

        if (shop != null)
        {
            shop.SetActive(true); // Shop Ȱ��ȭ
        }

        if (timer != null)
        {
            timer.PauseTimer(); // Ÿ�̸� �Ͻ�����
        }

        stageChanged = false;
    }
}
