using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; } // �̱��� �ν��Ͻ� ������Ƽ
    public GameObject Settingpanel; // �г� ������Ʈ�� �����մϴ�.
    bool Setting = false; // �ʱⰪ�� false�� �����մϴ�.
    public Slider HpBarSlider;
    protected float curHealth = 100; //* ���� ü��
    public float maxHealth = 100; //* �ִ� ü��
    public GameObject RE;
    public GameObject Bm;
    public GameObject QU;
    public int dia; // ���̾Ƹ�� ��
    public TextMeshProUGUI diacount; // ���̾Ƹ�� ���� ǥ���� UI
    private RectTransform diacountParent; // TextMeshProUGUI�� �θ� RectTransform
    private Vector2 initialPosition = new Vector2(510, -170); // �ʱ� ��ġ�� ������ ����

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        dia = 0;
    }

    void Start()
    {
        Cursor.visible = false;
        if (Settingpanel != null)
        {
            Settingpanel.SetActive(false); // �г��� ���� �� ��Ȱ��ȭ�մϴ�.
        }
        if (diacount != null)
        {
            diacountParent = diacount.GetComponentInParent<RectTransform>();
            if (diacountParent != null)
            {
                diacountParent.anchoredPosition = initialPosition; // �ʱ� ��ġ ����
            }
        }
        UpdateDiaCount(); // �ʱ� ���̾Ƹ�� �� ������Ʈ

        // ��ư�� OnClick �̺�Ʈ�� �������� ����
        if (RE != null)
        {
            RE.GetComponent<Button>().onClick.AddListener(Re);
        }
        if (Bm != null)
        {
            Bm.GetComponent<Button>().onClick.AddListener(BM);
        }
        if (QU != null)
        {
            QU.GetComponent<Button>().onClick.AddListener(quit);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Setting = !Setting; // Setting ������ ���� ����մϴ�.
            if (Setting)
            {
                Time.timeScale = 0; // ���� �ð� ����
                if (Settingpanel != null)
                {
                    Settingpanel.SetActive(true); // �г��� Ȱ��ȭ�մϴ�.
                }
            }
            else
            {
                Time.timeScale = 1; // ���� �ð� �簳
                if (Settingpanel != null)
                {
                    Settingpanel.SetActive(false); // �г��� ��Ȱ��ȭ�մϴ�.
                }
                Cursor.visible = false;
            }
        }
    }

    public void CheckHp() //*HP ����
    {
        if (HpBarSlider != null)
            HpBarSlider.value = curHealth / maxHealth;
    }

    public void Damage(float damage)
    {
        if (maxHealth == 0 || curHealth <= 0)
            return;
        curHealth -= damage;
        CheckHp();
        if (curHealth <= 0)
        {
            SceneManager.LoadScene("GAME OVER");
        }
    }

    public void BM()
    {
        SceneManager.LoadScene("Mainmenu");
    }

    public void quit()
    {
        Application.Quit();
    }

    public void Re()
    {
        Settingpanel.gameObject.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false; // ���콺 Ŀ�� ����
    }

    // ���̾Ƹ�� ���� ������Ʈ�ϴ� �޼���
    public void UpdateDiaCount()
    {
        if (diacount != null)
        {
            diacount.text = dia.ToString();

            // ���̾Ƹ�� ���� �ڸ����� ���� �θ� RectTransform�� ��ġ�� ����
            if (diacountParent != null)
            {
                int digitCount = dia.ToString().Length;
                float offset = digitCount * 10f; // �ڸ����� ���� �̵��� �Ÿ� ���� (�ʿ信 ���� ����)
                diacountParent.anchoredPosition = new Vector2(initialPosition.x - offset - 20, initialPosition.y);
            }
        }
    }
}