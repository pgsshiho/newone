using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemtManager : MonoBehaviour
{
    public GameObject enemyPrefab; // �� ������
    public Transform leftSpawnPoint; // ���� ���ʿ��� ������ ��ġ
    public Transform rightSpawnPoint; // ���� �����ʿ��� ������ ��ġ
    private Timer timer; // Timer ��ũ��Ʈ ����
    private List<GameObject> enemies = new List<GameObject>(); // ������ ���� �����ϴ� ����Ʈ

    public float spawnInterval = 8.0f; // ���� �����Ǵ� ����

    void Start()
    {
        // ������ �ùٸ��� �����Ǿ����� Ȯ��
        if (enemyPrefab == null)
        {
            Debug.LogError("enemyPrefab�� �Ҵ���� �ʾҽ��ϴ�.");
        }
        if (leftSpawnPoint == null)
        {
            Debug.LogError("leftSpawnPoint�� �Ҵ���� �ʾҽ��ϴ�.");
        }
        if (rightSpawnPoint == null)
        {
            Debug.LogError("rightSpawnPoint�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        timer = FindObjectOfType<Timer>();
        if (timer == null)
        {
            Debug.LogError("Timer ��ũ��Ʈ�� ã�� �� �����ϴ�. ���� Timer�� �ִ��� Ȯ���ϼ���.");
        }
        else
        {
            // ���� �����ϴ� �ڷ�ƾ ����
            StartCoroutine(SpawnEnemyCoroutine());
        }
    }

    // ���ο� ���� �����ϴ� �޼ҵ�
    public void SpawnEnemy(Transform spawnPoint, bool isLeft)
    {
        if (timer != null && !timer.TimerEnded)
        {
            if (enemyPrefab != null && spawnPoint != null)
            {
                GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                enemies.Add(enemy); // ������ ���� ����Ʈ�� �߰�

                // ���ʿ��� ������ ���� flipX�� true�� ����
                SpriteRenderer spriteRenderer = enemy.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.flipX = isLeft;
                }

                EnemySlime enemySlime = enemy.GetComponent<EnemySlime>();
                if (enemySlime != null)
                {
                    enemySlime.isLeft = isLeft; // ���ʿ��� ������ ��� true, �����ʿ��� ������ ��� false
                }
            }
            else
            {
                Debug.LogError("enemyPrefab �Ǵ� spawnPoint�� null�Դϴ�.");
            }
        }
        else
        {
            Debug.Log("Ÿ�̸Ӱ� ���� �� �̻� ���� �������� �ʽ��ϴ�.");
        }
    }

    private IEnumerator SpawnEnemyCoroutine()
    {
        while (timer != null && !timer.TimerEnded)
        {
            SpawnEnemy(leftSpawnPoint, true); // ���ʿ��� ����
            SpawnEnemy(rightSpawnPoint, false); // �����ʿ��� ����
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // ��� �� ���� �޼ҵ�
    public void RemoveAllEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        enemies.Clear();
        Debug.Log("��� ���� ���ŵǾ����ϴ�.");
    }
}
