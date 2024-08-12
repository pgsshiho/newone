using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; } // 싱글톤 인스턴스 프로퍼티
    public GameObject Settingpanel; // 패널 오브젝트를 공개합니다.
    bool Setting = false; // 초기값을 false로 설정합니다.
    public Slider HpBarSlider;
    protected float curHealth = 100; //* 현재 체력
    public float maxHealth = 100; //* 최대 체력
    public GameObject RE;
    public GameObject Bm;
    public GameObject QU;
    public int dia; // 다이아몬드 수
    public TextMeshProUGUI diacount; // 다이아몬드 수를 표시할 UI
    private RectTransform diacountParent; // TextMeshProUGUI의 부모 RectTransform
    private Vector2 initialPosition = new Vector2(510, -170); // 초기 위치를 저장할 변수

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
            Settingpanel.SetActive(false); // 패널을 시작 시 비활성화합니다.
        }
        if (diacount != null)
        {
            diacountParent = diacount.GetComponentInParent<RectTransform>();
            if (diacountParent != null)
            {
                diacountParent.anchoredPosition = initialPosition; // 초기 위치 설정
            }
        }
        UpdateDiaCount(); // 초기 다이아몬드 수 업데이트

        // 버튼의 OnClick 이벤트를 동적으로 설정
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
            Setting = !Setting; // Setting 변수의 값을 토글합니다.
            if (Setting)
            {
                Time.timeScale = 0; // 게임 시간 멈춤
                if (Settingpanel != null)
                {
                    Settingpanel.SetActive(true); // 패널을 활성화합니다.
                }
            }
            else
            {
                Time.timeScale = 1; // 게임 시간 재개
                if (Settingpanel != null)
                {
                    Settingpanel.SetActive(false); // 패널을 비활성화합니다.
                }
                Cursor.visible = false;
            }
        }
    }

    public void CheckHp() //*HP 갱신
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
        Cursor.visible = false; // 마우스 커서 숨김
    }

    // 다이아몬드 수를 업데이트하는 메서드
    public void UpdateDiaCount()
    {
        if (diacount != null)
        {
            diacount.text = dia.ToString();

            // 다이아몬드 수의 자릿수에 따라 부모 RectTransform의 위치를 조정
            if (diacountParent != null)
            {
                int digitCount = dia.ToString().Length;
                float offset = digitCount * 10f; // 자릿수에 따라 이동할 거리 설정 (필요에 따라 조정)
                diacountParent.anchoredPosition = new Vector2(initialPosition.x - offset - 20, initialPosition.y);
            }
        }
    }
}