using System.Collections;
using UnityEngine;

public class BossEasy : MonoBehaviour
{
    [SerializeField]
    public float speed = 2.0f; // ������ �̵� �ӵ�
    public float damage = 30.0f; // �÷��̾�� ���� ������
    public float maxHealth = 300.0f; // ������ �ִ� ü��
    public float curHealth; // ������ ���� ü��
    private Transform player;  // ���ΰ��� Transform
    private SpriteRenderer sr; // ������ SpriteRenderer
    private Rigidbody2D rb; // ������ Rigidbody2D
    private bool isPaused = false; // ������ ���� �������� ����
    private StageManager stageManager; // StageManager �ν��Ͻ�

    void Start()
    {
        // ���ΰ� ������Ʈ�� ã���ϴ�
        player = GameObject.FindGameObjectWithTag("Player").transform;
        curHealth = maxHealth; // �ʱ� ü���� �ִ� ü������ ����
        sr = GetComponent<SpriteRenderer>(); // SpriteRenderer ������Ʈ�� �����ɴϴ�
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D ������Ʈ�� �����ɴϴ�

        // ������ �߷��� ������ ���� �ʵ��� ����
        rb.gravityScale = 0;

        // StageManager �ν��Ͻ��� �����ɴϴ�
        stageManager = StageManager.Instance;
        if (stageManager == null)
        {
            Debug.LogError("StageManager instance not found!");
        }

        // 3�ʸ��� 2�ʾ� ���ߵ��� �ڷ�ƾ ����
        StartCoroutine(MovePauseCycle());
    }

    void Update()
    {
        if (isPaused) return; // ���� ���¿����� �̵����� ����

        // ������ ���ΰ��� ���� �̵��ϵ��� ����
        Vector3 direction = player.position - transform.position;
        direction.Normalize();

        // �̵� ���⿡ ���� ��������Ʈ ������
        if (direction.x > 0) // ���������� �̵� ��
        {
            sr.flipX = true;
        }
        else if (direction.x < 0) // �������� �̵� ��
        {
            sr.flipX = false;
        }

        // ������ �̵� ó��
        transform.position += direction * speed * Time.deltaTime;

        // Y ���� 0 ���Ϸ� �������� �ʵ��� ����
        if (transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
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
            if (stageManager != null)
            {
                stageManager.dia += 70;
                stageManager.UpdateDiaCount(); // ���̾Ƹ�� �� ������Ʈ
            }
            else
            {
                Debug.LogWarning("StageManager ��ü�� null�Դϴ�.");
            }
            Destroy(gameObject);
        }
    }

    private IEnumerator MovePauseCycle()
    {
        while (true)
        {
            // 3�ʰ� �̵�
            isPaused = false;
            yield return new WaitForSeconds(3f);

            // 2�ʰ� ����
            isPaused = true;
            yield return new WaitForSeconds(2f);
        }
    }
}
