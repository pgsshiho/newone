using System.Collections;
using UnityEngine;

public class EnemySlimeNomal : MonoBehaviour
{
    [SerializeField]
    public float speed = 2.0f; // ���� �̵� �ӵ�
    public float damage = 5.0f; // �÷��̾�� ���� ������
    public float maxHealth = 10.0f; // ���� �ִ� ü��
    public float curHealth; // ���� ���� ü��
    private Transform player;  // ���ΰ��� Transform
    private SpriteRenderer sr; // ���� SpriteRenderer
    private Rigidbody2D rb; // ���� Rigidbody2D
    private ChooseNanEDo Nan; // ChooseNanEDo �ν��Ͻ�

    private bool isKnockedBack = false; // �˹� ����
    public bool isLeft; // ���� ���ʿ��� ������ ��� true, �����ʿ��� ������ ��� false

    private StageManager stageManager; // StageManager �ν��Ͻ�

    void Start()
    {
        // ChooseNanEDo �ν��Ͻ� ��������

        // ���ΰ� ������Ʈ�� ã���ϴ�
        player = GameObject.FindGameObjectWithTag("Player").transform;
        curHealth = maxHealth; // �ʱ� ü���� �ִ� ü������ ����
        sr = GetComponent<SpriteRenderer>(); // SpriteRenderer ������Ʈ�� �����ɴϴ�
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D ������Ʈ�� �����ɴϴ�

        // StageManager �ν��Ͻ��� �����ɴϴ�
        stageManager = StageManager.Instance;
        if (stageManager == null)
        {
            Debug.LogError("StageManager instance not found!");
        }
    }

    void Update()
    {
        if (isKnockedBack) return; // �˹� �߿��� �̵����� �ʽ��ϴ�

        // ���� ���ΰ��� ���� �̵��ϵ��� ����
        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // �÷��̾�� �������� �����ϴ�
            Player playerScript = collision.gameObject.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.Damage(damage);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        curHealth -= damage;

        if (curHealth <= 0)
        {
            // nan�� stageManager�� null�� �ƴ��� Ȯ��
            if (stageManager != null)
            {
                        stageManager.dia += 30;

                    stageManager.UpdateDiaCount(); // ���̾Ƹ�� �� ������Ʈ
            }
            else
            {
                Debug.LogWarning("StageManager ��ü�� null�Դϴ�.");
            }
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        sr.color = Color.red; // ���������� ����
        isKnockedBack = true;

        // �˹� ó��
        Vector2 knockbackDirection = (transform.position - player.position).normalized;
        rb.AddForce(knockbackDirection * 5f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1.5f);

        sr.color = Color.white; // ���� ������ ����
        isKnockedBack = false;
    }
}
