using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab; // 적 프리팹
    public Transform leftSpawnPoint; // 적이 왼쪽에서 생성될 위치
    public Transform rightSpawnPoint; // 적이 오른쪽에서 생성될 위치
    private Timer timer; // Timer 스크립트 참조
    private List<GameObject> enemies = new List<GameObject>(); // 생성된 적을 추적하는 리스트

    public float spawnInterval = 8.0f; // 적이 생성되는 간격
    public TextMeshProUGUI enemyCountText; // 남은 적 수를 표시할 텍스트

    void Start()
    {
        // 참조가 올바르게 설정되었는지 확인
        if (enemyPrefab == null)
        {
            Debug.LogError("enemyPrefab이 할당되지 않았습니다.");
        }
        if (leftSpawnPoint == null)
        {
            Debug.LogError("leftSpawnPoint가 할당되지 않았습니다.");
        }
        if (rightSpawnPoint == null)
        {
            Debug.LogError("rightSpawnPoint가 할당되지 않았습니다.");
        }
        if (enemyCountText == null)
        {
            Debug.LogError("enemyCountText가 할당되지 않았습니다.");
        }

        timer = FindObjectOfType<Timer>();
        if (timer == null)
        {
            Debug.LogError("Timer 스크립트를 찾을 수 없습니다. 씬에 Timer가 있는지 확인하세요.");
        }
        else
        {
            // 적을 생성하는 코루틴 시작
            StartCoroutine(SpawnEnemyCoroutine());
        }

        UpdateEnemyCountDisplay(); // 시작 시 남은 적 수 업데이트
    }

    // 새로운 적을 생성하는 메소드
    public void SpawnEnemy(Transform spawnPoint, bool isLeft)
    {
        if (timer != null && !timer.TimerEnded)
        {
            if (enemyPrefab != null && spawnPoint != null)
            {
                GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                enemies.Add(enemy); // 생성된 적을 리스트에 추가
                EnemySlime enemySlime = enemy.GetComponent<EnemySlime>();
                if (enemySlime != null)
                {
                    enemySlime.isLeft = isLeft; // 왼쪽에서 생성된 경우 true, 오른쪽에서 생성된 경우 false
                    enemySlime.enemyManager = this; // EnemyManager에 대한 참조를 설정
                }

                UpdateEnemyCountDisplay(); // 적이 생성될 때 남은 적 수 업데이트
            }
            else
            {
                Debug.LogError("enemyPrefab 또는 spawnPoint가 null입니다.");
            }
        }
        else
        {
            Debug.Log("타이머가 끝나 더 이상 적을 생성하지 않습니다.");
        }
    }

    private IEnumerator SpawnEnemyCoroutine()
    {
        while (timer != null && !timer.TimerEnded)
        {
            SpawnEnemy(leftSpawnPoint, true); // 왼쪽에서 생성
            SpawnEnemy(rightSpawnPoint, false); // 오른쪽에서 생성
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // 특정 적을 제거하는 메서드
    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
        Destroy(enemy);
        UpdateEnemyCountDisplay(); // 적이 제거될 때 남은 적 수 업데이트
    }

    // 모든 적 제거 메소드
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
        UpdateEnemyCountDisplay(); // 적이 제거된 후 남은 적 수 업데이트
        Debug.Log("모든 적이 제거되었습니다.");
    }

    // 남은 적 수를 업데이트하고 화면에 표시하는 메서드
    private void UpdateEnemyCountDisplay()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = $"{enemies.Count}";
        }
    }

    // 적이 남아있는지 확인하는 메소드
    public bool AreAllEnemiesRemoved()
    {
        return enemies.Count == 0;
    }
}
