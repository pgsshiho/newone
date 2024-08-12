using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab; // �� ������
    public Transform leftSpawnPoint; // ���� ���ʿ��� ������ ��ġ
    public Transform rightSpawnPoint; // ���� �����ʿ��� ������ ��ġ
    private Timer timer; // Timer ��ũ��Ʈ ����
    private List<GameObject> enemies = new List<GameObject>(); // ������ ���� �����ϴ� ����Ʈ

    public float spawnInterval = 8.0f; // ���� �����Ǵ� ����
    public TextMeshProUGUI enemyCountText; // ���� �� ���� ǥ���� �ؽ�Ʈ

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
        if (enemyCountText == null)
        {
            Debug.LogError("enemyCountText�� �Ҵ���� �ʾҽ��ϴ�.");
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

        UpdateEnemyCountDisplay(); // ���� �� ���� �� �� ������Ʈ
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
                EnemySlime enemySlime = enemy.GetComponent<EnemySlime>();
                if (enemySlime != null)
                {
                    enemySlime.isLeft = isLeft; // ���ʿ��� ������ ��� true, �����ʿ��� ������ ��� false
                    enemySlime.enemyManager = this; // EnemyManager�� ���� ������ ����
                }

                UpdateEnemyCountDisplay(); // ���� ������ �� ���� �� �� ������Ʈ
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

    // Ư�� ���� �����ϴ� �޼���
    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
        Destroy(enemy);
        UpdateEnemyCountDisplay(); // ���� ���ŵ� �� ���� �� �� ������Ʈ
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
        UpdateEnemyCountDisplay(); // ���� ���ŵ� �� ���� �� �� ������Ʈ
        Debug.Log("��� ���� ���ŵǾ����ϴ�.");
    }

    // ���� �� ���� ������Ʈ�ϰ� ȭ�鿡 ǥ���ϴ� �޼���
    private void UpdateEnemyCountDisplay()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = $"{enemies.Count}";
        }
    }

    // ���� �����ִ��� Ȯ���ϴ� �޼ҵ�
    public bool AreAllEnemiesRemoved()
    {
        return enemies.Count == 0;
    }
}
