using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] goodItems;
    public GameObject[] badItems;
    public float interval = 1.2f;
    [Range(0f, 1f)] public float badChance = 0.25f;

    float timer;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Spawn();
            timer = interval;
        }
    }

    void Spawn()
    {
        GameObject[] pool = (Random.value < badChance && badItems.Length > 0) ? badItems : goodItems;
        if (pool.Length == 0) return;

        GameObject prefab = pool[Random.Range(0, pool.Length)];
        Camera cam = Camera.main;
        float halfWidth = cam.orthographicSize * cam.aspect - 0.5f;
        float x = Random.Range(-halfWidth, halfWidth);
        float y = cam.orthographicSize + 1f;
        Instantiate(prefab, new Vector3(x, y, 0f), Quaternion.identity);
    }
}