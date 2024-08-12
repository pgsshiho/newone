using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed = 10.0f;
    public float jumpPower = 5.0f;
    public Vector2 inputVec;
    private Rigidbody2D rigid;
    private SpriteRenderer sr;
    private Animator animator;
    public bool isJump = false;
    public Slider HpBarSlider;
    [SerializeField]
    public float curHealth = 100; // ���� ü��
    public float maxHealth = 100; // �ִ� ü��
    private bool isInvincible = false; // ���� ����
    public float invincibilityDuration = 1.0f; // ���� ���� �ð�
    public float flashDuration = 0.1f; // ������ ����
    [SerializeField]
    public float AttackDamage = 5;
    public float attackRange = 3.0f; // ���� ����
    [SerializeField]
    private int test = 0;
    StageManager stageManager; // StageManager �ν��Ͻ�
    Shop shop;

    public float sliderLerpSpeed = 2.0f; // �����̴� ��ȭ �ӵ�

    private bool canAttack = true; // ���� ���� ����
    public float attackCooldown = 1.0f; // ���� ��Ÿ�� �ð�
    public bool canMove = true; // ������ ���� ����

    private void Awake()
    {
        Time.timeScale = 1;
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigid.freezeRotation = true;

        // StageManager �ν��Ͻ��� ������
        stageManager = FindObjectOfType<StageManager>();

        // Shop �ν��Ͻ��� ������
        shop = FindObjectOfType<Shop>();
    }

    public void SetHp(float amount) // Hp ����
    {
        maxHealth = amount;
        curHealth = maxHealth;
        CheckHp();
    }

    public void Update()
    {
        if (shop != null)
        {
            AttackDamage = shop.swordCount * 5;
        }
        if (shop != null)
        {
            maxHealth = shop.clothCount * 10;
        }

        if (!canMove) return; // �������� �Ұ����ϸ� �Է� ����

        inputVec.x = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            Jump();
        }
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            animator.SetTrigger("Attack");

            DetectAndAttackEnemy();
            StartCoroutine(AttackCooldownCoroutine());
        }
        if (inputVec.x != 0)
        {
            animator.SetBool("Right_Move", true);
            animator.SetBool("Idle", false);
            sr.flipX = inputVec.x < 0;
        }
        else
        {
            animator.SetBool("Right_Move", false);
            animator.SetBool("Idle", true);
        }
        if (SceneManager.GetActiveScene().name == "Easy")
        {
            test = 0;
        }
        if (SceneManager.GetActiveScene().name == "Nomal")
        {
            test = 1;
        }
        if (SceneManager.GetActiveScene().name == "Hard")
        {
            test = 2;
        }
    }

    public void FixedUpdate()
    {
        if (!canMove) return; // �������� �Ұ����ϸ� ���� ������Ʈ ����

        CheckGround();
        Vector2 nextVec = inputVec.normalized * maxSpeed;
        rigid.velocity = new Vector2(nextVec.x, rigid.velocity.y);
    }

    private void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));

        if (hit.collider != null)
        {
            isJump = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isJump = false;
            animator.SetBool("jumping", false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, Vector3.down);
    }

    public void CheckHp() // HP ����
    {
        if (HpBarSlider != null)
        {
            StartCoroutine(LerpHpBar(curHealth)); // �ڷ�ƾ�� ����Ͽ� �����̴� ���� �ε巴�� ����
        }
    }

    private IEnumerator LerpHpBar(float targetHealth)
    {
        float startValue = HpBarSlider.value;
        float endValue = targetHealth;
        float elapsedTime = 0f;

        while (elapsedTime < 1f / sliderLerpSpeed)
        {
            elapsedTime += Time.deltaTime * sliderLerpSpeed;
            HpBarSlider.value = Mathf.Lerp(startValue, endValue, elapsedTime);
            yield return null;
        }

        HpBarSlider.value = endValue; // ������ ���� ��Ȯ�ϰ� ����
    }

    public void Damage(float damage)
    {
        if (isInvincible || curHealth <= 0)
            return;

        curHealth -= damage; // ���� ü�� ����
        CheckHp(); // �������� ���� �� ü�¹� ����

        if (curHealth <= 0)
        {
            SceneManager.LoadScene("GAME OVER");
            // death ���� �߰�
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        for (float i = 0; i < invincibilityDuration; i += flashDuration)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(flashDuration);
        }
        sr.enabled = true;
        isInvincible = false;
    }

    public void Attack(Transform target)
    {
        if (target == null)
        {
            Debug.LogError("Attack target is null.");
            return;
        }

        // ���� ���� �߰� (Ÿ���� ü�� ���� ��)
        var enemySlime = target.GetComponent<EnemySlime>();
        var enemySlimeHard = target.GetComponent<EnemySlimeHard>();
        var enemySlimeNomal = target.GetComponent<EnemySlimeNomal>();
        var wormEasy = target.GetComponent<WormEasy>();

        if (enemySlime != null)
        {
            enemySlime.TakeDamage(AttackDamage);
        }
        else if (enemySlimeHard != null)
        {
            enemySlimeHard.TakeDamage(AttackDamage);
        }
        else if (enemySlimeNomal != null)
        {
            enemySlimeNomal.TakeDamage(AttackDamage);
        }
        else if (wormEasy != null)
        {
            wormEasy.TakeDamage(AttackDamage);
        }
        else
        {
            Debug.LogError("EnemySlime, EnemySlimeHard, EnemySlimeNomal, or WormEasy component not found on target.");
        }
    }

    public void DetectAndAttackEnemy()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Enemy"));

        if (hitEnemies != null)
        {
            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy != null && enemy.transform != null)
                {
                    if (Mathf.Abs(enemy.transform.position.x - transform.position.x) <= 3.0f)
                    {
                        Attack(enemy.transform);
                        break;
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("No enemies found within the attack range.");
        }
    }

    private IEnumerator AttackCooldownCoroutine()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void Jump()
    {
        animator.SetBool("jumping", true);
        isJump = true;
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    // �ִϸ��̼� �̺�Ʈ�� ���� ȣ��Ǵ� �޼���
    public void EndAttack()
    {
        animator.ResetTrigger("Attack");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
