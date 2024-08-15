using System.Collections;
using UnityEngine;

public class BossEasy : MonoBehaviour
{
    [SerializeField]
    public float speed = 2.0f; // 보스의 이동 속도
    public float damage = 30.0f; // 플레이어에게 입힐 데미지
    public float maxHealth = 300.0f; // 보스의 최대 체력
    public float curHealth; // 보스의 현재 체력
    private Transform player;  // 주인공의 Transform
    private SpriteRenderer sr; // 보스의 SpriteRenderer
    private Rigidbody2D rb; // 보스의 Rigidbody2D
    private bool isPaused = false; // 보스가 멈춘 상태인지 여부
    private StageManager stageManager; // StageManager 인스턴스

    void Start()
    {
        // 주인공 오브젝트를 찾습니다
        player = GameObject.FindGameObjectWithTag("Player").transform;
        curHealth = maxHealth; // 초기 체력을 최대 체력으로 설정
        sr = GetComponent<SpriteRenderer>(); // SpriteRenderer 컴포넌트를 가져옵니다
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트를 가져옵니다

        // 보스가 중력의 영향을 받지 않도록 설정
        rb.gravityScale = 0;

        // StageManager 인스턴스를 가져옵니다
        stageManager = StageManager.Instance;
        if (stageManager == null)
        {
            Debug.LogError("StageManager instance not found!");
        }

        // 3초마다 2초씩 멈추도록 코루틴 실행
        StartCoroutine(MovePauseCycle());
    }

    void Update()
    {
        if (isPaused) return; // 멈춘 상태에서는 이동하지 않음

        // 보스가 주인공을 향해 이동하도록 설정
        Vector3 direction = player.position - transform.position;
        direction.Normalize();

        // 이동 방향에 따라 스프라이트 뒤집기
        if (direction.x > 0) // 오른쪽으로 이동 중
        {
            sr.flipX = true;
        }
        else if (direction.x < 0) // 왼쪽으로 이동 중
        {
            sr.flipX = false;
        }

        // 보스의 이동 처리
        transform.position += direction * speed * Time.deltaTime;

        // Y 값이 0 이하로 내려가지 않도록 제한
        if (transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어에게 데미지를 입힙니다
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
                stageManager.UpdateDiaCount(); // 다이아몬드 수 업데이트
            }
            else
            {
                Debug.LogWarning("StageManager 객체가 null입니다.");
            }
            Destroy(gameObject);
        }
    }

    private IEnumerator MovePauseCycle()
    {
        while (true)
        {
            // 3초간 이동
            isPaused = false;
            yield return new WaitForSeconds(3f);

            // 2초간 멈춤
            isPaused = true;
            yield return new WaitForSeconds(2f);
        }
    }
}
