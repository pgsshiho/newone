using UnityEngine;

public class BossPortal : MonoBehaviour
{
    public GameObject Boss;
    public GameObject shop; // Shop ������Ʈ�� ���� �Ҵ�
    private Timer timer; // Timer �ν��Ͻ�
    private bool isPlayerInRange = false;
    private bool stageChanged = false;

    void Start()
    {
        // Boss ��Ȱ��ȭ, Shop Ȱ��ȭ
        Boss.SetActive(false);

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
            HandleStageTransition(); // ��Ż�� ���� Boss ���������� �̵�
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

        TransitionToBoss();
    }

    private void TransitionToBoss()
    {
        Debug.Log("Activating Boss Stage");
        Boss.SetActive(true); // ���� �������� Ȱ��ȭ
        stageChanged = true; // �������� ���� �Ϸ�

        if (timer != null)
        {
            timer.PauseTimer(); // ���� ������������ Ÿ�̸� ��Ȱ��ȭ
        }
    }

    private void OnTimerEnd()
    {
        Debug.Log("Timer ended, returning to shop.");

        Boss.SetActive(false);

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
