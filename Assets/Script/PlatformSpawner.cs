using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab; // Prefab for the platform to spawn
    public int count = 3;

    public float timeBetSpawnMin = 1.25f;
    public float timeBetSpawnMax = 2.25f;
    private float timeBetSpawn;

    public float yMin = -3.5f;
    public float yMax = 1.5f;
    private float xPos = 20f;

    private GameObject[] platforms;
    private int currentIndex = 0;

    private Vector3 poolPosition = new Vector3(0, -25, 0);
    private float lastSpawnTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (platformPrefab == null)
        {
            Debug.LogError("PlatformSpawner: platformPrefab이 할당되지 않았습니다! PlatformSpawner 오브젝트를 선택하고 Inspector에서 platformPrefab을 지정하세요.");
            return;
        }

        platforms = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            platforms[i] = Instantiate(platformPrefab, poolPosition, Quaternion.identity);
        }

        lastSpawnTime = 0f;
        timeBetSpawn = 0f;
    }



    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isGameOver)
        {
            return; // 게임이 끝나면 플랫폼 스폰 중지
        }

        if (Time.time >= lastSpawnTime + timeBetSpawn)
        {
            lastSpawnTime = Time.time;
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);

            // 플랫폼의 위치를 설정
            float yPos = Random.Range(yMin, yMax);
            if (platforms[currentIndex] == null)
            {
                Debug.LogError($"PlatformSpawner: platforms[{currentIndex}]가 null입니다. 프리팹이 파괴되었거나 생성에 실패했을 수 있습니다.");
            }
            else
            {
                platforms[currentIndex].SetActive(false);
                platforms[currentIndex].SetActive(true);
                platforms[currentIndex].transform.position = new UnityEngine.Vector3(xPos, yPos, 0f);
            }
            currentIndex++;
            if (currentIndex >= count)
            {
                currentIndex = 0;
            }
        }
    }
}
