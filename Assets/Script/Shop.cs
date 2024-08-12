using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    public static Shop instance;

    public GameObject dialoguePanel;
    private bool isPlayerInRange = false;
    public GameObject shop;

    public int meatCost = 50;
    public int meatCount = 1;
    public int clothCost = 50;
    public int clothCount = 1;
    public int swordCost = 50;
    public int swordCount = 1;

    public TextMeshProUGUI meatCostText;
    public TextMeshProUGUI clothCostText;
    public TextMeshProUGUI swordCostText;

    private StageManager stageManager;
    public GameObject purchasemeat;
    public GameObject purchasecloth;
    public GameObject purchasesword;
    public TextMeshProUGUI warning;
    Player player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        stageManager = FindObjectOfType<StageManager>();
    }

    void Start()
    {
        shop.SetActive(true);

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        UpdateCostText(); // ������ ��� �ؽ�Ʈ�� ������Ʈ
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (dialoguePanel != null)
            {
                bool isActive = !dialoguePanel.activeSelf;
                dialoguePanel.SetActive(isActive);

                Player player = FindObjectOfType<Player>();
                if (player != null)
                {
                    player.canMove = !isActive;
                }

                Cursor.visible = isActive;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;

            // ��Ż�� Ż �� Ÿ�̸� �ʱ�ȭ
            Timer timer = FindObjectOfType<Timer>();
            if (timer != null)
            {
                timer.ResetTimer(10f); // Ÿ�̸Ӹ� 10�ʷ� �ʱ�ȭ
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;

            if (dialoguePanel != null && dialoguePanel.activeSelf)
            {
                dialoguePanel.SetActive(false);

                Player player = FindObjectOfType<Player>();
                if (player != null)
                {
                    player.canMove = true;
                }

                Cursor.visible = false;
            }
        }
    }

    public void OpenShop()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
            Cursor.visible = true;
        }
    }

    public void PurchaseMeat()
    {
        Player player = FindObjectOfType<Player>();

        if (player != null && player.curHealth < player.maxHealth)
        {
            if (CanPurchase(meatCost))
            {
                stageManager.dia -= meatCost;
                stageManager.UpdateDiaCount(); // ���̾Ƹ�� �� ������Ʈ
                meatCount++;
                meatCost += 50;
                player.curHealth += 20;

                if (player.curHealth > player.maxHealth)
                {
                    player.curHealth = player.maxHealth;
                }
                UpdateCostText(); // ��� �ؽ�Ʈ ����
            }
        }
        else
        {
            ShowWarning("���� ü���� �̹� �ִ�ġ�Դϴ�.");
        }
    }

    public void PurchaseCloth()
    {
        if (CanPurchase(clothCost))
        {
            stageManager.dia -= clothCost;
            stageManager.UpdateDiaCount(); // ���̾Ƹ�� �� ������Ʈ
            clothCount++;
            clothCost += 50;

            // �÷��̾��� �ִ� ü���� 10 ������Ŵ
            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                player.maxHealth += 10;
                player.curHealth += 10; // ���� ü�µ� ������ �ִ� ü�¸�ŭ ����

                // ���� ü���� �ִ� ü���� �ʰ����� �ʵ��� ����
                if (player.curHealth > player.maxHealth)
                {
                    player.curHealth = player.maxHealth;
                }

                player.CheckHp(); // HP �����̴� ������Ʈ
            }

            UpdateCostText(); // ��� �ؽ�Ʈ ����
        }
    }

    public void PurchaseSword()
    {
        if (CanPurchase(swordCost))
        {
            stageManager.dia -= swordCost;
            stageManager.UpdateDiaCount(); // ���̾Ƹ�� �� ������Ʈ
            swordCount++;
            swordCost += 50;

            // �÷��̾��� ���ݷ��� +5 ������Ŵ
            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                player.AttackDamage += 5;
            }

            UpdateCostText(); // ��� �ؽ�Ʈ ����
        }
    }

    private void ShowWarning(string message)
    {
        if (warning != null)
        {
            warning.text = message;
            warning.gameObject.SetActive(true);
            Invoke("HideWarning", 2f); // 2�� �Ŀ� ��� �޽����� ����ϴ�.
        }
    }

    private void HideWarning()
    {
        if (warning != null)
        {
            warning.gameObject.SetActive(false);
        }
    }

    private void UpdateCostText()
    {
        if (meatCostText != null)
        {
            meatCostText.text = meatCost.ToString();
        }
        if (clothCostText != null)
        {
            clothCostText.text = clothCost.ToString();
        }
        if (swordCostText != null)
        {
            swordCostText.text = swordCost.ToString();
        }
    }

    private bool CanPurchase(int cost)
    {
        if (stageManager != null && stageManager.dia >= cost)
        {
            return true;
        }
        else
        {
            ShowWarning("���� �����մϴ�!");
            return false;
        }
    }
}
