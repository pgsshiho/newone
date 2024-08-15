using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;  // TextMeshProUGUI�� ����ϱ� ���� �߰�
using UnityEngine.UI;  // Slider�� ����ϱ� ���� �߰�

public class BossManager : MonoBehaviour
{
    public GameObject bossPrefab;  // ���� ������ (������ ����)
    public Transform spawnPoint;  // ������ ������ ��ġ (������ ����)
    public TextMeshProUGUI bossHealthText;  // ���� ü���� ǥ���� �ؽ�Ʈ
    public Slider bossHealthSlider;  // ���� ü���� ǥ���� �����̴�

    private Timer timer;  // Timer ��ũ��Ʈ ����
    private BossEasy boss; // BossEasy ��ũ��Ʈ ����

    void Start()
    {
        timer = FindObjectOfType<Timer>();
        if (timer == null)
        {
            Debug.LogError("Timer ��ũ��Ʈ�� ã�� �� �����ϴ�. ���� Timer�� �ִ��� Ȯ���ϼ���.");
        }
        else
        {
            // Ÿ�̸Ӱ� ����� �� ���� �̺�Ʈ ó�� ����
            timer.OnTimerEnd += OnTimerEnd;
        }

        if (bossHealthText == null)
        {
            Debug.LogError("bossHealthText�� �Ҵ���� �ʾҽ��ϴ�.");
        }
        if (bossHealthSlider == null)
        {
            Debug.LogError("bossHealthSlider�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        // BossEasy ������Ʈ�� ���� ���� ������Ʈ�� ã�� �����մϴ�.
        boss = FindObjectOfType<BossEasy>();
        if (boss == null)
        {
            Debug.LogError("BossEasy ��ũ��Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    void Update()
    {
        // ������ ü���� ���������� UI�� �ݿ�
        if (boss != null)
        {
            UpdateBossHealthUI();
        }
    }

    private void UpdateBossHealthUI()
    {
        if (boss != null)
        {
            // ������ ü�� ���� �ؽ�Ʈ�� �����̴��� �ݿ��մϴ�.
            bossHealthText.text = $"{boss.curHealth}/{boss.maxHealth}";
            bossHealthSlider.value = boss.curHealth / boss.maxHealth;
        }
    }

    // Ÿ�̸Ӱ� ����Ǿ��� �� ȣ��Ǵ� �޼���
    private void OnTimerEnd()
    {
        Debug.Log("Ÿ�̸Ӱ� ����Ǿ����ϴ�. ���� ���� �̺�Ʈ�� ó���� �� �ֽ��ϴ�.");
    }

    void OnDestroy()
    {
        if (timer != null)
        {
            timer.OnTimerEnd -= OnTimerEnd;
        }
    }
}
