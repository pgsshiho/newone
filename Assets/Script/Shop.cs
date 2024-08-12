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

        UpdateCostText(); // 아이템 비용 텍스트를 업데이트
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

            // 포탈을 탈 때 타이머 초기화
            Timer timer = FindObjectOfType<Timer>();
            if (timer != null)
            {
                timer.ResetTimer(10f); // 타이머를 10초로 초기화
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
                stageManager.UpdateDiaCount(); // 다이아몬드 수 업데이트
                meatCount++;
                meatCost += 50;
                player.curHealth += 20;

                if (player.curHealth > player.maxHealth)
                {
                    player.curHealth = player.maxHealth;
                }
                UpdateCostText(); // 비용 텍스트 갱신
            }
        }
        else
        {
            ShowWarning("현재 체력이 이미 최대치입니다.");
        }
    }

    public void PurchaseCloth()
    {
        if (CanPurchase(clothCost))
        {
            stageManager.dia -= clothCost;
            stageManager.UpdateDiaCount(); // 다이아몬드 수 업데이트
            clothCount++;
            clothCost += 50;

            // 플레이어의 최대 체력을 10 증가시킴
            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                player.maxHealth += 10;
                player.curHealth += 10; // 현재 체력도 증가한 최대 체력만큼 증가

                // 현재 체력이 최대 체력을 초과하지 않도록 설정
                if (player.curHealth > player.maxHealth)
                {
                    player.curHealth = player.maxHealth;
                }

                player.CheckHp(); // HP 슬라이더 업데이트
            }

            UpdateCostText(); // 비용 텍스트 갱신
        }
    }

    public void PurchaseSword()
    {
        if (CanPurchase(swordCost))
        {
            stageManager.dia -= swordCost;
            stageManager.UpdateDiaCount(); // 다이아몬드 수 업데이트
            swordCount++;
            swordCost += 50;

            // 플레이어의 공격력을 +5 증가시킴
            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                player.AttackDamage += 5;
            }

            UpdateCostText(); // 비용 텍스트 갱신
        }
    }

    private void ShowWarning(string message)
    {
        if (warning != null)
        {
            warning.text = message;
            warning.gameObject.SetActive(true);
            Invoke("HideWarning", 2f); // 2초 후에 경고 메시지를 숨깁니다.
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
            ShowWarning("돈이 부족합니다!");
            return false;
        }
    }
}
